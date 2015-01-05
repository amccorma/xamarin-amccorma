using System;
using Xamarin.Forms;

namespace Masked
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			return new NavigationPage (new MyMask ());
		}
	}
}

