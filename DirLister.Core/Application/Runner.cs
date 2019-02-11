using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

			var entries = new FileReader(_configuration).GetEntries();

			if (entries == null || entries.Count == 0)
			{
				_configuration.LoggingAction.Invoke(TraceLevel.Error, "No files found or error gathering data!");
				return null;
			}

			if (_configuration.IncludeMediaInfo)
			{
				var mediaReader = new MediaInfoReader(_configuration);
				mediaReader.AddMediaInfo(entries);
			}

			if (_skipListMaking)
				return entries;

			var writer = new OutputFileWriter(_configuration);
			writer.Write(entries);

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

				if (!_skipListMaking)
				{
					if (_configuration.OutputFormats == null || _configuration.OutputFormats.Count == 0)
					{
						isValid = false;
						_configuration.LoggingAction.Invoke(TraceLevel.Error,
							"At least one output format needs to be set!");
					}

					Directory.CreateDirectory(_configuration.OutputFolder);
				}

				ValidateInputFolders(ref isValid);

				return isValid;
			}
			catch (Exception e)
			{
				_configuration.LoggingAction.Invoke(TraceLevel.Error, e.ToString());
				return false;
			}
		}

		private void ValidateInputFolders(ref bool isValid)
		{

			if (_configuration.InputFolders == null || _configuration.InputFolders.Count == 0)
			{
				isValid = false;
				_configuration.LoggingAction.Invoke(TraceLevel.Error,
					"At least one input folder needs to be set!");
			}
			else
			{
				foreach (var inputFolder in _configuration.InputFolders)
				{
					if (!Directory.Exists(inputFolder))
					{
						isValid = false;
						_configuration.LoggingAction.Invoke(TraceLevel.Error,
							$"Folder \"{inputFolder}\" does not exist.");
					}
				}
			}
		}
	}
}
