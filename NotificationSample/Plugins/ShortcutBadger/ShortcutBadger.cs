using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Badges.ShortcutBadger.Implementations;
using Badges.ShortcutBadger.Infrastructure;

namespace Badges.ShortcutBadger
{
	public static class ShortcutBadger
	{
		//https://github.com/wcoder/ShortcutBadger
		const string LogTag = "ShortcutBadger";

		private static IShortcutBadger _mShortcutBadger;

		private static IEnumerable<Type> _badgeImplementations = new List<Type>
		{
			typeof(AdwHomeBadger),
			typeof(ApexHomeBadger),
			typeof(AsusHomeLauncher),
			typeof(DefaultBadger),
			typeof(LgHomeBadger),
			typeof(NewHtcHomeBadger),
			typeof(NovaHomeBadger),
			typeof(SamsungHomeBadger),
			typeof(SolidHomeBadger),
			typeof(SonyHomeBadger),
			typeof(XiaomiHomeBadger)
		};

		public static void SetBadge(Context context, int badgeCount)
		{
			try
			{
				GetShortcutBadger(context).ExecuteBadge(badgeCount);
			}
			catch (Exception e)
			{
				throw new ShortcutBadgeException("Unable to execute badge:" + e.Message);
			}
		}

		private static IShortcutBadger GetShortcutBadger(Context context)
		{
			if (_mShortcutBadger != null)
			{
				return _mShortcutBadger;
			}

			Log.Debug(LogTag, "Finding badger");

			//find the home launcher Package
			try
			{
				var intent = new Intent(Intent.ActionMain);
				intent.AddCategory(Intent.CategoryHome);
				var resolveInfo = context.PackageManager.ResolveActivity(intent, PackageInfoFlags.MatchDefaultOnly);
				var currentHomePackage = resolveInfo.ActivityInfo.PackageName;

				if (Build.Manufacturer.Equals("xiaomi"))
				{
					_mShortcutBadger = new XiaomiHomeBadger(context);
					return _mShortcutBadger;
				}

				foreach (var badgeImplementation in _badgeImplementations)
				{
					var instanse = (IShortcutBadger) Activator.CreateInstance(badgeImplementation, context);
					if (instanse.SupportLaunchers.Contains(currentHomePackage))
					{
						_mShortcutBadger = instanse;
						break;
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(LogTag, e.Message, e);
			}

			if (_mShortcutBadger == null)
			{
				_mShortcutBadger = new DefaultBadger(context);
			}

			Log.Debug(LogTag, "Returning badger:" + _mShortcutBadger.GetType());
			return _mShortcutBadger;
		}
	}
}

