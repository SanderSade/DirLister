using System;

namespace Sander.DirLister.Core.TagLib.Riff
{
	/// <summary>
	///     This class extends <see cref="TagLib.File" /> to provide
	///     support for reading and writing tags and properties for files
	///     using the RIFF file format such as AVI and Wave files.
	/// </summary>
	[SupportedMimeType("taglib/avi", "avi")]
	[SupportedMimeType("taglib/wav", "wav")]
	[SupportedMimeType("taglib/divx", "divx")]
	public sealed class File : TagLib.File
	{
		/// <summary>
		///     The identifier used to recognize a RIFF files.
		/// </summary>
		/// <value>
		///     "RIFF"
		/// </value>
		public static readonly ReadOnlyByteVector FileIdentifier = "RIFF";

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
		public File(IFileAbstraction abstraction,
			ReadStyle propertiesStyle) : base(abstraction)
		{
			Mode = AccessMode.Read;
			try
			{
				Read(true, propertiesStyle, out var riff_size,
					out var tag_start, out var tag_end);
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
		///     the size of the riff data, the area the tagging is in,
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
		/// <param name="riff_size">
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
			out uint riff_size, out long tag_start,
			out long tag_end)
		{
			Seek(0);
			if (ReadBlock(4) != FileIdentifier)
			{
				throw new CorruptFileException(
					"File does not begin with RIFF identifier");
			}

			riff_size = ReadBlock(4)
				.ToUInt(false);

			var stream_format = ReadBlock(4);
			tag_start = -1;
			tag_end = -1;

			long position = 12;
			var length = Length;
			var duration = TimeSpan.Zero;
			var codecs = new ICodec [0];

			uint size;
			// Read until there are less than 8 bytes to read.
			do
			{
				Seek(position);
				var fourcc = ReadBlock(4)
					.ToString(StringType.UTF8);

				size = ReadBlock(4)
					.ToUInt(false);

				switch (fourcc)
				{
					// "fmt " is used by Wave files to hold the
					// WaveFormatEx structure.
					case "fmt ":
						if (style == ReadStyle.None ||
							stream_format != "WAVE")
						{
							break;
						}

						Seek(position + 8);
						codecs = new ICodec[] { new WaveFormatEx(ReadBlock(18), 0) };
						break;

					// "data" contains the audio data for wave
					// files. It's contents represent the invariant
					// portion of the file and is used to determine
					// the duration of a file. It should always
					// appear after "fmt ".
					case "data":
						if (stream_format != "WAVE")
						{
							break;
						}

						InvariantStartPosition = position;
						InvariantEndPosition = position + size;

						if (style == ReadStyle.None ||
							codecs.Length != 1 ||
							!(codecs[0] is WaveFormatEx))
						{
							break;
						}

						duration += TimeSpan.FromSeconds(
							size / (double)
							((WaveFormatEx)codecs[0])
							.AverageBytesPerSecond);

						break;

					// Lists are used to store a variety of data
					// collections. Read the type and act on it.
					case "LIST":
						{
							switch (ReadBlock(4)
								.ToString(StringType.UTF8))
							{
								// "hdlr" is used by AVI files to hold
								// a media header and BitmapInfoHeader
								// and WaveFormatEx structures.
								case "hdrl":
									if (style == ReadStyle.None ||
										stream_format != "AVI ")
									{
										continue;
									}

									var header_list =
										new AviHeaderList(this,
											position + 12,
											(int)(size - 4));

									duration = header_list.Header.Duration;
									codecs = header_list.Codecs;
									break;

								// "movi" contains the media data for
								// and AVI and its contents represent
								// the invariant portion of the file.
								case "movi":
									if (stream_format != "AVI ")
									{
										break;
									}

									InvariantStartPosition = position;
									InvariantEndPosition = position + size;
									break;
							}

							break;
						}

					// "JUNK" is a padding element that could be
					// associated with tag data.
					case "JUNK":
						if (tag_end == position)
						{
							tag_end = position + 8 + size;
						}

						break;
				}

				// Determine the region of the file that
				// contains tags.
				// Move to the next item.
			} while ((position += 8L + size) + 8 < length);

			// If we're reading properties, and one were found,
			// throw an exception. Otherwise, create the Properties
			// object.
			if (style != ReadStyle.None)
			{
				if (codecs.Length == 0)
				{
					throw new UnsupportedFormatException(
						"Unsupported RIFF type.");
				}

				properties = new Properties(duration, codecs);
			}
		}
	}
}
