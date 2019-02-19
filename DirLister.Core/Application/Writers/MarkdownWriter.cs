using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Sander.DirLister.Core.Application.Writers
{
	internal sealed class MarkdownWriter : BaseWriter
	{
		private readonly StringBuilder _sb;


		internal MarkdownWriter(Configuration configuration, DateTimeOffset endDate) : base(configuration, endDate)
		{
			_sb = new StringBuilder();
		}


		/// <inheritdoc />
		protected internal override string Write(List<FileEntry> entries)
		{
			
			var groups = GroupByFolder(entries).ToList();
			AppendHeader(groups, entries);

			foreach (var group in groups)
			{
				AppendFolder(group);
			}

			return WriteFile(_sb, OutputFormat.Md);
		}


		private void AppendFolder(IGrouping<string, FileEntry> group)
		{
			_sb.AppendLine();
			_sb.AppendLine();
			_sb.Append($"### {WebUtility.HtmlEncode(group.Key)} `files: {group.Count()}");
			if (Configuration.IncludeSize)
				_sb.Append($"({Utils.ReadableSize(group.Sum(x => x.Size))})");

			_sb.AppendLine("`");
			_sb.AppendLine();

			foreach (var entry in group)
			{
				_sb.Append($"* {entry.Filename}");
				if (Configuration.IncludeSize)
					_sb.Append($" `{entry.ReadableSize}`");

				if (Configuration.IncludeFileDates)
					_sb.Append(
						$" `Created: {entry.Created.ToLocalTime().ToString(FileDateFormat)}, modified: {entry.Modified.ToLocalTime().ToString(FileDateFormat)}`");

				if (Configuration.IncludeMediaInfo && entry.MediaInfo != null)
				{
					_sb.Append($" `{entry.MediaInfo.MediaType}");
					if (entry.MediaInfo.Duration != TimeSpan.Zero)
						_sb.Append($" {FormatDuration(entry.MediaInfo.Duration)}");

					if (entry.MediaInfo.Height > 0)
						_sb.Append($" {entry.MediaInfo.Height}x{entry.MediaInfo.Width},");

					_sb.Append(GetMediaInfo("bpp", entry.MediaInfo.BitsPerPixel));

					if (entry.MediaInfo.AudioBitRate != 0 || entry.MediaInfo.AudioSampleRate != 0 ||
						entry.MediaInfo.AudioChannels != 0)
					{
						_sb.Remove(_sb.Length - 1, 1);
						_sb.Append(" :: audio");
						_sb.Append(GetMediaInfo("bitrate", entry.MediaInfo.AudioBitRate));
						_sb.Append(GetMediaInfo("channels", entry.MediaInfo.AudioChannels));

						if (entry.MediaInfo.AudioSampleRate > 0)
							_sb.Append(GetMediaInfo("sample rate", entry.MediaInfo.AudioSampleRate / 1000f));
					}

					_sb.Remove(_sb.Length - 1, 1); //remove trailing comma
					_sb.Append("`");
				}

				_sb.AppendLine();
			}
		}


		private string GetMediaInfo<T>(string name, T value)
		{
			return value == null || value.Equals(default(T))
				? string.Empty
				: FormattableString.Invariant($" {name}: {value.ToString()},");
		}


		private void AppendHeader(List<IGrouping<string, FileEntry>> folderList, List<FileEntry> entries)
		{
			_sb.AppendLine("## [DirLister](https://github.com/SanderSade/DirLister/) output for:");

			foreach (var folder in Configuration.InputFolders)
			{
				var files = folderList.Where(x => x.Key.StartsWith(folder, StringComparison.Ordinal))
									  .SelectMany(x => x.ToList())
									  .ToList();
				_sb.AppendLine(
					$"* {WebUtility.HtmlEncode(folder)} `files: {files.Count}, size: {Utils.ReadableSize(files.Sum(x => x.Size))}`");
			}

			if (Configuration.InputFolders.Count > 1)
			{
				_sb.AppendLine();
				{
					_sb.AppendLine(
						$"**Total: {entries.Count} files, {Utils.ReadableSize(entries.Sum(x => x.Size))}**");
				}
			}
		}
	}
}
