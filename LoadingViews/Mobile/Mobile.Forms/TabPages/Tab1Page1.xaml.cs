using System;
using Xamarin.Forms;
using mobile.pages.Overlay;

namespace mobile.pages
{
	public partial class Tab1Page1 : ContentPage
	{
		private IShowOverLay manager;

		public Tab1Page1 ()
		{
			this.Title = "Tab1";
			InitializeComponent ();
			manager = DependencyService.Get<IShowOverLay> ();
		}

		protected async void Button1Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new Page1 ());
		}

		protected void ShowLoadingPanel(object sender, EventArgs e)
		{
			manager.ShowLoadingScreen (new OverlayDetails {
				Alpha = Device.OnPlatform(.5f, 150, 1),
				BackgroundColor = Color.Gray,
				HasNavigationBar = true,
				HasTabbedBar = true
			});
		}

		protected void ShowErrorPanel(object sender, EventArgs e)
		{
			manager.ShowDisabledScreen (new OverlayDetails {
				Alpha = 255,
				BackgroundColor = Color.Red,
				HasNavigationBar = true,
				HasTabbedBar = true
			});
		}

		protected void HideAll(object sender, EventArgs e)
		{
			manager.HideAll ();
		}
	}
}

