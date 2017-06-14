using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace NotificationSample.Droid
{
	public class GlobalSettings {

		public static bool getIsAppForeground()
		{
			bool result = IsAppActive;
			if (result)
			{
				string val = new SharedPrefs().Get(MainApplication.appStateKey);
				if (String.IsNullOrEmpty(val)) return true;

				if (val == MainApplication.appPausedValue || val == MainApplication.appstopValue)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			return false;
		}

		public static void RequestMainThread(Action action) {
			if (Android.App.Application.SynchronizationContext == SynchronizationContext.Current)
				action();
			else
				Android.App.Application.SynchronizationContext.Post(x => {
					try {
						action();
					}
					catch { }
				}, null);
		}

		public static async System.Threading.Tasks.Task InvokeOnMainThreadAndWait(Action act)
		{
			await System.Threading.Tasks.Task.Run (() => {
				using (var evt = new System.Threading.ManualResetEvent (false)) {
					(GlobalSettings.GetActivity).RunOnUiThread(()=> {
						if (act != null)
						{
							act.Invoke();
						}
						evt.Set();
					});

					evt.WaitOne();
				}
			});
		}

		public static void PostOnMainThread(Action act)
		{
			Android.App.Application.SynchronizationContext.Post ((x) => act.Invoke(), null);
		}


		public static void InvokeOnMainThread(Action act)
		{
			if (Looper.MyLooper() == Looper.MainLooper)
			{
				act.Invoke();
			}
			else
			{
				var activity = GlobalSettings.GetActivity;
				if (activity != null)
				{
					activity.RunOnUiThread(act);
				}
			}
		}

		public static bool IsAppActive
		{
			get
			{
				return MainApplication.getIsRunning();
			}
		}

		public static Context GetContext
		{
			get 
			{ 
				try
				{
					return Xamarin.Forms.Forms.Context ?? Android.App.Application.Context as Context;
				}
				catch {
					return Android.App.Application.Context;
				}
			}
		}

		public static FormsAppCompatActivity GetActivity {
			get 
			{
				//var app = MainApplication.AppContext;
				//var app2 = app as FormsAppCompatActivity;
				//var app3 = app as Android.Support.V7.App.AppCompatActivity;
				return GetContext as FormsAppCompatActivity;
			}
		}

		public static ContentResolver GetResolver
		{
			get
			{
				return GetContext.ContentResolver;
				//var r = MainApplication.AppContext.ContentResolver;
				//return MainApplication.AppContext.ContentResolver;
			}
		}

		public static Android.Support.V7.App.ActionBar GetActionBar {
			get 
			{
				var c = GetActivity;
				return c.SupportActionBar;
			}
		}

		/// <summary>
		/// Get Android System Service
		/// </summary>
		/// <returns>The service.</returns>
		/// <param name="name">Name.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T GetService<T>(string name) where T : class
		{
			return GetContext.GetSystemService(name) as T;
		}

		/// <summary>
		/// Get Android System String
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="name">Name.</param>
		public static string GetString(string name)
		{
			return global::Android.Provider.Settings.Secure.GetString(GetResolver,name);			 
		}

		/// <summary>
		/// <code>
		///  var isCameraAvailable = AndroidGlobalSettings.GetContext.PackageManager.HasSystemFeature(PackageManager.FeatureCamera);
		/// </code>
		/// </summary>
		/// <returns><c>true</c> if has system feature the specified name; otherwise, <c>false</c>.</returns>
		/// <param name="name">Name.</param>
		public static bool HasSystemFeature(string name)
		{
			return GetContext.PackageManager.HasSystemFeature(name);
		}

		/// <summary>
		/// get the size of the Topbar
		/// </summary>
		/// <value>The size of the navigation bar.</value>
		public static Int32 TopBarSize
		{
			get
			{
				var temp = GlobalSettings.GetActivity;
				if (temp != null) {
					return temp.ActionBar.Height;
				}
				return 0;
			}
		}

		/// <summary>
		/// Has navigation bar. 
		/// </summary>-4
		/// <value><c>true</c> if has navigation bar; otherwise, <c>false</c>.</value>
		public static bool HasNavigationBar
		{
			get {
				try
				{
					var temp = GlobalSettings.GetActivity;
					if (temp != null) {
						return temp.ActionBar.IsShowing;
					}
					return false;
				}
				catch {
					return false;
				}
			}
		}

		/// <summary>
		/// Gets the Top View
		/// </summary>
		/// <value>The top view.</value>
		public static Android.Views.View TopView
		{
			get{
				return RootView;
			}
		}

		/// <summary>
		/// Gets the root view control.
		/// </summary>
		/// <value>The root view control.</value>
		public static Android.Views.View RootView {
			get 
			{				
				var a = (Activity)Forms.Context;
				var v = a.FindViewById(Android.Resource.Id.Content);
				return v;
			}
		}
	}
}