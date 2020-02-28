using System;
using System.Collections.Generic;
using System.Text;

namespace Sander.DirLister.Core.TagLib
{
	/// <summary>
	///     This class implements <see cref="IAudioCodec" />,
	///     <see
	///         cref="IVideoCodec" />
	///     and combines codecs to create generic media properties
	///     for a file.
	/// </summary>
	public sealed class Properties : IAudioCodec, IVideoCodec
	{
		/// <summary>
		///     Contains the codecs.
		/// </summary>
		private readonly ICodec[] codecs = new ICodec [0];

		/// <summary>
		///     Contains the duration.
		/// </summary>
		private readonly TimeSpan duration = TimeSpan.Zero;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="Properties" />
		///     with no codecs or duration.
		/// </summary>
		/// <remarks>
		///     <para>
		///         This constructor is used when media properties are
		///         not read.
		///     </para>
		/// </remarks>
		public Properties()
		{
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="Properties" />
		///     with a specified duration and array
		///     of codecs.
		/// </summary>
		/// <param name="duration">
		///     A <see cref="TimeSpan" /> containing the duration of the
		///     media, or <see cref="TimeSpan.Zero" /> if the duration is
		///     to be read from the codecs.
		/// </param>
		/// <param name="codecs">
		///     A <see cref="T:T:ICodec[]" /> containing the codecs to be
		///     used in the new instance.
		/// </param>
		public Properties(TimeSpan duration, params ICodec[] codecs)
		{
			this.duration = duration;
			if (codecs != null)
			{
				this.codecs = codecs;
			}
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="Properties" />
		///     with a specified duration and
		///     enumaration of codecs.
		/// </summary>
		/// <param name="duration">
		///     A <see cref="TimeSpan" /> containing the duration of the
		///     media, or <see cref="TimeSpan.Zero" /> if the duration is
		///     to be read from the codecs.
		/// </param>
		/// <param name="codecs">
		///     A <see cref="T:System.Collections.Generic.IEnumerable`1" /> object containing the
		///     codec to be used in the new instance.
		/// </param>
		public Properties(TimeSpan duration, IEnumerable<ICodec> codecs)
		{
			this.duration = duration;
			if (codecs != null)
			{
				this.codecs = new List<ICodec>(codecs)
					.ToArray();
			}
		}


		/// <summary>
		///     Gets the codecs contained in the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="T:System.Collections.Generic.IEnumerable`1" /> object containing the
		///     <see cref="ICodec" /> objects contained in the current
		///     instance.
		/// </value>
		public IEnumerable<ICodec> Codecs => codecs;

		/// <summary>
		///     Gets a string description of the media represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="string" /> object containing a description
		///     of the media represented by the current instance.
		/// </value>
		/// <remarks>
		///     The value contains the descriptions of the codecs joined
		///     by colons.
		/// </remarks>
		public string Description
		{
			get
			{
				var builder = new StringBuilder();
				foreach (var codec in codecs)
				{
					if (codec == null)
					{
						continue;
					}

					if (builder.Length != 0)
					{
						builder.Append("; ");
					}
				}

				return builder.ToString();
			}
		}

		/// <summary>
		///     Gets the number of bits per sample in the audio
		///     represented by the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the number of bits
		///     per sample in the audio represented by the current
		///     instance.
		/// </value>
		/// <remarks>
		///     This value is equal to the first non-zero quantization.
		/// </remarks>
		public int BitsPerSample
		{
			get
			{
				foreach (var codec in codecs)
				{
					if (codec == null ||
					    (codec.MediaTypes & MediaTypes.Audio) == 0)
					{
						continue;
					}

					if (codec is ILosslessAudioCodec lossless && lossless.BitsPerSample != 0)
					{
						return lossless.BitsPerSample;
					}
				}

				return 0;
			}
		}

		/// <summary>
		///     Gets the duration of the media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="TimeSpan" /> containing the duration of the
		///     media represented by the current instance.
		/// </value>
		/// <remarks>
		///     If the duration was set in the constructor, that value is
		///     returned. Otherwise, the longest codec duration is used.
		/// </remarks>
		public TimeSpan Duration
		{
			get
			{
				var duration = this.duration;

				if (duration != TimeSpan.Zero)
				{
					return duration;
				}

				foreach (var codec in codecs)
				{
					if (codec != null &&
					    codec.Duration > duration)
					{
						duration = codec.Duration;
					}
				}

				return duration;
			}
		}

		/// <summary>
		///     Gets the types of media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A bitwise combined <see cref="MediaTypes" /> containing
		///     the types of media represented by the current instance.
		/// </value>
		public MediaTypes MediaTypes
		{
			get
			{
				var types = MediaTypes.None;

				foreach (var codec in codecs)
				{
					if (codec != null)
					{
						types |= codec.MediaTypes;
					}
				}

				return types;
			}
		}

		/// <summary>
		///     Gets the bitrate of the audio represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> containing the bitrate of the audio
		///     represented by the current instance.
		/// </value>
		/// <remarks>
		///     This value is equal to the first non-zero audio bitrate.
		/// </remarks>
		public int AudioBitrate
		{
			get
			{
				foreach (var codec in codecs)
				{
					if (codec == null ||
					    (codec.MediaTypes & MediaTypes.Audio) == 0)
					{
						continue;
					}

					if (codec is IAudioCodec audio && audio.AudioBitrate != 0)
					{
						return audio.AudioBitrate;
					}
				}

				return 0;
			}
		}

		/// <summary>
		///     Gets the sample rate of the audio represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> containing the sample rate of the
		///     audio represented by the current instance.
		/// </value>
		/// <remarks>
		///     This value is equal to the first non-zero audio sample
		///     rate.
		/// </remarks>
		public int AudioSampleRate
		{
			get
			{
				foreach (var codec in codecs)
				{
					if (codec == null ||
					    (codec.MediaTypes & MediaTypes.Audio) == 0)
					{
						continue;
					}

					if (codec is IAudioCodec audio && audio.AudioSampleRate != 0)
					{
						return audio.AudioSampleRate;
					}
				}

				return 0;
			}
		}

		/// <summary>
		///     Gets the number of channels in the audio represented by
		///     the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> object containing the number of
		///     channels in the audio represented by the current
		///     instance.
		/// </value>
		/// <remarks>
		///     This value is equal to the first non-zero audio channel
		///     count.
		/// </remarks>
		public int AudioChannels
		{
			get
			{
				foreach (var codec in codecs)
				{
					if (codec == null ||
					    (codec.MediaTypes & MediaTypes.Audio) == 0)
					{
						continue;
					}

					if (codec is IAudioCodec audio && audio.AudioChannels != 0)
					{
						return audio.AudioChannels;
					}
				}

				return 0;
			}
		}

		/// <summary>
		///     Gets the width of the video represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> containing the width of the video
		///     represented by the current instance.
		/// </value>
		/// <remarks>
		///     This value is equal to the first non-zero video width.
		/// </remarks>
		public int VideoWidth
		{
			get
			{
				foreach (var codec in codecs)
				{
					if (codec == null ||
					    (codec.MediaTypes & MediaTypes.Video) == 0)
					{
						continue;
					}

					if (codec is IVideoCodec video && video.VideoWidth != 0)
					{
						return video.VideoWidth;
					}
				}

				return 0;
			}
		}

		/// <summary>
		///     Gets the height of the video represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> containing the height of the video
		///     represented by the current instance.
		/// </value>
		/// <remarks>
		///     This value is equal to the first non-zero video height.
		/// </remarks>
		public int VideoHeight
		{
			get
			{
				foreach (var codec in codecs)
				{
					if (codec == null ||
					    (codec.MediaTypes & MediaTypes.Video) == 0)
					{
						continue;
					}

					if (codec is IVideoCodec video && video.VideoHeight != 0)
					{
						return video.VideoHeight;
					}
				}

				return 0;
			}
		}
	}
}
