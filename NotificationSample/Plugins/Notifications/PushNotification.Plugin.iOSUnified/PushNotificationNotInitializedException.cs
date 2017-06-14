using System;

namespace PushNotification.Plugin
{
	/// <summary>
	/// Push Notification not Initialized Exception class
	/// </summary>
	public class PushNotificationNotInitializedException : Exception
    {
          /// <summary>
          /// Default Contructor
          /// </summary>
          public PushNotificationNotInitializedException()
          {
          }
          /// <summary>
          /// Constructor with message
          /// </summary>
          public PushNotificationNotInitializedException(string message): base(message)
          {
          }
    }
}
