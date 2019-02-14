using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sander.DirLister.Core;

namespace DirLister.Core.Tests
{
	[TestClass]
	public class FullRun
	{

		private static Configuration GetConfiguration()
		{
			var configuration = new Configuration
			{
				IncludeFileDates = true,
				IncludeMediaInfo = true,
				IncludeSize = true,
				OpenAfter = true,
				IncludeHidden = true,
				IncludeSubfolders = true,
				InputFolders = new List<string>
				{
					@"C:\Temp", @"c:\tools\"
				},
				ProgressAction = (i, s) => Trace.WriteLine($"[Progress] {i} {s}")
			};

			return configuration;
		}

		[TestMethod]
		public void TestXmlOut()
		{
			var configuration = GetConfiguration();
			configuration.OutputFormats = new List<OutputFormat>
				{ OutputFormat.Xml };
			Sander.DirLister.Core.DirLister.List(configuration);
		}

		[TestMethod]
		public void TestCsvOut()
		{
			var configuration = GetConfiguration();
			configuration.OutputFormats = new List<OutputFormat>
				{ OutputFormat.Csv };
			Sander.DirLister.Core.DirLister.List(configuration);
		}


		[TestMethod]
		public void TestJsonOut()
		{
			var configuration = GetConfiguration();
			configuration.OutputFormats = new List<OutputFormat>
				{ OutputFormat.Json };
			Sander.DirLister.Core.DirLister.List(configuration);
		}


		[TestMethod]
		public void TestTxtOut()
		{
			var configuration = GetConfiguration();
			configuration.OutputFormats = new List<OutputFormat>
				{ OutputFormat.Txt };
			Sander.DirLister.Core.DirLister.List(configuration);
		}


		[TestMethod]
		public void TestHtmlOut()
		{
			var configuration = GetConfiguration();
			configuration.OutputFormats = new List<OutputFormat>
				{ OutputFormat.Html };
			configuration.EnableMultithreading = true;
			//configuration.CssContent = "* {}";
			//configuration.IncludeSubfolders = false;
			//configuration.IncludeFileDates = false;
			//configuration.Filter = new Filter(new []{"*.avi", "*.exe"});
			Sander.DirLister.Core.DirLister.List(configuration);
		}

		[TestMethod]
		public void TestAllOut()
		{
			var configuration = GetConfiguration();
			configuration.OutputFormats = new List<OutputFormat>
				{ OutputFormat.Html, OutputFormat.Csv, OutputFormat.Json, OutputFormat.Txt, OutputFormat.Xml };
			Sander.DirLister.Core.DirLister.List(configuration);
		}
	}
}
