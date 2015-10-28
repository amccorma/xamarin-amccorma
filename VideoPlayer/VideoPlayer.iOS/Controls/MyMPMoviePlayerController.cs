using System;
using MonoTouch.MediaPlayer;
using MonoTouch.Foundation;
using VideoSamples.Controls;
using System.Threading;
using System.Threading.Tasks;
using MonoTouch.UIKit;

namespace VideoSamples.iOS
{
	public class MyMPMoviePlayerController : MPMoviePlayerController
	{
		private WeakReference _ParentElement;

		private CancellationTokenSource _Token;

		private bool _EndOfVideo = true;


		public MyMPMoviePlayerController (MyVideoPlayer Parent)
		{
			this.ParentElement = Parent;

			NSNotificationCenter.DefaultCenter.AddObserver (this, 
				new MonoTouch.ObjCRuntime.Selector("LoadChangedCallback:"),
				MPMoviePlayerController.LoadStateDidChangeNotification,
				null
			);

			this._Token = new CancellationTokenSource ();

			Task.Run (async() => {
				while(true && this._Token.Token.IsCancellationRequested == false)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(200), this._Token.Token);

					var t = this.CurrentPlaybackTime;
					var d = this.PlayableDuration;

					// is there a better way to calculate the end of video???
					if (this.PlayableDuration > 1.000 && (Math.Abs(t - d) <= 0.0001 * double.Epsilon))
					{
						this.ParentElement.State = VideoSamples.Library.VideoState.ENDED;
						this.ParentElement.Info = new VideoData
						{
							State = VideoSamples.Library.VideoState.ENDED,
							At = t,
							Duration = d
						};
						this._EndOfVideo = true;
					}
					else
					{
						this._EndOfVideo = false;

						if (this.PlaybackState == MPMoviePlaybackState.Paused)
						{
							this.ParentElement.State = VideoSamples.Library.VideoState.PAUSE;
						}
						else if (this.PlaybackState == MPMoviePlaybackState.Stopped)
						{
							this.ParentElement.State = VideoSamples.Library.VideoState.STOP;

						}
						else if (this.PlaybackState == MPMoviePlaybackState.Playing)
						{
							this.ParentElement.State = VideoSamples.Library.VideoState.PLAY;
						}

						if (this.PlayableDuration > 1.000)
						{
							this.ParentElement.Info = new VideoData
							{
								State = this.ParentElement.State,
								At = (double.IsNaN(t) ? -1 : t),
								Duration = d
							};
						}
					}
				}

			}, this._Token.Token);
		}

		[Export("LoadChangedCallback:")]
		public void LoadChangedCallback(NSObject o)
		{			
			if (this.ErrorLog != null) {
				System.Diagnostics.Debug.WriteLine (this.ErrorLog.Description);
				ParentElement.HasError = true;
			}
		}
			
		protected internal void Load(NSUrl url, bool http = false)
		{
			ParentElement.HasError = false;
			if (http) {
				// may get error:
				// App Transport Security has blocked a cleartext HTTP (http://) resource load since it is insecure. Temporary exceptions can be configured via your app's Info.plist file.
				// need to modify Info.plist or use https
				this.SourceType = MPMovieSourceType.Streaming;
			} else {
				this.SourceType = MPMovieSourceType.File;
			}
			this.ContentUrl = url;
		}

		protected internal MyVideoPlayer ParentElement
		{
			get {
				return this._ParentElement.Target as MyVideoPlayer;
			}
			set {
				this._ParentElement = new WeakReference (value);
			}
		}

		protected override void Dispose (bool disposing)
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver (MPMoviePlayerController.LoadStateDidChangeNotification);


			this._Token.Cancel ();
			this._Token.Dispose ();

			base.Dispose (disposing);
		}
	}
}

