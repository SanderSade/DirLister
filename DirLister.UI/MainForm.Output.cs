using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.Core.Application;
using Sander.DirLister.UI.App;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	public sealed partial class MainForm
	{
		private bool _isStartup;
		internal void InitializeOutput()
		{
			_isStartup = true;
			SetFormats();
			IncludeSize.Checked = _configuration.IncludeSize;
			IncludeFileDates.Checked = _configuration.IncludeFileDates;
			IncludeMediaInfo.Checked = _configuration.IncludeMediaInfo;

			OutputFolder.Text = _configuration.OutputFolder;
			OpenAfter.Checked = _configuration.OpenAfter;

			EnableShellCheck.Checked = Settings.Default.EnableShellIntegration;
			OpenUiCheck.Checked = Settings.Default.ShowUiFromShell;
			ProgressWindowCheck.Checked = Settings.Default.ShowProgressWindow;
			KeepOnTop.Checked = Settings.Default.KeepOnTop;
			_isStartup = false;
		}

		private void SetFormats()
		{
			var formats = OutFormats.Controls.OfType<CheckBox>().ToList();
			foreach (var format in _configuration.OutputFormats)
			{
				var ch = formats.FirstOrDefault(x =>
					string.Compare((string)x.Tag, format.ToString(), StringComparison.OrdinalIgnoreCase) == 0);

				if (ch != null)
					ch.Checked = true;
			}
		}

		private void EnableShellCheck_CheckedChanged(object sender, EventArgs e)
		{
			ProgressWindowCheck.Enabled = OpenUiCheck.Enabled = EnableShellCheck.Checked;

			if (EnableShellCheck.Checked)
			{
				if (!_isStartup)
					ShellIntegration.Create();
			}
			else
			{
				if (!_isStartup)
					ShellIntegration.Remove();

				OpenUiCheck.Checked = Settings.Default.ShowUiFromShell = false;
				ProgressWindowCheck.Checked = Settings.Default.ShowProgressWindow = false;
			}
		}

		private void OpenUiCheck_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.ShowUiFromShell = OpenUiCheck.Checked;
		}

		private void ProgressWindowCheck_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.ShowProgressWindow = ProgressWindowCheck.Checked;
		}


		private void SetDefault_Click(object sender, EventArgs e)
		{
			if (!ValidateOutputFolder()) return;

			if (!GetFormats(out var formats))
				return;

			Settings.Default.OutputFormats = new StringCollection();
			Settings.Default.OutputFormats.AddRange(formats.Select(x => x.ToString()).ToArray());

			Settings.Default.SelectedFilter = FilenameFilter.Text;
			Settings.Default.IncludeSize = IncludeSize.Checked;
			Settings.Default.IncludeFileDates = IncludeFileDates.Checked;
			Settings.Default.IncludeMediaInfo = IncludeMediaInfo.Checked;
			Settings.Default.OpenAfter = OpenAfter.Checked;
			Settings.Default.IncludeHidden = IncludeHidden.Checked;
			Settings.Default.IncludeSubfolders = IncludeSubfolders.Checked;
			Settings.Default.KeepOnTop = KeepOnTop.Checked;
			FirstRunLabel.Hide();
			Settings.Default.FirstRun = false;
			Settings.Default.Save();
			StartButton.Enabled = true;
		}


		private bool ValidateOutputFolder()
		{
			try
			{
				var path = Path.GetFullPath(OutputFolder.Text);
				Directory.CreateDirectory(path);
				Settings.Default.OutputFolder = path;
			}
			catch (Exception)
			{
				MessageBox.Show(this, "Output folder must be a valid directory!", "Invalid output path!",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			return true;
		}


		private bool GetFormats(out List<OutputFormat> formatList)
		{
			var formats = OutFormats.Controls.OfType<CheckBox>().Where(x => x.Checked).Select(x => (string)x.Tag).ToList();
			if (formats.Count == 0)
			{
				MessageBox.Show(this, "At least one output format must be set!", "No output formats!",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				formatList = null;
				return false;
			}

			
			formatList = formats.Select(x => (OutputFormat)Enum.Parse(typeof(OutputFormat), x, true))
			                    .ToList();
			
			return true;
		}

		private void SelectOutputFolder_Click(object sender, EventArgs e)
		{
			FolderSelectionDialog.Description = "Select output folder";
			FolderSelectionDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			FolderSelectionDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			if (FolderSelectionDialog.ShowDialog() == DialogResult.OK)
			{
				var directory = Utils.EnsureBackslash(FolderSelectionDialog.SelectedPath);
				OutputFolder.Text = directory;
			}
		}

		private void KeepOnTop_CheckedChanged(object sender, EventArgs e)
		{
			TopMost = KeepOnTop.Checked;
		}
	}
}
