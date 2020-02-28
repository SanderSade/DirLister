using System;
using System.Diagnostics.CodeAnalysis;

namespace Sander.DirLister.Core.TagLib.Matroska
{
	/// <summary>
	///     Read a Matroska EBML element from a file, but also provides basic modifications to an
	///     EBML element directly on the file (write). This can also represent an abstract EBML
	///     on the file (placeholder).
	/// </summary>
	/// <remarks>
	///     This was intitialy called <see cref="EBMLelement" />, but this was in fact a file-reader.
	///     The name <see cref="EBMLelement" /> correspond more to the class which has been created to
	///     represent an EBML structure (regardless of file-issues) to support the EBML writing to file.
	/// </remarks>
	public class EBMLreader
	{
		private uint ebml_id;
		private File file;
		private ulong offset;


		/// <summary>
		///     Constructs a root <see cref="EBMLreader" /> instance, by reading from
		///     the provided file position.
		/// </summary>
		/// <param name="_file"><see cref="File" /> File instance to read from.</param>
		/// <param name="position">Position in the file to start reading from.</param>
		public EBMLreader(File _file, ulong position)
		{
			// Keep a reference to the file
			file = _file;
			Parent = null;

			// Initialize attributes
			offset = position;
			DataOffset = position;
			ebml_id = 0;
			DataSize = 0;

			// Actually read the EBML on the file
			Read(true);
		}


		/// <summary>
		///     Constructs a child <see cref="EBMLreader" /> reading the data from the
		///     EBML parent at the provided file position.
		/// </summary>
		/// <param name="parent">The <see cref="EBMLreader" /> that contains the instance to be created.</param>
		/// <param name="position">Position in the file to start reading from.</param>
		public EBMLreader(EBMLreader parent, ulong position)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("file");
			}

			// Keep a reference to the file
			file = parent.file;
			Parent = parent;

			// Initialize attributes
			offset = position;
			DataOffset = position;
			ebml_id = 0;
			DataSize = 0;

