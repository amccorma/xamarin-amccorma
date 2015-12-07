using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Java.Text;


namespace Masked.Android
{
	[Activity(Label = "Masked.Android", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Xamarin.Forms.Forms.Init (this, bundle);

			//PackageInfo.lastUpdateTime

			var a = this.ApplicationContext.PackageManager.GetPackageInfo (this.PackageName, 0).LastUpdateTime;
			var d = JavaLongToDate (a);
		}


		public DateTime JavaLongToDate(long javaLong)
		{
			DateTime unixYear0 = new DateTime(1970, 1, 1);
			long unixTimeStampInTicks = javaLong / 1000 * TimeSpan.TicksPerSecond;
			DateTime dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
			return dtUnix;
		}
	}
}

