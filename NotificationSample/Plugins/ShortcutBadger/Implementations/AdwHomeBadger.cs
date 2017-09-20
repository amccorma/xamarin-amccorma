using System.Collections.Generic;
using Android.Content;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{
	/**
	 * @author Gernot Pansy
	 */
	internal class AdwHomeBadger : BaseShortcutBadger
	{
		const string IntentUpdateCounter = "org.adw.launcher.counter.SEND";
		const string Packagename = "PNAME";
		const string Count = "Count";

		public AdwHomeBadger(Context context)
			: base(context)
		{
		}

		#region IShortcutBadger implementation

		public override void ExecuteBadge(int badgeCount)
		{
			var intent = new Intent(IntentUpdateCounter);
			intent.PutExtra(Packagename, ContextPackageName);
			intent.PutExtra(Count, badgeCount);
			_context.SendBroadcast(intent);
		}

		public override IEnumerable<string> SupportLaunchers
		{
			get
			{
				return new []
				{
					"org.adw.launcher",
					"org.adwfreak.launcher"
				};
			}
		}

		#endregion
	}
}