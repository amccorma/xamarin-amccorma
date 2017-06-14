using Foundation;
using UIKit;
using System;
using System.Collections.Generic;
using System.Linq;
using UserNotifications;

namespace PushNotification.Plugin
{
	/// <summary>
	/// Implementation for PushNotification
	/// </summary>
	public class PushNotificationImplementation : IPushNotification
	{
		public string Token
		{
			get
			{
				return NSUserDefaults.StandardUserDefaults.StringForKey(CrossPushNotification.NotificationTokenKey);
			}
		}

		public void Register()
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
		  	{
				  if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
				  {
					  UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge, (approved, err) =>
						{
							if (approved)
							{
								UIApplication.SharedApplication.InvokeOnMainThread(() =>
							   {
								   UIApplication.SharedApplication.RegisterForRemoteNotifications();
							   });
							}
						});
				  }
				  else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
				  {
					  UIUserNotificationType userNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
					  UIUserNotificationSettings settings = UIUserNotificationSettings.GetSettingsForTypes(userNotificationTypes, null);
					  UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
				  }
				  else
				  {
					  UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
					  UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
				  }
			  });
		}

		public void Unregister()
		{
			UIApplication.SharedApplication.UnregisterForRemoteNotifications();
		}

		#region IPushNotificationListener implementation

		public void OnMessageReceived(NSDictionary userInfo, bool AppActive)
		{
			var parameters = getNotificationDictionary(userInfo);
			CrossPushNotification.PushNotificationListener.OnMessage(parameters, AppActive);
		}

		public Dictionary<string, string> getNotificationDictionary(NSDictionary userInfo)
		{
			var parameters = new Dictionary<string, string>();
			var keyAps = new NSString("aps");

			if (userInfo.ContainsKey(keyAps))
			{
				NSDictionary aps = userInfo.ValueForKey(keyAps) as NSDictionary;

				if (aps != null)
				{
					foreach (var apsKey in aps)
					{
						parameters.Add(apsKey.Key.ToString(), apsKey.Value.ToString());
					}
				}

				var items = userInfo.Keys.Where(x => x.ToString() != keyAps);
				foreach (var item in items)
				{
					parameters.Add(item.ToString(), userInfo[item].ToString());
				}
			}
			return parameters;
		}

		public void OnErrorReceived(NSError error)
		{
			CrossPushNotification.PushNotificationListener.OnError(new Exception(error.LocalizedDescription));
		}

		public void OnRegisteredSuccess(NSData token)
		{
			//Debug.WriteLine("iOS Succesfully Registered.");
			string trimmedDeviceToken = token.Description;
			if (!string.IsNullOrWhiteSpace(trimmedDeviceToken))
			{
				trimmedDeviceToken = trimmedDeviceToken.Trim('<');
				trimmedDeviceToken = trimmedDeviceToken.Trim('>');
				trimmedDeviceToken = trimmedDeviceToken.Trim();
				trimmedDeviceToken = trimmedDeviceToken.Replace(" ", "");
			}
			//System.Diagnostics.Debug.WriteLine("Token: " + trimmedDeviceToken);
			NSUserDefaults.StandardUserDefaults.SetString(trimmedDeviceToken, CrossPushNotification.NotificationTokenKey);
			NSUserDefaults.StandardUserDefaults.Synchronize();

			CrossPushNotification.PushNotificationListener.StoreRegistrationId(trimmedDeviceToken);
			CrossPushNotification.PushNotificationListener.OnRegistered(trimmedDeviceToken);
		}

		public void OnUnregisteredSuccess()
		{
			var t = Token;
			NSUserDefaults.StandardUserDefaults.SetString(string.Empty, CrossPushNotification.NotificationTokenKey);
			NSUserDefaults.StandardUserDefaults.Synchronize();
			CrossPushNotification.PushNotificationListener.OnUnregistered(t);
		}

		#endregion
	}
}