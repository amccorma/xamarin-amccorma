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

	public class DropDownConstants
	{
		public UIColor HudBackgroundColour 
		{
			get
			{
				return UIColor.FromWhiteAlpha (0.0f, 0.8f);
			}
		}

		/// <summary>
		/// window padding. left, right, bottom, top
		/// </summary>
		public float WindowPadding = 10;

		public int HeaderHeight {
			get;
			set;
		}

		public float HeaderFontHeight {
			get;
			set;
		}

		public int BorderWidth = 1;
	}

	public class DropDownViewHeader : UIView
	{
		private UILabel _headerLabel;
		private UIButton _ButtonClose;
		private WeakReference _Parent;


		public DropDownView Parent
		{
			get {
				return this._Parent.Target as DropDownView;
			}
			set {
				this._Parent = new WeakReference (value);
			}
		}

		public DropDownViewHeader(DropDownView view, string title)
		{
			// set the parent
			this.Parent = view;

			this._headerLabel = new UILabel ();
			this._headerLabel.Text = title;
			this._headerLabel.TextColor = UIColor.White;


			this._ButtonClose = new UIButton ();
			this._ButtonClose.SetTitle ("Close", UIControlState.Normal);
			this._ButtonClose.BackgroundColor = UIColor.Red;
			this._ButtonClose.SetTitleColor (UIColor.White, UIControlState.Normal);

			this.Add (this._headerLabel);
			this.Add (this._ButtonClose);
			// R (0) G (70) B (172)
			this.BackgroundColor = UIColor.FromRGB(0, 70, 172);
			this.Frame = new CGRect (0, 0, 200, 20);

			this._ButtonClose.TouchDown += ButtonClose_TouchDown;
		}

		void ButtonClose_TouchDown (object sender, EventArgs e)
		{
			Parent.HideDropDown ();
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			Parent.HideDropDown ();
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

			this._ButtonClose.Font = UIFont.SystemFontOfSize (Parent._Values.HeaderFontHeight);
			this._headerLabel.Font = UIFont.SystemFontOfSize (Parent._Values.HeaderFontHeight);

			var screen = UIScreen.MainScreen.Bounds;
			var padding = Parent._Values.WindowPadding * 2;
			var buttonWidth = 60;
			var headerHeight = Parent._Values.HeaderHeight;
			var labelIndent = 4;

			this.Frame = new CGRect (0, 0, screen.Width - padding, headerHeight);
			this._headerLabel.Frame = new CGRect (labelIndent, 0, screen.Width - buttonWidth - (padding * 2), headerHeight);
			this._ButtonClose.Frame = new CGRect (screen.Width - buttonWidth - padding, 0, buttonWidth, headerHeight);

			this.AddBorder (UIRectEdge.Bottom, UIColor.Black, Parent._Values.BorderWidth);
			this._ButtonClose.AddBorder (UIRectEdge.Left, UIColor.Black, Parent._Values.BorderWidth);
		}

		protected override void Dispose (bool disposing)
		{
			this._ButtonClose.TouchDown -= ButtonClose_TouchDown;
			base.Dispose (disposing);
		}
	}
	
	public class DropDownView : UIView
	{
		public event EventHandler<DropDownViewArgs> OnChanged;

		private DropDownControl _DropDownControl;
		private DropDownTable _TblView;
		private UIView _MainView;
		private UIView _OverLayView;
		private UIView _HeaderView;
		private nfloat _PopupHeight;
		private bool _IsXamarinForms = false;
		private CGRect _LastPopupFrame;
		private CGRect _FormsFrame;
		private  string _Title;
		private string _SelectedText;
		private bool _IsShowing;
		private UITapGestureRecognizer _Tap;

		protected internal nfloat _CellHeight;

		protected internal DropDownConstants _Values;

		public Action<string> SelectedText; 

		public DropDownView (string Title, IList<string> data, nfloat fSize, 
			string iOSTitle = "",
			string SelectedText = "") : 
		this (Title, data, fSize, 0, 0, null, null, UIColor.Black)
		{

		}

		public DropDownView (string Title, IList<string> data, nfloat fSize, nint cellHeight, UIColor borderColor, 
			string iOSTitle = "",
			string SelectedText = "") : 
		this (Title, data, fSize, cellHeight, 0, null, null, borderColor, iOSTitle, SelectedText)
		{

		}

		public DropDownView (string Title, IList<string> data, nfloat fSize, nint cellHeight, UIColor cellSelectedBackgroundColor, 
			UIColor cellSelectedTextColor, UIColor borderColor, 
			string iOSTitle = "",
			string SelectedText = "") :
		this(Title, data, fSize, cellHeight, 0, cellSelectedBackgroundColor, cellSelectedTextColor, borderColor, iOSTitle, SelectedText)
		{

		}

		public DropDownView (string Title, IList<string> data, nfloat fSize, nint cellHeight, float layoutHeight, UIColor borderColor, 
			string iOSTitle = "",
			string SelectedText = "") :
		this(Title, data, fSize, cellHeight, layoutHeight, null, null, borderColor, iOSTitle, SelectedText)
		{

		}

		public DropDownView (string Title, IList<string> data, nfloat fSize, 
			nint cellHeight, float layoutHeight, UIColor cellSelectedBackgroundColor, UIColor cellSelectedTextColor,
			UIColor borderColor, 
			string iOSTitle = "",
			string selectedText = "")
		{
			_Values = new DropDownConstants ();
			_Values.HeaderHeight = 20;
			_Values.HeaderFontHeight = 12;

			BorderColor = borderColor;
			NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, OrientationChanged);
			this.Title = Title;
			this._CellHeight = cellHeight;
			this._IsShowing = false;

			this.ControlHeight = layoutHeight;
			this._DropDownControl = new DropDownControl (this, fSize);

			this._TblView = new DropDownTable (data, cellHeight, fSize, cellSelectedBackgroundColor, cellSelectedTextColor, selectedText);

			this._OverLayView = new UIView ();

			// DropDown PopupView
			this._MainView = new UIView ();
			this._MainView.Layer.MasksToBounds = false;
			this._MainView.Layer.BorderColor = this.BorderColor.CGColor;
			this._MainView.Layer.BorderWidth = this._Values.BorderWidth;

			_MainView.UserInteractionEnabled = true;
			this._MainView.UserInteractionEnabled = true;
			this.MultipleTouchEnabled = true;
			this._TblView.MultipleTouchEnabled = true;


			this._HeaderView = new DropDownViewHeader (this, iOSTitle);

			// add DropDownControl
			Add(this._DropDownControl);

			if (this._IsXamarinForms == false) {
				//this.Add (this._MainView);
			}

			this._OverLayView.Add (this._MainView);
			this._Tap = new UITapGestureRecognizer (() => {
				HideDropDown();
			});

			this._Tap.ShouldReceiveTouch = (recognizer, touch) => {
				var view = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
				var pos = touch.LocationInView(view);

				var f = this._MainView.Frame;
				if(f.Contains(pos) == false)
				{
					return true;
				}

				return false;
			};
			// hide if tapped
			this._OverLayView.AddGestureRecognizer(this._Tap);

			_MainView.Add (this._HeaderView);
			_MainView.AddSubview (_TblView);
			_OverLayView.Hidden = true;

			//AddSubview (this.overlay);

			var w = UIApplication.SharedApplication.KeyWindow;
			var v = w.RootViewController.View;
			v.Add (this._OverLayView);

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
				handle (this, new DropDownViewArgs { IsShowing = this._IsShowing, Frame = this.ViewFrame });
			}
		}

		/// <summary>
		/// min value = 10
		/// </summary>
		/// <value>The height of the header.</value>
		public Int32 HeaderHeight {
			get
			{
				return _Values == null ? 20 : _Values.HeaderHeight;
			}
			set
			{
				if (value >= 10) {					
					if (_Values == null)
						_Values = new DropDownConstants ();
					_Values.HeaderHeight = value;
				}
			}
		}

		/// <summary>
		/// min value = 8
		/// </summary>
		/// <value>The height of the header font.</value>
		public float HeaderFontHeight {
			get
			{
				return _Values == null ? 12 : _Values.HeaderFontHeight;
			}
			set
			{
				if (value >= 8) {
					if (_Values == null)
						_Values = new DropDownConstants ();
					_Values.HeaderFontHeight = value;
				}
			}
		}

		public UIColor BorderColor {
			get;
			set;
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

		public Int32 DropDownHeightPortait
		{
			get;
			set;
		}

		public Int32 DropDownHeightLandscape
		{
			get;
			set;
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
				if (this._IsShowing) {
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
			if (this._OverLayView != null && this._OverLayView.Hidden == false) {
				if (this._IsShowing) {
					RenderPopup ();
				}
			}
		}

		protected override void Dispose (bool disposing)
		{
			this._OverLayView.RemoveGestureRecognizer(this._Tap);
			this._Tap.Dispose ();

			this._OverLayView.RemoveFromSuperview ();
			this._MainView.RemoveFromSuperview ();
			this._DropDownControl.RemoveFromSuperview ();
			this._TblView.RemoveFromSuperview ();

			this._HeaderView.Dispose ();
			this._TblView.Dispose ();
			this._MainView.Dispose ();
			this._OverLayView.Dispose ();
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
				var f2 = this._OverLayView == null ? new CGRect(0,0,0,0) : this._OverLayView.Frame;
				var w = (f.Width > f2.Width) ? f.Width : f2.Width;
				var h = f.Height + f2.Height;
				var frame = new CGRect (f.X, f.Y, w, h);
				return frame;
			}
		}

		private void ShowDropDown (CGRect frame)
		{
			if (this._IsShowing == false) {
				this._IsShowing = true;

				this._LastPopupFrame = frame;
				var f = this.Frame;

				if (this._IsXamarinForms == false) {
					this.Frame = new CGRect (f.X, f.Y, this._LastPopupFrame.Width, this.PopupHeight);
				} 

				RenderPopup ();
				_OverLayView.Hidden = false;

			} else {
				HideDropDown ();
			}

			FireChanged();
		}

		private void RenderPopup()
		{
			var screen = UIScreen.MainScreen.Bounds;
			var navSize = TopBarSize ();
			var top = navSize;
			var padding2x = this._Values.WindowPadding * 2;

			this._OverLayView.Frame = new CGRect (0, 0, screen.Width, screen.Height);
			this._OverLayView.BackgroundColor = this._Values.HudBackgroundColour;


			var temp = 0;
			if (IsPortrait) {
				temp = this.DropDownHeightPortait;
			} else {
				temp = this.DropDownHeightLandscape;
			}
			if (temp > 0) {
				top = Convert.ToInt32 ((((int)screen.Height - (int)temp)) / 2.0);
				this._MainView.Frame = new CGRect (this._Values.WindowPadding, 
					top, 
					screen.Width - padding2x, temp);
				_TblView.Frame = new CGRect (0, this._Values.HeaderHeight, 
					screen.Width - padding2x, 
					temp - this._Values.HeaderHeight);
			} else {
				this._MainView.Frame = new CGRect (this._Values.WindowPadding, 
					navSize + this._Values.WindowPadding, 
					screen.Width - padding2x, 
					screen.Height - top - padding2x);
				_TblView.Frame = new CGRect (0, this._Values.HeaderHeight, 
					screen.Width - padding2x, 
					screen.Height - top - padding2x - this._Values.HeaderHeight);
			}
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

		public bool IsPortrait
		{
			get
			{
				var b = UIScreen.MainScreen.Bounds;
				return b.Height > b.Width;
			}
		}

		public void HideDropDown()
		{
			this._IsShowing = false;
			if (this._OverLayView != null) {
				this._OverLayView.Hidden = true;
			}
			var f = this.Frame;
			f.Height = this.ControlHeight;
			this.Frame = f;
		}
	}
}

