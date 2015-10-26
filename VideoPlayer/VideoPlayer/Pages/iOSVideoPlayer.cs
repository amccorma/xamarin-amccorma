using System;

using Xamarin.Forms;
using LabSamples.Views;

namespace LabSamples
{
	public class iOSVideoPlayer : ContentPage
	{
		private VideoPlayerView player;

		public iOSVideoPlayer ()
		{
			// this demo has no build in controller.  create your only controller
			player = new VideoPlayerView {
				// location in Assets folder.  file marked as Asset, NOT Resource
				FileSource = "sample.m4v",
			};

			player.HeightRequest = 200;
			player.VideoPlayer.UseBuiltInMediaPlayer = false;

			// autoplay video
			player.VideoPlayer.AutoPlay = true;

			this.Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children =  
				{
					// can attach buttons to play and stop
					// hook into the player.VideoPlayer methods;
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children = 
						{
							new Button 
							{
								Text = "Restart",
								Command = new Command(() => {
									this.player.VideoPlayer.Seek = 0;
								})
							},
							new Button 
							{
								Text = "Stop",
								Command = new Command( () => {
									this.player.VideoPlayer.Stop();
								})
							},
							new Button
							{
								Text = "Play",
								Command = new Command( () => {
									this.player.VideoPlayer.Play();
								})
							},
						}
					},
					player
				}
			};
		}


	}
}


