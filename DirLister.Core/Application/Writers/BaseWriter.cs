using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sander.DirLister.Core.Application.Writers
{
	internal abstract class BaseWriter
	{
		protected readonly Configuration Configuration;

		protected BaseWriter(Configuration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// Write output file, returning filename
		/// </summary>
		/// <param name="entries"></param>
		/// <returns></returns>
		protected internal abstract string Write(List<FileEntry> entries);

		/// <summary>
		/// Return filename, fullpath.
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		protected internal string GetFilename(OutputFormat format)
		{

			if (Configuration.InputFolders.Count == 1)
				return Path.Combine(Configuration.OutputFolder, $"DirLister.{DateTimeOffset.Now:yyyy-MM-dd.HHmmss}.",
					ReplacePathCharacters(format));

			return
				$"{Path.Combine(Configuration.OutputFolder,$"DirLister.{DateTimeOffset.Now:yyyy-MM-dd.HHmmss}.{Configuration.InputFolders.Count}-folders")}{format.ToString().ToLowerInvariant()}";


		}


		private string ReplacePathCharacters(OutputFormat format)
		{
			var chars = Path.GetInvalidFileNameChars();
			var filename = chars.Aggregate(Configuration.InputFolders[0], (current, c) => current.Replace(c, '_'));

			return $"{filename}.{format.ToString().ToLowerInvariant()}";
		}
	}
}
