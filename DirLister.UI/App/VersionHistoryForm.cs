using System.Windows.Forms;

namespace Sander.DirLister.UI.App
{
	public partial class VersionHistoryForm : Form
	{
		public VersionHistoryForm(string newVersion, string historyHtml)
		{
			InitializeComponent();
			VersionAvailable.Text =
				$"New version available! Current version: {Program.Version}, new version {newVersion}. Click the link to get the new version";

			HistoryView.DocumentText = historyHtml;
		}
	}
}
