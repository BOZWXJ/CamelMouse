using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelMouse.Lib
{
	static class RectangleExtensions
	{
		public static int X2(this Rectangle rect)
		{
			return rect.X + rect.Width - 1;
		}

		public static int Y2(this Rectangle rect)
		{
			return rect.Y + rect.Height - 1;
		}
	}
}
