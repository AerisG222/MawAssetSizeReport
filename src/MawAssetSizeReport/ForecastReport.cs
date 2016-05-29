using System;
using System.Collections.Generic;
using System.Linq;


namespace MawAssetSizeReport
{
	public class ForecastReport
		: IReport
	{
		const int YEARS_FOR_AVG = 3;
		public string Name { get { return "Forecast Report (with RAW)"; } }
		public string[] Columns { get { return new string[] { "Year", "Yearly (MB)", "Yearly (GB)", "Total (MB)", "Total (GB)" }; } }


		public IEnumerable<object[]> GenerateReport(IEnumerable<CategoryInfo> data)
		{
			var currYear = DateTime.Now.Year;
			var result = GetYearlyForecastInMb(data);

			for (int i = 0; i < 10; i++) {
				yield return new object[] {
					currYear + i,
					result,
					result / 1000d,
					(1 + i) * result,
					(1 + i) * result / 1000d
				};
			}
		}


		public double GetYearlyForecastInMb(IEnumerable<CategoryInfo> data)
		{
			var currYear = DateTime.Now.Year;
			var startYear = currYear - (YEARS_FOR_AVG + 1);
			var endYear = currYear - 1;

			return data
				.Where (x => x.Year >= startYear && x.Year <= endYear)
				.Sum (x => x.TotalSizeInMegaBytes) / (double)YEARS_FOR_AVG;
		}


		public double GetYearlyForecastNoRawInMb(IEnumerable<CategoryInfo> data)
		{
			var currYear = DateTime.Now.Year;
			var startYear = currYear - (YEARS_FOR_AVG + 1);
			var endYear = currYear - 1;

			return data
				.Where (x => x.Year >= startYear && x.Year <= endYear)
				.SelectMany(x => x.ChildDirectoryInfo, (cat, s) => new { ScaleName = s.Name, SizeInMegaBytes = s.SizeInMegaBytes })
				.Where(x => x.ScaleName != "raw")
				.Sum(x =>  x.SizeInMegaBytes) / (double)YEARS_FOR_AVG;
		}
	}
}

