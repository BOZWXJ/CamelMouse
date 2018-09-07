using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tomochan154.Configuration;

namespace CamelMouse.Config
{
	[Flags]
	public enum Modifiers { None, LShift = 0x1, RShift = 0x2, Shift = LShift | RShift, LControl = 0x4, RControl = 0x8, Control = LControl | RControl }

	[PortableSettingsPath(DirectoryName = @".\conf\", FileName = "Sample.conf")]
	[DataContract]
	public class AppSettings : PortableSettingsBase<AppSettings>
	{
		public static readonly AppSettings Instance = Load();

		protected override void OnPropertyChanging(PropertyChangingEventArgs args)
		{
			base.OnPropertyChanging(args);
		}

		protected override void OnLoaded(EventArgs args)
		{
			WarpLeft = true;
			WarpRight = true;
			WarpUp = true;
			WarpDown = true;
			Margin = 5;
			Hold = false;
			HoldCount = 20;
			CornerDisable = true;
			CornerSize = 20;
			HotKey = Keys.None;
			MoveToCenterKey = Keys.Q;
			MoveToCenterKeyModifiers = Modifiers.Control;
			base.OnLoaded(args);
		}

		/// <summary>
		/// ワープさせる 左端
		/// </summary>
		[DataMember]
		public bool WarpLeft
		{
			get { return Get(a => a.WarpLeft); }
			set { Set(a => a.WarpLeft, value); }
		}
		/// <summary>
		/// ワープさせる 右端
		/// </summary>
		[DataMember]
		public bool WarpRight
		{
			get { return Get(a => a.WarpRight); }
			set { Set(a => a.WarpRight, value); }
		}
		/// <summary>
		/// ワープさせる 上端
		/// </summary>
		[DataMember]
		public bool WarpUp
		{
			get { return Get(a => a.WarpUp); }
			set { Set(a => a.WarpUp, value); }
		}
		/// <summary>
		/// ワープさせる 下端
		/// </summary>
		[DataMember]
		public bool WarpDown
		{
			get { return Get(a => a.WarpDown); }
			set { Set(a => a.WarpDown, value); }
		}

		/// <summary>
		/// 移動先を内側にする
		/// </summary>
		[DataMember]
		public int Margin
		{
			get { return Get(a => a.Margin); }
			set { Set(a => a.Margin, value); }
		}

		/// <summary>
		/// 引っかかり
		/// </summary>
		[DataMember]
		public bool Hold
		{
			get { return Get(a => a.Hold); }
			set { Set(a => a.Hold, value); }
		}

		/// <summary>
		/// 引っかかり量
		/// </summary>
		[DataMember]
		public int HoldCount
		{
			get { return Get(a => a.HoldCount); }
			set { Set(a => a.HoldCount, value); }
		}

		/// <summary>
		/// 四隅は移動させない
		/// </summary>
		[DataMember]
		public bool CornerDisable
		{
			get { return Get(a => a.CornerDisable); }
			set { Set(a => a.CornerDisable, value); }
		}

		/// <summary>
		/// 四隅のサイズ
		/// </summary>
		[DataMember]
		public int CornerSize
		{
			get { return Get(a => a.CornerSize); }
			set { Set(a => a.CornerSize, value); }
		}

		/// <summary>
		/// 押している時のみ移動
		/// </summary>
		[DataMember]
		public Keys HotKey
		{
			get { return Get(a => a.HotKey); }
			set { Set(a => a.HotKey, value); }
		}

		/// <summary>
		/// 中央に移動
		/// </summary>
		[DataMember]
		public Keys MoveToCenterKey
		{
			get { return Get(a => a.MoveToCenterKey); }
			set { Set(a => a.MoveToCenterKey, value); }
		}
		public Modifiers MoveToCenterKeyModifiers
		{
			get { return Get(a => a.MoveToCenterKeyModifiers); }
			set { Set(a => a.MoveToCenterKeyModifiers, value); }
		}

		// 選択して移動

		// 場所を表示

		// 終了


	}
}
