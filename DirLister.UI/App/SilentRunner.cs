using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.UI.DTO;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI.App
{
	internal sealed class SilentRunner
	{
		private readonly Configuration _configuration;

		internal SilentRunner(Configuration configuration)
		{
			_configuration = configuration;
		}

		internal void RunSilent(string[] folders)
		{
			_configuration.InputFolders = new List<string>(folders);


			if (Settings.Default.ShowProgressWindow)
			{
				RunSilentWithProgress();
				Mediator.AddToHistory(folders);
				return;
			}

			RunSilently(folders);
		}

		private void RunSilently(string[] folders)
		{
			var log = new ConcurrentBag<LogEntry>();
			_configuration.LoggingAction = delegate (TraceLevel level, string message)
			{
				log.Add(new LogEntry(level, message));
			};
			var result = Core.DirLister.List(_configuration);

			if (!result)
			{
				Thread.Sleep(500);
				Application.Run(new MainForm(_configuration, log.OrderBy(x => x.Timestamp).ToList(), folders));
			}
			else
			{
				Mediator.AddToHistory(folders);
			}
		}

		private void RunSilentWithProgress()
		{
			Application.Run(new ProgressForm(_configuration) { TopMost = true, StartPosition = FormStartPosition.Manual });
		}
	}
}
