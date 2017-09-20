using System;
using System.Collections.Generic;
using System.Linq;
using PushNotification.Plugin;

namespace NotificationSample.iOS
{
	public class PushNotificationListener : IPushListener
	{
		#region IPushNotificationListener implementation

		public void StoreRegistrationId(string regId)
		{
			new SharedPrefs().Save(CrossPushNotification.NotificationTokenKey, regId);
		}

		// TODO: this method will change based on your app rules to handle notifications
		// see Notification Objects format in notificationFormats.txt
		// If you use a different platform, you have a different values
		public object getMessage(Dictionary<string, string> values, bool AppActive)
		{
			string body = string.Empty;
			string title = string.Empty;
			Int32 messageID = 0;
			NotificationAlertMessage msg = null;
			foreach (var item in values)
			{
				if (item.Key == "alert")
				{
					/* Format Expected 
							{
								body = "Andrew test message.  testing psea notifications.";
								title = test;
							}
					 */
					// TODO: change based on your format
					var obj = AlertKeyParse(item.Value);
					body = obj["body"];
					title = obj["title"];
				}
				else if (item.Key == "messageID")
				{
					messageID = Convert.ToInt32(item.Value.ToString());
				}
			}

			if (body != null)
			{
				// TODO: create object 
				msg = new NotificationAlertMessage();
				msg.Message = body;
				msg.MessageID = messageID;
				msg.Title = title;

				return msg;
			}

			return null;
		}

		public void OnMessage(Dictionary<string, string> values, bool AppActive)
		{
			try
			{
				// TODO: this is where you handle the values returned
				var msg = getMessage(values, AppActive) as NotificationAlertMessage;
				if (msg != null)
				{
					new NotificationActions().CreateNotification(msg, AppActive, null);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private Dictionary<string, string> AlertKeyParse(string text)
		{
			try
			{
				return text.Replace("{", "").Replace("}", "").Trim().Split(';')
							   .Select(x => x.Trim().Split('='))
							   .Where(x => x.Length == 2)
							   .ToDictionary(x => x[0].Trim(),
											 x => x[1].Trim().Trim('"'));
			}
			catch (Exception)
			{
				HandleException($"Expected dictionary pair, got {text}");
			}
			// TODO: default return
			return new Dictionary<string, string>()
			{
				{ "title" , "My title" },
				{ "body" , "" }
			};
		}

		public void OnRegistered(string token)
		{
			CrossPushNotification.NotificationError = false;
			new NotificationActions().RegisterToService(token);
		}

		public void OnUnregistered(string token)
		{
			new SharedPrefs().Save(CrossPushNotification.NotificationTokenKey, "");
			new NotificationActions().UnRegisterToService(token);
		}

		public void OnError(Exception message)
		{
			HandleException(message);
		}

		private void HandleException(string message)
		{
			CrossPushNotification.NotificationError = true;
			// TODO: Handle Exception
		}

		private void HandleException(Exception message)
		{
			CrossPushNotification.NotificationError = true;
			// TODO: Handle Excepti
		}

		public bool ShouldShowNotification()
		{
			return true;
		}

		#endregion
	}
}

