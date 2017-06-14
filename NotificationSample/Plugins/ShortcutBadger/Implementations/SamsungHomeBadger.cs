using System.Collections.Generic;
using Android.Content;
using Java.Lang;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{

	/**
	 * @author Leo Lin
	 * Deprecated, Samesung devices will use DefaultBadger
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/SamsungHomeBadger.java
	 */
	[Deprecated]
	internal class SamsungHomeBadger : BaseShortcutBadger
	{
		public SamsungHomeBadger(Context context)
			: base(context)
		{
		}

		public override IEnumerable<string> SupportLaunchers
		{
			get { return new List<string>(); }
		}
	}
}