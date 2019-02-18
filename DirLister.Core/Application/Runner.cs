using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Sander.DirLister.Core.Application.Media;

namespace Sander.DirLister.Core.Application
{
	/// <summary>
	///
	/// </summary>
	internal sealed class Runner
	{
		private readonly Configuration _configuration;
		private readonly bool _skipListMaking;

		internal Runner(Configuration configuration, bool skipListMaking)
		{
			_configuration = configuration;
			_skipListMaking = skipListMaking;
		}

		internal List<FileEntry> Run()
		{
			if (!ValidateConfiguration())
			{
				Trace.Flush();
				return null;
			}

			var sw = Stopwatch.StartNew();

			_configuration.SendProgress(10, "Fetching files");
			var entries = new FileReader(_configuration).GetEntries();

			if (entries == null || entries.Count == 0)
			{
				_configuration.Log(TraceLevel.Error, "No files found or error gathering data!");
				return null;
			}

			if (_configuration.IncludeMediaInfo)
			{
				var mediaReader = new MediaInfoReader(_configuration);
				mediaReader.AddMediaInfo(entries);
			}

			if (_skipListMaking)
			{
				RunComplete(sw, entries);
				return entries;
			}

			_configuration.SendProgress(95, "Creating output file(s)");

			var writer = new OutputFileWriter(_configuration);
			writer.Write(entries);

			RunComplete(sw, entries);

			if (_configuration.OpenAfter)
				writer.OpenFileOrFolder();

			return entries;
		}

		private void RunComplete(Stopwatch sw, List<FileEntry> entries)
		{
			_configuration.SendProgress(100, "All done");
			_configuration.Log(TraceLevel.Info,
				$"All done. Total time: {sw.Elapsed}, total files: {entries.Count}, total size: {Utils.ReadableSize(entries.Sum(x => x.Size))}");
			//this has no useful effect if our LoggingAction is not Trace-based
			Trace.Flush();
		}

		internal bool ValidateConfiguration()
		{
			if (_configuration.LoggingAction == null)
				throw new MissingMethodException("Logging action is null in configuration. Cannot continue.");

			var isValid = true;
			try
			{
				if (_configuration.Filter == null)
					_configuration.Filter = new Filter();

				if (string.IsNullOrWhiteSpace(_configuration.FileDateFormat))
					_configuration.FileDateFormat = "yyyy-MM-dd HH:mm:ss";

				if (!_skipListMaking)
				{
					if (_configuration.OutputFormats == null || _configuration.OutputFormats.Count == 0)
					{
						isValid = false;
						_configuration.Log(TraceLevel.Error,
							"At least one output format needs to be set!");
					}
					else
						_configuration.OutputFormats = _configuration.OutputFormats.Distinct().ToList();

					Directory.CreateDirectory(_configuration.OutputFolder);
				}

				ValidateInputFolders(ref isValid);

				return isValid;
			}
			catch (Exception e)
			{
				_configuration.Log(TraceLevel.Error, e.ToString());
				return false;
			}
		}

		private void ValidateInputFolders(ref bool isValid)
		{
			if (_configuration.InputFolders == null || _configuration.InputFolders.Count == 0)
			{
				isValid = false;
				_configuration.Log(TraceLevel.Error,
					"At least one input folder needs to be set!");
			}
			else
			{
				var folders = new List<string>(_configuration.InputFolders.Count);
				foreach (var inputFolder in _configuration.InputFolders)
				{
					var mappedFolder = Utils.GetUncPath(inputFolder);

					if (string.Compare(mappedFolder, inputFolder, StringComparison.Ordinal) != 0)
						_configuration.Log(TraceLevel.Warning, $"Using \"{inputFolder}\" as UNC path \"{mappedFolder}\"");

					if (!Directory.Exists(mappedFolder))
					{
						isValid = false;
						_configuration.Log(TraceLevel.Warning,
							$"Folder \"{mappedFolder}\" does not exist.");
					}
					else
					{
						if (mappedFolder[mappedFolder.Length - 1] != Path.DirectorySeparatorChar)
							folders.Add(mappedFolder + Path.DirectorySeparatorChar);
						else
							folders.Add(mappedFolder);
					}
				}

				_configuration.InputFolders = folders;
			}
		}

	}
}
