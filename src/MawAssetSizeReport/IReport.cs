using System.Collections.Generic;


namespace MawAssetSizeReport
{
	public interface IReport
	{
		string Name { get; }
		string[] Columns { get; }
		IEnumerable<object[]> GenerateReport(IEnumerable<CategoryInfo> data);
	}
}
