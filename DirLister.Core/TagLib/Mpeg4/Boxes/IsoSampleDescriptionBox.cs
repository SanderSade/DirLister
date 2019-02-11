using System;
using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///    This class extends <see cref="FullBox" /> to provide an
	///    implementation of a ISO/IEC 14496-12 SampleDescriptionBox.
	/// </summary>
	public class IsoSampleDescriptionBox : FullBox
	{
		/// <summary>
		///    Contains the children of the box.
		/// </summary>
		private readonly IEnumerable<Box> children;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="IsoSampleDescriptionBox" /> with a provided header
		///    and handler by reading the contents from a specified
		///    file.
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
		public IsoSampleDescriptionBox(BoxHeader header,
			TagLib.File file,
			IsoHandlerBox handler)
			: base(header, file, handler)
		{
			if (file == null)
				throw new ArgumentNullException("file");

			EntryCount = file.ReadBlock(4)
			                  .ToUInt();
			children = LoadChildren(file);
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
		///    Gets the number of boxes at the begining of the children
		///    that will be stored as <see cref="IsoAudioSampleEntry" />
		///    of <see cref="IsoVisualSampleEntry" /> objects, depending
		///    on the handler.
		/// </summary>
		/// <value>
		///    A <see cref="uint" /> value containing the number of
		///    children that will appear as sample entries.
		/// </value>
		public uint EntryCount { get; }

		/// <summary>
		///    Gets the children of the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="T:System.Collections.Generic.IEnumerable`1" /> object enumerating the
		///    children of the current instance.
		/// </value>
		public override IEnumerable<Box> Children => children;
	}
}