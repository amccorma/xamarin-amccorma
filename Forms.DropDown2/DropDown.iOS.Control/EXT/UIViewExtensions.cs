using System;
using UIKit;
using CoreAnimation;
using CoreGraphics;

namespace DropDown.iOS.Control
{
	public static class UIViewExtensions
	{
		public static void AddBorder(this UIView view, UIRectEdge edge, UIColor color, 
			nfloat thickness, float radius = 0.0f) {

			var border = new CALayer ();
			var f = view.Frame;
			switch(edge)
			{
			case UIRectEdge.Top:
				border.Frame = new CGRect(0, 0, f.Width, thickness);
				break;
			case UIRectEdge.Bottom:
				border.Frame = new CGRect (0, f.Height - thickness, f.Width, thickness);
				break;
			case UIRectEdge.Left:
				border.Frame = new CGRect(0, 0, thickness, f.Height);
				break;
			case UIRectEdge.Right:
				border.Frame = new CGRect(f.Width - thickness, 0, thickness, f.Height);
				break;
			default:
				break;
			}
				
			if (radius > 0) {
				border.CornerRadius = radius;
			}
			border.BackgroundColor = color.CGColor;
			view.Layer.AddSublayer (border);
		}
	}
}

