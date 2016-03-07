using Xamarin.Forms;
using System;
using mobile.pages.Overlay;

namespace mobile.pages
{
	public partial class Tab1Page2 : ContentPage
	{
		private IShowOverLay manager;

		public Tab1Page2 ()
		{
			this.Title = "Tab2";
			InitializeComponent ();
			manager = DependencyService.Get<IShowOverLay> ();
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

