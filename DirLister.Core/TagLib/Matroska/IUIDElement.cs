using System;

namespace Sander.DirLister.Core.TagLib.Matroska
{
	/// <summary>
	/// Represent a Matroska element that has an Unique Identifier, and can be tagged.
	/// </summary>
	public interface IUIDElement
	{
		/// <summary>
		/// Unique ID representing the file, as random as possible (setting zero will generate automatically a new one).
		/// </summary>
		ulong UID { get; set; }

		/// <summary>
		/// Get the Tag type the UID should be represented by, or 0 if undefined
		/// </summary>
		MatroskaID UIDType { get; }
	}

	/// <summary>
	/// Represent a basic Matroska UID element
	/// </summary>
	public class UIDElement : IUIDElement
	{
		private static readonly Random random = new Random();

		private ulong _UID = GenUID();


		/// <summary>
		/// Create a UIDElement Stub
		/// </summary>
		/// <param name="type">Tag-type the UID represents</param>
		/// <param name="uid">UID of the element</param>
		public UIDElement(MatroskaID type, ulong uid)
		{
			UID = uid;
			if (type == MatroskaID.TagTrackUID
			    || type == MatroskaID.TagEditionUID
			    || type == MatroskaID.TagChapterUID
			    || type == MatroskaID.TagAttachmentUID
			)
				UIDType = type;
			else UIDType = 0;
		}


		/// <summary>
		/// Unique ID representing the element, as random as possible (setting zero will generate automatically a new one).
		/// </summary>
		public ulong UID
		{
			get => _UID;
			set => _UID = GenUID(value);
		}

		/// <summary>
		/// Get the Tag type the UID should be represented by, or 0 if undefined
		/// </summary>
		public MatroskaID UIDType { get; } = 0;


		/// <summary>
		/// Generate a new random UID
		/// </summary>
		/// <param name="ret">Value of the UID to be generated. A zero value will randomize it.</param>
		/// <returns>Generated UID.</returns>
		public static ulong GenUID(ulong ret = 0)
		{
			while (ret == 0)
			{
				ret = (ulong)random.Next() << 32;
				ret |= (uint)random.Next();
			}

			return ret;
		}
	}
}
