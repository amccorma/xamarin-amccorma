using System;
using Xamarin.Forms;

namespace PickerToButton
{
	public class MyPicker : Picker
	{
		public MyPicker ()
		{
		}

		public static readonly BindableProperty PickItemsProperty =
			BindableProperty.Create ("PickItems", typeof(bool), typeof(MyPicker), false);

		public bool PickItems
		{
			get {
				return (bool)GetValue(PickItemsProperty); 
			}
			set {
				this.SetValue(PickItemsProperty, value);                
			}
		}
	}
}

