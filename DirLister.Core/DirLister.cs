using System.Collections.Generic;
using System.Threading.Tasks;
using Sander.DirLister.Core.Application;

namespace Sander.DirLister.Core
{
	/// <summary>
	///     Create directory listings
	/// </summary>
	public static class DirLister
	{
		/// <summary>
		///     Run directory listing with specified configuration, creating output files as needed
		/// </summary>
		/// <param name="configuration"></param>
		public static bool List(Configuration configuration)
		{
			var runner = new Runner(configuration, false);
			runner.Run(out var noErrors);
			return noErrors;
		}


		/// <summary>
		///     Run directory listing with specified configuration, creating output files as needed
		/// </summary>
		/// <param name="configuration"></param>
		public static async Task<bool> ListAsync(Configuration configuration)
		{
			return await Task.Run(() => List(configuration));
		}


		/// <summary>
		///     Run directory listing with specified configuration, returning file info.
		///     Output files are not created.
		///     This is intended to be used by other apps.
		///     Can return null in case of issues.
		/// </summary>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static List<FileEntry> Run(Configuration configuration)
		{
			var runner = new Runner(configuration, true);
			return runner.Run(out var noErrors);
		}


		/// <summary>
		///     Run directory listing with specified configuration, returning file info.
		///     Output files are not created.
		///     This is intended to be used by other apps.
		///     Can return null as task result in case of issues.
		/// </summary>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static async Task<List<FileEntry>> RunAsync(Configuration configuration)
		{
			return await Task.Run(() => Run(configuration));
		}
	}
}
