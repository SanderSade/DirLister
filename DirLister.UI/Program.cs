using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sander.DirLister.UI.App;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(params string[] folders)
		{
			if (folders != null && folders.Length == 1)
			{
				var command = folders[0].Trim('/', '-');
				if (string.Compare(command, "uninstall", StringComparison.OrdinalIgnoreCase) == 0
				    || string.Compare(command, "remove", StringComparison.OrdinalIgnoreCase) == 0)
				{
					ShellIntegration.Remove();
					Environment.Exit(0);
					return;
				}
			}

			if (Settings.Default.EnableShellIntegration)
			{
				Task.Run(() => ShellIntegration.Create());
			}

			Application.ApplicationExit += (sender, args) =>
			{
				Settings.Default.Save();

			};
			Mediator.Run(folders);




		}
	}
}
