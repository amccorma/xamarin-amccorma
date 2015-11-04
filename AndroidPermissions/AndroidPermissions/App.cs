using System;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace AndroidPermissions
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							XAlign = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						},
						new Button
						{
							Text = "Permissions",
							Command = new Command( async() => {
								var o = new PermissionObject();
//								o.Permissions = new string[] {
//									"android.permission.ACCESS_COARSE_LOCATION",
//									"android.permission.ACCESS_FINE_LOCATION"
//								};
//
//								o.PermissionToTest = "android.permission.ACCESS_COARSE_LOCATION";
//
//								DependencyService.Get<IPermissions>().RequestPermissions(o);
//
//								await o.Result.Task;
//								System.Diagnostics.Debug.WriteLine("after call");

								o.Permissions = new string[] {
									"android.permission.RECORD_AUDIO"
								};

								o.PermissionToTest = "android.permission.RECORD_AUDIO";
								o.Reason = "Access Contacts/Allow Access";
								o.AsPopup = true;
								DependencyService.Get<IPermissions>().RequestPermissionsReason(o);

								await o.Result.Task;
								System.Diagnostics.Debug.WriteLine("after call");
							})
						}
					}
				}
			};
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

