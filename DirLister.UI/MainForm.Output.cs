using System;
using System.Linq;
using System.Windows.Forms;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	public partial class MainForm
	{
		internal void InitializeOutput()
		{
			SetFormats();
			SizeCheck.Checked = _configuration.IncludeSize;
			FileDateCheck.Checked = _configuration.IncludeFileDates;
			MediaInfoCheck.Checked = _configuration.IncludeMediaInfo;

			OutputFolder.Text = _configuration.OutputFolder;
			OpenAfter.Checked = _configuration.OpenAfter;

			EnableShellCheck.Checked = Settings.Default.EnableShellIntegration;
			OpenUiCheck.Checked = Settings.Default.ShowUiFromShell;
			ProgressWindowCheck.Checked = Settings.Default.ShowProgressWindow;
		}

		private void SetFormats()
		{
			var formats = OutFormats.Controls.OfType<CheckBox>().ToList();
			foreach (var format in _configuration.OutputFormats)
			{
				var ch = formats.FirstOrDefault(x =>
					string.Compare((string) x.Tag, format.ToString(), StringComparison.OrdinalIgnoreCase) == 0);

				if (ch != null)
					ch.Checked = true;
			}
		}
	}
}
