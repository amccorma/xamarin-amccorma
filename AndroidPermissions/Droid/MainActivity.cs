using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace AndroidPermissions.Droid
{
	[Activity (Label = "AndroidPermissions.Droid", Icon = "@drawable/icon", 
		Theme="@style/Theme.AppCompat.Light.NoActionBar", 
		MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}

		private Android.Views.View GetSnackbarAnchorView()
		{
			var a = (Activity)Forms.Context;

			//			var v2 = a.FindViewById(Android.Resource.Id.Content);
			//			var v3 = v2.RootView;
			//			var v =  a.CurrentFocus;
			//			return v;

			var v3 = a.FindViewById(Android.Resource.Id.Content);
			//var v = a.FindViewById(Resource.Id.decor_content_parent);
			//var v2 = a.FindViewById(Resource.Id.concontent);
			return v3;
		}

		public override void OnRequestPermissionsResult (int requestCode, string[] permissions, Permission[] grantResults)
		{
			if (grantResults[0] == (int)Permission.Granted)
			{					
				SetPermissions.OKResultHandler (requestCode);
			}
			else
			{
				SetPermissions.FailedResultHandler (requestCode);
			}
		}

		public override void OnRequestPermissionsResult (int requestCode, string[] permissions)
		{
			base.OnRequestPermissionsResult (requestCode, permissions);
		}

//		const int RequestLocationId = 0;
//
//		private Task<bool> _Task;
//
//		readonly string [] PermissionsLocation = 
//		{
//			Manifest.Permission.AccessCoarseLocation,
//			Manifest.Permission.AccessFineLocation
//		};
//
//
//		public void GetLocationCompatAsync(Task<bool> task)
//		{
//			this._Task = task;
//			const string permission = Manifest.Permission.AccessFineLocation;
//
//			var c = Xamarin.Forms.Forms.Context;
//			if (ContextCompat.CheckSelfPermission(c, permission) == (int)Android.Content.PM.Permission.Granted)
//			{
//				System.Diagnostics.Debug.WriteLine ("permission ok");
//
//			}
//
//			if (ActivityCompat.ShouldShowRequestPermissionRationale(Xamarin.Forms.Forms.Context as Activity, permission))
//			{
//				Snackbar.Make(GetSnackbarAnchorView(), "Location access is required to show coffee shops nearby.",
//					Snackbar.LengthIndefinite)
//					.SetAction("OK", v => ActivityCompat.RequestPermissions(Xamarin.Forms.Forms.Context as MainActivity, PermissionsLocation, RequestLocationId))
//					.Show();
//				return;
//			}
//
//			ActivityCompat.RequestPermissions(Xamarin.Forms.Forms.Context as Activity, PermissionsLocation, RequestLocationId); 
//		}
//
//		public override void OnRequestPermissionsResult (int requestCode, string[] permissions, Permission[] grantResults)
//		{
//			switch (requestCode)
//			{
//				case RequestLocationId:
//				{
//					if (grantResults[0] == (int)Permission.Granted)
//					{
//						//Permission granted
//						Snackbar.Make(GetSnackbarAnchorView(), "Location permission is available, getting lat/long",
//							Snackbar.LengthLong)
//							.Show();					
//
//					}
//					else
//					{
//						Snackbar.Make(GetSnackbarAnchorView(), "Location permission is denied",
//							Snackbar.LengthLong)
//							.Show();
//					}
//				}
//				break;
//			}
//		}
	}
}

