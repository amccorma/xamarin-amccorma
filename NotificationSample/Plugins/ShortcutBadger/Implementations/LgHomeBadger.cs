using System.Collections.Generic;
using Android.Content;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{
	/**
	 * @author Leo Lin
	 * Deprecated, LG devices will use DefaultBadger
	 */
	internal class LgHomeBadger : BaseShortcutBadger
	{
		const string IntentAction = "android.intent.action.BADGE_COUNT_UPDATE";
		const string IntentExtraBadgeCount = "badge_count";
		const string IntentExtraPackagename = "badge_count_package_name";
		const string IntentExtraActivityName = "badge_count_class_name";

		public LgHomeBadger(Context context)
			: base(context)
		{
		}

		#region IShortcutBadger implementation

		public override void ExecuteBadge(int badgeCount)
		{
			var intent = new Intent(IntentAction);
			intent.PutExtra(IntentExtraBadgeCount, badgeCount);
			intent.PutExtra(IntentExtraPackagename, ContextPackageName);
			intent.PutExtra(IntentExtraActivityName, EntryActivityName);
			_context.SendBroadcast(intent);
		}

		public override IEnumerable<string> SupportLaunchers
		{
			get
			{
				return new[]
				{
					"com.lge.launcher",
					"com.lge.launcher2"
				};
			}
		}

		#endregion
	}
}