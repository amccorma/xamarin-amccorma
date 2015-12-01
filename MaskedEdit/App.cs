using System;
using Xamarin.Forms;

namespace Masked
{
	public class App : Application
	{
		public App()
		{	
			this.MainPage = new NavigationPage (new MyMask ());
		}
	}
}

