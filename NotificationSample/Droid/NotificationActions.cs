using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using MyNotification.Plugin.Android;

namespace NotificationSample.Droid
{
	public class NotificationActions 
	{
		private static object _NIDLocker = new object();
		private bool _IsRegisteringNow;
		private const String CHECK_OP_NO_THROW = "checkOpNoThrow";
		private const String OP_POST_NOTIFICATION = "OP_POST_NOTIFICATION";

		// TODO: this key must be unique
		public const string notificationIntentKey = "mynotify";
			
		private bool _isRunningNotificationRegister = false;

		private bool IsRunning
		{
			get
			{
				return MainApplication.getIsRunning();
			}
		}


		public NotificationActions ()
		{
		}


		private bool IsUnRegistered()
		{
			return String.IsNullOrEmpty (getNotificationToken()) == true;
		}

        public bool getHasRegisterationID()
        {
            var token = getNotificationToken();
            return String.IsNullOrEmpty(token) == false;
        }

		public void Register ()
		{
            if (this.IsRegisteringNow == false)
            {
                IsRegisteringNow = true;
                MyPushNotification.NotificationError = false;
                MyPushNotification.Current.Register();
            }
		}

		public bool IsRegisteringNow
		{
			get
			{
				return this._IsRegisteringNow;
			}
			set
			{
				this._IsRegisteringNow = value;
			}
		}

		/// <summary>
		/// Deletes the notification from the screen and database
		/// </summary>
		/// <param name="notificationID">NotificationID.</param>
		public void Cancel(int notificationID)
		{
			try
			{
				// TODO: handle Cancel or keep
				setBadgeNumber(0);
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}
		}

		/// <summary>
		/// Cancel all shown Notifications
		/// </summary>
		public void CancelAll()
		{
			try
			{
				// TODO: handle Cancel ALL or keep
				var notificationManager = GlobalSettings.GetService<NotificationManager>(Context.NotificationService);
				notificationManager.CancelAll();
				setBadgeNumber(0);

				notificationManager.Dispose();
				notificationManager = null;
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}
		}

        public Task<bool> UnRegisterToService(string registerationID)
        {
            return Task.Run<bool>(() =>
            {
                return true;
            });
        }

		public string getNotificationToken()
		{
			return MyNotification.Plugin.Android.MyPushNotification.Current.GetToken();
		}


		public void CreateRemoteNotification(string message, string title)
		{
            var context = Application.Context;
            var builder = new NotificationCompat.Builder(context);
            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            var options = new Bundle();

			// TODO: change getNotificationID or keep ID system
            var id = GetNotificationID();


            // TODO: set drawable icon
            //builder.SetSmallIcon(Resource.Drawable.notificationicon1);
            builder.SetAutoCancel(true);


            builder.SetContentTitle(title);
            builder.SetContentText(message);

            //textStyle.SetSummaryText (data.SummaryText);
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                // TODO: keep notification style or change
                var style = new NotificationCompat.BigTextStyle();
                style.BigText(message);
                builder.SetStyle(style);
            }
            
			options.PutString(notificationIntentKey, "1" );
			// TODO: Any any Extra Intents
            //options.PutString("", "");

            Intent resultIntent = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
            if (options != null) { resultIntent.PutExtras(options); }

            const int pendingIntentId = 0;
            PendingIntent resultPendingIntent = PendingIntent.GetActivity(context, pendingIntentId, resultIntent, PendingIntentFlags.OneShot);
            builder.SetContentIntent(resultPendingIntent);

            notificationManager.Notify(id, builder.Build());

            if (notificationManager != null)
            {
                notificationManager.Dispose();
            }

            notificationManager = null;
		}

		public bool RegisterError
		{
			get
			{
				return MyPushNotification.NotificationError;
			}
		}

		public void HandleRegisterError(string message, string deviceOS)
		{
			HandleExceptions(new Exception(message));
		}

		private void HandleExceptions(Exception ex)
		{
			// TODO: Handle any exceptions
		}

		private Int32 GetNotificationID()
		{
			// TODO: change or use for unique Notification IDs
			lock (_NIDLocker)
			{
				var pref = new SharedPrefs();
				var id = pref.Get("lastnID");
				Int32 idx = 0;
				if (Int32.TryParse(id, out idx))
				{
					idx = idx + 1;
				}
				else
				{
					idx = 1;
				}
				return idx;
			}
		}

		public bool RegisterToService(string registrationID)
		{
			// TODO: Save the registrationID
			return true;
		}

		public Task<bool> checkIfNotificationsSupported()
		{
			return Task.Run(() =>
			{
				var context = GlobalSettings.GetContext;
				AppOpsManager mAppOps = (AppOpsManager)context.GetSystemService(global::Android.Content.Context.AppOpsService);
				ApplicationInfo appInfo = context.ApplicationInfo;
				String pkg = context.ApplicationContext.PackageName;
				int uid = appInfo.Uid;
				try
				{

					var appOpsClass = Java.Lang.Class.ForName("android.app.AppOpsManager");
					var checkOpNoThrowMethod = appOpsClass.GetMethod(CHECK_OP_NO_THROW, Java.Lang.Integer.Type, Java.Lang.Integer.Type, new Java.Lang.String().Class);//need to add String.Type

					var opPostNotificationValue = appOpsClass.GetDeclaredField(OP_POST_NOTIFICATION);
					var value = (int)opPostNotificationValue.GetInt(Java.Lang.Integer.Type);
					var mode = (int)checkOpNoThrowMethod.Invoke(mAppOps, value, uid, pkg);
					return (mode == (int)AppOpsManagerMode.Allowed);

				}
				catch (Exception)
				{
					System.Diagnostics.Debug.WriteLine("Notification services is off or not supported");
					return false;
				}
			});
		}

		/// <summary>
		/// Updates the badge number.
		/// </summary>
		/// <param name="count">New Count.</param>
		public void setBadgeNumber(int count)
		{
			// TODO: Handle update Android Badge numbers 
			// remove method if not using Badge numbers
			Badges.ShortcutBadger.ShortcutBadger.SetBadge(GlobalSettings.GetContext, count);
		}
	}
}

