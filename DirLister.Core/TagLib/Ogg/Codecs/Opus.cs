using System;

namespace Sander.DirLister.Core.TagLib.Ogg.Codecs
{
	/// <summary>
	///    This class extends <see cref="Codec" /> and implements <see
	///    cref="IAudioCodec" /> to provide support for processing Ogg
	///    Opus bitstreams.
	/// </summary>
	public class Opus : Codec, IAudioCodec
	{
		/// <summary>
		///    Contains the file identifier.
		/// </summary>
		private static readonly ByteVector magic_signature_base = "Opus";

		private static readonly ByteVector magic_signature_header = "OpusHead";
		private static readonly ByteVector magic_signature_comment = "OpusTags";
		private static readonly int magic_signature_length = 8;

		/// <summary>
		///    Contains the comment data.
		/// </summary>
		private ByteVector comment_data;

		/// <summary>
		///    Contains the header packet.
		/// </summary>
		private HeaderPacket header;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="Opus" />.
		/// </summary>
		private Opus()
		{
		}


		/// <summary>
		///    Gets the raw Xiph comment data contained in the codec.
		/// </summary>
		/// <value>
		///    A <see cref="ByteVector" /> object containing a raw Xiph
		///    comment or <see langword="null"/> if none was found.
		/// </value>
		public override ByteVector CommentData => comment_data;

		/// <summary>
		///    Gets the bitrate of the audio represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing a bitrate of the
		///    audio represented by the current instance.
		/// </value>
		/// <remarks>
		///    Always returns zero, since bitrate is variable and no
		///    information is stored in the Ogg header (unlike e.g. Vorbis).
		/// </remarks>
		public int AudioBitrate => 0;

		/// <summary>
		///    Gets the sample rate of the audio represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the original
		///    sample rate of the audio represented by the current instance.
		/// </value>
		public int AudioSampleRate => (int)header.input_sample_rate;

		/// <summary>
		///    Gets the number of channels in the audio represented by
		///    the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the number of
		///    channels in the audio represented by the current
		///    instance.
		/// </value>
		public int AudioChannels => (int)header.channel_count;

		/// <summary>
		///    Gets the types of media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    Always <see cref="MediaTypes.Audio" />.
		/// </value>
		public override MediaTypes MediaTypes => MediaTypes.Audio;


		/// <summary>
		///    Implements the <see cref="T:CodecProvider" /> delegate to
		///    provide support for recognizing a Opus stream from the
		///    header packet.
		/// </summary>
		/// <param name="packet">
		///    A <see cref="ByteVector" /> object containing the stream
		///    header packet.
		/// </param>
		/// <returns>
		///    A <see cref="Codec"/> object containing a codec capable
		///    of parsing the stream of <see langref="null" /> if the
		///    stream is not a Opus stream.
		/// </returns>
		public static Codec FromPacket(ByteVector packet)
		{
			return MagicSignature(packet) == magic_signature_header ? new Opus() : null;
		}


		/// <summary>
		///    Gets the magic signature for a specified Opus packet.
		/// </summary>
		/// <param name="packet">
		///    A <see cref="ByteVector" /> object containing a Opus
		///    packet.
		/// </param>
		/// <returns>
		///    A <see cref="ByteVector" /> value containing the magic
		///    signature or null if the packet is invalid.
		/// </returns>
		private static ByteVector MagicSignature(ByteVector packet)
		{
			if (packet.Count < magic_signature_length)
				return null;

			for (var i = 0; i < magic_signature_base.Count; i++)
				if (packet[i] != magic_signature_base[i])
					return null;

			return packet.Mid(0, magic_signature_length);
		}


		/// <summary>
		///    Reads a Ogg packet that has been encountered in the
		///    stream.
		/// </summary>
		/// <param name="packet">
		///    A <see cref="ByteVector" /> object containing a packet to
		///    be read by the current instance.
		/// </param>
		/// <param name="index">
		///    A <see cref="int" /> value containing the index of the
		///    packet in the stream.
		/// </param>
		/// <returns>
		///    <see langword="true" /> if the codec has read all the
		///    necessary packets for the stream and does not need to be
		///    called again, typically once the Xiph comment has been
		///    found. Otherwise <see langword="false" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="packet" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///    <paramref name="index" /> is less than zero.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///    The data does not conform to the specificiation for the
		///    codec represented by the current instance.
		/// </exception>
		public override bool ReadPacket(ByteVector packet, int index)
		{
			if (packet == null)
				throw new ArgumentNullException("packet");

			if (index < 0)
				throw new ArgumentOutOfRangeException("index",
					"index must be at least zero.");

			var signature = MagicSignature(packet);
			if (signature != magic_signature_header && index == 0)
				throw new CorruptFileException(
					"Stream does not begin with opus header.");

			if (comment_data == null)
			{
				if (signature == magic_signature_header)
					header = new HeaderPacket(packet);
				else if (signature == magic_signature_comment)
					comment_data =
						packet.Mid(magic_signature_length);
				else
					return true;
			}

			return comment_data != null;
		}


		/// <summary>
		///    Computes the duration of the stream using the first and
		///    last granular positions of the stream.
		/// </summary>
		/// <param name="firstGranularPosition">
		///    A <see cref="long" /> value containing the first granular
		///    position of the stream.
		/// </param>
		/// <param name="lastGranularPosition">
		///    A <see cref="long" /> value containing the last granular
		///    position of the stream.
		/// </param>
		/// <returns>
		///    A <see cref="TimeSpan" /> value containing the duration
		///    of the stream.
		/// </returns>
		public override TimeSpan GetDuration(long firstGranularPosition,
			long lastGranularPosition)
		{
			return TimeSpan.FromSeconds((lastGranularPosition -
			                             firstGranularPosition
			                             - 2 * header.pre_skip) /
			                            (double)48000);
		}


		/// <summary>
		///    This structure represents a Opus header packet.
		/// </summary>
		private struct HeaderPacket
		{
			public readonly uint opus_version;
			public readonly uint channel_count;
			public readonly uint pre_skip;
			public readonly uint input_sample_rate;
			public uint output_gain;
			public readonly uint channel_map;
			public uint stream_count;
			public uint two_channel_stream_count;
			public readonly uint[] channel_mappings;


			public HeaderPacket(ByteVector data)
			{
				opus_version = data[8];
				channel_count = data[9];
				pre_skip = data.Mid(10, 2)
				               .ToUInt(false);
				input_sample_rate = data.Mid(12, 4)
				                        .ToUInt(false);
				output_gain = data.Mid(16, 2)
				                  .ToUInt(false);
				channel_map = data[18];

				if (channel_map == 0)
				{
					stream_count = 1;
					two_channel_stream_count = channel_count - 1;

					channel_mappings = new uint[channel_count];
					channel_mappings[0] = 0;
					if (channel_count == 2)
					{
						channel_mappings[1] = 1;
					}
				}
				else
				{
					stream_count = data[19];
					two_channel_stream_count = data[20];

					channel_mappings = new uint[channel_count];
					for (var i = 0; i < channel_count; i++)
					{
						channel_mappings[i] = data[21 + i];
					}
				}
			}
		}
	}
}
