using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using Microsoft.Win32;


namespace Sander.DirLister.UI.App
{
	internal static class ShellIntegration
	{
		private static void CreateShortcut()
		{
			var shortcutLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"Microsoft\\Windows\\SendTo\\DirLister (list files).lnk");
			var shell = new WshShell();
			var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
			var target = Assembly.GetEntryAssembly()
			                     .Location;
			shortcut.Description = "DirLister (list directory contents)";
			shortcut.IconLocation = $"{target}, 0";
			shortcut.TargetPath = target;
			shortcut.Save();
		}

		private static void CreateRegistryEntries()
		{
			CreateRegistryEntry(@"Software\Classes\Directory\", "L");
			CreateRegistryEntry(@"Software\Classes\Directory\Background\", "v");
			CreateRegistryEntry(@"Software\Classes\Drive\", "L");
			CreateRegistryEntry(@"Software\Classes\Drive\Background\", "v");
		}

		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		private static void CreateRegistryEntry(string parent, string value)
		{
			using (var directoryKey = Registry.CurrentUser.CreateSubKey(parent, true))
			{
				using (var shellKey = directoryKey.CreateSubKey("shell", true))
				{
					using (var sub = shellKey.CreateSubKey("DirLister (list files)"))
					{
						using (var commandKey = sub.CreateSubKey("Command"))
						{
							commandKey.SetValue(string.Empty, $"\"{Application.ExecutablePath}\" \"%{value}\"");
						}

						sub.SetValue("icon", $"\"{Application.ExecutablePath}\", 0");
					}
				}
			}
		}

		internal static void Create()
		{
			CreateShortcut();
			CreateRegistryEntries();
		}

		internal static void Remove()
		{
			throw new NotImplementedException();
		}
	}
}
