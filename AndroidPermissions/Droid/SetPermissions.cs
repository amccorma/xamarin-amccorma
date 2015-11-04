using System;
using Android;
using System.Threading.Tasks;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Content.PM;
using System.Linq;

[assembly: Xamarin.Forms.Dependency (typeof (AndroidPermissions.Droid.SetPermissions))]
namespace AndroidPermissions.Droid
{
	public class SetPermissions : IPermissions
	{
		private static HashSet<PermissionObject> _PermissionTable = new HashSet<PermissionObject>();

		public SetPermissions ()
		{
			
		}

		public static void OKResultHandler(Int32 id)
		{
			System.Diagnostics.Debug.WriteLine ("OK id:=" + id);
			var item = _PermissionTable.FirstOrDefault (x => x.ID == id);
			if (item != null) {
				item.Result.SetResult (true);
				_PermissionTable.Remove (item);
			}
		}

		public static void FailedResultHandler(Int32 id)
		{
			System.Diagnostics.Debug.WriteLine ("failed id:=" + id);
			var item = _PermissionTable.FirstOrDefault (x => x.ID == id);
			if (item != null) {
				item.Result.SetResult (false);
				_PermissionTable.Remove (item);
			}
		}

		private Activity MainActivity
		{
			get
			{
				return Xamarin.Forms.Forms.Context as Activity;
			}
		}

		public void HasPermissions(PermissionObject obj)
		{
			if (obj == null)
				return;
			
			bool r;
			if (string.IsNullOrEmpty (obj.PermissionToTest) == false) {
				r = ContextCompat.CheckSelfPermission (MainActivity, obj.PermissionToTest) == (int)Permission.Granted;
				if (r == false) {
					obj.Result.SetResult (false);
					return;
				}
			}
			
			// test (Permissions Test and Add)
			foreach (var p in obj.Permissions) {
				r = ContextCompat.CheckSelfPermission (MainActivity, p) == (int)Permission.Granted;
				if (r == false) {
					obj.Result.SetResult (false);
					return;
				}
			}

			obj.Result.SetResult (true);
		}

		public bool Has(PermissionObject obj)
		{
			if (obj == null)
				return false;

			bool r;
			if (string.IsNullOrEmpty (obj.PermissionToTest) == false) {
				r = ContextCompat.CheckSelfPermission (MainActivity, obj.PermissionToTest) == (int)Permission.Granted;
				if (r == false) {
					return false;
				}
			}

			// test (Permissions Test and Add)
			foreach (var p in obj.Permissions) {
				r = ContextCompat.CheckSelfPermission (MainActivity, p) == (int)Permission.Granted;
				if (r == false) {
					return false;
				}
			}

			return true;
		}

		public void RequestPermissions(PermissionObject obj)
		{
			_PermissionTable.Add (obj);
			if (Has (obj) == false) {
				ActivityCompat.RequestPermissions (MainActivity, obj.Permissions, obj.ID); 
			} else {
				SetPermissions.OKResultHandler(obj.ID);
			}
		}

		public void RequestPermissionsReason(PermissionObject obj)
		{			
			_PermissionTable.Add (obj);

			var c = Xamarin.Forms.Forms.Context;
			if (ContextCompat.CheckSelfPermission (c, obj.PermissionToTest) == (int)Android.Content.PM.Permission.Granted) {
				// granted permission ok
				SetPermissions.OKResultHandler (obj.ID);
			} else {
				if (String.IsNullOrEmpty (obj.Reason) == false) {

					if (obj.AsPopup == false) {
						AlertDialog.Builder builder = new AlertDialog.Builder (Xamarin.Forms.Forms.Context as Activity);
						builder.SetMessage (obj.Reason);
						builder.SetPositiveButton ("OK", (o, x) => {
							ActivityCompat.RequestPermissions (MainActivity, obj.Permissions, obj.ID); 
						});
						builder.SetNegativeButton ("Cancel", (o, x) => {
							SetPermissions.FailedResultHandler (obj.ID);
						});
						builder.Create ();
						builder.Show ();
					} else {
						// snackbar implementation
						Snackbar.Make (GetSnackbarAnchorView (), obj.Reason, 
							(int)TimeSpan.FromSeconds (10).TotalMilliseconds)
							.SetAction ("OK", (x) => {
						})
							.SetCallback (new MySnackBarCallback (obj))
							.Show ();
					}
				} else {
					ActivityCompat.RequestPermissions (MainActivity, obj.Permissions, obj.ID); 
				}
			}
		}

		private class MySnackBarCallback : Snackbar.Callback
		{
			private PermissionObject Parent;
			public MySnackBarCallback(PermissionObject id)
			{
				this.Parent = id;
			}

			public override void OnDismissed (Snackbar snackbar, int evt)
			{
				if (evt == 2) {
					System.Diagnostics.Debug.WriteLine ("cancelled");
					SetPermissions.FailedResultHandler (this.Parent.ID);
				}
				else
				{
					ActivityCompat.RequestPermissions (Xamarin.Forms.Forms.Context as MainActivity, Parent.Permissions, Parent.ID);
				}

				base.OnDismissed (snackbar, evt);
			}
		}

		private Android.Views.View GetSnackbarAnchorView()
		{
			var a = (Activity)Xamarin.Forms.Forms.Context;

			//			var v2 = a.FindViewById(Android.Resource.Id.Content);
			//			var v3 = v2.RootView;
			//			var v =  a.CurrentFocus;
			//			return v;

			var v3 = a.FindViewById(Android.Resource.Id.Content);
			//var v = a.FindViewById(Resource.Id.decor_content_parent);
			//var v2 = a.FindViewById(Resource.Id.concontent);
			return v3;
		}
	}
}

