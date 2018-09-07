using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Tomochan154.Configuration;

namespace CamelMouse.Config
{
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
			WarpLeft = false;
			WarpRight = false;
			WarpUp = false;
			WarpDown = false;
			Margin = 5;
			Hold = false;
			HoldCount = 20;
			CornerDisable = true;
			CornerSize = 20;
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
	}
}
