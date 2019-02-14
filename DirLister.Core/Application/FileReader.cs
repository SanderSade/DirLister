using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sander.DirLister.Core.Application
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal sealed class FileReader
	{
		private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
		private readonly Configuration _configuration;


		/// <inheritdoc />
		internal FileReader(Configuration configuration)
		{
			_configuration = configuration;
		}


		internal List<FileEntry> GetEntries()
		{
			_configuration.LoggingAction.Invoke(TraceLevel.Info, "Gathering files...");

			var fileList = ListFiles();

			if (fileList.Count == 0)
			{
				_configuration.LoggingAction.Invoke(TraceLevel.Error, "Input folders did not contain any files!");
				return null;
			}

			_configuration.LoggingAction.Invoke(TraceLevel.Info, $"Found {fileList.Count} matching files");
			return fileList;
		}


		private List<FileEntry> ListFiles()
		{
			var fileList = new List<FileEntry>(100000);

			float maxProgress = _configuration.IncludeMediaInfo ? 30 : 80;
			var stepSize = (int)Math.Round(maxProgress / _configuration.InputFolders.Count, 0);
			var position = 10;
			foreach (var inputFolder in _configuration.InputFolders)
			{
				if (!Directory.Exists(inputFolder))
				{
					_configuration.LoggingAction.Invoke(TraceLevel.Warning,
						$"Input folder \"{inputFolder}\" does not exist!");
					continue;
				}

				var files = ParallelFindNextFile(inputFolder);
				position += stepSize;
				_configuration.SendProgress(position, "Gathering files");

				if (files.Count == 0)
				{
					_configuration.LoggingAction.Invoke(TraceLevel.Warning,
						$"Input folder \"{inputFolder}\" does not contain any files!");
					continue;
				}

				//sort here, so we still have same input folder order
				//NTFS should give us already-sorted as it is
				fileList.AddRange(files.Where(x => _configuration.Filter.IsMatch(x.Filename)).OrderBy(x => x.Fullname));
			}

			return fileList;
		}


		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATAW lpFindFileData);


		[DllImport("kernel32.dll")]
		private static extern bool FindClose(IntPtr hFindFile);



		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern IntPtr FindFirstFileEx(
			string lpFileName,
			FINDEX_INFO_LEVELS fInfoLevelId,
			out WIN32_FIND_DATAW lpFindFileData,
			FINDEX_SEARCH_OPS fSearchOp,
			IntPtr lpSearchFilter,
			int dwAdditionalFlags);

		private enum FINDEX_INFO_LEVELS
		{
			FindExInfoBasic = 1
		}
		private enum FINDEX_SEARCH_OPS
		{
			FindExSearchNameMatch = 0
		}


		private bool FindNextFile(string path, out List<FileEntry> files)
		{
			var fileList = new List<FileEntry>(32);
			var findHandle = INVALID_HANDLE_VALUE;

			try
			{
				findHandle = FindFirstFileEx(string.Concat(path, "\\*"), FINDEX_INFO_LEVELS.FindExInfoBasic, out var findData, FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, 2);
				if (findHandle != INVALID_HANDLE_VALUE)
				{
					do
					{
						// Skip current directory and parent directory symbols that are returned.
						if (!findData.cFileName.Equals(".", StringComparison.OrdinalIgnoreCase) && !findData.cFileName.Equals("..",
								StringComparison.OrdinalIgnoreCase))
						{
							var fullPath = string.Concat(path, "\\", findData.cFileName);
							// Check if this is a directory and not a symbolic link since symbolic links could lead to repeated files and folders as well as infinite loops.
							if (findData.dwFileAttributes.HasFlag(FileAttributes.Directory) && !findData.dwFileAttributes.HasFlag(FileAttributes.ReparsePoint))
							{
								if (FindNextFile(fullPath, out var subDirectoryFileList))
								{
									fileList.AddRange(subDirectoryFileList);
								}
							}
							else if (!findData.dwFileAttributes.HasFlag(FileAttributes.Directory))
							{
								var fileEntry = GetFileEntry(fullPath, findData);
								if (fileEntry != null)
									fileList.Add(fileEntry);
							}
						}
					} while (FindNextFile(findHandle, out findData));
				}
			}
			catch (Exception exception)
			{
				_configuration.LoggingAction.Invoke(TraceLevel.Warning, $"Caught exception while trying to enumerate a directory. {exception}");

				if (findHandle != INVALID_HANDLE_VALUE) FindClose(findHandle);
				files = null;
				return false;
			}

			if (findHandle != INVALID_HANDLE_VALUE) FindClose(findHandle);
			files = fileList;
			return true;
		}


		private List<FileEntry> ParallelFindNextFile(string path)
		{
			var fileList = new ConcurrentBag<FileEntry>();
			var directoryList = new List<string>(32);

			var findHandle = INVALID_HANDLE_VALUE;
			try
			{
				path = path[path.Length - 1] == '\\' ? path : FormattableString.Invariant($@"{path}\");
				findHandle = FindFirstFileEx(string.Concat(path, "*"), FINDEX_INFO_LEVELS.FindExInfoBasic, out var findData, FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, 2);
				if (findHandle != INVALID_HANDLE_VALUE)
				{
					do
					{
						// Skip current directory and parent directory symbols that are returned.
						if (!findData.cFileName.Equals(".", StringComparison.OrdinalIgnoreCase) && !findData.cFileName.Equals("..",
								StringComparison.OrdinalIgnoreCase))
						{
							var fullPath = path + findData.cFileName;
							// Check if this is a directory and not a symbolic link since symbolic links could lead to repeated files and folders as well as infinite loops.
							if (findData.dwFileAttributes.HasFlag(FileAttributes.Directory) && !findData.dwFileAttributes.HasFlag(FileAttributes.ReparsePoint))
							{
								if (_configuration.Recursive)
									directoryList.Add(fullPath);
							}
							else if (!findData.dwFileAttributes.HasFlag(FileAttributes.Directory))
							{
								var fileEntry = GetFileEntry(fullPath, findData);
								if (fileEntry != null)
									fileList.Add(fileEntry);

							}
						}
					} while (FindNextFile(findHandle, out findData));

					directoryList
						.AsParallel()
						.WithDegreeOfParallelism(_configuration.EnableMultithreading ? Environment.ProcessorCount : 1)
						.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
						.ForAll(x =>
						{
							if (FindNextFile(x, out var subDirectoryFileList))
							{
								foreach (var entry in subDirectoryFileList)
								{
									fileList.Add(entry);
								}
							}
						});
				}
			}
			catch (Exception exception)
			{
				_configuration.LoggingAction.Invoke(TraceLevel.Warning, $"Caught exception while trying to enumerate a directory. {exception}");
				if (findHandle != INVALID_HANDLE_VALUE) FindClose(findHandle);
				return fileList.ToList();
			}

			if (findHandle != INVALID_HANDLE_VALUE) FindClose(findHandle);

			return fileList.ToList();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private FileEntry GetFileEntry(string fullPath, WIN32_FIND_DATAW findData)
		{
			if (!_configuration.IncludeHidden && findData.dwFileAttributes.HasFlag(FileAttributes.System | FileAttributes.Hidden))
				return null;

			var fileEntry = new FileEntry
			{
				Fullname = fullPath,
				Size = findData.nFileSizeLow,
			};

			if (_configuration.IncludeFileDates)
			{
				fileEntry.Created = Utils.FileTimeToDateTimeOffset(findData.ftCreationTime);
				fileEntry.Modified = Utils.FileTimeToDateTimeOffset(findData.ftLastWriteTime);
			}

			return fileEntry;
		}


		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WIN32_FIND_DATAW
		{
			internal readonly FileAttributes dwFileAttributes;
#pragma warning disable 618
			internal readonly FILETIME ftCreationTime;
			internal readonly FILETIME ftLastAccessTime;
			internal readonly FILETIME ftLastWriteTime;
#pragma warning restore 618
			internal readonly int nFileSizeHigh;
			internal readonly int nFileSizeLow;
			internal readonly int dwReserved0;
			internal readonly int dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			internal readonly string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			internal readonly string cAlternateFileName;
		}
	}
}
