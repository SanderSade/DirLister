using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sander.DirLister.UI.App
{
	public partial class ProgressForm : Form
	{
		public ProgressForm()
		{
			InitializeComponent();
			ProgressDelegate = SetProgress;
			//SetState(ProgressBar, 3);
		}


		internal DoProgress ProgressDelegate { get; }


		//[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
		//public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);


		//public static void SetState(ProgressBar pBar, int state)
		//{
		//	SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
		//}


		internal void SetProgress(int progress, string message)
		{
			ProgressBar.Value = progress;
			ProgressLabel.Text = message;
			Application.DoEvents();
		}


		internal delegate void DoProgress(int progress, string message);
	}
}
