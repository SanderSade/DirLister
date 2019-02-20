using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.UI.DTO;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	public partial class MainForm : Form
	{
		private readonly Configuration _configuration;

		public MainForm(Configuration configuration, List<LogEntry> logs = null,
			IEnumerable<string> inputFolders = null)
		{
			_configuration = configuration;
			InitializeComponent();
			InitializeInput(inputFolders);
			InitializeOutput();

			if (logs != null && logs.Count > 0)
			{
				LogBox.Lines = logs.Select(x => x.ToString()).ToArray();
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
				if (!File.GetAttributes(folder).HasFlag(FileAttributes.Directory))
					continue;

				var directory = folder[folder.Length - 1] != Path.DirectorySeparatorChar
					? folder + Path.DirectorySeparatorChar
					: folder;
				AddFolderToList(directory);
				AddFolderToHistory(directory);
			}
		}


		private void LogBox_TextChanged(object sender, EventArgs e)
		{
			LogBox.SelectionStart = LogBox.Text.Length;
			LogBox.ScrollToCaret();
		}
	}
}
