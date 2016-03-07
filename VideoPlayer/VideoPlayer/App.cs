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
			if (Device.OS == TargetPlatform.Android) {
				this.MainPage = new NavigationPage (new AndroidVideoPlayer ());
			} else {
				this.MainPage = new NavigationPage (new iOSVideoPlayer ());
			}
        }
    }
}
