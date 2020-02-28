using System;

namespace Sander.DirLister.Core.TagLib.Asf
{
	/// <summary>
	///     This class extends <see cref="Object" /> to provide a
	///     representation of an ASF Padding object which can be read from
	///     and written to disk.
	/// </summary>
	public class PaddingObject : Object
	{
		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="PaddingObject" />
		///     by reading the contents from a
		///     specified position in a specified file.
		/// </summary>
		/// <param name="file">
		///     A <see cref="Asf.File" /> object containing the file from
		///     which the contents of the new instance are to be read.
		/// </param>
		/// <param name="position">
		///     A <see cref="long" /> value specify at what position to
		///     read the object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///     <paramref name="position" /> is less than zero or greater
		///     than the size of the file.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///     The object read from disk does not have the correct GUID
		///     or smaller than the minimum size.
		/// </exception>
		public PaddingObject(File file, long position)
			: base(file, position)
		{
			if (!Guid.Equals(Asf.Guid.AsfPaddingObject))
			{
				throw new CorruptFileException(
					"Object GUID incorrect.");
			}

			if (OriginalSize < 24)
			{
				throw new CorruptFileException(
					"Object size too small.");
			}

			Size = OriginalSize;
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="PaddingObject" />
		///     of a specified size.
		/// </summary>
		/// <param name="size">
		///     A <see cref="uint" /> value specifying the number of
		///     bytes the new instance is to take up on disk.
		/// </param>
		public PaddingObject(uint size)
			: base(Asf.Guid.AsfPaddingObject)
		{
			Size = size;
		}


		/// <summary>
		///     Gets and sets the number of bytes the current instance
		///     will take up on disk.
		/// </summary>
		/// <value>
		///     A <see cref="ulong" /> value containing the size of the
		///     current instance on disk.
		/// </value>
		public ulong Size { get; set; }


		/// <summary>
		///     Renders the current instance as a raw ASF object.
		/// </summary>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered version of the current instance.
		/// </returns>
		public override ByteVector Render()
		{
			return Render(new ByteVector((int)(Size - 24)));
		}
	}
}
