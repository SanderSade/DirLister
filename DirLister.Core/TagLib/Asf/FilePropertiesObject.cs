using System;

namespace Sander.DirLister.Core.TagLib.Asf
{
	/// <summary>
	///     This class extends <see cref="Object" /> to provide a
	///     representation of an ASF File Properties object which can be read
	///     from and written to disk.
	/// </summary>
	public class FilePropertiesObject : Object
	{
		/// <summary>
		///     Contains the creation date.
		/// </summary>
		private readonly ulong creation_date;

		/// <summary>
		///     Contains the play duration.
		/// </summary>
		private readonly ulong play_duration;

		/// <summary>
		///     Contains the send duration.
		/// </summary>
		private readonly ulong send_duration;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="FilePropertiesObject" />
		///     by reading the contents
		///     from a specified position in a specified file.
		/// </summary>
		/// <param name="file">
		///     A <see cref="Asf.File" /> object containing the file from
		///     which the contents of the new instance are to be read.
		/// </param>
		/// <param name="position">
		///     A <see cref="long" /> value specify at what position to
		///     read the object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///     <paramref name="position" /> is less than zero or greater
		///     than the size of the file.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///     The object read from disk does not have the correct GUID
		///     or smaller than the minimum size.
		/// </exception>
		public FilePropertiesObject(File file, long position)
			: base(file, position)
		{
			if (!Guid.Equals(Asf.Guid.AsfFilePropertiesObject))
			{
				throw new CorruptFileException(
					"Object GUID incorrect.");
			}

			if (OriginalSize < 104)
			{
				throw new CorruptFileException(
					"Object size too small.");
			}

			FileId = file.ReadGuid();
			FileSize = file.ReadQWord();
			creation_date = file.ReadQWord();
			DataPacketsCount = file.ReadQWord();
			send_duration = file.ReadQWord();
			play_duration = file.ReadQWord();
			Preroll = file.ReadQWord();
			Flags = file.ReadDWord();
			MinimumDataPacketSize = file.ReadDWord();
			MaximumDataPacketSize = file.ReadDWord();
			MaximumBitrate = file.ReadDWord();
		}


		/// <summary>
		///     Gets the GUID for the file described by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="System.Guid" /> value containing the GUID
		///     for the file described by the current instance.
		/// </value>
		public System.Guid FileId { get; }

		/// <summary>
		///     Gets the size of the file described by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="ulong" /> value containing the size of the
		///     file described by the current instance.
		/// </value>
		public ulong FileSize { get; }

		/// <summary>
		///     Gets the creation date of the file described by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="DateTime" /> value containing the creation
		///     date of the file described by the current instance.
		/// </value>
		public DateTime CreationDate => new DateTime((long)creation_date);

		/// <summary>
		///     Gets the number of data packets in the file described by
		///     the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="ulong" /> value containing the number of
		///     data packets in the file described by the current
		///     instance.
		/// </value>
		public ulong DataPacketsCount { get; }

		/// <summary>
		///     Gets the play duration of the file described by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="TimeSpan" /> value containing the play
		///     duration of the file described by the current instance.
		/// </value>
		public TimeSpan PlayDuration => new TimeSpan((long)play_duration);

		/// <summary>
		///     Gets the send duration of the file described by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="TimeSpan" /> value containing the send
		///     duration of the file described by the current instance.
		/// </value>
		public TimeSpan SendDuration => new TimeSpan((long)send_duration);

		/// <summary>
		///     Gets the pre-roll of the file described by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="ulong" /> value containing the pre-roll of
		///     the file described by the current instance.
		/// </value>
		public ulong Preroll { get; }

		/// <summary>
		///     Gets the flags of the file described by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="uint" /> value containing the flags of the
		///     file described by the current instance.
		/// </value>
		public uint Flags { get; }

		/// <summary>
		///     Gets the minimum data packet size of the file described
		///     by the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="uint" /> value containing the minimum data
		///     packet size of the file described by the current
		///     instance.
		/// </value>
		public uint MinimumDataPacketSize { get; }

		/// <summary>
		///     Gets the maximum data packet size of the file described
		///     by the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="uint" /> value containing the maximum data
		///     packet size of the file described by the current
		///     instance.
		/// </value>
		public uint MaximumDataPacketSize { get; }

		/// <summary>
		///     Gets the maximum bitrate of the file described by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="uint" /> value containing the maximum
		///     bitrate of the file described by the current instance.
		/// </value>
		public uint MaximumBitrate { get; }


		/// <summary>
		///     Renders the current instance as a raw ASF object.
		/// </summary>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered version of the current instance.
		/// </returns>
		public override ByteVector Render()
		{
			ByteVector output = FileId.ToByteArray();
			output.Add(RenderQWord(FileSize));
			output.Add(RenderQWord(creation_date));
			output.Add(RenderQWord(DataPacketsCount));
			output.Add(RenderQWord(send_duration));
			output.Add(RenderQWord(play_duration));
			output.Add(RenderQWord(Preroll));
			output.Add(RenderDWord(Flags));
			output.Add(RenderDWord(MinimumDataPacketSize));
			output.Add(RenderDWord(MaximumDataPacketSize));
			output.Add(RenderDWord(MaximumBitrate));

			return Render(output);
		}
	}
}
