using System;

namespace Sander.DirLister.Core.TagLib.Audible
{
	/// <summary>
	///     This class extends <see cref="TagLib.File" /> to provide tagging
	///     and properties support for Audible inc's aa file format.
	/// </summary>
	[SupportedMimeType("taglib/aa", "aa")]
	[SupportedMimeType("taglib/aax", "aax")]
	public sealed class File : TagLib.File
	{
		/// <summary>
		///     The offset to the tag block.
		/// </summary>
		public const short TagBlockOffset = 0xBD;

		/// <summary>
		///     The offset to the end of tag pointer.
		/// </summary>
		public const short OffsetToEndTagPointer = 0x38;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified path in the local file
		///     system and specified read style.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object containing the path of the
		///     file to use in the new instance.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path, ReadStyle propertiesStyle)
			: this(new LocalFileAbstraction(path),
				propertiesStyle)
		{
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified path in the local file
		///     system with an average read style.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object containing the path of the
		///     file to use in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path) : this(path, ReadStyle.Average)
		{
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified file abstraction and
		///     specified read style.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading from and writing to the file.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="abstraction" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///     The file is not the write length.
		/// </exception>
		public File(IFileAbstraction abstraction,
			ReadStyle propertiesStyle) : base(abstraction)
		{
			Mode = AccessMode.Read;

			try
			{
				// get the pointer to the end of the tag block
				// and calculate the tag block length
				Seek(OffsetToEndTagPointer);
				var tagLen = (int)ReadBlock(4)
					.ToUInt(true) - TagBlockOffset;

				// read the whole tag and send to Tag class
				Seek(TagBlockOffset);
				var bv = ReadBlock(tagLen);
			}
			finally
			{
				Mode = AccessMode.Closed;
			}
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified file abstraction with an
		///     average read style.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading from and writing to the file.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="abstraction" /> is <see langword="null" />.
		/// </exception>
		public File(IFileAbstraction abstraction)
			: this(abstraction, ReadStyle.Average)
		{
		}


		/// <summary>
		///     Gets the media properties of the file represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="TagLib.Properties" /> object containing the
		///     media properties of the file represented by the current
		///     instance.
		/// </value>
		public override Properties Properties { get; } = new Properties();
	}
}
