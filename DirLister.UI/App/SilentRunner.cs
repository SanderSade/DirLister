using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.UI.DTO;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI.App
{
	internal sealed class SilentRunner
	{
		private readonly Configuration _configuration;
		private volatile bool _hasError;
		private ProgressForm _progressForm;

		internal SilentRunner(Configuration configuration)
		{
			_configuration = configuration;
		}

		internal void RunSilent(string[] folders)
		{
			_configuration.InputFolders = new List<string>(folders);

			var log = new ConcurrentBag<LogEntry>();


			_configuration.LoggingAction = delegate(TraceLevel level, string message)
			{
				if (level == TraceLevel.Error)
				{
					_progressForm.Invoke(_progressForm.ProgressDelegate, 100,
						message);
					_hasError = true;
				}

				log.Add(new LogEntry(level, message));
			};

			if (Settings.Default.ShowProgressWindow)
				RunSilentWithProgress();
			else
				Core.DirLister.List(_configuration);


			if (_hasError)
			{
				Application.Run(new MainForm(_configuration, log.ToList(), folders));
			}
			else
			{
				Mediator.AddToHistory(folders);
				Settings.Default.Save();
			}
		}

		private void RunSilentWithProgress()
		{
			_progressForm = new ProgressForm {TopMost = true, StartPosition = FormStartPosition.Manual};
			using (_progressForm)
			{
				_progressForm.Left = Screen.PrimaryScreen.WorkingArea.Right - _progressForm.Width - 20;
				_progressForm.Top = Screen.PrimaryScreen.WorkingArea.Bottom - _progressForm.Height - 30;

				_configuration.ProgressAction = delegate(int progress, string message)
				{
					if (_hasError)
						progress = 100;

					// ReSharper disable AccessToDisposedClosure
					_progressForm.Invoke(_progressForm.ProgressDelegate, progress,
						message);
				};

				var task = Task.Run(() => Core.DirLister.List(_configuration));
				Application.Run(_progressForm);
				task.GetAwaiter().GetResult();
			}
		}
	}
}
