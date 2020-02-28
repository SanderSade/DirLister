using System;

namespace Sander.DirLister.Core.TagLib.Aac
{
	/// <summary>
	///     This structure implements <see cref="IAudioCodec" /> and provides
	///     information about an ADTS AAC audio stream.
	/// </summary>
	public class AudioHeader : IAudioCodec
	{
		/// <summary>
		///     An empty and unset header.
		/// </summary>
		public static readonly AudioHeader Unknown =
			new AudioHeader();

		/// <summary>
		///     Contains a sample rate table for ADTS AAC audio.
		/// </summary>
		private static readonly int[] sample_rates = new int[13] { 96000, 88200, 64000, 48000, 44100, 32000, 24000, 22050, 16000, 12000, 11025, 8000, 7350 };

		/// <summary>
		///     Contains a channel table for ADTS AAC audio.
		/// </summary>
		private static readonly int[] channels = new int[8] { 0, 1, 2, 3, 4, 5, 6, 8 };

		/// <summary>
		///     Contains the audio stream length.
		/// </summary>
		private long stream_length;


		/// <summary>
		///     Constructs and initializes a new empty instance of
		///     <see
		///         cref="AudioHeader" />
		/// </summary>
		private AudioHeader()
		{
			stream_length = 0;
			Duration = TimeSpan.Zero;
			AudioChannels = 0;
			AudioBitrate = 0;
			AudioSampleRate = 0;
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="AudioHeader" />
		///     by populating it with specified
		///     values.
		/// </summary>
		/// <param name="channels">
		///     A <see cref="int" /> value indicating the number
		///     of channels in the audio stream
		/// </param>
		/// <param name="bitrate">
		///     A <see cref="int" /> value indicating the bitrate
		///     of  the audio stream
		/// </param>
		/// <param name="samplerate">
		///     A <see cref="int" /> value indicating the samplerate
		///     of  the audio stream
		/// </param>
		/// <param name="numberofsamples">
		///     A <see cref="int" /> value indicating the number
		///     of samples in the audio stream
		/// </param>
		/// <param name="numberofframes">
		///     A <see cref="int" /> value indicating the number
		///     of frames in the audio stream
		/// </param>
		private AudioHeader(int channels, int bitrate,
			int samplerate, int numberofsamples, int numberofframes)
		{
			Duration = TimeSpan.Zero;
			stream_length = 0;
			AudioChannels = channels;
			AudioBitrate = bitrate;
			AudioSampleRate = samplerate;
		}


		/// <summary>
		///     Gets a text description of the media represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="string" /> object containing a description
		///     of the media represented by the current instance.
		/// </value>
		public string Description => "ADTS AAC";

		/// <summary>
		///     Gets the types of media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     Always <see cref="MediaTypes.Audio" />.
		/// </value>
		public MediaTypes MediaTypes => MediaTypes.Audio;

		/// <summary>
		///     Gets the duration of the media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="TimeSpan" /> containing the duration of the
		///     media represented by the current instance.
		/// </value>
		/// <remarks>
		///     If <see cref="SetStreamLength" /> has not been called, this
		///     value will not be correct.
		/// </remarks>
		public TimeSpan Duration { get; private set; }

		/// <summary>
		///     Gets the bitrate of the audio represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing a bitrate of the
		///     audio represented by the current instance.
		/// </value>
		public int AudioBitrate { get; }

		/// <summary>
		///     Gets the sample rate of the audio represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the sample rate of
		///     the audio represented by the current instance.
		/// </value>
		public int AudioSampleRate { get; }

		/// <summary>
		///     Gets the number of channels in the audio represented by
		///     the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the number of
		///     channels in the audio represented by the current
		///     instance.
		/// </value>
		public int AudioChannels { get; }


		/// <summary>
		///     Sets the length of the audio stream represented by the
		///     current instance.
		/// </summary>
		/// <param name="streamLength">
		///     A <see cref="long" /> value specifying the length in
		///     bytes of the audio stream represented by the current
		///     instance.
		/// </param>
		/// <remarks>
		///     The this value has been set, <see cref="Duration" /> will
		///     return an incorrect value.
		/// </remarks>
		public void SetStreamLength(long streamLength)
		{
			stream_length = streamLength;
			Duration = TimeSpan.FromSeconds(stream_length * 8.0 / AudioBitrate);
		}


		/// <summary>
		///     Searches for an audio header in a <see cref="TagLib.File" /> starting at a specified position and searching through
		///     a specified number of bytes.
		/// </summary>
		/// <param name="header">
		///     A <see cref="AudioHeader" /> object in which the found
		///     header will be stored.
		/// </param>
		/// <param name="file">
		///     A <see cref="TagLib.File" /> object to search.
		/// </param>
		/// <param name="position">
		///     A <see cref="long" /> value specifying the seek position
		///     in <paramref name="file" /> at which to start searching.
		/// </param>
		/// <param name="length">
		///     A <see cref="int" /> value specifying the maximum number
		///     of bytes to search before aborting.
		/// </param>
		/// <returns>
		///     A <see cref="bool" /> value indicating whether or not a
		///     header was found.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		public static bool Find(out AudioHeader header,
			TagLib.File file, long position, int length)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}

