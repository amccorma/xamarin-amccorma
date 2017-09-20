using System;
using System.Collections.Generic;

namespace MyNotification.Plugin.Android
{
	/// <summary>
	/// Push Events Listener
	/// </summary>
	public interface IPushListener
    {
		/// <summary>
		/// Register failed, retry
		/// </summary>
		void RetryRegister();
		/// <summary>
		/// Handle Saving the Token
		/// </summary>
		/// <param name="regId">Reg identifier.</param>
		void StoreRegistrationId (string regId);
        /// <summary>
        /// On Message Received
        /// </summary>
        /// <param name="values"></param>
        /// <param name="deviceType"></param>
		void OnMessage(Dictionary<string, string> values);
        /// <summary>
        /// On Registered
        /// </summary>
        /// <param name="token"></param>
        /// <param name="deviceType"></param>
        void OnRegistered(string token);
        /// <summary>
        /// On Unregistered
        /// </summary>
        /// <param name="deviceType"></param>
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
