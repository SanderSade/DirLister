using System;

namespace Sander.DirLister.Core.TagLib.Mpeg
{
	/// <summary>
	///    This structure provides information about a variable bitrate MPEG
	///    audio stream.
	/// </summary>
	public struct XingHeader
	{

		/// <summary>
		///    Contains te Xing identifier.
		/// </summary>
		/// <value>
		///    "Xing"
		/// </value>
		public static readonly ReadOnlyByteVector FileIdentifier = "Xing";

		/// <summary>
		///    An empty and unset Xing header.
		/// </summary>
		public static readonly XingHeader Unknown = new XingHeader(0, 0);


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="XingHeader" /> with a specified frame count and
		///    size.
		/// </summary>
		/// <param name="frame">
		///    A <see cref="uint" /> value specifying the frame count of
		///    the audio represented by the new instance.
		/// </param>
		/// <param name="size">
		///    A <see cref="uint" /> value specifying the stream size of
		///    the audio represented by the new instance.
		/// </param>
		private XingHeader(uint frame, uint size)
		{
			TotalFrames = frame;
			TotalSize = size;
			Present = false;
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="XingHeader" /> by reading its raw contents.
		/// </summary>
		/// <param name="data">
		///    A <see cref="ByteVector" /> object containing the raw
		///    Xing header.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="data" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///    <paramref name="data" /> does not start with <see
		///    cref="FileIdentifier" />.
		/// </exception>
		public XingHeader(ByteVector data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			// Check to see if a valid Xing header is available.
			if (!data.StartsWith(FileIdentifier))
				throw new CorruptFileException(
					"Not a valid Xing header");

			var position = 8;

			if ((data[7] & 0x01) != 0)
			{
				TotalFrames = data.Mid(position, 4)
				             .ToUInt();
				position += 4;
			}
			else
				TotalFrames = 0;

			if ((data[7] & 0x02) != 0)
			{
				TotalSize = data.Mid(position, 4)
				           .ToUInt();
				position += 4;
			}
			else
				TotalSize = 0;

			Present = true;
		}


		/// <summary>
		///    Gets the total number of frames in the file, as indicated
		///    by the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="uint" /> value containing the number of
		///    frames in the file, or <c>0</c> if not specified.
		/// </value>
		public uint TotalFrames { get; }

		/// <summary>
		///    Gets the total size of the file, as indicated by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="uint" /> value containing the total size of
		///    the file, or <c>0</c> if not specified.
		/// </value>
		public uint TotalSize { get; }

		/// <summary>
		///    Gets whether or not a physical Xing header is present in
		///    the file.
		/// </summary>
		/// <value>
		///    A <see cref="bool" /> value indicating whether or not the
		///    current instance represents a physical Xing header.
		/// </value>
		public bool Present { get; }


		/// <summary>
		///    Gets the offset at which a Xing header would appear in an
		///    MPEG audio packet based on the version and channel mode.
		/// </summary>
		/// <param name="version">
		///    A <see cref="Version" /> value specifying the version of
		///    the MPEG audio packet.
		/// </param>
		/// <param name="channelMode">
		///    A <see cref="ChannelMode" /> value specifying the channel
		///    mode of the MPEG audio packet.
		/// </param>
		/// <returns>
		///    A <see cref="int" /> value indicating the offset in an
		///    MPEG audio packet at which the Xing header would appear.
		/// </returns>
		public static int XingHeaderOffset(Version version,
			ChannelMode channelMode)
		{
			var single_channel =
				channelMode == ChannelMode.SingleChannel;

			if (version == Version.Version1)
				return single_channel ? 0x15 : 0x24;
			return single_channel ? 0x0D : 0x15;
		}
	}
}
