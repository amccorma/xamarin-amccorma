using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace VideoSamples
{
    public class App : Application
    {
		public App()
        {
			//this.MainPage = new NavigationPage (new AndroidVideoPlayer ());
			this.MainPage = new NavigationPage (new iOSVideoPlayer ());
        }
    }
}
