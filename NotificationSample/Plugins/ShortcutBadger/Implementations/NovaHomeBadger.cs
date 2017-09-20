using System;
using System.Collections.Generic;
using Android.Content;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger.Implementations
{
	/**
	 * Shortcut Badger support for Nova Launcher.
	 * TeslaUnread must be installed.
	 * User: Gernot Pansy
	 * Date: 2014/11/03
	 * Time: 7:15
	 */
	internal class NovaHomeBadger : BaseShortcutBadger
	{
		const string ContentUri = "content://com.teslacoilsw.notifier/unread_count";
		const string Count = "count";
		const string Tag = "tag";
		const string IntentTagValueFormat = "{0}/{1}";

		public NovaHomeBadger(Context context)
			: base(context)
		{
		}

		#region IShortcutBadger implementation

		public override void ExecuteBadge(int badgeCount)
		{
			try
			{
				ContentValues contentValues = new ContentValues();
				contentValues.Put(Tag, string.Format(IntentTagValueFormat, ContextPackageName, EntryActivityName));
				contentValues.Put(Count, badgeCount);
				_context.ContentResolver.Insert(Android.Net.Uri.Parse(ContentUri), contentValues);
			}
			catch (Java.Lang.IllegalArgumentException)
			{
				/* Fine, TeslaUnread is not installed. */
			}
			catch (Exception ex)
			{
				/* Some other error, possibly because the format
				of the ContentValues are incorrect. */

				throw new ShortcutBadgeException(ex.Message);
			}
		}

		public override IEnumerable<string> SupportLaunchers
		{
			get { return new[] { "com.teslacoilsw.launcher" }; }
		}

		#endregion
	}
}