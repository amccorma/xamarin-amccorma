using System;
using System.Collections.Generic;

namespace PushNotification.Plugin
{
	/// <summary>
	/// Push Events Listener
	/// </summary>
	public interface IPushListener
	{
		/// <summary>
		/// gets the Notification Message. need to cast it to proper type.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="values">Values.</param>
		/// <param name="AppActive">If set to <c>true</c> app active.</param>
		object getMessage(Dictionary<string, string> values, bool AppActive = false);
		/// <summary>
		/// Handle Saving the Token
		/// </summary>
		/// <param name="regId">Reg identifier.</param>
		void StoreRegistrationId(string regId);
		/// <summary>
		/// On Message Received
		/// </summary>
		/// <param name="values">Key/Value pairs</param>
		/// <param name="AppActive">iOS only, fromFinishedLaunching</param>
		void OnMessage(Dictionary<string, string> values, bool AppActive);
		/// <summary>
		/// On Registered
		/// </summary>
		/// <param name="token"></param>
		/// <param name="deviceType"></param>
		void OnRegistered(string token);
		/// <summary>
		/// On Unregistered
		/// </summary>
		void OnUnregistered(string token);
		/// <summary>
		/// OnError
		/// </summary>
		void OnError(Exception message);
		/// <summary>
		/// Should Show Notification
		/// </summary>
		bool ShouldShowNotification();
	}

}
