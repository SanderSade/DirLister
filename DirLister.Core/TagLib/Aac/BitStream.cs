using System;
using System.Collections;
using System.Diagnostics;

namespace Sander.DirLister.Core.TagLib.Aac
{
	/// <summary>
	///     This class is used to help reading arbitary number of bits from
	///     a fixed array of bytes
	/// </summary>
	public class BitStream
	{
		private readonly BitArray bits;
		private int bitindex;


		/// <summary>
		///     Construct a new <see cref="BitStream" />.
		/// </summary>
		/// <param name="buffer">
		///     A <see cref="T:System.Byte[]" />, must be 7 bytes long.
		/// </param>
		public BitStream(byte[] buffer)
		{
			Debug.Assert(buffer.Length == 7, "buffer.Length == 7", "buffer size invalid");

			if (buffer.Length != 7)
			{
				throw new ArgumentException("Buffer size must be 7 bytes");
			}

			// Reverse bits
			bits = new BitArray(buffer.Length * 8);
			for (var i = 0; i < buffer.Length; i++)
			{
				for (var y = 0; y < 8; y++)
				{
					bits[i * 8 + y] = (buffer[i] & (1 << (7 - y))) > 0;
				}
			}

			bitindex = 0;
		}


		/// <summary>
		///     Reads an Int32 from the bitstream
		/// </summary>
		/// <param name="numberOfBits">
		///     A <see cref="int" /> value containing the number
		///     of bits to read from the bitstream
		/// </param>
		public int ReadInt32(int numberOfBits)
		{
			Debug.Assert(numberOfBits > 0, "numberOfBits < 1");
			Debug.Assert(numberOfBits <= 32, "numberOfBits <= 32");

			if (numberOfBits <= 0)
			{
				throw new ArgumentException("Number of bits to read must be >= 1");
			}

			if (numberOfBits > 32)
			{
				throw new ArgumentException("Number of bits to read must be <= 32");
			}

			var value = 0;
			var start = bitindex + numberOfBits - 1;
			for (var i = 0; i < numberOfBits; i++)
			{
				value += bits[start] ? 1 << i : 0;
				bitindex++;
				start--;
			}

			return value;
		}
	}
}
