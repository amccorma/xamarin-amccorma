using System;
namespace NotificationSample.iOS
{
	public class NotificationAlertMessage
	{
		public NotificationAlertMessage()
		{
		}

		public string Message { get; internal set; }
		public int MessageID { get; internal set; }
		public string Title { get; internal set; }
	}
}
