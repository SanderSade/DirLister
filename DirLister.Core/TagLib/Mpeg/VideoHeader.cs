using System;

namespace Sander.DirLister.Core.TagLib.Mpeg
{
	/// <summary>
	///     This structure implements <see cref="IVideoCodec" /> and provides
	///     information about an MPEG video stream.
	/// </summary>
	public struct VideoHeader : IVideoCodec
	{
		/// <summary>
		///     Contains frame rate values.
		/// </summary>
		private static readonly double[] frame_rates = new double[9] { 0, 24000d / 1001d, 24, 25, 30000d / 1001d, 30, 50, 60000d / 1001d, 60 };

		/// <summary>
		///     Contains the video height.
		/// </summary>
		private readonly int height;

		/// <summary>
		///     Contains the index in <see cref="frame_rates" /> of the
		///     video frame rate.
		/// </summary>
		private readonly int frame_rate_index;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="VideoHeader" />
		///     by reading it from a specified
		///     location in a specified file.
		/// </summary>
		/// <param name="file">
		///     A <see cref="TagLib.File" /> object to read from.
		/// </param>
		/// <param name="position">
		///     A <see cref="long" /> value indicating the position in
		///     <paramref name="file" /> at which the header begins.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///     Insufficient data could be read for the header.
		/// </exception>
		public VideoHeader(TagLib.File file, long position)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}

			file.Seek(position);
			var data = file.ReadBlock(7);

			if (data.Count < 7)
			{
				throw new CorruptFileException(
					"Insufficient data in header.");
			}

			VideoWidth = data.Mid(0, 2)
				.ToUShort() >> 4;

			height = data.Mid(1, 2)
				.ToUShort() & 0x0FFF;

			frame_rate_index = data[3] & 0x0F;
			VideoBitrate = (int)((data.Mid(4, 3)
				                     .ToUInt() >> 6) &
			                     0x3FFFF);
		}


		/// <summary>
		///     Gets the duration of the media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     Always <see cref="TimeSpan.Zero" />.
		/// </value>
		public TimeSpan Duration => TimeSpan.Zero;

		/// <summary>
		///     Gets the types of media represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     Always <see cref="MediaTypes.Video" />.
		/// </value>
		public MediaTypes MediaTypes => MediaTypes.Video;

		/// <summary>
		///     Gets a text description of the media represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="string" /> object containing a description
		///     of the media represented by the current instance.
		/// </value>
		public string Description => "MPEG Video";

		/// <summary>
		///     Gets the width of the video represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the width of the
		///     video represented by the current instance.
		/// </value>
		public int VideoWidth { get; }

		/// <summary>
		///     Gets the height of the video represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing the height of the
		///     video represented by the current instance.
		/// </value>
		public int VideoHeight => height;

		/// <summary>
		///     Gets the frame rate of the video represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="double" /> value containing the frame rate
		///     of the video represented by the current instance.
		/// </value>
		public double VideoFrameRate => frame_rate_index < 9 ? frame_rates[frame_rate_index] : 0;

		/// <summary>
		///     Gets the bitrate of the video represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="int" /> value containing a bitrate of the
		///     video represented by the current instance.
		/// </value>
		public int VideoBitrate { get; }
	}
}
