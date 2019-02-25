using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.Core.Application;
using Sander.DirLister.UI.DTO;
using Sander.DirLister.UI.Properties;

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
			Text = Program.VersionString;
			TopMost = Settings.Default.KeepOnTop;

			FilterTabs.SelectedTab = NoneTab;
			InitializeInput(inputFolders);
			InitializeOutput();

			if (logs != null && logs.Count > 0)
			{
				LogBox.Lines = logs.Select(x => x.ToString())
								   .ToArray();
				LogBox.SelectionStart = LogBox.Text.Length;
				LogBox.ScrollToCaret();
				MainTabs.SelectedTab = LogTab;
			}

			if (Settings.Default.FirstRun)
				MainTabs.SelectedTab = OutputTab;


			ConfigureCallbacks();
		}


		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}


		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			var folders = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach (var folder in folders)
			{
				if (!File.GetAttributes(folder)
						 .HasFlag(FileAttributes.Directory))
					continue;

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
				return;

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
	}
}
