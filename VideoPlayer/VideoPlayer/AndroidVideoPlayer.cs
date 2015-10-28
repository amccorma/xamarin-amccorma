using System;
using Xamarin.Forms;
using VideoSamples.Views;
using VideoSamples.Controls;

namespace VideoSamples
{
	public class AndroidVideoPlayer : ContentPage
	{
		private VideoPlayerView player;

		public AndroidVideoPlayer ()
		{
			player = new VideoPlayerView ();

			this.ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Controller",
				Command = new Command( () => {
					this.player.VideoPlayer.AddVideoController = !this.player.VideoPlayer.AddVideoController;
				})
			});

			this.ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Full Screen",
				Command = new Command( () => {

					// resize the Content for full screen mode
					this.player.VideoPlayer.FullScreen = !this.player.VideoPlayer.FullScreen;
					if (this.player.VideoPlayer.FullScreen)
					{
						this.player.HeightRequest = -1;
						this.Content.VerticalOptions = LayoutOptions.FillAndExpand;
						player.VideoPlayer.FullScreen = true;
					}
					else
					{
						this.player.HeightRequest = 200;
						this.Content.VerticalOptions = LayoutOptions.StartAndExpand;
						player.VideoPlayer.FullScreen = false;
					}
				})
			});

			this.ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Play",
				Command = new Command( () => {
					this.player.VideoPlayer.PlayerAction = VideoSamples.Library.VideoState.PLAY;
				})
			});

			this.ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Stop",
				Command = new Command( () => {
					this.player.VideoPlayer.PlayerAction = VideoSamples.Library.VideoState.STOP;
				})
			});

			this.ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Pause",
				Command = new Command( () => {
					this.player.VideoPlayer.PlayerAction = VideoSamples.Library.VideoState.PAUSE;
				})
			});

			this.ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Restart",
				Command = new Command( () => {
					this.player.VideoPlayer.PlayerAction = VideoSamples.Library.VideoState.RESTART;
				})
			});

			// heightRequest must be set it not full screen
			player.HeightRequest = 200;
			player.VideoPlayer.AddVideoController = false;


			// location in Assets folder.  file marked as Asset, NOT Resource
			player.VideoPlayer.FileSource = "sample";

			// autoplay video
			//player.VideoPlayer.AutoPlay = true;

			this.Content = new StackLayout
			{				
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children =  
				{
					player
				}
			};
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
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


