using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.UI.App;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	public partial class MainForm
	{
		private bool _isStartup;
		internal void InitializeOutput()
		{
			_isStartup = true;
			SetFormats();
			SizeCheck.Checked = _configuration.IncludeSize;
			FileDateCheck.Checked = _configuration.IncludeFileDates;
			MediaInfoCheck.Checked = _configuration.IncludeMediaInfo;

			OutputFolder.Text = _configuration.OutputFolder;
			OpenAfter.Checked = _configuration.OpenAfter;

			EnableShellCheck.Checked = Settings.Default.EnableShellIntegration;
			OpenUiCheck.Checked = Settings.Default.ShowUiFromShell;
			ProgressWindowCheck.Checked = Settings.Default.ShowProgressWindow;
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
				return;
			}


			if (!GetFormats())
			{
				MessageBox.Show(this, "At least one output format must be set!", "No output formats!",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Settings.Default.IncludeSize = SizeCheck.Checked;
			Settings.Default.IncludeFileDates = FileDateCheck.Checked;
			Settings.Default.IncludeMediaInfo = MediaInfoCheck.Checked;
			Settings.Default.OpenAfter = OpenAfter.Checked;
			Settings.Default.IncludeHidden = IncludeHidden.Checked;
			Settings.Default.IncludeSubfolders = Recursive.Checked;

			FirstRunLabel.Hide();
			Settings.Default.FirstRun = false;
			Settings.Default.Save();
			StartButton.Enabled = true;
		}

		private bool GetFormats()
		{
			var formats = OutFormats.Controls.OfType<CheckBox>().Where(x => x.Checked).Select(x => (string)x.Tag).ToList();
			if (formats.Count == 0)
				return false;

			Settings.Default.OutputFormats = new StringCollection();
			Settings.Default.OutputFormats.AddRange(formats.Select(x => (OutputFormat)Enum.Parse(typeof(OutputFormat), x, true)).Select(x => x.ToString()).ToArray());
			return true;
		}
	}
}
