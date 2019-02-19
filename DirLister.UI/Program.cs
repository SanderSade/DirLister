using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sander.DirLister.UI.App;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	internal static class Program
	{
		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(params string[] folders)
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

			if (Settings.Default.EnableShellIntegration) Task.Run(() => ShellIntegration.Create());

			Application.ApplicationExit += (sender, args) => Settings.Default.Save();


			AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs args)
			{
				LogUnhandledException(args.ExceptionObject as Exception,
					$"Uncaught exception: {sender}, terminating: {args.IsTerminating}");
			};

			TaskScheduler.UnobservedTaskException += delegate(object sender, UnobservedTaskExceptionEventArgs args)
			{
				LogUnhandledException(args.Exception,
					$"Uncaught task exception: {sender}");
				args.SetObserved();
			};

			Mediator.Run(folders);
		}


		private static void LogUnhandledException(Exception ex, string message)
		{
			MessageBox.Show($"Fatal error. {message}\r\n\r\n{ex}",
				"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Environment.Exit(0);
		}
	}
}
