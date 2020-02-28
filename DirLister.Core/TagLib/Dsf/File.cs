using System;

namespace Sander.DirLister.Core.TagLib.Dsf
{
	/// <summary>
	///     This class extends <see cref="TagLib.File" /> to provide
	///     support for reading and writing tags and properties for files
	///     using the AIFF file format.
	/// </summary>
	[SupportedMimeType("taglib/dsf", "dsf")]
	public sealed class File : TagLib.File
	{
		/// <summary>
		///     The identifier used to recognize a DSF file.
		/// </summary>
		/// <value>
		///     "DSD "
		/// </value>
		public static readonly ReadOnlyByteVector FileIdentifier = "DSD ";

		/// <summary>
		///     The identifier used to recognize a Format chunk.
		/// </summary>
		/// <value>
		///     "fmt "
		/// </value>
		public static readonly ReadOnlyByteVector FormatIdentifier = "fmt ";

		/// <summary>
		///     The identifier used to recognize a DSF ID3 chunk.
		/// </summary>
		/// <value>
		///     "ID3 "
		/// </value>
		public static readonly ReadOnlyByteVector ID3Identifier = "ID3";

		/// <summary>
		///     Contains the size of the DSF File
		/// </summary>
		private readonly uint dsf_size;

		/// <summary>
		///     Contains the end position of the Tag
		/// </summary>
		private readonly long tag_end;

		/// <summary>
		///     Contains the start position of the Tag
		/// </summary>
		private readonly long tag_start;

		/// <summary>
		///     Contains the address of the DSF header block.
		/// </summary>
		private ByteVector header_block;

		/// <summary>
		///     Contains the media properties.
		/// </summary>
		private Properties properties;


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
		public File(string path)
			: this(path, ReadStyle.Average)
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
		public File(IFileAbstraction abstraction,
			ReadStyle propertiesStyle)
			: base(abstraction)
		{
			Mode = AccessMode.Read;
			try
			{
				Read(true, propertiesStyle, out dsf_size,
					out tag_start, out tag_end);
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
		public override Properties Properties => properties;


		/// <summary>
		///     Reads the contents of the current instance determining
		///     the size of the dsf data, the area the tagging is in,
		///     and optionally reading in the tags and media properties.
		/// </summary>
		/// <param name="read_tags">
		///     If <see langword="true" />, any tags found will be read
		///     into the current instance.
		/// </param>
		/// <param name="style">
		///     A <see cref="ReadStyle" /> value specifying how the media
		///     data is to be read into the current instance.
		/// </param>
		/// <param name="dsf_size">
		///     A <see cref="uint" /> value reference to be filled with
		///     the size of the RIFF data as read from the file.
		/// </param>
		/// <param name="tag_start">
		///     A <see cref="long" /> value reference to be filled with
		///     the absolute seek position at which the tagging data
		///     starts.
		/// </param>
		/// <param name="tag_end">
		///     A <see cref="long" /> value reference to be filled with
		///     the absolute seek position at which the tagging data
		///     ends.
		/// </param>
		/// <exception cref="CorruptFileException">
		///     The file does not begin with <see cref="FileIdentifier" />.
		/// </exception>
		private void Read(bool read_tags, ReadStyle style,
			out uint dsf_size, out long tag_start,
			out long tag_end)
		{
			Seek(0);
			if (ReadBlock(4) != FileIdentifier)
			{
				throw new CorruptFileException(
					"File does not begin with DSF identifier");
			}

			Seek(12);
			dsf_size = ReadBlock(8)
				.ToUInt(false);

			tag_start = (long)ReadBlock(8)
				.ToULong(false);

			tag_end = -1;

			// Get the properties of the file
			if (header_block == null &&
			    style != ReadStyle.None)
			{
				var fmt_chunk_pos = Find(FormatIdentifier, 0);

				if (fmt_chunk_pos == -1)
				{
					throw new CorruptFileException(
						"No Format chunk available in DSF file.");
				}

				Seek(fmt_chunk_pos);
				header_block = ReadBlock((int)StreamHeader.Size);

				var header = new StreamHeader(header_block, dsf_size);
				properties = new Properties(TimeSpan.Zero, header);
			}
		}
	}
}
