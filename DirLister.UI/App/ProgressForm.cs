using System.Windows.Forms;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI.App
{
	public partial class ProgressForm : Form
	{
		public ProgressForm()
		{
			InitializeComponent();
			TopLabel.Text = Program.VersionString;
			ProgressDelegate = SetProgress;
		}


		internal DoProgress ProgressDelegate { get; }


		internal void SetProgress(int progress, string message)
		{
			ProgressBar.Value = progress;
			ProgressLabel.Text = message;
			if (progress >= 100)
				Close();
		}


		internal delegate void DoProgress(int progress, string message);

		private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Settings.Default.Save();
		}

		private void HideLabel_Click(object sender, System.EventArgs e)
		{
			Settings.Default.ShowProgressWindow = false;
			Hide();
		}
	}
}
