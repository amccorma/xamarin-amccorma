using System;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
using System.Drawing;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.MediaPlayer;
using VideoSamples.Controls;
using System.Linq;

namespace VideoSamples.iOS
{
	public class MyPlayerView : UIView
	{
		MyMPMoviePlayerController _MoviePlayer;
		private bool _UsingFullWidth;
		private bool _UsingFullHeight;


		public MyPlayerView (MyVideoPlayer Parent)
		{
			this.BackgroundColor = UIColor.Clear;

			this._MoviePlayer = new MyMPMoviePlayerController (Parent);	

			// add to subview
			this.AddSubview (this._MoviePlayer.View);
		}

		public override void Draw (RectangleF rect)
		{
			base.Draw (rect);

			var screenRect = UIScreen.MainScreen.Bounds;
			var w = screenRect.Size.Width;
			var h = screenRect.Size.Height;

			if (this._UsingFullHeight) {
				// screen - top bar (nav + statusbar)
				rect.Height = h - TopBarSize();
			}
			if (this._UsingFullWidth) {
				rect.Width = w;
			}

			this._MoviePlayer.View.Frame = rect;
			this.Frame = rect;
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
				this._UsingFullWidth = false;
				this._UsingFullHeight = false;

				var screenRect = UIScreen.MainScreen.Bounds;
				var w = screenRect.Size.Width;
				var h = screenRect.Size.Height;

				// update frame
				var f = this.Frame;
				if (source.ContentWidth <= 0) {
					f.Width = w;
					this._UsingFullWidth = true;
				}
				else
				{
					f.Width = Convert.ToSingle (source.ContentWidth);
				}

				if (source.ContentHeight <= 0) {
					f.Height = h;
					this._UsingFullHeight = true;
				} else {
					f.Height = Convert.ToSingle (source.ContentHeight);
				}


				this.Frame = f;
				this.SetNeedsDisplay ();
			});
		}

		protected internal void Start()
		{
			if (this._MoviePlayer.PlaybackState == MPMoviePlaybackState.Paused) {
				// handle pause
				this._MoviePlayer.Play ();
			} else {
				this._MoviePlayer.PrepareToPlay ();
			}
		}

		protected internal void Stop()
		{			
			this._MoviePlayer.Stop ();
		}

		protected internal void Pause()
		{
			this._MoviePlayer.Pause ();
		}

		protected internal void SeekTo(double pos)
		{
			this._MoviePlayer.CurrentPlaybackTime = pos;
		}

		protected internal void Load(string file)
		{
			// file must be set to Content
			// url starts with http
			if (String.IsNullOrEmpty (file) == false) {
				file = file.ToLower ();
				if (file.StartsWith ("http") == false) {
					this._MoviePlayer.Load (NSUrl.FromFilename (file), false);
				} else {
					this._MoviePlayer.Load (NSUrl.FromString (file), true);
				}
			}
		}


		protected internal bool FullScreen {			
			set {
				if (value) {
					this._MoviePlayer.SetFullscreen (true, true);
				} else {
					this._MoviePlayer.SetFullscreen (false, true);
				}
			}
		}

		protected internal bool FitToWindow
		{
			set {
				if (value) {
					this._MoviePlayer.View.ClipsToBounds = true;
					this._MoviePlayer.ScalingMode = MPMovieScalingMode.AspectFill;
				} else {
					this._MoviePlayer.View.ClipsToBounds = false;
					this._MoviePlayer.ScalingMode = MPMovieScalingMode.None;
				}
			}
		}

		protected internal bool AddController
		{
			set {
				if (value) {
					this._MoviePlayer.ControlStyle = MPMovieControlStyle.Embedded;
				} else {
					this._MoviePlayer.ControlStyle = MPMovieControlStyle.None;
				}
			}
		}

		protected override void Dispose (bool disposing)
		{
			this._MoviePlayer.Dispose ();
			base.Dispose (disposing);
		}
	}
}