			var end = position + length;
			header = Unknown;

			file.Seek(position);

			var buffer = file.ReadBlock(3);

			if (buffer.Count < 3)
			{
				return false;
			}

			do
			{
				file.Seek(position + 3);
				buffer = buffer.Mid(buffer.Count - 3);
				buffer.Add(file.ReadBlock(
					(int)TagLib.File.BufferSize));

				for (var i = 0;
					i < buffer.Count - 3 &&
					(length < 0 || position + i < end);
					i++)
				{
					if (buffer[i] == 0xFF
					    && buffer[i + 1] >= 0xF0) // 0xFFF
					{
						try
						{
							var bits = new BitStream(buffer.Mid(i, 7)
								.Data);

							// 12 bits sync header 
							bits.ReadInt32(12);

							// 1 bit mpeg 2/4
							bits.ReadInt32(1);

							// 2 bits layer
							bits.ReadInt32(2);

							// 1 bit protection absent  
							bits.ReadInt32(1);

							// 2 bits profile object type
							bits.ReadInt32(2);

							// 4 bits sampling frequency index                            
							var samplerateindex = bits.ReadInt32(4);
							if (samplerateindex >= sample_rates.Length)
							{
								return false;
							}

							long samplerate = sample_rates[samplerateindex];

							// 1 bit private bit
							bits.ReadInt32(1);

							// 3 bits channel configuration
							var channelconfigindex = bits.ReadInt32(3);
							if (channelconfigindex >= channels.Length)
							{
								return false;
							}

							// 4 copyright bits
							bits.ReadInt32(4);

							// 13 bits frame length
							long framelength = bits.ReadInt32(13); // double check framelength
							if (framelength < 7)
							{
								return false;
							}

							// 11 bits buffer fullness
							bits.ReadInt32(11);

							// 2 bits number of raw data blocks in frame
							var numberofframes = bits.ReadInt32(2) + 1;

							long numberofsamples = numberofframes * 1024;
							var bitrate = framelength * 8 * samplerate / numberofsamples;

							header = new AudioHeader(channels[channelconfigindex],
								(int)bitrate,
								(int)samplerate,
								(int)numberofsamples,
								numberofframes);

							return true;
						}
						catch (CorruptFileException)
						{
						}
					}
				}

				position += TagLib.File.BufferSize;
			} while (buffer.Count > 3 && (length < 0 || position < end));

			return false;
		}


		/// <summary>
		///     Searches for an audio header in a <see cref="TagLib.File" /> starting at a specified position and searching to the
		///     end of the file.
		/// </summary>
		/// <param name="header">
		///     A <see cref="AudioHeader" /> object in which the found
		///     header will be stored.
		/// </param>
		/// <param name="file">
		///     A <see cref="TagLib.File" /> object to search.
		/// </param>
		/// <param name="position">
		///     A <see cref="long" /> value specifying the seek position
		///     in <paramref name="file" /> at which to start searching.
		/// </param>
		/// <returns>
		///     A <see cref="bool" /> value indicating whether or not a
		///     header was found.
		/// </returns>
		/// <remarks>
		///     Searching to the end of the file can be very, very slow
		///     especially for corrupt or non-MPEG files. It is
		///     recommended to use
		///     <see
		///         cref="M:AudioHeader.Find(AudioHeader,TagLib.File,long,int)" />
		///     instead.
		/// </remarks>
		public static bool Find(out AudioHeader header,
			TagLib.File file, long position)
		{
			return Find(out header, file, position, -1);
		}
	}
}
