using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;

namespace NotificationSample.iOS
{
	public class SharedPrefs 
	{
		private static object Locker = new object();

		public bool InServiceOnly
		{
			get {
				return false;
			}
		}

		public static NSUserDefaults DefaultPrefs
		{
			get {
				return NSUserDefaults.StandardUserDefaults;
			}
		}

		public void Save(string key, string value) 
		{
			lock (Locker) {
				if (String.IsNullOrEmpty(key) == false)
				{
					DefaultPrefs.SetString(value ?? "", key);
					DefaultPrefs.Synchronize();
				}
			}
		}

		public void Save(Dictionary<string, string> values)
		{
			lock (Locker) {
				if (values != null) {
					var editor = DefaultPrefs;
					foreach (var item in values) {
						editor.SetString(item.Value?? "", item.Key);
					}
					editor.Synchronize ();
				}
			}
		}

		public string Get(string key)
		{
			lock (Locker) {
				try
				{
					return DefaultPrefs.StringForKey (key);
				}
				catch(Exception) {
					return String.Empty;
				}
			}
		}

		public Dictionary<string, string> Get(List<string> keys)
		{
			lock (Locker) {
				var item = new Dictionary<string, string> ();
				var p = DefaultPrefs;
				foreach (var k in keys) {
					item.Add (k, p.StringForKey (k));
				}
				return item;
			}
		}

		public List<string> AllKeys()
		{
			lock (Locker) {
				return DefaultPrefs.ToDictionary ().Keys.Select (x => x.ToString ()).ToList ();
			}
			
		}

		public List<string> AllKeys(string filter)
		{
			lock (Locker) {
				if (String.IsNullOrEmpty (filter))
					filter = "";

				return AllKeys ().Where (x => x.Contains (filter)).Select (item => item).ToList ();
			}
		}
	}
}

