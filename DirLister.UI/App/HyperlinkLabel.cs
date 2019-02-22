using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Sander.DirLister.UI.App
{
	/// <summary>
	///     Label that has hyperlink
	/// </summary>
	public sealed class HyperlinkLabel : Label
	{
		public HyperlinkLabel()
		{
			Click += (sender, args) => Process.Start(Url);
			ForeColor = Color.Navy;
			Cursor = Cursors.Hand;
			Font = new Font(DefaultFont, FontStyle.Underline);
		}

		[DefaultValue(null)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string Url { get; set; }
	}
}
