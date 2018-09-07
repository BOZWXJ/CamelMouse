using Microsoft.VisualStudio.TestTools.UnitTesting;
using CamelMouse.Ctrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelMouse.Ctrl.Tests
{
	[TestClass()]
	public class RangeDataTests
	{
		[TestMethod()]
		public void ClearTest()
		{
			RangeData rangeData = new RangeData();
			rangeData.AddRange(0, 1920);
			rangeData.Clear();
			rangeData.Count.Is(0);
		}

		[TestMethod()]
		public void AddRangeTest()
		{
			RangeData rangeData = new RangeData();
			rangeData.AddRange(100, 100);
			rangeData.Count.Is(1);
			rangeData.Range.Take(1).Is(new(int, int)[] { (100, 100) });

			rangeData.AddRange(200, 100);
			rangeData.Count.Is(2);
			rangeData.Range.Take(2).Is(new(int, int)[] { (100, 100), (200, 100) });

			rangeData.AddRange(0, 100);
			rangeData.Count.Is(3);
			rangeData.Range.Take(3).Is(new(int, int)[] { (0, 100), (100, 100), (200, 100) });
		}

		[TestMethod()]
		public void ConvertTest1()
		{
			RangeData rangeData = new RangeData();
			rangeData.AddRange(0, 1920);
			rangeData.Convert(1024, 0, 4096).Is(480);
		}

		[TestMethod()]
		public void ConvertTest2()
		{
			RangeData rangeData = new RangeData();
			rangeData.AddRange(0, 1920);
			rangeData.AddRange(1920, 1920);
			rangeData.Convert(1024, 0, 4096).Is(960);
		}

		[TestMethod()]
		public void ConvertTest3()
		{
			RangeData rangeData = new RangeData();
			rangeData.AddRange(0, 1920);
			rangeData.AddRange(2176, 1920);
			rangeData.Convert(1024, 0, 4096).Is(960);
			rangeData.Convert(3072, 0, 4096).Is(3136);
			rangeData.Convert(0, 0, 4096).Is(0);
			rangeData.Convert(4095, 0, 4096).Is(4095);

			rangeData.Convert(1984, 0, 4096).Is(1860);
			rangeData.Convert(2112, 0, 4096).Is(2236);
		}

		[TestMethod()]
		public void ConvertTest4()
		{
			RangeData rangeData = new RangeData();
			rangeData.AddRange(0, 4096);
			rangeData.Convert(480, 0, 1920).Is(1024);
		}

	}
}