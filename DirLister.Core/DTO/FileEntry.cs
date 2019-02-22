using System;
using System.Globalization;
using System.Runtime.Serialization;
using Sander.DirLister.Core.Application;

namespace Sander.DirLister.Core
{
	/// <summary>
	///     File information.
	///     Serializable with DataContractSerializer
	/// </summary>
	[DataContract]
	public sealed class FileEntry
	{
		private string _extension;
		private string _filename;
		private string _folder;

		/// <summary>
		///     Full name of the file, including path and extension.
		/// </summary>
		[DataMember(Name = "name", Order = 1)]
		public string Fullname { get; set; }

		/// <summary>
		///     File folder
		/// </summary>
		public string Folder => _folder ?? (_folder = Utils.GetPath(Fullname));

		/// <summary>
		///     File name including extension
		/// </summary>
		public string Filename => _filename ?? (_filename = Utils.GetFileName(Fullname));

		/// <summary>
		///     File extension without the period (.). Does not handle multi-dot extensions.
		/// </summary>
		public string Extension => _extension ?? (_extension = Utils.GetExtension(Fullname));

		/// <summary>
		///     File size in bytes
		/// </summary>
		[DataMember(Name = "size", EmitDefaultValue = false, IsRequired = false, Order = 2)]
		public long Size { get; set; }

		/// <summary>
		///     Get human-readable file size (1.24GB
		/// </summary>
		public string ReadableSize => Utils.ReadableSize(Size);

		/// <summary>
		///     File creation date
		/// </summary>
		public DateTimeOffset Created { get; set; }

		[DataMember(Name = "created", EmitDefaultValue = false, IsRequired = false, Order = 3)]
		private string CreatedString => Created == DateTimeOffset.MinValue ? null : Created.ToLocalTime().ToString("O", CultureInfo.InvariantCulture);

		/// <summary>
		///     Last-modified date
		/// </summary>
		public DateTimeOffset Modified { get; set; }

		[DataMember(Name = "modified", EmitDefaultValue = false, IsRequired = false, Order = 4)]
		private string ModifiedString => Modified == DateTimeOffset.MinValue ? null : Modified.ToLocalTime().ToString("O", CultureInfo.InvariantCulture);

		/// <summary>
		///     Media information, encapsulating image/audio/video data
		/// </summary>
		[DataMember(Name = "mediaInfo", EmitDefaultValue = false, IsRequired = false, Order = 5)]
		public MediaInfo MediaInfo { get; set; }

		/// <summary>
		///     Returns full file name - drive:\path\name.ext
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Fullname;
		}
	}
}
