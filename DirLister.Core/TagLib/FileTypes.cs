using System;
using System.Collections.Generic;
using Sander.DirLister.Core.TagLib.Mpeg;

namespace Sander.DirLister.Core.TagLib
{
	/// <summary>
	///     This static class provides a mechanism for registering file
	///     classes and mime-types, to be used when constructing a class via
	///     <see cref="File.Create(string)" />.
	/// </summary>
	/// <seealso cref="SupportedMimeType" />
	public static class FileTypes
	{
		/// <summary>
		///     Contains a static array of file types contained in the
		///     TagLib# assembly.
		/// </summary>
		/// <remarks>
		///     A static Type array is used instead of getting types by
		///     reflecting the executing assembly as Assembly.GetTypes is
		///     very inefficient and leaks every type instance under
		///     Mono. Not reflecting taglib-sharp.dll saves about 120KB
		///     of heap.
		/// </remarks>
		private static readonly Type[] StaticFileTypes =
		{
			typeof(Aac.File), typeof(Aiff.File), typeof(Ape.File), typeof(Asf.File), typeof(Audible.File), typeof(Dsf.File), typeof(Flac.File),
			typeof(Matroska.File), typeof(Mpeg4.File), typeof(AudioFile), typeof(Mpeg.File), typeof(Mpc.File), typeof(Ogg.File), typeof(Riff.File),
			typeof(WavPack.File)
		};


		/// <summary>
		///     Constructs and initializes the <see cref="FileTypes" />
		///     class by registering the default types.
		/// </summary>
		static FileTypes()
		{
			AvailableTypes = new Dictionary<string, Type>();

			foreach (var type in StaticFileTypes)
			{
				var attrs = Attribute.GetCustomAttributes(type,
					typeof(SupportedMimeType), false);

				if (attrs.Length == 0)
				{
					return;
				}

				foreach (SupportedMimeType attr in attrs)
				{
					AvailableTypes.Add(attr.MimeType, type);
				}
			}

			//To get all supported extensions
			//var list = string.Join(", ", StaticFileTypes.Select(type => Attribute.GetCustomAttributes(type,
			//	                                         typeof(SupportedMimeType), false))
			//                                 .OfType<SupportedMimeType[]>()
			//                                 .SelectMany(x => x)
			//                                 .Where(x => !string.IsNullOrWhiteSpace(x.Extension))
			//                                 .Select(x => x.Extension)
			//                                 .OrderBy(x => x));
		}


		/// <summary>
		///     Gets a dictionary containing all the supported mime-types
		///     and file classes used by <see cref="File.Create(string)" />.
		/// </summary>
		/// <value>
		///     A <see cref="T:System.Collections.Generic.IDictionary`2" /> object containing the
		///     supported mime-types.
		/// </value>
		public static Dictionary<string, Type> AvailableTypes { get; }
	}
}
