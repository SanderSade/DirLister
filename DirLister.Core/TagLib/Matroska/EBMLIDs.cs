namespace Sander.DirLister.Core.TagLib.Matroska
{
	/// <summary>
	/// Public enumeration listing the possible EBML element identifiers.
	/// </summary>
	public enum EBMLID
	{
		/// <summary>
		/// Indicates an EBML Header element.
		/// </summary>
		EBMLHeader = 0x1A45DFA3,

		/// <summary>
		/// Indicates an EBML Version element.
		/// </summary>
		EBMLVersion = 0x4286,

		/// <summary>
		/// Indicates an EBML Read Version element.
		/// </summary>
		EBMLReadVersion = 0x42F7,

		/// <summary>
		/// Indicates an EBML Max ID Length element.
		/// </summary>
		EBMLMaxIDLength = 0x42F2,

		/// <summary>
		/// Indicates an EBML Max Size Length element.
		/// </summary>
		EBMLMaxSizeLength = 0x42F3,

		/// <summary>
		/// Indicates an EBML Doc Type element.
		/// </summary>
		EBMLDocType = 0x4282,

		/// <summary>
		/// Indicates an EBML Doc Type Version element.
		/// </summary>
		EBMLDocTypeVersion = 0x4287,

		/// <summary>
		/// Indicates an EBML Doc Type Read Version element.
		/// </summary>
		EBMLDocTypeReadVersion = 0x4285,

		/// <summary>
		/// Indicates an EBML Void element.
		/// </summary>
		EBMLVoid = 0xEC
	}
}
