namespace Sander.DirLister.Core.TagLib.Mpeg4
{
	/// <summary>
	///     <see cref="BoxType" /> provides references to different box types
	///     used by the library.
	/// </summary>
	internal static class BoxType
	{
		public static readonly ReadOnlyByteVector Aart = "aART";

		public static readonly ReadOnlyByteVector Cond = "cond";
		public static readonly ReadOnlyByteVector Covr = "covr";
		public static readonly ReadOnlyByteVector Co64 = "co64";
		public static readonly ReadOnlyByteVector Cpil = "cpil";
		public static readonly ReadOnlyByteVector Cprt = "cprt";
		public static readonly ReadOnlyByteVector Data = "data";
		public static readonly ReadOnlyByteVector Desc = "desc";
		public static readonly ReadOnlyByteVector Disk = "disk";
		public static readonly ReadOnlyByteVector Dtag = "dtag";
		public static readonly ReadOnlyByteVector Esds = "esds";
		public static readonly ReadOnlyByteVector Ilst = "ilst";
		public static readonly ReadOnlyByteVector Free = "free";

		public static readonly ReadOnlyByteVector Gnre = "gnre";

		public static readonly ReadOnlyByteVector Hdlr = "hdlr";

		public static readonly ReadOnlyByteVector Mdat = "mdat";
		public static readonly ReadOnlyByteVector Mdia = "mdia";
		public static readonly ReadOnlyByteVector Meta = "meta";
		public static readonly ReadOnlyByteVector Mean = "mean";
		public static readonly ReadOnlyByteVector Minf = "minf";
		public static readonly ReadOnlyByteVector Moov = "moov";
		public static readonly ReadOnlyByteVector Mvhd = "mvhd";

		public static readonly ReadOnlyByteVector Name = "name";
		public static readonly ReadOnlyByteVector Role = "role";
		public static readonly ReadOnlyByteVector Skip = "skip";
		public static readonly ReadOnlyByteVector Soaa = "soaa"; // Album Artist Sort
		public static readonly ReadOnlyByteVector Soar = "soar"; // Performer Sort
		public static readonly ReadOnlyByteVector Soco = "soco"; // Composer Sort
		public static readonly ReadOnlyByteVector Sonm = "sonm"; // Track Title Sort
		public static readonly ReadOnlyByteVector Soal = "soal"; // Album Title Sort
		public static readonly ReadOnlyByteVector Stbl = "stbl";
		public static readonly ReadOnlyByteVector Stco = "stco";
		public static readonly ReadOnlyByteVector Stsd = "stsd";
		public static readonly ReadOnlyByteVector Subt = "Subt";
		public static readonly ReadOnlyByteVector Text = "text";
		public static readonly ReadOnlyByteVector Tmpo = "tmpo";
		public static readonly ReadOnlyByteVector Trak = "trak";
		public static readonly ReadOnlyByteVector Trkn = "trkn";
		public static readonly ReadOnlyByteVector Udta = "udta";

		public static readonly ReadOnlyByteVector Uuid = "uuid";
		public static readonly ReadOnlyByteVector DASH = "----";

		// Handler types.
		public static readonly ReadOnlyByteVector Soun = "soun";

		public static readonly ReadOnlyByteVector Vide = "vide";

		// Another handler type, found in wild in audio file ripped using iTunes
		public static readonly ReadOnlyByteVector Alis = "alis";
	}
}
