namespace Sander.DirLister.UI.App
{
	partial class VersionHistoryForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionHistoryForm));
			this.TopPanel = new System.Windows.Forms.Panel();
			this.HistoryView = new System.Windows.Forms.WebBrowser();
			this.VersionAvailable = new System.Windows.Forms.Label();
			this.HomepageLinkLabel = new Sander.DirLister.UI.App.HyperlinkLabel();
			this.TopPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// TopPanel
			// 
			this.TopPanel.Controls.Add(this.HomepageLinkLabel);
			this.TopPanel.Controls.Add(this.VersionAvailable);
			this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.TopPanel.Location = new System.Drawing.Point(0, 0);
			this.TopPanel.Name = "TopPanel";
			this.TopPanel.Size = new System.Drawing.Size(800, 77);
			this.TopPanel.TabIndex = 0;
			// 
			// HistoryView
			// 
			this.HistoryView.AllowNavigation = false;
			this.HistoryView.AllowWebBrowserDrop = false;
			this.HistoryView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.HistoryView.IsWebBrowserContextMenuEnabled = false;
			this.HistoryView.Location = new System.Drawing.Point(0, 77);
			this.HistoryView.MinimumSize = new System.Drawing.Size(20, 20);
			this.HistoryView.Name = "HistoryView";
			this.HistoryView.ScriptErrorsSuppressed = true;
			this.HistoryView.Size = new System.Drawing.Size(800, 373);
			this.HistoryView.TabIndex = 1;
			this.HistoryView.WebBrowserShortcutsEnabled = false;
			// 
			// VersionAvailable
			// 
			this.VersionAvailable.Dock = System.Windows.Forms.DockStyle.Top;
			this.VersionAvailable.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.VersionAvailable.Location = new System.Drawing.Point(0, 0);
			this.VersionAvailable.Name = "VersionAvailable";
			this.VersionAvailable.Size = new System.Drawing.Size(800, 18);
			this.VersionAvailable.TabIndex = 0;
			this.VersionAvailable.Text = "New version of DirLister is available";
			// 
			// HomepageLinkLabel
			// 
			this.HomepageLinkLabel.AutoSize = true;
			this.HomepageLinkLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.HomepageLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline);
			this.HomepageLinkLabel.ForeColor = System.Drawing.Color.Navy;
			this.HomepageLinkLabel.Location = new System.Drawing.Point(12, 32);
			this.HomepageLinkLabel.Name = "HomepageLinkLabel";
			this.HomepageLinkLabel.Size = new System.Drawing.Size(292, 20);
			this.HomepageLinkLabel.TabIndex = 1;
			this.HomepageLinkLabel.Text = "https://github.com/SanderSade/DirLister";
			this.HomepageLinkLabel.Url = "https://github.com/SanderSade/DirLister";
			this.HomepageLinkLabel.Click += new System.EventHandler(this.HomepageLinkLabel_Click);
			// 
			// VersionHistoryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.HistoryView);
			this.Controls.Add(this.TopPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "VersionHistoryForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Version history";
			this.TopPanel.ResumeLayout(false);
			this.TopPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel TopPanel;
		private System.Windows.Forms.WebBrowser HistoryView;
		private System.Windows.Forms.Label VersionAvailable;
		private HyperlinkLabel HomepageLinkLabel;
	}
}