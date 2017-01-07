using System.Collections.Generic;
using System.Linq;


namespace MawAssetSizeReport
{
	public class TotalSummaryReport
		: IReport
	{
		public string Name { get { return "Total Summary Report"; } }
		public string[] Columns { get { return new string[] { "Total (MB)", "Total (GB)" }; } }


		public IEnumerable<object[]> GenerateReport(IEnumerable<CategoryInfo> data)
		{
			yield return new object[] {
				data.Sum(x => x.TotalSizeInMegaBytes),
				data.Sum(x => x.TotalSizeInGigaBytes)
			};
		}
	}
}
