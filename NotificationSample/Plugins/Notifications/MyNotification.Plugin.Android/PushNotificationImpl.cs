using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;

namespace MyNotification.Plugin.Android
{
	[global::Android.Runtime.Preserve(AllMembers=true)]
	public class PushNotificationImpl : IPushNotification
	{
		public void Register ()
		{
			Task.Run(() => {
				var context = Application.Context;
				Intent intent = new Intent ("com.google.android.c2dm.intent.REGISTER");
				intent.SetPackage ("com.google.android.gsf");
				intent.PutExtra ("app", PendingIntent.GetBroadcast (context, 0, new Intent (), 0));
				intent.PutExtra ("sender", MyPushNotification.SenderId); // string.Join (",", senderID));
				context.StartService (intent);
			});
		}

		public void Unregister ()
		{
			//System.Diagnostics.Debug.WriteLine("{0} - Unregister -  Unregistering push notifications");
			var context = Application.Context;
			Intent intent = new Intent("com.google.android.c2dm.intent.UNREGISTER");
			intent.PutExtra("app", PendingIntent.GetBroadcast (context, 0, new Intent (), 0));
			context.StartService (intent);
		}

		public string Token {
			get {
				string retVal = "";
				Context context = global::Android.App.Application.Context;
				ISharedPreferences prefs = MyPushNotification.GCMPrefs;
				string registrationId = prefs.GetString(MyPushNotification.NotificationTokenKey, string.Empty);
				if (string.IsNullOrEmpty(registrationId))
				{
					return retVal;
				}

				return registrationId;
			}
		}

		public void SaveToken(string token)
		{
			ISharedPreferences prefs = MyPushNotification.GCMPrefs;
			var edit = prefs.Edit ();
			edit.PutString (MyPushNotification.NotificationTokenKey, token);
			edit.Commit ();
		}

		internal static int GetAppVersion(Context context)
		{
			try
			{
				PackageInfo packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
				return packageInfo.VersionCode;
			}
			catch (PackageManager.NameNotFoundException e)
			{
				// should never happen
				throw new Java.Lang.RuntimeException("Could not get package name: " + e);
			}
		}

		public string GetToken()
		{
			ISharedPreferences prefs = MyPushNotification.GCMPrefs;
			return prefs.GetString(MyPushNotification.NotificationTokenKey, "");
		}
	}
}

