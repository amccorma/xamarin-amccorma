using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using mobile.pages.Overlay;

namespace mobile.app.ios.Overlays
{
	public static class Ext
	{
		/// <summary>
		/// Flatten the specified source and recursion.
		/// foreach(var category in categories.Flatten(c => c.Children))
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="recursion">Recursion.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
		{
			foreach (var item in source)
			{
				yield return item;
				foreach (var child in childrenSelector(item).Flatten(childrenSelector))
				{
					yield return child;
				}
			}
		}

		public static T FirstOrDefaultFromMany<T>(
			this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector,
			Predicate<T> condition)
		{
			// return default if no items
			if(source == null || !source.Any()) return default(T);

			// return result if found and stop traversing hierarchy
			var attempt = source.FirstOrDefault(t => condition(t));
			if(!Equals(attempt,default(T))) return attempt;

			// recursively call this function on lower levels of the
			// hierarchy until a match is found or the hierarchy is exhausted
			return source.SelectMany(childrenSelector)
				.FirstOrDefaultFromMany(childrenSelector, condition);
		}
	}


   public abstract class OverlayView : UIView
   {
      internal protected readonly float NavigationBarHeightPortrait = 45;
      internal protected readonly float NavigationBarHeightOther = 33;  // + 1
      internal protected readonly float TabBarHeight = 49;
      internal protected readonly float NotPortraitHeightOffset = 10;
      private bool RegisteredForObserver = false;
      private OverlayDetails ViewDetails;

      public OverlayView(IntPtr h)
         : base(h)
      {

      }

      public OverlayView()
      {

      }
			

		private void RegisterForObserver()
		{
			 var notificationCenter = NSNotificationCenter.DefaultCenter;
			 notificationCenter.AddObserver(UIApplication.DidChangeStatusBarOrientationNotification, DeviceOrientationDidChange);
			 UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
			 RegisteredForObserver = true;
		}

		public OverlayView(OverlayDetails details, bool XibView = false)
		{
			// configurable bits
			this.ViewDetails = details;
			//this.BackgroundColor = UIColor.Black;
			if (this.ViewDetails.HasNavigationBar == false)
			{
				// check NavigationBar
				this.ViewDetails.HasNavigationBar = UIApplication.SharedApplication.KeyWindow.Subviews.FirstOrDefaultFromMany (item => item.Subviews, x => x is UINavigationBar) != null;
				this.NavigationBarHeightOther = 0;
				this.NavigationBarHeightPortrait = 0;
			}
			if (XibView == false)
			{
				SetViewProperties(this);
			}
			RegisterForObserver();
		}

		internal protected void SetViewProperties(UIView view)
		{
			if (this.ViewDetails.BackgroundColor != Xamarin.Forms.Color.Default) {
				this.BackgroundColor = this.ViewDetails.BackgroundColor.ToUIColor ();
			} else {
				this.BackgroundColor = UIColor.White;
			}
			this.Alpha = 1; //(nfloat)this.ViewDetails.Alpha;
		}

		void DeviceOrientationDidChange(NSNotification notification)
		{
			DoLayout();
		}

		internal abstract void DoLayout();


		/// <summary>
		/// Fades out the control and then removes it from the super view
		/// </summary>
		internal protected void Hide()
		{
			if (this.ViewDetails != null && this.ViewDetails.AnimateClosing == true)
			{
				UIView.Animate(
				   0.5, // duration
				   () => { Alpha = 0; },
				   () => { RemoveFromSuperview(); }
				);
			}
			else
			{
				RemoveFromSuperview();
			}

			if (this.RegisteredForObserver)
			{
				var notificationCenter = NSNotificationCenter.DefaultCenter;
				notificationCenter.RemoveObserver(this, UIDevice.OrientationDidChangeNotification, UIApplication.SharedApplication);
				UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();
				notificationCenter.RemoveObserver(this, UIApplication.DidBecomeActiveNotification, UIApplication.SharedApplication);
			}

			this.Dispose();
		}

		/// <summary>
		/// Gets the height of the status bar.
		/// </summary>
		/// <value>The height of the status bar.</value>
		internal protected nfloat StatusBarHeight
		{
			get
			{
				bool statusHidden = UIApplication.SharedApplication.StatusBarHidden;
				nfloat statusHeight = 0;
				if (statusHidden == false)
				{
				   statusHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
				}
				return statusHeight;
			}
		}

		internal protected void SetFrame()
		{
			this.Frame = GetFrame();
		}

		internal protected CGRect GetFrame()
		{
			var bounds = UIScreen.MainScreen.ApplicationFrame;
			CGRect frame;
			if (UIApplication.SharedApplication.StatusBarOrientation != UIInterfaceOrientation.Portrait)
			{
				bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
			}

			if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.Portrait ||
				UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.PortraitUpsideDown)
			{
				frame = new CGRect(0, NavigationBarHeightPortrait + this.StatusBarHeight, bounds.Width,
				   bounds.Height - NavigationBarHeightPortrait);
			}
			else
			{
			// no status bar here
				frame = new CGRect(0, NavigationBarHeightOther, bounds.Height,
				   bounds.Width - this.NotPortraitHeightOffset);
			}

			if (ViewDetails.HasTabbedBar)
			{
				// standard height: 49px from bottom
				frame = new CGRect(frame.X, frame.Y, frame.Width, frame.Height - this.TabBarHeight);
			}

			return frame;
		}
	}
}

