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
			if (Settings.Default.EnableShellIntegration)
			{
				Task.Run(() => ShellIntegration.Create());
			}
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());

			Application.ApplicationExit += (sender, args) => { Settings.Default.Save(); };
		}
	}
}
