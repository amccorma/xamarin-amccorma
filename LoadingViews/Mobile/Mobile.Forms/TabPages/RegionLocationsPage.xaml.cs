using System;
using Xamarin.Forms;
using mobile.models.Controls;

namespace mobile.pages
{
	public partial class RegionLocationsPage : BasePage
	{
		public RegionLocationsPage ()
		{
			InitializeComponent ();
		}

		protected async void Button1Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new Page1 ());
		}
	}
}

