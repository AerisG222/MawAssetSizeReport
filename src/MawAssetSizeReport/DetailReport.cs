using System.Collections.Generic;


namespace MawAssetSizeReport
{
	public class DetailReport
		: IReport
	{
		public string Name { get { return "Detail Report"; } }
		public string[] Columns { get { return new string[] { "Year", "Category", "Scaled Dir", "Total (MB)" }; } }


		public IEnumerable<object[]> GenerateReport(IEnumerable<CategoryInfo> data)
		{
			foreach(var ci in data)
			{
				foreach(var si in ci.ChildDirectoryInfo)
				{
					yield return new object[] {
						ci.Year, 
						ci.Name, 
						si.Name, 
						si.SizeInMegaBytes
					};
				}
			}
		}
	}
}
