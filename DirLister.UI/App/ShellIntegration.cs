using System;
using System.IO;
using System.Reflection;
using IWshRuntimeLibrary;


namespace Sander.DirLister.UI.App
{
	internal static class ShellIntegration
	{
		private static void CreateShortcut()
		{
			var shortcutLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"Microsoft\\Windows\\SendTo\\DirLister (list directory contents).lnk");
			var shell = new WshShell();
			var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
			var target = Assembly.GetEntryAssembly()
			                     .Location;
			shortcut.Description = "DirLister (list directory contents)";
			shortcut.IconLocation = $"{target}, 0";
			shortcut.TargetPath = target;
			shortcut.Save();
		}


		internal static void Create()
		{
			CreateShortcut();
		}
	}
}
