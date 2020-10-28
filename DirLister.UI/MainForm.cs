using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Sander.DirLister.Core.Application;
using Sander.DirLister.UI.App;
using Sander.DirLister.UI.DTO;
using Sander.DirLister.UI.Properties;
using Configuration = Sander.DirLister.Core.Configuration;

namespace Sander.DirLister.UI
{
	public sealed partial class MainForm : Form
	{
		private readonly Configuration _configuration;


		public MainForm(Configuration configuration,
			List<LogEntry> logs = null,
			IEnumerable<string> inputFolders = null)
		{
			_configuration = configuration;
			InitializeComponent();
			VersionLabel.Text = Text = Program.VersionString;


			TopMost = Settings.Default.KeepOnTop;

			FilterTabs.SelectedTab = NoneTab;
			InitializeInput(inputFolders);
			InitializeOutput();

			if (logs?.Count > 0)
			{
				LogBox.Lines = logs.Select(x => x.ToString())
					.ToArray();

				LogBox.SelectionStart = LogBox.Text.Length;
				LogBox.ScrollToCaret();
				MainTabs.SelectedTab = LogTab;
			}

			if (Settings.Default.FirstRun)
			{
				MainTabs.SelectedTab = OutputTab;
			}


			ConfigureCallbacks();
		}


		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}


		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			var folders = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach (var folder in folders)
			{
				if ((File.GetAttributes(folder)
& FileAttributes.Directory) == 0)
				{
					continue;
				}

				var directory = Utils.EnsureBackslash(folder);
				AddFolderToList(directory);
				AddFolderToHistory(directory);
			}
		}


		private void LogBox_TextChanged(object sender, EventArgs e)
		{
			LogBox.SelectionStart = LogBox.Text.Length;
			LogBox.ScrollToCaret();
		}


		private void MainForm_Load(object sender, EventArgs e)
		{
			if (Settings.Default.FirstRun || History.Default.WindowSize == Rectangle.Empty)
			{
				return;
			}

			//default is Normal and we never want to minimize the on load
			if (History.Default.WindowState == FormWindowState.Maximized)
			{
				WindowState = FormWindowState.Maximized;
				return;
			}

			var size = History.Default.WindowSize;

			Top = size.Top;
			Left = size.Left;
			Width = size.Width;
			Height = size.Height;
		}


		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			History.Default.WindowState = WindowState;
			if (WindowState != FormWindowState.Maximized)
			{
				History.Default.WindowSize = new Rectangle(Location, Size);
			}

			History.Default.Save();
		}


		private void ConfigurationFolderButton_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe",
				Path.GetDirectoryName(ConfigurationManager
					.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath));
		}


		private void ProgramFolderButton_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe",
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
		}


		private async void UpdateCheck_Click(object sender, EventArgs e)
		{
			UpdateCheck.Enabled = false;
			try
			{
				SetLog(TraceLevel.Info, "Checking for updates");
				using (var client = new WebClient())
				{
					var version = await client.DownloadStringTaskAsync(
						new Uri("https://raw.githubusercontent.com/SanderSade/DirLister/master/Version.txt")).ConfigureAwait(false);

					version = version.Trim();
					if (string.Equals(version, Program.Version, StringComparison.OrdinalIgnoreCase))
					{
						SetLog(TraceLevel.Info, $"Found \"{version}\". No updates available");
						MessageBox.Show("You have the latest version", "No updates available", MessageBoxButtons.OK,
							MessageBoxIcon.Information);

						return;
					}

					SetLog(TraceLevel.Info, $"Update found: \"{version}\", fetching release notes...");
					var releaseNotes = await client.DownloadStringTaskAsync(
						new Uri("https://raw.githubusercontent.com/SanderSade/DirLister/master/VersionHistory.html")).ConfigureAwait(false);

					SetLog(TraceLevel.Info, "Received release notes");

					using (var historyForm = new VersionHistoryForm(version, releaseNotes))
					{
						historyForm.ShowDialog(this);
					}
				}
			}
			catch (Exception ex)
			{
				var message = $"Error getting version information:\r\n{ex}";
				SetLog(TraceLevel.Error, message);
				MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				UpdateCheck.Enabled = true;
			}
		}
	}
}
