using System;

namespace Sander.DirLister.Core.TagLib.Asf
{
	/// <summary>
	///     This abstract class provides a basic representation of an ASF
	///     object which can be read from and written to disk.
	/// </summary>
	public abstract class Object
	{
		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="Object" />
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
		protected Object(File file, long position)
		{
			if (file == null)
			{
				throw new ArgumentNullException(nameof(file));
			}

			if (position < 0 ||
			    position > file.Length - 24)
			{
				throw new ArgumentOutOfRangeException(
					nameof(position));
			}

			file.Seek(position);
			Guid = file.ReadGuid();
			OriginalSize = file.ReadQWord();
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="Object" />
		///     with a specified GUID.
		/// </summary>
		/// <param name="guid">
		///     A <see cref="System.Guid" /> value containing the GUID to
		///     use for the new instance.
		/// </param>
		protected Object(System.Guid guid)
		{
			Guid = guid;
		}


		/// <summary>
		///     Gets the GUID for the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="System.Guid" /> object containing the GUID
		///     of the current instance.
		/// </value>
		public System.Guid Guid { get; }

		/// <summary>
		///     Gets the original size of the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="ulong" /> value containing the size of the
		///     current instance as it originally appeared on disk.
		/// </value>
		public ulong OriginalSize { get; }


		/// <summary>
		///     Renders the current instance as a raw ASF object.
		/// </summary>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered version of the current instance.
		/// </returns>
		/// <seealso cref="Render(ByteVector)" />
		public abstract ByteVector Render();


		/// <summary>
		///     Renders the current instance as a raw ASF object
		///     containing specified data.
		/// </summary>
		/// <param name="data">
		///     A <see cref="ByteVector" /> object containing the data to
		///     contained in the rendered version of the current
		///     instance.
		/// </param>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered version of the current instance.
		/// </returns>
		/// <remarks>
		///     Child classes implementing <see cref="Render()" /> should
		///     render their contents and then send the data through this
		///     method to produce the final output.
		/// </remarks>
		protected ByteVector Render(ByteVector data)
		{
			var length = (ulong)
				(((data?.Count) ?? 0) + 24);

			ByteVector v = Guid.ToByteArray();
			v.Add(RenderQWord(length));
			v.Add(data);
			return v;
		}


		/// <summary>
		///     Renders a Unicode (wide) string.
		/// </summary>
		/// <param name="value">
		///     A <see cref="string" /> object containing the text to
		///     render.
		/// </param>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered value.
		/// </returns>
		public static ByteVector RenderUnicode(string value)
		{
			var v = ByteVector.FromString(value,
				StringType.UTF16LE);

			v.Add(RenderWord(0));
			return v;
		}


		/// <summary>
		///     Renders a 4-byte DWORD.
		/// </summary>
		/// <param name="value">
		///     A <see cref="uint" /> value containing the DWORD to
		///     render.
		/// </param>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered value.
		/// </returns>
		public static ByteVector RenderDWord(uint value)
		{
			return ByteVector.FromUInt(value, false);
		}


		/// <summary>
		///     Renders a 8-byte QWORD.
		/// </summary>
		/// <param name="value">
		///     A <see cref="ulong" /> value containing the QWORD to
		///     render.
		/// </param>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered value.
		/// </returns>
		public static ByteVector RenderQWord(ulong value)
		{
			return ByteVector.FromULong(value, false);
		}


		/// <summary>
		///     Renders a 2-byte WORD.
		/// </summary>
		/// <param name="value">
		///     A <see cref="ushort" /> value containing the WORD to
		///     render.
		/// </param>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered value.
		/// </returns>
		public static ByteVector RenderWord(ushort value)
		{
			return ByteVector.FromUShort(value, false);
		}
	}
}
