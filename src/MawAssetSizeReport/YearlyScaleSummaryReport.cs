using System;
using System.Collections.Generic;
using System.Linq;


namespace MawAssetSizeReport
{
	public class YearlyScaleSummaryReport
		: IReport
	{
		List<string> _cols = new List<string>();

		public string Name { get { return "Yearly Scale Summary Report"; } }


		public string[] Columns { 
			get 
			{ 
				return _cols.ToArray();
			} 
		}


		public IEnumerable<object[]> GenerateReport(IEnumerable<CategoryInfo> data)
		{
			_cols.Clear();
			_cols.Add("Year");

			var result = data
				.SelectMany(x => x.ChildDirectoryInfo, (parent, child) => new { 
					Year = parent.Year,
					ScaleName = child.Name,
					ScaleSizeInMegaBytes = child.SizeInMegaBytes
				})
				.GroupBy(x => new 
					{ 
						x.Year, 
						x.ScaleName
					}, (grp, kids) => new
					{
						grp.Year,
						grp.ScaleName,
						TotalSizeInMegaBytes = kids.Sum(k => k.ScaleSizeInMegaBytes)
					}
				)
				.OrderBy(x => x.Year)
				.ToList();

			_cols.AddRange(result.Select(x => x.ScaleName + " (MB)").Distinct().OrderBy(x => x).ToList());

			foreach(var year in result.Select(x => x.Year).Distinct())
			{
				var o = new object[_cols.Count];

				o[0] = year;

				for(int i = 1; i < _cols.Count; i++)
				{
					var theYear = year;
					var scaleName = _cols[i].Replace(" (MB)", "");

					o[i] = result.Where(x => x.Year == theYear && string.Equals(x.ScaleName, scaleName, StringComparison.OrdinalIgnoreCase))
						.Select(x => x.TotalSizeInMegaBytes)
						.SingleOrDefault();
				}

				yield return o;
			}
		}
	}
}

