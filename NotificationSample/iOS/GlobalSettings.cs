using System;
using UIKit;
using Foundation;
using System.Threading.Tasks;

namespace NotificationSample.iOS
{
    public class GlobalSettings
    {
        public GlobalSettings()
        {

        }

		public static async Task InvokeOnMainThreadAndWait(Action act)
		{
			await Task.Run (() => {
				using (var evt = new System.Threading.ManualResetEvent (false)) {
					MainApplication.InvokeOnMainThread( ()=> {
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

		public static bool IsAppActive
		{
			get
			{
				try
				{
					return Xamarin.Forms.Forms.IsInitialized;
				}
				catch
				{
					return false;
				}
			}
		}
			

		public static void InvokeOnMainThread(Action act)
		{
			MainApplication.InvokeOnMainThread (act);
		}
			
			
		public static string PackageName 
		{
			get { 
				return NSBundle.MainBundle.BundleIdentifier;
			}
		}

		public static Double VersionNumber {
			get { 
				Double d;
				if (Double.TryParse (NSBundle.MainBundle.InfoDictionary ["CFBundleVersion"].ToString (), out d)) {
					return d;
				}
				return 0.0;
			}
		}

		/// <summary>
		/// Gets the main application.
		/// </summary>
		/// <value>The main application.</value>
		public static UIApplication MainApplication
		{
			get 
			{
				return UIApplication.SharedApplication;
			}
		}

		/// <summary>
		/// Gets the main window.
		/// </summary>
		/// <value>The main window.</value>
		public static UIWindow MainWindow {
			get 
			{
				return UIApplication.SharedApplication.KeyWindow;
			}
		}

		/// <summary>
		/// returns the top level activity class name
		/// </summary>
		/// <value>The top activity.</value>
		public static string TopWindowClassName
		{
			get
			{
				var window = RootViewControl;
				if (window != null)
				{
					var temp = window.PresentedViewController as UINavigationController;
					if (temp != null && temp.TopViewController != null)
					{
						return temp.TopViewController.Class.Name;
					}
				}
				return "";

			}
		}

		/// <summary>
		/// Gets the root view control.
		/// </summary>
		/// <value>The root view control.</value>
		public static UIViewController RootViewControl {
			get 
			{				
				var r = UIApplication.SharedApplication.KeyWindow;
				if (r != null) {
					return r.RootViewController;
				}
				return null;
			}
		}
    }
}