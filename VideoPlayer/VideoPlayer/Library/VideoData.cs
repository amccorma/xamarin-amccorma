using System;

using Xamarin.Forms;

namespace LabSamples
{
	public class VideoData : ContentPage
	{
		public VideoData()
		{

		}

		/// <summary>
		/// Current position
		/// </summary>
		/// <value>At.</value>
		public double At {get;set;}

		/// <summary>
		/// In Milliseconds
		/// </summary>
		/// <value>The duration.</value>
		public double Duration {get;set;}

		/// <summary>
		/// in Milliseconds
		/// </summary>
		/// <value>The position.</value>
		public double Position { get; set; }

		public override string ToString ()
		{
			return string.Format ("[VideoData: At={0}, Duration={1}, Position={2}]", At, Duration, Position);
		}
	}
}


