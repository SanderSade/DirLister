

using System;
using Sander.DirLister.Core.Application;

namespace Sander.DirLister.Core
{
	/// <summary>
	/// File information
	/// </summary>
	public sealed class FileEntry
	{
		private string _extension;
		private string _filename;
		private string _folder;

		internal string Fullname { get; set; }

		/// <summary>
		/// File folder
		/// </summary>
		public string Folder => _folder ?? (_folder = Utils.GetPath(Fullname));

		/// <summary>
		/// File name including extension
		/// </summary>
		public string Filename => _filename ?? (_filename = Utils.GetFileName(Fullname));

		/// <summary>
		/// File extension without the period (.). Does not handle multi-dot extensions.
		/// </summary>
		public string Extension => _extension ?? (_extension = Utils.GetExtension(Fullname));

		/// <summary>
		/// File size in bytes
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		/// Get human-readable file size (1.24GB
		/// </summary>
		public string ReadableSize => Utils.ReadableSize(Size);

		/// <summary>
		/// File creation date
		/// </summary>
		public DateTimeOffset Created { get; set; }

		/// <summary>
		/// Last-modified date
		/// </summary>
		public DateTimeOffset Modified { get; set; }

		/// <summary>
		/// Media information, encapsulating image/audio/video data
		/// </summary>
		public MediaInfo MediaInfo { get; set; }

		/// <summary>
		/// Returns full file name - drive:\path\name.ext
		/// </summary>
		/// <returns></returns>
		public override string ToString() => Fullname;
	}
}
