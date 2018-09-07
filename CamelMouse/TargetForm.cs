﻿using CamelMouse.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CamelMouse
{
	public partial class TargetForm : Form
	{
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

		public void SetLayeredWindow(Bitmap srcBitmap)
		{
			// GetDeviceContext
			IntPtr screenDc = IntPtr.Zero;
			IntPtr memDc = IntPtr.Zero;
			IntPtr hBitmap = IntPtr.Zero;
			IntPtr hOldBitmap = IntPtr.Zero;
			try {
				screenDc = GetDC(IntPtr.Zero);
				memDc = CreateCompatibleDC(screenDc);
				hBitmap = srcBitmap.GetHbitmap(Color.FromArgb(0));
				hOldBitmap = SelectObject(memDc, hBitmap);

				// init BLENDFUNCTION
				BLENDFUNCTION blend = new BLENDFUNCTION();
				blend.BlendOp = AC_SRC_OVER;
				blend.BlendFlags = 0;
				blend.SourceConstantAlpha = 255;
				blend.AlphaFormat = AC_SRC_ALPHA;

				// Update Layered Window
				this.Size = new Size(srcBitmap.Width, srcBitmap.Height);
				Point pptDst = new Point(this.Left, this.Top);
				Size psize = new Size(this.Width, this.Height);
				Point pptSrc = new Point(0, 0);
				UpdateLayeredWindow(this.Handle, screenDc, ref pptDst, ref psize, memDc,
				  ref pptSrc, 0, ref blend, ULW_ALPHA);

			} finally {
				if (screenDc != IntPtr.Zero) {
					ReleaseDC(IntPtr.Zero, screenDc);
				}
				if (hBitmap != IntPtr.Zero) {
					SelectObject(memDc, hOldBitmap);
					DeleteObject(hBitmap);
				}
				if (memDc != IntPtr.Zero) {
					DeleteDC(memDc);
				}
			}
		}

		public TargetForm()
		{
			InitializeComponent();
		}

		protected override CreateParams CreateParams
		{
			get
			{
				System.Windows.Forms.CreateParams cp = base.CreateParams;

				cp.ExStyle = cp.ExStyle | WindowsConst.WS_EX_LAYERED;
				//必要に応じて WS_EX_TRANSPARENT をつける
				if (this.FormBorderStyle != FormBorderStyle.None) {
					cp.Style = cp.Style & (~WindowsConst.WS_BORDER);
					cp.Style = cp.Style & (~WindowsConst.WS_THICKFRAME);
				}

				return cp;
			}
		}

		private void TargetForm_Load(object sender, EventArgs e)
		{
			Bitmap bmp = new Bitmap(@"c:\map\chn2.bmp");    // todo:

			Rectangle bmBounds = new Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData bmpd = bmp.LockBits(bmBounds, ImageLockMode.ReadOnly, bmp.PixelFormat);

			Bitmap bmp2 = new Bitmap(bmpd.Width, bmpd.Height, bmpd.Stride, PixelFormat.Format32bppArgb, bmpd.Scan0);

			bmp.UnlockBits(bmpd);
			SetLayeredWindow(bmp2);
		}
	}
}
