using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Preferences;

namespace NotificationSample.Droid
{
	public class SharedPrefs 
	{
		private static object locker = new object();

		public static ISharedPreferences DefaultPrefs
		{
			get {
				return PreferenceManager.GetDefaultSharedPreferences (Application.Context);
			}
		}

		public static ISharedPreferences GCMPrefs
		{
			get {
				return Application.Context.GetSharedPreferences ("com.google.android.gcm", FileCreationMode.Private);
			}
		}

		public void Save(string key, string value) 
		{
			lock (locker) {
				var editor = DefaultPrefs.Edit ();
				if (String.IsNullOrEmpty (key) == false) {
					editor.PutString (key, value);
					editor.Apply ();
				}
			}
		}

		public void Save(Dictionary<string, string> values)
		{
			lock (locker) {
				if (values != null) {
					var editor = DefaultPrefs.Edit ();
					foreach (var item in values) {
						if (String.IsNullOrEmpty(item.Key) == false)
						{
							editor.PutString(item.Key, item.Value);
						}
					}
					editor.Apply ();
				}
			}
		}

		public string Get(string key)
		{
			lock (locker) {
				return DefaultPrefs.GetString (key, "");
			}
		}

		public Dictionary<string, string> Get(List<string> keys)
		{
			lock (locker) {
				var item = new Dictionary<string, string> ();
				var p = DefaultPrefs;
				foreach (var k in keys) {
					item.Add (k, p.GetString (k, ""));
				}
				return item;
			}
		}

		public List<string> AllKeys()
		{
			lock (locker) {
				return DefaultPrefs.All.Select (x => x.Key).ToList ();
			}
		}

		public List<string> AllKeys(string filter)
		{
			lock (locker) {
				if (String.IsNullOrEmpty (filter))
					filter = "";

				return DefaultPrefs.All.Where (x => x.Key.Contains (filter)).Select (item => item.Key).ToList ();
			}
		}
	}
}

