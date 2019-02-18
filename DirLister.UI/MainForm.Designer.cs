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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("c:\\");
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("C:\\Users\\Sander\\Source\\Repos\\DirLister\\DirLister.UI");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.BottomPanel = new System.Windows.Forms.Panel();
			this.StartButton = new System.Windows.Forms.Button();
			this.Progress = new System.Windows.Forms.ProgressBar();
			this.ProgressLabel = new System.Windows.Forms.Label();
			this.FirstRunLabel = new System.Windows.Forms.Label();
			this.MainTabs = new System.Windows.Forms.TabControl();
			this.InputTab = new System.Windows.Forms.TabPage();
			this.OutputTab = new System.Windows.Forms.TabPage();
			this.LogTab = new System.Windows.Forms.TabPage();
			this.AboutTab = new System.Windows.Forms.TabPage();
			this.FilterBox = new System.Windows.Forms.GroupBox();
			this.IncludeHidden = new System.Windows.Forms.CheckBox();
			this.Recursive = new System.Windows.Forms.CheckBox();
			this.FilenameFilter = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.FolderHistory = new System.Windows.Forms.ComboBox();
			this.AddButton = new System.Windows.Forms.Button();
			this.SelectLink = new System.Windows.Forms.Label();
			this.RemoveAll = new System.Windows.Forms.Label();
			this.DirectoryList = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BottomPanel.SuspendLayout();
			this.MainTabs.SuspendLayout();
			this.InputTab.SuspendLayout();
			this.FilterBox.SuspendLayout();
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
			// Progress
			// 
			this.Progress.ForeColor = System.Drawing.Color.DarkRed;
			this.Progress.Location = new System.Drawing.Point(157, 45);
			this.Progress.Name = "Progress";
			this.Progress.Size = new System.Drawing.Size(244, 23);
			this.Progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.Progress.TabIndex = 1;
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
			// MainTabs
			// 
			this.MainTabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.MainTabs.Controls.Add(this.InputTab);
			this.MainTabs.Controls.Add(this.OutputTab);
			this.MainTabs.Controls.Add(this.LogTab);
			this.MainTabs.Controls.Add(this.AboutTab);
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
			// InputTab
			// 
			this.InputTab.Controls.Add(this.DirectoryList);
			this.InputTab.Controls.Add(this.RemoveAll);
			this.InputTab.Controls.Add(this.SelectLink);
			this.InputTab.Controls.Add(this.AddButton);
			this.InputTab.Controls.Add(this.FolderHistory);
			this.InputTab.Controls.Add(this.FilterBox);
			this.InputTab.Location = new System.Drawing.Point(4, 29);
			this.InputTab.Name = "InputTab";
			this.InputTab.Padding = new System.Windows.Forms.Padding(3);
			this.InputTab.Size = new System.Drawing.Size(937, 347);
			this.InputTab.TabIndex = 0;
			this.InputTab.Text = "Input";
			this.InputTab.UseVisualStyleBackColor = true;
			// 
			// OutputTab
			// 
			this.OutputTab.Location = new System.Drawing.Point(4, 29);
			this.OutputTab.Name = "OutputTab";
			this.OutputTab.Padding = new System.Windows.Forms.Padding(3);
			this.OutputTab.Size = new System.Drawing.Size(792, 336);
			this.OutputTab.TabIndex = 1;
			this.OutputTab.Text = "Output";
			this.OutputTab.UseVisualStyleBackColor = true;
			// 
			// LogTab
			// 
			this.LogTab.Location = new System.Drawing.Point(4, 29);
			this.LogTab.Name = "LogTab";
			this.LogTab.Padding = new System.Windows.Forms.Padding(3);
			this.LogTab.Size = new System.Drawing.Size(792, 336);
			this.LogTab.TabIndex = 2;
			this.LogTab.Text = "Log";
			this.LogTab.UseVisualStyleBackColor = true;
			// 
			// AboutTab
			// 
			this.AboutTab.Location = new System.Drawing.Point(4, 29);
			this.AboutTab.Name = "AboutTab";
			this.AboutTab.Size = new System.Drawing.Size(792, 336);
			this.AboutTab.TabIndex = 3;
			this.AboutTab.Text = "About";
			this.AboutTab.UseVisualStyleBackColor = true;
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
			// FolderHistory
			// 
			this.FolderHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FolderHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FolderHistory.FormattingEnabled = true;
			this.FolderHistory.Location = new System.Drawing.Point(9, 7);
			this.FolderHistory.Name = "FolderHistory";
			this.FolderHistory.Size = new System.Drawing.Size(466, 23);
			this.FolderHistory.TabIndex = 1;
			// 
			// AddButton
			// 
			this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.AddButton.Location = new System.Drawing.Point(481, 6);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(59, 23);
			this.AddButton.TabIndex = 2;
			this.AddButton.Text = "Add";
			this.AddButton.UseVisualStyleBackColor = true;
			// 
			// SelectLink
			// 
			this.SelectLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SelectLink.AutoSize = true;
			this.SelectLink.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SelectLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SelectLink.ForeColor = System.Drawing.Color.Navy;
			this.SelectLink.Location = new System.Drawing.Point(546, 10);
			this.SelectLink.Name = "SelectLink";
			this.SelectLink.Size = new System.Drawing.Size(41, 15);
			this.SelectLink.TabIndex = 3;
			this.SelectLink.Text = "Select";
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
			this.columnHeader1.Width = 90000;
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
			this.MainTabs.ResumeLayout(false);
			this.InputTab.ResumeLayout(false);
			this.InputTab.PerformLayout();
			this.FilterBox.ResumeLayout(false);
			this.FilterBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel BottomPanel;
		private System.Windows.Forms.Button StartButton;
		private System.Windows.Forms.ProgressBar Progress;
		private System.Windows.Forms.Label ProgressLabel;
		private System.Windows.Forms.Label FirstRunLabel;
		private System.Windows.Forms.TabControl MainTabs;
		private System.Windows.Forms.TabPage InputTab;
		private System.Windows.Forms.TabPage OutputTab;
		private System.Windows.Forms.TabPage LogTab;
		private System.Windows.Forms.TabPage AboutTab;
		private System.Windows.Forms.GroupBox FilterBox;
		private System.Windows.Forms.CheckBox Recursive;
		private System.Windows.Forms.CheckBox IncludeHidden;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox FilenameFilter;
		private System.Windows.Forms.Label SelectLink;
		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.ComboBox FolderHistory;
		private System.Windows.Forms.ListView DirectoryList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Label RemoveAll;
	}
}

