using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Sander.DirLister.Core
{
	/// <summary>
	///     Media info, encapsulating image/audio/video data
	/// </summary>
	[DataContract]
	public sealed partial class MediaInfo
	{
		[DataMember(Name = "mediaType", Order = 1)]
		private string MediaTypeString => Enum.GetName(typeof(MediaType), MediaType);


		[DataMember(Name = "duration", Order = 2, EmitDefaultValue = false, IsRequired = false)]
		private string DurationSeconds =>
			Duration == TimeSpan.Zero
				? null
				: Duration.TotalSeconds.ToString(CultureInfo.InvariantCulture);
	}
}
