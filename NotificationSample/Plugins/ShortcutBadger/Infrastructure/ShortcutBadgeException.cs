using System;

namespace Badges.ShortcutBadger.Infrastructure
{
	public class ShortcutBadgeException : Exception
	{
		public ShortcutBadgeException(string message)
			: base(message)
		{
		}
	}
}

