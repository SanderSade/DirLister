using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Sander.DirLister.Core.Application;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	public sealed partial class MainForm
	{
		private void AddFolderToHistory(string directory)
		{
			Mediator.AddToHistory(directory);
			while (HistoryMenu.Items.Count > 2)
				HistoryMenu.Items.RemoveAt(2);
			AddHistoryToMenu();
		}

		private void AddHistoryToMenu()
		{
			HistoryMenu.Items.AddRange(
				// ReSharper disable once CoVariantArrayConversion
				History.Default.DirectoryHistory
					.Cast<string>()
					.Distinct()
					.Select(x =>
					{
						var menuItem = new ToolStripMenuItem(x);
						menuItem.Click += delegate(object sender, EventArgs args)
						{
							if (sender is ToolStripMenuItem item) AddFolderToList(item.Text);
						};
						return menuItem;
					})
					.ToArray());
		}


		private void AddFolderToList(string directory)
		{
			var addToList = true;
			foreach (ListViewItem listItem in DirectoryList.Items)
				if (string.Compare(listItem.Text, directory, StringComparison.OrdinalIgnoreCase) == 0)
					addToList = false;

			if (addToList) DirectoryList.Items.Add(new ListViewItem {Text = directory});
		}


		/// <summary>
		///     Remove all items from DirectoryList
		/// </summary>
		private void RemoveAll_Click(object sender, EventArgs e)
		{
			DirectoryList.Items.Clear();
		}


		/// <summary>
		///     Open folder in Explorer
		/// </summary>
		private void DirectoryList_DoubleClick(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", DirectoryList.SelectedItems[0]
				.Text);
		}


		/// <summary>
		///     Clear history menu
		/// </summary>
		private void HistoryClearMenuItem_Click(object sender, EventArgs e)
		{
			var count = HistoryMenu.Items.Count;
			if (count == 2)
				return;
			for (var i = 2; i < count; i++) HistoryMenu.Items.RemoveAt(2);
			History.Default.DirectoryHistory.Clear();
			History.Default.Save();
		}


		private void InitializeInput(IEnumerable<string> inputFolders)
		{
			if (History.Default?.DirectoryHistory != null)
				AddHistoryToMenu();

			if (History.Default?.WildcardFilter != null)
				WildcardEdit.Items.AddRange(History.Default.WildcardFilter.Cast<string>().Distinct().ToArray());

			if (History.Default?.RegexFilter != null)
				RegexCombo.Items.AddRange(History.Default.RegexFilter.Cast<string>().Distinct().ToArray());

			if (inputFolders != null)
				foreach (var folder in inputFolders)
					AddFolderToList(folder);

			IncludeHidden.Checked = _configuration.IncludeHidden;
			IncludeSubfolders.Checked = _configuration.IncludeSubfolders;

			FirstRunLabel.Visible = Settings.Default.FirstRun;
			StartButton.Enabled = !Settings.Default.FirstRun;
			ProgressLabel.Text = string.Empty;
		}


		private void DirectoryList_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				if (DirectoryList.FocusedItem.Bounds.Contains(e.Location))
				{
					DirectoryMenu.Items[nameof(MoveUp)]
							.Visible = DirectoryList.FocusedItem.Index > 0;

					DirectoryMenu.Items[nameof(MoveDown)]
							.Visible = DirectoryList.FocusedItem.Index < DirectoryList.Items.Count - 1;
					DirectoryMenu.Show(Cursor.Position);
				}
		}


		private void MoveUp_Click(object sender, EventArgs e)
		{
			var item = DirectoryList.FocusedItem;
			var position = item.Index;
			DirectoryList.FocusedItem.Remove();
			DirectoryList.Items.Insert(position - 1, item);
		}


		private void MoveDown_Click(object sender, EventArgs e)
		{
			var item = DirectoryList.FocusedItem;
			var position = item.Index;
			DirectoryList.FocusedItem.Remove();
			DirectoryList.Items.Insert(position + 1, item);
		}


		private void RemoveFolder_Click(object sender, EventArgs e)
		{
			DirectoryList.FocusedItem.Remove();
		}

		private void BrowseButton_Click(object sender, EventArgs e)
		{
			FolderSelectionDialog.Description = "Select source folder";
			FolderSelectionDialog.RootFolder = Environment.SpecialFolder.MyComputer;

			if (FolderSelectionDialog.ShowDialog() == DialogResult.OK)
			{
				var directory = Utils.EnsureBackslash(FolderSelectionDialog.SelectedPath);
				AddFolderToList(directory);
				AddFolderToHistory(directory);
			}
		}

		private void LabelHomepage_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				Process.Start("explorer.exe",
					Path.GetDirectoryName(ConfigurationManager
						.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath));
			else
				Process.Start("https://github.com/SanderSade/DirLister");
		}

		private void WildcardEdit_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(WildcardEdit.Text))
				AddWildcard(WildcardEdit.Text);
		}

		private void AddWildcard(string wildcard)
		{
			var addToList = true;
			foreach (ListViewItem listItem in WildcardList.Items)
				if (string.Compare(listItem.Text, wildcard, StringComparison.OrdinalIgnoreCase) == 0)
					addToList = false;

			if (addToList)
			{
				WildcardList.Items.Add(new ListViewItem {Text = wildcard});
				WildcardEdit.Text = string.Empty;

				var wildcards = WildcardEdit.Items.Cast<string>().ToList();
				if (wildcards
					.Any(x => string.Compare(x, wildcard, StringComparison.OrdinalIgnoreCase) == 0))
					return;

				WildcardEdit.Items.Add(wildcard);

				if (History.Default.WildcardFilter == null)
					History.Default.WildcardFilter = new StringCollection();

				wildcards.Insert(0, wildcard);
				History.Default.WildcardFilter.AddRange(wildcards.Take(History.Default.FilterHistoryLength).ToArray());

				History.Default.Save();
			}
		}

		private void AddWildcardButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(WildcardEdit.Text))
				AddWildcard(WildcardEdit.Text);
		}

		private void ClearWildcardsButton_Click(object sender, EventArgs e)
		{
			WildcardList.Items.Clear();
		}

		private void ValidateRegexButton_Click(object sender, EventArgs e)
		{
			RegexErrorLabel.Text = string.Empty;
			RegexErrorLabel.ForeColor = Color.Black;
			if (string.IsNullOrWhiteSpace(RegexCombo.Text))
				return;

			string validationError = null;
			var regex = ValidateRegex(ref validationError);
			if (regex != null)
			{
				RegexErrorLabel.Text = "Valid!";
				AddRegexToHistory(regex);
			}
			else
			{
				RegexErrorLabel.ForeColor = Color.Red;
				RegexErrorLabel.Text = validationError;
			}
		}

		private Regex ValidateRegex(ref string regexError)
		{
			try
			{
				return new Regex(RegexCombo.Text,
					RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
			}
			catch (Exception e)
			{
				regexError = e.Message;
				return null;
			}
		}

		private void RegexCombo_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				ValidateRegexButton_Click(sender, e);
			else
				RegexErrorLabel.Text = string.Empty;
		}

		private void AddRegexToHistory(Regex regex)
		{
			var regexSource = regex.ToString();

			var regexes = RegexCombo.Items.Cast<string>().ToList();
			if (regexes.Any(x => string.Compare(x, regexSource, StringComparison.OrdinalIgnoreCase) == 0))
				return;

			RegexCombo.Items.Add(regexSource);

			if (History.Default.RegexFilter == null)
				History.Default.RegexFilter = new StringCollection();

			regexes.Insert(0, regexSource);
			History.Default.RegexFilter.AddRange(regexes.Take(History.Default.FilterHistoryLength).ToArray());

			History.Default.Save();
		}
	}
}
