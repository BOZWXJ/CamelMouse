using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CamelMouse.Lib
{
	public static class Win32Api
	{
		/// <summary>
		/// ダブルクリック間隔を取得します。
		/// </summary>
		/// <returns>ダブルクリック間隔(ms)</returns>
		[DllImport("User32.dll")]
		public static extern uint GetDoubleClickTime();
	}
}
