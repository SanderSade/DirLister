using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sander.DirLister.Core.Application.Writers;

namespace Sander.DirLister.Core.Application
{
	internal sealed class OutputFileWriter
	{
		private readonly Configuration _configuration;
		private List<string> _outputFiles;

		internal OutputFileWriter(Configuration configuration)
		{
			_configuration = configuration;
			_outputFiles = new List<string>(_configuration.OutputFormats.Count);
		}

		internal void Write(List<FileEntry> entries)
		{
			_configuration.LoggingAction.Invoke(TraceLevel.Info, "Creating output files...");

			var tasks = new List<Task<string>>();
			//todo: more elegant than repeated if

			if (_configuration.OutputFormats.Contains(OutputFormat.Csv))
				tasks.Add(Task.Run(() => new CsvWriter(_configuration).Write(entries)));

			if (_configuration.OutputFormats.Contains(OutputFormat.Html))
				tasks.Add(Task.Run(() => new HtmlWriter(_configuration).Write(entries)));

			if (_configuration.OutputFormats.Contains(OutputFormat.Json))
				tasks.Add(Task.Run(() => new JsonWriter(_configuration).Write(entries)));

			if (_configuration.OutputFormats.Contains(OutputFormat.Txt))
				tasks.Add(Task.Run(() => new TxtWriter(_configuration).Write(entries)));

			if (_configuration.OutputFormats.Contains(OutputFormat.Xml))
				tasks.Add(Task.Run(() => new XmlWriter(_configuration).Write(entries)));

			// ReSharper disable once CoVariantArrayConversion
			Task.WaitAll(tasks.ToArray());

			_outputFiles = tasks.Select(x =>  x.Result)
				.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			_configuration.LoggingAction.Invoke(TraceLevel.Info, $"{_outputFiles.Count} output file(s) created");
		}

		internal void OpenFileOrFolder()
		{
			if (_outputFiles.Count == 1)
			{
				_configuration.LoggingAction.Invoke(TraceLevel.Info, "Opening the output file with the default viewer");
				Process.Start(_outputFiles.First());
				return;
			}

			_configuration.LoggingAction.Invoke(TraceLevel.Info, "Opening the output folder");
			ShowSelectedInExplorer.FilesOrFolders(_configuration.OutputFolder, _outputFiles.Select(Path.GetFileName).ToArray());
		}
	}
}
