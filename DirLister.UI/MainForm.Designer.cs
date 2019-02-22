using Sander.DirLister.UI.App;

namespace Sander.DirLister.UI
{
	sealed partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.BottomPanel = new System.Windows.Forms.Panel();
			this.FirstRunLabel = new System.Windows.Forms.Label();
			this.ProgressLabel = new System.Windows.Forms.Label();
			this.Progress = new System.Windows.Forms.ProgressBar();
			this.StartButton = new System.Windows.Forms.Button();
			this.SetDefault = new System.Windows.Forms.Button();
			this.LabelHomepage = new System.Windows.Forms.Label();
			this.HistoryMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.HistoryClearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HistorySeparator = new System.Windows.Forms.ToolStripSeparator();
			this.LogTab = new System.Windows.Forms.TabPage();
			this.LogBox = new System.Windows.Forms.TextBox();
			this.OutputTab = new System.Windows.Forms.TabPage();
			this.KeepOnTop = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.ProgressWindowCheck = new System.Windows.Forms.CheckBox();
			this.OpenUiCheck = new System.Windows.Forms.CheckBox();
			this.EnableShellCheck = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.IncludeMediaInfo = new System.Windows.Forms.CheckBox();
			this.IncludeFileDates = new System.Windows.Forms.CheckBox();
			this.IncludeSize = new System.Windows.Forms.CheckBox();
			this.OutFormats = new System.Windows.Forms.GroupBox();
			this.MdCheck = new System.Windows.Forms.CheckBox();
			this.JsonCheck = new System.Windows.Forms.CheckBox();
			this.XmlCheck = new System.Windows.Forms.CheckBox();
			this.CsvCheck = new System.Windows.Forms.CheckBox();
			this.TxtCheck = new System.Windows.Forms.CheckBox();
			this.HtmlCheck = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.OpenAfter = new System.Windows.Forms.CheckBox();
			this.SelectOutputFolder = new System.Windows.Forms.Button();
			this.OutputFolder = new System.Windows.Forms.TextBox();
			this.InputTab = new System.Windows.Forms.TabPage();
			this.RemoveAllButton = new System.Windows.Forms.Button();
			this.HistoryButton = new Sander.DirLister.UI.App.SplitButton();
			this.DirectoryList = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BrowseButton = new System.Windows.Forms.Button();
			this.FilterBox = new System.Windows.Forms.GroupBox();
			this.FilterTabs = new System.Windows.Forms.TabControl();
			this.NoneTab = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.WildcardTab = new System.Windows.Forms.TabPage();
			this.WildcardList = new System.Windows.Forms.ListView();
			this.WildcardColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ClearWildcardsButton = new System.Windows.Forms.Button();
			this.AddWildcardButton = new System.Windows.Forms.Button();
			this.WildcardEdit = new System.Windows.Forms.ComboBox();
			this.RegexTab = new System.Windows.Forms.TabPage();
			this.FilterLabel = new System.Windows.Forms.Label();
			this.IncludeSubfolders = new System.Windows.Forms.CheckBox();
			this.IncludeHidden = new System.Windows.Forms.CheckBox();
			this.MainTabs = new System.Windows.Forms.TabControl();
			this.AboutTab = new System.Windows.Forms.TabPage();
			this.FolderSelectionDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.DirectoryMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.OpenFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.RemoveFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MoveUp = new System.Windows.Forms.ToolStripMenuItem();
			this.MoveDown = new System.Windows.Forms.ToolStripMenuItem();
			this.BottomPanel.SuspendLayout();
			this.HistoryMenu.SuspendLayout();
			this.LogTab.SuspendLayout();
			this.OutputTab.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.OutFormats.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.InputTab.SuspendLayout();
			this.FilterBox.SuspendLayout();
			this.FilterTabs.SuspendLayout();
			this.NoneTab.SuspendLayout();
			this.WildcardTab.SuspendLayout();
			this.MainTabs.SuspendLayout();
			this.AboutTab.SuspendLayout();
			this.DirectoryMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// BottomPanel
			// 
			this.BottomPanel.Controls.Add(this.FirstRunLabel);
			this.BottomPanel.Controls.Add(this.ProgressLabel);
			this.BottomPanel.Controls.Add(this.Progress);
			this.BottomPanel.Controls.Add(this.StartButton);
			this.BottomPanel.Controls.Add(this.SetDefault);
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
			this.FirstRunLabel.Size = new System.Drawing.Size(456, 16);
			this.FirstRunLabel.TabIndex = 3;
			this.FirstRunLabel.Text = "First run - choose the default output options and click \"Set as default options\"";
			// 
			// ProgressLabel
			// 
			this.ProgressLabel.AutoSize = true;
			this.ProgressLabel.Location = new System.Drawing.Point(407, 50);
			this.ProgressLabel.Name = "ProgressLabel";
			this.ProgressLabel.Size = new System.Drawing.Size(0, 13);
			this.ProgressLabel.TabIndex = 2;
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
			this.StartButton.Text = "&Start";
			this.StartButton.UseVisualStyleBackColor = false;
			this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
			// 
			// SetDefault
			// 
			this.SetDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SetDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SetDefault.Location = new System.Drawing.Point(814, 6);
			this.SetDefault.Name = "SetDefault";
			this.SetDefault.Size = new System.Drawing.Size(121, 63);
			this.SetDefault.TabIndex = 0;
			this.SetDefault.Text = "Set as &default options";
			this.SetDefault.UseVisualStyleBackColor = true;
			this.SetDefault.Click += new System.EventHandler(this.SetDefault_Click);
			// 
			// LabelHomepage
			// 
			this.LabelHomepage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LabelHomepage.AutoSize = true;
			this.LabelHomepage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LabelHomepage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LabelHomepage.ForeColor = System.Drawing.Color.Navy;
			this.LabelHomepage.Location = new System.Drawing.Point(349, 177);
			this.LabelHomepage.Name = "LabelHomepage";
			this.LabelHomepage.Size = new System.Drawing.Size(224, 15);
			this.LabelHomepage.TabIndex = 5;
			this.LabelHomepage.Text = "https://github.com/SanderSade/DirLister";
			this.LabelHomepage.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.LabelHomepage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LabelHomepage_MouseClick);
			// 
			// HistoryMenu
			// 
			this.HistoryMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HistoryClearMenuItem,
            this.HistorySeparator});
			this.HistoryMenu.Name = "HistoryMenu";
			this.HistoryMenu.ShowImageMargin = false;
			this.HistoryMenu.Size = new System.Drawing.Size(77, 32);
			// 
			// HistoryClearMenuItem
			// 
			this.HistoryClearMenuItem.Name = "HistoryClearMenuItem";
			this.HistoryClearMenuItem.Size = new System.Drawing.Size(76, 22);
			this.HistoryClearMenuItem.Text = "Clear";
			this.HistoryClearMenuItem.Click += new System.EventHandler(this.HistoryClearMenuItem_Click);
			// 
			// HistorySeparator
			// 
			this.HistorySeparator.Name = "HistorySeparator";
			this.HistorySeparator.Size = new System.Drawing.Size(73, 6);
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
			this.LogBox.TextChanged += new System.EventHandler(this.LogBox_TextChanged);
			// 
			// OutputTab
			// 
			this.OutputTab.Controls.Add(this.KeepOnTop);
			this.OutputTab.Controls.Add(this.groupBox3);
			this.OutputTab.Controls.Add(this.groupBox2);
			this.OutputTab.Controls.Add(this.OutFormats);
			this.OutputTab.Controls.Add(this.groupBox1);
			this.OutputTab.Location = new System.Drawing.Point(4, 29);
			this.OutputTab.Name = "OutputTab";
			this.OutputTab.Padding = new System.Windows.Forms.Padding(3);
			this.OutputTab.Size = new System.Drawing.Size(937, 347);
			this.OutputTab.TabIndex = 1;
			this.OutputTab.Text = "Output";
			this.OutputTab.UseVisualStyleBackColor = true;
			// 
			// KeepOnTop
			// 
			this.KeepOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.KeepOnTop.AutoSize = true;
			this.KeepOnTop.Location = new System.Drawing.Point(436, 283);
			this.KeepOnTop.Name = "KeepOnTop";
			this.KeepOnTop.Size = new System.Drawing.Size(256, 19);
			this.KeepOnTop.TabIndex = 7;
			this.KeepOnTop.Text = "Keep DirLister on top of the other windows";
			this.KeepOnTop.UseVisualStyleBackColor = true;
			this.KeepOnTop.CheckedChanged += new System.EventHandler(this.KeepOnTop_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.ProgressWindowCheck);
			this.groupBox3.Controls.Add(this.OpenUiCheck);
			this.groupBox3.Controls.Add(this.EnableShellCheck);
			this.groupBox3.Location = new System.Drawing.Point(419, 117);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(510, 118);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Windows Explorer integration";
			// 
			// ProgressWindowCheck
			// 
			this.ProgressWindowCheck.AutoSize = true;
			this.ProgressWindowCheck.Location = new System.Drawing.Point(37, 79);
			this.ProgressWindowCheck.Name = "ProgressWindowCheck";
			this.ProgressWindowCheck.Size = new System.Drawing.Size(370, 19);
			this.ProgressWindowCheck.TabIndex = 8;
			this.ProgressWindowCheck.Text = "Show progress window during list making started from Explorer";
			this.ProgressWindowCheck.UseVisualStyleBackColor = true;
			this.ProgressWindowCheck.CheckedChanged += new System.EventHandler(this.ProgressWindowCheck_CheckedChanged);
			// 
			// OpenUiCheck
			// 
			this.OpenUiCheck.AutoSize = true;
			this.OpenUiCheck.Location = new System.Drawing.Point(37, 50);
			this.OpenUiCheck.Name = "OpenUiCheck";
			this.OpenUiCheck.Size = new System.Drawing.Size(359, 19);
			this.OpenUiCheck.TabIndex = 7;
			this.OpenUiCheck.Text = "Open UI instead of silently creating file list with default options";
			this.OpenUiCheck.UseVisualStyleBackColor = true;
			this.OpenUiCheck.CheckedChanged += new System.EventHandler(this.OpenUiCheck_CheckedChanged);
			// 
			// EnableShellCheck
			// 
			this.EnableShellCheck.AutoSize = true;
			this.EnableShellCheck.Location = new System.Drawing.Point(17, 21);
			this.EnableShellCheck.Name = "EnableShellCheck";
			this.EnableShellCheck.Size = new System.Drawing.Size(449, 19);
			this.EnableShellCheck.TabIndex = 6;
			this.EnableShellCheck.Text = "Enable shell integration (right-click menu for directory and drive, Send To entry" +
    "";
			this.EnableShellCheck.UseVisualStyleBackColor = true;
			this.EnableShellCheck.CheckedChanged += new System.EventHandler(this.EnableShellCheck_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.IncludeMediaInfo);
			this.groupBox2.Controls.Add(this.IncludeFileDates);
			this.groupBox2.Controls.Add(this.IncludeSize);
			this.groupBox2.Location = new System.Drawing.Point(203, 117);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 203);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Additional information";
			// 
			// IncludeMediaInfo
			// 
			this.IncludeMediaInfo.AutoSize = true;
			this.IncludeMediaInfo.Location = new System.Drawing.Point(7, 79);
			this.IncludeMediaInfo.Name = "IncludeMediaInfo";
			this.IncludeMediaInfo.Size = new System.Drawing.Size(84, 19);
			this.IncludeMediaInfo.TabIndex = 2;
			this.IncludeMediaInfo.Text = "Media info";
			this.IncludeMediaInfo.UseVisualStyleBackColor = true;
			// 
			// IncludeFileDates
			// 
			this.IncludeFileDates.AutoSize = true;
			this.IncludeFileDates.Location = new System.Drawing.Point(7, 50);
			this.IncludeFileDates.Name = "IncludeFileDates";
			this.IncludeFileDates.Size = new System.Drawing.Size(153, 19);
			this.IncludeFileDates.TabIndex = 1;
			this.IncludeFileDates.Text = "Created/modified dates";
			this.IncludeFileDates.UseVisualStyleBackColor = true;
			// 
			// IncludeSize
			// 
			this.IncludeSize.AutoSize = true;
			this.IncludeSize.Location = new System.Drawing.Point(7, 21);
			this.IncludeSize.Name = "IncludeSize";
			this.IncludeSize.Size = new System.Drawing.Size(71, 19);
			this.IncludeSize.TabIndex = 0;
			this.IncludeSize.Text = "File size";
			this.IncludeSize.UseVisualStyleBackColor = true;
			// 
			// OutFormats
			// 
			this.OutFormats.Controls.Add(this.MdCheck);
			this.OutFormats.Controls.Add(this.JsonCheck);
			this.OutFormats.Controls.Add(this.XmlCheck);
			this.OutFormats.Controls.Add(this.CsvCheck);
			this.OutFormats.Controls.Add(this.TxtCheck);
			this.OutFormats.Controls.Add(this.HtmlCheck);
			this.OutFormats.Location = new System.Drawing.Point(6, 117);
			this.OutFormats.Name = "OutFormats";
			this.OutFormats.Size = new System.Drawing.Size(187, 203);
			this.OutFormats.TabIndex = 2;
			this.OutFormats.TabStop = false;
			this.OutFormats.Text = "Output formats";
			// 
			// MdCheck
			// 
			this.MdCheck.AutoSize = true;
			this.MdCheck.Location = new System.Drawing.Point(6, 166);
			this.MdCheck.Name = "MdCheck";
			this.MdCheck.Size = new System.Drawing.Size(113, 19);
			this.MdCheck.TabIndex = 5;
			this.MdCheck.Tag = "md";
			this.MdCheck.Text = "Markdown (md)";
			this.MdCheck.UseVisualStyleBackColor = true;
			// 
			// JsonCheck
			// 
			this.JsonCheck.AutoSize = true;
			this.JsonCheck.Location = new System.Drawing.Point(6, 137);
			this.JsonCheck.Name = "JsonCheck";
			this.JsonCheck.Size = new System.Drawing.Size(58, 19);
			this.JsonCheck.TabIndex = 4;
			this.JsonCheck.Tag = "json";
			this.JsonCheck.Text = "JSON";
			this.JsonCheck.UseVisualStyleBackColor = true;
			// 
			// XmlCheck
			// 
			this.XmlCheck.AutoSize = true;
			this.XmlCheck.Location = new System.Drawing.Point(6, 108);
			this.XmlCheck.Name = "XmlCheck";
			this.XmlCheck.Size = new System.Drawing.Size(52, 19);
			this.XmlCheck.TabIndex = 3;
			this.XmlCheck.Tag = "xml";
			this.XmlCheck.Text = "XML";
			this.XmlCheck.UseVisualStyleBackColor = true;
			// 
			// CsvCheck
			// 
			this.CsvCheck.AutoSize = true;
			this.CsvCheck.Location = new System.Drawing.Point(6, 79);
			this.CsvCheck.Name = "CsvCheck";
			this.CsvCheck.Size = new System.Drawing.Size(49, 19);
			this.CsvCheck.TabIndex = 2;
			this.CsvCheck.Tag = "csv";
			this.CsvCheck.Text = "CSV";
			this.CsvCheck.UseVisualStyleBackColor = true;
			// 
			// TxtCheck
			// 
			this.TxtCheck.AutoSize = true;
			this.TxtCheck.Location = new System.Drawing.Point(6, 50);
			this.TxtCheck.Name = "TxtCheck";
			this.TxtCheck.Size = new System.Drawing.Size(99, 19);
			this.TxtCheck.TabIndex = 1;
			this.TxtCheck.Tag = "txt";
			this.TxtCheck.Text = "Plain text (txt)";
			this.TxtCheck.UseVisualStyleBackColor = true;
			// 
			// HtmlCheck
			// 
			this.HtmlCheck.AutoSize = true;
			this.HtmlCheck.Location = new System.Drawing.Point(6, 21);
			this.HtmlCheck.Name = "HtmlCheck";
			this.HtmlCheck.Size = new System.Drawing.Size(60, 19);
			this.HtmlCheck.TabIndex = 0;
			this.HtmlCheck.Tag = "html";
			this.HtmlCheck.Text = "HTML";
			this.HtmlCheck.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.OpenAfter);
			this.groupBox1.Controls.Add(this.SelectOutputFolder);
			this.groupBox1.Controls.Add(this.OutputFolder);
			this.groupBox1.Location = new System.Drawing.Point(12, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(795, 79);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Output folder";
			// 
			// OpenAfter
			// 
			this.OpenAfter.AutoSize = true;
			this.OpenAfter.Location = new System.Drawing.Point(6, 60);
			this.OpenAfter.Name = "OpenAfter";
			this.OpenAfter.Size = new System.Drawing.Size(253, 19);
			this.OpenAfter.TabIndex = 2;
			this.OpenAfter.Text = "Open output file/folder after list generation";
			this.OpenAfter.UseVisualStyleBackColor = true;
			// 
			// SelectOutputFolder
			// 
			this.SelectOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SelectOutputFolder.Location = new System.Drawing.Point(713, 21);
			this.SelectOutputFolder.Name = "SelectOutputFolder";
			this.SelectOutputFolder.Size = new System.Drawing.Size(75, 23);
			this.SelectOutputFolder.TabIndex = 1;
			this.SelectOutputFolder.Text = "Select";
			this.SelectOutputFolder.UseVisualStyleBackColor = true;
			this.SelectOutputFolder.Click += new System.EventHandler(this.SelectOutputFolder_Click);
			// 
			// OutputFolder
			// 
			this.OutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OutputFolder.Location = new System.Drawing.Point(6, 22);
			this.OutputFolder.Name = "OutputFolder";
			this.OutputFolder.Size = new System.Drawing.Size(701, 21);
			this.OutputFolder.TabIndex = 0;
			// 
			// InputTab
			// 
			this.InputTab.Controls.Add(this.RemoveAllButton);
			this.InputTab.Controls.Add(this.HistoryButton);
			this.InputTab.Controls.Add(this.DirectoryList);
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
			// RemoveAllButton
			// 
			this.RemoveAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RemoveAllButton.Location = new System.Drawing.Point(559, 6);
			this.RemoveAllButton.Name = "RemoveAllButton";
			this.RemoveAllButton.Size = new System.Drawing.Size(93, 23);
			this.RemoveAllButton.TabIndex = 7;
			this.RemoveAllButton.Text = "Remove &all";
			this.RemoveAllButton.UseVisualStyleBackColor = true;
			this.RemoveAllButton.Click += new System.EventHandler(this.RemoveAll_Click);
			// 
			// HistoryButton
			// 
			this.HistoryButton.Location = new System.Drawing.Point(118, 6);
			this.HistoryButton.Menu = this.HistoryMenu;
			this.HistoryButton.Name = "HistoryButton";
			this.HistoryButton.Size = new System.Drawing.Size(75, 23);
			this.HistoryButton.SplitWidth = 30;
			this.HistoryButton.TabIndex = 6;
			this.HistoryButton.Text = "&History";
			this.HistoryButton.UseVisualStyleBackColor = true;
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
			this.DirectoryList.LabelWrap = false;
			this.DirectoryList.Location = new System.Drawing.Point(9, 37);
			this.DirectoryList.Name = "DirectoryList";
			this.DirectoryList.Size = new System.Drawing.Size(643, 297);
			this.DirectoryList.TabIndex = 5;
			this.DirectoryList.UseCompatibleStateImageBehavior = false;
			this.DirectoryList.View = System.Windows.Forms.View.Details;
			this.DirectoryList.DoubleClick += new System.EventHandler(this.DirectoryList_DoubleClick);
			this.DirectoryList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DirectoryList_MouseClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Width = 24464;
			// 
			// BrowseButton
			// 
			this.BrowseButton.Location = new System.Drawing.Point(9, 6);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(103, 23);
			this.BrowseButton.TabIndex = 2;
			this.BrowseButton.Text = "Select &folder";
			this.BrowseButton.UseVisualStyleBackColor = true;
			this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// FilterBox
			// 
			this.FilterBox.Controls.Add(this.FilterTabs);
			this.FilterBox.Controls.Add(this.FilterLabel);
			this.FilterBox.Controls.Add(this.IncludeSubfolders);
			this.FilterBox.Controls.Add(this.IncludeHidden);
			this.FilterBox.Dock = System.Windows.Forms.DockStyle.Right;
			this.FilterBox.Location = new System.Drawing.Point(658, 3);
			this.FilterBox.Name = "FilterBox";
			this.FilterBox.Size = new System.Drawing.Size(276, 341);
			this.FilterBox.TabIndex = 0;
			this.FilterBox.TabStop = false;
			this.FilterBox.Text = "Options";
			// 
			// FilterTabs
			// 
			this.FilterTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FilterTabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.FilterTabs.Controls.Add(this.NoneTab);
			this.FilterTabs.Controls.Add(this.WildcardTab);
			this.FilterTabs.Controls.Add(this.RegexTab);
			this.FilterTabs.HotTrack = true;
			this.FilterTabs.ItemSize = new System.Drawing.Size(67, 23);
			this.FilterTabs.Location = new System.Drawing.Point(10, 97);
			this.FilterTabs.Name = "FilterTabs";
			this.FilterTabs.SelectedIndex = 0;
			this.FilterTabs.Size = new System.Drawing.Size(260, 238);
			this.FilterTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.FilterTabs.TabIndex = 4;
			// 
			// NoneTab
			// 
			this.NoneTab.BackColor = System.Drawing.Color.LightGray;
			this.NoneTab.Controls.Add(this.label1);
			this.NoneTab.Location = new System.Drawing.Point(4, 27);
			this.NoneTab.Name = "NoneTab";
			this.NoneTab.Padding = new System.Windows.Forms.Padding(3);
			this.NoneTab.Size = new System.Drawing.Size(252, 207);
			this.NoneTab.TabIndex = 0;
			this.NoneTab.Text = "None";
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(246, 201);
			this.label1.TabIndex = 0;
			this.label1.Text = "Filters are disabled for listings run from the shell integration (Explorer right-" +
    "click menu).";
			// 
			// WildcardTab
			// 
			this.WildcardTab.Controls.Add(this.WildcardList);
			this.WildcardTab.Controls.Add(this.ClearWildcardsButton);
			this.WildcardTab.Controls.Add(this.AddWildcardButton);
			this.WildcardTab.Controls.Add(this.WildcardEdit);
			this.WildcardTab.Location = new System.Drawing.Point(4, 27);
			this.WildcardTab.Name = "WildcardTab";
			this.WildcardTab.Padding = new System.Windows.Forms.Padding(3);
			this.WildcardTab.Size = new System.Drawing.Size(252, 207);
			this.WildcardTab.TabIndex = 1;
			this.WildcardTab.Text = "Wildcard";
			this.WildcardTab.UseVisualStyleBackColor = true;
			// 
			// WildcardList
			// 
			this.WildcardList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.WildcardList.BackColor = System.Drawing.Color.LightGray;
			this.WildcardList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.WildcardList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.WildcardColumn});
			this.WildcardList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.WildcardList.LabelWrap = false;
			this.WildcardList.Location = new System.Drawing.Point(7, 37);
			this.WildcardList.Name = "WildcardList";
			this.WildcardList.Size = new System.Drawing.Size(166, 164);
			this.WildcardList.TabIndex = 3;
			this.WildcardList.UseCompatibleStateImageBehavior = false;
			this.WildcardList.View = System.Windows.Forms.View.Details;
			// 
			// WildcardColumn
			// 
			this.WildcardColumn.Width = 9000;
			// 
			// ClearWildcardsButton
			// 
			this.ClearWildcardsButton.AutoSize = true;
			this.ClearWildcardsButton.Location = new System.Drawing.Point(179, 37);
			this.ClearWildcardsButton.Name = "ClearWildcardsButton";
			this.ClearWildcardsButton.Size = new System.Drawing.Size(67, 25);
			this.ClearWildcardsButton.TabIndex = 2;
			this.ClearWildcardsButton.Text = "Clear";
			this.ClearWildcardsButton.UseVisualStyleBackColor = true;
			this.ClearWildcardsButton.Click += new System.EventHandler(this.ClearWildcardsButton_Click);
			// 
			// AddWildcardButton
			// 
			this.AddWildcardButton.AutoSize = true;
			this.AddWildcardButton.Location = new System.Drawing.Point(179, 6);
			this.AddWildcardButton.Name = "AddWildcardButton";
			this.AddWildcardButton.Size = new System.Drawing.Size(67, 25);
			this.AddWildcardButton.TabIndex = 1;
			this.AddWildcardButton.Text = "Add";
			this.AddWildcardButton.UseVisualStyleBackColor = true;
			this.AddWildcardButton.Click += new System.EventHandler(this.AddWildcardButton_Click);
			// 
			// WildcardEdit
			// 
			this.WildcardEdit.Location = new System.Drawing.Point(7, 6);
			this.WildcardEdit.Name = "WildcardEdit";
			this.WildcardEdit.Size = new System.Drawing.Size(166, 23);
			this.WildcardEdit.TabIndex = 0;
			this.WildcardEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WildcardEdit_KeyDown);
			// 
			// RegexTab
			// 
			this.RegexTab.Location = new System.Drawing.Point(4, 27);
			this.RegexTab.Name = "RegexTab";
			this.RegexTab.Padding = new System.Windows.Forms.Padding(3);
			this.RegexTab.Size = new System.Drawing.Size(252, 207);
			this.RegexTab.TabIndex = 2;
			this.RegexTab.Text = "Regex";
			this.RegexTab.UseVisualStyleBackColor = true;
			// 
			// FilterLabel
			// 
			this.FilterLabel.AutoSize = true;
			this.FilterLabel.Location = new System.Drawing.Point(7, 69);
			this.FilterLabel.Name = "FilterLabel";
			this.FilterLabel.Size = new System.Drawing.Size(88, 15);
			this.FilterLabel.TabIndex = 3;
			this.FilterLabel.Text = "Filename filter:";
			// 
			// IncludeSubfolders
			// 
			this.IncludeSubfolders.AutoSize = true;
			this.IncludeSubfolders.Location = new System.Drawing.Point(7, 45);
			this.IncludeSubfolders.Name = "IncludeSubfolders";
			this.IncludeSubfolders.Size = new System.Drawing.Size(226, 19);
			this.IncludeSubfolders.TabIndex = 1;
			this.IncludeSubfolders.Text = "Recursive mode (include subfolders)";
			this.IncludeSubfolders.UseVisualStyleBackColor = true;
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
			// AboutTab
			// 
			this.AboutTab.Controls.Add(this.LabelHomepage);
			this.AboutTab.Location = new System.Drawing.Point(4, 29);
			this.AboutTab.Name = "AboutTab";
			this.AboutTab.Padding = new System.Windows.Forms.Padding(3);
			this.AboutTab.Size = new System.Drawing.Size(937, 347);
			this.AboutTab.TabIndex = 3;
			this.AboutTab.Text = "About";
			this.AboutTab.UseVisualStyleBackColor = true;
			// 
			// FolderSelectionDialog
			// 
			this.FolderSelectionDialog.Description = "Select folder to add...";
			this.FolderSelectionDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
			this.FolderSelectionDialog.ShowNewFolderButton = false;
			// 
			// DirectoryMenu
			// 
			this.DirectoryMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFolder,
            this.RemoveFolder,
            this.toolStripSeparator1,
            this.MoveUp,
            this.MoveDown});
			this.DirectoryMenu.Name = "DirectoryMenu";
			this.DirectoryMenu.ShowImageMargin = false;
			this.DirectoryMenu.Size = new System.Drawing.Size(113, 98);
			// 
			// OpenFolder
			// 
			this.OpenFolder.Name = "OpenFolder";
			this.OpenFolder.Size = new System.Drawing.Size(112, 22);
			this.OpenFolder.Text = "Open folder";
			this.OpenFolder.Click += new System.EventHandler(this.DirectoryList_DoubleClick);
			// 
			// RemoveFolder
			// 
			this.RemoveFolder.Name = "RemoveFolder";
			this.RemoveFolder.Size = new System.Drawing.Size(112, 22);
			this.RemoveFolder.Text = "Remove";
			this.RemoveFolder.Click += new System.EventHandler(this.RemoveFolder_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(109, 6);
			// 
			// MoveUp
			// 
			this.MoveUp.Name = "MoveUp";
			this.MoveUp.Size = new System.Drawing.Size(112, 22);
			this.MoveUp.Text = "Move up";
			this.MoveUp.Click += new System.EventHandler(this.MoveUp_Click);
			// 
			// MoveDown
			// 
			this.MoveDown.Name = "MoveDown";
			this.MoveDown.Size = new System.Drawing.Size(112, 22);
			this.MoveDown.Text = "Move down";
			this.MoveDown.Click += new System.EventHandler(this.MoveDown_Click);
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
			this.MinimumSize = new System.Drawing.Size(961, 500);
			this.Name = "MainForm";
			this.Text = "DirLister v2";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.BottomPanel.ResumeLayout(false);
			this.BottomPanel.PerformLayout();
			this.HistoryMenu.ResumeLayout(false);
			this.LogTab.ResumeLayout(false);
			this.LogTab.PerformLayout();
			this.OutputTab.ResumeLayout(false);
			this.OutputTab.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.OutFormats.ResumeLayout(false);
			this.OutFormats.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.InputTab.ResumeLayout(false);
			this.FilterBox.ResumeLayout(false);
			this.FilterBox.PerformLayout();
			this.FilterTabs.ResumeLayout(false);
			this.NoneTab.ResumeLayout(false);
			this.WildcardTab.ResumeLayout(false);
			this.WildcardTab.PerformLayout();
			this.MainTabs.ResumeLayout(false);
			this.AboutTab.ResumeLayout(false);
			this.AboutTab.PerformLayout();
			this.DirectoryMenu.ResumeLayout(false);
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
		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.GroupBox FilterBox;
		private System.Windows.Forms.Label FilterLabel;
		private System.Windows.Forms.CheckBox IncludeSubfolders;
		private System.Windows.Forms.CheckBox IncludeHidden;
		private System.Windows.Forms.TabControl MainTabs;
		private System.Windows.Forms.Label LabelHomepage;
		private System.Windows.Forms.Button RemoveAllButton;
		private System.Windows.Forms.FolderBrowserDialog FolderSelectionDialog;
		private System.Windows.Forms.ContextMenuStrip DirectoryMenu;
		private System.Windows.Forms.ToolStripMenuItem OpenFolder;
		private System.Windows.Forms.ToolStripMenuItem RemoveFolder;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem MoveUp;
		private System.Windows.Forms.ToolStripMenuItem MoveDown;
		private System.Windows.Forms.Button SetDefault;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button SelectOutputFolder;
		private System.Windows.Forms.TextBox OutputFolder;
		private System.Windows.Forms.CheckBox OpenAfter;
		private System.Windows.Forms.GroupBox OutFormats;
		private System.Windows.Forms.CheckBox MdCheck;
		private System.Windows.Forms.CheckBox JsonCheck;
		private System.Windows.Forms.CheckBox XmlCheck;
		private System.Windows.Forms.CheckBox CsvCheck;
		private System.Windows.Forms.CheckBox TxtCheck;
		private System.Windows.Forms.CheckBox HtmlCheck;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox IncludeMediaInfo;
		private System.Windows.Forms.CheckBox IncludeFileDates;
		private System.Windows.Forms.CheckBox IncludeSize;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox OpenUiCheck;
		private System.Windows.Forms.CheckBox EnableShellCheck;
		private System.Windows.Forms.CheckBox ProgressWindowCheck;
		private System.Windows.Forms.CheckBox KeepOnTop;
		private System.Windows.Forms.TabPage AboutTab;
		private System.Windows.Forms.TabControl FilterTabs;
		private System.Windows.Forms.TabPage NoneTab;
		private System.Windows.Forms.TabPage WildcardTab;
		private System.Windows.Forms.TabPage RegexTab;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox WildcardEdit;
		private System.Windows.Forms.Button AddWildcardButton;
		private System.Windows.Forms.Button ClearWildcardsButton;
		private System.Windows.Forms.ListView WildcardList;
		private System.Windows.Forms.ColumnHeader WildcardColumn;
	}
}

