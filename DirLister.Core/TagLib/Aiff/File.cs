using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Sander.DirLister.Core.TagLib.Aiff
{
	/// <summary>
	///    This class extends <see cref="TagLib.File" /> to provide
	///    support for reading and writing tags and properties for files
	///    using the AIFF file format.
	/// </summary>
	[SupportedMimeType("taglib/aif", "aif")]
	[SupportedMimeType("taglib/aiff", "aiff")]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public sealed class File : TagLib.File
	{
		/// <summary>
		///    The identifier used to recognize a AIFF files.
		/// </summary>
		/// <value>
		///    "FORM"
		/// </value>
		public static readonly ReadOnlyByteVector FileIdentifier = "FORM";

		/// <summary>
		///    The identifier used to recognize a AIFF Common chunk.
		/// </summary>
		/// <value>
		///    "COMM"
		/// </value>
		public static readonly ReadOnlyByteVector CommIdentifier = "COMM";

		/// <summary>
		///    The identifier used to recognize a AIFF Sound Data Chunk.
		/// </summary>
		/// <value>
		///    "SSND"
		/// </value>
		public static readonly ReadOnlyByteVector SoundIdentifier = "SSND";

		/// <summary>
		///    The identifier used to recognize a AIFF ID3 chunk.
		/// </summary>
		/// <value>
		///    "ID3 "
		/// </value>
		public static readonly ReadOnlyByteVector ID3Identifier = "ID3 ";

		/// <summary>
		///    The identifier used to recognize a AIFF Form type.
		/// </summary>
		/// <value>
		///    "AIFF"
		/// </value>
		public static readonly ReadOnlyByteVector AIFFFormType = "AIFF";
		/// <summary>
		///    Contains the address of the AIFF header block.
		/// </summary>
		private ByteVector header_block;

		/// <summary>
		///  Contains the media properties.
		/// </summary>
		private Properties properties;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified path in the local file
		///    system and specified read style.
		/// </summary>
		/// <param name="path">
		///    A <see cref="string" /> object containing the path of the
		///    file to use in the new instance.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path, ReadStyle propertiesStyle)
			: this(new LocalFileAbstraction(path),
				propertiesStyle)
		{
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified path in the local file
		///    system with an average read style.
		/// </summary>
		/// <param name="path">
		///    A <see cref="string" /> object containing the path of the
		///    file to use in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path)
			: this(path, ReadStyle.Average)
		{
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified file abstraction and
		///    specified read style.
		/// </summary>
		/// <param name="abstraction">
		///    A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///    reading from and writing to the file.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="abstraction" /> is <see langword="null"
		///    />.
		/// </exception>
		public File(IFileAbstraction abstraction,
			ReadStyle propertiesStyle)
			: base(abstraction)
		{
			Mode = AccessMode.Read;
			try
			{
				Read(true, propertiesStyle, out var _,
					out var _, out var _);
			}
			finally
			{
				Mode = AccessMode.Closed;
			}
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified file abstraction with an
		///    average read style.
		/// </summary>
		/// <param name="abstraction">
		///    A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///    reading from and writing to the file.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="abstraction" /> is <see langword="null"
		///    />.
		/// </exception>
		public File(IFileAbstraction abstraction)
			: this(abstraction, ReadStyle.Average)
		{
		}


		/// <summary>
		///    Gets the media properties of the file represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="TagLib.Properties" /> object containing the
		///    media properties of the file represented by the current
		///    instance.
		/// </value>
		public override Properties Properties => properties;


		/// <summary>
		///    Search the file for a chunk whose name is given by
		///    the chunkName parameter, starting from startPos.
		///    Note that startPos must be a valid position for a
		///    chunk, or else finding will fail.
		/// </summary>
		/// <param name="chunkName">Name of the chunk to search for</param>
		/// <param name="startPos">Position for starting the search</param>
		/// <returns>
		///    Position of the chunk in the stream, or -1
		///    if no chunk was found.
		/// </returns>
		private long FindChunk(ByteVector chunkName, long startPos)
		{
			var initialPos = Tell;

			try
			{
				// Start at the given position
				Seek(startPos);

				// While not eof
				while (Tell < Length)
				{
					// Read 4-byte chunk name
					var chunkHeader = ReadBlock(4);

					if (chunkHeader == chunkName)
					{
						// We found a matching chunk, return the position
						// of the header start
						return Tell - 4;
					}
					else
					{
						// This chunk is not the one we are looking for
						// Continue the search, seeking over the chunk
						var chunkSize = ReadBlock(4)
							.ToUInt();
						// Seek forward "chunkSize" bytes
						Seek(chunkSize, SeekOrigin.Current);
					}
				}

				// We did not find the chunk
				return -1;
			}
			finally
			{
				Seek(initialPos);
			}
		}


		/// <summary>
		///    Reads the contents of the current instance determining
		///    the size of the riff data, the area the tagging is in,
		///    and optionally reading in the tags and media properties.
		/// </summary>
		/// <param name="read_tags">
		///    If <see langword="true" />, any tags found will be read
		///    into the current instance.
		/// </param>
		/// <param name="style">
		///    A <see cref="ReadStyle"/> value specifying how the media
		///    data is to be read into the current instance.
		/// </param>
		/// <param name="aiff_size">
		///    A <see cref="uint"/> value reference to be filled with
		///    the size of the RIFF data as read from the file.
		/// </param>
		/// <param name="tag_start">
		///    A <see cref="long" /> value reference to be filled with
		///    the absolute seek position at which the tagging data
		///    starts.
		/// </param>
		/// <param name="tag_end">
		///    A <see cref="long" /> value reference to be filled with
		///    the absolute seek position at which the tagging data
		///    ends.
		/// </param>
		/// <exception cref="CorruptFileException">
		///    The file does not begin with <see cref="FileIdentifier"
		///    />.
		/// </exception>
		private void Read(bool read_tags, ReadStyle style,
			out uint aiff_size, out long tag_start,
			out long tag_end)
		{
			Seek(0);
			if (ReadBlock(4) != FileIdentifier)
				throw new CorruptFileException(
					"File does not begin with AIFF identifier");

			aiff_size = ReadBlock(4)
				.ToUInt(true);
			tag_start = -1;
			tag_end = -1;

			// Check formType
			if (ReadBlock(4) != AIFFFormType)
				throw new CorruptFileException(
					"File form type is not AIFF");

			var formBlockChunksPosition = Tell;

			// Get the properties of the file
			if (header_block == null &&
			    style != ReadStyle.None)
			{
				var common_chunk_pos = FindChunk(CommIdentifier, formBlockChunksPosition);

				if (common_chunk_pos == -1)
				{
					throw new CorruptFileException(
						"No Common chunk available in AIFF file.");
				}

				Seek(common_chunk_pos);
				header_block = ReadBlock((int)StreamHeader.Size);

				var header = new StreamHeader(header_block, aiff_size);
				properties = new Properties(TimeSpan.Zero, header);
			}

			// Search for the sound chunk
			var sound_chunk_pos = FindChunk(SoundIdentifier, formBlockChunksPosition);

			// Ensure there is a sound chunk for the file to be valid
			if (sound_chunk_pos == -1)
			{
				throw new CorruptFileException(
					"No Sound chunk available in AIFF file.");
			}
		}
	}
}
