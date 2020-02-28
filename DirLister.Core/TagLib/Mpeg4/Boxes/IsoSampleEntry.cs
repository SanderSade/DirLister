using System;

namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///     This class extends <see cref="Box" /> to provide an
	///     implementation of a ISO/IEC 14496-12 SampleEntry.
	/// </summary>
	public class IsoSampleEntry : Box
	{
		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="IsoSampleEntry" />
		///     with a provided header and
		///     handler by reading the contents from a specified file.
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
		public IsoSampleEntry(BoxHeader header, TagLib.File file,
			IsoHandlerBox handler)
			: base(header, handler)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}

			file.Seek(base.DataPosition + 6);
			DataReferenceIndex = file.ReadBlock(2)
				.ToUShort();
		}


		/// <summary>
		///     Gets the position of the data contained in the current
		///     instance, after any box specific headers.
		/// </summary>
		/// <value>
		///     A <see cref="long" /> value containing the position of
		///     the data contained in the current instance.
		/// </value>
		protected override long DataPosition => base.DataPosition + 8;

		/// <summary>
		///     Gets the data reference index of the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="ushort" /> value containing the data
		///     reference index of the current instance.
		/// </value>
		public ushort DataReferenceIndex { get; }
	}
}
