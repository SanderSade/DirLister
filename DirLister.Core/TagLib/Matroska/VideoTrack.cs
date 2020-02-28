using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Matroska
{
	/// <summary>
	///     Enumeration describing supported Video Aspect Ratio types.
	/// </summary>
	public enum VideoAspectRatioType
	{
		/// <summary>
		///     Free Aspect Ratio.
		/// </summary>
		AspectRatioModeFree = 0x0,

		/// <summary>
		///     Keep Aspect Ratio.
		/// </summary>
		AspectRatioModeKeep = 0x1,

		/// <summary>
		///     Fixed Aspect Ratio.
		/// </summary>
		AspectRatioModeFixed = 0x2
	}

	/// <summary>
	///     Describes a Matroska Video Track.
	/// </summary>
	public class VideoTrack : Track, IVideoCodec
	{
		/// <summary>
		///     Constructs a <see cref="VideoTrack" /> parsing from provided
		///     file data.
		///     Parsing will be done reading from _file at position references by
		///     parent element's data section.
		/// </summary>
		/// <param name="_file"><see cref="File" /> instance to read from.</param>
		/// <param name="element">Parent <see cref="EBMLreader" />.</param>
		public VideoTrack(File _file, EBMLreader element)
			: base(_file, element)
		{
			MatroskaID matroska_id;

			// Here we handle the unknown elements we know, and store the rest
			foreach (var elem in base.UnknownElements)
			{
				matroska_id = elem.ID;

				switch (matroska_id)
				{
					case MatroskaID.TrackVideo:
					{
						ulong i = 0;

						while (i < elem.DataSize)
						{
							var child = new EBMLreader(_file, elem.DataOffset + i);

							matroska_id = child.ID;

							switch (matroska_id)
							{
								case MatroskaID.VideoDisplayWidth:
									disp_width = child.ReadULong();
									break;
								case MatroskaID.VideoDisplayHeight:
									disp_height = child.ReadULong();
									break;
								case MatroskaID.VideoPixelWidth:
									width = child.ReadULong();
									break;
								case MatroskaID.VideoPixelHeight:
									height = child.ReadULong();
									break;
								case MatroskaID.VideoFrameRate:
									framerate = child.ReadDouble();
									break;
								case MatroskaID.VideoFlagInterlaced:
									interlaced = child.ReadBool();
									break;
								case MatroskaID.VideoAspectRatioType:
									ratio_type = (VideoAspectRatioType)child.ReadULong();
									break;
								case MatroskaID.VideoColourSpace:
									fourcc = child.ReadBytes();
									break;
								default:
									UnknownElements.Add(child);
									break;
							}

							i += child.Size;
						}

						break;
					}

					case MatroskaID.TrackDefaultDuration:
						var tmp = elem.ReadULong();
						framerate = 1000000000.0 / tmp;
						break;

					default:
						UnknownElements.Add(elem);
						break;
				}
			}
		}


		/// <summary>
		///     List of unknown elements encountered while parsing.
		/// </summary>
		public new List<EBMLreader> UnknownElements { get; } = new List<EBMLreader>();

		/// <summary>
		///     This type of track only has video media type.
		/// </summary>
		public override MediaTypes MediaTypes => MediaTypes.Video;

		/// <summary>
		///     Describes video track width in pixels.
		/// </summary>
		public int VideoWidth => (int)width;

		/// <summary>
		///     Describes video track height in pixels.
		/// </summary>
		public int VideoHeight => (int)height;
#pragma warning disable 414 // Assigned, never used
		private readonly ulong width;
		private readonly ulong height;
		private readonly ulong disp_width;
		private readonly ulong disp_height;
		private readonly double framerate;
		private readonly bool interlaced;
		private readonly VideoAspectRatioType ratio_type;
		private readonly ByteVector fourcc;
#pragma warning restore 414
	}
}
