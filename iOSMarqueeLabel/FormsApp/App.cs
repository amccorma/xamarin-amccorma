using System;
using Xamarin.Forms;

namespace FormsApp
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
			this.MainPage = new NavigationPage(new Page1());
        }
    }
}
