using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using CoreGraphics;
using mobile.models.Overlay;

namespace mobile.app.ios.Overlays
{
   public sealed class BlankView : OverlayView
   {
      private UIView container;
      public BlankView(OverlayDetails details)
         : base(details)
      {
         var bounds = UIScreen.MainScreen.ApplicationFrame;

         SetFrame();

         container = new UIView();

         container.Frame = new CGRect(
            0,
            0,
            container.Frame.Width,
            container.Frame.Height);

         AddSubview(container);
      }

      internal override void DoLayout()
      {
         base.SetFrame();

         if (container != null)
         {
            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            container.Frame = new CGRect(
               centerX - (container.Frame.Width / 2),
               centerY - (container.Frame.Height / 2),
               container.Frame.Width,
               container.Frame.Height);
         }
      }
   }
}