using System;
using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Ogg
{
	/// <summary>
	///     This class provides a representation of an Ogg page.
	/// </summary>
	public class Page
	{
		/// <summary>
		///     Contains the packets.
		/// </summary>
		private readonly ByteVectorCollection packets;


		/// <summary>
		///     Constructs and intializes a new instance of
		///     <see
		///         cref="Page" />
		///     with a specified header and no packets.
		/// </summary>
		/// <param name="header">
		///     A <see cref="PageHeader" /> object to use as the header of
		///     the new instance.
		/// </param>
		protected Page(PageHeader header)
		{
			Header = header;
			packets = new ByteVectorCollection();
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="Page" />
		///     by reading a raw Ogg page from a specified
		///     position in a specified file.
		/// </summary>
		/// <param name="file">
		///     A <see cref="File" /> object containing the file from
		///     which the contents of the new instance are to be read.
		/// </param>
		/// <param name="position">
		///     A <see cref="long" /> value specify at what position to
		///     read.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///     <paramref name="position" /> is less than zero or greater
		///     than the size of the file.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///     The Ogg identifier could not be found at the correct
		///     location.
		/// </exception>
		public Page(File file, long position)
			: this(new PageHeader(file, position))
		{
			file.Seek(position + Header.Size);

			foreach (var packet_size in Header.PacketSizes)
			{
				packets.Add(file.ReadBlock(packet_size));
			}
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="Page" />
		///     with a specified header and packets.
		/// </summary>
		/// <param name="packets">
		///     A <see cref="ByteVectorCollection" /> object containing
		///     packets to use for the new instance.
		/// </param>
		/// <param name="header">
		///     A <see cref="PageHeader" /> object to use as the header of
		///     the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="packets" /> is <see langword="null" />.
		/// </exception>
		public Page(ByteVectorCollection packets, PageHeader header)
			: this(header)
		{
			if (packets == null)
			{
				throw new ArgumentNullException(nameof(packets));
			}

			this.packets = new ByteVectorCollection(packets);

			var packet_sizes = new List<int>();

			// Build a page from the list of packets.
			foreach (var v in packets)
			{
				packet_sizes.Add(v.Count);
			}

			header.PacketSizes = packet_sizes.ToArray();
		}


		/// <summary>
		///     Gets the header of the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="PageHeader" /> object that applies to the
		///     current instance.
		/// </value>
		public PageHeader Header { get; }

		/// <summary>
		///     Gets the packets contained in the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="T:ByteVector[]" /> containing the packets
		///     contained in the current instance.
		/// </value>
		public ByteVector[] Packets => packets.ToArray();

		/// <summary>
		///     Gets the total size of the current instance as it
		///     appeared on disk.
		/// </summary>
		/// <value>
		///     A <see cref="uint" /> value containing the size of the
		///     page, including the header, as it appeared on disk.
		/// </value>
		public uint Size => Header.Size + Header.DataSize;


		/// <summary>
		///     Renders the current instance as a raw Ogg page.
		/// </summary>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered version of the current instance.
		/// </returns>
		public ByteVector Render()
		{
			var data = Header.Render();

			foreach (var v in packets)
			{
				data.Add(v);
			}

			// Compute and set the checksum for the Ogg page. The
			// checksum is taken over the entire page with the 4
			// bytes reserved for the checksum zeroed and then
			// inserted in bytes 22-25 of the page header.

			var checksum = ByteVector.FromUInt(
				data.Checksum, false);

			for (var i = 0; i < 4; i++)
			{
				data[i + 22] = checksum[i];
			}

			return data;
		}
	}
}
