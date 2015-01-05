using System;
using Xamarin.Forms;

namespace DeviceEncryption
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			return new NavigationPage(new TestPage());
		}
	}
}

