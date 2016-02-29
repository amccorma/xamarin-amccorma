using System;

using Xamarin.Forms;

namespace FrameBorder
{
	public enum StrokeType{
		Solid,

		Dotted,

		Dashed
	}

	public class FrameRect
	{
		public FrameRect() 
		{
			Top = Left = Right = Bottom = 0;
		}

		/// <summary>
		/// ShadowRect. Android Only
		/// </summary>
		/// <param name="l">Left Size</param>
		/// <param name="t">Top Size</param>
		/// <param name="r">Right Size.</param>
		/// <param name="b">Bottom Size</param>
		public FrameRect(Int32 l, Int32 t, Int32 r, Int32 b)
		{
			Top = t;
			Left = l;
			Right = r;
			Bottom = b;
		}

		/// <summary>
		/// ShadowRect. Android Only
		/// </summary>
		/// <param name="h">Hortizonal Distance (sets left and Right to same values)</param>
		/// <param name="v">Vertical Distance (sets left and Right to same values)</param>
		public FrameRect(Int32 h, Int32 v)
		{
			Left = v;
			Right = v;
			Top = h;
			Bottom = h;
		}

		public Int32 Top { get; set; }

		public Int32 Bottom { get; set; }

		public Int32 Left { get; set; }

		public Int32 Right { get; set; }
	}


	/// <summary>
	/// Frame Implementation
	/// <remarks>
	/// Android:  	Create a Shadow effect by setting the background Color, border and Stroke properties.
	/// 			A dropdown shadow:  border.Bottom=1, all other false. StrokeThickness: 3, OutlineColor=shadow color
	/// 			Each border can be turned on and off.  Border Off=0, All other values on
	/// </remarks>
	/// </summary>
	public class MyFrame : Frame
	{
		public MyFrame ()
		{
			this.OutlineColor = Color.Transparent;
			this.HasShadow = false;
			StrokeType = StrokeType.Solid;

			// Borders on.  Turn off set 0
			Borders = new FrameRect (1, 1, 1, 1);

			//this.Padding = new Thickness (0, 0);
			ShadowOpacity = 0f;
		}
			
		/// <summary>
		/// The stroke type property.
		/// </summary>
		public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create("StrokeType", typeof(Int32), 
			typeof(MyFrame), 1, BindingMode.OneWay, null, null, null, null);

//		/// <summary>
//		/// <para>Android/iOS Property</para>
//		/// <para>Android: left, top, right, bottom</para>
//		/// <para>iOS: Left, Bottom, Ignores other values</para>
//		/// </summary>
//		public static new BindableProperty PaddingProperty = BindableProperty.Create("Padding", typeof(Thickness), 
//			typeof(MyFrame), new Thickness(0,0), BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The stroke type property.
		/// </summary>
		public static readonly BindableProperty StrokeTypeProperty = BindableProperty.Create("StrokeType", typeof(StrokeType), 
			typeof(MyFrame), StrokeType.Solid, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// <para>Android/iOS Property</para>
		/// <para>Android: Border/Shadow Radius</para>
		/// <para>The Corner radius property.</para>
		/// </summary>
		public static readonly BindableProperty RadiusProperty = 
			BindableProperty.Create<MyFrame, float>(p => p.Radius, 5.0f);

		/// <summary>
		/// <para>Android/iOS Property</para>
		/// <para>Android, iOS Borders. Each Side (L,T,B,R) is either 0 or 1<para>
		/// <para>0 off, 1 on </para>
		/// <para>default all borders on</para>
		/// </summary>
		/// <value>The borders.</value>
		public FrameRect Borders { get; set; }

		/// <summary>
		/// <para>Android Property</para>
		/// <para>Android. Turn on/off shsadow. Each Side (L,T,B,R) is either 0 or 1<para>
		/// <para>0 off, 1 on </para>
		/// <para>HasShadow must be set to true</para>
		/// </summary>
		/// <value>The borders.</value>
		public FrameRect ShadowBorders { get; set; }

		/// <summary>
		/// <para>iOS property</para>
		/// <para>iOS Shadow offset (x, y)</para>
		/// <para>trail and error values</para>
		/// <para>HasShadow need to be set to true</para>
		/// </summary>
		/// <value>The iOS shadow offset.</value>
		public Point ShadowOffset { get; set; }

		/// <summary>
		/// <para>iOS property</para>
		/// <para>iOS Shadow Opacity</para>
		/// <para>The value in this property must be in the range 0.0 (transparent) to 1.0 (opaque)</para>
		/// <para>HasShadow need to be set to true</para>
		/// </summary>
		/// <value>The iOS shadow Opacity.</value>
		public float ShadowOpacity { get; set; }

		/// <summary>
		/// <para>Android/iOS Property</para>
		/// <para>iOS property</para>
		/// <para>Drop Shadow Color. HasShadow needs be true</para>
		/// </summary>
		/// <value>Shadow Color (iOS only).</value>
		public Xamarin.Forms.Color ShadowColor  { get; set; }

		/// <summary>
		/// <para>Android, iOS property</para>
		/// <para>Shadow Radius</para>
		/// </summary>
		/// <value>Shadow Radius</value>
		public Int32 ShadowRadius { get; set; }

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
		/// </summary>
		public new Thickness Padding
		{
			get {
				return (Thickness)base.GetValue(PaddingProperty);
			}
			set {
				base.SetValue(PaddingProperty, value);
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

