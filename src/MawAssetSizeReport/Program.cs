using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace MawAssetSizeReport
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			if(args.Length < 1)
			{
				ShowUsage();
				Environment.Exit(1);
			}

			var dir = new DirectoryInfo(args[0]);

			if(!dir.Exists)
			{
				Console.WriteLine($"directory [{dir}] does not exist!");
				Environment.Exit(2);
			}

			var reports = GetReportsToRun(args);
			var data = GatherDirectoryData(dir).ToList();

			var formatter = new ConsoleReportFormatter();

			foreach(var report in reports)
			{
				formatter.OutputReport(report, report.GenerateReport(data).ToList());
			}
		}


		static IEnumerable<IReport> GetReportsToRun(string[] args)
		{
			var reports = new List<IReport>();

			if(args.Length == 2)
			{
				var arg = args[1];

				foreach(char c in arg)
				{
					switch(c)
					{
						case 'c':
							reports.Add(new CategorySummaryReport());
							break;
						case 'd':
							reports.Add(new DetailReport());
							break;
						case 'f':
							reports.Add (new ForecastReport());
							break;
						case 'r':
							reports.Add (new RawForecastReport());
							break;
						case 's':
							reports.Add(new YearlyScaleSummaryReport());
							break;
						case 't':
							reports.Add(new TotalSummaryReport());
							break;
						case 'y':
							reports.Add(new YearlySummaryReport());
							break;
					}
				}
			}

			if(reports.Count == 0)
			{
				reports.Add(new CategorySummaryReport());
			}

			return reports;
		}


		static IEnumerable<CategoryInfo> GatherDirectoryData(DirectoryInfo rootDir)
		{
			var yearDirs = rootDir.GetDirectories();

			foreach(var yearDir in yearDirs)
			{
				uint year;

				if(uint.TryParse(yearDir.Name, out year))
				{
					Console.WriteLine("Gathering details for year {0}...", year);

					foreach(var catDir in yearDir.GetDirectories())
					{
						var catInfo = new CategoryInfo { 
							Name = catDir.Name,
							Year = year
						};

						foreach(var scaleDir in catDir.GetDirectories())
						{
							var scaleInfo = new DirInfo {
								Name = scaleDir.Name
							};

							long size = 0;

							foreach(var file in scaleDir.GetFiles())
							{
								size += file.Length;
							}

							scaleInfo.SizeInBytes = size;

							catInfo.ChildDirectoryInfo.Add(scaleInfo);
						}

						yield return catInfo;
					}
				}
			}
		}


		static void ShowUsage()
		{
			Console.WriteLine("msr.exe <dir> [reports]");
			Console.WriteLine("  where <dir> is the root of images or videos");
			Console.WriteLine("  and [reports] is the type of reports to run:");
			Console.WriteLine("      c = category summary");
			Console.WriteLine("      d = detailed");
			Console.WriteLine("      f = forecast");
			Console.WriteLine("      s = yearly scale summary");
			Console.WriteLine("      t = total summary");
			Console.WriteLine("      y = yearly summary");
		}
	}
}
