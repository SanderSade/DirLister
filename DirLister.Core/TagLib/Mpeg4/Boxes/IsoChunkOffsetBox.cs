using System;

namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///    This class extends <see cref="FullBox" /> to provide an
	///    implementation of a ISO/IEC 14496-12 ChunkOffsetBox.
	/// </summary>
	/// <remarks>
	///    <see cref="IsoChunkOffsetBox" /> and <see
	///    cref="IsoChunkLargeOffsetBox" /> contain offsets of media data
	///    within the file. As such, if the file changes by even one byte,
	///    these values are devalidatated and the box will have to be
	///    overwritten to maintain playability.
	/// </remarks>
	public class IsoChunkOffsetBox : FullBox
	{


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="IsoChunkOffsetBox" /> with a provided header and
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
		public IsoChunkOffsetBox(BoxHeader header, TagLib.File file,
			IsoHandlerBox handler)
			: base(header, file, handler)
		{
			var box_data = file.ReadBlock(DataSize);

			Offsets = new uint [(int)
				box_data.Mid(0, 4)
				        .ToUInt()];

			for (var i = 0; i < Offsets.Length; i++)
				Offsets[i] = box_data.Mid(4 + i * 4,
					                     4)
				                     .ToUInt();
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
			get
			{
				var output = ByteVector.FromUInt((uint)
					Offsets.Length);
				for (var i = 0; i < Offsets.Length; i++)
					output.Add(ByteVector.FromUInt(
						Offsets[i]));

				return output;
			}
		}

		/// <summary>
		///    Gets the offset table contained in the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="T:uint[]" /> containing the offset table
		///    contained in the current instance.
		/// </value>
		public uint[] Offsets { get; }


		/// <summary>
		///    Overwrites the existing box in the file after updating
		///    the table for a size change.
		/// </summary>
		/// <param name="file">
		///    A <see cref="File" /> object containing the file to which
		///    the current instance belongs and wo which modifications
		///    must be applied.
		/// </param>
		/// <param name="sizeDifference">
		///    A <see cref="long" /> value containing the size
		///    change that occurred in the file.
		/// </param>
		/// <param name="after">
		///    A <see cref="long" /> value containing the position in
		///    the file after which offsets will be invalidated. If an
		///    offset is before this point, it won't be updated.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <see param="file" /> is <see langword="null" />.
		/// </exception>
		public void Overwrite(File file, long sizeDifference,
			long after)
		{
			if (file == null)
				throw new ArgumentNullException("file");
		}


		/// <summary>
		///    Renders the current instance after updating the table for
		///    a size change.
		/// </summary>
		/// <param name="sizeDifference">
		///    A <see cref="long" /> value containing the size
		///    change that occurred in the file.
		/// </param>
		/// <param name="after">
		///    A <see cref="long" /> value containing the position in
		///    the file after which offsets will be invalidated. If an
		///    offset is before this point, it won't be updated.
		/// </param>
		/// <returns>
		///    A <see cref="ByteVector" /> object containing the
		///    rendered version of the file.
		/// </returns>
		public ByteVector Render(long sizeDifference, long after)
		{
			for (var i = 0; i < Offsets.Length; i++)
				if (Offsets[i] >= (uint)after)
					Offsets[i] = (uint)
						(Offsets[i] + sizeDifference);

			return Render();
		}
	}
}
