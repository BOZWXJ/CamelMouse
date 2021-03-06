﻿using System;
using System.Collections.Generic;
using System.Drawing;
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

		/// <summary>
		/// レイヤードウィンドウの位置、サイズ、形、内容、透明度を更新します。
		/// </summary>
		/// <param name="hwnd">レイヤードウィンドウのハンドル</param>
		/// <param name="hdcDst">画面のデバイスコンテキストのハンドル</param>
		/// <param name="pptDst">画面の新しい位置</param>
		/// <param name="psize">レイヤードウィンドウの新しいサイズ</param>
		/// <param name="hdcSrc">サーフェスのデバイスコンテキストのハンドル</param>
		/// <param name="pptSrc">レイヤの位置</param>
		/// <param name="crKey">カラーキー</param>
		/// <param name="pblend">ブレンド機能</param>
		/// <param name="dwFlags">フラグ</param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int UpdateLayeredWindow(
			IntPtr hwnd,
			IntPtr hdcDst,
			[System.Runtime.InteropServices.In()]
			ref Point pptDst,
			[System.Runtime.InteropServices.In()]
			ref Size psize,
			IntPtr hdcSrc,
			[System.Runtime.InteropServices.In()]
			ref Point pptSrc,
			int crKey,
			[System.Runtime.InteropServices.In()]
			ref BLENDFUNCTION pblend,
			int dwFlags);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DeleteObject(IntPtr hobject);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DeleteDC(IntPtr hdc);

		public const byte AC_SRC_OVER = 0;
		public const byte AC_SRC_ALPHA = 1;
		public const int ULW_ALPHA = 2;

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct BLENDFUNCTION
		{
			public byte BlendOp;
			public byte BlendFlags;
			public byte SourceConstantAlpha;
			public byte AlphaFormat;
		}

		public const int WM_NCHITTEST = 0x0084;

		//
		// Window Styles
		//
		public const int WS_OVERLAPPED = 0x00000000;
		public const long WS_POPUP = 0x80000000L;       //unchecked((int) WS_POPUP) 
		public const int WS_CHILD = 0x40000000;
		public const int WS_MINIMIZE = 0x20000000;
		public const int WS_VISIBLE = 0x10000000;
		public const int WS_DISABLED = 0x08000000;
		public const int WS_CLIPSIBLINGS = 0x04000000;
		public const int WS_CLIPCHILDREN = 0x02000000;
		public const int WS_MAXIMIZE = 0x01000000;
		public const int WS_CAPTION = 0x00C00000;       // WS_BORDER | WS_DLGFRAME
		public const int WS_BORDER = 0x00800000;
		public const int WS_DLGFRAME = 0x00400000;
		public const int WS_VSCROLL = 0x00200000;
		public const int WS_HSCROLL = 0x00100000;
		public const int WS_SYSMENU = 0x00080000;
		public const int WS_THICKFRAME = 0x00040000;
		public const int WS_GROUP = 0x00020000;
		public const int WS_TABSTOP = 0x00010000;

		public const int WS_MINIMIZEBOX = 0x00020000;
		public const int WS_MAXIMIZEBOX = 0x00010000;

		public const int WS_TILED = WS_OVERLAPPED;
		public const int WS_ICONIC = WS_MINIMIZE;
		public const int WS_SIZEBOX = WS_THICKFRAME;
		public const int WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;

		//
		// Common Window Styles
		//
		public const int WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED |
												WS_CAPTION |
												WS_SYSMENU |
												WS_THICKFRAME |
												WS_MINIMIZEBOX |
												WS_MAXIMIZEBOX);

		public const long WS_POPUPWINDOW = (WS_POPUP |
											WS_BORDER |
											WS_SYSMENU);

		public const int WS_CHILDWINDOW = (WS_CHILD);

		//
		// Extended Window Styles
		//
		public const int WS_EX_DLGMODALFRAME = 0x00000001;
		public const int WS_EX_NOPARENTNOTIFY = 0x00000004;
		public const int WS_EX_TOPMOST = 0x00000008;
		public const int WS_EX_ACCEPTFILES = 0x00000010;
		public const int WS_EX_TRANSPARENT = 0x00000020;
		public const int WS_EX_MDICHILD = 0x00000040;
		public const int WS_EX_TOOLWINDOW = 0x00000080;
		public const int WS_EX_WINDOWEDGE = 0x00000100;
		public const int WS_EX_CLIENTEDGE = 0x00000200;
		public const int WS_EX_CONTEXTHELP = 0x00000400;

		public const int WS_EX_RIGHT = 0x00001000;
		public const int WS_EX_LEFT = 0x00000000;
		public const int WS_EX_RTLREADING = 0x00002000;
		public const int WS_EX_LTRREADING = 0x00000000;
		public const int WS_EX_LEFTSCROLLBAR = 0x00004000;
		public const int WS_EX_RIGHTSCROLLBAR = 0x00000000;

		public const int WS_EX_CONTROLPARENT = 0x00010000;
		public const int WS_EX_STATICEDGE = 0x00020000;
		public const int WS_EX_APPWINDOW = 0x00040000;

		public const int WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
		public const int WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
		public const int WS_EX_LAYERED = 0x00080000;

		public const int WS_EX_NOINHERITLAYOUT = 0x00100000;    // Disable inheritence of mirroring by children
		public const int WS_EX_LAYOUTRTL = 0x00400000;          // Right to left mirroring

		public const int WS_EX_COMPOSITED = 0x02000000;
		public const int WS_EX_NOACTIVATE = 0x08000000;

		//
		// WM_NCHITTEST and MOUSEHOOKSTRUCT Mouse Position Codes
		//
		public const int HTERROR = (-2);
		public const int HTTRANSPARENT = (-1);
		public const int HTNOWHERE = 0;
		public const int HTCLIENT = 1;
		public const int HTCAPTION = 2;
		public const int HTSYSMENU = 3;
		public const int HTGROWBOX = 4;
		public const int HTSIZE = HTGROWBOX;
		public const int HTMENU = 5;
		public const int HTHSCROLL = 6;
		public const int HTVSCROLL = 7;
		public const int HTMINBUTTON = 8;
		public const int HTMAXBUTTON = 9;
		public const int HTLEFT = 10;
		public const int HTRIGHT = 11;
		public const int HTTOP = 12;
		public const int HTTOPLEFT = 13;
		public const int HTTOPRIGHT = 14;
		public const int HTBOTTOM = 15;
		public const int HTBOTTOMLEFT = 16;
		public const int HTBOTTOMRIGHT = 17;
		public const int HTBORDER = 18;
		public const int HTREDUCE = HTMINBUTTON;
		public const int HTZOOM = HTMAXBUTTON;
		public const int HTSIZEFIRST = HTLEFT;
		public const int HTSIZELAST = HTBOTTOMRIGHT;

		public const int HTOBJECT = 19;
		public const int HTCLOSE = 20;
		public const int HTHELP = 21;

	}
}
