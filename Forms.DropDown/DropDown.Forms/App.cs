using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DropDown.Forms
{
	public class App : Application
	{
		public App ()
		{
			this.MainPage = new NavigationPage (new Page1 ());
		}
	}
}

