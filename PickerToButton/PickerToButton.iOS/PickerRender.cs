using Xamarin.Forms;
using PickerToButton;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyPicker), typeof(PickerToButton.Droid.PickerRender))]
namespace PickerToButton.Droid
{
	public class PickerRender : Xamarin.Forms.Platform.iOS.PickerRenderer 
   {
		protected override void OnElementChanged (ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged (e);
			if (this.Control != null) {
				this.Control.Alpha = 0;
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == MyPicker.PickItemsProperty.PropertyName) {
				Device.BeginInvokeOnMainThread (() => {
                    this.Control.BecomeFirstResponder();
					this.Control.SendActionForControlEvents (UIKit.UIControlEvent.TouchDown);
					//this.Control.PerformSelector(new ObjCRuntime.Selector("Click"), this.Control);
				});
			}
		}
   }
}