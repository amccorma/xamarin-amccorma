using System;
using Android.App;
using Android.Content;

namespace MyNotification.Plugin.Android
{
	/// <summary>
	/// Cross platform PushNotification implemenations
	/// </summary>
	public class MyPushNotification
	{
    	public static bool IsInitialized { get { return (PushNotificationListener != null);  } }
		public static IPushListener PushNotificationListener { get; set; }
		static Lazy<IPushNotification> Implementation = new Lazy<IPushNotification>(() => CreatePushNotification(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

		public static string SenderId { get; set; }

	    public static string NotificationTokenKey { get; set; }
	    public static string NotificationAppVersionKey { get; set; }
		public static string NotificationTagKey { get; set; }
		public static bool NotificationError { get; set; }

		public static void Initialize<T>(T listener, string senderId = "") where T : IPushListener
		{
			SenderId = senderId;

			NotificationTagKey = String.IsNullOrEmpty (NotificationTagKey) ? "MyPushTag" : NotificationTagKey;
			NotificationTokenKey = String.IsNullOrEmpty (NotificationTokenKey) ? "MyNotifyTag" : NotificationTokenKey;

			if (PushNotificationListener == null)
			{
				PushNotificationListener = listener;
			}
		}

		public static IPushNotification Current
		{
			get
			{
				//Should always initialize plugin before use
				if (!MyPushNotification.IsInitialized)
				{
					throw NewPushNotificationNotInitializedException();
				}

				var ret = Implementation.Value;
				if (ret == null)
				{
					throw NotImplementedInReferenceAssembly();
				}
				return ret;
			}
		}

		static IPushNotification CreatePushNotification()
		{
			return new PushNotificationImpl();
		}

		public static void Initialize<T>(string senderId) where T : IPushListener, new()
	    {
	      	Initialize<T>(new T(), senderId);
	    }
			

		public static ISharedPreferences GCMPrefs
		{
			get {
				return Application.Context.GetSharedPreferences ("com.google.android.gcm", FileCreationMode.Private);
			}
		}

		public static Exception NotImplementedInReferenceAssembly()
		{
			return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
		}

		public static PushNotificationNotInitializedException NewPushNotificationNotInitializedException()
		{
			string description = "CrossPushNotification Plugin is not initialized. Should initialize before use with CrossPushNotification Initialize method. Example:  CrossPushNotification.Initialize<CrossPushNotificationListener>()";
			return new PushNotificationNotInitializedException(description);
		}
    }
}
