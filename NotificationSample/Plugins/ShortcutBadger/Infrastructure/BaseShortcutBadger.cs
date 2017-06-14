using System;
using System.Collections.Generic;
using Android.Content;

namespace Badges.ShortcutBadger.Infrastructure
{
	public abstract class BaseShortcutBadger : IShortcutBadger
	{
		protected Context _context;

		protected string ContextPackageName
		{
			get
			{
				return _context.PackageName;
			}
		}

		protected string EntryActivityName
		{
			get
			{
				var componentName = _context.PackageManager.GetLaunchIntentForPackage(_context.PackageName).Component;
				return componentName.ClassName;
			}
		}

		protected BaseShortcutBadger(Context context)
		{
			_context = context;
		}

		#region IShortcutBadger implementation

		public virtual void ExecuteBadge(int badgeCount)
		{
			throw new NotImplementedException();
		}

		public virtual IEnumerable<string> SupportLaunchers
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}

