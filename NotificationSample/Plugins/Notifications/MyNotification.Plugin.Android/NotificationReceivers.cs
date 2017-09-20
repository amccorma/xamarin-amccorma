using Android.App;
using Android.Content;
using Android.Runtime;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
namespace MyNotification.Plugin.Android
{
    [BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND")]
	[IntentFilter(new string[] { "com.google.android.c2dm.intent.RECEIVE" }, Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { "com.google.android.gcm.intent.RETRY" }, Categories = new string[] { "@PACKAGE_NAME@" })]
	public class NotificationReceiver : BroadcastReceiver
	{

		public override void OnReceive (Context context, Intent intent)
		{
			MyNotificationService.RunIntentInService(context, intent);
			SetResult(Result.Ok, null, null);
		}
	}

	[BroadcastReceiver]
	[Register("mynotification.plugin.android.NotificationBootReceiver")]
	[IntentFilter (new[]{Intent.ActionBootCompleted}, Categories=new[]{"android.intent.category.HOME"})]
	public class NotificationBootReceiver : BroadcastReceiver  // 
	{
		public override void OnReceive(Context context, Intent intent)
		{
			MyNotificationService.RunIntentInService(context, intent);
			SetResult(Result.Ok, null, null);
		}
	}
}

