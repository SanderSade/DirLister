﻿using Sander.DirLister.Core.TagLib.Mpeg4.Boxes;

namespace Sander.DirLister.Core.TagLib.Mpeg4
{
	/// <summary>
	///    This static class provides support for reading boxes from a file.
	/// </summary>
	public static class BoxFactory
	{
		/// <summary>
		///    Creates a box by reading it from a file given its header,
		///    parent header, handler, and index in its parent.
		/// </summary>
		/// <param name="file">
		///    A <see cref="TagLib.File" /> object containing the file
		///    to read from.
		/// </param>
		/// <param name="header">
		///    A <see cref="BoxHeader" /> object containing the header
		///    of the box to create.
		/// </param>
		/// <param name="parent">
		///    A <see cref="BoxHeader" /> object containing the header
		///    of the parent box.
		/// </param>
		/// <param name="handler">
		///    A <see cref="IsoHandlerBox" /> object containing the
		///    handler that applies to the new box.
		/// </param>
		/// <param name="index">
		///    A <see cref="int" /> value containing the index of the
		///    new box in its parent.
		/// </param>
		/// <returns>
		///    A newly created <see cref="Box" /> object.
		/// </returns>
		private static Box CreateBox(TagLib.File file,
			BoxHeader header,
			BoxHeader parent,
			IsoHandlerBox handler,
			int index)
		{
			// The first few children of an "stsd" are sample
			// entries.
			if (parent.BoxType == BoxType.Stsd &&
			    parent.Box is IsoSampleDescriptionBox &&
			    index < (parent.Box as IsoSampleDescriptionBox).EntryCount)
			{
				if (handler != null && handler.HandlerType == BoxType.Soun)
					return new IsoAudioSampleEntry(header, file, handler);
				if (handler != null && handler.HandlerType == BoxType.Vide)
					return new IsoVisualSampleEntry(header, file, handler);
				if (handler != null && handler.HandlerType == BoxType.Alis)
				{
					if (header.BoxType == BoxType.Text)
						return new TextBox(header, file, handler);
					// This could be anything, so just parse it
					return new UnknownBox(header, file, handler);
				}

				return new IsoSampleEntry(header,
					file, handler);
			}

			// Standard items...
			var type = header.BoxType;

			if (type == BoxType.Mvhd)
				return new IsoMovieHeaderBox(header, file,
					handler);
			if (type == BoxType.Stbl)
				return new IsoSampleTableBox(header, file,
					handler);
			if (type == BoxType.Stsd)
				return new IsoSampleDescriptionBox(header,
					file, handler);
			if (type == BoxType.Stco)
				return new IsoChunkOffsetBox(header, file,
					handler);
			if (type == BoxType.Co64)
				return new IsoChunkLargeOffsetBox(header, file,
					handler);
			if (type == BoxType.Hdlr)
				return new IsoHandlerBox(header, file,
					handler);
			if (type == BoxType.Udta)
				return new IsoUserDataBox(header, file,
					handler);
			if (type == BoxType.Meta)
				return new IsoMetaBox(header, file, handler);
			if (type == BoxType.Ilst)
				return new AppleItemListBox(header, file,
					handler);
			if (type == BoxType.Data)
				return new AppleDataBox(header, file, handler);
			if (type == BoxType.Esds)
				return new AppleElementaryStreamDescriptor(
					header, file, handler);
			if (type == BoxType.Free || type == BoxType.Skip)
				return new IsoFreeSpaceBox(header, file,
					handler);
			if (type == BoxType.Mean || type == BoxType.Name)
				return new AppleAdditionalInfoBox(header, file,
					handler);

			// If we still don't have a tag, and we're inside an
			// ItemListBox, load the box as an AnnotationBox
			// (Apple tag item).
			if (parent.BoxType == BoxType.Ilst)
				return new AppleAnnotationBox(header, file,
					handler);

			// Nothing good. Go generic.
			return new UnknownBox(header, file, handler);
		}


