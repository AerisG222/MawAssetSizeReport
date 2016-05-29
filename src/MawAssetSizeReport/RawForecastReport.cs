using System;
using System.Collections.Generic;
using System.Linq;


namespace MawAssetSizeReport
{
	public class RawForecastReport
		: IReport
	{
		public string Name { get { return "Raw Forecast Report"; } }
		public string[] Columns { get { return new string[] { "Year", "Yearly w/ Raw (GB)", "Yearly w/o Raw (GB)", "Total w/ Raw (GB)", "Total w/o Raw (GB)" }; } }


		public IEnumerable<object[]> GenerateReport(IEnumerable<CategoryInfo> data)
		{
			var fr = new ForecastReport ();
			var yrly = fr.GetYearlyForecastInMb (data);
			var yrlyNoRaw = fr.GetYearlyForecastNoRawInMb (data);

			var allData = data.SelectMany (x => x.ChildDirectoryInfo, (catInfo, dirInfoCol) => new { 
				Year = catInfo.Year, 
				CategoryName = catInfo.Name, 
				ScaleName = dirInfoCol.Name,
				ScaleSizeInMb = dirInfoCol.SizeInMegaBytes
			});

			var noRaw = allData
				.Where (x => x.ScaleName != "raw")
				.GroupBy(x => x.Year, (grp, kids) => new { 
					Year = grp, 
					TotalSizeInMb = kids.Sum(x => x.ScaleSizeInMb)
				})
				.OrderBy(x => x.Year);

			var withRaw = allData
				.GroupBy(x => x.Year, (grp, kids) => new { 
					Year = grp, 
					TotalSizeInMb = kids.Sum(x => x.ScaleSizeInMb)
				})
				.OrderBy(x => x.Year);

			double totalWithRaw = 0;
			double totalNoRaw = 0;
			uint lastYear = 0;

			for(int i = 0; i < noRaw.Count(); i++)
			{
				var currNoRaw = noRaw.ElementAt(i);
				var currRaw = withRaw.ElementAt(i);

				totalWithRaw += currRaw.TotalSizeInMb;
				totalNoRaw += currNoRaw.TotalSizeInMb;

				yield return new object[] {
					currRaw.Year,
					currRaw.TotalSizeInMb / 1000d,
					currNoRaw.TotalSizeInMb / 1000d,
					totalWithRaw / 1000d,
					totalNoRaw / 1000d
				};

				lastYear = currRaw.Year;
			}

			for(int i = 1; i <= 10; i++)
			{
				totalNoRaw += yrlyNoRaw;
				totalWithRaw += yrly;

				yield return new object[] {
					lastYear + i,
					yrly / 1000d,
					yrlyNoRaw / 1000d,
					totalWithRaw / 1000d,
					totalNoRaw / 1000d
				};
			}
		}
	}
}

