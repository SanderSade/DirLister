using Sander.DirLister.UI.App;

namespace Sander.DirLister.UI
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("c:\\");
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("C:\\Users\\Sander\\Source\\Repos\\DirLister\\DirLister.UI");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.BottomPanel = new System.Windows.Forms.Panel();
			this.FirstRunLabel = new System.Windows.Forms.Label();
			this.ProgressLabel = new System.Windows.Forms.Label();
			this.Progress = new System.Windows.Forms.ProgressBar();
			this.StartButton = new System.Windows.Forms.Button();
			this.HistoryMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.HistoryClearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HistorySeparator = new System.Windows.Forms.ToolStripSeparator();
			this.LogTab = new System.Windows.Forms.TabPage();
			this.LogBox = new System.Windows.Forms.TextBox();
			this.OutputTab = new System.Windows.Forms.TabPage();
			this.InputTab = new System.Windows.Forms.TabPage();
			this.FilterBox = new System.Windows.Forms.GroupBox();
			this.IncludeHidden = new System.Windows.Forms.CheckBox();
			this.Recursive = new System.Windows.Forms.CheckBox();
			this.FilenameFilter = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.BrowseButton = new System.Windows.Forms.Button();
			this.RemoveAll = new System.Windows.Forms.Label();
			this.DirectoryList = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.HistoryButton = new Sander.DirLister.UI.App.SplitButton();
			this.MainTabs = new System.Windows.Forms.TabControl();
			this.BottomPanel.SuspendLayout();
			this.HistoryMenu.SuspendLayout();
			this.LogTab.SuspendLayout();
			this.InputTab.SuspendLayout();
			this.FilterBox.SuspendLayout();
			this.MainTabs.SuspendLayout();
			this.SuspendLayout();
			// 
			// BottomPanel
			// 
			this.BottomPanel.Controls.Add(this.FirstRunLabel);
			this.BottomPanel.Controls.Add(this.ProgressLabel);
			this.BottomPanel.Controls.Add(this.Progress);
			this.BottomPanel.Controls.Add(this.StartButton);
			this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.BottomPanel.Location = new System.Drawing.Point(0, 380);
			this.BottomPanel.Name = "BottomPanel";
			this.BottomPanel.Size = new System.Drawing.Size(945, 81);
			this.BottomPanel.TabIndex = 0;
			// 
			// FirstRunLabel
			// 
			this.FirstRunLabel.AutoSize = true;
			this.FirstRunLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FirstRunLabel.ForeColor = System.Drawing.Color.Red;
			this.FirstRunLabel.Location = new System.Drawing.Point(156, 17);
			this.FirstRunLabel.Name = "FirstRunLabel";
			this.FirstRunLabel.Size = new System.Drawing.Size(391, 16);
			this.FirstRunLabel.TabIndex = 3;
			this.FirstRunLabel.Text = "First run - choose the default output options and click \"Set default\"";
			// 
			// ProgressLabel
			// 
			this.ProgressLabel.AutoSize = true;
			this.ProgressLabel.Location = new System.Drawing.Point(407, 50);
			this.ProgressLabel.Name = "ProgressLabel";
			this.ProgressLabel.Size = new System.Drawing.Size(52, 13);
			this.ProgressLabel.TabIndex = 2;
			this.ProgressLabel.Text = "Waiting...";
			// 
			// Progress
			// 
			this.Progress.ForeColor = System.Drawing.Color.DarkRed;
			this.Progress.Location = new System.Drawing.Point(157, 45);
			this.Progress.Name = "Progress";
			this.Progress.Size = new System.Drawing.Size(244, 23);
			this.Progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.Progress.TabIndex = 1;
			// 
			// StartButton
			// 
			this.StartButton.BackColor = System.Drawing.SystemColors.ControlDark;
			this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartButton.Location = new System.Drawing.Point(12, 6);
			this.StartButton.Margin = new System.Windows.Forms.Padding(20);
			this.StartButton.Name = "StartButton";
			this.StartButton.Size = new System.Drawing.Size(121, 63);
			this.StartButton.TabIndex = 0;
			this.StartButton.Text = "Start";
			this.StartButton.UseVisualStyleBackColor = false;
			// 
			// HistoryMenu
			// 
			this.HistoryMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HistoryClearMenuItem,
            this.HistorySeparator});
			this.HistoryMenu.Name = "HistoryMenu";
			this.HistoryMenu.Size = new System.Drawing.Size(102, 32);
			// 
			// HistoryClearMenuItem
			// 
			this.HistoryClearMenuItem.Name = "HistoryClearMenuItem";
			this.HistoryClearMenuItem.Size = new System.Drawing.Size(101, 22);
			this.HistoryClearMenuItem.Text = "Clear";
			this.HistoryClearMenuItem.Click += new System.EventHandler(this.HistoryClearMenuItem_Click);
			// 
			// HistorySeparator
			// 
			this.HistorySeparator.Name = "HistorySeparator";
			this.HistorySeparator.Size = new System.Drawing.Size(98, 6);
			// 
			// LogTab
			// 
			this.LogTab.Controls.Add(this.LogBox);
			this.LogTab.Location = new System.Drawing.Point(4, 29);
			this.LogTab.Name = "LogTab";
			this.LogTab.Padding = new System.Windows.Forms.Padding(3);
			this.LogTab.Size = new System.Drawing.Size(937, 347);
			this.LogTab.TabIndex = 2;
			this.LogTab.Text = "Log";
			this.LogTab.UseVisualStyleBackColor = true;
			// 
			// LogBox
			// 
			this.LogBox.BackColor = System.Drawing.Color.WhiteSmoke;
			this.LogBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LogBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LogBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LogBox.Location = new System.Drawing.Point(3, 3);
			this.LogBox.Multiline = true;
			this.LogBox.Name = "LogBox";
			this.LogBox.ReadOnly = true;
			this.LogBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.LogBox.Size = new System.Drawing.Size(931, 341);
			this.LogBox.TabIndex = 0;
			// 
			// OutputTab
			// 
			this.OutputTab.Location = new System.Drawing.Point(4, 29);
			this.OutputTab.Name = "OutputTab";
			this.OutputTab.Padding = new System.Windows.Forms.Padding(3);
			this.OutputTab.Size = new System.Drawing.Size(937, 347);
			this.OutputTab.TabIndex = 1;
			this.OutputTab.Text = "Output";
			this.OutputTab.UseVisualStyleBackColor = true;
			// 
			// InputTab
			// 
			this.InputTab.Controls.Add(this.HistoryButton);
			this.InputTab.Controls.Add(this.DirectoryList);
			this.InputTab.Controls.Add(this.RemoveAll);
			this.InputTab.Controls.Add(this.BrowseButton);
			this.InputTab.Controls.Add(this.FilterBox);
			this.InputTab.Location = new System.Drawing.Point(4, 29);
			this.InputTab.Name = "InputTab";
			this.InputTab.Padding = new System.Windows.Forms.Padding(3);
			this.InputTab.Size = new System.Drawing.Size(937, 347);
			this.InputTab.TabIndex = 0;
			this.InputTab.Text = "Input";
			this.InputTab.UseVisualStyleBackColor = true;
			// 
			// FilterBox
			// 
			this.FilterBox.Controls.Add(this.label1);
			this.FilterBox.Controls.Add(this.FilenameFilter);
			this.FilterBox.Controls.Add(this.Recursive);
			this.FilterBox.Controls.Add(this.IncludeHidden);
			this.FilterBox.Dock = System.Windows.Forms.DockStyle.Right;
			this.FilterBox.Location = new System.Drawing.Point(687, 3);
			this.FilterBox.Name = "FilterBox";
			this.FilterBox.Size = new System.Drawing.Size(247, 341);
			this.FilterBox.TabIndex = 0;
			this.FilterBox.TabStop = false;
			this.FilterBox.Text = "Options";
			// 
			// IncludeHidden
			// 
			this.IncludeHidden.AutoSize = true;
			this.IncludeHidden.Location = new System.Drawing.Point(7, 21);
			this.IncludeHidden.Name = "IncludeHidden";
			this.IncludeHidden.Size = new System.Drawing.Size(197, 19);
			this.IncludeHidden.TabIndex = 0;
			this.IncludeHidden.Text = "Include hidden and system files";
			this.IncludeHidden.UseVisualStyleBackColor = true;
			// 
			// Recursive
			// 
			this.Recursive.AutoSize = true;
			this.Recursive.Location = new System.Drawing.Point(7, 47);
			this.Recursive.Name = "Recursive";
			this.Recursive.Size = new System.Drawing.Size(226, 19);
			this.Recursive.TabIndex = 1;
			this.Recursive.Text = "Recursive mode (include subfolders)";
			this.Recursive.UseVisualStyleBackColor = true;
			// 
			// FilenameFilter
			// 
			this.FilenameFilter.DisplayMember = "0";
			this.FilenameFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FilenameFilter.Items.AddRange(new object[] {
            "None",
            "Wildcard",
            "Regular expression"});
			this.FilenameFilter.Location = new System.Drawing.Point(7, 106);
			this.FilenameFilter.Name = "FilenameFilter";
			this.FilenameFilter.Size = new System.Drawing.Size(226, 23);
			this.FilenameFilter.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 79);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 15);
			this.label1.TabIndex = 3;
			this.label1.Text = "Filename filter:";
			// 
			// BrowseButton
			// 
			this.BrowseButton.Location = new System.Drawing.Point(9, 6);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(67, 23);
			this.BrowseButton.TabIndex = 2;
			this.BrowseButton.Text = "Browse...";
			this.BrowseButton.UseVisualStyleBackColor = true;
			// 
			// RemoveAll
			// 
			this.RemoveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RemoveAll.AutoSize = true;
			this.RemoveAll.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RemoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RemoveAll.ForeColor = System.Drawing.Color.Navy;
			this.RemoveAll.Location = new System.Drawing.Point(593, 10);
			this.RemoveAll.Name = "RemoveAll";
			this.RemoveAll.Size = new System.Drawing.Size(69, 15);
			this.RemoveAll.TabIndex = 4;
			this.RemoveAll.Text = "Remove all";
			this.RemoveAll.Click += new System.EventHandler(this.RemoveAll_Click);
			// 
			// DirectoryList
			// 
			this.DirectoryList.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.DirectoryList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DirectoryList.BackColor = System.Drawing.Color.LightGray;
			this.DirectoryList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DirectoryList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.DirectoryList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.DirectoryList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
			this.DirectoryList.LabelWrap = false;
			this.DirectoryList.Location = new System.Drawing.Point(9, 37);
			this.DirectoryList.Name = "DirectoryList";
			this.DirectoryList.Size = new System.Drawing.Size(672, 297);
			this.DirectoryList.TabIndex = 5;
			this.DirectoryList.UseCompatibleStateImageBehavior = false;
			this.DirectoryList.View = System.Windows.Forms.View.Details;
			this.DirectoryList.DoubleClick += new System.EventHandler(this.DirectoryList_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Width = 24464;
			// 
			// HistoryButton
			// 
			this.HistoryButton.Location = new System.Drawing.Point(82, 6);
			this.HistoryButton.Menu = this.HistoryMenu;
			this.HistoryButton.Name = "HistoryButton";
			this.HistoryButton.Size = new System.Drawing.Size(75, 23);
			this.HistoryButton.SplitWidth = 30;
			this.HistoryButton.TabIndex = 6;
			this.HistoryButton.Text = "History";
			this.HistoryButton.UseVisualStyleBackColor = true;
			// 
			// MainTabs
			// 
			this.MainTabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.MainTabs.Controls.Add(this.InputTab);
			this.MainTabs.Controls.Add(this.OutputTab);
			this.MainTabs.Controls.Add(this.LogTab);
			this.MainTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainTabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainTabs.HotTrack = true;
			this.MainTabs.Location = new System.Drawing.Point(0, 0);
			this.MainTabs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
			this.MainTabs.Name = "MainTabs";
			this.MainTabs.Padding = new System.Drawing.Point(6, 4);
			this.MainTabs.SelectedIndex = 0;
			this.MainTabs.Size = new System.Drawing.Size(945, 380);
			this.MainTabs.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(945, 461);
			this.Controls.Add(this.MainTabs);
			this.Controls.Add(this.BottomPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "DirLister v2";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.BottomPanel.ResumeLayout(false);
			this.BottomPanel.PerformLayout();
			this.HistoryMenu.ResumeLayout(false);
			this.LogTab.ResumeLayout(false);
			this.LogTab.PerformLayout();
			this.InputTab.ResumeLayout(false);
			this.InputTab.PerformLayout();
			this.FilterBox.ResumeLayout(false);
			this.FilterBox.PerformLayout();
			this.MainTabs.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel BottomPanel;
		private System.Windows.Forms.Button StartButton;
		private System.Windows.Forms.ProgressBar Progress;
		private System.Windows.Forms.Label ProgressLabel;
		private System.Windows.Forms.Label FirstRunLabel;
		private System.Windows.Forms.ContextMenuStrip HistoryMenu;
		private System.Windows.Forms.ToolStripMenuItem HistoryClearMenuItem;
		private System.Windows.Forms.ToolStripSeparator HistorySeparator;
		private System.Windows.Forms.TabPage LogTab;
		private System.Windows.Forms.TextBox LogBox;
		private System.Windows.Forms.TabPage OutputTab;
		private System.Windows.Forms.TabPage InputTab;
		private SplitButton HistoryButton;
		private System.Windows.Forms.ListView DirectoryList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Label RemoveAll;
		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.GroupBox FilterBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox FilenameFilter;
		private System.Windows.Forms.CheckBox Recursive;
		private System.Windows.Forms.CheckBox IncludeHidden;
		private System.Windows.Forms.TabControl MainTabs;
	}
}

