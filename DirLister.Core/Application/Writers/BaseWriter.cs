using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sander.DirLister.Core.Application.Writers
{
	internal abstract class BaseWriter
	{
		protected readonly Configuration Configuration;
		protected static readonly string FileDateFormat = "yyyy-MM-dd HH:mm:ss";


		protected BaseWriter(Configuration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		///     Write output file, returning filename
		/// </summary>
		/// <param name="entries"></param>
		/// <returns></returns>
		protected internal abstract string Write(List<FileEntry> entries);

		/// <summary>
		///     Return filename, fullpath.
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		protected internal string GetFilename(OutputFormat format)
		{
			if (Configuration.InputFolders.Count == 1)
				return Path.Combine(Configuration.OutputFolder, $"DirLister.{DateTimeOffset.Now:yyyy-MM-dd.HHmmss}.{ReplacePathCharacters(format)}");

			return
				$"{Path.Combine(Configuration.OutputFolder, $"DirLister.{DateTimeOffset.Now:yyyy-MM-dd.HHmmss}.{Configuration.InputFolders.Count}-folders")}.{format.ToString().ToLowerInvariant()}";
		}

		/// <summary>
		/// Replace path characters in filename
		/// </summary>
		private string ReplacePathCharacters(OutputFormat format)
		{
			var chars = Path.GetInvalidFileNameChars();
			var filename = chars.Aggregate(Configuration.InputFolders[0], (current, c) => current.Replace(c, '_'));

			return $"{filename}.{format.ToString().ToLowerInvariant()}";
		}

		/// <summary>
		/// Group entries by folder
		/// </summary>
		protected IEnumerable<IGrouping<string, FileEntry>> GroupByFolder(List<FileEntry> entries)
		{
			return entries.GroupBy(x => x.Folder);
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
