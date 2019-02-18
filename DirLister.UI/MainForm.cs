using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Sander.DirLister.UI
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			FilenameFilter.SelectedIndex = 0;
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
				var add = true;

				foreach (ListViewItem listItem in DirectoryList.Items)
				{
					if (string.Compare(listItem.Text, directory, StringComparison.OrdinalIgnoreCase) == 0)
						add = false;
				}

				if (add)
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
	}
}
