namespace MawAssetSizeReport
{
	public class DirInfo
	{
		public string Name { get; set; }
		public long SizeInBytes { get; set;}

		public double SizeInKiloBytes {
			get {
				return SizeInBytes / 1000d;  // 1000 to try and better match how vendors report disk size
			}
		}

		public double SizeInMegaBytes {
			get {
				return SizeInBytes / (1000d * 1000d);
			}
		}

		public double SizeInGigaBytes {
			get {
				return SizeInBytes / (1000d * 1000d * 1000d);
			}
		}
	}
}
