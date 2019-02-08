using System.Collections.Generic;

namespace Sander.DirLister.Core
{
	/// <summary>
	/// Configuration for directory listing, containing all relevant information
	/// </summary>
	public sealed class Configuration
	{
		/// <summary>
		/// Input folders
		/// </summary>
		public List<string> InputFolders { get; set; }

		/// <summary>
		/// Exclude hidden and system files. Defaults to true
		/// </summary>
		public bool ExcludeHidden { get; set; } = true;

	}
}
