using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using MyNotification.Plugin.Android;

namespace NotificationSample.Droid
{
	[Android.App.Application]
    public class MainApplication : Android.App.Application, Android.App.Application.IActivityLifecycleCallbacks
    {
        public static bool IsInit = false;

		// TODO: Update Key Names
		// key names
		protected static string TokenKey = "regIDKey";
		protected static string VersionKey = "verIDKey";
		protected internal static string appStateKey = "appStateKey";
		protected internal static string appPausedValue = "paused";
		protected internal static string appResumeValue = "resume";
		protected internal static string appStatedValue = "start";
		protected internal static string appstopValue = "stop";

        /// <summary>
        /// set to an object in MainActivity. If Application is closed, it will be null
        /// </summary>
        public static object RunningApp;

        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
			// TODO: set Values
			// set the application tag ID. 
			// used for logging
            MyPushNotification.NotificationTagKey = "MyApp";
			// saved identifier key name. Anything you like.
            MyPushNotification.NotificationTokenKey = TokenKey;
			// saved version key name. Anything you like.
            MyPushNotification.NotificationAppVersionKey = VersionKey;

			// TODO: set google Project ID
			// set google project ID from
			// https://console.developers.google.com/
            MyPushNotification.Initialize<PushNotificationListener>("1111111111");
        }

        /// <summary>
        /// Is Application Running
        /// </summary>
        /// <value><c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
        protected internal static bool getIsRunning()
        {
            return RunningApp != null;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            this.RegisterActivityLifecycleCallbacks(this);
        }

        /// <summary>
        /// set Badge number equal to alert messages unread count
        /// </summary>
        protected internal void setApplicationEventBadge()
        {
			// TODO: set count or do not use Badges
			Int32 count = 10;
            Badges.ShortcutBadger.ShortcutBadger.SetBadge(GlobalSettings.GetContext, count);
        }

        public void OnActivityCreated(Android.App.Activity activity, Android.OS.Bundle savedInstanceState)
        {           
            if (IsInit == false)
            {
                MainApplication.RunningApp = new object();

				IsInit = true;
                //AppContext = this.ApplicationContext;
                StartPushService();
            }
        }

        public static void StartPushService()
        {
            var context = GlobalSettings.GetContext;
            context.StartService(new Intent(context, typeof(PushNotificationService)));

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                PendingIntent pintent = PendingIntent.GetService(context, 0, new Intent(context, typeof(PushNotificationService)), 0);
                AlarmManager alarm = (AlarmManager)context.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }

        public static void StopPushService()
        {
            var context = GlobalSettings.GetContext;
            context.StopService(new Intent(GlobalSettings.GetContext, typeof(PushNotificationService)));

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                PendingIntent pintent = PendingIntent.GetService(GlobalSettings.GetContext, 0, new Intent(context, typeof(PushNotificationService)), 0);
                AlarmManager alarm = (AlarmManager)context.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }

        public void OnActivityDestroyed(Android.App.Activity activity)
        {
            if (activity is MainActivity)
            {
                setApplicationEventBadge();
            }
        }

        public void OnActivityPaused(Android.App.Activity activity)
        {
			if (activity is MainActivity)
            {
				new SharedPrefs().Save(appStateKey, appPausedValue);
            }
            setApplicationEventBadge();
        }

        public void OnActivityStopped(Android.App.Activity activity)
        {
			if (activity is MainActivity)
            {
				new SharedPrefs().Save(appStateKey, appstopValue);
            }
            setApplicationEventBadge();
        }

        public override void OnTerminate()
        {
            MainApplication.RunningApp = null;
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

		public void OnActivityResumed(Activity activity)
		{
            if (activity is MainActivity)
            {
				new SharedPrefs().Save(appStateKey, appResumeValue);
            }
		}

		public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
		{
		}

		public void OnActivityStarted(Activity activity)
		{
			if (activity is MainActivity)
            {
				new SharedPrefs().Save(appStateKey, appStatedValue);
            }
		}
	}
}

