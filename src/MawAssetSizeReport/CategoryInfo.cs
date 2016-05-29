using System;
using System.Collections.Generic;
using System.Linq;


namespace MawAssetSizeReport
{
	public class CategoryInfo
	{
		public uint Year { get; set; }
		public string Name { get; set; }
		public IList<DirInfo> ChildDirectoryInfo { get; private set; }

		public long TotalSizeInBytes
		{
			get {
				return ChildDirectoryInfo.Sum(x => x.SizeInBytes);
			}
		}

		public double TotalSizeInKiloBytes
		{
			get {
				return ChildDirectoryInfo.Sum(x => x.SizeInKiloBytes);
			}
		}

		public double TotalSizeInMegaBytes {
			get {
				return ChildDirectoryInfo.Sum(x => x.SizeInMegaBytes);
			}
		}

		public double TotalSizeInGigaBytes {
			get {
				return ChildDirectoryInfo.Sum(x => x.SizeInGigaBytes);
			}
		}


		public CategoryInfo()
		{
			ChildDirectoryInfo = new List<DirInfo>();
		}
	}
}

