using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace PickerToButton
{
	public class MyPickerView : ContentView
	{
		public MyPicker picker;

		public MyPickerView ()
		{
			var list = new List<string>();
			list.Add("one");
			list.Add("two");
			list.Add("three");

			picker = new MyPicker
			{
				Title = "Test Picker",
				HorizontalOptions = LayoutOptions.FillAndExpand,            
			};

			foreach(var items in list)
			{
				picker.Items.Add(items);
			}

			Content = new StackLayout
			{
				Children = {
					picker
				}
			};
		}
	}
}

