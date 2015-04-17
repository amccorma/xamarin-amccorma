using Android.App;
using Android.Content.PM;
using Android.OS;
using System;
using Xamarin.Forms.Platform.Android;
namespace PSEA.Mobile.Droid
{
	[Activity(Label = "PSEA", Theme = "@style/Theme.Pseaapp", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsApplicationActivity
	{

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);


			Xamarin.Forms.Forms.Init (this, savedInstanceState);

			LoadApplication (new mobile.pages.App());
		}
	}
}

