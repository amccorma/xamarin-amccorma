using System;

using Xamarin.Forms;
using VideoSamples.Views;
using VideoSamples.Controls;

namespace VideoSamples
{
	public class iOSVideoPlayer : ContentPage
	{
		private VideoPlayerView player;

		public iOSVideoPlayer ()
		{
			this.ToolbarItems.Add (new ToolbarItem {
				Text = "Actions",
				Command = new Command (async () => {
						var result = await DisplayActionSheet("Select", "Cancel", null, 
							"Full UIScreen", 
							"Full Screen",
							"Fit Screen",
							"Restart",
							"Play",
							"Stop",
							"Resize",
							"Pause",
							"StatusBar Hide");

					if (result == "Full UIScreen")
					{
						// this uiscreen.MainScreen.Bounds
						this.player.VideoPlayer.ContentHeight = -1;
						this.player.VideoPlayer.ContentWidth = -1;
					}
					else if (result == "Full Screen")
					{
						// this goes to full device screen
						player.VideoPlayer.FullScreen = !player.VideoPlayer.FullScreen;
					}
					else if (result == "Fit Screen")
					{
						player.VideoPlayer.FitInWindow = !player.VideoPlayer.FitInWindow;
					}
					else if (result == "Restart")
					{
						this.player.VideoPlayer.Seek = 0;
					}
					else if (result == "Play")
					{
						this.player.VideoPlayer.PlayerAction = VideoSamples.Library.VideoState.PLAY;
					}
					else if (result == "Stop")
					{
						this.player.VideoPlayer.PlayerAction = VideoSamples.Library.VideoState.STOP;
					}
					else if (result == "Resize")
					{
						if (test == 0)
						{
							this.player.HeightRequest = 400;
							this.player.VideoPlayer.ContentHeight = 400;
						}
						else if (test == 1)
						{
							this.player.HeightRequest = 200;
							this.player.VideoPlayer.ContentHeight = 100;
						}
						else if (test == 2)
						{
							this.player.HeightRequest = 100;
							this.player.VideoPlayer.ContentHeight = 100;
							test = -1;
						}
						test++;
					}
					else if (result == "Pause")
					{
						this.player.VideoPlayer.PlayerAction = VideoSamples.Library.VideoState.PAUSE;
					}
					else if (result == "StatusBar Hide")
					{
						this.player.VideoPlayer.ActionBarHide = !this.player.VideoPlayer.ActionBarHide;
					}
				})
			});


			player = new VideoPlayerView();

			player.HeightRequest = 200;
			player.VideoPlayer.AddVideoController = true;
			player.VideoPlayer.FileSource = "sample.m4v"; //"http://192.168.202.78/sample.m4v";

			// autoplay video
			player.VideoPlayer.AutoPlay = true;

			this.Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				Spacing = 0,
				Padding = new Thickness(0,0),
				Children =  
				{
					player
				}
			};
		}

		private Int32 test = 0;

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			player.VideoPlayer.OnTap += (MyVideoPlayer player, bool IsDoubleTap) => {
				System.Diagnostics.Debug.WriteLine("tapped fired, is double: " + IsDoubleTap);
			};

			this.player.VideoPlayer.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if (e.PropertyName == MyVideoPlayer.StateProperty.PropertyName)
				{
					var s = this.player.VideoPlayer.State;
					if (s == VideoSamples.Library.VideoState.ENDED)
					{
						System.Diagnostics.Debug.WriteLine("State: ENDED");
					}
					else if (s == VideoSamples.Library.VideoState.PAUSE)
					{
						System.Diagnostics.Debug.WriteLine("State: PAUSE");
					}
					else if (s == VideoSamples.Library.VideoState.PLAY)
					{
						System.Diagnostics.Debug.WriteLine("State: PLAY");
					}
					else if (s == VideoSamples.Library.VideoState.STOP)
					{
						System.Diagnostics.Debug.WriteLine("State: STOP");
					}
				}
				else if (e.PropertyName == MyVideoPlayer.InfoProperty.PropertyName)
				{
					System.Diagnostics.Debug.WriteLine("Info:\r\n" + this.player.VideoPlayer.Info);
				}
			};
		}

		protected override void OnSizeAllocated (double width, double height)
		{
			this.player.VideoPlayer.ContentHeight = height;
			this.player.VideoPlayer.ContentWidth = width;
			if (width < height) {

				this.player.VideoPlayer.Orientation = VideoSamples.Controls.MyVideoPlayer.ScreenOrientation.PORTRAIT;
			} else {
				this.player.VideoPlayer.Orientation = VideoSamples.Controls.MyVideoPlayer.ScreenOrientation.LANDSCAPE;
			}
			this.player.VideoPlayer.OrientationChanged ();
			base.OnSizeAllocated (width, height);
		}
	}
}