		/// <summary>
		///    Creates a box by reading it from a file given its
		///    position in the file, parent header, handler, and index
		///    in its parent.
		/// </summary>
		/// <param name="file">
		///    A <see cref="TagLib.File" /> object containing the file
		///    to read from.
		/// </param>
		/// <param name="position">
		///    A <see cref="long" /> value specifying at what seek
		///    position in <paramref name="file" /> to start reading.
		/// </param>
		/// <param name="parent">
		///    A <see cref="BoxHeader" /> object containing the header
		///    of the parent box.
		/// </param>
		/// <param name="handler">
		///    A <see cref="IsoHandlerBox" /> object containing the
		///    handler that applies to the new box.
		/// </param>
		/// <param name="index">
		///    A <see cref="int" /> value containing the index of the
		///    new box in its parent.
		/// </param>
		/// <returns>
		///    A newly created <see cref="Box" /> object.
		/// </returns>
		internal static Box CreateBox(TagLib.File file, long position,
			BoxHeader parent,
			IsoHandlerBox handler, int index)
		{
			var header = new BoxHeader(file, position);
			return CreateBox(file, header, parent, handler, index);
		}


		/// <summary>
		///    Creates a box by reading it from a file given its
		///    position in the file and handler.
		/// </summary>
		/// <param name="file">
		///    A <see cref="TagLib.File" /> object containing the file
		///    to read from.
		/// </param>
		/// <param name="position">
		///    A <see cref="long" /> value specifying at what seek
		///    position in <paramref name="file" /> to start reading.
		/// </param>
		/// <param name="handler">
		///    A <see cref="IsoHandlerBox" /> object containing the
		///    handler that applies to the new box.
		/// </param>
		/// <returns>
		///    A newly created <see cref="Box" /> object.
		/// </returns>
		public static Box CreateBox(TagLib.File file, long position,
			IsoHandlerBox handler)
		{
			return CreateBox(file, position, BoxHeader.Empty,
				handler, -1);
		}


		/// <summary>
		///    Creates a box by reading it from a file given its
		///    position in the file.
		/// </summary>
		/// <param name="file">
		///    A <see cref="TagLib.File" /> object containing the file
		///    to read from.
		/// </param>
		/// <param name="position">
		///    A <see cref="long" /> value specifying at what seek
		///    position in <paramref name="file" /> to start reading.
		/// </param>
		/// <returns>
		///    A newly created <see cref="Box" /> object.
		/// </returns>
		public static Box CreateBox(TagLib.File file, long position)
		{
			return CreateBox(file, position, null);
		}


		/// <summary>
		///    Creates a box by reading it from a file given its header
		///    and handler.
		/// </summary>
		/// <param name="file">
		///    A <see cref="TagLib.File" /> object containing the file
		///    to read from.
		/// </param>
		/// <param name="header">
		///    A <see cref="BoxHeader" /> object containing the header
		///    of the box to create.
		/// </param>
		/// <param name="handler">
		///    A <see cref="IsoHandlerBox" /> object containing the
		///    handler that applies to the new box.
		/// </param>
		/// <returns>
		///    A newly created <see cref="Box" /> object.
		/// </returns>
		public static Box CreateBox(TagLib.File file, BoxHeader header,
			IsoHandlerBox handler)
		{
			return CreateBox(file, header, BoxHeader.Empty,
				handler, -1);
		}


		/// <summary>
		///    Creates a box by reading it from a file given its header
		///    and handler.
		/// </summary>
		/// <param name="file">
		///    A <see cref="TagLib.File" /> object containing the file
		///    to read from.
		/// </param>
		/// <param name="header">
		///    A <see cref="BoxHeader" /> object containing the header
		///    of the box to create.
		/// </param>
		/// <returns>
		///    A newly created <see cref="Box" /> object.
		/// </returns>
		public static Box CreateBox(TagLib.File file, BoxHeader header)
		{
			return CreateBox(file, header, null);
		}
	}
}