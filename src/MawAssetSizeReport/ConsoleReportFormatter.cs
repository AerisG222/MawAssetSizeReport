using System;
using System.Linq;
using System.Collections.Generic;


namespace MawAssetSizeReport
{
	public class ConsoleReportFormatter
		: IReportFormatter
	{
		static readonly string HLINE = new String('-', 80);


		public void OutputReport(IReport report, IEnumerable<object[]> data)
		{
			int[] widths = GetColumnWidths(report, data);

			Console.WriteLine("{0}:", report.Name);
			Console.WriteLine(HLINE);
			WriteRow(report.Columns, widths);
			Console.WriteLine(HLINE);

			foreach(var d in data)
			{
				WriteRow(d, widths);
			}

			Console.WriteLine();
		}


		void WriteRow(object[] data, int[] widths)
		{
			for(int i = 0; i < widths.Length; i++)
			{
				if(i > 0)
				{
					Console.Write("|");
				}

				object d = data[i];

				if(d is int || d is uint || d is long || d is ulong || d is short || d is ushort || d is byte || d is double || d is float)
				{
					Console.Write(Stringify(d).PadLeft(widths[i]));
				}
				else
				{
					Console.Write(Stringify(d).PadRight(widths[i]));
				}
			}

			Console.WriteLine();
		}


		int[] GetColumnWidths(IReport report, IEnumerable<object[]> data)
		{
			var widths = new int[report.Columns.Length];

			for(int i = 0; i < report.Columns.Length; i++)
			{
				widths[i] = Math.Max(report.Columns[i].Length, data.Max(x => Stringify(x[i]).Length));
			}

			return widths;
		}


		string Stringify(object value)
		{
			if(value == null)
			{
				return "--";
			}

			if(value is double || value is float)
			{
				return ((double)value).ToString("N2");
			}

			return value.ToString();
		}
	}
}
