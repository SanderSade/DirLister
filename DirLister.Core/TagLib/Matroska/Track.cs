using System;
using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Matroska
{
	/// <summary>
	///     Describes a Matroska Track.
	/// </summary>
	public class Track : ICodec, IUIDElement
	{
		private ulong _UID = UIDElement.GenUID();


		/// <summary>
		///     Constructs a <see cref="Track" /> parsing from provided
		///     file data.
		///     Parsing will be done reading from _file at position references by
		///     parent element's data section.
		/// </summary>
		/// <param name="_file"><see cref="File" /> instance to read from.</param>
		/// <param name="element">Parent <see cref="EBMLreader" />.</param>
		public Track(File _file, EBMLreader element)
		{
			ulong i = 0;

			while (i < element.DataSize)
			{
				var child = new EBMLreader(_file, element.DataOffset + i);

				var matroska_id = child.ID;

				switch (matroska_id)
				{
					case MatroskaID.TrackNumber:
						track_number = child.ReadULong();
						break;
					case MatroskaID.TrackUID:
						_UID = child.ReadULong();
						break;
					case MatroskaID.CodecID:
						track_codec_id = child.ReadString();
						break;
					case MatroskaID.CodecName:
						track_codec_name = child.ReadString();
						break;
					case MatroskaID.TrackName:
						track_name = child.ReadString();
						break;
					case MatroskaID.TrackLanguage:
						track_language = child.ReadString();
						break;
					case MatroskaID.TrackFlagEnabled:
						track_enabled = child.ReadBool();
						break;
					case MatroskaID.TrackFlagDefault:
						track_default = child.ReadBool();
						break;
					case MatroskaID.CodecPrivate:
						codec_data = child.ReadBytes();
						break;
					default:
						UnknownElements.Add(child);
						break;
				}

				i += child.Size;
			}
		}


		/// <summary>
		///     List of unknown elements encountered while parsing.
		/// </summary>
		public List<EBMLreader> UnknownElements { get; } = new List<EBMLreader>();

		/// <summary>
		///     Track description.
		/// </summary>
		public virtual string Description => string.Format("{0} {1}", track_codec_name, track_language);

		/// <summary>
		///     Describes track duration.
		/// </summary>
		public virtual TimeSpan Duration => TimeSpan.Zero;

		/// <summary>
		///     Describes track media types.
		/// </summary>
		public virtual MediaTypes MediaTypes => MediaTypes.None;

		/// <summary>
		///     Unique ID representing the element, as random as possible (setting zero will generate automatically a new one).
		/// </summary>
		public ulong UID
		{
			get => _UID;
			set => _UID = UIDElement.GenUID(value);
		}

		/// <summary>
		///     Get the Tag type the UID should be represented by, or 0 if undefined
		/// </summary>
		public MatroskaID UIDType => MatroskaID.TagTrackUID;
#pragma warning disable 414 // Assigned, never used
		private readonly ulong track_number;
		private readonly string track_codec_id;
		private readonly string track_codec_name;
		private readonly string track_name;
		private readonly string track_language;
		private readonly bool track_enabled;
		private readonly bool track_default;
		private readonly ByteVector codec_data;
#pragma warning restore 414
	}
}
