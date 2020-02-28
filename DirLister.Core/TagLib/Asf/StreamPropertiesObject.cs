using System;
using Sander.DirLister.Core.TagLib.Riff;

namespace Sander.DirLister.Core.TagLib.Asf
{
	/// <summary>
	///     This class extends <see cref="Object" /> to provide a
	///     representation of an ASF Stream Properties object which can be
	///     read from and written to disk.
	/// </summary>
	public class StreamPropertiesObject : Object
	{
		/// <summary>
		///     Contains the reserved data.
		/// </summary>
		private readonly uint reserved;

		/// <summary>
		///     Contains the stream type GUID.
		/// </summary>
		private readonly System.Guid stream_type;

		/// <summary>
		///     Contains the time offset of the stream.
		/// </summary>
		private readonly ulong time_offset;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="PaddingObject" />
		///     by reading the contents from a
		///     specified position in a specified file.
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
		public StreamPropertiesObject(File file, long position)
			: base(file, position)
		{
			if (!Guid.Equals(Asf.Guid.AsfStreamPropertiesObject))
			{
				throw new CorruptFileException(
					"Object GUID incorrect.");
			}

			if (OriginalSize < 78)
			{
				throw new CorruptFileException(
					"Object size too small.");
			}

			stream_type = file.ReadGuid();
			ErrorCorrectionType = file.ReadGuid();
			time_offset = file.ReadQWord();

			var type_specific_data_length = (int)file.ReadDWord();
			var error_correction_data_length = (int)
				file.ReadDWord();

			Flags = file.ReadWord();
			reserved = file.ReadDWord();
			TypeSpecificData =
				file.ReadBlock(type_specific_data_length);

			ErrorCorrectionData =
				file.ReadBlock(error_correction_data_length);
		}


		/// <summary>
		///     Gets the codec information contained in the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="ICodec" /> object containing the codec
		///     information read from <see cref="TypeSpecificData" /> or
		///     <see langword="null" /> if the data could not be decoded.
		/// </value>
		public ICodec Codec
		{
			get
			{
				if (stream_type == Asf.Guid.AsfAudioMedia)
				{
					return new WaveFormatEx(
						TypeSpecificData, 0);
				}

				if (stream_type == Asf.Guid.AsfVideoMedia)
				{
					return new BitmapInfoHeader(
						TypeSpecificData, 11);
				}

				return null;
			}
		}

		/// <summary>
		///     Gets the stream type GUID of the current instance.
		/// </summary>
		/// <summary>
		///     A <see cref="System.Guid" /> object containing the stream
		///     type GUID of the current instance.
		/// </summary>
		public System.Guid StreamType => stream_type;

		/// <summary>
		///     Gets the error correction type GUID of the current
		///     instance.
		/// </summary>
		/// <summary>
		///     A <see cref="System.Guid" /> object containing the error
		///     correction type GUID of the current instance.
		/// </summary>
		public System.Guid ErrorCorrectionType { get; }

		/// <summary>
		///     Gets the time offset at which the stream described by the
		///     current instance begins.
		/// </summary>
		/// <value>
		///     A <see cref="TimeSpan" /> value containing the time
		///     offset at which the stream described by the current
		///     instance begins.
		/// </value>
		public TimeSpan TimeOffset => new TimeSpan((long)time_offset);

		/// <summary>
		///     Gets the flags that apply to the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="ushort" /> value containing the flags that
		///     apply to the current instance.
		/// </value>
		public ushort Flags { get; }

		/// <summary>
		///     Gets the type specific data contained in the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="ByteVector" /> object containing the type
		///     specific data contained in the current instance.
		/// </value>
		/// <remarks>
		///     The contents of this value are dependant on the type
		///     contained in <see cref="StreamType" />.
		/// </remarks>
		public ByteVector TypeSpecificData { get; }

		/// <summary>
		///     Gets the error correction data contained in the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="ByteVector" /> object containing the error
		///     correction data contained in the current instance.
		/// </value>
		/// <remarks>
		///     The contents of this value are dependant on the type
		///     contained in <see cref="ErrorCorrectionType" />.
		/// </remarks>
		public ByteVector ErrorCorrectionData { get; }


		/// <summary>
		///     Renders the current instance as a raw ASF object.
		/// </summary>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the
		///     rendered version of the current instance.
		/// </returns>
		public override ByteVector Render()
		{
			ByteVector output = stream_type.ToByteArray();
			output.Add(ErrorCorrectionType.ToByteArray());
			output.Add(RenderQWord(time_offset));
			output.Add(RenderDWord((uint)
				TypeSpecificData.Count));

			output.Add(RenderDWord((uint)
				ErrorCorrectionData.Count));

			output.Add(RenderWord(Flags));
			output.Add(RenderDWord(reserved));
			output.Add(TypeSpecificData);
			output.Add(ErrorCorrectionData);

			return Render(output);
		}
	}
}
