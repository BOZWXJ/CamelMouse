using CamelMouse.Lib;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace CamelMouse
{
	public partial class TargetForm : Form
	{
		Bitmap image;
		Bitmap bmp;

		double scale = 1;

		public TargetForm()
		{
			InitializeComponent();

			image = new Bitmap(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"image\Target.png"));  // todo: 設定項目
			bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppPArgb);
			using (Graphics g = Graphics.FromImage(bmp)) {
				g.DrawImage(image, 0, 0);
			}
		}

		public new void Show()
		{
			scale = 1;
			timer1.Enabled = true;
			base.Show();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			scale -= 1.0 / 20;      // todo: 設定項目
			if (scale < 0.01) {
				timer1.Enabled = false;
				this.Hide();
			}

			using (Graphics g = Graphics.FromImage(bmp)) {
				g.Clear(Color.Transparent);
				float w = (float)(image.Width * scale);
				float h = (float)(image.Height * scale);
				float x = (image.Width - w) / 2;
				float y = (image.Height - h) / 2;
				g.DrawImage(image, x, y, w, h);
			}
			this.Size = image.Size;
			this.Location = new Point(Cursor.Position.X - image.Width / 2, Cursor.Position.Y - image.Height / 2);
			SetLayeredWindow(bmp);
		}

		public void SetLayeredWindow(Bitmap srcBitmap)
		{
			// GetDeviceContext
			IntPtr screenDc = IntPtr.Zero;
			IntPtr memDc = IntPtr.Zero;
			IntPtr hBitmap = IntPtr.Zero;
			IntPtr hOldBitmap = IntPtr.Zero;
			try {
				screenDc = Win32Api.GetDC(IntPtr.Zero);
				memDc = Win32Api.CreateCompatibleDC(screenDc);
				hBitmap = srcBitmap.GetHbitmap(Color.FromArgb(0));
				hOldBitmap = Win32Api.SelectObject(memDc, hBitmap);

				// init BLENDFUNCTION
				Win32Api.BLENDFUNCTION blend = new Win32Api.BLENDFUNCTION();
				blend.BlendOp = Win32Api.AC_SRC_OVER;
				blend.BlendFlags = 0;
				blend.SourceConstantAlpha = 255;
				blend.AlphaFormat = Win32Api.AC_SRC_ALPHA;

				// Update Layered Window
				Point pptDst = new Point(this.Left, this.Top);
				Size psize = new Size(this.Width, this.Height);
				Point pptSrc = new Point(0, 0);
				Win32Api.UpdateLayeredWindow(this.Handle, screenDc, ref pptDst, ref psize, memDc, ref pptSrc, 0, ref blend, Win32Api.ULW_ALPHA);

			} finally {
				if (screenDc != IntPtr.Zero) {
					Win32Api.ReleaseDC(IntPtr.Zero, screenDc);
				}
				if (hBitmap != IntPtr.Zero) {
					Win32Api.SelectObject(memDc, hOldBitmap);
					Win32Api.DeleteObject(hBitmap);
				}
				if (memDc != IntPtr.Zero) {
					Win32Api.DeleteDC(memDc);
				}
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;

				cp.ExStyle = cp.ExStyle | Win32Api.WS_EX_LAYERED | Win32Api.WS_EX_TOPMOST;
				if (this.FormBorderStyle != FormBorderStyle.None) {
					cp.Style = cp.Style & (~Win32Api.WS_BORDER);
					cp.Style = cp.Style & (~Win32Api.WS_THICKFRAME);
				}

				return cp;
			}
		}

	}
}
