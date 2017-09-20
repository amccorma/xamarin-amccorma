using System.Collections.Generic;
using Android.Content;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{
	/**
	 * @author Leo Lin
	 */
	internal class NewHtcHomeBadger : BaseShortcutBadger
	{
		const string IntentUpdateShortcut = "com.htc.launcher.action.UPDATE_SHORTCUT";
		const string IntentSetNotification = "com.htc.launcher.action.SET_NOTIFICATION";
		const string Packagename = "packagename";
		const string Count = "count";
		const string ExtraComponent = "com.htc.launcher.extra.COMPONENT";
		const string ExtraCount = "com.htc.launcher.extra.Count";

		public NewHtcHomeBadger(Context context)
			: base(context)
		{
		}

		#region IShortcutBadger implementation

		public override void ExecuteBadge(int badgeCount)
		{
			var intent1 = new Intent(IntentSetNotification);
			var localComponentName = new ComponentName(ContextPackageName, EntryActivityName);
			intent1.PutExtra(ExtraComponent, localComponentName.FlattenToShortString());
			intent1.PutExtra(ExtraCount, badgeCount);
			_context.SendBroadcast(intent1);

			var intent = new Intent(IntentUpdateShortcut);
			intent.PutExtra(Packagename, ContextPackageName);
			intent.PutExtra(Count, badgeCount);
			_context.SendBroadcast(intent);
		}

		public override IEnumerable<string> SupportLaunchers
		{
			get { return new[] { "com.htc.launcher" }; }
		}

		#endregion
	}
}