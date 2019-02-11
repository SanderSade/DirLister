using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sander.DirLister.Core.Application.Writers
{
	internal sealed class CsvWriter : BaseWriter
	{
		internal CsvWriter(Configuration configuration) : base(configuration)
		{
		}

		protected internal override string Write(List<FileEntry> entries)
		{
			var sb = new StringBuilder();
			GetHeaderLine(sb);

			foreach (var entry in entries)
			{
				GetFileLine(sb, entry);
			}

			var fileName = GetFilename(OutputFormat.Csv);

			using (var sw = new StreamWriter(fileName, false, Encoding.UTF8, 2 << 16 /* 128KB*/))
			{
				sw.Write(sb.ToString());
			}

			return fileName;
		}

		private void GetHeaderLine(StringBuilder sb)
		{
			sb.Append(Quote("File"));

			if (Configuration.IncludeSize)
				sb.Append($",{Quote(nameof(FileEntry.Size))}");

			if (Configuration.IncludeFileDates)
			{
				sb.Append($",{Quote(nameof(FileEntry.Created))}");
				sb.Append($",{Quote(nameof(FileEntry.Modified))}");
			}

			if (Configuration.IncludeMediaInfo)
			{
				sb.Append($",{Quote(nameof(FileEntry.MediaInfo.MediaType))}");
				sb.Append($",{Quote($"{nameof(FileEntry.MediaInfo.Duration)}(seconds)")} ");
				sb.Append($",{Quote(nameof(FileEntry.MediaInfo.Height))}");
				sb.Append($",{Quote(nameof(FileEntry.MediaInfo.Width))}");
				sb.Append($",{Quote(nameof(FileEntry.MediaInfo.BitsPerPixel))}");
				sb.Append($",{Quote(nameof(FileEntry.MediaInfo.AudioBitRate))}");
				sb.Append($",{Quote(nameof(FileEntry.MediaInfo.AudioChannels))}");
				sb.Append($",{Quote(nameof(FileEntry.MediaInfo.AudioSampleRate))}");
			}

			sb.AppendLine();
		}

		private void GetFileLine(StringBuilder sb, FileEntry entry)
		{
			sb.Append(Quote(entry.Fullname));

			if (Configuration.IncludeSize)
				sb.Append($",{Quote(entry.Size)}");

			if (Configuration.IncludeFileDates)
			{
				sb.Append($",{Quote(entry.Created.ToLocalTime().ToString())}");
				sb.Append($",{Quote(entry.Modified.ToLocalTime())}");
			}

			if (Configuration.IncludeMediaInfo && entry.MediaInfo != null)
			{
				sb.Append($",{Quote(entry.MediaInfo.MediaType.ToString())}");
				sb.Append($",{Quote(entry.MediaInfo.Duration.TotalSeconds)}");
				sb.Append($",{Quote(entry.MediaInfo.Height)}");
				sb.Append($",{Quote(entry.MediaInfo.Width)}");
				sb.Append($",{Quote(entry.MediaInfo.BitsPerPixel)}");
				sb.Append($",{Quote(entry.MediaInfo.AudioBitRate)}");
				sb.Append($",{Quote(entry.MediaInfo.AudioChannels)}");
				sb.Append($",{Quote(entry.MediaInfo.AudioSampleRate)}");
			}

			sb.AppendLine();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string Quote<T>(T value)
		{
			return value == null || value.Equals(default(T))
				? "\"\""
				: FormattableString.Invariant($"\"{value.ToString()}\"");
		}

	}
}
