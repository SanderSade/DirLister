using System;

namespace Sander.DirLister.Core.TagLib.Dsf
{
	/// <summary>
	///    This struct implements <see cref="IAudioCodec" /> to provide
	///    support for reading DSF stream properties.
	/// </summary>
	public struct StreamHeader : IAudioCodec, ILosslessAudioCodec
	{
		/// <summary>
		///    Contains the version.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (12-15).
		///    Currently only value of 1 is valid.
		/// </remarks>
		private readonly ushort version;

		/// <summary>
		///    The Format Id.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (16-19).
		///    0: DSD Raw
		/// </remarks>
		private readonly ushort format_id;

		/// <summary>
		///    The Channel Type.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (20-23).
		///    1: mono 
		///    2:stereo 
		///    3:3 channels 
		///    4: quad 
		///    5: 4 channels 
		///    6: 5 channels 
		///    7: 5.1 channels 
		/// </remarks>
		private readonly ushort channel_type;

		/// <summary>
		///    Contains the number of channels.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (24-27).
		///    1 is monophonic, 2 is stereo, 4 means 4 channels, etc..
		///    up to 6 channels may be represented
		/// </remarks>
		private readonly ushort channels;

		/// <summary>
		///    Contains the sample rate.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (28-31).
		///    the sample rate at which the sound is to be played back, 
		///    in Hz: 2822400, 5644800
		/// </remarks>
		private readonly ulong sample_rate;

		/// <summary>
		///    Contains the number of bits per sample.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (32-35).
		///    It can be any number from 1 to 8.
		/// </remarks>
		private readonly ushort bits_per_sample;

		/// <summary>
		///    Contains the number of sample frames per channel.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (36-43).
		/// </remarks>
		private readonly ulong sample_count;

		/// <summary>
		///    Contains the Block size per channel.
		/// </summary>
		/// <remarks>
		///    This value is stored in bytes (44-47).
		///    Always: 4096
		/// </remarks>
		private readonly uint channel_blksize;

		/// <summary>
		///    Contains the length of the audio stream.
		/// </summary>
		/// <remarks>
		///    This value is provided by the constructor.
		/// </remarks>
		private readonly long stream_length;

		/// <summary>
		///    The size of an DSF Format chunk
		/// </summary>
		public const uint Size = 52;

		/// <summary>
		///    The identifier used to recognize a DSF file.
		///    Altough an DSF file start with "DSD ", we're interested
		///    in the Format chunk only, which contains the properties we need.
		/// </summary>
		/// <value>
		///    "fmt "
		/// </value>
		public static readonly ReadOnlyByteVector FileIdentifier =
			"fmt ";


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
		///    DSF Audio stream in bytes.
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

			// The first 12 bytes contain the Format chunk identifier "fmt "
			// And the size of the format chunk, which is always 52
			version = data.Mid(12, 4)
			              .ToUShort(false);
			format_id = data.Mid(16, 4)
			                .ToUShort(false);
			channel_type = data.Mid(20, 4)
			                   .ToUShort(false);
			channels = data.Mid(24, 4)
			               .ToUShort(false);
			sample_rate = data.Mid(28, 4)
			                  .ToULong(false);
			bits_per_sample = data.Mid(32, 4)
			                      .ToUShort(false);
			sample_count = data.Mid(36, 8)
			                   .ToULong(false);
			channel_blksize = data.Mid(44, 4)
			                      .ToUShort(false);
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
				if (sample_rate <= 0 || sample_count <= 0)
					return TimeSpan.Zero;

				return TimeSpan.FromSeconds(
					sample_count /
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
		public string Description => "DSF Audio";

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
