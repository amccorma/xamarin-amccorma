using System.Collections.Generic;

namespace Badges.ShortcutBadger.Infrastructure
{
	public interface IShortcutBadger
	{
		IEnumerable<string> SupportLaunchers { get; }

		void ExecuteBadge(int badgeCount);
	}
}

