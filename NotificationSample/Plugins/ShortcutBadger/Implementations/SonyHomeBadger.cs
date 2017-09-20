using System.Collections.Generic;
using Android.Content;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{
	/**
	 * @author Leo Lin
	 */
	internal class SonyHomeBadger : BaseShortcutBadger
	{
		const string IntentAction = "com.sonyericsson.home.action.UPDATE_BADGE";
		const string IntentExtraPackageName = "com.sonyericsson.home.intent.extra.badge.PACKAGE_NAME";
		const string IntentExtraActivityName = "com.sonyericsson.home.intent.extra.badge.ACTIVITY_NAME";
		const string IntentExtraMessage = "com.sonyericsson.home.intent.extra.badge.MESSAGE";
		const string IntentExtraShowMessage = "com.sonyericsson.home.intent.extra.badge.SHOW_MESSAGE";

		public SonyHomeBadger(Context context) : base(context)
		{
		}

		#region IShortcutBadger implementation

		public override void ExecuteBadge(int badgeCount)
		{
			var intent = new Intent(IntentAction);
			intent.PutExtra(IntentExtraPackageName, ContextPackageName);
			intent.PutExtra(IntentExtraActivityName, EntryActivityName);
			intent.PutExtra(IntentExtraMessage, badgeCount.ToString());
			intent.PutExtra(IntentExtraShowMessage, badgeCount > 0);
			_context.SendBroadcast(intent);
		}

		public override IEnumerable<string> SupportLaunchers
		{
			get { return new[] { "com.sonyericsson.home" }; }
		}

		#endregion
	}
}