using System;

namespace Sander.DirLister.Core.TagLib.Ogg.Codecs
{
	/// <summary>
	///     This class extends <see cref="Codec" /> and implements
	///     <see
	///         cref="IVideoCodec" />
	///     to provide support for processing Ogg
	///     Theora bitstreams.
	/// </summary>
	public class Theora : Codec, IVideoCodec
	{
		/// <summary>
		///     Contains the file identifier.
		/// </summary>
		private static readonly ByteVector id = "theora";

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
		///         cref="Theora" />
		///     .
		/// </summary>
		private Theora()
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
		///     Gets the width of the video represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the width of the
		///     video represented by the current instance.
		/// </value>
		public int VideoWidth => header.width;

		/// <summary>
		///     Gets the height of the video represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the height of the
		///     video represented by the current instance.
		/// </value>
		public int VideoHeight => header.height;

		/// <summary>
		///     Gets the types of media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     Always <see cref="MediaTypes.Video" />.
		/// </value>
		public override MediaTypes MediaTypes => MediaTypes.Video;


		/// <summary>
		///     Implements the <see cref="T:CodecProvider" /> delegate to
		///     provide support for recognizing a Theora stream from the
		///     header packet.
		/// </summary>
		/// <param name="packet">
		///     A <see cref="ByteVector" /> object containing the stream
		///     header packet.
		/// </param>
		/// <returns>
		///     A <see cref="Codec" /> object containing a codec capable
		///     of parsing the stream of <see langref="null" /> if the
		///     stream is not a Theora stream.
		/// </returns>
		public static Codec FromPacket(ByteVector packet)
		{
			return PacketType(packet) == 0x80 ? new Theora() : null;
		}


		/// <summary>
		///     Gets the packet type for a specified Theora packet.
		/// </summary>
		/// <param name="packet">
		///     A <see cref="ByteVector" /> object containing a Theora
		///     packet.
		/// </param>
		/// <returns>
		///     A <see cref="int" /> value containing the packet type or
		///     -1 if the packet is invalid.
		/// </returns>
		private static int PacketType(ByteVector packet)
		{
			if (packet.Count <= id.Count || packet[0] < 0x80)
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
				throw new ArgumentNullException("packet");
			}

			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index",
					"index must be at least zero.");
			}

			var type = PacketType(packet);
			if (type != 0x80 && index == 0)
			{
				throw new CorruptFileException(
					"Stream does not begin with theora header.");
			}

			if (comment_data == null)
			{
				if (type == 0x80)
				{
					header = new HeaderPacket(packet);
				}
				else if (type == 0x81)
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
			return TimeSpan.FromSeconds(
				header.GranuleTime(lastGranularPosition) -
				header.GranuleTime(firstGranularPosition));
		}


		/// <summary>
		///     This structure represents a Theora header packet.
		/// </summary>
		private struct HeaderPacket
		{
			public readonly byte major_version;
			public readonly byte minor_version;
			public byte revision_version;
			public readonly int width;
			public readonly int height;
			public readonly int fps_numerator;
			public readonly int fps_denominator;
			public readonly int keyframe_granule_shift;


			public HeaderPacket(ByteVector data)
			{
				major_version = data[7];
				minor_version = data[8];
				revision_version = data[9];
				// width = data.Mid (10, 2).ToShort () << 4;
				// height = data.Mid (12, 2).ToShort () << 4;
				width = (int)data.Mid(14, 3)
					.ToUInt(); // Frame Width.

				height = (int)data.Mid(17, 3)
					.ToUInt(); // Frame Height.

				// Offset X.
				// Offset Y.
				fps_numerator = (int)data.Mid(22, 4)
					.ToUInt();

				fps_denominator = (int)data.Mid(26, 4)
					.ToUInt();

				// Aspect Numerator.
				// Aspect Denominator.
				// Colorspace.
				// Target bitrate.
				var last_bits = data.Mid(40, 2)
					.ToUShort();

				keyframe_granule_shift = (last_bits >> 5) & 0x1F;
			}


			/// <summary>
			///     Converts an absolute granular position into a
			///     seconds.
			/// </summary>
			/// <param name="granularPosition">
			///     A <see cref="long" /> value containing the
			///     absolute granular position.
			/// </param>
			/// <returns>
			///     A <see cref="double" /> value containing the time
			///     at <paramref name="granularPosition" /> in
			///     seconds.
			/// </returns>
			/// <remarks>
			///     Many thanks to the good people at
			///     irc://irc.freenode.net#theora for making this
			///     code a reality.
			/// </remarks>
			public double GranuleTime(long granularPosition)
			{
				var iframe = granularPosition >>
				             keyframe_granule_shift;

				var pframe = granularPosition -
				             (iframe << keyframe_granule_shift);

				return (iframe + pframe) *
				       (fps_denominator /
				        (double)fps_numerator);
			}
		}
	}
}
