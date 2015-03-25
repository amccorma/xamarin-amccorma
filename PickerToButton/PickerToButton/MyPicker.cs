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
			BindableProperty.Create<MyPicker, bool>(
				p => p.PickItems, false);

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

