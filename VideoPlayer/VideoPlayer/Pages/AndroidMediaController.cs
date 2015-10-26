using System;
using Xamarin.Forms;
using LabSamples.Views;

namespace LabSamples
{
	public class AndroidMediaController : ContentPage
	{
		private VideoPlayerView player;


		public AndroidMediaController ()		
		{				
			player = new VideoPlayerView {
				// Android/Resource/raw folder
				//FileSource = "sample",

				// http
				FileSource = "http://techslides.com/demos/sample-videos/small.mp4"
			};		

			player.HeightRequest = 300;
			player.VideoPlayer.UseBuiltInMediaPlayer = true;

			// autoplay video
			// player.VideoPlayer.AutoPlay = true;

			player.VideoPlayer.OnBufferUpdate += (int percent) => {
				System.Diagnostics.Debug.WriteLine("buffer update: " + percent);
			};

			player.VideoPlayer.OnChange += (option) => {
				System.Diagnostics.Debug.WriteLine(option.ToString());
			};

			player.VideoPlayer.OnError += (msg) => {
				System.Diagnostics.Debug.WriteLine("Error: " + msg);
			};

			player.VideoPlayer.OnStateChanged += (evt) => {
				System.Diagnostics.Debug.WriteLine("state: " + evt);
			};

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


