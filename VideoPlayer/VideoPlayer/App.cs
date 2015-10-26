using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace LabSamples
{
    public class App : Application
    {
		public App()
        {
			//this.MainPage = new NavigationPage(new MaskPage());           


			//this.MainPage = new NavigationPage(new LabSamples.AndroidMediaController());
			//this.MainPage = new NavigationPage (new NoMediaController ());
			this.MainPage = new NavigationPage (new iOSVideoPlayer ());

            //this.MainPage = new LabSamples.Pages.VideoViewPage();
			//this.MainPage = new NavigationPage(new PlayerPage());
        }
    }
}
