using System;
using Xamarin.Forms;
using System.Linq.Expressions;
using VideoSamples.Controls;

namespace VideoSamples.Views
{
	public class VideoPlayerView : ContentView
	{
		/// <summary>
		/// attached event handlers to this object
		/// </summary>
		private MyVideoPlayer _VideoPlayer;

		public MyVideoPlayer VideoPlayer {
			get {
				return this._VideoPlayer;
			}
		}

		public VideoPlayerView ()
		{
            this._VideoPlayer = new MyVideoPlayer();
			this.HorizontalOptions = LayoutOptions.FillAndExpand;
			this.VerticalOptions = LayoutOptions.FillAndExpand;

			this.Content = this._VideoPlayer;
		}
	}
}

