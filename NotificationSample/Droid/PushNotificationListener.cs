using System;
using System.Collections.Generic;
using MyNotification.Plugin.Android;

namespace NotificationSample.Droid
{
	public class PushNotificationListener : IPushListener
	{
		// TODO: Replace default Title of Notification
		private string defaultTitle = "AppName";

		public PushNotificationListener()
		{
			
		}

		private NotificationActions NotificationActionService
		{
			get
			{
				return new NotificationActions();
			}
		}

		#region IPushNotificationListener implementation

		public void RetryRegister ()
		{
			var temp = NotificationActionService;
			if (temp != null) {
				temp.Register ();
			}
		}

		public void StoreRegistrationId (string regId)
		{
			MyPushNotification.Current.SaveToken (regId);
		}

		// TODO: this method will change based on your app rules to handle notifications
		// see Notification Objects format in notificationFormats.txt
		// If you use a different platform, you have a different values
		public async void OnMessage (Dictionary<string, string> values)
		{
			try
			{
				MyPushNotification.NotificationError = false;
				string message = "";
				var messagefound = false;

				// TODO: replace object with your JSON object
				dynamic payload = null;
				Int32 messageID = -1;
				if (values != null) {
					foreach (var item in values) {
						if (item.Key == "message")
						{
							message = item.Value.ToString();
						}
						else if (item.Key == "default")
						{
							message = item.Value.ToString();
						}
						else if (item.Key == "myObject")
						{
							// TODO: deserialize any payload objects
							payload = null;
						}
						else if (item.Key == "messageID")
						{
							messageID = Convert.ToInt32(item.Value.ToString());
						}
					}
				}

				// TODO: set title
				var title = ""; //payload.Title;

				// TODO : set message or leave message above
				//message = ""; // (payload == null) ? message : payload.Body;

				var isAppActive = GlobalSettings.getIsAppForeground();

				if (isAppActive)
				{
					// get the message syncs
					if (messageID > 0)
					{
						// TODO: remove if not using messageID. 
						// I fetched the notification from a web api service.
						//messagefound = await new Services.AlertServiceCalls().getAlertMessageID(messageID);

						// TODO: remove this if not using
						messagefound = false;
					}

					// TODO: app is app. Handle the alert differently if you wish
					if (true)
					{
						GlobalSettings.InvokeOnMainThread(() =>
					   {
						   // System.Diagnostics.Debug.WriteLine($"App is Active. Sync Messages");
						   var builder = new Android.Support.V7.App.AlertDialog.Builder(GlobalSettings.GetContext);
						   builder.SetTitle(title);
						   builder.SetMessage(message);
						   builder.SetPositiveButton("View", (sender, e) =>
						   {
							   // TODO: I used messagecenter to notify the user a notification arrived.
						   });
						   builder.SetNegativeButton("Cancel", (sender, e) => { });
						   //builder.SetIcon(Resource.Mipmap.appicon);
						   builder.Show();
					   });
					}
					else if (messagefound)
					{
						// TODO: I used messagecenter to notify the user a notification arrived.
					}
				}
				else 
				{
					// fire service to get message
					if (messageID > 0)
					{
						// TODO: My fetch handlers to get the notification from a webapi service.
						//var context = GlobalSettings.GetContext;
						//if (context != null)
						//{
						//	Intent alertIntent = new Intent(GlobalSettings.GetContext, typeof(AlertFetchService));
						//	alertIntent.PutExtra("id", $"{messageID}");
						//	Application.Context.StartService(alertIntent);
						//}
						//else
						//{
						//	var task = new Notifications.Services.FetchAlertTask();
						//	task.Execute(messageID);
						//}
					}

					// TODO: check and create your own notification
					new NotificationActions().CreateRemoteNotification(
						title,
						message);
				}
			}
			catch(Exception ex) {
				HandleException (ex);
			}
		}

		public void OnRegistered (string token)
		{
			// TODO: Handle token registration
			MyPushNotification.NotificationError = false;
			NotificationActionService.RegisterToService (token);

		}

		public void OnUnregistered (string token)
		{
			MyPushNotification.NotificationError = false;
			MyPushNotification.Current.SaveToken ("");
		}

		public void OnError (Exception message)
		{
			MyPushNotification.NotificationError = true;
			HandleException (message);
		}

		private void HandleException(string message)
		{
			// TODO: Add any error handling
		}

		private void HandleException(Exception message)
		{
			// TODO: Add any error handling
		}

		public bool ShouldShowNotification ()
		{
			return true;
		}

		#endregion
	}
}

