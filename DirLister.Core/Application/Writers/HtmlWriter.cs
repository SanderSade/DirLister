using System.Collections.Generic;

namespace Sander.DirLister.Core.Application.Writers
{
	internal sealed class HtmlWriter: BaseWriter
	{
		public HtmlWriter(Configuration configuration) : base(configuration)
		{
		}

		protected internal override string Write(List<FileEntry> entries)
		{
			throw new System.NotImplementedException();
		}
	}
}
