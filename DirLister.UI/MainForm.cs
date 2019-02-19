using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.UI.DTO;

namespace Sander.DirLister.UI
{
	public partial class MainForm : Form
	{
		public MainForm(Configuration configuration, List<LogEntry> logs = null,
			IEnumerable<string> inputFolders = null)
		{
			InitializeComponent();
			InitializeInput(configuration, inputFolders);

			if (logs != null && logs.Count > 0)
			{
				LogBox.Lines = logs.Select(x => x.ToString()).ToArray();
				LogBox.SelectionStart = LogBox.Text.Length;
				LogBox.ScrollToCaret();
				LogBox.Refresh();
				MainTabs.SelectedTab = LogTab;

			}
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
	}
}
