using System;

namespace Sander.DirLister.Core.TagLib.Ogg
{
	/// <summary>
	///     This class accepts a sequence of pages belonging to a single
	///     logical bitstream, processes them, and extracts the tagging and
	///     media information.
	/// </summary>
	public class Bitstream
	{
		/// <summary>
		///     Contains the absolute granular position of the first
		///     page.
		/// </summary>
		private readonly long first_absolute_granular_position;

		/// <summary>
		///     Contains the index of the next packet to be processed.
		/// </summary>
		private int packet_index;

		/// <summary>
		///     Contains the last packet of the previous page in case it
		///     is continued in the next frame.
		/// </summary>
		private ByteVector previous_packet;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="Bitstream" />
		///     capable of processing a specified
		///     page.
		/// </summary>
		/// <param name="page">
		///     The first <see cref="Page" /> object of the stream to be
		///     processed by the new instance.
		/// </param>
		/// <remarks>
		///     The constructor only sets the new instance up to read the
		///     packet, but doesn't actually read it.
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="page" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="UnsupportedFormatException">
		///     No registered codec capable of processing
		///     <paramref
		///         name="page" />
		///     could be found.
		/// </exception>
		public Bitstream(Page page)
		{
			if (page == null)
			{
				throw new ArgumentNullException("page");
			}

			// Assume that the first packet is completely enclosed.
			// This should be sufficient for codec recognition.
			Codec = Codec.GetCodec(page.Packets[0]);

			first_absolute_granular_position =
				page.Header.AbsoluteGranularPosition;
		}


		/// <summary>
		///     Gets the codec object used to interpret the stream
		///     represented by the current instance.
		/// </summary>
		/// <value>
		///     The <see cref="Codec" /> object used by the current
		///     instance.
		/// </value>
		public Codec Codec { get; }


		/// <summary>
		///     Sents a packet to the codec processor to read it.
		/// </summary>
		/// <param name="packet">
		///     A <see cref="ByteVector" /> object containing the next
		///     packet in the stream.
		/// </param>
		/// <returns>
		///     <see langword="true" /> if the codec has read all the
		///     necessary packets for the stream and does not need to be
		///     called again, typically once the Xiph comment has been
		///     found. Otherwise <see langword="false" />.
		/// </returns>
		private bool ReadPacket(ByteVector packet)
		{
			return Codec.ReadPacket(packet, packet_index++);
		}


		/// <summary>
		///     Reads the next logical page in the stream.
		/// </summary>
		/// <param name="page">
		///     The next logical <see cref="Page" /> object in the
		///     stream.
		/// </param>
		/// <returns>
		///     <see langword="true" /> if the codec has read all the
		///     necessary packets for the stream and does not need to be
		///     called again, typically once the Xiph comment has been
		///     found. Otherwise <see langword="false" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="page" /> is <see langword="null" />.
		/// </exception>
		public bool ReadPage(Page page)
		{
			if (page == null)
			{
				throw new ArgumentNullException("page");
			}

			var packets = page.Packets;

			for (var i = 0; i < packets.Length; i++)
			{
				if ((page.Header.Flags &
				     PageFlags.FirstPacketContinued) == 0 &&
				    previous_packet != null)
				{
					if (ReadPacket(previous_packet))
					{
						return true;
					}

					previous_packet = null;
				}

				var packet = packets[i];

				// If we're at the first packet of the page, and
				// we're continuing an old packet, combine the
				// old with the new.
				if (i == 0 && (page.Header.Flags &
				               PageFlags.FirstPacketContinued) != 0 &&
				    previous_packet != null)
				{
					previous_packet.Add(packet);
					packet = previous_packet;
				}

				previous_packet = null;

				if (i == packets.Length - 1)
				{
					// If we're at the last packet of the
					// page, store it.
					previous_packet = new ByteVector(packet);
				}
				else if (ReadPacket(packet))
				{
					// Otherwise, we need to process it.
					return true;
				}
			}

			return false;
		}


		/// <summary>
		///     Gets the duration of the stream represented by the
		///     current instance.
		/// </summary>
		/// <param name="lastAbsoluteGranularPosition">
		///     A <see cref="long" /> value containing the absolute
		///     granular position of the last page in the bitstream.
		/// </param>
		/// <returns>
		///     A <see cref="TimeSpan" /> object containing the duration
		///     of the stream represented by the current instance.
		/// </returns>
		public TimeSpan GetDuration(long lastAbsoluteGranularPosition)
		{
			return Codec.GetDuration(
				first_absolute_granular_position,
				lastAbsoluteGranularPosition);
		}
	}
}
