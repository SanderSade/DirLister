using System;

namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///     Represent a MP4 URL box
	/// </summary>
	public class UrlBox : Box
	{
		/// <summary>
		///     Contains the box's data.
		/// </summary>
		private ByteVector data;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="UnknownBox" />
		///     with a provided header and handler
		///     by reading the contents from a specified file.
		/// </summary>
		/// <param name="header">
		///     A <see cref="BoxHeader" /> object containing the header
		///     to use for the new instance.
		/// </param>
		/// <param name="file">
		///     A <see cref="TagLib.File" /> object to read the contents
		///     of the box from.
		/// </param>
		/// <param name="handler">
		///     A <see cref="IsoHandlerBox" /> object containing the
		///     handler that applies to the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		public UrlBox(BoxHeader header, TagLib.File file,
			IsoHandlerBox handler)
			: base(header, handler)
		{
			if (file == null)
			{
				throw new ArgumentNullException(nameof(file));
			}

			data = LoadData(file);
		}


		/// <summary>
		///     Gets and sets the box data contained in the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="ByteVector" /> object containing the box
		///     data contained in the current instance.
		/// </value>
		public override ByteVector Data
		{
			get => data;
			set => data = value;
		}
	}
}