			// Actually read the EBML on the file
			Read(true);
		}


		/// <summary>
		///     Create a new abstract <see cref="EBMLreader" /> with arbitrary attributes,
		///     without reading its information on the file.
		/// </summary>
		/// <param name="parent">The <see cref="EBMLreader" /> that contains the instance to be described.</param>
		/// <param name="position">Position in the file.</param>
		/// <param name="ebmlid">EBML ID of the element</param>
		/// <param name="size">Total size of the EBML, in bytes</param>
		public EBMLreader(EBMLreader parent, ulong position, MatroskaID ebmlid, ulong size = 0)
		{
			// Keep a reference to the file
			if (parent != null)
			{
				file = parent.file;
			}

			Parent = parent;

			// Initialize attributes
			offset = position;
			DataOffset = offset;
			ebml_id = (uint)ebmlid;
			DataSize = size;
		}


		/// <summary>
		///     EBML Element Identifier.
		/// </summary>
		public MatroskaID ID => (MatroskaID)ebml_id;

		/// <summary>
		///     EBML Parent instance.
		/// </summary>
		public EBMLreader Parent { get; }

		/// <summary>
		///     EBML Element size in bytes.
		/// </summary>
		public ulong Size
		{
			set => DataSize = value - (DataOffset - offset);
			get => DataOffset - offset + DataSize;
		}

		/// <summary>
		///     EBML Element data size in bytes.
		/// </summary>
		public ulong DataSize { set; get; }

		/// <summary>
		///     EBML Element data offset position in file in bytes.
		/// </summary>
		public ulong DataOffset { get; private set; }

		/// <summary>
		///     EBML Element offset position in file in bytes.
		/// </summary>
		public ulong Offset
		{
			set
			{
				DataOffset = (ulong)((long)DataOffset + ((long)value - (long)offset));
				offset = value;
			}
			get => offset;
		}

		/// <summary>
		///     Defines that the EBML element is not read-out from file,
		///     but is an abstract representation of an element on the disk.
		/// </summary>
		public bool Abstract => offset == DataOffset;


		/// <summary>
		///     Read EBML header and data-size if it is an abstract one.
		///     It then becomes a non abstract EBML.
		/// </summary>
		/// <param name="throwException">Throw exception on invalid EBML read if true (Default: false).</param>
		/// <returns>True if successful.</returns>
		[SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
		public bool Read(bool throwException = false)
		{
			if (!Abstract)
			{
				return true;
			}

			if (file == null)
			{
				throw new ArgumentNullException("file");
			}

			try
			{
				var ex = new InvalidOperationException("Invalid EBML format Read");

				if (offset >= (ulong)file.Length - 1)
				{
					throw ex;
				}

				// Prepare for Consitency check
				var ebml_id_check = ebml_id;
				var ebml_size_check = Size;

				file.Seek((long)offset);

				// Get the header byte
				var vector = file.ReadBlock(1);
				var header_byte = vector[0];
				// Define a mask
				byte mask = 0x80, id_length = 1;
				// Figure out the size in bytes
				while (id_length <= 4 && (header_byte & mask) == 0)
				{
					id_length++;
					mask >>= 1;
				}

				if (id_length > 4)
				{
					throw ex;
				}

				// Now read the rest of the EBML ID
				if (id_length > 1)
				{
					vector.Add(file.ReadBlock(id_length - 1));
				}

				ebml_id = vector.ToUInt();

				vector.Clear();

				// Get the size length
				vector = file.ReadBlock(1);
				header_byte = vector[0];
				mask = 0x80;
				byte size_length = 1;

				// Iterate through various possibilities
				while (size_length <= 8 && (header_byte & mask) == 0)
				{
					size_length++;
					mask >>= 1;
				}

				if (size_length > 8)
				{
					size_length = 1; // Special: Empty element (all zero state)
				}
				else
				{
					vector[0] &= (byte)(mask - 1); // Clear the marker bit
				}

				// Now read the rest of the EBML element size
				if (size_length > 1)
				{
					vector.Add(file.ReadBlock(size_length - 1));
				}

				DataSize = vector.ToULong();

				// Special: Auto-size (0xFF byte)
				if (size_length == 1 && DataSize == 0x7F)
				{
					// Resolve auto-size to fill in to its containing element
					var bound = Parent == null ? (ulong)file.Length : Parent.Offset + Parent.Size;
					DataSize = bound - offset - (ulong)(id_length + size_length);
				}

				DataOffset = offset + id_length + size_length;

				// Consistency check: Detect descrepencies between read data and abstract data
				if (ebml_id_check != 0 && ebml_id_check != ebml_id)
				{
					throw ex;
				}

				if (ebml_size_check != 0 && ebml_size_check != Size)
				{
					throw ex;
				}

				return true;
			}
			catch (Exception ex)
			{
				if (throwException)
				{
					throw ex;
				}

				return false;
			}
		}


		/// <summary>
		///     Reads a vector of bytes (raw data) from EBML Element's data section.
		/// </summary>
		/// <returns>a <see cref="ByteVector" /> containing the parsed value.</returns>
		public ByteVector ReadBytes()
		{
			if (file == null)
			{
				return null;
			}

			file.Seek((long)DataOffset);

			var vector = file.ReadBlock((int)DataSize);

			return vector;
		}


		/// <summary>
		///     Reads a string from EBML Element's data section (UTF-8).
		/// </summary>
		/// <returns>a string object containing the parsed value.</returns>
		public string ReadString()
		{
			if (file == null)
			{
				return null;
			}

			var vector = ReadBytes();
			var ebml = new EBMLelement((MatroskaID)ebml_id, vector);
			return ebml.GetString();
		}


		/// <summary>
		///     Reads a boolean from EBML Element's data section.
		/// </summary>
		/// <returns>a bool containing the parsed value.</returns>
		public bool ReadBool()
		{
			if (file == null || DataSize == 0)
			{
				return false;
			}

			var vector = ReadBytes();
			var ebml = new EBMLelement((MatroskaID)ebml_id, vector);
			return ebml.GetBool();
		}


		/// <summary>
		///     Reads a double from EBML Element's data section.
		/// </summary>
		/// <returns>a double containing the parsed value.</returns>
		public double ReadDouble()
		{
			if (file == null || DataSize == 0)
			{
				return 0;
			}

			var vector = ReadBytes();
			var ebml = new EBMLelement((MatroskaID)ebml_id, vector);
			return ebml.GetDouble();
		}


		/// <summary>
		///     Reads an unsigned integer (any size from 1 to 8 bytes) from EBML Element's data section.
		/// </summary>
		/// <returns>a ulong containing the parsed value.</returns>
		public ulong ReadULong()
		{
			if (file == null || DataSize == 0)
			{
				return 0;
			}

			var vector = ReadBytes();
			var ebml = new EBMLelement((MatroskaID)ebml_id, vector);
			return ebml.GetULong();
		}


		/// <summary>
		///     Write the <see cref="DataSize" /> to the EBML file.
		///     Resize the data-size length to 8 bytes.
		///     This will *not* insert extra bytes, but overwrite next contiguous bytes.
		///     It will claim the size added on the value of the data-size.
		/// </summary>
		/// <returns>Offset created in Writing the new data-size</returns>
		public long WriteDataSize()
		{
			var value = DataSize;
			const ulong newsize_length = 8;

			// Figure out the ID size in bytes
			ulong mask = 0xFF000000, id_length = 4;
			while (id_length > 0 && (ebml_id & mask) == 0)
			{
				id_length--;
				mask >>= 8;
			}

			if (id_length == 0)
			{
				throw new CorruptFileException("invalid EBML ID (zero)");
			}

			// Figure out the Data size length in bytes
			var size_length = DataOffset - offset - id_length;
			if (size_length > 8)
			{
				throw new CorruptFileException("invalid EBML element size");
			}

			// Construct the data-size field
			var vector = new ByteVector((int)newsize_length);
			mask = value;
			for (var i = (int)newsize_length - 1; i >= 0; i--)
			{
				vector[i] = (byte)(mask & 0xFF);
				mask >>= 8;
			}

			// Set the marker bit
			vector[0] |= 0x100 >> (int)newsize_length;

			// Update fields
			var woffset = newsize_length - size_length;
			DataOffset = DataOffset + woffset;
			DataSize = value - woffset;

			return (long)woffset;
		}


		/// <summary>
		///     Change an EBML element to a Abstract Void element, but do not write to the file.
		/// </summary>
		/// <remarks>
		///     To do a real conversion to Void EBML element on the file, use <see cref="WriteVoid()" />.
		/// </remarks>
		public void SetVoid()
		{
			var size = Size;

			// Update this object
			ebml_id = (uint)MatroskaID.Void;
			DataOffset = offset; // This will make it abstract
			DataSize = size; // Keep the size unchanged
		}


		/// <summary>
		///     Change an EBML element to a Void element directly on the file.
		/// </summary>
		public void WriteVoid()
		{
			if (Size < 2)
			{
				throw new ArgumentOutOfRangeException("WriteVoid Size < 2");
			}

			if (file == null)
			{
				throw new ArgumentNullException("WriteVoid file");
			}

			if (offset + Size > (ulong)file.Length)
			{
				throw new ArgumentOutOfRangeException("WriteVoid tries to write out of the file");
			}

			ByteVector vector;
			int datasize;

			if (Size < 100)
			{
				vector = new ByteVector(2);
				datasize = (int)Size - 2;
				vector[0] = (byte)MatroskaID.Void; // size = 1
				vector[1] = (byte)(0x80 | datasize); // Marker + data-size
			}
			else
			{
				vector = new ByteVector(9);
				datasize = (int)Size - 9;
				vector[0] = (byte)MatroskaID.Void; // size = 1
				vector[1] = 0x01; // set marker

				// Set data size
				var mask = datasize;
				for (var i = 8; i > 1; i--)
				{
					vector[i] = (byte)(mask & 0xFF);
					mask >>= 8;
				}
			}

			// Update this object
			ebml_id = (uint)MatroskaID.Void;
			DataOffset = Offset + (ulong)vector.Count;
			DataSize = (ulong)datasize;
		}


		/// <summary>
		///     Remove the EBML element from the file
		/// </summary>
		/// <returns>Size difference compare to previous EBML size</returns>
		public long Remove()
		{
			var ret = -(long)Size;

			// Invalidate this object
			ebml_id = 0;
			DataOffset = offset;
			DataSize = 0;
			file = null;

			return ret;
		}
	}
}
