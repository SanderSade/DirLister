using System;

namespace Sander.DirLister.Core.TagLib.NonContainer
{
	/// <summary>
	///    This abstract class extends <see cref="TagLib.File" /> to provide
	///    tagging and properties for files that contain an indeterminite
	///    number of tags at their beginning or end.
	/// </summary>
	/// <remarks>
	///    <para>When extending this class, <see cref="ReadStart" />, <see
	///    cref="ReadEnd" />, and <see cref="ReadProperties" /> should be
	///    overrided methods that read the format specific information from
	///    the file.</para>
	///    <para>The file is read upon construction in the following
	///    manner:</para>
	///    <list type="number">
	///       <item><term>The file is opened for reading.</term></item>
	///       <item><term>The tags at the start of the file are
	///       read.</term></item>
	///       <item><term><see cref="ReadStart" /> is called.</term></item>
	///       <item><term>The tags at the end of the file are
	///       read.</term></item>
	///       <item><term><see cref="ReadEnd" /> is called.</term></item>
	///       <item><term>If reading with a style other than <see
	///       cref="ReadStyle.None" />, <see cref="ReadProperties" /> is
	///       called.</term></item>
	///       <item><term>The file is closed.</term></item>
	///    </list>
	/// </remarks>
	public abstract class File : TagLib.File
	{
		/// <summary>
		///    Contains the media properties.
		/// </summary>
		private Properties properties;


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified path in the local file
		///    system and specified read style.
		/// </summary>
		/// <param name="path">
		///    A <see cref="string" /> object containing the path of the
		///    file to use in the new instance.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		protected File(string path, ReadStyle propertiesStyle)
			: base(path)
		{
			Read(propertiesStyle);
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified path in the local file
		///    system with an average read style.
		/// </summary>
		/// <param name="path">
		///    A <see cref="string" /> object containing the path of the
		///    file to use in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		protected File(string path) : this(path, ReadStyle.Average)
		{
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified file abstraction and
		///    specified read style.
		/// </summary>
		/// <param name="abstraction">
		///    A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///    reading from and writing to the file.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="abstraction" /> is <see langword="null"
		///    />.
		/// </exception>
		protected File(IFileAbstraction abstraction,
			ReadStyle propertiesStyle)
			: base(abstraction)
		{
			Read(propertiesStyle);
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="File" /> for a specified file abstraction with an
		///    average read style.
		/// </summary>
		/// <param name="abstraction">
		///    A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///    reading from and writing to the file.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="abstraction" /> is <see langword="null"
		///    />.
		/// </exception>
		protected File(IFileAbstraction abstraction)
			: this(abstraction, ReadStyle.Average)
		{
		}


		/// <summary>
		///    Gets the media properties of the file represented by the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="TagLib.Properties" /> object containing the
		///    media properties of the file represented by the current
		///    instance.
		/// </value>
		public override Properties Properties => properties;


		/// <summary>
		///    Reads the file with a specified read style.
		/// </summary>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		private void Read(ReadStyle propertiesStyle)
		{
			Mode = AccessMode.Read;
			try
			{
				ReadStart(InvariantStartPosition, propertiesStyle);

				InvariantEndPosition = Length;

				ReadEnd(InvariantEndPosition, propertiesStyle);

				// Read the audio properties.
				properties = (propertiesStyle & ReadStyle.Average) != 0
					? ReadProperties(InvariantStartPosition,
						InvariantEndPosition, propertiesStyle)
					: null;
			}
			finally
			{
				Mode = AccessMode.Closed;
			}
		}


		/// <summary>
		///    Reads format specific information at the start of the
		///    file.
		/// </summary>
		/// <param name="start">
		///    A <see cref="long" /> value containing the seek position
		///    at which the tags end and the media data begins.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <remarks>
		///    This method is called by the constructor immediately
		///    after the tags at the start of the file have been read
		///    and as such (so the internal seek mechanism is close to
		///    the start). It should be used for reading any content
		///    specific information, such as an audio header from the
		///    start of the file.
		/// </remarks>
		protected virtual void ReadStart(long start,
			ReadStyle propertiesStyle)
		{
		}


		/// <summary>
		///    Reads format specific information at the end of the
		///    file.
		/// </summary>
		/// <param name="end">
		///    A <see cref="long" /> value containing the seek position
		///    at which the media data ends and the tags begin.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <remarks>
		///    This method is called by the constructor immediately
		///    after the tags at the end of the file have been read
		///    and as such (so the internal seek mechanism is close to
		///    the end). It should be used for reading any content
		///    specific information, such as an audio header from the
		///    end of the file.
		/// </remarks>
		protected virtual void ReadEnd(long end,
			ReadStyle propertiesStyle)
		{
		}


		/// <summary>
		///    Reads the audio properties from the file represented by
		///    the current instance.
		/// </summary>
		/// <param name="start">
		///    A <see cref="long" /> value containing the seek position
		///    at which the tags end and the media data begins.
		/// </param>
		/// <param name="end">
		///    A <see cref="long" /> value containing the seek position
		///    at which the media data ends and the tags begin.
		/// </param>
		/// <param name="propertiesStyle">
		///    A <see cref="ReadStyle" /> value specifying at what level
		///    of accuracy to read the media properties, or <see
		///    cref="ReadStyle.None" /> to ignore the properties.
		/// </param>
		/// <returns>
		///    A <see cref="TagLib.Properties" /> object describing the
		///    media properties of the file represented by the current
		///    instance.
		/// </returns>
		/// <remarks>
		///    This method is called ONLY IF the file is constructed
		///    with a read style other than <see cref="ReadStyle.None"
		///    />, and as such MUST NOT return <see langword="null" />.
		///    It is guaranteed that <see cref="ReadStart" /> and <see
		///    cref="ReadEnd" /> will have been called first and this
		///    method should be strictly used to perform final
		///    processing on already read data.
		/// </remarks>
		protected abstract Properties ReadProperties(long start,
			long end,
			ReadStyle propertiesStyle);
	}
}
