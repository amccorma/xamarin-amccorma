using System;
using Xamarin.Forms;

namespace FrameBorder
{
	public class MyFrame : Frame
	{
		public MyFrame ()
		{
			//			this.OutlineColor = Color.Transparent;
			//			this.HasShadow = false;
		}

		/// <summary>
		/// The stroke type property.
		/// </summary>
		public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create("StrokeThickness", typeof(Int32), 
			typeof(MyFrame), 1, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The stroke type property.
		/// </summary>
		public static readonly BindableProperty StrokeTypeProperty =
			BindableProperty.Create ("StrokeType", typeof(StrokeType), typeof(MyFrame), StrokeType.Solid);
		
		/// <summary>
		/// <para>Android/iOS Property</para>
		/// <para>Android: Border/Shadow Radius</para>
		/// <para>The Corner radius property.</para>
		/// </summary>
		public static readonly BindableProperty RadiusProperty =
			BindableProperty.Create ("Radius", typeof(float), typeof(MyFrame), 5.0f);


		/// <summary>
		/// <para>Android/iOS Property</para>
		/// </summary>
		public static readonly BindableProperty BordersProperty =
			BindableProperty.Create ("Borders", typeof(FrameRect), typeof(MyFrame), new FrameRect(1,1,1,1));		

		/// <summary>
		/// <para>Android Property</para>
		/// </summary>
		public static readonly BindableProperty ShadowBordersProperty =
			BindableProperty.Create ("ShadowBorders", typeof(FrameRect), typeof(MyFrame), null);

		/// <summary>
		/// <para>iOS property</para>
		/// </summary>
		public static readonly BindableProperty ShadowOffsetProperty =
			BindableProperty.Create ("ShadowOffset", typeof(Point), typeof(MyFrame), new Point(0,0));
		
		/// <summary>
		/// <para>iOS property</para>
		/// </summary>
		public static readonly BindableProperty ShadowOpacityProperty =
			BindableProperty.Create ("ShadowOpacity", typeof(float), typeof(MyFrame), 0f);

		/// <summary>
		/// <para>Android/iOS Property</para>
		/// </summary>
		public static readonly BindableProperty ShadowColorProperty =
			BindableProperty.Create ("ShadowColor", typeof(Xamarin.Forms.Color), typeof(MyFrame), Xamarin.Forms.Color.Default);
		
		/// <summary>
		/// <para>Android, iOS property</para>
		/// <para>iOS: Shadow Radius (float)</para>
		/// <para>Android: Shadow Thickness (integer)</para>
		/// </summary>
		public static readonly BindableProperty ShadowRadiusProperty =
			BindableProperty.Create ("ShadowRadius", typeof(float), typeof(MyFrame), 0f);

		/// <summary>
		/// <para>Android/iOS Property</para>
		/// <para>Android, iOS Borders. Each Side (L,T,B,R) is either 0 or 1<para>
		/// <para>0 off, 1 on </para>
		/// <para>default all borders on</para>
		/// </summary>
		/// <value>The borders.</value>
		public FrameRect Borders
		{
			get {
				return (FrameRect)base.GetValue(BordersProperty);
			}
			set {
				base.SetValue(BordersProperty, value);
			}
		}

		/// <summary>
		/// <para>Android Property</para>
		/// <para>Android. Turn on/off shsadow. Each Side (L,T,B,R) is either 0 or 1<para>
		/// <para>0 off, 1 on </para>
		/// <para>HasShadow must be set to true</para>
		/// </summary>
		/// <value>The borders.</value>
		public FrameRect ShadowBorders
		{
			get {
				return (FrameRect)base.GetValue(ShadowBordersProperty);
			}
			set {
				base.SetValue(ShadowBordersProperty, value);
			}
		}

		/// <summary>
		/// <para>iOS property</para>
		/// <para>iOS Shadow offset (x, y)</para>
		/// <para>trail and error values</para>
		/// <para>HasShadow need to be set to true</para>
		/// </summary>
		/// <value>The iOS shadow offset.</value>
		public Point ShadowOffset
		{
			get {
				return (Point)base.GetValue(ShadowOffsetProperty);
			}
			set {
				base.SetValue(ShadowOffsetProperty, value);
			}
		}

		/// <summary>
		/// <para>iOS property</para>
		/// <para>iOS Shadow Opacity</para>
		/// <para>The value in this property must be in the range 0.0 (transparent) to 1.0 (opaque)</para>
		/// <para>HasShadow need to be set to true</para>
		/// </summary>
		/// <value>The iOS shadow Opacity.</value>
		public float ShadowOpacity
		{
			get {
				return (float)base.GetValue(ShadowOpacityProperty);
			}
			set {
				base.SetValue(ShadowOpacityProperty, value);
			}
		}

		/// <summary>
		/// <para>Android/iOS Property</para>
		/// <para>iOS property</para>
		/// <para>Drop Shadow Color. HasShadow needs be true</para>
		/// </summary>
		/// <value>Shadow Color (iOS only).</value>
		public Xamarin.Forms.Color ShadowColor
		{
			get {
				return (Xamarin.Forms.Color)base.GetValue(ShadowColorProperty);
			}
			set {
				base.SetValue(ShadowColorProperty, value);
			}
		}

		/// <summary>
		/// <para>Android, iOS property</para>
		/// <para>iOS: Shadow Radius (float)</para>
		/// <para>Android: Shadow Thickness (integer)</para>
		/// </summary>
		/// <value>Shadow Radius</value>
		public float ShadowRadius
		{
			get {
				return (float)base.GetValue(ShadowRadiusProperty);
			}
			set {
				base.SetValue(ShadowRadiusProperty, value);
			}
		}

		public bool AllBorders {
			get {
				if (Borders.Bottom >= 1 && Borders.Right >= 1 && Borders.Left >= 1 && Borders.Right >= 1) {
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// <para>Android/iOS Property</para>
		/// <para>Android: Border, Shadow Radius</para>
		/// <para>iOS: Border Radius, use ShadowRadius for Shadow</para>
		/// </summary>
		/// <value>The radius.</value>
		public float Radius {
			get {
				return (float)base.GetValue(RadiusProperty);
			}
			set {
				base.SetValue(RadiusProperty, value);
			}
		}

		/// <summary>
		/// <para>Android Only</para>
		/// <para>Line Type</para>
		/// </summary>
		/// <value>The type of the stroke.</value>
		public StrokeType StrokeType {
			get {
				return (StrokeType)base.GetValue(StrokeTypeProperty);
			}
			set {

				base.SetValue(StrokeTypeProperty, value);
			}
		}

		/// <summary>
		/// size of the border
		/// </summary>
		/// <value>The stroke thickness.</value>
		public Int32 StrokeThickness {
			get {
				return (Int32)base.GetValue(StrokeThicknessProperty);
			}
			set {
				base.SetValue(StrokeThicknessProperty, value);
			}
		}
	}
}

