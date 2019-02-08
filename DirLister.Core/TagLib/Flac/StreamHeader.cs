using System;

namespace Sander.DirLister.Core.TagLib.Flac
{
	/// <summary>
	///    This structure implements <see cref="IAudioCodec" /> and provides
	///    information about a Flac audio stream.
	/// </summary>
	public struct StreamHeader : IAudioCodec, ILosslessAudioCodec
	{
		/// <summary>
		///    Contains the flags.
		/// </summary>
		private readonly uint flags;

		/// <summary>
		///    Contains the low portion of the length.
		/// </summary>
		private readonly uint low_length;

		/// <summary>
		///    Contains the stream length.
		/// </summary>
		private readonly long stream_length;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="StreamHeader" /> by reading a raw stream header
		///    structure and using the stream length.
		/// </summary>
		/// <param name="data">
		///    A <see cref="ByteVector" /> object containing the raw
		///    stream header.
		/// </param>
		/// <param name="streamLength">
		///    A <see cref="long" /> value containing the length of the
		///    stream.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="data" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///    <paramref name="data" /> contains less than 18 bytes.
		/// </exception>
		public StreamHeader(ByteVector data, long streamLength)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			if (data.Count < 18)
				throw new CorruptFileException(
					"Not enough data in FLAC header.");

			stream_length = streamLength;
			flags = data.Mid(10, 4)
			            .ToUInt(true);
			low_length = data.Mid(14, 4)
			                 .ToUInt(true);
		}


		/// <summary>
		///    Gets the duration of the media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="TimeSpan" /> containing the duration of the
		///    media represented by the current instance.
		/// </value>
		public TimeSpan Duration => AudioSampleRate > 0 && stream_length > 0
			? TimeSpan.FromSeconds(
				low_length /
				(double)AudioSampleRate +
				HighLength)
			: TimeSpan.Zero;

		/// <summary>
		///    Gets the bitrate of the audio represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing a bitrate of the
		///    audio represented by the current instance.
		/// </value>
		public int AudioBitrate => (int)(Duration > TimeSpan.Zero
			? stream_length * 8L /
			  Duration.TotalSeconds / 1000
			: 0);

		/// <summary>
		///    Gets the sample rate of the audio represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the sample rate of
		///    the audio represented by the current instance.
		/// </value>
		public int AudioSampleRate => (int)(flags >> 12);

		/// <summary>
		///    Gets the number of channels in the audio represented by
		///    the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the number of
		///    channels in the audio represented by the current
		///    instance.
		/// </value>
		public int AudioChannels => (int)(((flags >> 9) & 7) + 1);

		/// <summary>
		///    Gets the types of media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    Always <see cref="MediaTypes.Audio" />.
		/// </value>
		public MediaTypes MediaTypes => MediaTypes.Audio;

		/// <summary>
		///    Gets the sample width of the audio represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the sample width of
		///    the audio represented by the current instance.
		/// </value>
		[Obsolete("This property is depreciated, use BitsPerSample instead")]
		public int AudioSampleWidth => BitsPerSample;

		/// <summary>
		///    Gets the number of bits per sample in the audio
		///    represented by the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the number of bits
		///    per sample in the audio represented by the current
		///    instance.
		/// </value>
		public int BitsPerSample => (int)(((flags >> 4) & 31) + 1);

		/// <summary>
		///    Gets a text description of the media represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="string" /> object containing a description
		///    of the media represented by the current instance.
		/// </value>
		public string Description => "Flac Audio";

		/// <summary>
		///    Gets a high portion of the length of the audio
		///    represented by the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="uint" /> value containing the high portion
		///    of the length.
		/// </value>
		private uint HighLength => (uint)(AudioSampleRate > 0
			? (((flags & 0xf) << 28) /
			   AudioSampleRate) << 4
			: 0);
	}
}
