using Android.App;
using Android.Content;
using Android.OS;

namespace MyNotification.Plugin.Android
{
	[Service]
    public class PushNotificationService : Service
    {
        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}