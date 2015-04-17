using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using mobile.models;
using mobile.models.MVVM;
using mobile.models.MVVM.Navigation;

namespace mobile.pages
{
	public partial class App : Application
	{
		public App ()
		{
			ViewFactory.Register<RegionInfoViewModel, RegionInfoPage> ();
			ViewFactory.Register<RegionOfficersViewModel, RegionOfficersPage> ();
			ViewFactory.Register<RegionLocationsViewModel, RegionLocationsPage> ();	
			ViewFactory.Register<ServiceErrorViewModel, ServiceError> ();

			ViewFactory.Register<Page1ViewModel, Page1> ();

			InitializeComponent ();	
			this.MainPage = new NavigationView(ViewFactory.CreatePage<Page1ViewModel> ());
		}
	
	}
}

