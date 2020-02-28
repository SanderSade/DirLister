using System;

namespace Sander.DirLister.Core.TagLib.Asf
{
	/// <summary>
	///     This class extends <see cref="Object" /> to provide a
	///     representation of an unknown object which can be read from and
	///     written to disk.
	/// </summary>
	public class UnknownObject : Object
	{
		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="UnknownObject" />
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
		public UnknownObject(File file, long position)
			: base(file, position)
		{
			Data = file.ReadBlock((int)(OriginalSize - 24));
		}


		/// <summary>
		///     Gets and sets the data contained in the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="ByteVector" /> object containing the data
		///     contained in the current instance.
		/// </value>
		public ByteVector Data { get; set; }


		/// <summary>
		///     Renders the current instance as a raw ASF object.
		/// </summary>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered version of the current instance.
		/// </returns>
		public override ByteVector Render()
		{
			return Render(Data);
		}
	}
}
