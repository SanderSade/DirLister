using System;

namespace Sander.DirLister.Core
{
	/// <summary>
	///     Media info, encapsulating image/audio/video data
	/// </summary>
	public sealed class MediaInfo
	{
		internal MediaInfo(MediaType mediaType)
		{
			MediaType = mediaType;
		}

		/// <summary>
		///     Media type of the current file
		/// </summary>
		public MediaType MediaType { get; }

		/// <summary>
		///     Width in pixels
		/// </summary>
		public int Width { get; internal set; }

		/// <summary>
		///     Height in pixels
		/// </summary>
		public int Height { get; internal set; }

		/// <summary>
		///     Bits per pixel (BPP)
		/// </summary>
		public int BitsPerPixel { get; internal set; }

		/// <summary>
		///     Duration of the current media
		/// </summary>
		public TimeSpan Duration { get; internal set; }

		/// <summary>
		///     Audio channels
		/// </summary>
		public int AudioChannels { get; internal set; }

		/// <summary>
		///     Audio samplerate
		/// </summary>
		public int AudioSampleRate { get; internal set; }

		/// <summary>
		///     Audio bitrate
		/// </summary>
		public int AudioBitRate { get; internal set; }
	}
}
