﻿using System;
using System.Globalization;
using System.IO;

namespace Sander.DirLister.Core.TagLib.Mpc
{
	/// <summary>
	///     This struct implements <see cref="IAudioCodec" /> to provide
	///     support for reading MusePack audio properties.
	/// </summary>
	public struct StreamHeader : IAudioCodec
	{
		private static readonly ushort[] sftable = { 44100, 48000, 37800, 32000 };

		/// <summary>
		///     Contains the number of bytes in the stream.
		/// </summary>
		private readonly long stream_length;

		/// <summary>
		///     Contains additional header information.
		/// </summary>
		private uint header_data;

		/// <summary>
		///     Contains the number of frames in the stream.
		/// </summary>
		private uint frames;

		/// <summary>
		///     Contains the count of frames in the stream.
		/// </summary>
		private ulong framecount;

		/// <summary>
		///     The size of a MusePack SV7 header.
		/// </summary>
		public const uint SizeSV7 = 56;

		/// <summary>
		///     The identifier used to recognize a Musepack SV7 file.
		/// </summary>
		/// <value>
		///     "MP+"
		/// </value>
		public static readonly ReadOnlyByteVector FileIdentifierSv7 = "MP+";

		/// <summary>
		///     The identifier used to recognize a Musepack SV8 file.
		/// </summary>
		/// <value>
		///     "MPCK"
		/// </value>
		public static readonly ReadOnlyByteVector FileIdentifierSv8 = "MPCK";


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="StreamHeader" />
		///     for a specified header block and
		///     stream length.
		/// </summary>
		/// <param name="file">
		///     A <see cref="File" /> object containing the stream
		///     header data.
		/// </param>
		/// <param name="streamLength">
		///     A <see cref="long" /> value containing the length of the
		///     MusePAck stream in bytes.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///     <paramref name="file" /> does not begin with
		///     <see
		///         cref="FileIdentifierSv7" />
		///     or with
		///     <see
		///         cref="FileIdentifierSv8" />
		///     or is less than
		///     <see cref="P:File.Size" /> bytes long.
		/// </exception>
		public StreamHeader(File file, long streamLength)
		{
			if (file == null)
			{
				throw new ArgumentNullException(nameof(file));
			}

			// Assign default values, to be able to call methods
			// in the constructor
			stream_length = streamLength;
			Version = 7;
			header_data = 0;
			frames = 0;
			AudioSampleRate = 0;
			AudioChannels = 2;
			framecount = 0;

			file.Seek(0);
			var magic = file.ReadBlock(4);
			if (magic.StartsWith(FileIdentifierSv7))
				// SV7 Format has a fixed Header size
			{
				ReadSv7Properties(magic + file.ReadBlock((int)SizeSV7 - 4));
			}
			else if (magic.StartsWith(FileIdentifierSv8))
				// for SV8 the properties need to be read from
				// packet information inside the file 
			{
				ReadSv8Properties(file);
			}
			else
			{
				throw new CorruptFileException(
					"Data does not begin with identifier.");
			}
		}


		private void ReadSv7Properties(ByteVector data)
		{
			if (data.Count < SizeSV7)
			{
				throw new CorruptFileException(
					"Insufficient data in stream header");
			}

			Version = data[3] & 15;
			AudioChannels = 2;

			if (Version == 7)
			{
				frames = data.Mid(4, 4)
					.ToUInt(false);

				var flags = data.Mid(8, 4)
					.ToUInt(false);

				AudioSampleRate = sftable[(int)(((flags >> 17) &
				                                 1) * 2 + ((flags >> 16) & 1))];

				header_data = 0;
			}
			else
			{
				header_data = data.Mid(0, 4)
					.ToUInt(false);

				Version = (int)((header_data >> 11) & 0x03ff);
				AudioSampleRate = 44100;
				frames = data.Mid(4,
						Version >= 5 ? 4 : 2)
					.ToUInt(false);
			}
		}


		private void ReadSv8Properties(File file)
		{
			var foundSH = false;

			while (!foundSH)
			{
				var packetType = file.ReadBlock(2);

				uint packetSizeLength = 0;
				var eof = false;

				var packetSize = ReadSize(file, ref packetSizeLength, ref eof);
				if (eof)
				{
					break;
				}

				var payloadSize = packetSize - 2 - packetSizeLength;
				var data = file.ReadBlock((int)payloadSize);

				if (packetType == "SH")
				{
					foundSH = true;

					if (payloadSize <= 5)
					{
						break;
					}

					var pos = 4;
					Version = data[pos];
					pos++;
					frames = (uint)ReadSize(data, ref pos);
					if (pos > (uint)payloadSize - 3)
					{
						break;
					}

					var beginSilence = ReadSize(data, ref pos);
					if (pos > (uint)payloadSize - 2)
					{
						break;
					}

					var flags = data.Mid(pos, 1)
						.ToUShort(true);

					AudioSampleRate = sftable[(flags >> 13) & 0x07];
					AudioChannels = ((flags >> 4) & 0x0F) + 1;

					framecount = frames - beginSilence;
				}
				else if (packetType == "SE")
				{
					break;
				}
				else
				{
					file.Seek((int)payloadSize, SeekOrigin.Current);
				}
			}
		}


		private ulong ReadSize(File file, ref uint packetSizeLength, ref bool eof)
		{
			uint tmp;
			ulong size = 0;

			do
			{
				var b = file.ReadBlock(1);
				if (b.IsEmpty)
				{
					eof = true;
					break;
				}

				tmp = b.ToUInt();
				size = (size << 7) | (tmp & 0x7F);
				packetSizeLength++;
			} while ((tmp & 0x80) == 1);

			return size;
		}


