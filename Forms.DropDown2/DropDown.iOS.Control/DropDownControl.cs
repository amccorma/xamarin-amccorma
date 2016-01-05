using System;
using UIKit;
using CoreGraphics;

namespace DropDown.iOS.Control
{
	public class DropDownControl : UIView
	{
		private UIView _DropDownView;
		private UIButton _Button1;
		private WeakReference _Parent;
		protected internal Action<string> SelectedText;

		private DropDownView Parent
		{
			get {
				return this._Parent.Target as DropDownView;
			}
			set {
				this._Parent = new WeakReference (value);
			}
		}

		//private UIView MainRect;

		public DropDownControl(DropDownView view, nfloat fSize) : base()
		{
			this.Parent = view;
			UserInteractionEnabled = true;
			MultipleTouchEnabled = true;

			this._Button1 = new UIButton ();
			this._Button1.SetTitle (view.Title, UIControlState.Normal);
			this._Button1.SetTitleColor(UIColor.Black, UIControlState.Normal);
			this._Button1.BackgroundColor = UIColor.White;
			this._Button1.UserInteractionEnabled = true;
			this._Button1.MultipleTouchEnabled = true;

			if (fSize > 1) {
				this._Button1.Font = UIFont.SystemFontOfSize (fSize);
			}

			this._DropDownView = new UIView ();
			this._DropDownView.Layer.MasksToBounds = false;
			//			this._DropDownView.Layer.BorderColor = UIColor.Clear.CGColor;
			//			this._DropDownView.Layer.CornerRadius = 4;
			//			this._DropDownView.Layer.BorderWidth = 1;
			//			this._DropDownView.AddBorder (UIRectEdge.Left, UIColor.Black, 1);

			this._DropDownView.BackgroundColor = UIColor.Clear;
			this._DropDownView.Layer.BackgroundColor = UIColor.Clear.CGColor; // = UIColor.FromRGB (0, 175, 63).CGColor;

		

			this._Button1.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			this._Button1.TitleEdgeInsets = new UIEdgeInsets (0, 10, 0, 0);
			this._Button1.MultipleTouchEnabled = true;

			// add to view
			//this.Add (this._Button1);
			//this.Add (this._DropDownView);

			//MainRect = new UIView ();
			this.AddSubview (this._Button1);


			var top = UIApplication.SharedApplication.KeyWindow.RootViewController;
//			var v = top.View;
//			v.MultipleTouchEnabled = true;

			top.Add (_DropDownView);

			//this.AddSubview (this._DropDownView);
			//this.MainRect.BackgroundColor = UIColor.Yellow;

			this._Button1.TouchDown += Button1Click;

			// handle selection
			SelectedText = (x) => {
				SetTitle(x);
			};
//
//			this.BackgroundColor = UIColor.Clear;
			this.Layer.BorderColor = UIColor.Blue.CGColor;
			this.Layer.BackgroundColor = UIColor.Green.CGColor;
			this.Layer.BorderWidth = 1;
			this.Layer.CornerRadius = 4;
		}

		public DropDownControl (DropDownView view) : this (view, 0)
		{

		}

		protected internal void SetTitle(string Title)
		{
			InvokeOnMainThread (() => {
				this._Button1.SetTitle (Title, UIControlState.Normal);
			});
		}

		private void Button1Click(object sender, EventArgs e)
		{
			var t = this.Subviews;
			Parent.ShowDropDownHelper ();
		}

		/// <summary>
		/// release memory
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose (bool disposing)
		{
			this._Button1.TouchDown -= Button1Click;
			this._Button1.RemoveFromSuperview ();
			this._DropDownView.RemoveFromSuperview ();

			base.Dispose (disposing);
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);
			var width = rect.Width;
			this.Frame = rect;

			//this.MainRect.Frame = new CoreGraphics.CGRect (rect.X, rect.Y, width, rect.Height);
			this._Button1.Frame = new CoreGraphics.CGRect (rect.X, rect.Y, width, rect.Height);
//			this._Button1.AddBorder(UIRectEdge.Top, Parent.BorderColor, 1);
//			this._Button1.AddBorder(UIRectEdge.Left, Parent.BorderColor, 1, 3);
//			this._Button1.AddBorder(UIRectEdge.Right, UIColor.Black, 1);
//
			//this._DropDownView.Frame = new CGRect (width - 25, rect.Y, 25, rect.Height);

		}


		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			var p = touches.AnyObject as UITouch;
			if (p != null) {
				var point = p.LocationInView (this);
				if (point.Y <= Parent.ControlHeight && point.X >= this._DropDownView.Frame.X)
				{
					Parent.ShowDropDownHelper ();
				}
			}

			base.TouchesBegan (touches, evt);
		}
	}
}

