using System;

namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///    This class extends <see cref="Box" /> to provide an
	///    implementation of a ISO/IEC 14496-12 FullBox.
	/// </summary>
	public abstract class FullBox : Box
	{

		/// <summary>
		///    Contains the box version.
		/// </summary>
		private byte version;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="FullBox" /> with a provided header and handler by
		///    reading the contents from a specified file.
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
		protected FullBox(BoxHeader header, TagLib.File file,
			IsoHandlerBox handler)
			: base(header, handler)
		{
			if (file == null)
				throw new ArgumentNullException("file");

			file.Seek(base.DataPosition);
			var header_data = file.ReadBlock(4);
			version = header_data[0];
			Flags = header_data.Mid(1, 3)
			                   .ToUInt();
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="FullBox" /> with a provided header, version, and
		///    flags.
		/// </summary>
		/// <param name="header">
		///    A <see cref="BoxHeader" /> object containing the header
		///    to use for the new instance.
		/// </param>
		/// <param name="version">
		///    A <see cref="byte" /> value containing the version of the
		///    new instance.
		/// </param>
		/// <param name="flags">
		///    A <see cref="byte" /> value containing the flags for the
		///    new instance.
		/// </param>
		protected FullBox(BoxHeader header, byte version, uint flags)
			: base(header)
		{
			this.version = version;
			Flags = flags;
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="FullBox" /> with a provided header, version, and
		///    flags.
		/// </summary>
		/// <param name="type">
		///    A <see cref="ByteVector" /> object containing the four
		///    byte box type.
		/// </param>
		/// <param name="version">
		///    A <see cref="byte" /> value containing the version of the
		///    new instance.
		/// </param>
		/// <param name="flags">
		///    A <see cref="byte" /> value containing the flags for the
		///    new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="type" /> is <see langword="null" /> of
		///    equal to "<c>uuid</c>".
		/// </exception>
		/// <exception cref="ArgumentException">
		///    <paramref name="type" /> isn't exactly 4 bytes long.
		/// </exception>
		protected FullBox(ByteVector type, byte version, uint flags)
			: this(new BoxHeader(type), version, flags)
		{
		}


		/// <summary>
		///    Gets the position of the data contained in the current
		///    instance, after any box specific headers.
		/// </summary>
		/// <value>
		///    A <see cref="long" /> value containing the position of
		///    the data contained in the current instance.
		/// </value>
		protected override long DataPosition => base.DataPosition + 4;

		/// <summary>
		///    Gets and sets the version number of the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="byte" /> value containing the version
		///    number of the current instance.
		/// </value>
		public uint Version
		{
			get => version;
			set => version = (byte)value;
		}

		/// <summary>
		///    Gets and sets the flags that apply to the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="uint" /> value containing the flags that
		///    apply to the current instance.
		/// </value>
		public uint Flags { get; set; }


		/// <summary>
		///    Renders the current instance, including its children, to
		///    a new <see cref="ByteVector" /> object, preceeding the
		///    contents with a specified block of data.
		/// </summary>
		/// <param name="topData">
		///    A <see cref="ByteVector" /> object containing box
		///    specific header data to preceed the content.
		/// </param>
		/// <returns>
		///    A <see cref="ByteVector" /> object containing the
		///    rendered version of the current instance.
		/// </returns>
		protected override ByteVector Render(ByteVector topData)
		{
			var output = new ByteVector(version)
			{
				ByteVector.FromUInt(Flags)
				          .Mid(1, 3),
				topData
			};

			return base.Render(output);
		}
	}
}
