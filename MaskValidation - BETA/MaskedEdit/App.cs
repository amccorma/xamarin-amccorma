using System;
using Xamarin.Forms;

namespace Masked
{
	public class App : Application
	{
		public App()
		{	
			// mask with validation
			/* alpha testing */
			this.MainPage = new NavigationPage (new MaskValidatePage ());


			//this.MainPage = new NavigationPage (new MyMask ());
		}
	}
}

