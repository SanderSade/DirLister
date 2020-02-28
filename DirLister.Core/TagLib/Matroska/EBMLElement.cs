using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sander.DirLister.Core.TagLib.Matroska
{
	/// <summary>
	///     Represent a generic EBML Element and its content.
	/// </summary>
	public class EBMLelement
	{
		/// <summary>
		///     Get or set the element embedded in the EBML
		/// </summary>
		public List<EBMLelement> Children;

		/// <summary>
		///     Get or set the data represented by the EBML
		/// </summary>
		public ByteVector Data;

		/// <summary>
		///     EBML Element Identifier.
		/// </summary>
		public MatroskaID ID = 0;

		/// <summary>
		///     Get or set whether the EBML should have a size of one byte more
		///     than the optimal size.
		/// </summary>
		public bool IncSize;


		/// <summary>
		///     Constructs an empty <see cref="EBMLelement" />.
		/// </summary>
		public EBMLelement()
		{
		}


		/// <summary>
		///     Construct a <see cref="EBMLelement" /> to contain children elements.
		/// </summary>
		/// <param name="ebmlid">EBML ID of the element to be created.</param>
		public EBMLelement(MatroskaID ebmlid)
		{
			ID = ebmlid;
			Children = new List<EBMLelement>();
		}


		/// <summary>
		///     Construct a <see cref="EBMLelement" /> to contain data.
		/// </summary>
		/// <param name="ebmlid">EBML ID of the element to be created.</param>
		/// <param name="data">EBML data of the element to be created.</param>
		public EBMLelement(MatroskaID ebmlid, ByteVector data)
		{
			ID = ebmlid;
			Data = data;
		}


		/// <summary>
		///     Construct <see cref="EBMLelement" /> to contain data.
		/// </summary>
		/// <param name="ebmlid">EBML ID of the element to be created.</param>
		/// <param name="value">EBML data as an <see cref="ulong" /> value.</param>
		public EBMLelement(MatroskaID ebmlid, ulong value)
		{
			ID = ebmlid;
			SetData(value);
		}


		/// <summary>
		///     EBML Element size in bytes.
		/// </summary>
		public long Size
		{
			get
			{
				var size_length = DataSize;
				return IDSize + EBMLByteSize((ulong)size_length) + (IncSize ? 1 : 0) + size_length;
			}
		}

		/// <summary>
		///     Get the size of the EBML ID, in bytes
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
		public long IDSize
		{
			get
			{
				var ebml_id = (uint)ID;

				// Figure out the ID size in bytes
				long mask = 0xFF000000, id_length = 4;
				while (id_length > 0 && (ebml_id & mask) == 0)
				{
					id_length--;
					mask >>= 8;
				}

				if (id_length == 0)
				{
					throw new CorruptFileException("invalid EBML ID (zero)");
				}

				return id_length;
			}
		}

		/// <summary>
		///     Get the size of the EBML data-size, in bytes
		/// </summary>
		public long DataSizeSize => EBMLByteSize((ulong)DataSize) + (IncSize ? 1 : 0);

		/// <summary>
		///     EBML Element data/content size in bytes.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
		public long DataSize
		{
			get
			{
				long ret = 0;

				if (Data != null)
				{
					// Get Data size
					ret = Data.Count;

					if (Children != null)
					{
						throw new UnsupportedFormatException("EBML element cannot contain both Data and Children");
					}
				}
				else
				{
					// Get the content size
					foreach (var child in Children)
					{
						ret += child.Size;
					}
				}

				return ret;
			}
		}

		/// <summary>
		///     Get the EBML ID and data-size as a vector of bytes.
		/// </summary>
		public ByteVector Header
		{
			get
			{
				// Retrieve sizes
				var id_length = IDSize;
				var size_length = DataSizeSize;

				// Create vector
				var vector = new ByteVector((int)(id_length + size_length));

				// Construct the ID field
				var ebml_id = (uint)ID;
				var mask = ebml_id;
				for (var i = (int)id_length - 1; i >= 0; i--)
				{
					vector[i] = (byte)(mask & 0xFF);
					mask >>= 8;
				}

				// Construct the data-size field
				var lmask = (ulong)DataSize;
				for (var i = (int)(id_length + size_length - 1); i >= id_length; i--)
				{
					vector[i] = (byte)(lmask & 0xFF);
					lmask >>= 8;
				}

				// Set the marker bit of the Data-size
				vector[(int)id_length] |= (byte)(0x100 >> (int)size_length);

				return vector;
			}
		}


		/// <summary>
		///     Get the byte-size required to encode an EBML value with the leading 1.
		/// </summary>
		/// <param name="value">Encoded value</param>
		/// <returns>size in bytes</returns>
		public static long EBMLByteSize(ulong value)
		{
			// Figure out the required data-size size in bytes
			long size_length;
			if (value == 0x7F)
			{
				// Special case: Avoid element-size reserved word of 0xFF (all ones)
				size_length = 2;
			}
			else
			{
				size_length = 8;
				var mask = (ulong)0x7F << (7 * 7);
				while (size_length > 1 && (value & mask) == 0)
				{
					size_length--;
					mask >>= 7;
				}
			}

			return size_length;
		}


		/// <summary>
		///     Try to increase the size of the EBML by 1 byte.
		/// </summary>
		/// <returns>True if successfully increased size, false if failed.</returns>
		public bool IncrementSize()
		{
			// Try to extend current DataSizeSize
			if (!IncSize && DataSizeSize < 8)
			{
				return IncSize = true;
			}

			// Try to extend one of the children
			if (Children != null)
			{
				foreach (var child in Children)
				{
					if (child.IncrementSize())
					{
						return true;
					}
				}
			}

			// Failed
			return false;
		}


		/// <summary>
		///     Get a string from EBML Element's data section (UTF-8).
		///     Handle null-termination.
		/// </summary>
		/// <returns>a string object containing the parsed value.</returns>
		public string GetString()
		{
			if (Data == null)
			{
				return null;
			}

			var idx = Data.IndexOf(0x00); // Detected Null termination
			if (idx >= 0)
			{
				return Data.ToString(StringType.UTF8, 0, idx);
			}

			return Data.ToString(StringType.UTF8);
		}


		/// <summary>
		///     Get a boolean from EBML Element's data section.
		/// </summary>
		/// <returns>a bool containing the parsed value.</returns>
		public bool GetBool()
		{
			if (Data == null)
			{
				return false;
			}

			return Data.ToUInt() > 0;
		}


		/// <summary>
		///     Get a double from EBML Element's data section.
		/// </summary>
		/// <returns>a double containing the parsed value.</returns>
		public double GetDouble()
		{
			if (Data == null)
			{
				return 0;
			}

			var result = 0.0;

			if (Data.Count == 4)
			{
				result = Data.ToFloat();
			}
			else if (Data.Count == 8)
			{
				result = Data.ToDouble();
			}
			else
			{
				throw new UnsupportedFormatException("Can not read a Double with sizes differing from 4 or 8");
			}

			return result;
		}


		/// <summary>
		///     Get an unsigned integer (any size from 1 to 8 bytes) from EBML Element's data section.
		/// </summary>
		/// <returns>a ulong containing the parsed value.</returns>
		public ulong GetULong()
		{
			if (Data == null)
			{
				return 0;
			}

			return Data.ToULong();
		}


		/// <summary>
		///     Get a bytes vector from EBML Element's data section.
		/// </summary>
		/// <returns>a <see cref="ByteVector" /> containing the parsed value.</returns>
		public ByteVector GetBytes()
		{
			return Data;
		}


		/// <summary>
		///     Set data content as <see cref="string" /> to the EBML file
		/// </summary>
		/// <param name="data">data as <see cref="string" /></param>
		public void SetData(string data)
		{
			Data = data;
		}


		/// <summary>
		///     Set data content as <see cref="ulong" /> to the EBML file
		/// </summary>
		/// <param name="data">unsigned long number to write</param>
		public void SetData(ulong data)
		{
			const ulong mask = 0xffffffff00000000;
			var isLong = (data & mask) != 0;

			var vector = new ByteVector(isLong ? 8 : 4);
			for (var i = vector.Count - 1; i >= 0; i--)
			{
				vector[i] = (byte)(data & 0xff);
				data >>= 8;
			}

			Data = vector;
		}
	}
}
