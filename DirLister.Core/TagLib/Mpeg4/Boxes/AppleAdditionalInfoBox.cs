using System;
using System.Diagnostics.CodeAnalysis;

namespace Sander.DirLister.Core.TagLib.Mpeg4.Boxes
{
	/// <summary>
	///     This class extends <see cref="Box" /> to provide an
	///     implementation of an Apple AdditionalInfoBox.
	/// </summary>
	public class AppleAdditionalInfoBox : Box
	{
		/// <summary>
		///     Contains the box data.
		/// </summary>
		private ByteVector data;


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="AppleAdditionalInfoBox" />
		///     with a provided header
		///     and handler by reading the contents from a specified
		///     file.
		/// </summary>
		/// <param name="header">
		///     A <see cref="BoxHeader" /> object containing the header
		///     to use for the new instance.
		/// </param>
		/// <param name="file">
		///     A <see cref="TagLib.File" /> object to read the contents
		///     of the box from.
		/// </param>
		/// <param name="handler">
		///     A <see cref="IsoHandlerBox" /> object containing the
		///     handler that applies to the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public AppleAdditionalInfoBox(BoxHeader header, TagLib.File file, IsoHandlerBox handler) : base(header, handler)
		{
			// We do not care what is in this custom data section
			// see: https://developer.apple.com/library/mac/#documentation/QuickTime/QTFF/QTFFChap2/qtff2.html
			Data = LoadData(file);
		}


		/// <summary>
		///     Constructs and initializes a new instance of
		///     <see
		///         cref="AppleAdditionalInfoBox" />
		///     using specified header, version and flags
		/// </summary>
		/// <param name="header">defines the header data</param>
		public AppleAdditionalInfoBox(ByteVector header) : base(header)
		{
		}


		/// <summary>
		///     Gets and sets the data contained in the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="ByteVector" /> object containing the data
		///     contained in the current instance.
		/// </value>
		public override ByteVector Data
		{
			get => data;
			set => data = value ?? new ByteVector();
		}

		/// <summary>
		///     Gets and sets the text contained in the current instance.
		/// </summary>
		/// <value>
		///     A <see cref="string" /> object containing the text
		///     contained in the current instance.
		/// </value>
		public string Text
		{
			get => Data.ToString(StringType.Latin1)
				.TrimStart('\0');
			set => Data = ByteVector.FromString(value,
				StringType.Latin1);
		}
	}
}
