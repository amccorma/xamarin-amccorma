namespace MyNotification.Plugin.Android
{
	public class MyPlugConstants
	{
		public const string BACKOFF_MS = "backoff_ms";
		public const string GSF_PACKAGE = "com.google.android.gsf";

		public const int DEFAULT_BACKOFF_MS = 3000;
		public const string PROPERTY_REG_ID = "regId";
		public const string PROPERTY_APP_VERSION = "appVersion";
		public const string EXTRA_REGISTRATION_ID = "registration_id";
		public const string EXTRA_ERROR = "error";
		public const string EXTRA_UNREGISTERED = "unregistered";
		public const string ERROR_SERVICE_NOT_AVAILABLE = "SERVICE_NOT_AVAILABLE";

		public const string GCM_Registeration = "com.google.android.c2dm.intent.REGISTRATION";
		public const string GCM_Message = "com.google.android.c2dm.intent.RECEIVE";
		public const string INTENT_FROM_GCM_LIBRARY_RETRY = "com.google.android.gcm.intent.RETRY";
		public const string GCM_BOOTCOMPLETED = "android.intent.action.BOOT_COMPLETED";

		public const string TOKEN = "";
		public const string EXTRA_TOKEN = "token";
		public const int MAX_BACKOFF_MS = 1200000; // 20 minutes
	}
}

