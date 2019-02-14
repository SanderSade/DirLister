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
				return null;

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
				_configuration.SendProgress(100, "All done");
				_configuration.Log(TraceLevel.Info,
					$"All done. Total time: {sw.Elapsed}, total files: {entries.Count}, total size: {Utils.ReadableSize(entries.Sum(x => x.Size))}");

				return entries;
			}
			_configuration.SendProgress(95, "Creating output file(s)");

			var writer = new OutputFileWriter(_configuration);
			writer.Write(entries);

			_configuration.Log(TraceLevel.Info,
				$"All done. Total time: {sw.Elapsed}, total files: {entries.Count}, total size: {Utils.ReadableSize(entries.Sum(x => x.Size))}");
			_configuration.SendProgress(100, "All done");

			if (_configuration.OpenAfter)
				writer.OpenFileOrFolder();

			return entries;
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
					if (!Directory.Exists(inputFolder))
					{
						isValid = false;
						_configuration.Log(TraceLevel.Warning,
							$"Folder \"{inputFolder}\" does not exist.");
					}
					else
					{
						if (inputFolder[inputFolder.Length - 1] != Path.DirectorySeparatorChar)
							folders.Add(inputFolder + Path.DirectorySeparatorChar);
						else
							folders.Add(inputFolder);
					}
				}

				_configuration.InputFolders = folders;
			}
		}

	}
}
