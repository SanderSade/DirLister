using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sander.DirLister.Core.Application.Writers
{
	internal sealed class TxtWriter : BaseWriter
	{
		public TxtWriter(Configuration configuration) : base(configuration)
		{
		}

		protected internal override string Write(List<FileEntry> entries)
		{
			var sb = AppendHeader(entries);
			var groups = GroupByFolder(entries);

			foreach (var group in groups)
			{
				AppendFolder(group, sb);
			}

			var fileName = GetFilename(OutputFormat.Txt);

			using (var sw = new StreamWriter(fileName, false, Encoding.UTF8, 2 << 16 /* 128KB*/))
			{
				sw.Write(sb.ToString());
			}

			return fileName;
		}

		private void AppendFolder(IGrouping<string, FileEntry> group, StringBuilder sb)
		{
			sb.AppendLine();
			sb.AppendLine();
			var folderWrapper = new string('=', group.Key.Length);
			sb.AppendLine(folderWrapper);
			sb.AppendLine(group.Key);
			sb.AppendLine(folderWrapper);

			foreach (var entry in group)
			{
				sb.Append(entry.Filename);
				if (Configuration.IncludeSize)
					sb.Append($" :: {entry.ReadableSize}");

				if (Configuration.IncludeFileDates)
					sb.Append(
						$" :: Created: {entry.Created.ToLocalTime():yyyy-MM-dd HH:mm:ss}, modified: {entry.Modified.ToLocalTime():yyyy-MM-dd HH:mm:ss}");

				if (Configuration.IncludeMediaInfo && entry.MediaInfo != null)
				{
					sb.Append($" :: {entry.MediaInfo.MediaType}");
					if (entry.MediaInfo.Duration != TimeSpan.Zero)
						sb.Append($" {FormatDuration(entry.MediaInfo.Duration)}");

					if (entry.MediaInfo.Height > 0)
						sb.Append($" {entry.MediaInfo.Height}x{entry.MediaInfo.Width},");

					sb.Append(GetMediaInfo("bpp", entry.MediaInfo.BitsPerPixel));

					if (entry.MediaInfo.AudioBitRate != 0 || entry.MediaInfo.AudioSampleRate != 0 ||
					    entry.MediaInfo.AudioChannels != 0)
					{
						sb.Remove(sb.Length - 1, 1);
						sb.Append(" :: audio");
						sb.Append(GetMediaInfo("bitrate", entry.MediaInfo.AudioBitRate));
						sb.Append(GetMediaInfo("channels", entry.MediaInfo.AudioChannels));

						if (entry.MediaInfo.AudioSampleRate > 0)
							sb.Append(GetMediaInfo("sample rate", entry.MediaInfo.AudioSampleRate / 1000f));
					}

					sb.Remove(sb.Length - 1, 1);//remove trailing comma
				}

				sb.AppendLine();
			}
		}


		private string GetMediaInfo<T>(string name, T value)
		{
			return value == null || value.Equals(default(T))
				? string.Empty
				: FormattableString.Invariant($" {name}: {value.ToString()},");
		}

		private StringBuilder AppendHeader(List<FileEntry> entries)
		{
			var sb = new StringBuilder();
			sb.AppendLine("DirLister output for:");
			foreach (var inputFolder in Configuration.InputFolders)
			{
				sb.AppendLine($"* {inputFolder}");
			}

			sb.AppendLine($"Total {entries.Count} files, {Utils.ReadableSize(entries.Sum(x => x.Size))}");
			sb.AppendLine();
			sb.AppendLine("Source, updates and support: https://github.com/SanderSade/DirLister/");
			return sb;
		}
	}
}
