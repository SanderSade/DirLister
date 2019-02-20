namespace Sander.DirLister.UI.App
{
	partial class ProgressForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.ProgressLabel = new System.Windows.Forms.Label();
			this.TopLabel = new System.Windows.Forms.Label();
			this.HideLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// ProgressBar
			// 
			this.ProgressBar.BackColor = System.Drawing.SystemColors.ButtonShadow;
			this.ProgressBar.ForeColor = System.Drawing.Color.Black;
			this.ProgressBar.Location = new System.Drawing.Point(12, 61);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(387, 12);
			this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.ProgressBar.TabIndex = 0;
			// 
			// ProgressLabel
			// 
			this.ProgressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ProgressLabel.ForeColor = System.Drawing.Color.White;
			this.ProgressLabel.Location = new System.Drawing.Point(12, 30);
			this.ProgressLabel.Name = "ProgressLabel";
			this.ProgressLabel.Size = new System.Drawing.Size(387, 20);
			this.ProgressLabel.TabIndex = 1;
			this.ProgressLabel.Text = "Waiting...";
			// 
			// TopLabel
			// 
			this.TopLabel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.TopLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.TopLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TopLabel.Location = new System.Drawing.Point(0, 0);
			this.TopLabel.Name = "TopLabel";
			this.TopLabel.Size = new System.Drawing.Size(411, 16);
			this.TopLabel.TabIndex = 2;
			this.TopLabel.Text = "DirLister v2";
			this.TopLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// HideLabel
			// 
			this.HideLabel.AutoSize = true;
			this.HideLabel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.HideLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.HideLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HideLabel.ForeColor = System.Drawing.Color.Maroon;
			this.HideLabel.Location = new System.Drawing.Point(381, 2);
			this.HideLabel.Name = "HideLabel";
			this.HideLabel.Size = new System.Drawing.Size(29, 13);
			this.HideLabel.TabIndex = 3;
			this.HideLabel.Text = "Hide";
			this.HideLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.HideLabel.Click += new System.EventHandler(this.HideLabel_Click);
			// 
			// ProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.ClientSize = new System.Drawing.Size(411, 85);
			this.Controls.Add(this.HideLabel);
			this.Controls.Add(this.TopLabel);
			this.Controls.Add(this.ProgressLabel);
			this.Controls.Add(this.ProgressBar);
			this.Cursor = System.Windows.Forms.Cursors.AppStarting;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressForm";
			this.Opacity = 0.8D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "ProgressForm";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProgressForm_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.ProgressBar ProgressBar;
		private System.Windows.Forms.Label ProgressLabel;
		private System.Windows.Forms.Label TopLabel;
		private System.Windows.Forms.Label HideLabel;
	}
}