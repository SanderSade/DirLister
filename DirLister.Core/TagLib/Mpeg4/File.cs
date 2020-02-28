using System;
using System.Collections.Generic;
using Sander.DirLister.Core.TagLib.Mpeg4.Boxes;

namespace Sander.DirLister.Core.TagLib.Mpeg4
{
	/// <summary>
	///     This class extends <see cref="TagLib.File" /> to provide tagging
	///     and properties support for MPEG-4 files.
	/// </summary>
	[SupportedMimeType("taglib/m4a", "m4a")]
	[SupportedMimeType("taglib/m4b", "m4b")]
	[SupportedMimeType("taglib/m4v", "m4v")]
	[SupportedMimeType("taglib/m4p", "m4p")]
	[SupportedMimeType("taglib/mp4", "mp4")]
	public sealed class File : TagLib.File
	{
		/// <summary>
		///     Contains the ISO user data boxes.
		/// </summary>
		private readonly List<IsoUserDataBox> udta_boxes = new List<IsoUserDataBox>();

		/// <summary>
		///     Contains the media properties.
		/// </summary>
		private Properties properties;


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
			: base(path)
		{
			Read(propertiesStyle);
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
		public File(string path) : this(path, ReadStyle.Average)
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
			Read(propertiesStyle);
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
		public override Properties Properties => properties;


		/// <summary>
		///     Reads the file with a specified read style.
		/// </summary>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying at what level
		///     of accuracy to read the media properties, or
		///     <see
		///         cref="ReadStyle.None" />
		///     to ignore the properties.
		/// </param>
		private void Read(ReadStyle propertiesStyle)
		{
			// TODO: Support Id3v2 boxes!!!
			Mode = AccessMode.Read;
			try
			{
				var parser = new FileParser(this);

				if ((propertiesStyle & ReadStyle.Average) == 0)
				{
					parser.ParseTag();
				}
				else
				{
					parser.ParseTagAndProperties();
				}

				InvariantStartPosition = parser.MdatStartPosition;
				InvariantEndPosition = parser.MdatEndPosition;

				udta_boxes.AddRange(parser.UserDataBoxes);

				// Ensure our collection contains at least a single empty box
				if (udta_boxes.Count == 0)
				{
					var dummy = new IsoUserDataBox();
					udta_boxes.Add(dummy);
				}

				// If we're not reading properties, we're done.
				if ((propertiesStyle & ReadStyle.Average) == 0)
				{
					Mode = AccessMode.Closed;
					return;
				}

				// Get the movie header box.
				var mvhd_box = parser.MovieHeaderBox;
				if (mvhd_box == null)
				{
					Mode = AccessMode.Closed;
					throw new CorruptFileException(
						"mvhd box not found.");
				}

				var audio_sample_entry =
					parser.AudioSampleEntry;

				var visual_sample_entry =
					parser.VisualSampleEntry;

				// Read the properties.
				properties = new Properties(mvhd_box.Duration,
					audio_sample_entry, visual_sample_entry);
			}
			finally
			{
				Mode = AccessMode.Closed;
			}
		}
	}
}
