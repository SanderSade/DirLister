﻿using System.Collections.Generic;

namespace Sander.DirLister.Core.Application.Writers
{
	internal sealed class CsvWriter : BaseWriter
	{
		internal CsvWriter(Configuration configuration) : base(configuration)
		{
		}

		protected internal override string Write(List<FileEntry> entries)
		{
			throw new System.NotImplementedException();
		}
	}
}
