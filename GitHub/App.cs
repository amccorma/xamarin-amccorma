using System;
using Xamarin.Forms;

namespace GitHub
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			return new NavigationPage (new MyMask ());
		}
	}
}

