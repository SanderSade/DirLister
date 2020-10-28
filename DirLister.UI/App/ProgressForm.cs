using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.UI.DTO;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI.App
{
	public partial class ProgressForm : Form
	{
		private readonly Configuration _configuration;
		private readonly ConcurrentBag<LogEntry> _log = new ConcurrentBag<LogEntry>();


		public ProgressForm(Configuration configuration)
		{
			_configuration = configuration;

			InitializeComponent();
			TopLabel.Text = Program.VersionString;
			ProgressDelegate = SetProgress;
		}


		internal DoProgress ProgressDelegate { get; }


		internal void SetProgress(int progress, string message)
		{
			ProgressBar.Value = progress;
			ProgressLabel.Text = message;
		}


		private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			History.Default.Save();
		}


		private void HideLabel_Click(object sender, EventArgs e)
		{
			Settings.Default.ShowProgressWindow = false;
			Hide();
		}


		private void ProgressForm_Load(object sender, EventArgs e)
		{
			Left = Screen.PrimaryScreen.WorkingArea.Right - Width - 20;
			Top = Screen.PrimaryScreen.WorkingArea.Bottom - Height - 30;

			_configuration.ProgressAction = (int progress, string message) =>
			{
				// ReSharper disable AccessToDisposedClosure
				Invoke(ProgressDelegate, progress,
					message);
			};

			_configuration.LoggingAction = (TraceLevel level, string message) => _log.Add(new LogEntry(level, message));
		}


		internal delegate void DoProgress(int progress, string message);


		private async void ProgressForm_Shown(object sender, EventArgs e)
		{
			var isSuccess = await Core.DirLister.ListAsync(_configuration).ConfigureAwait(false);
			if (!isSuccess)
			{
				await Task.Delay(500).ConfigureAwait(false);
				Hide();
				var mainForm = new MainForm(_configuration, _log.OrderBy(x => x.Timestamp).ToList(), _configuration.InputFolders);
				mainForm.ShowDialog(this);
			}

			Close();
		}
	}
}
