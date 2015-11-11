using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace DropDown.Forms
{
	public class Page2 : ContentPage
	{
		private Button _Button1, _Button2;
		private DropDownPicker _Drop1, _Drop2;

		private void ReloadData()
		{
			var d = new List<string> ();
			d.Add ("New York");
			d.Add ("San Francisco");
			d.Add ("Washington D.C.");
			d.Add ("San Diego");
			d.Add ("test1");
			d.Add ("test6");
			d.Add ("test5");
			d.Add ("test4");

			this._Drop1.Source = d;
		}
		public Page2 ()
		{
			_Button1 = new Button {
				Text = "Test"
			};
			_Button2 = new Button {
				Text = "Reload",
				Command = new Command( () => {
					ReloadData();
				})
			};

			this._Drop1 = new DropDownPicker
			{
				WidthRequest = Device.OnPlatform(100, 120, 100),
				HeightRequest = 25,
				DropDownHeight = 150,
				Title = "Locations",
				SelectedText = "",
				FontSize = Device.OnPlatform(10, 14, 10),
				CellHeight = 20,
				SelectedBackgroundColor = Color.FromRgb (0, 70, 172),
				SelectedTextColor = Color.White
			};

			this._Drop2 = new DropDownPicker
			{
				WidthRequest = Device.OnPlatform(100, 120, 100),
				HeightRequest = 25,
				DropDownHeight = 150,
				Title = "Area",
				FontSize = Device.OnPlatform(10, 14, 10),
				CellHeight = 20,
				SelectedBackgroundColor = Color.FromRgb (0, 70, 172),
				SelectedTextColor = Color.White
			};

			var data = new List<string> ();
			data.Add ("New York");
			data.Add ("San Francisco");
			data.Add ("Washington D.C.");
			data.Add ("San Diego");
			data.Add ("Orlando");
			data.Add ("Charleston");
			data.Add ("Boston");
			data.Add ("New Orleans");
			data.Add ("Las Vegas");
			data.Add ("Anchorage");
			data.Add ("California");
			data.Add ("Detroit");
			data.Add ("North.");
			data.Add ("South");
			data.Add ("Michigan");
			data.Add ("Nevada");
			data.Add ("Ohio");
			data.Add ("Lansing");
			data.Add ("Grand Rapids");
			data.Add ("Europe");

			this._Drop1.Source = data;
			this._Drop2.Source = data;

			this.Content = new StackLayout {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Start,
				Children = {
					_Button1,
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						VerticalOptions = LayoutOptions.Start,
						Children = 
						{
							this._Drop1,
							new Button
							{
								Text = "Button 1"
							},
							this._Drop2,
						}
					},
					_Button2,
					new Button
					{
						Text = "Pop Page",
						Command = new Command(async () => {
							await Navigation.PopAsync();
						})
					}
				}
			};
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			this._Drop1.OnSelected += Drop1Selected;

			this._Drop2.OnSelected += Drop2Selected;

			if (Device.OS == TargetPlatform.iOS) {

				DropDownPicker.AddTapEvents ();
				DropDownPicker.OnTapFrom += OnTapFrom;
			}
		}

		protected override void OnDisappearing ()
		{
			if (Device.OS == TargetPlatform.iOS) {
				DropDownPicker.OnTapFrom -= OnTapFrom;
				DropDownPicker.RemoveEvents ();
			}

			this._Drop1.OnSelected -= Drop1Selected;

			this._Drop2.OnSelected -= Drop2Selected;

			base.OnDisappearing ();
		}

		private void OnTapFrom(object sender, DropDownTapArgs e)
		{
			this._Drop1.DoHideDropDownOnTap (e);
			this._Drop2.DoHideDropDownOnTap (e);
		}

		private void Drop1Selected(object sender, string e)
		{
			System.Diagnostics.Debug.WriteLine ("selected text change Drop1: " + e);
		}

		private void Drop2Selected(object sender, string e)
		{
			System.Diagnostics.Debug.WriteLine ("selected text change Drop2: " + e);
		}
	}
}


