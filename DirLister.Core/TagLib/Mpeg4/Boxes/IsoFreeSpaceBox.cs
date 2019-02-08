namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///    This class extends <see cref="Box" /> to provide an
	///    implementation of a ISO/IEC 14496-12 FreeSpaceBox.
	/// </summary>
	public class IsoFreeSpaceBox : Box
	{
		/// <summary>
		///    Contains the size of the padding.
		/// </summary>
		private long padding;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="IsoMetaBox" /> with a provided header and
		///    handler by reading the contents from a specified file.
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
		public IsoFreeSpaceBox(BoxHeader header, TagLib.File file,
			IsoHandlerBox handler)
			: base(header, handler)
		{
			padding = DataSize;
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="IsoFreeSpaceBox" /> to occupy a specified number of
		///    bytes.
		/// </summary>
		/// <param name="padding">
		///    A <see cref="long" /> value specifying the number of
		///    bytes the new instance should occupy when rendered.
		/// </param>
		public IsoFreeSpaceBox(long padding) : base("free")
		{
			PaddingSize = padding;
		}


		/// <summary>
		///    Gets and sets the data contained in the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="ByteVector" /> object containing the data
		///    contained in the current instance.
		/// </value>
		public override ByteVector Data
		{
			get => new ByteVector((int)padding);
			set => padding = value != null ? value.Count : 0;
		}

		/// <summary>
		///    Gets and sets the size the current instance will occupy
		///    when rendered.
		/// </summary>
		/// <value>
		///    A <see cref="long" /> value containing the size the
		///    current instance will occupy when rendered.
		/// </value>
		public long PaddingSize
		{
			get => padding + 8;
			set => padding = value - 8;
		}
	}
}
