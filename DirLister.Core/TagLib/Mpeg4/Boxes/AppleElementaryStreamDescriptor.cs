using System;

namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///    This class extends <see cref="FullBox" /> to provide an
	///    implementation of an Apple ElementaryStreamDescriptor.
	/// </summary>
	/// <remarks>
	///    This box may appear as a child of a <see
	///    cref="IsoAudioSampleEntry" /> and provided further information
	///    about an audio stream.
	/// </remarks>
	public class AppleElementaryStreamDescriptor : FullBox
	{
		/// <summary>
		///    Contains the average bitrate.
		/// </summary>
		private readonly uint average_bitrate;

		/// <summary>
		///    Contains the maximum bitrate.
		/// </summary>
		private readonly uint max_bitrate;

		/// <summary>
		///    Contains the stream type.
		/// </summary>
		private readonly byte stream_type;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="AppleElementaryStreamDescriptor" /> with a provided
		///    header and handler by reading the contents from a
		///    specified file.
		/// </summary>
		/// <param name="header">
		///    A <see cref="BoxHeader" /> object containing the header
		///    to use for the new instance.
		/// </param>
		/// <param name="file">
		///    A <see cref="TagLib.File" /> object to read the contents
		///    of the box from.
		/// </param>
		/// <param name="handler">
		///    A <see cref="IsoHandlerBox" /> object containing the
		///    handler that applies to the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///    Valid data could not be read.
		/// </exception>
		public AppleElementaryStreamDescriptor(BoxHeader header,
			TagLib.File file,
			IsoHandlerBox handler)
			: base(header, file, handler)
		{
			var offset = 0;
			var box_data = file.ReadBlock(DataSize);
			DecoderConfig = new ByteVector();

			// Elementary Stream Descriptor Tag
			if (box_data[offset++] == 3)
			{
				// We have a descriptor tag. Check that it's at
				// least 20 long.
				if (ReadLength(box_data, ref offset) < 20)
					throw new CorruptFileException(
						"Insufficient data present.");

				StreamId = box_data.Mid(offset, 2)
				                .ToUShort();
				offset += 2;
				StreamPriority = box_data[offset++];
			}
			else
			{
				// The tag wasn't found, so the next two byte
				// are the ID, and after that, business as
				// usual.
				StreamId = box_data.Mid(offset, 2)
				                .ToUShort();
				offset += 2;
			}

			// Verify that the next data is the Decoder
			// Configuration Descriptor Tag and escape if it won't
			// work out.
			if (box_data[offset++] != 4)
				throw new CorruptFileException(
					"Could not identify decoder configuration descriptor.");

			// Check that it's at least 15 long.
			if (ReadLength(box_data, ref offset) < 15)
				throw new CorruptFileException(
					"Could not read data. Too small.");

			// Read a lot of good info.
			ObjectTypeId = box_data[offset++];
			stream_type = box_data[offset++];
			BufferSizeDB = box_data.Mid(offset, 3)
			                         .ToUInt();
			offset += 3;
			max_bitrate = box_data.Mid(offset, 4)
			                      .ToUInt();
			offset += 4;
			average_bitrate = box_data.Mid(offset, 4)
			                          .ToUInt();
			offset += 4;

			// Verify that the next data is the Decoder Specific
			// Descriptor Tag and escape if it won't work out.
			if (box_data[offset++] != 5)
				throw new CorruptFileException(
					"Could not identify decoder specific descriptor.");

			// The rest of the info is decoder specific.
			var length = ReadLength(box_data, ref offset);
			DecoderConfig = box_data.Mid(offset, (int)length);
		}


		/// <summary>
		///    Gets the ID of the stream described by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="ushort" /> value containing the ID of the
		///    stream described by the current instance.
		/// </value>
		public ushort StreamId { get; }

		/// <summary>
		///    Gets the priority of the stream described by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="byte" /> value containing the priority of
		///    the stream described by the current instance.
		/// </value>
		public byte StreamPriority { get; }

		/// <summary>
		///    Gets the object type ID of the stream described by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="byte" /> value containing the object type ID
		///    of the stream described by the current instance.
		/// </value>
		public byte ObjectTypeId { get; }

		/// <summary>
		///    Gets the type the stream described by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="byte" /> value containing the type the
		///    stream described by the current instance.
		/// </value>
		public byte StreamType => stream_type;

		/// <summary>
		///    Gets the buffer size DB value the stream described by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="uint" /> value containing the buffer size DB
		///    value the stream described by the current instance.
		/// </value>
		public uint BufferSizeDB { get; }

		/// <summary>
		///    Gets the maximum bitrate the stream described by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="uint" /> value containing the maximum
		///    bitrate the stream described by the current instance.
		/// </value>
		public uint MaximumBitrate => max_bitrate / 1000;

		/// <summary>
		///    Gets the maximum average the stream described by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="uint" /> value containing the average
		///    bitrate the stream described by the current instance.
		/// </value>
		public uint AverageBitrate => average_bitrate / 1000;

		/// <summary>
		///    Gets the decoder config data of stream described by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="ByteVector" /> object containing the decoder
		///    config data of the stream described by the current
		///    instance.
		/// </value>
		public ByteVector DecoderConfig { get; }


		/// <summary>
		///    Reads a section length and updates the offset to the end
		///    of of the length block.
		/// </summary>
		/// <param name="data">
		///    A <see cref="ByteVector" /> object to read from.
		/// </param>
		/// <param name="offset">
		///    A <see cref="int" /> value reference specifying the
		///    offset at which to read. This value gets updated to the
		///    position following the size data.
		/// </param>
		/// <returns>
		///    A <see cref="uint" /> value containing the length that
		///    was read.
		/// </returns>
		private static uint ReadLength(ByteVector data, ref int offset)
		{
			byte b;
			var end = offset + 4;
			uint length = 0;

			do
			{
				b = data[offset++];
				length = length << 7 |
				         (uint)(b & 0x7f);
			} while ((b & 0x80) != 0 && offset <= end);

			return length;
		}
	}
}
