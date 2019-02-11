using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Sander.DirLister.Core.TagLib;

namespace Sander.DirLister.Core.Application.Media
{
	internal sealed class MediaInfoProvider : BaseProvider
	{
		internal MediaInfoProvider(Configuration configuration) : base(configuration)
		{
			SupportedExtensions = new HashSet<string>(
				@"aa, aac, aax, aif, aiff, ape, asf, avi, divx, dsf, fla, flac, m2a, m2v, m4a, m4b, m4p, m4v,
mka, mks, mkv, mp+, mp1, mp2, mp3, mp4, mpc, mpe, mpeg, mpg, mpp, mpv2, oga, ogg, ogv, opus, wav, webm, wma, wmv, wv"
					.Split(',').Select(x => x.Trim('.', ' ')).OrderBy(x => x),
				StringComparer.OrdinalIgnoreCase);

			Configuration.LoggingAction.Invoke(TraceLevel.Info,
				$"Supported media extensions: {string.Join(", ", SupportedExtensions)}");
		}



		internal void GetMediaInfo(FileEntry entry)
		{
			try
			{
				using (var file = File.Create(new File.LocalFileAbstraction(entry.Fullname), null, ReadStyle.Average))
				{
					if (file == null || file.Properties.Duration == TimeSpan.Zero)
						return;

					if ((file.Properties.MediaTypes & MediaTypes.Video) != 0)
					{
						entry.MediaInfo = GetVideoInfo(file.Properties);
						return;
					}

					if ((file.Properties.MediaTypes & MediaTypes.Audio) != 0)
					{
						entry.MediaInfo = GetAudioInfo(file.Properties);						;
					}
				}
			}
			catch (Exception e)
			{
				Configuration.LoggingAction.Invoke(TraceLevel.Warning,
					$"Error getting media information for \"{entry.Fullname}\":{Environment.NewLine}, {e}");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private MediaInfo GetAudioInfo(Properties fileProperties)
		{
			var mediaInfo = new MediaInfo(MediaType.Audio)
			{
				Duration = fileProperties.Duration,
				AudioChannels = fileProperties.AudioChannels,
				AudioSampleRate = fileProperties.AudioSampleRate,
				AudioBitRate = fileProperties.AudioBitrate
			};
			return mediaInfo;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private MediaInfo GetVideoInfo(Properties fileProperties)
		{
			var mediaInfo = new MediaInfo(MediaType.Video)
			{
				Height = fileProperties.VideoHeight,
				Width = fileProperties.VideoWidth,
				BitsPerPixel = fileProperties.BitsPerSample,
				Duration = fileProperties.Duration,
				AudioChannels = fileProperties.AudioChannels,
				AudioSampleRate = fileProperties.AudioSampleRate,
				AudioBitRate = fileProperties.AudioBitrate
			};
			return mediaInfo;
		}

	}
}
