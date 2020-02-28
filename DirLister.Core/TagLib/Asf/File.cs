using System;
using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Asf
{
	/// <summary>
	///     This class extends <see cref="TagLib.File" /> to provide tagging
	///     and properties support for Microsoft's ASF files.
	/// </summary>
	[SupportedMimeType("taglib/wma", "wma")]
	[SupportedMimeType("taglib/wmv", "wmv")]
	[SupportedMimeType("taglib/asf", "asf")]
	public sealed class File : TagLib.File
	{
		/// <summary>
		///     Contains the file's properties.
		/// </summary>
		private Properties properties;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified path in the local file
		///     system and specified read style.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object containing the path of the
		///     file to use in the new instance.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path, ReadStyle propertiesStyle)
			: base(path)
		{
			Read(propertiesStyle);
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified path in the local file
		///     system with an average read style.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object containing the path of the
		///     file to use in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path) : this(path, ReadStyle.Average)
		{
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified file abstraction and
		///     specified read style.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading from and writing to the file.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="abstraction" /> is <see langword="null" />.
		/// </exception>
		public File(IFileAbstraction abstraction,
			ReadStyle propertiesStyle) : base(abstraction)
		{
			Read(propertiesStyle);
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified file abstraction with an
		///     average read style.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading from and writing to the file.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="abstraction" /> is <see langword="null" />.
		/// </exception>
		public File(IFileAbstraction abstraction)
			: this(abstraction, ReadStyle.Average)
		{
		}


		/// <summary>
		///     Gets the media properties of the file represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="TagLib.Properties" /> object containing the
		///     media properties of the file represented by the current
		///     instance.
		/// </value>
		public override Properties Properties => properties;


		/// <summary>
		///     Reads the contents of the current instance.
		/// </summary>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		private void Read(ReadStyle propertiesStyle)
		{
			Mode = AccessMode.Read;
			try
			{
				var header = new HeaderObject(this, 0);

				InvariantStartPosition = (long)header.OriginalSize;
				InvariantEndPosition = Length;

				if ((propertiesStyle & ReadStyle.Average) != 0)
				{
					properties = header.Properties;
				}
			}
			finally
			{
				Mode = AccessMode.Closed;
			}
		}


		/// <summary>
		///     Reads a 2-byte WORD from the current instance.
		/// </summary>
		/// <returns>
		///     A <see cref="ushort" /> value containing the WORD read
		///     from the current instance.
		/// </returns>
		public ushort ReadWord()
		{
			return ReadBlock(2)
				.ToUShort(false);
		}


		/// <summary>
		///     Reads a 4-byte DWORD from the current instance.
		/// </summary>
		/// <returns>
		///     A <see cref="uint" /> value containing the DWORD read
		///     from the current instance.
		/// </returns>
		public uint ReadDWord()
		{
			return ReadBlock(4)
				.ToUInt(false);
		}


		/// <summary>
		///     Reads a 8-byte QWORD from the current instance.
		/// </summary>
		/// <returns>
		///     A <see cref="ulong" /> value containing the QWORD read
		///     from the current instance.
		/// </returns>
		public ulong ReadQWord()
		{
			return ReadBlock(8)
				.ToULong(false);
		}


		/// <summary>
		///     Reads a 16-byte GUID from the current instance.
		/// </summary>
		/// <returns>
		///     A <see cref="System.Guid" /> value containing the GUID
		///     read from the current instance.
		/// </returns>
		public System.Guid ReadGuid()
		{
			return new System.Guid(ReadBlock(16)
				.Data);
		}


		/// <summary>
		///     Reads a Unicode (UTF-16LE) string of specified length
		///     from the current instance.
		/// </summary>
		/// <param name="length">
		///     A <see cref="int" /> value specifying the number of bytes
		///     to read. This should always be an even number.
		/// </param>
		/// <returns>
		///     A <see cref="string" /> object containing the Unicode
		///     string read from the current instance.
		/// </returns>
		public string ReadUnicode(int length)
		{
			var data = ReadBlock(length);
			var output = data.ToString(StringType.UTF16LE);
			var i = output.IndexOf('\0');
			return i >= 0 ? output.Substring(0, i) : output;
		}


		/// <summary>
		///     Reads a collection of objects from the current instance.
		/// </summary>
		/// <param name="count">
		///     A <see cref="uint" /> value specifying the number of
		///     objects to read.
		/// </param>
		/// <param name="position">
		///     A <see cref="long" /> value specifying the seek position
		///     at which to start reading.
		/// </param>
		/// <returns>
		///     A new <see cref="T:System.Collections.Generic.IEnumerable`1" /> object enumerating
		///     through the <see cref="Object" /> objects read from the
		///     current instance.
		/// </returns>
		public IEnumerable<Object> ReadObjects(uint count,
			long position)
		{
			for (var i = 0; i < (int)count; i++)
			{
				var obj = ReadObject(position);
				position += (long)obj.OriginalSize;
				yield return obj;
			}
		}


		/// <summary>
		///     Reads a <see cref="Object" /> from the current instance.
		/// </summary>
		/// <param name="position">
		///     A <see cref="long" /> value specifying the seek position
		///     at which to start reading.
		/// </param>
		/// <returns>
		///     A new <see cref="Object" /> object of appropriate type as
		///     read from the current instance.
		/// </returns>
		public Object ReadObject(long position)
		{
			Seek(position);
			var id = ReadGuid();

			if (id.Equals(Guid.AsfFilePropertiesObject))
			{
				return new FilePropertiesObject(this,
					position);
			}

			if (id.Equals(Guid.AsfStreamPropertiesObject))
			{
				return new StreamPropertiesObject(this,
					position);
			}

			if (id.Equals(Guid.AsfContentDescriptionObject))
			{
				return new ContentDescriptionObject(this,
					position);
			}

			if (id.Equals(
				Guid.AsfExtendedContentDescriptionObject))
			{
				return new ExtendedContentDescriptionObject(
					this, position);
			}

			if (id.Equals(Guid.AsfPaddingObject))
			{
				return new PaddingObject(this, position);
			}

			if (id.Equals(Guid.AsfHeaderExtensionObject))
			{
				return new HeaderExtensionObject(this,
					position);
			}

			if (id.Equals(Guid.AsfMetadataLibraryObject))
			{
				return new MetadataLibraryObject(this,
					position);
			}

			return new UnknownObject(this, position);
		}
	}
}
