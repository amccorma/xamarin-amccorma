using System.Collections.Generic;
using Foundation;

namespace PushNotification.Plugin
{
	/// <summary>
	/// Interface for PushNotification
	/// </summary>
	public interface IPushNotification
	{
		/// <summary>
		/// gets the notification key/values
		/// </summary>
		/// <returns>The dictionary.</returns>
		/// <param name="userInfo">User info.</param>
		Dictionary<string, string> getNotificationDictionary(NSDictionary userInfo);

		void Unregister();

		string Token { get; }

		void Register();

		/// <summary>
		/// ON Notification Message received
		/// </summary>
		/// <param name="parameters">Parameters.</param>
		/// <param name="AppActive">If set to <c>true</c> app active.</param>
		void OnMessageReceived(NSDictionary parameters, bool AppActive);

		void OnErrorReceived(NSError error);

		void OnRegisteredSuccess(NSData token);

	}
}