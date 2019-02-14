using System;
using System.Runtime.Serialization;

namespace Sander.DirLister.Core
{
	/// <summary>
	///     Media info, encapsulating image/audio/video data
	/// </summary>
	public sealed partial class MediaInfo
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
		///     Duration of the current media
		/// </summary>
		public TimeSpan Duration { get; internal set; }


		/// <summary>
		///     Height in pixels
		/// </summary>
		[DataMember(Name = "height", Order = 3, EmitDefaultValue = false, IsRequired = false)]
		public int Height { get; internal set; }

		/// <summary>
		///     Width in pixels
		/// </summary>
		[DataMember(Name = "width", Order = 4, EmitDefaultValue = false, IsRequired = false)]
		public int Width { get; internal set; }


		/// <summary>
		///     Bits per pixel (BPP)
		/// </summary>
		[DataMember(Name = "bitsPerPixel", Order = 5, EmitDefaultValue = false, IsRequired = false)]
		public int BitsPerPixel { get; internal set; }


		/// <summary>
		///     Audio channels
		/// </summary>
		[DataMember(Name = "audioChannels", Order = 6, EmitDefaultValue = false, IsRequired = false)]
		public int AudioChannels { get; internal set; }

		/// <summary>
		///     Audio samplerate
		/// </summary>
		[DataMember(Name = "audioSampleRate", Order = 7, EmitDefaultValue = false, IsRequired = false)]
		public int AudioSampleRate { get; internal set; }

		/// <summary>
		///     Audio bitrate
		/// </summary>
		[DataMember(Name = "audioBitRate", Order = 8, EmitDefaultValue = false, IsRequired = false)]
		public int AudioBitRate { get; internal set; }
	}
}
