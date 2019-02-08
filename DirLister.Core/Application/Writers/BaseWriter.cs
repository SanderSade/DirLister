using System;
using System.Collections.Generic;

namespace Sander.DirLister.Core.Application.Writers
{
	internal abstract class BaseWriter
	{
		protected readonly Configuration _configuration;

		protected BaseWriter(Configuration configuration)
		{
			_configuration = configuration;
		}

		/// <summary>
		/// Write output file, returning filename
		/// </summary>
		/// <param name="entries"></param>
		/// <returns></returns>
		protected internal abstract string Write(List<FileEntry> entries);

		protected string GetFilename(OutputFormat format)
		{
			throw new NotImplementedException();
		}

	}
}
