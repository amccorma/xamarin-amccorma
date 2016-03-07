using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace mobile.pages
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent ();	
			this.MainPage = new NavigationPage (new Page1 ());
		}
	
	}
}

