using System;
using System.Collections.Generic;
using System.Linq;


namespace MawAssetSizeReport
{
	public class CategorySummaryReport
		: IReport
	{
		public string Name { get { return "Category Summary Report"; } }
		public string[] Columns { get { return new string[] { "Year", "Category", "Total (MB)", "Total (GB)" }; } }


		public IEnumerable<object[]> GenerateReport(IEnumerable<CategoryInfo> data)
		{
			foreach(var d in data)
			{
				yield return new object[] { 
					d.Year,
					d.Name,
					d.TotalSizeInMegaBytes,
					d.TotalSizeInGigaBytes
				};
			}
		}
	}
}

