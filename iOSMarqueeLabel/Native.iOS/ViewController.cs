using System;

using UIKit;

namespace NativeiOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.'

			var label = new MarqueeBinding.MarqueeLabel();
			label.Text = "I'm guessing this project is abandoned or at least relegated to old hardware & OSes";
			label.Frame = new CoreGraphics.CGRect(10, 100, 200, 40);
			this.View.Add(label);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

