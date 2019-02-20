using System.Windows.Forms;

namespace Sander.DirLister.UI.App
{
	public partial class ProgressForm : Form
	{
		public ProgressForm()
		{
			InitializeComponent();
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
	}
}
