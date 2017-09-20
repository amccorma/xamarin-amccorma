using System;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using MediaPlayer;
using VideoSamples.Controls;

namespace VideoSamples.iOS.Controls
{
	public class MyMPMoviePlayerController : MPMoviePlayerController
	{
		private WeakReference _ParentElement;

		private CancellationTokenSource _Token;

		private bool _EndOfVideo = true;


		public MyMPMoviePlayerController (MyVideoPlayer Parent)
		{
			ParentElement = Parent;

		    NSNotificationCenter.DefaultCenter.AddObserver(LoadStateDidChangeNotification, LoadChangedCallback);

            _Token = new CancellationTokenSource ();

			Task.Run (async() => {
				while(true && _Token.Token.IsCancellationRequested == false)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(200), _Token.Token);

					var t = CurrentPlaybackTime;
					var d = PlayableDuration;

					// is there a better way to calculate the end of video???
					if (PlayableDuration > 1.000 && (Math.Abs(t - d) <= 0.0001 * double.Epsilon))
					{
						ParentElement.State = Library.VideoState.ENDED;
						ParentElement.Info = new VideoData
						{
							State = Library.VideoState.ENDED,
							At = t,
							Duration = d
						};
						_EndOfVideo = true;
					}
					else
					{
						_EndOfVideo = false;

						if (PlaybackState == MPMoviePlaybackState.Paused)
						{
							ParentElement.State = Library.VideoState.PAUSE;
						}
						else if (PlaybackState == MPMoviePlaybackState.Stopped)
						{
							ParentElement.State = Library.VideoState.STOP;

						}
						else if (PlaybackState == MPMoviePlaybackState.Playing)
						{
							ParentElement.State = Library.VideoState.PLAY;
						}

						if (PlayableDuration > 1.000)
						{
							ParentElement.Info = new VideoData
							{
								State = ParentElement.State,
								At = (double.IsNaN(t) ? -1 : t),
								Duration = d
							};
						}
					}
				}

			}, _Token.Token);
		}

		[Export("LoadChangedCallback:")]
		public void LoadChangedCallback(NSObject o)
		{			
			if (ErrorLog != null) {
				System.Diagnostics.Debug.WriteLine (ErrorLog.Description);
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
				SourceType = MPMovieSourceType.Streaming;
			} else {
				SourceType = MPMovieSourceType.File;
			}
			ContentUrl = url;
		}

		protected internal MyVideoPlayer ParentElement
		{
			get {
				return _ParentElement.Target as MyVideoPlayer;
			}
			set {
				_ParentElement = new WeakReference (value);
			}
		}

		protected override void Dispose (bool disposing)
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver (LoadStateDidChangeNotification);


			_Token.Cancel ();
			_Token.Dispose ();

			base.Dispose (disposing);
		}
	}
}

