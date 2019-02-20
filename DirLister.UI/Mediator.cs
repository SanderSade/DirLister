using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.UI.App;
using Sander.DirLister.UI.DTO;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	internal static class Mediator
	{
		internal static void Run(params string[] folders)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var configuration = ReadConfiguration();
			folders = new[] { @"c:\temp", @"c:\tools", @"c:\dev" };
			configuration.IncludeMediaInfo = true;
			if (/*Settings.Default.FirstRun ||*/ Settings.Default.ShowUiFromShell || folders == null || folders.Length == 0)
			{

				Application.Run(new MainForm(configuration, null, folders));
				Settings.Default.Save();
			}
			else
			{
				RunSilent(folders, configuration);
			}
		}

		private static void RunSilent(string[] folders, Configuration configuration)
		{
			configuration.InputFolders = new List<string>(folders);

			var log = new ConcurrentBag<LogEntry>();
			var hasError = false;

			configuration.LoggingAction = delegate (TraceLevel level, string message)
			{
				if (level == TraceLevel.Error)
					hasError = true;
				log.Add(new LogEntry(level, message));
			};

			if (Settings.Default.ShowProgressWindow)
			{
				using (var progressForm = new ProgressForm { TopMost = true, StartPosition = FormStartPosition.Manual, })
				{
					progressForm.Left = Screen.PrimaryScreen.WorkingArea.Right - progressForm.Width - 20;
					progressForm.Top = Screen.PrimaryScreen.WorkingArea.Bottom - progressForm.Height - 30;

					configuration.ProgressAction = delegate (int progress, string message)
					{
						//hide the window if we have an error. This may not be foolproof, so rethink
						if (hasError)
							progress = 100;

						// ReSharper disable AccessToDisposedClosure
						progressForm.Invoke(progressForm.ProgressDelegate, progress,
							message);
					};

					Task.Run(() => Core.DirLister.List(configuration));
					Application.Run(progressForm);
				}
			}
			else
				Core.DirLister.List(configuration);


			if (hasError)
			{
				Application.Run(new MainForm(configuration, log.ToList(), folders));
			}
			else
			{
				AddToHistory(folders);
				Settings.Default.Save();
			}
		}

		private static Configuration ReadConfiguration()
		{
			var configuration = new Configuration();

			if (!string.IsNullOrWhiteSpace(Settings.Default.CssFile) && File.Exists(Settings.Default.CssFile))
				configuration.CssContent = File.ReadAllText(Settings.Default.CssFile);

			configuration.EnableMultithreading = Settings.Default.EnableMultithreading;
			configuration.FileDateFormat = Settings.Default.FileDateFormat;

			switch (Settings.Default.SelectedFilter)
			{
				case "Wildcard":
					if (Settings.Default.WildcardFilter != null && Settings.Default.WildcardFilter.Count > 0)
						configuration.Filter = new Filter(Settings.Default.WildcardFilter.Cast<string>().ToArray());
					break;
				case "Regular expression":
					if (!string.IsNullOrWhiteSpace(Settings.Default.RegexFilter))
						configuration.Filter = new Filter(Settings.Default.RegexFilter);
					break;
				default:
					configuration.Filter = null;
					break;
			}

			configuration.IncludeFileDates = Settings.Default.IncludeFileDates;
			configuration.IncludeHidden = Settings.Default.IncludeHidden;
			configuration.IncludeMediaInfo = Settings.Default.IncludeMediaInfo;
			configuration.IncludeSize = Settings.Default.IncludeSize;
			configuration.IncludeSubfolders = Settings.Default.IncludeSubfolders;
			configuration.OpenAfter = Settings.Default.OpenAfter;
			configuration.OutputFolder = string.IsNullOrWhiteSpace(Settings.Default.OutputFolder) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DirLister") : Settings.Default.OutputFolder;

			if (Settings.Default.OutputFormats != null && Settings.Default.OutputFormats.Count > 0)
				configuration.OutputFormats = Settings.Default.OutputFormats.Cast<string>().Select(x => (OutputFormat)Enum.Parse(typeof(OutputFormat), x)).ToList();

			return configuration;
		}


		internal static void AddToHistory(params string[] folders)
		{
			if (Settings.Default.DirectoryHistory == null)
				Settings.Default.DirectoryHistory = new StringCollection();

			var history = Settings.Default.DirectoryHistory.Cast<string>().ToList();

			history.InsertRange(0, folders);

			Settings.Default.DirectoryHistory.Clear();
			Settings.Default.DirectoryHistory.AddRange(history.Distinct().Take(Settings.Default.HistoryLength).ToArray());
		}
	}
}
