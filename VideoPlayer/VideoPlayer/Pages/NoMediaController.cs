using System;
using Xamarin.Forms;
using LabSamples.Views;

namespace LabSamples
{
	public class NoMediaController : ContentPage
	{
		private VideoPlayerView player;

		public NoMediaController ()
		{
			// this demo has no build in controller.  create your only controller

			player = new VideoPlayerView {
				// location in Assets folder.  file marked as Asset, NOT Resource
				FileSource = "sample.mp4",
			};

			// this works for android
			player.HeightRequest = 300;
			player.VideoPlayer.UseBuiltInMediaPlayer = false;

			// autoplay video
			player.VideoPlayer.AutoPlay = true;

			this.Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children =  
				{
					player
				}
			};
		}
	}
}


