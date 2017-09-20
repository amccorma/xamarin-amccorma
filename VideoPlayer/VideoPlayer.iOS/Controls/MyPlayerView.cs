using System;
using CoreGraphics;
using Foundation;
using MediaPlayer;
using UIKit;
using VideoSamples.Controls;

namespace VideoSamples.iOS.Controls
{
	public class MyPlayerView : UIView
	{
		MyMPMoviePlayerController _MoviePlayer;
		private bool _UsingFullWidth;
		private bool _UsingFullHeight;


		public MyPlayerView (MyVideoPlayer Parent)
		{
			BackgroundColor = UIColor.Clear;

			_MoviePlayer = new MyMPMoviePlayerController (Parent);	

			// add to subview
			AddSubview (_MoviePlayer.View);
		}

	    public override void Draw(CGRect rect)
	    {
	        base.Draw(rect);

	        var screenRect = UIScreen.MainScreen.Bounds;
	        var w = screenRect.Size.Width;
	        var h = screenRect.Size.Height;

	        if (_UsingFullWidth)
	        {
	            // screen - top bar (nav + statusbar)
	            rect.Height = h - TopBarSize();
	        }
	        if (_UsingFullWidth)
	        {
	            rect.Width = w;
	        }

	        _MoviePlayer.View.Frame = rect;
	        Frame = rect;

	    }

        private Int32 TopBarSize()
		{
			var s = 0;
			var h = 0;

			// find Navigationbar. could be null if no navigationbar.
			// xamarin support hack
			var hasNav = UIApplication.SharedApplication.KeyWindow.Subviews.FirstOrDefaultFromMany (item => item.Subviews, x => x is UINavigationBar);

			if (UIApplication.SharedApplication.StatusBarHidden == false) {
				s = Convert.ToInt32 (UIApplication.SharedApplication.StatusBarFrame.Height);
			}

			if (hasNav != null) {
				h = Convert.ToInt32 (hasNav.Frame.Height);
			}

			return s + h;

		}

		protected internal void UpdateFrame(MyVideoPlayer source)
		{
			UIApplication.SharedApplication.InvokeOnMainThread (() => {
				_UsingFullWidth = false;
				_UsingFullHeight = false;

				var screenRect = UIScreen.MainScreen.Bounds;
				var w = screenRect.Size.Width;
				var h = screenRect.Size.Height;

				// update frame
				var f = Frame;
				if (source.ContentWidth <= 0) {
					f.Width = w;
					_UsingFullWidth = true;
				}
				else
				{
					f.Width = Convert.ToSingle (source.ContentWidth);
				}

				if (source.ContentHeight <= 0) {
					f.Height = h;
					_UsingFullHeight = true;
				} else {
					f.Height = Convert.ToSingle (source.ContentHeight);
				}


				Frame = f;
				SetNeedsDisplay ();
			});
		}

		protected internal void Start()
		{
			if (_MoviePlayer.PlaybackState == MPMoviePlaybackState.Paused) {
				// handle pause
				_MoviePlayer.Play ();
			} else {
				_MoviePlayer.PrepareToPlay ();
			}
		}

		protected internal void Stop()
		{			
			_MoviePlayer.Stop ();
		}

		protected internal void Pause()
		{
			_MoviePlayer.Pause ();
		}

		protected internal void SeekTo(double pos)
		{
			_MoviePlayer.CurrentPlaybackTime = pos;
		}

		/// <summary>
		/// TimBarton fix 02/2016 (https://github.com/amccorma/xamarin-amccorma/issues/1)
		/// </summary>
		/// <param name="file">File.</param>
		protected internal void Load(NSString file)
		{
			// file must be set to Content
			// url starts with http
			if (String.IsNullOrEmpty (file) == false) {
				if (file.ToString().ToLower().StartsWith ("http") == false) {
					_MoviePlayer.Load (NSUrl.FromFilename (file), false);
				} else {
					_MoviePlayer.Load (NSUrl.FromString (file), true);
				}
			}
		}


		protected internal bool FullScreen {			
			set {
				if (value) {
					_MoviePlayer.SetFullscreen (true, true);
				} else {
					_MoviePlayer.SetFullscreen (false, true);
				}
			}
		}

		protected internal bool FitToWindow
		{
			set {
				if (value) {
					_MoviePlayer.View.ClipsToBounds = true;
					_MoviePlayer.ScalingMode = MPMovieScalingMode.AspectFill;
				} else {
					_MoviePlayer.View.ClipsToBounds = false;
					_MoviePlayer.ScalingMode = MPMovieScalingMode.None;
				}
			}
		}

		protected internal bool AddController
		{
			set {
				if (value) {
					_MoviePlayer.ControlStyle = MPMovieControlStyle.Embedded;
				} else {
					_MoviePlayer.ControlStyle = MPMovieControlStyle.None;
				}
			}
		}

		protected override void Dispose (bool disposing)
		{
			_MoviePlayer.Dispose ();
			base.Dispose (disposing);
		}
	}
}

