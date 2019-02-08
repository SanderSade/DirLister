using System;
using System.Collections.Generic;
using Sander.DirLister.Core.TagLib.Ogg.Codecs;

namespace Sander.DirLister.Core.TagLib.Ogg
{
	/// <summary>
	///    This abstract class implements <see cref="ICodec" /> to provide
	///    support for processing packets from an Ogg logical bitstream.
	/// </summary>
	/// <remarks>
	///    Unsupported Ogg codecs can be added by creating child classes and
	///    registering them using <see cref="AddCodecProvider" />.
	/// </remarks>
	/// <seealso cref="CodecProvider" />
	/// <seealso cref="AddCodecProvider" />
	public abstract class Codec : ICodec
	{
		/// <summary>
		///    Represents a method capable of checking an Ogg header
		///    packet to see it is matches a given codec.
		/// </summary>
		/// <param name="packet">
		///    A <see cref="ByteVector" /> object containing an Ogg
		///    header packet.
		/// </param>
		/// <returns>
		///    A <see cref="Codec" /> object capable of processing the
		///    stream from which the packet was generated; otherwise
		///    <see langword="null" />.
		/// </returns>
		/// <remarks>
		///    <para>In order to add support for a new Ogg codec in
		///    TagLib#, a derivative class of <see cref="Codec" /> needs
		///    to be created. The class then needs to be added to the
		///    codec detection system by creating a <see
		///    cref="CodecProvider" /> and registering it with <see
		///    cref="AddCodecProvider"/>.</para>
		///    <para>A method implementing <see cref="CodecProvider" />
		///    should read <paramref name="packet" /> to determine if
		///    it's subclass can handle the data. If it can, it should
		///    return a new instance of that class, but in no way act
		///    upon the data. If the class cannot be used to read the
		///    packet, <see langref="null" /> indicates to the system
		///    that it needs to try anther codec provider.</para>
		/// </remarks>
		/// <example>
		///    <para>The following example would check for a Speex
		///    packet and return a Speex codec:</para>
		///    <code lang="C++">
		/// Codec.AddCodecProvider (delegate (ByteVector packet) {
		/// 	return packet.StartsWith ("Speex   ") ? new MySpeexCodec () : null;
		/// });
		///    </code>
		/// </example>
		public delegate Codec CodecProvider(ByteVector packet);

		/// <summary>
		///    Contains registered codec providers.
		/// </summary>
		private static readonly List<CodecProvider> providers =
			new List<CodecProvider>();

		/// <summary>
		///    Gets the raw Xiph comment data contained in the codec.
		/// </summary>
		/// <value>
		///    A <see cref="ByteVector" /> object containing a raw Xiph
		///    comment or <see langword="null"/> if none was found.
		/// </value>
		public abstract ByteVector CommentData { get; }

		/// <summary>
		///    Gets the types of media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    A bitwise combined <see cref="MediaTypes" /> containing
		///    the types of media represented by the current instance.
		/// </value>
		public abstract MediaTypes MediaTypes { get; }

		/// <summary>
		///    Gets the duration of the media represented by the current
		///    instance.
		/// </summary>
		/// <value>
		///    Always <see cref="TimeSpan.Zero" />.
		/// </value>
		/// <remarks>
		///    In order to determine the duration of an Ogg stream, the
		///    first and last granular positions will be passed to <see
		///    cref="GetDuration" />.
		/// </remarks>
		public TimeSpan Duration => TimeSpan.Zero;


		/// <summary>
		///    Determines the correct codec to use for a stream header
		///    packet.
		/// </summary>
		/// <param name="packet">
		///    A <see cref="ByteVector" /> object containing the first
		///    packet of an Ogg logical bitstream.
		/// </param>
		/// <returns>
		///    A <see cref="Codec" /> object capable of handling
		///    <paramref name="packet" /> and subsequent packets from
		///    the same stream.
		/// </returns>
		/// <exception cref="UnsupportedFormatException">
		///    No registered codec capable of processing <paramref
		///    name="packet" /> could be found.
		/// </exception>
		/// <remarks>
		///    This method will first use <see cref="CodecProvider" />
		///    delegates registered with <see cref="AddCodecProvider" />
		///    and then attempt to use the built-in codecs.
		/// </remarks>
		public static Codec GetCodec(ByteVector packet)
		{
			Codec c = null;

			foreach (var p in providers)
			{
				c = p(packet);
				if (c != null) return c;
			}

			c = Vorbis.FromPacket(packet);
			if (c != null)
				return c;

			c = Theora.FromPacket(packet);
			if (c != null)
				return c;

			c = Opus.FromPacket(packet);
			if (c != null)
				return c;

			throw new UnsupportedFormatException("Unknown codec.");
		}


		/// <summary>
		///    Adds a codec
		/// </summary>
		/// <param name="provider">
		/// A <see cref="CodecProvider"/>
		/// </param>
		/// <remarks>
		///    A <see cref="CodecProvider" /> delegate is used to add
		///    support for new <see cref="Codec" /> subclasses in <see
		///    cref="GetCodec" />.
		/// </remarks>
		/// <seealso cref="CodecProvider" />
		public static void AddCodecProvider(CodecProvider provider)
		{
			providers.Insert(0, provider);
		}


		/// <summary>
		///    Reads a Ogg packet that has been encountered in the
		///    stream.
		/// </summary>
		/// <param name="packet">
		///    A <see cref="ByteVector" /> object containing a packet to
		///    be read by the current instance.
		/// </param>
		/// <param name="index">
		///    A <see cref="int" /> value containing the index of the
		///    packet in the stream.
		/// </param>
		/// <returns>
		///    <see langword="true" /> if the codec has read all the
		///    necessary packets for the stream and does not need to be
		///    called again, typically once the Xiph comment has been
		///    found. Otherwise <see langword="false" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="packet" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///    <paramref name="index" /> is less than zero.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///    The data does not conform to the specificiation for the
		///    codec represented by the current instance.
		/// </exception>
		public abstract bool ReadPacket(ByteVector packet, int index);


		/// <summary>
		///    Computes the duration of the stream using the first and
		///    last granular positions of the stream.
		/// </summary>
		/// <param name="firstGranularPosition">
		///    A <see cref="long" /> value containing the first granular
		///    position of the stream.
		/// </param>
		/// <param name="lastGranularPosition">
		///    A <see cref="long" /> value containing the last granular
		///    position of the stream.
		/// </param>
		/// <returns>
		///    A <see cref="TimeSpan" /> value containing the duration
		///    of the stream.
		/// </returns>
		public abstract TimeSpan GetDuration(long firstGranularPosition,
			long lastGranularPosition);
	}
}
