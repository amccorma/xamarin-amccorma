using System;
using PickerToButton;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(MyPickerView), typeof(PickerToButton.Droid.MyPickerViewRenderer))]
namespace PickerToButton.Droid
{
	public class MyPickerViewRenderer : ViewRenderer<MyPickerView, UIButton>
	{
		public MyPickerViewRenderer ()
		{

		}

		protected override void OnElementChanged (ElementChangedEventArgs<MyPickerView> e)
		{
			base.OnElementChanged (e);
			if (this.Control == null)
			{
				var myButton = new UIButton ();
				myButton.TouchUpInside += (object sender, EventArgs e2) => {
					this.Element.picker.PickItems = true;
				};

				myButton.SetTitle("New Button Text", UIControlState.Application);
				myButton.SetTitle("New Button Text", UIControlState.Normal);
				myButton.BackgroundColor = UIColor.Black;
				myButton.TintColor = UIColor.White;
				SetNativeControl(myButton);
			}
		}
	}
}

