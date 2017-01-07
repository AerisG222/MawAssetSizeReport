using System.Collections.Generic;


namespace MawAssetSizeReport
{
	public interface IReportFormatter
	{
		void OutputReport(IReport report, IEnumerable<object[]> data);
	}
}
