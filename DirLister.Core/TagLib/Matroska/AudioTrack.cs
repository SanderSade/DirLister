using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Matroska
{
	/// <summary>
	/// Describes a Matroska Audio track.
	/// </summary>
	public class AudioTrack : Track, IAudioCodec
	{
#pragma warning disable 414 // Assigned, never used
		private readonly double rate;
		private readonly ulong channels;
		private readonly ulong depth;
#pragma warning restore 414


		/// <summary>
		///  Construct a <see cref="AudioTrack" /> reading information from 
		///  provided file data.
		/// Parsing will be done reading from _file at position references by 
		/// parent element's data section.
		/// </summary>
		/// <param name="_file"><see cref="File" /> instance to read from.</param>
		/// <param name="element">Parent <see cref="EBMLreader" />.</param>
		public AudioTrack(File _file, EBMLreader element)
			: base(_file, element)
		{
			MatroskaID matroska_id;

			// Here we handle the unknown elements we know, and store the rest
			foreach (var elem in base.UnknownElements)
			{
				matroska_id = elem.ID;

				switch (matroska_id)
				{
					case MatroskaID.TrackAudio:
					{
						ulong i = 0;

						while (i < elem.DataSize)
						{
							var child = new EBMLreader(_file, elem.DataOffset + i);

							matroska_id = child.ID;

							switch (matroska_id)
							{
								case MatroskaID.AudioChannels:
									channels = child.ReadULong();
									break;
								case MatroskaID.AudioBitDepth:
									depth = child.ReadULong();
									break;
								case MatroskaID.AudioSamplingFreq:
									rate = child.ReadDouble();
									break;
								default:
									UnknownElements.Add(child);
									break;
							}

							i += child.Size;
						}

						break;
					}

					default:
						UnknownElements.Add(elem);
						break;
				}
			}
		}


		/// <summary>
		/// List of unknown elements encountered while parsing.
		/// </summary>
		public new List<EBMLreader> UnknownElements { get; } = new List<EBMLreader>();

		/// <summary>
		/// This type of track only has audio media type.
		/// </summary>
		public override MediaTypes MediaTypes => MediaTypes.Audio;

		/// <summary>
		/// Audio track bitrate.
		/// </summary>
		public int AudioBitrate => 0;

		/// <summary>
		/// Audio track sampling rate.
		/// </summary>
		public int AudioSampleRate => (int)rate;

		/// <summary>
		/// Number of audio channels in this track.
		/// </summary>
		public int AudioChannels => (int)channels;
	}
}
