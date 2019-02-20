using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Sander.DirLister.Core
{
	/// <summary>
	///     Configuration for directory listing, containing all relevant information
	/// </summary>
	public sealed class Configuration
	{
		/// <summary>
		///     Input folders
		/// </summary>
		public List<string> InputFolders { get; set; }

		/// <summary>
		///     Include hidden and system files. Defaults to false (not included)
		/// </summary>
		public bool IncludeHidden { get; set; } = false;

		/// <summary>
		///     Filter to apply for files.
		///     You can apply custom logic via IFilter interface
		/// </summary>
		public IFilter Filter { get; set; } = new Filter();

		/// <summary>
		///     Output formats. Defaults to HTML if omitted
		/// </summary>
		public List<OutputFormat> OutputFormats { get; set; } = new List<OutputFormat> {OutputFormat.Html};

		/// <summary>
		///     Output folder. Will be created if doesn't exist.
		///     Defaults to My Documents/DirLister.
		/// </summary>
		public string OutputFolder { get; set; } =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DirLister");

		/// <summary>
		///     Open the output file with the default viewer after list generation.
		///     If multiple output files are defined, folder is opened instead
		/// </summary>
		public bool OpenAfter { get; set; } = true;

		/// <summary>
		///     Whether or not directory listing is recursive (includes subfolders). Defaults to true.
		/// </summary>
		public bool IncludeSubfolders { get; set; } = true;

		/// <summary>
		///     Include size. Human-readable for txt and HTML, exact byte count for others
		/// </summary>
		public bool IncludeSize { get; set; } = true;

		/// <summary>
		///     Include media info. Defaults to false.
		/// </summary>
		public bool IncludeMediaInfo { get; set; } = false;

		/// <summary>
		///     Logging action. Defaults to simple Trace.WriteLine().
		///     * TraceLevel.Error means fatal exception was encountered.
		///     * TraceLevel.Warning is nonfatal.
		///     * TraceLevel.Info is informational message.
		///     NOT thread-safe (multiple threads may write at the same time)
		/// </summary>
		public Action<TraceLevel, string> LoggingAction { get; set; } =
			(traceLevel, message) => Trace.WriteLine($"[{traceLevel}] {message}");

		/// <summary>
		///     Enable multi-threaded file and media info gathering.
		///     Enable for SSDs *only*, as this is a bad performance hit for regular hard drives.
		///     Defaults to false.
		/// </summary>
		public bool EnableMultithreading { get; set; } = false;

		/// <summary>
		///     Include file creation and last modified date.
		///     Defaults to false.
		/// </summary>
		public bool IncludeFileDates { get; set; } = false;

		/// <summary>
		///     CSS to use for HTML output. If omitted, defaults to internal CSS.
		///     This the actual content of the CSS, not path to the file.
		///     See the documentation for details.
		/// </summary>
		public string CssContent { get; set; }

		/// <summary>
		///     Report operation progress (0..100).
		///     Not fully accurate, but good indicator.
		///     This is NOT thread-safe
		/// </summary>
		public Action<int, string> ProgressAction { get; set; }

		/// <summary>
		/// Date format used in .txt and .html output when IncludeFileDates is set.
		/// See https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings for details.
		/// Defaults to yyyy-MM-dd HH:mm:ss (ISO 8601)
		/// </summary>
		public string FileDateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";


		/// <summary>
		///     Send out progress indicator
		/// </summary>
		internal void SendProgress(int percentage, string description)
		{
			ProgressAction?.BeginInvoke(percentage, description, null, null);
		}


		/// <summary>
		///     Log w/o waiting the invoke to finish.
		/// </summary>
		internal void Log(TraceLevel level, string message)
		{
			LoggingAction.BeginInvoke(level, message, null, null);
		}
	}
}
