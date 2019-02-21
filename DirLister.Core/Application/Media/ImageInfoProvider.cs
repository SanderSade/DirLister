using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Sander.DirLister.Core.Application.Media
{
	internal sealed class ImageInfoProvider: BaseProvider
	{

		internal ImageInfoProvider(Configuration configuration):base(configuration)
		{
			 var extensions = GetAdditionalExtensions();

			 //natively supported
			extensions.Add(".PNG,.JPEG,.JPG,.GIF,.BMP,.TIF,.TIFF");
			var extSplit = string.Join(",", extensions).Split(',').Select(x => x.Trim('.', ' ')).OrderBy(x => x);

			SupportedExtensions = new HashSet<string>(extSplit, StringComparer.OrdinalIgnoreCase);

			Configuration.Log(TraceLevel.Info, $"Supported image extensions: {string.Join(", ", SupportedExtensions.Select(x => x.ToLowerInvariant()))}");
		}


		internal void GetImageInfo(FileEntry entry)
		{
			try
			{
				using (var stream = new FileStream(entry.Fullname, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					var bitmapFrame = BitmapFrame.Create(stream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
					entry.MediaInfo = new MediaInfo(MediaType.Image)
					{
						Width = bitmapFrame.PixelWidth,
						Height = bitmapFrame.PixelHeight,
						BitsPerPixel = bitmapFrame.Format.BitsPerPixel
					};
				}
			}
			catch (Exception e)
			{
				Configuration.Log(TraceLevel.Warning,
					$"Error getting image information for \"{entry.Fullname}\":{Environment.NewLine}, {e.Message}");
			}
		}


		/// <summary>
		/// Gets a list of additionally registered WIC decoder extensions
		/// Modified from https://stackoverflow.com/a/36391517/3248515
		/// Returns list of comma-separated extensions, including the . (.ARW,.CR2,.CRW,.ERF,.KDC,.MRW,.NEF,.NRW...)
		/// </summary>
		/// <returns></returns>
		internal static List<string> GetAdditionalExtensions()
		{
			var result = new List<string>();

			string baseKeyPath;

			// If we are a 32 bit process running on a 64 bit operating system,
			// we find our config in Wow6432Node subkey
			if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
			{
				baseKeyPath = "Wow6432Node\\CLSID";
			}
			else
			{
				baseKeyPath = "CLSID";
			}

			var baseKey = Registry.ClassesRoot.OpenSubKey(baseKeyPath, false);
			var categoryKey = baseKey?.OpenSubKey("{7ED96837-96F0-4812-B211-F13C24117ED3}" + "\\instance", false);
			if (categoryKey != null)
			{
				// Read the guids of the registered decoders
				var codecGuids = categoryKey.GetSubKeyNames();

				foreach (var codecGuid in codecGuids)
				{
					// Read the properties of the single registered decoder
					var codecKey = baseKey.OpenSubKey(codecGuid);
					if (codecKey != null)
					{
						result.Add(Convert.ToString(codecKey.GetValue("FileExtensions", string.Empty)));
					}
				}
			}

			return result;
		}
	}
}
