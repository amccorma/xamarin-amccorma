using System;

namespace TopAlert
{
	public class TopAlert
	{
		public TopAlert ()
		{
			this.TextSize = -1;
			this.AlertHeight = -1;
			this.BorderWidth = 1;
			this.TopOffset = 0;
			this.Intent = 0;

			if (Xamarin.Forms.Device.OS == Xamarin.Forms.TargetPlatform.iOS) {
				this.Duration = 1;
			} else {
				this.Duration = 1000;
			}
		}

		public bool FadeOut { get; set; }

		/// <summary>
		/// Padding Left and Right Size
		/// </summary>
		/// <value>The intent.</value>
		public Int32 Intent { get; set; }

		public Xamarin.Forms.Color BackgroundColor { get; set; }

		public Xamarin.Forms.Color BorderColor { get; set; }

		public Xamarin.Forms.Color TextColor  { get; set; }

		public string Text  { get; set; }

		public float TextSize { get; set; }

		public float AlertHeight { get; set; }

		public float BorderWidth { get; set; }

		/// <summary>
		/// <para>Gets or sets the duration.</para>
		/// <para>iOS: in seconds</para>
		/// <para>Android: in MILLiseconds</para>
		/// </summary>
		/// <value>The duration.</value>
		public Int32 Duration { get; set; }

		public Int32 TopOffset { get; set; }

		/// <summary>
		/// iOS has Navigation Bar (iOS only)
		/// </summary>
		/// <value><c>true</c> if this instance has navigation bar; otherwise, <c>false</c>.</value>
		public bool HasNavigationBar { get; set; }

	}
}

