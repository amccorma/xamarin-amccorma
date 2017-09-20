namespace MyNotification.Plugin.Android
{
	/// <summary>
	/// Interface for PushNotification
	/// </summary>
	public interface IPushNotification
	{
		/// <summary>
		/// Unregister
		/// </summary>
		void Unregister(); 

		/// <summary>
		/// Get Notification Token
		/// </summary>
		/// <value>The token.</value>
		string Token { get; }

		/// <summary>
		/// Save Notification Token
		/// </summary>
		/// <param name="token">Token.</param>
		void SaveToken (string token);

		/// <summary>
		/// get GCM Registration Token
		/// </summary>
		/// <returns>token</returns>
		string GetToken();

		/// <summary>
		/// Register
		/// </summary>
		void Register();
	}
}