using System;

namespace Sander.DirLister.Core.TagLib
{
	/// <summary>
	///    Indicates the types of media represented by a <see cref="ICodec"
	///    /> or <see cref="Properties" /> object.
	/// </summary>
	/// <remarks>
	///    These values can be bitwise combined to represent multiple media
	///    types.
	/// </remarks>
	[Flags]
	public enum MediaTypes
	{
		/// <summary>
		///    No media is present.
		/// </summary>
		None = 0,

		/// <summary>
		///    Audio is present.
		/// </summary>
		Audio = 1,

		/// <summary>
		///    Video is present.
		/// </summary>
		Video = 2
	}

	/// <summary>
	///    This interface provides basic information, common to all media
	///    codecs.
	/// </summary>
	public interface ICodec
	{
		/// <summary>
		///    Gets the duration of the media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="TimeSpan" /> containing the duration of the
		///    media represented by the current instance.
		/// </value>
		TimeSpan Duration { get; }

		/// <summary>
		///    Gets the types of media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A bitwise combined <see cref="MediaTypes" /> containing
		///    the types of media represented by the current instance.
		/// </value>
		MediaTypes MediaTypes { get; }
	}

	/// <summary>
	///    This interface inherits <see cref="ICodec" /> to provide
	///    information about an audio codec.
	/// </summary>
	/// <remarks>
	///    <para>When dealing with a <see cref="ICodec" />, if <see
	///    cref="ICodec.MediaTypes" /> contains <see cref="MediaTypes.Audio"
	///    />, it is safe to assume that the object also inherits <see
	///    cref="IAudioCodec" /> and can be recast without issue.</para>
	/// </remarks>
	public interface IAudioCodec : ICodec
	{
		/// <summary>
		///    Gets the bitrate of the audio represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing a bitrate of the
		///    audio represented by the current instance.
		/// </value>
		int AudioBitrate { get; }

		/// <summary>
		///    Gets the sample rate of the audio represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the sample rate of
		///    the audio represented by the current instance.
		/// </value>
		int AudioSampleRate { get; }

		/// <summary>
		///    Gets the number of channels in the audio represented by
		///    the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the number of
		///    channels in the audio represented by the current
		///    instance.
		/// </value>
		int AudioChannels { get; }
	}

	/// <summary>
	///    This interface provides information specific
	///    to lossless audio codecs.
	/// </summary>
	public interface ILosslessAudioCodec
	{
		/// <summary>
		///    Gets the number of bits per sample in the audio
		///    represented by the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the number of bits
		///    per sample in the audio represented by the current
		///    instance.
		/// </value>
		int BitsPerSample { get; }
	}

	/// <summary>
	///    This interface inherits <see cref="ICodec" /> to provide
	///    information about a video codec.
	/// </summary>
	/// <remarks>
	///    <para>When dealing with a <see cref="ICodec" />, if <see
	///    cref="ICodec.MediaTypes" /> contains <see cref="MediaTypes.Video"
	///    />, it is safe to assume that the object also inherits <see
	///    cref="IVideoCodec" /> and can be recast without issue.</para>
	/// </remarks>
	public interface IVideoCodec : ICodec
	{
		/// <summary>
		///    Gets the width of the video represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the width of the
		///    video represented by the current instance.
		/// </value>
		int VideoWidth { get; }

		/// <summary>
		///    Gets the height of the video represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the height of the
		///    video represented by the current instance.
		/// </value>
		int VideoHeight { get; }
	}
}
