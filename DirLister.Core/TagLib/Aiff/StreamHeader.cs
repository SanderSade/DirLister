using System;

namespace Sander.DirLister.Core.TagLib.Aiff
{
	/// <summary>
	///    This struct implements <see cref="IAudioCodec" /> to provide
	///    support for reading Apple's AIFF stream properties.
	/// </summary>
	public struct StreamHeader : IAudioCodec, ILosslessAudioCodec
	{
		/// <summary>
		///    Contains the number of channels.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (9,10).
		///    1 is monophonic, 2 is stereo, 4 means 4 channels, etc..
		///    any number of audio channels may be represented
		/// </remarks>
		private readonly ushort channels;

		/// <summary>
		///    Contains the number of sample frames in the Sound Data chunk.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (11-14).
		/// </remarks>
		private readonly ulong total_frames;

		/// <summary>
		///    Contains the number of bits per sample.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (15,16).
		///    It can be any number from 1 to 32.
		/// </remarks>
		private readonly ushort bits_per_sample;

		/// <summary>
		///    Contains the sample rate.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (17-26).
		///    the sample rate at which the sound is to be played back, 
		///    in sample frames per second
		/// </remarks>
		private readonly ulong sample_rate;

		/// <summary>
		///    Contains the length of the audio stream.
		/// </summary>
		/// <remarks>
		///    This value is provided by the constructor.
		/// </remarks>
		private readonly long stream_length;

		/// <summary>
		///    The size of an AIFF Common chunk
		/// </summary>
		public const uint Size = 26;

		/// <summary>
		///    The identifier used to recognize a AIFF file.
		///    Altough an AIFF file start with "FORM2, we're interested
		///    in the Common chunk only, which contains the properties we need.
		/// </summary>
		/// <value>
		///    "COMM"
		/// </value>
		public static readonly ReadOnlyByteVector FileIdentifier =
			"COMM";


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="StreamHeader" /> for a specified header block and
		///    stream length.
		/// </summary>
		/// <param name="data">
		///    A <see cref="ByteVector" /> object containing the stream
		///    header data.
		/// </param>
		/// <param name="streamLength">
		///    A <see cref="long" /> value containing the length of the
		///    AIFF Audio stream in bytes.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="data" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///    <paramref name="data" /> does not begin with <see
		///    cref="FileIdentifier" /> 
		/// </exception>
		public StreamHeader(ByteVector data, long streamLength)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			if (!data.StartsWith(FileIdentifier))
				throw new CorruptFileException(
					"Data does not begin with identifier.");

			stream_length = streamLength;

			// The first 8 bytes contain the Common chunk identifier "COMM"
			// And the size of the common chunk, which is always 18
			channels = data.Mid(8, 2)
			               .ToUShort(true);
			total_frames = data.Mid(10, 4)
			                   .ToULong(true);
			bits_per_sample = data.Mid(14, 2)
			                      .ToUShort(true);

			var sample_rate_indicator = data.Mid(17, 1);
			var sample_rate_tmp = data.Mid(18, 2)
			                          .ToULong(true);
			sample_rate = 44100; // Set 44100 as default sample rate

			// The following are combinations that iTunes 8 encodes to.
			// There may be other combinations in the field, but i couldn't test them.
			switch (sample_rate_tmp)
			{
				case 44100:
					if (sample_rate_indicator == 0x0E)
					{
						sample_rate = 44100;
					}
					else if (sample_rate_indicator == 0x0D)
					{
						sample_rate = 22050;
					}
					else if (sample_rate_indicator == 0x0C)
					{
						sample_rate = 11025;
					}

					break;

				case 48000:
					if (sample_rate_indicator == 0x0E)
					{
						sample_rate = 48000;
					}
					else if (sample_rate_indicator == 0x0D)
					{
						sample_rate = 24000;
					}

					break;

				case 64000:
					if (sample_rate_indicator == 0x0D)
					{
						sample_rate = 32000;
					}
					else if (sample_rate_indicator == 0x0C)
					{
						sample_rate = 16000;
					}
					else if (sample_rate_indicator == 0x0B)
					{
						sample_rate = 8000;
					}

					break;

				case 44510:
					if (sample_rate_indicator == 0x0D)
					{
						sample_rate = 22255;
					}

					break;

				case 44508:
					if (sample_rate_indicator == 0x0C)
					{
						sample_rate = 11127;
					}

					break;
			}
		}


		/// <summary>
		///    Gets the duration of the media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="TimeSpan" /> containing the duration of the
		///    media represented by the current instance.
		/// </value>
		public TimeSpan Duration
		{
			get
			{
				if (sample_rate <= 0 || total_frames <= 0)
					return TimeSpan.Zero;

				return TimeSpan.FromSeconds(
					total_frames /
					(double)sample_rate);
			}
		}

		/// <summary>
		///    Gets the types of media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    Always <see cref="MediaTypes.Audio" />.
		/// </value>
		public MediaTypes MediaTypes => MediaTypes.Audio;

		/// <summary>
		///    Gets a text description of the media represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="string" /> object containing a description
		///    of the media represented by the current instance.
		/// </value>
		public string Description => "AIFF Audio";

		/// <summary>
		///    Gets the bitrate of the audio represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing a bitrate of the
		///    audio represented by the current instance.
		/// </value>
		public int AudioBitrate
		{
			get
			{
				var d = Duration;
				if (d <= TimeSpan.Zero)
					return 0;

				return (int)(stream_length * 8L /
				             d.TotalSeconds) / 1000;
			}
		}

		/// <summary>
		///    Gets the sample rate of the audio represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the sample rate of
		///    the audio represented by the current instance.
		/// </value>
		public int AudioSampleRate => (int)sample_rate;

		/// <summary>
		///    Gets the number of channels in the audio represented by
		///    the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the number of
		///    channels in the audio represented by the current
		///    instance.
		/// </value>
		public int AudioChannels => channels;

		/// <summary>
		///    Gets the number of bits per sample in the audio
		///    represented by the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the number of bits
		///    per sample in the audio represented by the current
		///    instance.
		/// </value>
		public int BitsPerSample => bits_per_sample;
	}
}
