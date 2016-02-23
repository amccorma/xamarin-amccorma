using System;
using Xamarin.Forms;
using VideoSamples.Controls;
using VideoSamples.Library;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

[assembly: ExportRenderer(typeof(MyVideoPlayer), typeof(VideoSamples.iOS.Controls.MyVideoPlayerRenderer))]
namespace VideoSamples.iOS.Controls
{
	public class MyVideoPlayerRenderer : ViewRenderer<MyVideoPlayer, UIView>
	{
		private MyPlayerView _PlayerView;
		private DateTime _TouchStart = DateTime.MinValue;
		private bool _DidDouble = false;

		public MyVideoPlayerRenderer ()
		{
		}

		protected override void Dispose (bool disposing)
		{
			if (this._PlayerView != null) {
				this._PlayerView.Dispose ();
				this._PlayerView = null;
			}
			base.Dispose (disposing);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<MyVideoPlayer> e)
		{
			base.OnElementChanged (e);
			if (this.Control == null) {						
				this._PlayerView = new MyPlayerView (this.Element);
				SetNativeControl (this._PlayerView);
			}
				
			this._PlayerView.Load(new NSString(this.Element.FileSource));
			this._PlayerView.FitToWindow = this.Element.FitInWindow;
			this._PlayerView.AddController = this.Element.AddVideoController;

			// autoplay
			if (this.Element.AutoPlay) {
				this._PlayerView.Start ();
			}
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			if (DateTime.Now.Subtract(this._TouchStart).Milliseconds <= 500 && this._DidDouble == false) {	
				this._DidDouble = true;
				this._TouchStart = DateTime.MinValue;
				// double tap
				this.Element.FireTap(true);
			} else {
				this._DidDouble = false;
				this._TouchStart = DateTime.Now;
				this.Element.FireTap(false);
			}
			base.TouchesBegan (touches, evt);
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			var source = this.Element;
			if (source != null && this._PlayerView != null) {
				if (e.PropertyName == MyVideoPlayer.SeekProperty.PropertyName) {
					this._PlayerView.SeekTo ((int)this.Element.Seek);
				} else if (e.PropertyName == MyVideoPlayer.FileSourceProperty.PropertyName) {
					this._PlayerView.Load (new NSString(this.Element.FileSource));
				} else if (e.PropertyName == MyVideoPlayer.PlayerActionProperty.PropertyName) {
					if (source.PlayerAction == VideoState.PAUSE) {
						this._PlayerView.Pause ();
					} else if (source.PlayerAction == VideoState.PLAY) {
						this._PlayerView.Start ();
					} else if (source.PlayerAction == VideoState.RESTART) {
						this._PlayerView.SeekTo (0);
					} else if (source.PlayerAction == VideoState.STOP) {
						this._PlayerView.Stop ();
					}
				} else if (e.PropertyName == MyVideoPlayer.FullScreenProperty.PropertyName) {
					this._PlayerView.FullScreen = source.FullScreen;
				} else if (e.PropertyName == MyVideoPlayer.FitInWindowProperty.PropertyName) {
					this._PlayerView.FitToWindow = source.FitInWindow;
				} else if (e.PropertyName == MyVideoPlayer.AddVideoControllerProperty.PropertyName) {
					this._PlayerView.AddController = source.AddVideoController;
				} else if (e.PropertyName == MyVideoPlayer.ActionBarHideProperty.PropertyName)
				{
					UIApplication.SharedApplication.StatusBarHidden = source.ActionBarHide;	
				} else if (e.PropertyName == MyVideoPlayer.ContentHeightProperty.PropertyName || e.PropertyName ==
				           MyVideoPlayer.ContentWidthProperty.PropertyName) {
					this._PlayerView.UpdateFrame (source);
				}
			}
		}
	}
}

