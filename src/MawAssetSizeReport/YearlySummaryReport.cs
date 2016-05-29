using System;
using System.Collections.Generic;
using System.Linq;


namespace MawAssetSizeReport
{
	public class YearlySummaryReport
		: IReport
	{
		public string Name { get { return "Yearly Summary Report"; } }
		public string[] Columns { get { return new string[] { "Year", "Total (MB)" }; } }


		public IEnumerable<object[]> GenerateReport(IEnumerable<CategoryInfo> data)
		{
			var result = data.GroupBy(x => x.Year)
				             .Select(gr => new { 
					             Year = gr.Key,
					             TotalMb = gr.Sum(x => x.TotalSizeInMegaBytes)
				             })
				             .OrderBy(x => x.Year);

			foreach(var r in result)
			{
				yield return new object[] {
					r.Year,
					r.TotalMb
				};
			}
		}
	}
}

