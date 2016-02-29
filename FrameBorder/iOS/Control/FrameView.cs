using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using CoreAnimation;
using CoreGraphics;

namespace FrameBorder.iOS.Control
{
	public class FrameView : UIView
	{
		private WeakReference _SourceView;

		CALayer[] borderLayers = new CALayer[4];

		enum BorderPosition
		{
			Left,
			Top,
			Right,
			Bottom
		}

		public FrameView ()
		{
			this.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
			this.Layer.ShouldRasterize = true;
			this.Layer.MasksToBounds = false;
		}

		public override void Draw (CoreGraphics.CGRect rect)
		{
			base.Draw (rect);

			// set variables
			this.StrokeThickness = (float)SourceView.StrokeThickness;

			if (SourceView.AllBorders) {
				this.Layer.BorderWidth = StrokeThickness;
				this.Layer.BorderColor = SourceView.OutlineColor.ToCGColor ();
				this.Layer.CornerRadius = (nfloat)SourceView.Radius;
			} else {
				this.SetupLayer (rect.Width, rect.Height);
			}

			this.Layer.BackgroundColor = TranslateFormsColor (SourceView.BackgroundColor).CGColor;

			// check shadow
			if (SourceView.HasShadow) {
				// bottom shadow
				Layer.ShadowColor = SourceView.ShadowColor.ToCGColor ();
				Layer.ShadowOffset = new CGSize (SourceView.ShadowOffset.X, SourceView.ShadowOffset.Y);
				Layer.ShadowRadius = (nfloat)SourceView.ShadowRadius;
				Layer.ShadowOpacity = SourceView.ShadowOpacity;
			}
		}

		private nfloat StrokeThickness { get; set; }

		public MyFrame SourceView {
			get {
				return this._SourceView.Target as MyFrame;
			}
			set {
				this._SourceView = new WeakReference (value);
			}
		}

		void UpdateBorderLayer(BorderPosition borderPosition, nfloat thickness, nfloat width, nfloat height)
		{
			var borderLayer = borderLayers[(int)borderPosition];
			if (thickness <= 0)
			{
				if (borderLayer != null)
				{
					borderLayer.RemoveFromSuperLayer();
					borderLayers[(int)borderPosition] = null;
				}
			}
			else
			{
				if (borderLayer == null)
				{
					borderLayer = new CALayer();
					Layer.AddSublayer(borderLayer);
					borderLayers[(int)borderPosition] = borderLayer;
				}

				switch (borderPosition)
				{
					case BorderPosition.Left:
						borderLayer.Frame = new CGRect(0, 0, thickness, height);
						break;
					case BorderPosition.Top:
						borderLayer.Frame = new CGRect(0, 0, width, thickness);
						break;
					case BorderPosition.Right:
						borderLayer.Frame = new CGRect(width - thickness, 0, thickness, height);
						break;
					case BorderPosition.Bottom:
						borderLayer.Frame = new CGRect(0, height - thickness, width, thickness);
						break;
				}
				borderLayer.BackgroundColor = TranslateFormsColor (SourceView.OutlineColor).CGColor;
				borderLayer.CornerRadius = (nfloat)SourceView.Radius;
			}
		}

		private void SetupLayer(nfloat width, nfloat height)
		{
			if (SourceView == null || SourceView.Width <= 0 || SourceView.Height <= 0)
			{
				return;
			}


			Layer.CornerRadius = (float)SourceView.Radius;
			Layer.BackgroundColor = TranslateFormsColor (SourceView.BackgroundColor).CGColor;

			if (SourceView.Borders.Left >= 1) {
				UpdateBorderLayer (BorderPosition.Left, SourceView.StrokeThickness, width, height);
			}
			if (SourceView.Borders.Top >= 1) {				
				UpdateBorderLayer (BorderPosition.Top, SourceView.StrokeThickness, width, height);
			}
			if (SourceView.Borders.Right >= 1) {
				UpdateBorderLayer (BorderPosition.Right, SourceView.StrokeThickness, width, height);
			}
			if (SourceView.Borders.Bottom >= 1) {
				UpdateBorderLayer (BorderPosition.Bottom, SourceView.StrokeThickness, width, height);
			}
		}

		protected internal UIColor TranslateFormsColor(Xamarin.Forms.Color color)
		{
			if (color == Xamarin.Forms.Color.Transparent) {
				return UIColor.Clear;
			}
			else if (color!= Xamarin.Forms.Color.Default)
			{
				return color.ToUIColor();
			}
			else
			{
				return UIColor.White;
			}
		}
	}
}

