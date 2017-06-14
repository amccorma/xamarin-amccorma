
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace NotificationSample.Droid
{
	// TODO: required permissions set in Manifest.  All other permission be added automatically
	/*
		<uses-permission android:name="android.permission.BATTERY_STATS" />  
		<uses-permission android:name="android.permission.BIND_NOTIFICATION_LISTENER_SERVICE" />
		<uses-permission android:name="android.permission.RECEIVE_BOOT_COMPLETED" />
		<uses-permission android:name="android.permission.WAKE_LOCK" />

		<!-- Samsung Badges -->
		<uses-permission android:name="com.sec.android.provider.badge.permission.READ" />
		<uses-permission android:name="com.sec.android.provider.badge.permission.WRITE" />
		<!-- Sony Badges -->
		<uses-permission android:name="com.sonyericsson.home.permission.BROADCAST_BADGE" />
		<!-- HTC Badges -->
		<uses-permission android:name="com.htc.launcher.permission.UPDATE_SHORTCUT" />
		<uses-permission android:name="com.htc.launcher.permission.READ_SETTINGS" />
	*/
	[Activity(Label = "NotificationSample.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			// TODO: Parse Intent
			bool isFromNotification = fromRemoteNotification(this.Intent);

			// TODO: register for Notifications
			RegisterForNotifications();

			// TODO: handle the notification click
			LoadApplication(new App());
		}

		protected internal void RegisterForNotifications()
		{
			new NotificationActions().Register();
		}

		public bool fromRemoteNotification(Intent intent)
		{
			if (intent == null)
				return false;

			// local notification have no action in version 1.0.  
			// local notification to alert user of in application change
			var notify = intent.GetStringExtra(NotificationActions.notificationIntentKey);
			if (string.IsNullOrEmpty(notify) == false)
			{
				// TODO: parse any intent values set in NotificationActions;
				return true;
			}
			return false;
		}
	}
}
