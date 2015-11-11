using System;
using UIKit;
using CoreGraphics;
using CoreAnimation;
using System.Collections.Generic;
using Foundation;
using System.Linq;
using DropDown.iOS.Control.Table;

namespace DropDown.iOS.Control
{
	public class DropDownViewArgs
	{
		public DropDownViewArgs() 
		{
		}

		public bool IsShowing { get; set; }

		public CGRect Frame { get; set; }
	}
	
	public class DropDownView : UIView
	{
		public event EventHandler<DropDownViewArgs> OnChanged;

		private DropDownControl _DropDownControl;
		private DropDownTable _TblView;

		private UIView _MainView;
		private nfloat _PopupHeight;
		private bool _IsXamarinForms = false;
		private CGRect _LastPopupFrame;
		private CGRect _FormsFrame;
		public Action<string> SelectedText;
		private string _Title;
		protected internal nfloat _CellHeight;
		private string _SelectedText;
		private bool _IsShowing;

		public DropDownView (string Title, IList<string> data, nfloat fSize) : 
		this (Title, data, fSize, 0, 0, null, null)
		{

		}

		public DropDownView (string Title, IList<string> data, nfloat fSize, nfloat cellHeight) : 
		this (Title, data, fSize, cellHeight, 0, null, null)
		{

		}

		public DropDownView (string Title, IList<string> data, nfloat fSize, nfloat cellHeight, UIColor cellSelectedBackgroundColor, UIColor cellSelectedTextColor) :
		this(Title, data, fSize, cellHeight, 0, cellSelectedBackgroundColor, cellSelectedTextColor)
		{

		}

		public DropDownView (string Title, IList<string> data, nfloat fSize, nfloat cellHeight, float layoutHeight) :
		this(Title, data, fSize, cellHeight, layoutHeight, null, null)
		{

		}

		public DropDownView (string Title, IList<string> data, nfloat fSize, 
			nfloat cellHeight, float layoutHeight, UIColor cellSelectedBackgroundColor, UIColor cellSelectedTextColor)
		{
			NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, OrientationChanged);
			this.Title = Title;
			this._CellHeight = cellHeight;
			this.IsShowing = false;

			this.ControlHeight = layoutHeight;
			this._DropDownControl = new DropDownControl (this, fSize);

			this._TblView = new DropDownTable (data, cellHeight, fSize, cellSelectedBackgroundColor, cellSelectedTextColor);

			// DropDown PopupView
			this._MainView = new UIView ();
			this._MainView.Layer.MasksToBounds = false;
			this._MainView.Layer.BorderColor = UIColor.Black.CGColor;
			this._MainView.Layer.BorderWidth = 1;

			_MainView.UserInteractionEnabled = true;
			this._MainView.UserInteractionEnabled = true;
			this.MultipleTouchEnabled = true;
			this._TblView.MultipleTouchEnabled = true;

			// add DropDownControl
			Add(this._DropDownControl);

			if (this._IsXamarinForms == false) {
				this.Add (this._MainView);
			}

			_MainView.Add (_TblView);
			_MainView.Hidden = true;



			// handle selected Text
			this._TblView.SelectedText = (x) => {
				this._SelectedText = x;
				this._DropDownControl.SelectedText(x);
				if (SelectedText != null)
				{
					SelectedText(x);
				}
				HideDropDown();
			};
		}

		public void FireChanged()
		{
			var handle = OnChanged;
			if (handle != null) {
				handle (this, new DropDownViewArgs { IsShowing = this.IsShowing, Frame = this.ViewFrame });
			}
		}

		public bool IsShowing {
			get { return this._IsShowing; }
			set
			{
				this._IsShowing = value;
				FireChanged();
			}
		}

		public nfloat PopupHeight {
			get {
				return this._PopupHeight;
			}
			set {
				this._PopupHeight = value;
			}
		}

		public nfloat ControlHeight {
			get;
			set;
		}

		public string Title {
			get { 
				return _Title;
			}
			set
			{
				_Title = value;
				if (this._DropDownControl != null) {
					this._DropDownControl.SetTitle (value);
				}
			}
		}

		/// <summary>
		/// If using Xamarin Forms, set this value to the Rect of the control
		/// </summary>
		/// <value>The layout view.</value>
		public CGRect FormsFrame {
			get { return this._FormsFrame; }
			set {
				this._IsXamarinForms = true;
				this._FormsFrame= value;
				FireChanged();
			}
		}

