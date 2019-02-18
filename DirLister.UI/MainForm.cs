using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			FilenameFilter.SelectedIndex = 0;
			if (Settings.Default?.DirectoryHistory != null)
			{
				foreach (var folder in Settings.Default.DirectoryHistory)
				{
					AddFolderToHistory(folder, true);
				}
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

				var directory = folder[folder.Length - 1] != Path.DirectorySeparatorChar ? folder + Path.DirectorySeparatorChar : folder;
				AddFolderToList(directory);
				AddFolderToHistory(directory);
			}

		}


		private void AddFolderToHistory(string directory, bool isStartup = false)
		{
			var addToHistory = true;

			foreach (var historyMenuItem in HistoryMenu.Items.OfType<ToolStripMenuItem>())
			{
				if (string.Compare(historyMenuItem.Text, directory, StringComparison.OrdinalIgnoreCase) == 0)
					addToHistory = false;
			}

			if (addToHistory)
			{
				if (HistoryMenu.Items.Count >= 10)
					HistoryMenu.Items.RemoveAt(9);
				var menuItem = new ToolStripMenuItem(directory) { Tag = "folder" };
				menuItem.Click += delegate(object sender, EventArgs args)
				                  {
					                  if (sender is ToolStripMenuItem item)
					                  {
										  AddFolderToList(item.Text);
					                  }

				                  };
				HistoryMenu.Items.Insert(2, menuItem);
			}

			if (!isStartup)
			{
				Task.Run(() =>
					 {
						 Settings.Default.DirectoryHistory = new StringCollection();
						 foreach (var historyMenuItem in HistoryMenu.Items.OfType<ToolStripMenuItem>())
						 {
							 if (historyMenuItem.Text == HistoryClearMenuItem.Text)
								 continue;

							 Settings.Default.DirectoryHistory.Add(historyMenuItem.Text);
						 }
					 });
			}
		}


		private void AddFolderToList(string directory)
		{
			var addToList = true;
			foreach (ListViewItem listItem in DirectoryList.Items)
			{
				if (string.Compare(listItem.Text, directory, StringComparison.OrdinalIgnoreCase) == 0)
					addToList = false;
			}

			if (addToList)
			{
				DirectoryList.Items.Add(new ListViewItem { Text = directory });
			}
		}


		private void RemoveAll_Click(object sender, EventArgs e)
		{
			DirectoryList.Items.Clear();
		}

		private void DirectoryList_DoubleClick(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", DirectoryList.SelectedItems[0]
													   .Text);
		}

		private void HistoryClearMenuItem_Click(object sender, EventArgs e)
		{
			var count = HistoryMenu.Items.Count;
			for (int i = 2; i < count; i++)
			{
				HistoryMenu.Items.RemoveAt(2);
			}
		}
	}
}
