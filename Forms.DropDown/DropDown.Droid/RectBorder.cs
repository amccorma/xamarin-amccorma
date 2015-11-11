using System;

namespace DropDown.Droid
{
	public class RectBorder : global::Android.Graphics.Drawables.Drawable
	{
		private global::Android.Graphics.Paint _paint;
		private global::Android.Graphics.Rect _rect;
		private global::Android.Graphics.Color _backcolor;

		public RectBorder (Int32 width, global::Android.Graphics.Color BorderColor, global::Android.Graphics.Color backcolor)
		{
			this._backcolor = backcolor;

			// border color
			this._paint = new global::Android.Graphics.Paint();
			this._paint.Color = BorderColor;
			this._paint.StrokeWidth = width;
			this._paint.SetStyle (global::Android.Graphics.Paint.Style.Stroke);
		}

		protected override void OnBoundsChange (global::Android.Graphics.Rect bounds)
		{
			this._rect = bounds;
		}

		public override void Draw (global::Android.Graphics.Canvas canvas)
		{	
			// fill color
			var paint2 = new global::Android.Graphics.Paint();
			paint2.Color = this._backcolor;
			paint2.SetStyle (global::Android.Graphics.Paint.Style.Fill);
			canvas.DrawRect (this._rect, paint2);

			canvas.DrawRect (this._rect, this._paint);

		}

		public override void SetAlpha (int alpha)
		{
			
		}

		public override void SetColorFilter (global::Android.Graphics.ColorFilter colorFilter)
		{
			
		}

		public override int Opacity {
			get {
				return 0;
			}
		}
	}
}

