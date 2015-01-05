using System;
using Xamarin.Forms;

namespace DeviceEncryption
{
	public class TestPage : ContentPage
	{
		private Entry entry1, entry2;
		private Encrypt Simple, Complex;

		public TestPage ()
		{
			var btn1 = new Button { Text = "Simple Encrypt" };
			var btn2 = new Button { Text = "Simple Decrypt" };

			entry1 = new Entry ();

			btn1.Clicked += (object sender, EventArgs e) => 
			{
				if (entry1.Text != "")
				{
					Simple = DependencyService.Get<ISecure>().Encode(EncryptType.OK, entry2.Text);
					entry1.Text = "Encrypted";
				}
				else
				{
					DisplayAlert(null, "Enter text", "OK");
				}
			};

			btn2.Clicked += (object sender, EventArgs e) => {
				if (Simple != null)
				{
					entry1.Text = DependencyService.Get<ISecure>().Decode(Simple);
					Simple = null;
				}
				else
				{
					DisplayAlert(null, "Must Encrypt first", "OK");
				}
			};


			var btn3 = new Button { Text = "Encrypt" };
			var btn4 = new Button { Text = "Decrypt" };

			btn3.Clicked += (object sender, EventArgs e) => 
			{
				if (entry2.Text != "")
				{
					Complex = DependencyService.Get<ISecure>().Encode(EncryptType.STRONG, entry2.Text.Trim());
					entry2.Text = "Encrypted";
				}
				else
				{
					DisplayAlert(null, "Enter text", "OK");
				}
			};

			btn4.Clicked += (object sender, EventArgs e) => {
				if (Complex != null)
				{
					entry2.Text = DependencyService.Get<ISecure>().Decode(Complex);
					Complex = null;
				}
				else
				{
					DisplayAlert(null, "Must Encrypt first", "OK");
				}
			};


			entry2 = new Entry ();

			/* Secure Device Storage */

			var btn6 = new Button { Text = "Get" };
			var btn7 = new Button { Text = "Exists" };
			var btn8 = new Button { Text = "Clear" };
			var btn9 = new Button { Text = "Delete" };
			var btn10 = new Button { Text = "Save" };


			btn6.Clicked += (object sender, EventArgs e) => {
				var result = DependencyService.Get<ISecure>().Get("test");
				DisplayAlert("Result", result, "OK");
			};

			btn7.Clicked += (object sender, EventArgs e) => {
				var result = DependencyService.Get<ISecure>().Exists("test");
				if (result)
				{
					DisplayAlert(null, "Found", "OK");
				}
				else
				{
					DisplayAlert(null, "Not Found", "OK");
				}
			};

			btn8.Clicked += (object sender, EventArgs e) => {
				DependencyService.Get<ISecure>().Clear();
			};

			btn9.Clicked += (object sender, EventArgs e) => {
				DependencyService.Get<ISecure>().Delete("test");
			};

			btn10.Clicked += (object sender, EventArgs e) => {
				DependencyService.Get<ISecure>().Save("test", "value");
			};
				


			this.Content = new ScrollView {
				Content = 
					new StackLayout {
						Children = {
							entry1, 
							btn1, 
							btn2,
							new BoxView {
								BackgroundColor = Color.Red,
								HeightRequest = 5,
								Color = Color.Red,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							entry2,
							btn3,
							btn4,
							new BoxView {
								BackgroundColor = Color.Red,
								HeightRequest = 5,
								Color = Color.Red,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Label { 
								Text = "Secure Device Storage"
							},
							btn6,
							btn7,
							btn8,
							btn9,
							btn10
						}
					}
				
			};
		}
	}
}

