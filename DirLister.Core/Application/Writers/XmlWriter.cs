using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Sander.DirLister.Core.Application.Writers
{
	internal sealed class XmlWriter : BaseWriter
	{
		public XmlWriter(Configuration configuration) : base(configuration)
		{
		}

		protected internal override string Write(List<FileEntry> entries)
		{
			var folders = GroupByFolder(entries);

			var xRoot = new XElement(nameof(DirLister));
			var xFolders = new List<XElement>();
			foreach (var fileEntries in folders)
			{
				var xFolder = GetFolderElement(fileEntries);
				xFolders.Add(xFolder);
			}

			xRoot.Add(xFolders);

			var fileName = GetFilename(OutputFormat.Xml);
			xRoot.Save(fileName);

			return fileName;
		}

		private XElement GetFolderElement(IGrouping<string, FileEntry> entries)
		{
			var xfolder = new XElement("directory");
			xfolder.SetAttributeValue("name", entries.Key);
			xfolder.SetAttributeValue("fileCount", entries.Count());
			xfolder.SetAttributeValue("totalSize", entries.Sum(x => x.Size));
			var files = new List<XElement>();
			foreach (var entry in entries)
				files.Add(GetFileElement(entry));

			xfolder.Add(files);

			return xfolder;
		}

		private XElement GetFileElement(FileEntry entry)
		{
			var file = new XElement("file");
			file.SetAttributeValue("name", entry.Filename);

			if (Configuration.IncludeSize)
				file.SetAttributeValue(nameof(entry.Size), entry.Size);

			if (Configuration.IncludeFileDates)
			{
				file.SetAttributeValue(nameof(entry.Created), entry.Created.ToLocalTime());
				file.SetAttributeValue(nameof(entry.Modified), entry.Modified.ToLocalTime());
			}

			if (Configuration.IncludeMediaInfo && entry.MediaInfo != null)
			{
				var media = GetMediaInfo(entry);
				file.Add(media);
			}

			return file;
		}

		private static XElement GetMediaInfo(FileEntry entry)
		{
			var media = new XElement(nameof(entry.MediaInfo));
			media.SetAttributeValue(nameof(entry.MediaInfo.MediaType), entry.MediaInfo.MediaType);

			if (entry.MediaInfo.Duration != TimeSpan.Zero)
				media.SetAttributeValue(nameof(entry.MediaInfo.Duration), entry.MediaInfo.Duration.TotalSeconds);

			if (entry.MediaInfo.Height != 0)
				media.SetAttributeValue(nameof(entry.MediaInfo.Height), entry.MediaInfo.Height);

			if (entry.MediaInfo.Width != 0)
				media.SetAttributeValue(nameof(entry.MediaInfo.Width), entry.MediaInfo.Width);

			if (entry.MediaInfo.BitsPerPixel != 0)
				media.SetAttributeValue(nameof(entry.MediaInfo.BitsPerPixel), entry.MediaInfo.BitsPerPixel);

			if (entry.MediaInfo.AudioBitRate != 0)
				media.SetAttributeValue(nameof(entry.MediaInfo.AudioBitRate), entry.MediaInfo.AudioBitRate);

			if (entry.MediaInfo.AudioChannels != 0)
				media.SetAttributeValue(nameof(entry.MediaInfo.AudioChannels), entry.MediaInfo.AudioChannels);

			if (entry.MediaInfo.AudioSampleRate != 0)
				media.SetAttributeValue(nameof(entry.MediaInfo.AudioSampleRate), entry.MediaInfo.AudioSampleRate);

			return media;
		}
	}
}
