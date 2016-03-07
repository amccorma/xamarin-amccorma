using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using CoreGraphics;
using mobile.pages.Overlay;

namespace mobile.app.ios.Overlays
{
   public sealed class LoadingView : OverlayView
   {

      // control declarations
      UIActivityIndicatorView activitySpinner;

      public LoadingView(OverlayDetails details)
         : base(details)
      {

         //AutosizesSubviews = true;
         //AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

         //var bounds = UIScreen.MainScreen.Bounds; // portrait bounds
         //var bounds = UIScreen.MainScreen.ApplicationFrame;

         //var h3 = this.Self;
         //var bounds2 = UIScreen.MainScreen.Bounds;

         //var has = UIApplication.SharedApplication.StatusBarHidden;

         //var statusHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
         //
         //var nav2 = UIApplication.SharedApplication.Windows;
         //var navigationBar = OverlayHelper.NavigationBarHeight (true);
         //var nav3 = UIApplication.SharedApplication.KeyWindow;
         //var nav6 = nav3.Screen.Self;

         //this.AutosizesSubviews = true;

         var bounds = UIScreen.MainScreen.ApplicationFrame;
         DoLayout();

         // derive the center x and y
         nfloat centerX = Frame.Width / 2;
         nfloat centerY = Frame.Height / 2;

        // create the activity spinner, center it horizontall and put it 5 points above center x
		CGAffineTransform transform = CoreGraphics.CGAffineTransform.MakeScale(1.5f, 1.5f);
		activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray); //WhiteLarge);
			activitySpinner.Transform = transform;
        activitySpinner.Frame = new CGRect(
            centerX - (activitySpinner.Frame.Width / 2),
            centerY - (activitySpinner.Frame.Height / 2),
            activitySpinner.Frame.Width,
            activitySpinner.Frame.Height);
         activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
         AddSubview(activitySpinner);
         activitySpinner.StartAnimating();
      }

      internal override void DoLayout()
      {
         base.SetFrame();

         if (activitySpinner != null)
         {
            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            activitySpinner.Frame = new CGRect(
               centerX - (activitySpinner.Frame.Width / 2),
               centerY - (activitySpinner.Frame.Height / 2),
               activitySpinner.Frame.Width,
               activitySpinner.Frame.Height);
         }
      }
   }
}
