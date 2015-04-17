using Android.App;
using Android.Views;
using Android.Views.Animations;
using System;
using Xamarin.Forms.Platform.Android;
using mobile.models.Overlay;

namespace PSEA.Mobile.Droid.Overlays
{
	internal class OverLayFragments : Android.App.Fragment
	{
      internal protected readonly float NavigationBarHeightPortrait = 44;
      internal protected readonly float NavigationBarHeightOther = 32;
      internal protected readonly float TabBarHeight = 49;
      internal protected readonly float NotPortraitHeightOffset = 10;
      internal protected OverlayDetails ViewDetails;

      public OverLayFragments()
      {

      }

      public OverLayFragments(OverlayDetails details)
      {
         this.ViewDetails = details;
      }

      internal protected void SetView(View view, Activity activity)
      {
         if (ViewDetails != null)
         {
            if (ViewDetails.BackgroundColor == Xamarin.Forms.Color.Default)
            {
               var r = activity.Window.DecorView.RootView;
               view.Background = r.Background;
            }
            else
            {
               view.Background = new Android.Graphics.Drawables.ColorDrawable(ViewDetails.BackgroundColor.ToAndroid());
            }

            // 1 to 255 for Android
            // 0 (fully transparent) to 255 (completely opaque)
            if (ViewDetails.Alpha != 1)
            {
               view.Alpha = (float)(255.00 * (ViewDetails.Alpha));
            }
         }
      }

      internal void Hide()
      {
         if (this.ViewDetails != null && this.ViewDetails.AnimateClosing && this.View != null)
         {
            var animation1 = new AlphaAnimation(this.View.Alpha, 0);
            animation1.Duration = 500;
            animation1.StartOffset = 100;
            this.View.StartAnimation(animation1);
         }
      }
	}
}

