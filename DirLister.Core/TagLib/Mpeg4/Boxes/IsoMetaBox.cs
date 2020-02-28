using System;
using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///     This class extends <see cref="FullBox" /> to provide an
	///     implementation of a ISO/IEC 14496-12 MetaBox.
	/// </summary>
	public class IsoMetaBox : FullBox
	{
		/// <summary>
		///     Contains the children of the box.
		/// </summary>
		private readonly IEnumerable<Box> children;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="IsoMetaBox" />
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
		public IsoMetaBox(BoxHeader header, TagLib.File file,
			IsoHandlerBox handler)
			: base(header, file, handler)
		{
			children = LoadChildren(file);
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="IsoMetaBox" />
		///     with a specified handler.
		/// </summary>
		/// <param name="handlerType">
		///     A <see cref="ByteVector" /> object specifying a 4 byte
		///     handler type.
		/// </param>
		/// <param name="handlerName">
		///     A <see cref="string" /> object specifying the handler
		///     name.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="handlerType" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentException">
		///     <paramref name="handlerType" /> is less than 4 bytes
		///     long.
		/// </exception>
		public IsoMetaBox(ByteVector handlerType, string handlerName)
			: base("meta", 0, 0)
		{
			if (handlerType == null)
			{
				throw new ArgumentNullException("handlerType");
			}

			if (handlerType.Count < 4)
			{
				throw new ArgumentException(
					"The handler type must be four bytes long.",
					"handlerType");
			}

			children = new List<Box>();
			AddChild(new IsoHandlerBox(handlerType, handlerName));
		}


		/// <summary>
		///     Gets the children of the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="T:System.Collections.Generic.IEnumerable`1" /> object enumerating the
		///     children of the current instance.
		/// </value>
		public override IEnumerable<Box> Children => children;
	}
}
