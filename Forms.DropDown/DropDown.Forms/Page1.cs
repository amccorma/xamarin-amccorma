using System;
using Xamarin.Forms;

namespace DropDown.Forms
{
	public class Page1 : ContentPage
	{
		private Button _Button1;
		public Page1 ()
		{
			_Button1 = new Button ();
			_Button1.Text = "Push Page 2";
			_Button1.Command = new Command (async() => {
				await Navigation.PushAsync(new Page2());
			});
			this.Content = new StackLayout {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Start,
				Children = {
					new Label
					{ 
						Text = "Page1"
					},
					_Button1,

				}
			};
		}
	}
}

