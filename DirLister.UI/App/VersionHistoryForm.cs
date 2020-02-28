using System;
using System.Windows.Forms;

namespace Sander.DirLister.UI.App
{
	public partial class VersionHistoryForm : Form
	{
		public VersionHistoryForm(string newVersion, string historyHtml)
		{
			InitializeComponent();
			VersionAvailable.Text =
				$"New version available! Current version: \"{Program.Version}\", new version \"{newVersion}\"";

			HistoryView.DocumentText = historyHtml;
		}


		private void HomepageLinkLabel_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
