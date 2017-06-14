using System.Collections.Generic;
using Android.Content;
using Java.Lang;
using Java.Lang.Reflect;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{
	/**
	 * @author leolin
	 */
	internal class XiaomiHomeBadger : BaseShortcutBadger
	{
		const string IntentAction = "android.intent.action.APPLICATION_MESSAGE_UPDATE";
		const string ExtraUpdateAppComponentName = "android.intent.extra.update_application_component_name";
		const string ExtraUpdateAppMsgText = "android.intent.extra.update_application_message_text";

		public XiaomiHomeBadger(Context context)
			: base(context)
		{
		}

		#region IShortcutBadger implementation

		public override void ExecuteBadge(int badgeCount)
		{
			try
			{
				Class miuiNotificationClass = Class.ForName("android.app.MiuiNotification");
				Object miuiNotification = miuiNotificationClass.NewInstance();
				Field field = miuiNotificationClass.GetDeclaredField("messageCount");
				field.Accessible = true;
				field.Set(miuiNotification, String.ValueOf(badgeCount == 0 ? "" : badgeCount.ToString()));
			}
			catch (Exception)
			{
				Intent localIntent = new Intent(IntentAction);
				localIntent.PutExtra(ExtraUpdateAppComponentName, ContextPackageName + "/" + EntryActivityName);
				localIntent.PutExtra(ExtraUpdateAppMsgText, String.ValueOf(badgeCount == 0 ? "" : badgeCount.ToString()));
				_context.SendBroadcast(localIntent);
			}
		}

		public override IEnumerable<string> SupportLaunchers
		{
			get
			{
				return new[]
				{
					"com.miui.miuilite",
					"com.miui.home",
					"com.miui.miuihome",
					"com.miui.miuihome2",
					"com.miui.mihome",
					"com.miui.mihome2"
				};
			}
		}

		#endregion
	}
}