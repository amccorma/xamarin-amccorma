using System;
using UIKit;

namespace DropDown.iOS.Control.Table
{
	public class DropDownCell : UITableViewCell
	{
		public DropDownCell (UITableViewCellStyle style, string id) : base(style, id)
		{
			LayoutMargins = UIEdgeInsets.Zero;
			PreservesSuperviewLayoutMargins = false;
			SeparatorInset = UIEdgeInsets.Zero;
			MultipleTouchEnabled = true;


			ContentView.ExclusiveTouch = true;
			ExclusiveTouch = true;

		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			if (this.Frame.X >= 0) {
				this.Frame = new CoreGraphics.CGRect (this.Frame.X - 10, this.Frame.Y, this.Frame.Width + 10, this.Frame.Height);
			}
		}
	}
}
