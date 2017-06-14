using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using ObjCRuntime;
using PushNotification.Plugin;
using UIKit;

namespace NotificationSample.iOS
{
	public class NotificationActions
	{
        private bool _RegisterError = false;
        private bool _IsRegisteringNow;
        private bool _isRunningNotificationRegister;
		private const string CHECK_OP_NO_THROW = "checkOpNoThrow";
		private const string OP_POST_NOTIFICATION = "OP_POST_NOTIFICATION";

		// TODO: this key must be unique
		public const string notificationAlertKey = "mynotify";
		public const string notificationlocal = "nlocal";

        private bool IsRunning
        {
            get
            {
                return GlobalSettings.IsAppActive;
            }
        }

        public Task<bool> getHasRegisterationID()
        {
            return Task.Run(async () =>
            {
                var token = await getNotificationToken();
                return String.IsNullOrEmpty(token) == false;
            });
        }

		public bool IsSimulator
		{
			get
			{
				return Runtime.Arch == Arch.SIMULATOR;
			}
		}

        public void Register()
        {
			if (IsSimulator == false &&this.IsRegisteringNow == false)
			{
				IsRegisteringNow = true;
                CrossPushNotification.Current.Register();
            }
        }

		public bool IsRegisteringNow
		{
			get
			{
				return this._IsRegisteringNow;
			}
			set
			{
				this._IsRegisteringNow = value;
			}
		}

        private void HandleExceptions(Exception ex)
        {
            // TODO: Handle Exceptions
        }


        public void Cancel(int notificationID)
        {
            // not implentation as of iOS 9.0
        }

        public void CancelAll()
        {
            UIApplication.SharedApplication.CancelAllLocalNotifications();
        }

        public Task<bool> UnRegisterToService(string registerationID)
        {
			return Task.FromResult<bool>(true);
        }

        public Task<string> getNotificationToken()
        {
            return Task.Run(() =>
            {
                var token = new SharedPrefs().Get(CrossPushNotification.NotificationTokenKey);
                if (String.IsNullOrEmpty(token) == false)
                {
                    return token;
                }
                return String.Empty;
            });
        }

        public void CreateNotification(NotificationAlertMessage message, bool IsLocal, Dictionary<string, string> Parameters)
        {
    
            if (message != null)
            {
                if (IsLocal == false)
                {
                    //await IncrementNotificationCount();
                }

				// TODO: set values to be passed to notification when user clicks on it
                var dic = new NSMutableDictionary();
                dic.Add(new NSString(notificationlocal), new NSString(IsLocal ? "1" : "0"));
                if (Parameters == null)
                {
					dic.Add(new NSString(notificationAlertKey), new NSString("1"));
                }
                else
                {
                    foreach (var item in Parameters)
                    {
                        dic.Add(new NSString(item.Key), new NSString(item.Value));
                    }
                }

                // TODO: get the body of the notification from a service if available



                GlobalSettings.InvokeOnMainThread(() =>
               {
                   var state = UIApplication.SharedApplication.ApplicationState;
                   //System.Diagnostics.Debug.WriteLine("app state:= " + state);

                   var notification = new UILocalNotification();
                   notification.FireDate = Foundation.NSDate.Now.AddSeconds(1);
                   // configure the alert stuff
                   notification.AlertAction = message.Title;
                   notification.AlertBody = message.Message;
                   //notification.ApplicationIconBadgeNumber = getBadgeCountNoLock();
                   notification.UserInfo = dic;
                   notification.AlertTitle = message.Title;
                   // set the sound to be the default sound
                   notification.SoundName = UILocalNotification.DefaultSoundName;

                   // schedule it
                   UIApplication.SharedApplication.ScheduleLocalNotification(notification);
               });
            }
        }

		public void setBadgeNumber(Int32 count)
		{
			GlobalSettings.InvokeOnMainThread(() =>
			{
				UIApplication.SharedApplication.ApplicationIconBadgeNumber = count;
			});
		}

		public Int32 getBadgeCount()
		{
			return (int)UIApplication.SharedApplication.ApplicationIconBadgeNumber;
		}

		public bool RegisterError
		{
			get
			{
				return _RegisterError;
			}
			private set
			{
				_RegisterError = value;
			}
		}

		public void HandleRegisterError(string message, string deviceOS)
		{
			IsRegisteringNow = false;
			HandleException(new Exception(message));
		}

		private void HandleException(Exception ex)
		{
			// TODO: add Error Handler
		}

		public Task<bool> RegisterToService(string registerationID)
		{
            return Task.Run(() =>
            {            
				// run from Task or UI will lock up

				// TODO: Save the registrationID
				return true;
            });
		}
	}
}

