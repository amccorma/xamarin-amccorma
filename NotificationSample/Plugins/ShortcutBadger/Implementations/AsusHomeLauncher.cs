using System.Collections.Generic;
using Android.Content;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{
	/**
	 * @author leolin
	 */
	internal class AsusHomeLauncher : BaseShortcutBadger
	{
		const string IntentAction = "android.intent.action.BADGE_COUNT_UPDATE";
		const string IntentExtraBadgeCount = "badge_count";
		const string IntentExtraPackagename = "badge_count_package_name";
		const string IntentExtraActivityName = "badge_count_class_name";
		const string IntentExtraBadgeVipCount = "badge_vip_count";

		public AsusHomeLauncher(Context context)
			: base(context)
		{
		}

		#region IShortcutBadger implementation

		public override void ExecuteBadge(int badgeCount)
		{
			Intent intent = new Intent(IntentAction);
			intent.PutExtra(IntentExtraBadgeCount, badgeCount);
			intent.PutExtra(IntentExtraPackagename, ContextPackageName);
			intent.PutExtra(IntentExtraActivityName, EntryActivityName);
			intent.PutExtra(IntentExtraBadgeVipCount, 0);
			_context.SendBroadcast(intent);
		}

		public override IEnumerable<string> SupportLaunchers
		{
			get { return new[] {"com.asus.launcher"}; }
		}

		#endregion
	}
}