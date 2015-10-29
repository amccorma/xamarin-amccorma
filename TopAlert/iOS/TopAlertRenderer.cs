using System;
using UIKit;
using System.Drawing;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using Foundation;

[assembly: Xamarin.Forms.Dependency (typeof (TopAlert.iOS.TopAlertRenderer))]
namespace TopAlert.iOS
{
	public class TopAlertRenderer : ITopAlert
	{
		private static MyToast _Layout;

		public void Show(TopAlert alert)
		{
			if (_Layout != null) {				
				_Layout.Dispose ();
				_Layout = null;
			}

			var window = UIApplication.SharedApplication.KeyWindow.RootViewController;
			_Layout = new MyToast (alert);
			window.Add (_Layout);

			UIApplication.SharedApplication.KeyWindow.BringSubviewToFront (_Layout);
		}

		public void Kill()
		{
			if (_Layout != null) {				
				_Layout.Dispose ();
				_Layout = null;
			}
		}
	}

	public class MyToast : UIView
	{
		private TopAlert _Alert;
		private UILabel _Label;
		private UIGestureRecognizer _Gest1, _Gest2;

		public MyToast(TopAlert alert) : base(RectangleF.Empty)
		{
			this._Alert = alert;

			BackgroundColor = this._Alert.BackgroundColor.ToUIColor ();
			ClipsToBounds = false;
			UserInteractionEnabled = true;

			_Label = new UILabel { 
				Text = this._Alert.Text, 
				LineBreakMode = UILineBreakMode.TailTruncation, 
				TextColor = this._Alert.TextColor.ToUIColor() 
			};

			if (this._Alert.TextSize > 0) {
				this._Label.Font = UIFont.SystemFontOfSize (this._Alert.TextSize);
			}

			this.AddSubview (this._Label);

			var w = UIApplication.SharedApplication.KeyWindow.Frame;

			var intent = alert.Intent;

			this.Frame = new CGRect (intent, TopBarSize() + alert.TopOffset, 
				w.Width - intent * 2, (this._Alert.AlertHeight < 0 ? 40 : this._Alert.AlertHeight));

			NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, OrientationChanged);

			this._Gest1 = new UITapGestureRecognizer(MessageTapped);
			this._Gest2 = new UISwipeGestureRecognizer (MessageTapped);
			this.AddGestureRecognizer(this._Gest1);
			this.AddGestureRecognizer(this._Gest2);

			this.PerformSelector (new ObjCRuntime.Selector ("TimerElaspsed:"), null, this._Alert.Duration);
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

		[Export ("TimerElaspsed:")]
		private void TimerElaspsed(NSObject x)
		{
			System.Diagnostics.Debug.WriteLine ("timer");
			DismissMessage();
		}

		private void OrientationChanged(NSNotification notification)
		{
//			Frame = new CoreGraphics.CGRect((float) Frame.X, (float) Frame.Y, 
//				w.Width - intent * 2, (float) Frame.Height);

			var intent = this._Alert.Intent;
			var w = UIApplication.SharedApplication.KeyWindow.Frame;

			this.Frame = new CGRect (intent, TopBarSize() + this._Alert.TopOffset, 
				w.Width - intent * 2, (this._Alert.AlertHeight < 0 ? 40 : this._Alert.AlertHeight));
			
			SetNeedsDisplay();
		}

		private CGRect OrientFrame(CGRect frame)
		{
			if (IsDeviceLandscape(UIDevice.CurrentDevice.Orientation) ||
				IsStatusBarLandscape(UIApplication.SharedApplication.StatusBarOrientation))
			{
				frame = new RectangleF((float) frame.X, (float) frame.Y, (float) frame.Height, (float) frame.Width);
			}
			return frame;
		}

		private bool IsDeviceLandscape(UIDeviceOrientation orientation)
		{
			return orientation == UIDeviceOrientation.LandscapeLeft || orientation == UIDeviceOrientation.LandscapeRight;
		}

		private bool IsStatusBarLandscape(UIInterfaceOrientation orientation)
		{
			return orientation == UIInterfaceOrientation.LandscapeLeft ||
				orientation == UIInterfaceOrientation.LandscapeRight;
		}

		public override void Draw(CGRect rect)
		{
			var f = this.Frame;

			this._Label.Frame = new CGRect (5, 0, f.Width - 10, f.Height);

			if (this._Alert.BorderWidth > 0) {
				f.Width += this._Alert.BorderWidth;
				f.Height += this._Alert.BorderWidth;
				this.Layer.BorderColor = this._Alert.BorderColor.ToCGColor ();
				this.Layer.BorderWidth = this._Alert.BorderWidth;
			}
		}

		private void MessageTapped(UIGestureRecognizer recognizer)
		{
			DismissMessage();
		}

		private void DismissMessage()
		{
			if (this._Alert.FadeOut) {
				InvokeOnMainThread (() => {
					UIView.Animate (1.0, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
						this.Alpha = 0;
					}, () => {
						// completion
						this.RemoveFromSuperview ();
					});
				});
			} else {
				InvokeOnMainThread (() => {
					this.RemoveFromSuperview ();
				});
			}
		}

		protected override void Dispose (bool disposing)
		{
			if (this._Gest1 != null) {
				this.RemoveGestureRecognizer (this._Gest1);
				this._Gest1.Dispose ();
			}

			if (this._Gest2 != null) {
				this.RemoveGestureRecognizer (this._Gest2);
				this._Gest2.Dispose ();
			}
			base.Dispose (disposing);
		}
	}
}