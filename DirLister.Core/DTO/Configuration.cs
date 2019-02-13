using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
		/// Include hidden and system files. Defaults to false (not included)
		/// </summary>
		public bool IncludeHidden { get; set; } = false;

		/// <summary>
		/// Filter to apply for files
		/// </summary>
		public Filter Filter { get; set; } = new Filter();

		/// <summary>
		/// Output formats. Defaults to HTML if omitted
		/// </summary>
		public List<OutputFormat> OutputFormats { get; set; } = new List<OutputFormat> { OutputFormat.Html };

		/// <summary>
		/// Output folder. Will be created if doesn't exist.
		/// Defaults to My Documents/DirLister.
		/// </summary>
		public string OutputFolder { get; set; } =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DirLister");

		/// <summary>
		/// Open the output file with the defaul viewer after list generation.
		/// If multiple output files are defined, folder is opened instead
		/// </summary>
		public bool OpenAfter { get; set; }

		/// <summary>
		/// Whether or not directory listing is recursive (includes subfolders). Defaults to true.
		/// </summary>
		public bool Recursive { get; set; } = true;

		/// <summary>
		/// Include size. Human-readable for txt and HTML, exact byte count for others
		/// </summary>
		public bool IncludeSize { get; set; } = true;

		/// <summary>
		/// Include media info. Defaults to false.
		/// </summary>
		public bool IncludeMediaInfo { get; set; } = false;

		/// <summary>
		/// Logging action. Defaults to simple Trace.WriteLine().
		/// * TraceLevel.Error means fatal exception was encountered.
		/// * TraceLevel.Warning is nonfatal.
		/// * TraceLevel.Info is informational messages.
		/// </summary>
		public Action<TraceLevel, string> LoggingAction { get; set; } = (traceLevel, message) => Trace.WriteLine($"[{traceLevel}] {message}");

		/// <summary>
		/// Enable multi-threaded file and media info gathering.
		/// Enable for SSDs *only*, bad performance hit for regular hard drives.
		/// Defaults to false.
		/// </summary>
		public bool EnableMultithreading { get; set; } = false;

		/// <summary>
		/// Include file creation and last modified date.
		/// Defaults to false.
		/// </summary>
		public bool IncludeFileDates { get; set; } = false;

		/// <summary>
		/// CSS to use for HTML output. If omitted, defaults to indernal CSS.
		/// This the actual content of the CSS, not path to a file
		/// See the documentation for details.
		/// </summary>
		public string CssContent { get; set; }
	}
}