		private ulong ReadSize(ByteVector data, ref int pos)
		{
			uint tmp;
			ulong size = 0;

			do
			{
				tmp = data[pos++];
				size = (size << 7) | (tmp & 0x7F);
			} while ((tmp & 0x80) == 0x80 && pos < data.Count);

			return size;
		}


		/// <summary>
		///     Gets the duration of the media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="TimeSpan" /> containing the duration of the
		///     media represented by the current instance.
		/// </value>

		public TimeSpan Duration
		{
			get
			{
				if (AudioSampleRate <= 0 && stream_length <= 0)
				{
					return TimeSpan.Zero;
				}

				if (Version <= 7)
				{
					return TimeSpan.FromSeconds(
						(frames * 1152 - 576) /
						(double)AudioSampleRate + 0.5);
				}

				return TimeSpan.FromMilliseconds(
					framecount * 1000.0 /
					AudioSampleRate + 0.5);
			}
		}

		/// <summary>
		///     Gets the types of media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     Always <see cref="MediaTypes.Audio" />.
		/// </value>
		public MediaTypes MediaTypes => MediaTypes.Audio;

		/// <summary>
		///     Gets a text description of the media represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="string" /> object containing a description
		///     of the media represented by the current instance.
		/// </value>
		public string Description => string.Format(
			CultureInfo.InvariantCulture,
			"MusePack Version {0} Audio", Version);

		/// <summary>
		///     Gets the bitrate of the audio represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing a bitrate of the
		///     audio represented by the current instance.
		/// </value>
		public int AudioBitrate
		{
			get
			{
				if (header_data != 0)
				{
					return (int)((header_data >> 23) & 0x01ff);
				}

				if (Version <= 7)
				{
					return (int)(Duration > TimeSpan.Zero
						? stream_length * 8L /
						  Duration.TotalSeconds / 1000
						: 0);
				}

				return (int)(stream_length * 8 / Duration.TotalMilliseconds + 0.5);
			}
		}

		/// <summary>
		///     Gets the sample rate of the audio represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the sample rate of
		///     the audio represented by the current instance.
		/// </value>
		public int AudioSampleRate { get; private set; }

		/// <summary>
		///     Gets the number of channels in the audio represented by
		///     the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the number of
		///     channels in the audio represented by the current
		///     instance.
		/// </value>
		public int AudioChannels { get; private set; }

		/// <summary>
		///     Gets the WavPack version of the audio represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the WavPack version
		///     of the audio represented by the current instance.
		/// </value>
		public int Version { get; private set; }


		/// <summary>
		///     Generates a hash code for the current instance.
		/// </summary>
		/// <returns>
		///     A <see cref="int" /> value containing the hash code for
		///     the current instance.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (int)(header_data ^ AudioSampleRate ^
				             frames ^ Version);
			}
		}


		/// <summary>
		///     Checks whether or not the current instance is equal to
		///     another object.
		/// </summary>
		/// <param name="other">
		///     A <see cref="object" /> to compare to the current
		///     instance.
		/// </param>
		/// <returns>
		///     A <see cref="bool" /> value indicating whether or not the
		///     current instance is equal to <paramref name="other" />.
		/// </returns>
		/// <seealso cref="M:System.IEquatable`1.Equals" />
		public override bool Equals(object other)
		{
			if (!(other is StreamHeader))
			{
				return false;
			}

			return Equals((StreamHeader)other);
		}


		/// <summary>
		///     Checks whether or not the current instance is equal to
		///     another instance of <see cref="StreamHeader" />.
		/// </summary>
		/// <param name="other">
		///     A <see cref="StreamHeader" /> object to compare to the
		///     current instance.
		/// </param>
		/// <returns>
		///     A <see cref="bool" /> value indicating whether or not the
		///     current instance is equal to <paramref name="other" />.
		/// </returns>
		/// <seealso cref="M:System.IEquatable`1.Equals" />
		public bool Equals(StreamHeader other)
		{
			return header_data == other.header_data &&
			       AudioSampleRate == other.AudioSampleRate &&
			       Version == other.Version &&
			       frames == other.frames;
		}


		/// <summary>
		///     Gets whether or not two instances of
		///     <see
		///         cref="StreamHeader" />
		///     are equal to eachother.
		/// </summary>
		/// <param name="first">
		///     A <see cref="StreamHeader" /> object to compare.
		/// </param>
		/// <param name="second">
		///     A <see cref="StreamHeader" /> object to compare.
		/// </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="first" /> is
		///     equal to <paramref name="second" />. Otherwise,
		///     <see
		///         langword="false" />
		///     .
		/// </returns>
		public static bool operator ==(StreamHeader first,
			StreamHeader second)
		{
			return first.Equals(second);
		}


		/// <summary>
		///     Gets whether or not two instances of
		///     <see
		///         cref="StreamHeader" />
		///     differ.
		/// </summary>
		/// <param name="first">
		///     A <see cref="StreamHeader" /> object to compare.
		/// </param>
		/// <param name="second">
		///     A <see cref="StreamHeader" /> object to compare.
		/// </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="first" /> is
		///     unequal to <paramref name="second" />. Otherwise,
		///     <see
		///         langword="false" />
		///     .
		/// </returns>
		public static bool operator !=(StreamHeader first,
			StreamHeader second)
		{
			return !first.Equals(second);
		}
	}
}
