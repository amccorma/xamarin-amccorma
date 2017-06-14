using System;

namespace PushNotification.Plugin
{
	/// <summary>
	/// Cross platform PushNotification implemenations
	/// </summary>
	public class CrossPushNotification
	{
		static Lazy<IPushNotification> Implementation = new Lazy<IPushNotification>(() => CreatePushNotification(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
		public static bool IsInitialized { get { return (PushNotificationListener != null); } }
		public static IPushListener PushNotificationListener { get; private set; }

		public static string SenderId { get; set; }
		public static string NotificationTokenKey { get; set; }
		public static string NotificationAppVersionKey { get; set; }
		public static string NotificationTagKey { get; set; }
		public static bool NotificationError { get; set; }

		/// <summary>
		/// Android Version
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <param name="senderId">Android Google Sender  ID #</param>
		/// <typeparam name="T">IPushListener</typeparam>
		public static void Initialize<T>(T listener, string senderId = "") where T : IPushListener
		{
			SenderId = senderId;

			NotificationTokenKey = "NTokenKey";
			NotificationAppVersionKey = "AppVKey";

			if (PushNotificationListener == null)
			{
				PushNotificationListener = listener;
				//Debug.WriteLine("PushNotification plugin initialized.");
			}
			else
			{
				//Debug.WriteLine("PushNotification plugin already initialized.");
			}
		}

		/// <summary>
		/// iOS Version
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <typeparam name="T">IPushListener</typeparam>
		public static void Initialize<T>(T listener) where T : IPushListener
		{
			SenderId = "";

			NotificationTokenKey = "NTokenKey";
			NotificationAppVersionKey = "AppVKey";

			if (PushNotificationListener == null)
			{
				PushNotificationListener = listener;
			}
		}

		/// <summary>
		/// Android Version
		/// </summary>
		/// <param name="senderId">Android Google Sender  ID #</param>
		/// <typeparam name="T">IPushListener</typeparam>
		public static void Initialize<T>(string senderId) where T : IPushListener, new()
		{
			Initialize<T>(new T(), senderId);
		}

		/// <summary>
		/// iOS Version
		/// </summary>
		/// <typeparam name="T">IPushListener</typeparam>
		public static void Initialize<T>() where T : IPushListener, new()
		{
			Initialize<T>(new T());
		}

		/// <summary>
		/// Current settings to use
		/// </summary>
		public static IPushNotification Current
		{
			get
			{
				//Should always initialize plugin before use
				if (!CrossPushNotification.IsInitialized)
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

		public static IPushListener Listener
		{
			get
			{
				return PushNotificationListener;
			}
		}

		static IPushNotification CreatePushNotification()
		{
#if PORTABLE
			   return null;
#else
			return new PushNotificationImplementation();
#endif
		}

		internal static Exception NotImplementedInReferenceAssembly()
		{
			return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
		}

		internal static PushNotificationNotInitializedException NewPushNotificationNotInitializedException()
		{
			string description = "CrossPushNotification Plugin is not initialized. Should initialize before use with CrossPushNotification Initialize method. Example:  CrossPushNotification.Initialize<CrossPushNotificationListener>()";
			return new PushNotificationNotInitializedException(description);
		}

	}
}
