using System;
using System.Collections.Generic;
using System.IO;

namespace Sander.DirLister.Core.TagLib.Flac
{
	/// <summary>
	///    This class extends <see cref="TagLib.NonContainer.File" /> to
	///    provide tagging and properties support for Xiph's Flac audio
	///    files.
	/// </summary>
	[SupportedMimeType("taglib/flac", "flac")]
	[SupportedMimeType("taglib/fla", "fla")]
	public sealed class File : NonContainer.File
	{
		/// <summary>
		///    Contains the Flac header block.
		/// </summary>
		private ByteVector header_block;

		/// <summary>
		///    Contains the stream start position.
		/// </summary>
		private long stream_start;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified path in the local file
		///    system and specified read style.
		/// </summary>
		/// <param name="path">
		///    A <see cref="string" /> object containing the path of the
		///    file to use in the new instance.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path, ReadStyle propertiesStyle)
			: base(path, propertiesStyle)
		{
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified path in the local file
		///    system with an average read style.
		/// </summary>
		/// <param name="path">
		///    A <see cref="string" /> object containing the path of the
		///    file to use in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path)
			: base(path)
		{
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified file abstraction and
		///    specified read style.
		/// </summary>
		/// <param name="abstraction">
		///    A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///    reading from and writing to the file.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="abstraction" /> is <see langword="null"
		///    />.
		/// </exception>
		public File(IFileAbstraction abstraction,
			ReadStyle propertiesStyle)
			: base(abstraction, propertiesStyle)
		{
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified file abstraction with an
		///    average read style.
		/// </summary>
		/// <param name="abstraction">
		///    A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///    reading from and writing to the file.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="abstraction" /> is <see langword="null"
		///    />.
		/// </exception>
		public File(IFileAbstraction abstraction)
			: base(abstraction)
		{
		}


		/// <summary>
		///    Reads format specific information at the start of the
		///    file.
		/// </summary>
		/// <param name="start">
		///    A <see cref="long" /> value containing the seek position
		///    at which the tags end and the media data begins.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		protected override void ReadStart(long start,
			ReadStyle propertiesStyle)
		{
			var blocks = ReadBlocks(ref start, out var end,
				BlockMode.Whitelist, BlockType.StreamInfo,
				BlockType.XiphComment, BlockType.Picture);

			if ((propertiesStyle & ReadStyle.Average) != 0)
			{
				// Check that the first block is a
				// METADATA_BLOCK_STREAMINFO.
				if (blocks.Count == 0 ||
				    blocks[0]
					    .Type != BlockType.StreamInfo)
					throw new CorruptFileException(
						"FLAC stream does not begin with StreamInfo.");

				// The stream exists from the end of the last
				// block to the end of the file.
				stream_start = end;
				header_block = blocks[0]
					.Data;
			}
		}


		/// <summary>
		///    Reads format specific information at the end of the
		///    file.
		/// </summary>
		/// <param name="end">
		///    A <see cref="long" /> value containing the seek position
		///    at which the media data ends and the tags begin.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		protected override void ReadEnd(long end,
			ReadStyle propertiesStyle)
		{
		}


		/// <summary>
		///    Reads the audio properties from the file represented by
		///    the current instance.
		/// </summary>
		/// <param name="start">
		///    A <see cref="long" /> value containing the seek position
		///    at which the tags end and the media data begins.
		/// </param>
		/// <param name="end">
		///    A <see cref="long" /> value containing the seek position
		///    at which the media data ends and the tags begin.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <returns>
		///    A <see cref="TagLib.Properties" /> object describing the
		///    media properties of the file represented by the current
		///    instance.
		/// </returns>
		protected override Properties ReadProperties(long start,
			long end,
			ReadStyle propertiesStyle)
		{
			var header = new StreamHeader(header_block,
				end - stream_start);
			return new Properties(TimeSpan.Zero, header);
		}


		/// <summary>
		///    Reads all metadata blocks starting from the current
		///    instance, starting at a specified position.
		/// </summary>
		/// <param name="start">
		///    A <see cref="long" /> value reference specifying the
		///    position at which to start searching for the blocks. This
		///    will be updated to the position of the first block.
		/// </param>
		/// <param name="end">
		///    A <see cref="long" /> value reference updated to the
		///    position at which the last block ends.
		/// </param>
		/// <param name="mode">
		///    A <see cref="BlockMode" /> value indicating whether to
		///    white-list or black-list the contents of <paramref
		///    name="types" />.
		/// </param>
		/// <param name="types">
		///    A <see cref="T:BlockType[]" /> containing the types to look
		///    for or not look for as specified by <paramref name="mode"
		///    />.
		/// </param>
		/// <returns>
		///    A <see cref="T:System.Collections.Generic.IList`1" /> object containing the blocks
		///    read from the current instance.
		/// </returns>
		/// <exception cref="CorruptFileException">
		///    "<c>fLaC</c>" could not be found.
		/// </exception>
		private IList<Block> ReadBlocks(ref long start, out long end,
			BlockMode mode,
			params BlockType[] types)
		{
			var blocks = new List<Block>();

			var start_position = Find("fLaC", start);

			if (start_position < 0)
				throw new CorruptFileException(
					"FLAC stream not found at starting position.");

			end = start = start_position + 4;

			Seek(start);

			BlockHeader header;

			do
			{
				header = new BlockHeader(ReadBlock((int)
					BlockHeader.Size));

				var found = false;
				foreach (var type in types)
					if (header.BlockType == type)
					{
						found = true;
						break;
					}

				if (mode == BlockMode.Whitelist && found ||
				    mode == BlockMode.Blacklist && !found)
					blocks.Add(new Block(header,
						ReadBlock((int)
							header.BlockSize)));
				else
					Seek(header.BlockSize,
						SeekOrigin.Current);

				end += header.BlockSize + BlockHeader.Size;
			} while (!header.IsLastBlock);

			return blocks;
		}


		/// <summary>
		///    Indicates whether or not the block types passed into
		///    <see cref="ReadBlocks" /> are to be white-listed or
		///    black-listed.
		/// </summary>
		private enum BlockMode
		{
			/// <summary>
			///    All block types except those provided are to be
			///    returned.
			/// </summary>
			Blacklist,

			/// <summary>
			///    Only those block types provides should be
			///    returned.
			/// </summary>
			Whitelist
		}
	}
}
