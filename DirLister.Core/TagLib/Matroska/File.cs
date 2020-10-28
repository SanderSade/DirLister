using System;
using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Matroska
{
	/// <summary>
	///     Enumeration listing supported Matroska track types.
	/// </summary>
	public enum TrackType
	{
		/// <summary>
		///     Video track type.
		/// </summary>
		Video = 0x1,

		/// <summary>
		///     Audio track type.
		/// </summary>
		Audio = 0x2,

		/// <summary>
		///     Complex track type.
		/// </summary>
		Complex = 0x3,

		/// <summary>
		///     Logo track type.
		/// </summary>
		Logo = 0x10,

		/// <summary>
		///     Subtitle track type.
		/// </summary>
		Subtitle = 0x11,

		/// <summary>
		///     Buttons track type.
		/// </summary>
		Buttons = 0x12,

		/// <summary>
		///     Control track type.
		/// </summary>
		Control = 0x20
	}

	/// <summary>
	///     This class extends <see cref="TagLib.File" /> to provide tagging
	///     and properties support for Matroska files.
	/// </summary>
	[SupportedMimeType("taglib/mkv", "mkv")]
	[SupportedMimeType("taglib/mka", "mka")]
	[SupportedMimeType("taglib/mks", "mks")]
	[SupportedMimeType("video/webm", "webm")]
	public sealed class File : TagLib.File
	{

		private readonly List<Track> tracks = new List<Track>();
		private TimeSpan duration;
		private double duration_unscaled;
		private ulong time_scale;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified path in the local file
		///     system and specified read style.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object containing the path of the
		///     file to use in the new instance.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path, ReadStyle propertiesStyle)
			: this(new LocalFileAbstraction(path),
				propertiesStyle)
		{
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified path in the local file
		///     system with an average read style.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object containing the path of the
		///     file to use in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		public File(string path)
			: this(path, ReadStyle.Average)
		{
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified file abstraction and
		///     specified read style.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading from and writing to the file.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="abstraction" /> is <see langword="null" />.
		/// </exception>
		public File(IFileAbstraction abstraction,
			ReadStyle propertiesStyle)
			: base(abstraction)
		{
			Mode = AccessMode.Read;

			try
			{
				ReadWrite(propertiesStyle);
			}
			finally
			{
				Mode = AccessMode.Closed;
			}

			var codecs = new List<ICodec>();

			foreach (var track in tracks)
			{
				codecs.Add(track);
			}

			Properties = new Properties(duration, codecs);
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified file abstraction with an
		///     average read style.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading from and writing to the file.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="abstraction" /> is <see langword="null" />.
		/// </exception>
		public File(IFileAbstraction abstraction)
			: this(abstraction, ReadStyle.Average)
		{
		}


		/// <summary>
		///     Gets the media properties of the file represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="TagLib.Properties" /> object containing the
		///     media properties of the file represented by the current
		///     instance.
		/// </value>
		public override Properties Properties { get; }


		/// <summary>
		///     Reads (and Write, if file Mode is Write) the file with a specified read style.
		/// </summary>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		private void ReadWrite(ReadStyle propertiesStyle)
		{
			var offset = ReadLeadText();

			var hasSegment = false;
			while (offset < (ulong)Length)
			{
				EBMLreader element;
				try
				{
					element = new EBMLreader(this, offset);
				}
				catch (Exception)
				{
					// Sometimes, the file has zero padding at the end
					if (hasSegment)
					{
						break; // Avoid crash
					}

					throw;
				}

				var ebml_id = (EBMLID)element.ID;
				var matroska_id = element.ID;

				switch (ebml_id)
				{
					case EBMLID.EBMLHeader:
						ReadHeader(element);
						break;
				}

				switch (matroska_id)
				{
					case MatroskaID.Segment:
						ReadWriteSegment(element, propertiesStyle);
						hasSegment = true;
						break;
				}

				offset += element.Size;
			}
		}


		private void ReadWriteSegment(EBMLreader element, ReadStyle propertiesStyle, bool retry = true)
		{
			// First make reference of all EBML elements at level 1 (top) in the Segment

			var segm_list = ReadSegments(element, retry); // Try to get it from SeekHead the first time (way faster)

			// Now process (read and prepare to write) the referenced elements we care about

			EBMLelement ebml_sinfo = null;


			var valid = true;

			foreach (var child in segm_list)
			{
				// the child here may be Abstract if it has been retrieved in the SeekHead,
				// so child.Read() must be used to retrieve the full EBML header

				var matroska_id = child.ID;
				switch (matroska_id)
				{
					case MatroskaID.SeekHead:
						valid = child.Read();
						break;

					case MatroskaID.SegmentInfo:
						if (valid = child.Read())
						{
							ReadCreateSegmentInfo(child, ebml_sinfo);
						}

						break;

					case MatroskaID.Tracks:
						if ((propertiesStyle & ReadStyle.Average) != 0)
						{
							if (valid = child.Read())
							{
								ReadTracks(child);
							}
						}

						break;

					case MatroskaID.Tags:
						valid = child.Read();
						break;

					case MatroskaID.Attachments:
						valid = child.Read();
						if (valid)
						{
							ReadAttachments(child, propertiesStyle);
						}

						break;

					case MatroskaID.CRC32: // We don't support it
						valid = child.Read();
						break;
				}

				if (!valid)
				{
					break;
				}
			}

			// Detect invalid SeekHead
			if (!valid)
			{
				if (retry)
				{
					MarkAsCorrupt("Invalid Meta Seek");

					// Retry it one last time
					ReadWriteSegment(element, propertiesStyle, false);
				}
				else
				{
					MarkAsCorrupt("Invalid EBML element Read");
				}
			}
		}


		private void ReadCreateSegmentInfo(EBMLreader element, EBMLelement ebml_sinfo)
		{
			ulong i = 0;
			while (i < element.DataSize)
			{
				var child = new EBMLreader(element, element.DataOffset + i);
				var matroska_id = child.ID;

				switch (matroska_id)
				{
					case MatroskaID.Duration:
						duration_unscaled = child.ReadDouble();
						if (time_scale > 0)
						{
							duration = TimeSpan.FromMilliseconds(duration_unscaled * time_scale / 1000000);
						}

						break;
					case MatroskaID.TimeCodeScale:
						time_scale = child.ReadULong();
						if (duration_unscaled > 0)
						{
							duration = TimeSpan.FromMilliseconds(duration_unscaled * time_scale / 1000000);
						}

						break;
					case MatroskaID.Title:
						break;
				}

				i += child.Size;
			}
		}


		private ulong ReadLeadText()
		{
			ulong offset = 0;

			// look up the 0x1A start byte
			const int buffer_size = 64;
			ByteVector leadtxt;
			int idx;
			do
			{
				Seek((long)offset);
				leadtxt = ReadBlock(buffer_size);
				idx = leadtxt.IndexOf(0x1A);
				offset += buffer_size;
			} while (idx < 0 && offset < (ulong)Length);

			if (idx < 0)
			{
				throw new Exception("Invalid Matroska file, missing data 0x1A.");
			}

			return offset + (ulong)idx - buffer_size;
		}


		private void ReadHeader(EBMLreader element)
		{
			string doctype = null;
			ulong i = 0;

			while (i < element.DataSize)
			{
				var child = new EBMLreader(element, element.DataOffset + i);

				var ebml_id = (EBMLID)child.ID;

				switch (ebml_id)
				{
					case EBMLID.EBMLDocType:
						doctype = child.ReadString();
						break;
				}

				i += child.Size;
			}

			// Check DocType
			if (string.IsNullOrEmpty(doctype) || doctype != "matroska" && doctype != "webm")
			{
				throw new UnsupportedFormatException("DocType is not matroska or webm");
			}
		}


		private List<EBMLreader> ReadSegments(EBMLreader element, bool allowSeekHead)
		{
			var segm_list = new List<EBMLreader>(10);

			var foundCluster = false; // find first Cluster

			ulong i = 0;

			while (i < element.DataSize)
			{
				EBMLreader child;

				try
				{
					child = new EBMLreader(element, element.DataOffset + i);
				}
				catch
				{
					MarkAsCorrupt("Truncated file or invalid EBML entry");
					break; // Corrupted file: quit here and good luck for the rest
				}

				var matroska_id = child.ID;
				var refInSeekHead = false;

				switch (matroska_id)
				{
					case MatroskaID.SeekHead:
						if (allowSeekHead)
						{
							// Take only the first SeekHead into account
							var ebml_seek = new List<EBMLreader>(10) { child };
							if (ReadSeekHead(child, ebml_seek))
							{
								// Always reference the first element
								if (ebml_seek[0]
									.Offset > element.DataOffset)
								{
									ebml_seek.Insert(0, segm_list[0]);
								}

								segm_list = ebml_seek;
								i = element.DataSize; // Exit the loop: we got what we need
							}
							else
							{
								MarkAsCorrupt("Invalid Meta Seek");
								refInSeekHead = true;
							}
						}
						else
						{
							refInSeekHead = true;
						}

						break;

					case MatroskaID.Void: // extend SeekHead space to following void
						break;

					case MatroskaID.Cluster: // reference first Cluster only (too many)
						refInSeekHead = !foundCluster;
						foundCluster = true;
						break;

					// Reference the following elements
					case MatroskaID.Cues:
					case MatroskaID.Tracks:
					case MatroskaID.SegmentInfo:
					case MatroskaID.Tags:
					case MatroskaID.Attachments:
					default:
						refInSeekHead = true;
						break;
				}

				i += child.Size;

				if (refInSeekHead || i == 0)
				{
					segm_list.Add(child);
				}
			}

			return segm_list;
		}


		private bool ReadSeekHead(EBMLreader element, List<EBMLreader> segm_list)
		{
			MatroskaID ebml_id = 0;
			ulong ebml_position = 0;

			ulong i = 0;
			while (i < element.DataSize)
			{
				var ebml_seek = new EBMLreader(element, element.DataOffset + i);
				var matroska_id = ebml_seek.ID;

				if (matroska_id != MatroskaID.Seek)
				{
					return false; // corrupted SeekHead
				}

				ulong j = 0;
				while (j < ebml_seek.DataSize)
				{
					var child = new EBMLreader(ebml_seek, ebml_seek.DataOffset + j);
					matroska_id = child.ID;

					switch (matroska_id)
					{
						case MatroskaID.SeekID:
							ebml_id = (MatroskaID)child.ReadULong();
							break;
						case MatroskaID.SeekPosition:
							ebml_position = child.ReadULong() + element.Offset;
							break;
					}

					j += child.Size;
				}

				if (ebml_id > 0 && ebml_position > 0)
				{
					// Create abstract EBML representation of the segment EBML
					var ebml = new EBMLreader(element.Parent, ebml_position, ebml_id);

					// Sort the seek-entries by increasing position order
					int k;
					for (k = segm_list.Count - 1; k >= 0; k--)
					{
						if (ebml_position > segm_list[k]
							.Offset)
						{
							break;
						}
					}

					segm_list.Insert(k + 1, ebml);

					// Chained SeekHead recursive read
					if (ebml_id == MatroskaID.SeekHead)
					{
						if (!ebml.Read())
						{
							return false; // Corrupted
						}

						ReadSeekHead(ebml, segm_list);
					}
				}

				i += ebml_seek.Size;
			}

			return true;
		}


		private void ReadAttachments(EBMLreader element, ReadStyle propertiesStyle)
		{
			ulong i = 0;

			while (i < (ulong)(long)element.DataSize)
			{
				var child = new EBMLreader(element, element.DataOffset + i);

				var matroska_id = child.ID;

				switch (matroska_id)
				{
					case MatroskaID.AttachedFile:
						ReadAttachedFile(child, propertiesStyle);
						break;
				}

				i += child.Size;
			}
		}


		private void ReadAttachedFile(EBMLreader element, ReadStyle propertiesStyle)
		{
			ulong i = 0;
#pragma warning restore 219

			while (i < element.DataSize)
			{
				var child = new EBMLreader(element, element.DataOffset + i);

				var matroska_id = child.ID;

				switch (matroska_id)
				{
					case MatroskaID.FileName:
#pragma warning disable 219 // Assigned, never read
						var file_name = child.ReadString();
						break;
					case MatroskaID.FileMimeType:
						var file_mime = child.ReadString();
						break;
					case MatroskaID.FileDescription:
						var file_desc = child.ReadString();
						break;
					case MatroskaID.FileData:
						break;
					case MatroskaID.FileUID:
						var file_uid = child.ReadULong();
						break;
				}

				i += child.Size;
			}
		}


		private void ReadTracks(EBMLreader element)
		{
			ulong i = 0;

			while (i < element.DataSize)
			{
				var child = new EBMLreader(element, element.DataOffset + i);

				var matroska_id = child.ID;

				switch (matroska_id)
				{
					case MatroskaID.TrackEntry:
						ReadTrackEntry(child);
						break;
				}

				i += child.Size;
			}
		}


		private void ReadTrackEntry(EBMLreader element)
		{
			ulong i = 0;

			while (i < element.DataSize)
			{
				var child = new EBMLreader(element, element.DataOffset + i);

				var matroska_id = child.ID;

				switch (matroska_id)
				{
					case MatroskaID.TrackType:
					{
						var track_type = (TrackType)child.ReadULong();

						switch (track_type)
						{
							case TrackType.Video:
							{
								var track = new VideoTrack(this, element);

								tracks.Add(track);
								break;
							}
							case TrackType.Audio:
							{
								var track = new AudioTrack(this, element);

								tracks.Add(track);
								break;
							}
						}

						break;
					}
				}

				i += child.Size;
			}
		}
	}
}
