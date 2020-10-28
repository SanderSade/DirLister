using System;
using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Ogg
{
	/// <summary>
	///     This class extends <see cref="TagLib.File" /> to provide tagging
	///     and properties support for Ogg files.
	/// </summary>
	[SupportedMimeType("taglib/ogg", "ogg")]
	[SupportedMimeType("taglib/oga", "oga")]
	[SupportedMimeType("taglib/ogv", "ogv")]
	[SupportedMimeType("taglib/opus", "opus")]
	public sealed class File : TagLib.File
	{
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
				Read(propertiesStyle);
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
		///     Gets the last page header in the file.
		/// </summary>
		/// <value>
		///     A <see cref="PageHeader" /> object containing the last
		///     page header in the file.
		/// </value>
		/// <remarks>
		///     The last page header is used to determine the last
		///     absolute granular position of a stream so the duration
		///     can be calculated.
		/// </remarks>
		private PageHeader LastPageHeader
		{
			get
			{
				var last_page_header_offset = RFind("OggS");

				if (last_page_header_offset < 0)
				{
					throw new CorruptFileException(
						"Could not find last header.");
				}

				return new PageHeader(this,
					last_page_header_offset);
			}
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
		///     Reads the file with a specified read style.
		/// </summary>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		private void Read(ReadStyle propertiesStyle)
		{
			var streams = ReadStreams(null,
				out var end);

			var codecs = new List<ICodec>();
			InvariantStartPosition = end;
			InvariantEndPosition = Length;

			foreach (var id in streams.Keys)
			{
				codecs.Add(streams[id]
					.Codec);
			}

			if ((propertiesStyle & ReadStyle.Average) == 0)
			{
				return;
			}

			var last_header = LastPageHeader;

			var duration = streams[last_header
					.StreamSerialNumber]
				.GetDuration(
					last_header.AbsoluteGranularPosition);

			properties = new Properties(duration, codecs);
		}


		/// <summary>
		///     Reads the file until all streams have finished their
		///     property and tagging data.
		/// </summary>
		/// <param name="pages">
		///     A <see cref="T:System.Collections.Generic.List`1" />
		///     object to be filled with <see cref="Page" /> objects as
		///     they are read, or <see langword="null" /> if the pages
		///     are not to be stored.
		/// </param>
		/// <param name="end">
		///     A <see cref="long" /> value reference to be updated to
		///     the postion of the first page not read by the current
		///     instance.
		/// </param>
		/// <returns>
		///     A <see cref="T:System.Collections.Generic.Dictionary`2" /> object containing stream serial numbers as the keys
		///     <see cref="Bitstream" /> objects as the values.
		/// </returns>
		private Dictionary<uint, Bitstream> ReadStreams(List<Page> pages,
			out long end)
		{
			var streams =
				new Dictionary<uint, Bitstream>();

			var active_streams = new List<Bitstream>();

			long position = 0;

			do
			{
				Bitstream stream = null;
				var page = new Page(this, position);

				if ((page.Header.Flags &
				     PageFlags.FirstPageOfStream) != 0)
				{
					stream = new Bitstream(page);
					streams.Add(page.Header
						.StreamSerialNumber, stream);

					active_streams.Add(stream);
				}

				if (stream == null)
				{
					stream = streams[
						page.Header.StreamSerialNumber];
				}

				if (active_streams.Contains(stream)
				    && stream.ReadPage(page))
				{
					active_streams.Remove(stream);
				}

				pages?.Add(page);

				position += page.Size;
			} while (active_streams.Count > 0);

			end = position;

			return streams;
		}
	}
}
