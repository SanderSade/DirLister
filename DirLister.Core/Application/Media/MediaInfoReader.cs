using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sander.DirLister.Core.Application.Media
{
	internal sealed class MediaInfoReader
	{
		private readonly Configuration _configuration;
		private readonly ImageInfoProvider _imageInfoProvider;
		private readonly MediaInfoProvider _mediaInfoProvider;

		internal MediaInfoReader(Configuration configuration)
		{
			_configuration = configuration;
			_imageInfoProvider = new ImageInfoProvider(_configuration);
			_mediaInfoProvider = new MediaInfoProvider(_configuration);
		}

		internal void AddMediaInfo(List<FileEntry> entries)
		{
			if (entries.Count == 0)
				return;

			_configuration.LoggingAction.Invoke(TraceLevel.Info, "Fetching media info...");
			_configuration.SendProgress(50, "Fetching media info...");

			entries
				.AsParallel()
				.WithDegreeOfParallelism(_configuration.EnableMultithreading ? Environment.ProcessorCount : 1)
				.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
				.ForAll(x =>
				{
					if (_imageInfoProvider.IsMatch(x))
					{
						_imageInfoProvider.GetImageInfo(x);
					}
					else if (_mediaInfoProvider.IsMatch(x))
					{
						_mediaInfoProvider.GetMediaInfo(x);
					}
				});

			_configuration.SendProgress(90, "Media info fetched");
			_configuration.LoggingAction.Invoke(TraceLevel.Info, "Media info fetched");
		}

	}
}
