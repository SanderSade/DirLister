using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sander.DirLister.Core;

namespace Sander.DirLister.UI
{
	public sealed partial class MainForm
	{
		internal DoProgress ProgressDelegate { get; set; }

		internal delegate void DoProgress(int progress, string message);

		internal DoLog LogDelegate { get; set; }

		internal delegate void DoLog(TraceLevel level, string message);

		internal void SetProgress(int progress, string message)
		{
			Progress.Value = progress;
			ProgressLabel.Text = message;
			//if (progress >= 100)
			//	Close();
		}

		internal void SetLog(TraceLevel level, string message)
		{
			LogBox.AppendText($"[{DateTimeOffset.Now.ToLocalTime():G}] {level}: {message}{Environment.NewLine}");
		}

		internal void ConfigureCallbacks()
		{
			ProgressDelegate = SetProgress;
			LogDelegate = SetLog;

			_configuration.LoggingAction = (level, message) => Invoke(LogDelegate, level, message);
			_configuration.ProgressAction = (progress, message) => Invoke(ProgressDelegate, progress, message);
		}

		private void StartButton_Click(object sender, EventArgs e)
		{
			if (!UpdateConfiguration())
				return;

			ResetUi();
			StartButton.Enabled = false;
			MainTabs.SelectedTab = LogTab;
			var t = Task.Run(() => Core.DirLister.List(_configuration));
			
			StartButton.Enabled = true;
		}


		private void ResetUi()
		{
			LogBox.Clear();
			Progress.Value = 0;
			ProgressLabel.Text = string.Empty;
		}


		private bool UpdateConfiguration()
		{
			if (!ValidateOutputFolder())
				return false;

			if (!GetFormats(out var formats))
				return false;

			if (DirectoryList.Items.Count == 0)
			{
				MessageBox.Show(this,"No folders selected!", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			_configuration.InputFolders = DirectoryList.Items.OfType<ListViewItem>()
			                                           .Select(x => x.Text)
			                                           .ToList();

			_configuration.OutputFormats = formats;
			_configuration.OutputFolder = OutputFolder.Text;
			_configuration.Filter = GetFilter();
			_configuration.IncludeFileDates = IncludeFileDates.Checked;
			_configuration.IncludeHidden = IncludeHidden.Checked;
			_configuration.IncludeMediaInfo = IncludeMediaInfo.Checked;
			_configuration.IncludeSize = IncludeSize.Checked;
			_configuration.IncludeSubfolders = IncludeSubfolders.Checked;
			_configuration.OpenAfter = OpenAfter.Checked;

			return true;
		}


		private Filter GetFilter()
		{
			return null;
		}
	}
}
