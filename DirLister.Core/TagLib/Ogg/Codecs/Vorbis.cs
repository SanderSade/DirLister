using System;

namespace Sander.DirLister.Core.TagLib.Ogg.Codecs
{
	/// <summary>
	///     This class extends <see cref="Codec" /> and implements
	///     <see
	///         cref="IAudioCodec" />
	///     to provide support for processing Ogg
	///     Vorbis bitstreams.
	/// </summary>
	public sealed class Vorbis : Codec, IAudioCodec
	{
		/// <summary>
		///     Contains the file identifier.
		/// </summary>
		private static readonly ByteVector id = "vorbis";

		/// <summary>
		///     Contains the comment data.
		/// </summary>
		private ByteVector comment_data;

		/// <summary>
		///     Contains the header packet.
		/// </summary>
		private HeaderPacket header;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="Vorbis" />
		///     .
		/// </summary>
		private Vorbis()
		{
		}


		/// <summary>
		///     Gets the raw Xiph comment data contained in the codec.
		/// </summary>
		/// <value>
		///     A <see cref="ByteVector" /> object containing a raw Xiph
		///     comment or <see langword="null" /> if none was found.
		/// </value>
		public override ByteVector CommentData => comment_data;

		/// <summary>
		///     Gets the bitrate of the audio represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing a bitrate of the
		///     audio represented by the current instance.
		/// </value>
		public int AudioBitrate => (int)(header.bitrate_nominal /
			1000f + 0.5);

		/// <summary>
		///     Gets the sample rate of the audio represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the sample rate of
		///     the audio represented by the current instance.
		/// </value>
		public int AudioSampleRate => (int)header.sample_rate;

		/// <summary>
		///     Gets the number of channels in the audio represented by
		///     the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the number of
		///     channels in the audio represented by the current
		///     instance.
		/// </value>
		public int AudioChannels => (int)header.channels;

		/// <summary>
		///     Gets the types of media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     Always <see cref="MediaTypes.Audio" />.
		/// </value>
		public override MediaTypes MediaTypes => MediaTypes.Audio;


		/// <summary>
		///     Implements the <see cref="T:CodecProvider" /> delegate to
		///     provide support for recognizing a Vorbis stream from the
		///     header packet.
		/// </summary>
		/// <param name="packet">
		///     A <see cref="ByteVector" /> object containing the stream
		///     header packet.
		/// </param>
		/// <returns>
		///     A <see cref="Codec" /> object containing a codec capable
		///     of parsing the stream of <see langref="null" /> if the
		///     stream is not a Vorbis stream.
		/// </returns>
		public static Codec FromPacket(ByteVector packet)
		{
			return PacketType(packet) == 1 ? new Vorbis() : null;
		}


		/// <summary>
		///     Gets the packet type for a specified Vorbis packet.
		/// </summary>
		/// <param name="packet">
		///     A <see cref="ByteVector" /> object containing a Vorbis
		///     packet.
		/// </param>
		/// <returns>
		///     A <see cref="int" /> value containing the packet type or
		///     -1 if the packet is invalid.
		/// </returns>
		private static int PacketType(ByteVector packet)
		{
			if (packet.Count <= id.Count)
			{
				return -1;
			}

			for (var i = 0; i < id.Count; i++)
			{
				if (packet[i + 1] != id[i])
				{
					return -1;
				}
			}

			return packet[0];
		}


		/// <summary>
		///     Reads a Ogg packet that has been encountered in the
		///     stream.
		/// </summary>
		/// <param name="packet">
		///     A <see cref="ByteVector" /> object containing a packet to
		///     be read by the current instance.
		/// </param>
		/// <param name="index">
		///     A <see cref="int" /> value containing the index of the
		///     packet in the stream.
		/// </param>
		/// <returns>
		///     <see langword="true" /> if the codec has read all the
		///     necessary packets for the stream and does not need to be
		///     called again, typically once the Xiph comment has been
		///     found. Otherwise <see langword="false" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="packet" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///     <paramref name="index" /> is less than zero.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///     The data does not conform to the specificiation for the
		///     codec represented by the current instance.
		/// </exception>
		public override bool ReadPacket(ByteVector packet, int index)
		{
			if (packet == null)
			{
				throw new ArgumentNullException(nameof(packet));
			}

			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index),
					"index must be at least zero.");
			}

			var type = PacketType(packet);
			if (type != 1 && index == 0)
			{
				throw new CorruptFileException(
					"Stream does not begin with vorbis header.");
			}

			if (comment_data == null)
			{
				if (type == 1)
				{
					header = new HeaderPacket(packet);
				}
				else if (type == 3)
				{
					comment_data = packet.Mid(7);
				}
				else
				{
					return true;
				}
			}

			return comment_data != null;
		}


		/// <summary>
		///     Computes the duration of the stream using the first and
		///     last granular positions of the stream.
		/// </summary>
		/// <param name="firstGranularPosition">
		///     A <see cref="long" /> value containing the first granular
		///     position of the stream.
		/// </param>
		/// <param name="lastGranularPosition">
		///     A <see cref="long" /> value containing the last granular
		///     position of the stream.
		/// </param>
		/// <returns>
		///     A <see cref="TimeSpan" /> value containing the duration
		///     of the stream.
		/// </returns>
		public override TimeSpan GetDuration(long firstGranularPosition,
			long lastGranularPosition)
		{
			return header.sample_rate == 0
				? TimeSpan.Zero
				: TimeSpan.FromSeconds((lastGranularPosition -
				                        firstGranularPosition) /
				                       (double)header.sample_rate);
		}


		/// <summary>
		///     This structure represents a Vorbis header packet.
		/// </summary>
		private struct HeaderPacket
		{
			public readonly uint sample_rate;
			public readonly uint channels;
			public readonly uint vorbis_version;
			public uint bitrate_maximum;
			public readonly uint bitrate_nominal;
			public uint bitrate_minimum;


			public HeaderPacket(ByteVector data)
			{
				vorbis_version = data.Mid(7, 4)
					.ToUInt(false);

				channels = data[11];
				sample_rate = data.Mid(12, 4)
					.ToUInt(false);

				bitrate_maximum = data.Mid(16, 4)
					.ToUInt(false);

				bitrate_nominal = data.Mid(20, 4)
					.ToUInt(false);

				bitrate_minimum = data.Mid(24, 4)
					.ToUInt(false);
			}
		}
	}
}
