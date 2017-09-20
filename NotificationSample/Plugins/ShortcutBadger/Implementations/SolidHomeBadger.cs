using System.Collections.Generic;
using Android.Content;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{
	/**
	 * @author MajeurAndroid
	 */
	internal class SolidHomeBadger : BaseShortcutBadger
	{
		const string IntentUpdateCounter = "com.majeur.launcher.intent.action.UPDATE_BADGE";
		const string Packagename = "com.majeur.launcher.intent.extra.BADGE_PACKAGE";
		const string Count = "com.majeur.launcher.intent.extra.BADGE_COUNT";
		const string Class = "com.majeur.launcher.intent.extra.BADGE_CLASS";

		public SolidHomeBadger(Context context)
			: base(context)
		{
		}

		#region IShortcutBadger implementation

		public override void ExecuteBadge(int badgeCount)
		{
			var intent = new Intent(IntentUpdateCounter);
			intent.PutExtra(Packagename, ContextPackageName);
			intent.PutExtra(Count, badgeCount);
			intent.PutExtra(Class, EntryActivityName);
			_context.SendBroadcast(intent);
		}

		public override IEnumerable<string> SupportLaunchers {
			get { return new[] {"com.majeur.launcher"}; }
		}

		#endregion
	}
}