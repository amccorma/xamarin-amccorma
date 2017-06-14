using System.Collections.Generic;
using Android.Content;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{
	/**
	 * @author Gernot Pansy
	 */
	internal class ApexHomeBadger : BaseShortcutBadger
	{
		const string IntentUpdateCounter = "com.anddoes.launcher.COUNTER_CHANGED";
		const string Packagename = "package";
		const string Count = "count";
		const string Class = "class";

		public ApexHomeBadger(Context context)
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

		public override IEnumerable<string> SupportLaunchers
		{
			get
			{
				return new[] { "com.anddoes.launcher" };
			}
		}

		#endregion
	}
}