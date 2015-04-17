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
   public sealed class DisabledView : OverlayView
	{
		private UIView container;

		public DisabledView(OverlayDetails details) : base (details)
		{
			var bounds = UIScreen.MainScreen.ApplicationFrame;

			SetFrame ();

			container = new UIView ();
			container.Frame = new CoreGraphics.CGRect (0, 0, bounds.Width, 65);

			var label1 = new UILabel ();
			label1.Frame = new CoreGraphics.CGRect (0, 10, bounds.Width, 20);
			label1.TextAlignment = UITextAlignment.Center;
			label1.Text = "Something bad happened.";
			container.AddSubview (label1);

			var label2 = new UILabel ();
			label2.Frame = new CoreGraphics.CGRect (0, 35, bounds.Width, 20);
			label2.TextAlignment = UITextAlignment.Center;
			label2.Text = "Try your request again.";
			container.AddSubview (label2);


			// derive the center x and y
			nfloat centerX = Frame.Width / 2;
			nfloat centerY = Frame.Height / 2;

			container.Frame = new CGRect ( 
				centerX - (container.Frame.Width / 2) ,
				centerY - (container.Frame.Height /2),
				container.Frame.Width ,
				container.Frame.Height);

			AddSubview (container);
		}
			

		internal override void DoLayout ()
		{
			base.SetFrame ();

			if (container != null) {
				nfloat centerX = Frame.Width / 2;
				nfloat centerY = Frame.Height / 2;

				container.Frame = new CGRect ( 
					centerX - (container.Frame.Width / 2) ,
					centerY - (container.Frame.Height / 2),
					container.Frame.Width ,
					container.Frame.Height);
			}
		}
	}
}

