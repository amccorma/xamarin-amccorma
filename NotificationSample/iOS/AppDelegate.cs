using System;
using Foundation;
using PushNotification.Plugin;
using UIKit;

namespace NotificationSample.iOS
{
	// TODO: using the old Notification handlers.  iOS 10 changed to use a delegate, but supports the old way
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		protected internal static string appStateKey = "appStateKey";
		protected internal static string appResumeValue = "resume";
		protected internal static string appstopValue = "stop";
		protected internal static string appPausedValue = "paused";

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{

			// enable Notification Fetch Service
			UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);


			// TODO: set Values
			// set the application tag ID. 
			// used for logging
			CrossPushNotification.NotificationTokenKey = "TokenKey";
			CrossPushNotification.NotificationAppVersionKey = "VersionKey";
			CrossPushNotification.NotificationTagKey = "AppTag";
			CrossPushNotification.Initialize<PushNotificationListener>();

			// TODO: change code to handle your notifications
            HandleNotification(options);

			// TODO: register for Notifications
            RegisterForNotifications();

			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

		///// <summary>
		///// Called when [activated].
		///// </summary>
		///// <param name="uiApplication">The application.</param>
		public override void OnActivated(UIApplication uiApplication)
		{
			// TODO: set Badge count or remove
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 12;

			base.OnActivated(uiApplication);
		}

		/// <summary>
		/// Dids the enter background. App in Background
		/// </summary>
		/// <param name="uiApplication">The application.</param>
		public async override void DidEnterBackground(UIApplication uiApplication)
		{
			// TODO: set Badge count or remove
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 12;
			new SharedPrefs().Save(appStateKey, appPausedValue);

			base.DidEnterBackground(uiApplication);
		}

		/// <summary>
		/// Wills the enter foreground.  App is Running
		/// </summary>
		/// <param name="uiApplication">The application.</param>
		public async override void WillEnterForeground(UIApplication uiApplication)
		{
			new SharedPrefs().Save(appStateKey, appResumeValue);

			base.WillEnterForeground(uiApplication);
		}


		public override bool WillFinishLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
			new SharedPrefs().Save(appStateKey, appResumeValue);

			return base.WillFinishLaunching(uiApplication, launchOptions);
		}

		public override async void WillTerminate(UIApplication uiApplication)
		{
			// TODO: set Badge count or remove
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 10;
			new SharedPrefs().Save(appStateKey, appstopValue);

			base.WillTerminate(uiApplication);
		}

		/// <summary>
		/// Background Fetch
		/// </summary>
		/// <param name="application">Application.</param>
		/// <param name="completionHandler">Completion handler.</param>
		public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
		{
			var result = UIBackgroundFetchResult.NoData;
			try
			{
				// TODO: Make sure Background Fetch is enabled. 
				// TODO: have 30 seconds max to run any code here
				// TODO: Fetch notifications here if running in background or app is not active.
				var data = true;
				if (data)
				{
					//Indicate we have new data
					result = UIBackgroundFetchResult.NewData;
				}
			}
			catch (Exception ex)
			{
				//Indicate a failed fetch if there was an exception
				result = UIBackgroundFetchResult.Failed;
			}
			finally
			{
				//We really should call the completion handler with our result
				completionHandler(result);
			}
		}

		protected internal void RegisterForNotifications()
		{
			new NotificationActions().Register();
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			CrossPushNotification.Current.OnRegisteredSuccess(deviceToken);
		}

		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			application.RegisterForRemoteNotifications();
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			CrossPushNotification.Current.OnErrorReceived(error);
		}

		public override async void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			// duplicate this in the NotificationCenterDelegate
			var dic = notification.UserInfo;

			// remove local notification
			UIApplication.SharedApplication.CancelLocalNotification(notification);

			// TODO: this is customized to your environment
			var IsLocal = dic[NotificationActions.notificationlocal].ToString() == "1";
			var IsNotification = dic[NotificationActions.notificationAlertKey].ToString() == "1";
		}

		/// <summary>
		/// This will not get hit if the application has been closed
		/// </summary>
		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			// TODO: this get fired for Notifications when the UIApplicationState is Active
			var contentKey = "content-available";
			// TODO: this changes on your notification object
			var messageIDKey = "messageID";
			bool contentAvailable = false;
			var result = UIBackgroundFetchResult.NoData;
			string messageID = "";

			// TODO: based on your notification object and keys assigned
			if (userInfo != null && userInfo.ContainsKey(new NSString("aps")))
			{
				NSDictionary aps = userInfo.ValueForKey(new NSString("aps")) as NSDictionary;
				if (aps.ContainsKey(new NSString(contentKey)))
				{
					contentAvailable = (NSString)aps.ValueForKey(new NSString(contentKey)) == "1";
				}
				if (userInfo.ContainsKey(new NSString(messageIDKey)))
				{
					var obj = userInfo.ValueForKey(new NSString(messageIDKey)).ToString();
					messageID = obj;
				}

				if (contentAvailable)
				{
					result = UIBackgroundFetchResult.NewData;

					// TODO: perform fetch for notification body.
					// ? use web service or cloud

				}
				if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active)
				{
					CrossPushNotification.Current.OnMessageReceived(userInfo, true);
				}
				else if (UIApplication.SharedApplication.ApplicationState != UIApplicationState.Background)
				{
					CrossPushNotification.Current.OnMessageReceived(userInfo, false);
				}
			}

			completionHandler(result);
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			if (application.ApplicationState == UIApplicationState.Active)
			{
				// do alert
				CrossPushNotification.Current.OnMessageReceived(userInfo, true);
			}
			else
			{
				// app was just brought from background to foreground
				// no alert
				CrossPushNotification.Current.OnMessageReceived(userInfo, false);
			}
		}

		/// <summary>
		/// App was closed but received notification
		/// </summary>
		/// <param name="options">Options.</param>
		protected internal void HandleNotification(NSDictionary options)
		{
			if (options != null)
			{
				// check for a local notification
				if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
				{
					UILocalNotification localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
					if (localNotification != null)
					{
						// TODO: Local Notification received. Do something with it
						new UIAlertView(localNotification.AlertAction, localNotification.AlertBody, null, "OK", null).Show();
					}
				}

				// check for a remote notification
				if (options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
				{
				}
				else
				{
					// TODO: remote notification received
				}
			}
		}
	}
}