		public void UpdateSource(IList<string> data)
		{
			if (this._TblView != null && data != null) {
				if (this.IsShowing) {
					HideDropDown ();
				}
				var text = this._SelectedText;
				this._TblView.ReloadSource (data);

				var idx = data.IndexOf (text);
				if (idx >= 0) {
					this._TblView.SelectRow (NSIndexPath.FromRowSection(idx, 0), false, UITableViewScrollPosition.Middle);
					this._DropDownControl.SetTitle (text);
					// set title
				} else {
					// set title to default title
					this.Title = this.Title;
				}
			}
		}

		private void OrientationChanged(NSNotification notification)
		{
			if (this._MainView != null && this._MainView.Hidden == false) {
				var y = this._LastPopupFrame.Y + this._LastPopupFrame.Height;
				if (this._IsXamarinForms) {
					y = y + TopBarSize ();
				}
				this._MainView.Frame = new CGRect (this._LastPopupFrame.X, y, this._LastPopupFrame.Width, PopupHeight);
			}
		}

		protected override void Dispose (bool disposing)
		{
			this._MainView.RemoveFromSuperview ();
			this._DropDownControl.RemoveFromSuperview ();
			this._TblView.RemoveFromSuperview ();

			this._TblView.Dispose ();

			this._DropDownControl.Dispose ();
			base.Dispose (disposing);

			NSNotificationCenter.DefaultCenter.RemoveObserver(UIDevice.OrientationDidChangeNotification);
		}


		protected internal void ShowDropDownHelper()
		{
			if (this._TblView.HasData()) {
				CoreGraphics.CGRect frame;
				var forms = this.FormsFrame;
				// check if using Forms to show
				if (this._IsXamarinForms) {
					frame = new CGRect (forms.X, forms.Y, forms.Width, forms.Height);
				} else {
					frame = this._DropDownControl.Frame;
				}
				frame.Width = this._DropDownControl.Frame.Width;
				ShowDropDown (frame);
			}
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);
			var width = rect.Width;

			// draw control
			this._DropDownControl.Draw(rect);
		}

		public CGRect ViewFrame
		{
			get {
				var f = this._DropDownControl.Frame;
				if (this._IsXamarinForms) {
					f = this.FormsFrame;
					var t = this.TopBarSize ();
					f.Y = f.Y + t;
				}
				var f2 = this._MainView == null ? new CGRect(0,0,0,0) : this._MainView.Frame;
				var w = (f.Width > f2.Width) ? f.Width : f2.Width;
				var h = f.Height + f2.Height;
				var frame = new CGRect (f.X, f.Y, w, h);
				return frame;
			}
		}

		private void ShowDropDown (CGRect frame)
		{
			if (this.IsShowing == false) {
				this.IsShowing = true;

				this._LastPopupFrame = frame;
				//this.Frame = new CGRect (senderFrame.X, senderFrame.Y + 200, senderFrame.Width, senderFrame.Height + 200);
				var f = this.Frame;

				if (this._IsXamarinForms == false) {
					this.Frame = new CGRect (f.X, f.Y, this._LastPopupFrame.Width, this.PopupHeight);
				} else if (this._MainView.Hidden) {
					var w = UIApplication.SharedApplication.KeyWindow;
					var v = w.RootViewController.View;
					v.Add (this._MainView);
					v.MultipleTouchEnabled = true;
				}

				var y = this._LastPopupFrame.Y + this._LastPopupFrame.Height;
				if (this._IsXamarinForms) {
					y = y + TopBarSize ();
				}
				this._MainView.Frame = new CGRect (this._LastPopupFrame.X, y, this._LastPopupFrame.Width, this.PopupHeight);

				_TblView.Frame = new CGRect (0, 0, this._LastPopupFrame.Width, this.PopupHeight);
				_MainView.Hidden = false;

			} else {
				HideDropDown ();
			}

			FireChanged();
		}

		private Int32 TopBarSize()
		{
			var s = 0;
			var h = 0;

			// find Navigationbar. could be null if no navigationbar.
			// xamarin support hack
			var hasNav = UIApplication.SharedApplication.KeyWindow.Subviews.FirstOrDefaultFromMany (item => item.Subviews, x => x is UINavigationBar);

			if (UIApplication.SharedApplication.StatusBarHidden == false) {
				s = Convert.ToInt32 (UIApplication.SharedApplication.StatusBarFrame.Height);
			}

			if (hasNav != null) {
				h = Convert.ToInt32 (hasNav.Frame.Height);
			}

			return s + h;
		}

		public void HideDropDown()
		{
			this.IsShowing = false;
			if (this._MainView != null) {
				this._MainView.Hidden = true;
			}
			var f = this.Frame;
			f.Height = this.ControlHeight;
			this.Frame = f;

			if (this._IsXamarinForms) {
				
				this._MainView.RemoveFromSuperview ();
			}
		}
	}
}

