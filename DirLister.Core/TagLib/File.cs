using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sander.DirLister.Core.TagLib
{
	/// <summary>
	///     Specifies the options to use when reading the media.
	/// </summary>
	[Flags]
	public enum ReadStyle
	{
		/// <summary>
		///     The media properties will not be read.
		/// </summary>
		None = 0,

		// Fast = 1,

		/// <summary>
		///     The media properties will be read with average accuracy.
		/// </summary>
		Average = 2
	}

	/// <summary>
	///     This abstract class provides a basic framework for reading from
	///     and writing to a file, as well as accessing basic tagging and
	///     media properties.
	/// </summary>
	/// <remarks>
	///     <para>
	///         This class is agnostic to all specific media types. Its
	///         child classes, on the other hand, support the the intricacies of
	///         different media and tagging formats. For example,
	///         <see
	///             cref="Mpeg4.File" />
	///         supports the MPEG-4 specificication and
	///         Apple's tagging format.
	///     </para>
	///     <para>
	///         Each file type can be created using its format specific
	///         constructors, ie. <see cref="Mpeg4.File(string)" />, but the
	///         preferred method is to use
	///         <see
	///             cref="File.Create(string,string,ReadStyle)" />
	///         or one of its
	///         variants, as it automatically detects the appropriate class from
	///         the file extension or provided mime-type.
	///     </para>
	/// </remarks>
	public abstract class File : IDisposable
	{
		/// <summary>
		///     Specifies the type of file access operations currently
		///     permitted on an instance of <see cref="File" />.
		/// </summary>
		public enum AccessMode
		{
			/// <summary>
			///     Read operations can be performed.
			/// </summary>
			Read,

			/// <summary>
			///     Read and write operations can be performed.
			/// </summary>
			Write,

			/// <summary>
			///     The file is closed for both read and write
			///     operations.
			/// </summary>
			Closed
		}

		/// <summary>
		///     Contains buffer size to use when reading.
		/// </summary>
		private static readonly int buffer_size = 1024;

		private static readonly Dictionary<Type, TagFileConstructor<File>> TagFileConstructors = new Dictionary<Type, TagFileConstructor<File>>();


		/// <summary>
		///     The reasons (if any) why this file is marked as corrupt.
		/// </summary>
		private List<string> _corruptionReasons;

		/// <summary>
		///     Contains the current stream used in reading/writing.
		/// </summary>
		private Stream _fileStream;

		/// <summary>
		///     Contains position at which the invariant data portion of
		///     the file begins.
		/// </summary>
		private long _invariantStartPosition;

		/// <summary>
		///     Contains the internal file abstraction.
		/// </summary>
		protected IFileAbstraction file_abstraction;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified path in the local file
		///     system.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object containing the path of the
		///     file to use in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="path" /> is <see langword="null" />.
		/// </exception>
		protected File(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			file_abstraction = new LocalFileAbstraction(path);
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="File" />
		///     for a specified file abstraction.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading from and writing to the file.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="abstraction" /> is <see langword="null" />.
		/// </exception>
		protected File(IFileAbstraction abstraction)
		{
			file_abstraction = abstraction ?? throw new ArgumentNullException("abstraction");
		}


		/// <summary>
		///     The buffer size to use when reading large blocks of data
		///     in the <see cref="File" /> class.
		/// </summary>
		/// <value>
		///     A <see cref="uint" /> containing the buffer size to use
		///     when reading large blocks of data.
		/// </value>
		public static uint BufferSize => (uint)buffer_size;

		/// <summary>
		///     Gets the media properties of the file represented by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="TagLib.Properties" /> object containing the
		///     media properties of the file represented by the current
		///     instance.
		/// </value>
		public abstract Properties Properties { get; }

		/// <summary>
		///     Gets the name of the file as stored in its file
		///     abstraction.
		/// </summary>
		/// <value>
		///     A <see cref="string" /> object containing the name of the
		///     file as stored in the <see cref="TagLib.File.IFileAbstraction" />
		///     object used to create it or the path if created with a
		///     local path.
		/// </value>
		public string Name => file_abstraction.Name;

		/// <summary>
		///     Gets the mime-type of the file as determined by
		///     <see
		///         cref="Create(IFileAbstraction,string,ReadStyle)" />
		///     if
		///     that method was used to create the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="string" /> object containing the mime-type
		///     used to create the file or <see langword="null" /> if
		///     <see
		///         cref="Create(IFileAbstraction,string,ReadStyle)" />
		///     was
		///     not used to create the current instance.
		/// </value>
		public string MimeType { get; internal set; }

		/// <summary>
		///     Gets the seek position in the internal stream used by the
		///     current instance.
		/// </summary>
		/// <value>
		///     A <see cref="long" /> value representing the seek
		///     position, or 0 if the file is not open for reading.
		/// </value>
		public long Tell => Mode == AccessMode.Closed ? 0 : _fileStream.Position;

		/// <summary>
		///     Gets the length of the file represented by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="long" /> value representing the size of the
		///     file, or 0 if the file is not open for reading.
		/// </value>
		public long Length => Mode == AccessMode.Closed ? 0 : _fileStream.Length;

		/// <summary>
		///     Gets the position at which the invariant portion of the
		///     current instance begins.
		/// </summary>
		/// <value>
		///     A <see cref="long" /> value representing the seek
		///     position at which the file's invariant (media) data
		///     section begins. If the value could not be determined,
		///     <c>-1</c> is returned.
		/// </value>
		public long InvariantStartPosition
		{
			get => _invariantStartPosition;
			protected set => _invariantStartPosition = value;
		}

		/// <summary>
		///     Gets the position at which the invariant portion of the
		///     current instance ends.
		/// </summary>
		/// <value>
		///     A <see cref="long" /> value representing the seek
		///     position at which the file's invariant (media) data
		///     section ends. If the value could not be determined,
		///     <c>-1</c> is returned.
		/// </value>
		public long InvariantEndPosition { get; protected set; } = -1;

		/// <summary>
		///     Gets and sets the file access mode in use by the current
		///     instance.
		/// </summary>
		/// <value>
		///     A <see cref="AccessMode" /> value describing the features
		///     of stream currently in use by the current instance.
		/// </value>
		public AccessMode Mode
		{
			get => _fileStream == null ? AccessMode.Closed : _fileStream.CanWrite ? AccessMode.Write : AccessMode.Read;
			set
			{
				if (Mode == value || Mode == AccessMode.Write
					&& value == AccessMode.Read)
				{
					return;
				}

				if (_fileStream != null)
				{
					file_abstraction.CloseStream(_fileStream);
				}

				_fileStream = null;

				if (value == AccessMode.Read)
				{
					_fileStream = file_abstraction.ReadStream;
				}
				else if (value == AccessMode.Write)
				{
					_fileStream = file_abstraction.WriteStream;
				}

				Mode = value;
			}
		}

		/// <summary>
		///     Gets the <see cref="IFileAbstraction" /> representing the file.
		/// </summary>
		public IFileAbstraction FileAbstraction => file_abstraction;

		/// <summary>
		///     Indicates if tags can be written back to the current file or not
		/// </summary>
		/// <value>
		///     A <see cref="bool" /> which is true if tags can be written to the
		///     current file, otherwise false.
		/// </value>
		public virtual bool Writeable => !PossiblyCorrupt;

		/// <summary>
		///     Indicates whether or not this file may be corrupt.
		/// </summary>
		/// <value>
		///     <c>true</c> if possibly corrupt; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		///     Files with unknown corruptions should not be written.
		/// </remarks>
		public bool PossiblyCorrupt => _corruptionReasons != null;

		/// <summary>
		///     The reasons for which this file is marked as corrupt.
		/// </summary>
		public IEnumerable<string> CorruptionReasons => _corruptionReasons;


		/// <summary>
		///     Dispose the current file. Equivalent to setting the
		///     mode to closed
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
		public void Dispose()
		{
			Mode = AccessMode.Closed;
		}


		/// <summary>
		///     Mark the file as corrupt.
		/// </summary>
		/// <param name="reason">
		///     The reason why this file is considered to be corrupt.
		/// </param>
		internal void MarkAsCorrupt(string reason)
		{
			if (_corruptionReasons == null)
			{
				_corruptionReasons = new List<string>();
			}

			_corruptionReasons.Add(reason);
		}


		/// <summary>
		///     Reads a specified number of bytes at the current seek
		///     position from the current instance.
		/// </summary>
		/// <param name="length">
		///     A <see cref="int" /> value specifying the number of bytes
		///     to read.
		/// </param>
		/// <returns>
		///     A <see cref="ByteVector" /> object containing the data
		///     read from the current instance.
		/// </returns>
		/// <remarks>
		///     <para>
		///         This method reads the block of data at the current
		///         seek position. To change the seek position, use
		///         <see
		///             cref="Seek(long,System.IO.SeekOrigin)" />
		///         .
		///     </para>
		/// </remarks>
		/// <exception cref="ArgumentException">
		///     <paramref name="length" /> is less than zero.
		/// </exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ByteVector ReadBlock(int length)
		{
			if (length == 0)
			{
				return ByteVector.Zero;
			}

			Mode = AccessMode.Read;

			var buffer = new byte[length];

			int count, read = 0, needed = length;

			do
			{
				count = _fileStream.Read(buffer, read, needed);

				read += count;
				needed -= count;
			} while (needed > 0 && count != 0);

			return new ByteVector(buffer, read);
		}


		/// <summary>
		///     Searches forwards through a file for a specified
		///     pattern, starting at a specified offset.
		/// </summary>
		/// <param name="pattern">
		///     A <see cref="ByteVector" /> object containing a pattern
		///     to search for in the current instance.
		/// </param>
		/// <param name="startPosition">
		///     A <see cref="int" /> value specifying at what
		///     seek position to start searching.
		/// </param>
		/// <param name="before">
		///     A <see cref="ByteVector" /> object specifying a pattern
		///     that the searched for pattern must appear before. If this
		///     pattern is found first, -1 is returned.
		/// </param>
		/// <returns>
		///     A <see cref="long" /> value containing the index at which
		///     the value was found. If not found, -1 is returned.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="pattern" /> is <see langword="null" />.
		/// </exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long Find(ByteVector pattern, long startPosition,
			ByteVector before)
		{
			Mode = AccessMode.Read;

			if (pattern.Count > buffer_size)
			{
				return -1;
			}

			// The position in the file that the current buffer
			// starts at.

			var bufferOffset = startPosition;
			var originalPosition = _fileStream.Position;

			try
			{
				// Start the search at the offset.
				_fileStream.Position = startPosition;
				for (var buffer = ReadBlock(buffer_size); buffer.Count > 0; buffer = ReadBlock(buffer_size))
				{
					var location = buffer.Find(pattern);
					if (before != null)
					{
						var beforeLocation = buffer.Find(before);
						if (beforeLocation < location)
						{
							return -1;
						}
					}

					if (location >= 0)
					{
						return bufferOffset + location;
					}

					// Ensure that we always rewind the stream a little so we never have a partial
					// match where our data exists between the end of read A and the start of read B.
					bufferOffset += buffer_size - pattern.Count;
					if (before != null && before.Count > pattern.Count)
					{
						bufferOffset -= before.Count - pattern.Count;
					}

					_fileStream.Position = bufferOffset;
				}

				return -1;
			}
			finally
			{
				_fileStream.Position = originalPosition;
			}
		}


		/// <summary>
		///     Searches forwards through a file for a specified
		///     pattern, starting at a specified offset.
		/// </summary>
		/// <param name="pattern">
		///     A <see cref="ByteVector" /> object containing a pattern
		///     to search for in the current instance.
		/// </param>
		/// <param name="startPosition">
		///     A <see cref="int" /> value specifying at what
		///     seek position to start searching.
		/// </param>
		/// <returns>
		///     A <see cref="long" /> value containing the index at which
		///     the value was found. If not found, -1 is returned.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="pattern" /> is <see langword="null" />.
		/// </exception>
		public long Find(ByteVector pattern, long startPosition)
		{
			return Find(pattern, startPosition, null);
		}


		/// <summary>
		///     Searches forwards through a file for a specified
		///     pattern, starting at the beginning of the file.
		/// </summary>
		/// <param name="pattern">
		///     A <see cref="ByteVector" /> object containing a pattern
		///     to search for in the current instance.
		/// </param>
		/// <returns>
		///     A <see cref="long" /> value containing the index at which
		///     the value was found. If not found, -1 is returned.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="pattern" /> is <see langword="null" />.
		/// </exception>
		public long Find(ByteVector pattern)
		{
			return Find(pattern, 0);
		}


		/// <summary>
		///     Searches backwards through a file for a specified
		///     pattern, starting at a specified offset.
		/// </summary>
		/// <param name="pattern">
		///     A <see cref="ByteVector" /> object containing a pattern
		///     to search for in the current instance.
		/// </param>
		/// <param name="startPosition">
		///     A <see cref="int" /> value specifying at what
		///     seek position to start searching.
		/// </param>
		/// <param name="after">
		///     A <see cref="ByteVector" /> object specifying a pattern
		///     that the searched for pattern must appear after. If this
		///     pattern is found first, -1 is returned.
		/// </param>
		/// <returns>
		///     A <see cref="long" /> value containing the index at which
		///     the value was found. If not found, -1 is returned.
		/// </returns>
		/// <remarks>
		///     Searching for <paramref name="after" /> is not yet
		///     implemented.
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="pattern" /> is <see langword="null" />.
		/// </exception>
		private long RFind(ByteVector pattern, long startPosition,
			ByteVector after)
		{
			Mode = AccessMode.Read;

			if (pattern.Count > buffer_size)
			{
				return -1;
			}


			// These variables are used to keep track of a partial
			// match that happens at the end of a buffer.

			/*
			int previous_partial_match = -1;
			int after_previous_partial_match = -1;
			*/

			// Save the location of the current read pointer.  We
			// will restore the position using Seek() before all
			// returns.

			var original_position = _fileStream.Position;

			// Start the search at the offset.

			var buffer_offset = Length - startPosition;
			var read_size = buffer_size;

			read_size = (int)Math.Min(buffer_offset, buffer_size);
			buffer_offset -= read_size;
			_fileStream.Position = buffer_offset;


			// The position in the file that the current buffer
			// starts at.

			ByteVector buffer;
			// See the notes in find() for an explanation of this
			// algorithm.

			for (buffer = ReadBlock(read_size);
				buffer.Count > 0;
				buffer = ReadBlock(read_size))
			{
				// TODO: (1) previous partial match

				// (2) pattern contained in current buffer

				long location = buffer.RFind(pattern);
				if (location >= 0)
				{
					_fileStream.Position = original_position;
					return buffer_offset + location;
				}

				if (after != null && buffer.RFind(after) >= 0)
				{
					_fileStream.Position = original_position;
					return -1;
				}

				read_size = (int)Math.Min(buffer_offset, buffer_size);
				buffer_offset -= read_size;
				if (read_size + pattern.Count > buffer_size)
				{
					buffer_offset += pattern.Count;
				}

				_fileStream.Position = buffer_offset;
			}

			// Since we hit the end of the file, reset the status
			// before continuing.

			_fileStream.Position = original_position;
			return -1;
		}


		/// <summary>
		///     Searches backwards through a file for a specified
		///     pattern, starting at a specified offset.
		/// </summary>
		/// <param name="pattern">
		///     A <see cref="ByteVector" /> object containing a pattern
		///     to search for in the current instance.
		/// </param>
		/// <param name="startPosition">
		///     A <see cref="int" /> value specifying at what
		///     seek position to start searching.
		/// </param>
		/// <returns>
		///     A <see cref="long" /> value containing the index at which
		///     the value was found. If not found, -1 is returned.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="pattern" /> is <see langword="null" />.
		/// </exception>
		public long RFind(ByteVector pattern, long startPosition)
		{
			return RFind(pattern, startPosition, null);
		}


		/// <summary>
		///     Searches backwards through a file for a specified
		///     pattern, starting at the end of the file.
		/// </summary>
		/// <param name="pattern">
		///     A <see cref="ByteVector" /> object containing a pattern
		///     to search for in the current instance.
		/// </param>
		/// <returns>
		///     A <see cref="long" /> value containing the index at which
		///     the value was found. If not found, -1 is returned.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="pattern" /> is <see langword="null" />.
		/// </exception>
		public long RFind(ByteVector pattern)
		{
			return RFind(pattern, 0);
		}


		/// <summary>
		///     Seeks the read/write pointer to a specified offset in the
		///     current instance, relative to a specified origin.
		/// </summary>
		/// <param name="offset">
		///     A <see cref="long" /> value indicating the byte offset to
		///     seek to.
		/// </param>
		/// <param name="origin">
		///     A <see cref="System.IO.SeekOrigin" /> value specifying an
		///     origin to seek from.
		/// </param>
		public void Seek(long offset, SeekOrigin origin)
		{
			if (Mode != AccessMode.Closed)
			{
				_fileStream.Seek(offset, origin);
			}
		}


		/// <summary>
		///     Seeks the read/write pointer to a specified offset in the
		///     current instance, relative to the beginning of the file.
		/// </summary>
		/// <param name="offset">
		///     A <see cref="long" /> value indicating the byte offset to
		///     seek to.
		/// </param>
		public void Seek(long offset)
		{
			Seek(offset, SeekOrigin.Begin);
		}


		/// <summary>
		///     Creates a new instance of a <see cref="File" /> subclass
		///     for a specified path, guessing the mime-type from the
		///     file's extension and using the average read style.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object specifying the file to
		///     read from and write to.
		/// </param>
		/// <returns>
		///     A new instance of <see cref="File" /> as read from the
		///     specified path.
		/// </returns>
		/// <exception cref="CorruptFileException">
		///     The file could not be read due to corruption.
		/// </exception>
		/// <exception cref="UnsupportedFormatException">
		///     The file could not be read because the mime-type could
		///     not be resolved or the library does not support an
		///     internal feature of the file crucial to its reading.
		/// </exception>
		public static File Create(string path)
		{
			return Create(path, null, ReadStyle.Average);
		}


		/// <summary>
		///     Creates a new instance of a <see cref="File" /> subclass
		///     for a specified file abstraction, guessing the mime-type
		///     from the file's extension and using the average read
		///     style.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading to and writing from the current instance.
		/// </param>
		/// <returns>
		///     A new instance of <see cref="File" /> as read from the
		///     specified abstraction.
		/// </returns>
		/// <exception cref="CorruptFileException">
		///     The file could not be read due to corruption.
		/// </exception>
		/// <exception cref="UnsupportedFormatException">
		///     The file could not be read because the mime-type could
		///     not be resolved or the library does not support an
		///     internal feature of the file crucial to its reading.
		/// </exception>
		public static File Create(IFileAbstraction abstraction)
		{
			return Create(abstraction, null, ReadStyle.Average);
		}


		/// <summary>
		///     Creates a new instance of a <see cref="File" /> subclass
		///     for a specified path and read style, guessing the
		///     mime-type from the file's extension.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object specifying the file to
		///     read from and write to.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying the level of
		///     detail to use when reading the media information from the
		///     new instance.
		/// </param>
		/// <returns>
		///     A new instance of <see cref="File" /> as read from the
		///     specified path.
		/// </returns>
		/// <exception cref="CorruptFileException">
		///     The file could not be read due to corruption.
		/// </exception>
		/// <exception cref="UnsupportedFormatException">
		///     The file could not be read because the mime-type could
		///     not be resolved or the library does not support an
		///     internal feature of the file crucial to its reading.
		/// </exception>
		public static File Create(string path,
			ReadStyle propertiesStyle)
		{
			return Create(path, null, propertiesStyle);
		}


		/// <summary>
		///     Creates a new instance of a <see cref="File" /> subclass
		///     for a specified file abstraction and read style, guessing
		///     the mime-type from the file's extension.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading to and writing from the current instance.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying the level of
		///     detail to use when reading the media information from the
		///     new instance.
		/// </param>
		/// <returns>
		///     A new instance of <see cref="File" /> as read from the
		///     specified abstraction.
		/// </returns>
		/// <exception cref="CorruptFileException">
		///     The file could not be read due to corruption.
		/// </exception>
		/// <exception cref="UnsupportedFormatException">
		///     The file could not be read because the mime-type could
		///     not be resolved or the library does not support an
		///     internal feature of the file crucial to its reading.
		/// </exception>
		public static File Create(IFileAbstraction abstraction,
			ReadStyle propertiesStyle)
		{
			return Create(abstraction, null, propertiesStyle);
		}


		/// <summary>
		///     Creates a new instance of a <see cref="File" /> subclass
		///     for a specified path, mime-type, and read style.
		/// </summary>
		/// <param name="path">
		///     A <see cref="string" /> object specifying the file to
		///     read from and write to.
		/// </param>
		/// <param name="mimetype">
		///     A <see cref="string" /> object containing the mime-type
		///     to use when selecting the appropriate class to use, or
		///     <see langword="null" /> if the extension in
		///     <paramref
		///         name="path" />
		///     is to be used.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying the level of
		///     detail to use when reading the media information from the
		///     new instance.
		/// </param>
		/// <returns>
		///     A new instance of <see cref="File" /> as read from the
		///     specified path.
		/// </returns>
		/// <exception cref="CorruptFileException">
		///     The file could not be read due to corruption.
		/// </exception>
		/// <exception cref="UnsupportedFormatException">
		///     The file could not be read because the mime-type could
		///     not be resolved or the library does not support an
		///     internal feature of the file crucial to its reading.
		/// </exception>
		public static File Create(string path, string mimetype,
			ReadStyle propertiesStyle)
		{
			return Create(new LocalFileAbstraction(path),
				mimetype, propertiesStyle);
		}


		/// <summary>
		///     Creates a new instance of a <see cref="File" /> subclass
		///     for a specified file abstraction, mime-type, and read
		///     style.
		/// </summary>
		/// <param name="abstraction">
		///     A <see cref="TagLib.File.IFileAbstraction" /> object to use when
		///     reading to and writing from the current instance.
		/// </param>
		/// <param name="mimetype">
		///     A <see cref="string" /> object containing the mime-type
		///     to use when selecting the appropriate class to use, or
		///     <see langword="null" /> if the extension in
		///     <paramref
		///         name="abstraction" />
		///     is to be used.
		/// </param>
		/// <param name="propertiesStyle">
		///     A <see cref="ReadStyle" /> value specifying the level of
		///     detail to use when reading the media information from the
		///     new instance.
		/// </param>
		/// <returns>
		///     A new instance of <see cref="File" /> as read from the
		///     specified abstraction.
		/// </returns>
		/// <exception cref="CorruptFileException">
		///     The file could not be read due to corruption.
		/// </exception>
		/// <exception cref="UnsupportedFormatException">
		///     The file could not be read because the mime-type could
		///     not be resolved or the library does not support an
		///     internal feature of the file crucial to its reading.
		/// </exception>
		public static File Create(IFileAbstraction abstraction,
			string mimetype,
			ReadStyle propertiesStyle)
		{
			if (mimetype == null)
			{
				mimetype = $"taglib/{abstraction.Name.Substring(abstraction.Name.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLowerInvariant()}";
			}

			if (!FileTypes.AvailableTypes.TryGetValue(mimetype, out var fileType))
			{
				throw new UnsupportedFormatException(
					string.Format(
						CultureInfo.InvariantCulture,
						"{0} ({1})",
						abstraction.Name,
						mimetype));
			}

			if (!TagFileConstructors.TryGetValue(fileType, out var activator))
			{
				var ctor = fileType.GetConstructors().First(x =>
				{
					var parameters = x.GetParameters();
					return parameters[0].ParameterType == typeof(IFileAbstraction) &&
					       parameters[1].ParameterType == typeof(ReadStyle);
				});

				activator = CompileConstructor<File>(ctor);
				TagFileConstructors.Add(fileType, activator);
			}

			var file = activator(abstraction, propertiesStyle);
			file.MimeType = mimetype;
			return file;
		}


		/// <summary>
		///     After https://rogerjohansson.blog/2008/02/28/linq-expressions-creating-objects/
		/// </summary>
		private static TagFileConstructor<T> CompileConstructor<T>
			(ConstructorInfo constructorInfo)
		{
			var parameterInfo = constructorInfo.GetParameters();

			var parameter = Expression.Parameter(typeof(object[]), "args");

			var arguments = new Expression[parameterInfo.Length];

			for (var i = 0; i < parameterInfo.Length; i++)
			{
				arguments[i] = Expression.Convert(Expression.ArrayIndex(parameter, Expression.Constant(i)), parameterInfo[i].ParameterType);
			}

			var constructorExpression = Expression.New(constructorInfo, arguments);

			return (TagFileConstructor<T>)Expression.Lambda(typeof(TagFileConstructor<T>), constructorExpression, parameter)
				.Compile();
		}


		private delegate T TagFileConstructor<out T>(params object[] args);


		/// <summary>
		///     This class implements <see cref="TagLib.File.IFileAbstraction" />
		///     to provide support for accessing the local/standard file
		///     system.
		/// </summary>
		/// <remarks>
		///     This class is used as the standard file abstraction
		///     throughout the library.
		/// </remarks>
		public sealed class LocalFileAbstraction : IFileAbstraction
		{
			/// <summary>
			///     Constructs and initializes a new instance of
			///     <see cref="LocalFileAbstraction" /> for a
			///     specified path in the local file system.
			/// </summary>
			/// <param name="path">
			///     A <see cref="string" /> object containing the
			///     path of the file to use in the new instance.
			/// </param>
			/// <exception cref="ArgumentNullException">
			///     <paramref name="path" /> is <see langword="null" />.
			/// </exception>
			public LocalFileAbstraction(string path)
			{
				Name = path ?? throw new ArgumentNullException("path");
			}


			/// <summary>
			///     Gets the path of the file represented by the
			///     current instance.
			/// </summary>
			/// <value>
			///     A <see cref="string" /> object containing the
			///     path of the file represented by the current
			///     instance.
			/// </value>
			public string Name { get; }

			/// <summary>
			///     Gets a new readable, seekable stream from the
			///     file represented by the current instance.
			/// </summary>
			/// <value>
			///     A new <see cref="System.IO.Stream" /> to be used
			///     when reading the file represented by the current
			///     instance.
			/// </value>
			public Stream ReadStream => System.IO.File.Open(Name,
				FileMode.Open,
				FileAccess.Read,
				FileShare.Read);

			/// <summary>
			///     Gets a new writable, seekable stream from the
			///     file represented by the current instance.
			/// </summary>
			/// <value>
			///     A new <see cref="System.IO.Stream" /> to be used
			///     when writing to the file represented by the
			///     current instance.
			/// </value>
			public Stream WriteStream => System.IO.File.Open(Name,
				FileMode.Open,
				FileAccess.ReadWrite);


			/// <summary>
			///     Closes a stream created by the current instance.
			/// </summary>
			/// <param name="stream">
			///     A <see cref="System.IO.Stream" /> object
			///     created by the current instance.
			/// </param>
			public void CloseStream(Stream stream)
			{
				if (stream == null)
				{
					throw new ArgumentNullException("stream");
				}

				stream.Dispose();
				//stream.Close();
			}
		}

		/// <summary>
		///     This interface provides abstracted access to a file. It
		///     premits access to non-standard file systems and data
		///     retrieval methods.
		/// </summary>
		/// <remarks>
		///     <para>
		///         To use a custom abstraction, use
		///         <see
		///             cref="Create(IFileAbstraction)" />
		///         instead of
		///         <see
		///             cref="Create(string)" />
		///         when creating files.
		///     </para>
		/// </remarks>
		/// <example>
		///     <para>
		///         The following example uses Gnome VFS to open a file
		///         and read its title.
		///     </para>
		///     <code lang="C#">using TagLib;
		/// using Gnome.Vfs;
		/// 
		/// public class ReadTitle
		/// {
		///    public static void Main (string [] args)
		///    {
		///       if (args.Length != 1)
		///          return;
		/// 
		///       Gnome.Vfs.Vfs.Initialize ();
		/// 
		///       try {
		///           TagLib.File file = TagLib.File.Create (
		///              new VfsFileAbstraction (args [0]));
		///           System.Console.WriteLine (file.Tag.Title);
		///       } finally {
		///          Vfs.Shutdown()
		///       }
		///    }
		/// }
		/// 
		/// public class VfsFileAbstraction : TagLib.File.IFileAbstraction
		/// {
		///     private string name;
		/// 
		///     public VfsFileAbstraction (string file)
		///     {
		///         name = file;
		///     }
		/// 
		///     public string Name {
		///         get { return name; }
		///     }
		/// 
		///     public System.IO.Stream ReadStream {
		///         get { return new VfsStream(Name, System.IO.FileMode.Open); }
		///     }
		/// 
		///     public System.IO.Stream WriteStream {
		///         get { return new VfsStream(Name, System.IO.FileMode.Open); }
		///     }
		/// 
		///     public void CloseStream (System.IO.Stream stream)
		///     {
		///         stream.Close ();
		///     }
		/// }</code>
		///     <code lang="Boo">import TagLib from "taglib-sharp.dll"
		/// import Gnome.Vfs from "gnome-vfs-sharp"
		/// 
		/// class VfsFileAbstraction (TagLib.File.IFileAbstraction):
		/// 
		///         _name as string
		/// 
		///         def constructor(file as string):
		///                 _name = file
		/// 
		///         Name:
		///                 get:
		///                         return _name
		/// 
		///         ReadStream:
		///                 get:
		///                         return VfsStream(_name, FileMode.Open)
		/// 
		///         WriteStream:
		///                 get:
		///                         return VfsStream(_name, FileMode.Open)
		/// 
		/// if len(argv) == 1:
		///         Vfs.Initialize()
		/// 
		///         try:
		///                 file as TagLib.File = TagLib.File.Create (VfsFileAbstraction (argv[0]))
		///                 print file.Tag.Title
		///         ensure:
		///                 Vfs.Shutdown()</code>
		/// </example>
		public interface IFileAbstraction
		{
			/// <summary>
			///     Gets the name or identifier used by the
			///     implementation.
			/// </summary>
			/// <value>
			///     A <see cref="string" /> object containing the
			///     name or identifier used by the implementation.
			/// </value>
			/// <remarks>
			///     This value would typically represent a path or
			///     URL to be used when identifying the file in the
			///     file system, but it could be any value
			///     as appropriate for the implementation.
			/// </remarks>
			string Name { get; }

			/// <summary>
			///     Gets a readable, seekable stream for the file
			///     referenced by the current instance.
			/// </summary>
			/// <value>
			///     A <see cref="System.IO.Stream" /> object to be
			///     used when reading a file.
			/// </value>
			/// <remarks>
			///     This property is typically used when creating
			///     constructing an instance of <see cref="File" />.
			///     Upon completion of the constructor,
			///     <see
			///         cref="CloseStream" />
			///     will be called to close
			///     the stream. If the stream is to be reused after
			///     this point, <see cref="CloseStream" /> should be
			///     implemented in a way to keep it open.
			/// </remarks>
			Stream ReadStream { get; }

			/// <summary>
			///     Gets a writable, seekable stream for the file
			///     referenced by the current instance.
			/// </summary>
			/// <value>
			///     A <see cref="System.IO.Stream" /> object to be
			///     used when writing to a file.
			/// </value>
			Stream WriteStream { get; }


			/// <summary>
			///     Closes a stream originating from the current
			///     instance.
			/// </summary>
			/// <param name="stream">
			///     A <see cref="System.IO.Stream" /> object
			///     originating from the current instance.
			/// </param>
			/// <remarks>
			///     If the stream is to be used outside of the scope,
			///     of TagLib#, this method should perform no action.
			///     For example, a stream that was created outside of
			///     the current instance, or a stream that will
			///     subsequently be used to play the file.
			/// </remarks>
			void CloseStream(Stream stream);
		}
	}
}
