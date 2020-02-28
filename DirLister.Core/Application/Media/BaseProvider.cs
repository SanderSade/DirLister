using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sander.DirLister.Core.Application.Media
{
	internal abstract class BaseProvider
	{
		protected readonly Configuration Configuration;

		/// <summary>
		///     Extensions supported by the current media provider
		/// </summary>
		protected HashSet<string> SupportedExtensions;


		protected BaseProvider(Configuration configuration)
		{
			Configuration = configuration;
		}


		/// <summary>
		///     Can be handled by current provider
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool IsMatch(FileEntry entry)
		{
			return SupportedExtensions.Contains(entry.Extension);
		}
	}
}
