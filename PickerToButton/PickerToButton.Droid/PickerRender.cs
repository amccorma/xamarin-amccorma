using Android.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using PickerToButton;

[assembly: ExportRenderer(typeof(MyPicker), typeof(PickerToButton.Droid.PickerRender))]
namespace PickerToButton.Droid
{
	public class PickerRender : Xamarin.Forms.Platform.Android.PickerRenderer 
   {
		protected override void OnElementChanged (ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged (e);
			if (this.Control != null) {
				this.Control.Visibility = ViewStates.Invisible;
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == MyPicker.PickItemsProperty.PropertyName) {
				this.Control.PerformClick ();
			}
		}
   }
}