using System;
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
			_configuration.Log(TraceLevel.Info, "Creating output files...");
			var endDate = DateTimeOffset.Now;

			var tasks = new List<Task<string>>();
			//todo: more elegant than repeated if

			if (_configuration.OutputFormats.Contains(OutputFormat.Csv))
			{
				tasks.Add(Task.Run(() => new CsvWriter(_configuration, endDate, entries).Write()));
			}

			if (_configuration.OutputFormats.Contains(OutputFormat.Html))
			{
				tasks.Add(Task.Run(() => new HtmlWriter(_configuration, endDate, entries).Write()));
			}

			if (_configuration.OutputFormats.Contains(OutputFormat.Json))
			{
				tasks.Add(Task.Run(() => new JsonWriter(_configuration, endDate, entries).Write()));
			}

			if (_configuration.OutputFormats.Contains(OutputFormat.Txt))
			{
				tasks.Add(Task.Run(() => new TxtWriter(_configuration, endDate, entries).Write()));
			}

			if (_configuration.OutputFormats.Contains(OutputFormat.Xml))
			{
				tasks.Add(Task.Run(() => new XmlWriter(_configuration, endDate, entries).Write()));
			}

			if (_configuration.OutputFormats.Contains(OutputFormat.Md))
			{
				tasks.Add(Task.Run(() => new MarkdownWriter(_configuration, endDate, entries).Write()));
			}

			// ReSharper disable once CoVariantArrayConversion
			Task.WaitAll(tasks.ToArray());

			_outputFiles = tasks.Select(x => x.Result)
				.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			_configuration.Log(TraceLevel.Info, $"{_outputFiles.Count} output file(s) created");
		}


		internal void OpenFileOrFolder()
		{
			if (_outputFiles.Count == 1)
			{
				_configuration.Log(TraceLevel.Info, "Opening the output file with the default viewer");
				Process.Start(_outputFiles[0]);
				return;
			}

			_configuration.Log(TraceLevel.Info, "Opening the output folder");
			ShowSelectedInExplorer.FilesOrFolders(_configuration.OutputFolder, _outputFiles.Select(Path.GetFileName).ToArray());
		}
	}
}
