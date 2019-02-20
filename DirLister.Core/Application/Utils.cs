using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Sander.DirLister.Core.Application
{
	/// <summary>
	/// Misc utils
	/// </summary>
	public static class Utils
	{

		/// <summary>
		/// Found from the internets, heavily modified for optimization and flexibility
		/// </summary>
		/// <param name="size">Size in bytes</param>
		/// <param name="numberFormat">Format of the returned number. Defaults to 0.##</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string ReadableSize(long size, string numberFormat = "0.##")
		{
			size = Math.Abs(size);

			string suffix;
			float readable;
			switch (true)
			{
				case true when size >= 0x1000000000000000:
					suffix = "EB";
					readable = size >> 50;
					break;

				case true when size >= 0x4000000000000:

					suffix = "PB";
					readable = size >> 40;
					break;

				case true when size >= 0x10000000000:

					suffix = "TB";
					readable = size >> 30;
					break;

				case true when size >= 0x40000000:
					suffix = "GB";
					readable = size >> 20;
					break;

				case true when size >= 0x100000:
					suffix = "MB";
					readable = size >> 10;
					break;

				case true when size >= 0x400:
					suffix = "KB";
					readable = size;
					break;
				default:
					return FormattableString.Invariant($"{size.ToString(numberFormat, CultureInfo.InvariantCulture)}B");
			}

			return FormattableString.Invariant($"{(readable / 1024).ToString(numberFormat, CultureInfo.InvariantCulture)}{suffix}");
		}

		/// <summary>
		/// Return folder name. Should be faster than inbuilt method.
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string GetPath(string fullname)
		{
			return fullname.Substring(0, fullname.LastIndexOf(Path.DirectorySeparatorChar) + 1);
		}

		/// <summary>
		/// Return pathless filename. Should be faster than inbuilt method
		/// </summary>
		/// <param name="fullname"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string GetFileName(string fullname)
		{
			return fullname.Substring(fullname.LastIndexOf(Path.DirectorySeparatorChar) + 1);
		}

		/// <summary>
		/// Return extension without the period (.). Should be slightly faster than inbuilt method
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string GetExtension(string filename)
		{
			var lastDot = filename.LastIndexOf('.');
			if (lastDot == -1 || lastDot == 0) //not found or .file
				return null;
			return filename.Substring(lastDot + 1);
		}

		/// <summary>
		/// DateTimeOffset from FILETIME.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable 618
		internal static DateTimeOffset FileTimeToDateTimeOffset(FILETIME filetime)
#pragma warning restore 618
		{
			return DateTimeOffset.FromFileTime(((long)filetime.dwHighDateTime << 32) + filetime.dwLowDateTime);
		}

		/// <summary>
		/// Ensure path ends with backslash
		/// </summary>
		/// <param name="directory"></param>
		/// <returns></returns>
		public static string EnsureBackslash(string directory)
		{
			return directory[directory.Length - 1] != Path.DirectorySeparatorChar
				? directory + Path.DirectorySeparatorChar
				: directory;
		}


		[DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int WNetGetConnection(
			[MarshalAs(UnmanagedType.LPTStr)] string localName,
			[MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
			ref int length);

		/// <summary>
		/// From https://www.wiredprairie.us/blog/index.php/archives/22, modified
		/// Given a path, returns the UNC path or the original. (No exceptions
		/// are raised by this function directly). For example, "P:\2008-02-29"
		/// might return: "\\networkserver\Shares\Photos\2008-02-09"
		/// </summary>
		/// <param name="originalPath">The path to convert to a UNC Path</param>
		/// <returns>A UNC path. If a network drive letter is specified, the
		/// drive letter is converted to a UNC or network path. If the
		/// originalPath cannot be converted, it is returned unchanged.</returns>
		internal static string GetUncPath(string originalPath)
		{
			var sb = new StringBuilder(512);
			var size = sb.Capacity;

			// look for the {LETTER}: combination ...
			if (originalPath.Length > 2 && originalPath[1] == Path.VolumeSeparatorChar)
			{
				// don't use char.IsLetter here - as that can be misleading
				// the only valid drive letters are a-z && A-Z.
				var c = originalPath[0];
				if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
				{
					var error = WNetGetConnection(originalPath.Substring(0, 2),
						sb, ref size);
					if (error == 0)
					{

						var path = Path.GetFullPath(originalPath)
							.Substring(Path.GetPathRoot(originalPath).Length);
						return Path.Combine(sb.ToString().TrimEnd(), path);
					}
				}
			}

			return originalPath;
		}
	}
}
