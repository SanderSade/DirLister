using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///    This class extends <see cref="IsoSampleEntry" /> and implements
	///    <see cref="IAudioCodec" /> to provide an implementation of a
	///    ISO/IEC 14496-12 AudioSampleEntry and support for reading MPEG-4
	///    video properties.
	/// </summary>
	public class IsoAudioSampleEntry : IsoSampleEntry, IAudioCodec
	{
		/// <summary>
		///    Contains the channel count.
		/// </summary>
		private readonly ushort channel_count;

		/// <summary>
		///    Contains the children of the box.
		/// </summary>
		private readonly IEnumerable<Box> children;

		/// <summary>
		///    Contains the sample rate.
		/// </summary>
		private readonly uint sample_rate;

		/// <summary>
		///    Contains the sample size.
		/// </summary>
		private readonly ushort sample_size;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="IsoVisualSampleEntry" /> with a provided header and
		///    handler by reading the contents from a specified file.
		/// </summary>
		/// <param name="header">
		///    A <see cref="BoxHeader" /> object containing the header
		///    to use for the new instance.
		/// </param>
		/// <param name="file">
		///    A <see cref="TagLib.File" /> object to read the contents
		///    of the box from.
		/// </param>
		/// <param name="handler">
		///    A <see cref="IsoHandlerBox" /> object containing the
		///    handler that applies to the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		public IsoAudioSampleEntry(BoxHeader header, TagLib.File file,
			IsoHandlerBox handler)
			: base(header, file, handler)
		{
			if (file == null)
				throw new ArgumentNullException("file");

			file.Seek(base.DataPosition + 8);
			channel_count = file.ReadBlock(2)
			                    .ToUShort();
			sample_size = file.ReadBlock(2)
			                  .ToUShort();
			file.Seek(base.DataPosition + 16);
			sample_rate = file.ReadBlock(4)
			                  .ToUInt();
			children = LoadChildren(file);
		}


		/// <summary>
		///    Gets the position of the data contained in the current
		///    instance, after any box specific headers.
		/// </summary>
		/// <value>
		///    A <see cref="long" /> value containing the position of
		///    the data contained in the current instance.
		/// </value>
		protected override long DataPosition => base.DataPosition + 20;

		/// <summary>
		///    Gets the children of the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="T:System.Collections.Generic.IEnumerable`1" /> object enumerating the
		///    children of the current instance.
		/// </value>
		public override IEnumerable<Box> Children => children;

		/// <summary>
		///    Gets a text description of the media represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="string" /> object containing a description
		///    of the media represented by the current instance.
		/// </value>
		public string Description => string.Format(
			CultureInfo.InvariantCulture,
			"MPEG-4 Audio ({0})", BoxType);

		/// <summary>
		///    Gets the sample size of the audio represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the sample size of
		///    the audio represented by the current instance.
		/// </value>
		public int AudioSampleSize => sample_size;

		/// <summary>
		///    Gets the duration of the media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    Always <see cref="TimeSpan.Zero" />.
		/// </value>
		public TimeSpan Duration => TimeSpan.Zero;

		/// <summary>
		///    Gets the types of media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    Always <see cref="MediaTypes.Video" />.
		/// </value>
		public MediaTypes MediaTypes => MediaTypes.Audio;

		/// <summary>
		///    Gets the bitrate of the audio represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing a bitrate of the
		///    audio represented by the current instance.
		/// </value>
		public int AudioBitrate
		{
			get
			{
				// If we don't have an stream descriptor, we
				// don't know what's what.
				if (!(GetChildRecursively("esds") is AppleElementaryStreamDescriptor esds))
					return 0;

				// Return from the elementary stream descriptor.
				return (int)esds.AverageBitrate;
			}
		}

		/// <summary>
		///    Gets the sample rate of the audio represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the sample rate of
		///    the audio represented by the current instance.
		/// </value>
		public int AudioSampleRate => (int)(sample_rate >> 16);

		/// <summary>
		///    Gets the number of channels in the audio represented by
		///    the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the number of
		///    channels in the audio represented by the current
		///    instance.
		/// </value>
		public int AudioChannels => channel_count;
	}
}
