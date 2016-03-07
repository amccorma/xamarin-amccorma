using System;
using System.Collections.Generic;
using Xamarin.Forms;
using mobile.pages.Overlay;

namespace mobile.pages
{
	public partial class Page1 : ContentPage
	{
		private IShowOverLay manager;

		public Page1 ()
		{			
			InitializeComponent ();
			manager = DependencyService.Get<IShowOverLay> ();
		}

		protected void ShowLoadingPanel(object sender, EventArgs e)
		{
			manager.ShowLoadingScreen (new OverlayDetails {
				Alpha = Device.OnPlatform(.5f, 150, 1),
				BackgroundColor = Color.Gray,
				HasNavigationBar = true
			});
		}

		protected void ShowBlankPanel(object sender, EventArgs e)
		{
			manager.ShowBlankScreen (new OverlayDetails {
				Alpha = 255,
				BackgroundColor = Color.Red,
				HasNavigationBar = true
			});
		}
		protected void HideAll(object sender, EventArgs e)
		{
			manager.HideAll ();
		}

		protected void DisabledPanel(object sender, EventArgs e)
		{
			manager.ShowDisabledScreen (new OverlayDetails {
				Alpha = 255,
				BackgroundColor = Color.Blue,
				HasNavigationBar = true
			});
		}

		protected async void PushPage(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new TabPage());
		}
	}
}

