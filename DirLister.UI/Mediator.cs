using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.Core.Application;
using Sander.DirLister.UI.App;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	internal static class Mediator
	{
		internal static void Run(params string[] folders)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			folders = folders?.Select(x => Utils.EnsureBackslash(x.Trim('"'))).ToArray();

			var configuration = ReadConfiguration();
			//folders = new[] { @"c:\dev", @"c:\tools", @"c:\temp" };
			//configuration.IncludeMediaInfo = true;
			if (Settings.Default.FirstRun || Settings.Default.ShowUiFromShell || folders == null || folders.Length == 0)
			{

				Application.Run(new MainForm(configuration, null, folders));
				Settings.Default.Save();
			}
			else
			{
				var silent = new SilentRunner(configuration);
				silent.RunSilent(folders);
			}
		}


		private static Configuration ReadConfiguration()
		{
			var configuration = new Configuration();

			if (!string.IsNullOrWhiteSpace(Settings.Default.CssFile) && File.Exists(Settings.Default.CssFile))
				configuration.CssContent = File.ReadAllText(Settings.Default.CssFile);

			configuration.EnableMultithreading = Settings.Default.EnableMultithreading;
			configuration.FileDateFormat = Settings.Default.FileDateFormat;

			//see about less chatty implementation
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
			configuration.OutputFolder = string.IsNullOrWhiteSpace(Settings.Default.OutputFolder)
				? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DirLister")
				: Settings.Default.OutputFolder;

			if (Settings.Default.OutputFormats != null && Settings.Default.OutputFormats.Count > 0)
				configuration.OutputFormats = Settings.Default.OutputFormats.Cast<string>()
					.Select(x => (OutputFormat)Enum.Parse(typeof(OutputFormat), x)).ToList();

			return configuration;
		}


		internal static void AddToHistory(params string[] folders)
		{
			if (Settings.Default.DirectoryHistory == null)
				Settings.Default.DirectoryHistory = new StringCollection();

			var history = Settings.Default.DirectoryHistory.Cast<string>().ToList();

			history.InsertRange(0, folders);

			Settings.Default.DirectoryHistory.Clear();
			Settings.Default.DirectoryHistory.AddRange(
				history.Distinct(StringComparer.OrdinalIgnoreCase).Take(Settings.Default.HistoryLength).ToArray());
		}
	}
}
