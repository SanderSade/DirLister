using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sander.DirLister.Core.Application.Writers
{
	internal abstract class BaseWriter
	{
		protected List<FileEntry> Entries { get; }
		protected readonly Configuration Configuration;
		private readonly DateTimeOffset _endDate;
		protected string FileDateFormat;
		private readonly Lazy<List<IGrouping<string, FileEntry>>> _lazyGrouped;
		protected List<IGrouping<string, FileEntry>> GroupedEntries => _lazyGrouped.Value;


		protected BaseWriter(Configuration configuration, DateTimeOffset endDate, List<FileEntry> entries)
		{
			Entries = entries;
			Configuration = configuration;
			_endDate = endDate;
			FileDateFormat = configuration.FileDateFormat;
			_lazyGrouped = new Lazy<List<IGrouping<string, FileEntry>>>(() => Entries.GroupBy(x => x.Folder).ToList());
		}

		/// <summary>
		///     Write output file, returning filename
		/// </summary>
		/// <returns>Filename</returns>
		protected internal abstract string Write();

		/// <summary>
		///     Return filename, fullpath.
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		protected internal string GetFilename(OutputFormat format)
		{
			if (Configuration.InputFolders.Count == 1)
				return Path.Combine(Configuration.OutputFolder, $"DirLister.{_endDate.ToLocalTime():yyyy-MM-dd.HHmmss}.{ReplacePathCharacters(format)}");

			return
				$"{Path.Combine(Configuration.OutputFolder, $"DirLister.{Configuration.InputFolders.Count} folders")}.{_endDate.ToLocalTime():yyyy-MM-dd.HHmmss}.{format.ToString().ToLowerInvariant()}";
		}

		/// <summary>
		/// Replace path characters in filename
		/// </summary>
		private string ReplacePathCharacters(OutputFormat format)
		{
			var chars = Path.GetInvalidFileNameChars();
			var filename = chars.Aggregate(Configuration.InputFolders[0], (current, c) => current.Replace(c, '_')).Replace("__", "_").Trim('_', ' ');

			return $"{filename}.{format.ToString().ToLowerInvariant()}";
		}



		/// <summary>
		/// Format duration. Handles time better than inbuilt, but isn't culture-specific
		/// </summary>
		protected string FormatDuration(TimeSpan time)
		{
			if (time == TimeSpan.Zero)
				return string.Empty;

			var sb = new StringBuilder();
			if (time.Hours > 0) sb.Append($"{time.Hours}h");

			sb.Append($"{time.Minutes}m");
			sb.Append($"{time.Seconds}s");

			return sb.ToString();
		}

		/// <summary>
		/// Write the output file. Returns the filename
		/// </summary>
		protected string WriteFile(StringBuilder sb, OutputFormat format)
		{
			var fileName = GetFilename(format);

			using (var sw = new StreamWriter(fileName, false, Encoding.UTF8, 2 << 16 /* 128KB*/))
			{
				sw.Write(sb.ToString());
			}

			return fileName;
		}
	}
}
