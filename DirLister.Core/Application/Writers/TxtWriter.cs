using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sander.DirLister.Core.Application.Writers
{
	internal sealed class TxtWriter : BaseWriter
	{
		private readonly StringBuilder _sb;

		public TxtWriter(Configuration configuration, DateTimeOffset endDate, List<FileEntry> entries) : base(configuration, endDate, entries)
		{
			_sb = new StringBuilder();
		}

		protected internal override string Write()
		{
			AppendHeader();

			foreach (var group in GroupedEntries)
			{
				AppendFolder(group);
			}

			return WriteFile(_sb, OutputFormat.Txt);
		}

		private void AppendFolder(IGrouping<string, FileEntry> group)
		{
			_sb.AppendLine();
			_sb.AppendLine();
			var folderWrapper = new string('=', group.Key.Length);
			_sb.AppendLine(folderWrapper);
			_sb.AppendLine(group.Key);
			_sb.AppendLine(folderWrapper);

			foreach (var entry in group)
			{
				_sb.Append(entry.Filename);
				if (Configuration.IncludeSize)
					_sb.Append($" :: {entry.ReadableSize}");

				if (Configuration.IncludeFileDates)
					_sb.Append(
						$" :: Created: {entry.Created.ToLocalTime().ToString(Configuration.DateFormat)}, modified: {entry.Modified.ToLocalTime().ToString(Configuration.DateFormat)}");

				if (Configuration.IncludeMediaInfo && entry.MediaInfo != null)
				{
					_sb.Append($" :: {entry.MediaInfo.MediaType}");
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

					_sb.Remove(_sb.Length - 1, 1);//remove trailing comma
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

		private void AppendHeader()
		{
			_sb.AppendLine($"Directory listing by DirLister, {EndDate.ToLocalTime().ToString(Configuration.DateFormat)}:");
			foreach (var folder in Configuration.InputFolders)
			{
				var files = GroupedEntries.Where(x => x.Key.StartsWith(folder, StringComparison.Ordinal))
					.SelectMany(x => x.ToList())
					.ToList();
				_sb.AppendLine(
					$"* {folder} files: {files.Count}, size: {Utils.ReadableSize(files.Sum(x => x.Size))}");
			}

			if (Configuration.InputFolders.Count > 1)
			{
				_sb.AppendLine();
				_sb.AppendLine($"** Total: {Entries.Count} files, {Utils.ReadableSize(Entries.Sum(x => x.Size))}");
			}
			_sb.AppendLine();
			_sb.AppendLine("Source, updates and support: https://github.com/SanderSade/DirLister/");
		}
	}
}
