using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelMouse.Ctrl
{
	public class RangeData
	{
		public int Count { get; private set; }

		public (int Location, int Width)[] Range { get; private set; }

		public RangeData()
		{
			Count = 0;
			Range = new(int, int)[20];
		}

		public void Clear()
		{
			Count = 0;
		}

		/// <summary>
		/// 移動先追加
		/// </summary>
		/// <param name="location">先頭位置</param>
		/// <param name="width">幅</param>
		public void AddRange(int location, int width)
		{
			for (int i = 0; i < Count; i++) {
				if (Range[i].Location > location) {
					for (int j = Count; i < j; j--) {
						Range[j] = Range[j - 1];
					}
					Range[i].Location = location;
					Range[i].Width = width;
					Count++;
					return;
				}
			}
			Range[Count].Location = location;
			Range[Count].Width = width;
			Count++;
		}

		/// <summary>
		/// 移動先の位置を求める
		/// 引数の幅と、追加した移動先の幅の合計で比例計算
		/// </summary>
		/// <param name="x">位置</param>
		/// <param name="xLocation">先頭位置</param>
		/// <param name="xWidth">幅</param>
		/// <returns></returns>
		public int Convert(int x, int xLocation, int xWidth)
		{
			int a = xWidth;
			int b = 0;
			for (int i = 0; i < Count; i++) {
				b += Range[i].Width;
			}
			int result = b * (x - xLocation) / a;

			b = 0;
			int yLocation = 0;
			for (int i = 0; i < Count; i++) {
				b += Range[i].Width;
				if (result <= b) {
					yLocation += Range[i].Location;
					break;
				} else {
					yLocation -= Range[i].Width;
				}
			}
			result += yLocation;
			return result;
		}
	}
}
