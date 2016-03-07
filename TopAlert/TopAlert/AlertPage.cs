using System;

using Xamarin.Forms;

namespace TopAlert
{
	public class AlertPage : ContentPage
	{
		/*

		Android Alert Format (nice box)

		new Button {
			Text = "Alert",
			Command = new Command (() => {
				DependencyService.Get<ITopAlert> ().Show (new TopAlert {
					BackgroundColor = Xamarin.Forms.Color.Blue,
					BorderColor = Color.White,
					TextColor = Color.White, 
					Text = "Message Text " + DateTime.Now.ToString(),
					TextSize = 12,
					TopOffset = 0,
					Intent = 25,
					FadeOut = true,
					AlertHeight = Device.OnPlatform (20, 200, 20)
				});
			})
		}		



		*/
		public AlertPage ()
		{
			Content = new StackLayout {
				VerticalOptions = LayoutOptions.Center,
				Children = {
					new Label {
						HorizontalTextAlignment = TextAlignment.Center,
						Text = "Welcome to Xamarin Forms!"
					},
					new Button {
						Text = "Alert",
						Command = new Command (() => {
							DependencyService.Get<ITopAlert> ().Show (new TopAlert {
								BackgroundColor = Xamarin.Forms.Color.Blue,
								BorderColor = Color.White,
								TextColor = Color.White, 
								Text = "Message Text " + DateTime.Now.ToString(),
								TextSize = 12,
								TopOffset = 0,
								Intent = 5,
								FadeOut = false,
								AlertHeight = 40
							});
						})
					}					
				}
			};
		}
	}
}


