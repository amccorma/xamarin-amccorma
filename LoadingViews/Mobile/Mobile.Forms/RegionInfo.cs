using System;

using Xamarin.Forms;
using mobile.models.MVVM;
using mobile.models;

namespace mobile.pages
{
	public class RegionInfoPage : TabbedPage
	{	
		public RegionInfoPage ()
		{
			this.Children.Add (ViewFactory.CreatePage<RegionLocationsViewModel> ());		
			this.Children.Add (ViewFactory.CreatePage<RegionOfficersViewModel> ());
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
		}
	}
}


