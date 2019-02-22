using System;
using System.Diagnostics;
using System.Linq;
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

		private async void StartButton_Click(object sender, EventArgs e)
		{
			if (!UpdateConfiguration())
				return;

			ResetUi();
			StartButton.Enabled = false;
			MainTabs.SelectedTab = LogTab;
			await Core.DirLister.ListAsync(_configuration);			
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
				MessageBox.Show(this, "No folders selected!", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			_configuration.InputFolders = DirectoryList.Items.OfType<ListViewItem>()
													   .Select(x => x.Text)
													   .ToList();

			_configuration.OutputFormats = formats;
			_configuration.OutputFolder = OutputFolder.Text;
			var filter = GetFilter();
			if (filter == null)
				return false;

			_configuration.Filter = filter;
			_configuration.IncludeFileDates = IncludeFileDates.Checked;
			_configuration.IncludeHidden = IncludeHidden.Checked;
			_configuration.IncludeMediaInfo = IncludeMediaInfo.Checked;
			_configuration.IncludeSize = IncludeSize.Checked;
			_configuration.IncludeSubfolders = IncludeSubfolders.Checked;
			_configuration.OpenAfter = OpenAfter.Checked;

			return true;
		}


		/// <summary>
		/// Returns filter or null in case of error
		/// </summary>
		/// <returns></returns>
		private Filter GetFilter()
		{
			switch (FilterTabs.SelectedIndex)
			{
				case 1://wildcard
					if (WildcardList.Items.Count == 0)
						return new Filter();
					return new Filter(WildcardList.Items.Cast<ListViewItem>().Select(x => x.Text).ToArray());
				case 2: //regex
					if (string.IsNullOrWhiteSpace(RegexCombo.Text))
						return new Filter();
					string error = null;
					var regex = ValidateRegex(ref error);
					if (regex != null)
					{
						AddRegexToHistory(regex);
						return new Filter(regex);
					}
					MessageBox.Show(this, $"Invalid regex:{Environment.NewLine}{Environment.NewLine}{error}", "Invalid filter!", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return null;
				default://none
					return new Filter();

			}
		}
	}
}
