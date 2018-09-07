using CamelMouse.Config;
using CamelMouse.Ctrl;
using CamelMouse.Lib;
using MMFrame.Windows.GlobalHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CamelMouse
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			SettingRead();
			screenBounds = Screen.FromPoint(Cursor.Position).Bounds;

			MainFormUpdate();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			this.Hide();
			StartHook();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing) {
				this.Hide();
				e.Cancel = true;
			}
		}

		private void MainFormUpdate()
		{
			// MainFoam 表示内容更新

			TextBoxDisplay.Text = string.Empty;
			foreach (var s in Screen.AllScreens.Select((v, i) => new { v, i })) {
				TextBoxDisplay.Text += string.Format("{0} {1}:\"{2}\" {3}\r\n", s.v.Primary ? "[*]" : "[ ]", s.i, s.v.DeviceName, s.v.Bounds);
			}
			label1.Text = string.Format("{0}", screenBounds);
		}

		private void AppSettingUpdate()
		{
			// フォームから AppSetting へ

		}

		private void SettingRead()
		{
			settingWarpLeft = AppSettings.Instance.WarpLeft;
			settingWarpRight = AppSettings.Instance.WarpRight;
			settingWarpUp = AppSettings.Instance.WarpUp;
			settingWarpDown = AppSettings.Instance.WarpDown;
			settingMargin = AppSettings.Instance.Margin;
			settingHold = AppSettings.Instance.Hold;
			settingHoldCount = AppSettings.Instance.HoldCount;
			settingCornerDisable = AppSettings.Instance.CornerDisable;
			settingCornerSize = settingCornerDisable ? AppSettings.Instance.CornerSize : 0;
		}

		private void StartHook()
		{
			MouseHook.AddEvent(HookMouse);
			MouseHook.Start();

			KeyboardHook.AddEvent(HookKeyboard);
			KeyboardHook.Start();
		}

		private void StopHook()
		{
			MouseHook.RemoveEvent(HookMouse);
			MouseHook.Stop();

			KeyboardHook.RemoveEvent(HookKeyboard);
			KeyboardHook.Stop();
		}

		#region メニュー

		private void SettingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 設定
			MainFormUpdate();
			this.Show();
		}

		private void InfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// バージョン情報
			AboutBox.Show();
		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 終了
			Application.Exit();
		}

		#endregion

		#region コントロール

		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			// 設定
			MainFormUpdate();
			this.Show();
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			// OK
			AppSettingUpdate();
			SettingRead();
			this.Hide();
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			// Cancel
			this.Hide();
		}

		#endregion

		bool settingWarpLeft;
		bool settingWarpRight;
		bool settingWarpUp;
		bool settingWarpDown;
		int settingMargin;
		bool settingHold;
		int settingHoldCount;
		bool settingCornerDisable;
		int settingCornerSize;



		/// <summary>現在の画面</summary>
		private Rectangle screenBounds;
		/// <summary>引っかかりカウンタ</summary>
		private int hold = 0;
		private void HookMouse(ref MouseHook.StateMouse s)
		{
			int x = s.X, y = s.Y;

			bool left = x < screenBounds.X;
			bool right = x > screenBounds.X2();
			bool top = y < screenBounds.Y;
			bool bottom = y > screenBounds.Y2();
			bool leftCorner = x < screenBounds.X + settingCornerSize;
			bool rightCorner = x > screenBounds.X2() - settingCornerSize;
			bool topCorner = y < screenBounds.Y + settingCornerSize;
			bool bottomCorner = y > screenBounds.Y2() - settingCornerSize;

			hold++;
			if ((left && top) || (left && topCorner) || (leftCorner && top)) {
				// 左上
				if (!settingCornerDisable && (!settingHold || hold > settingHoldCount)) {
					if (settingWarpLeft && !settingWarpUp) {
						// 左へ移動
						MoveScreen(Direction.Left, true, ref x, ref y);
					} else if (!settingWarpLeft && settingWarpUp) {
						// 上へ移動
						MoveScreen(Direction.Up, true, ref x, ref y);
					} else {
						// 左上へ移動
						MoveScreen(Direction.LeftUp, settingWarpLeft && settingWarpUp, ref x, ref y);
					}
				} else {
					// 移動させない
					x = left ? screenBounds.X : x;
					y = top ? screenBounds.Y : y;
				}
			} else if ((right && top) || (right && topCorner) || (rightCorner && top)) {
				// 右上
				if (!settingCornerDisable && (!settingHold || hold > settingHoldCount)) {
					if (settingWarpRight && !settingWarpUp) {
						// 右へ移動
						MoveScreen(Direction.Right, true, ref x, ref y);
					} else if (!settingWarpRight && settingWarpUp) {
						// 上へ移動
						MoveScreen(Direction.Up, true, ref x, ref y);
					} else {
						// 右上へ移動
						MoveScreen(Direction.RightUp, settingWarpRight && settingWarpUp, ref x, ref y);
					}
				} else {
					// 移動させない
					x = right ? screenBounds.X2() : x;
					y = top ? screenBounds.Y : y;
				}
			} else if ((left && bottom) || (left && bottomCorner) || (leftCorner && bottom)) {
				// 左下
				if (!settingCornerDisable && (!settingHold || hold > settingHoldCount)) {
					if (settingWarpLeft && !settingWarpDown) {
						// 左へ移動
						MoveScreen(Direction.LeftUp, true, ref x, ref y);
					} else if (!settingWarpLeft && settingWarpDown) {
						// 下へ移動
						MoveScreen(Direction.Down, true, ref x, ref y);
					} else {
						// 左下へ移動
						MoveScreen(Direction.LeftDown, settingWarpLeft && settingWarpDown, ref x, ref y);
					}
				} else {
					// 移動させない
					x = left ? screenBounds.X : x;
					y = bottom ? screenBounds.Y2() : y;
				}
			} else if ((right && bottom) || (right && bottomCorner) || (rightCorner && bottom)) {
				// 右下
				if (!settingCornerDisable && (!settingHold || hold > settingHoldCount)) {
					if (settingWarpRight && !settingWarpDown) {
						// 右へ移動
						MoveScreen(Direction.Right, true, ref x, ref y);
					} else if (!settingWarpRight && settingWarpDown) {
						// 下へ移動
						MoveScreen(Direction.Down, true, ref x, ref y);
					} else {
						// 右下へ移動
						MoveScreen(Direction.RightDown, settingWarpRight && settingWarpDown, ref x, ref y);
					}
				} else {
					// 移動させない
					x = right ? screenBounds.X2() : x;
					y = bottom ? screenBounds.Y2() : y;
				}
			} else if (left && !topCorner && !bottomCorner) {
				// 左
				if (!settingHold || hold > settingHoldCount) {
					// 移動
					MoveScreen(Direction.Left, settingWarpLeft, ref x, ref y);
				} else {
					// 移動させない
					x = screenBounds.X;
				}
			} else if (right && !topCorner && !bottomCorner) {
				// 右
				if (!settingHold || hold > settingHoldCount) {
					// 移動
					MoveScreen(Direction.Right, settingWarpRight, ref x, ref y);
				} else {
					// 移動させない
					x = screenBounds.X2();
				}
			} else if (top && !leftCorner && !rightCorner) {
				// 上
				if (!settingHold || hold > settingHoldCount) {
					// 移動
					MoveScreen(Direction.Up, settingWarpUp, ref x, ref y);
				} else {
					// 移動させない
					y = screenBounds.Y;
				}
			} else if (bottom && !leftCorner && !rightCorner) {
				// 下
				if (!settingHold || hold > settingHoldCount) {
					// 移動
					MoveScreen(Direction.Down, settingWarpDown, ref x, ref y);
				} else {
					// 移動させない
					y = screenBounds.Y2();
				}
			} else {
				hold = 0;
			}

			screenBounds = Screen.FromPoint(new Point(x, y)).Bounds;
			if (x != s.X || y != s.Y) {
				this.BeginInvoke((Action)(() => Cursor.Position = new Point(x, y)));
			}
			this.BeginInvoke((Action)(() => LabelPosition.Text = string.Format("Position = ({0,5}, {1,5})", x, y)));
		}

		private void HookKeyboard(ref KeyboardHook.StateKeyboard s)
		{
			if (s.Stroke == KeyboardHook.Stroke.KEY_DOWN ) {
				// KeyEventArgs e

				// 中央に移動

				// 選択して移動

				// 場所を表示

				// 終了


			}
		}

		enum Direction { LeftDown = 1, Down, RightDown, Left, None, Right, LeftUp, Up, RightUp }
		RangeData range = new RangeData();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="direction">移動方向</param>
		/// <param name="warp"></param>
		/// <param name="x">X座標</param>
		/// <param name="y">Y座標</param>
		/// <returns></returns>
		private void MoveScreen(Direction direction, bool warp, ref int x, ref int y)
		{
			// 移動先 Screen 探索
			if (Screen.AllScreens.Length > 1) {
				range.Clear();
				int toX, toY;
				switch (direction) {
					case Direction.LeftUp:
						break;
					case Direction.RightUp:
						break;
					case Direction.LeftDown:
						break;
					case Direction.RightDown:
						break;
					case Direction.Left:
						toX = screenBounds.X - 1;
						foreach (var s in Screen.AllScreens) {
							if (toX == s.Bounds.X2() && screenBounds.Y <= s.Bounds.Y2() && screenBounds.Y2() >= s.Bounds.Y) {
								range.AddRange(s.Bounds.Y, s.Bounds.Width);
							}
						}
						if (range.Count > 0) {
							// 移動先がある
							x = toX - settingMargin;
							y = range.Convert(y, screenBounds.Y, screenBounds.Height);
							screenBounds = Screen.FromPoint(new Point(x, y)).Bounds;
							this.BeginInvoke((Action)(() => label1.Text = string.Format("{0}", screenBounds)));
							return;
						}
						break;
					case Direction.Right:
						toX = screenBounds.X2() + 1;
						foreach (var s in Screen.AllScreens) {
							if (toX == s.Bounds.X && screenBounds.Y <= s.Bounds.Y2() && screenBounds.Y2() >= s.Bounds.Y) {
								range.AddRange(s.Bounds.Y, s.Bounds.Width);
							}
						}
						if (range.Count > 0) {
							// 移動先がある
							x = toX + settingMargin;
							y = range.Convert(y, screenBounds.Y, screenBounds.Height);
							screenBounds = Screen.FromPoint(new Point(x, y)).Bounds;
							this.BeginInvoke((Action)(() => label1.Text = string.Format("{0}", screenBounds)));
							return;
						}
						break;
					case Direction.Up:
						break;
					case Direction.Down:
						break;
				}
			}

			// 移動先が無い
			// todo: MultiDisplay
			if (warp) {
				switch (direction) {
					case Direction.LeftUp:
						// 右下へ移動
						x = screenBounds.X2() - settingMargin;
						y = screenBounds.Y2() - settingMargin;
						break;
					case Direction.RightUp:
						// 左下へ移動
						x = screenBounds.X + settingMargin;
						y = screenBounds.Y2() - settingMargin;
						break;
					case Direction.LeftDown:
						// 右上へ移動
						x = screenBounds.X2() - settingMargin;
						y = screenBounds.Y + settingMargin;
						break;
					case Direction.RightDown:
						// 左上へ移動
						x = screenBounds.X + settingMargin;
						y = screenBounds.Y + settingMargin;
						break;
					case Direction.Left:
						// 右へ移動
						x = screenBounds.X2() - settingMargin;
						break;
					case Direction.Right:
						// 左へ移動
						x = screenBounds.X + settingMargin;
						break;
					case Direction.Up:
						// 下へ移動
						y = screenBounds.Y2() - settingMargin;
						break;
					case Direction.Down:
						// 上へ移動
						y = screenBounds.Y + settingMargin;
						break;
				}
			}

		}


		private void button1_Click(object sender, EventArgs e)
		{


		}

	}
}

