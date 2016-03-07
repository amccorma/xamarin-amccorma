using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PickerToButton;

[assembly: ExportRenderer(typeof(MyPickerView), typeof(PickerToButton.Droid.MyPickerViewRenderer))]
namespace PickerToButton.Droid
{
	public class MyPickerViewRenderer : ViewRenderer<MyPickerView, global::Android.Widget.Button>
	{
		public MyPickerViewRenderer ()
		{

		}

		protected override void OnElementChanged (ElementChangedEventArgs<MyPickerView> e)
		{
			base.OnElementChanged (e);
			if (this.Control == null)
			{
				var myButton = new global::Android.Widget.Button (this.Context);
				myButton.Click += (object sender, EventArgs e2) => {
					this.Element.picker.PickItems = true;
				};

				myButton.Text = "New Button Text";

				SetNativeControl(myButton);
			}
		}
	}
}

