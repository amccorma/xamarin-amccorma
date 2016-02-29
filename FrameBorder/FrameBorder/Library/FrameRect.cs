using System;
using Xamarin.Forms;

namespace FrameBorder
{
	[TypeConverter(typeof(FrameRectConverter))]
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
}

