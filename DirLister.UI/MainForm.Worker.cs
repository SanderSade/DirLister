using System;
using System.Diagnostics;

namespace Sander.DirLister.UI
{
	public partial class MainForm
	{
		internal DoProgress ProgressDelegate { get; set; }

		internal delegate void DoProgress(int progress, string message);

		internal DoLog LogDelegate { get; set; }

		internal delegate void DoLog(TraceLevel level, string message);

		internal void SetProgress(int progress, string message)
		{
			Progress.Value = progress;
			ProgressLabel.Text = message;
			//if (progress >= 100)
			//	Close();
		}

		internal void SetLog(TraceLevel level, string message)
		{
			LogBox.AppendText($"[{DateTimeOffset.Now.ToLocalTime():G}] {level}: {message}{Environment.NewLine}");
		}

		internal void ConfigureCallbacks()
		{
			ProgressDelegate = SetProgress;
			LogDelegate = SetLog;

			_configuration.LoggingAction = (level, s) => LogBox.Invoke(LogDelegate);
			_configuration.ProgressAction = (level, s) => LogBox.Invoke(ProgressDelegate);
		}
	}
}
