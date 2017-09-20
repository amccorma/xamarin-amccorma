using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using VideoSamples.Controls;
using VideoSamples.iOS;
using VideoSamples.iOS.Controls;
using VideoSamples.Library;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyVideoPlayer), typeof(MyVideoPlayerRenderer))]
namespace VideoSamples.iOS
{
	public class MyVideoPlayerRenderer : ViewRenderer<MyVideoPlayer, UIView>
	{
		private MyPlayerView _PlayerView;
		private DateTime _TouchStart = DateTime.MinValue;
		private bool _DidDouble;

		protected override void Dispose (bool disposing)
		{
			if (_PlayerView != null) {
				_PlayerView.Dispose ();
				_PlayerView = null;
			}
			base.Dispose (disposing);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<MyVideoPlayer> e)
		{
			base.OnElementChanged (e);
			if (Control == null) {						
				_PlayerView = new MyPlayerView (Element);
				SetNativeControl (_PlayerView);
			}
				
			_PlayerView.Load(new NSString(Element.FileSource));
			_PlayerView.FitToWindow = Element.FitInWindow;
			_PlayerView.AddController = Element.AddVideoController;

			// autoplay
			if (Element.AutoPlay) {
				_PlayerView.Start ();
			}
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			if (DateTime.Now.Subtract(_TouchStart).Milliseconds <= 500 && _DidDouble == false) {	
				_DidDouble = true;
				_TouchStart = DateTime.MinValue;
				// double tap
				Element.FireTap(true);
			} else {
				_DidDouble = false;
				_TouchStart = DateTime.Now;
				Element.FireTap(false);
			}
			base.TouchesBegan (touches, evt);
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			var source = Element;
			if (source != null && _PlayerView != null) {
				if (e.PropertyName == MyVideoPlayer.SeekProperty.PropertyName) {
					_PlayerView.SeekTo ((int)Element.Seek);
				} else if (e.PropertyName == MyVideoPlayer.FileSourceProperty.PropertyName) {
					_PlayerView.Load (new NSString(Element.FileSource));
				} else if (e.PropertyName == MyVideoPlayer.PlayerActionProperty.PropertyName) {
					if (source.PlayerAction == VideoState.PAUSE) {
						_PlayerView.Pause ();
					} else if (source.PlayerAction == VideoState.PLAY) {
						_PlayerView.Start ();
					} else if (source.PlayerAction == VideoState.RESTART) {
						_PlayerView.SeekTo (0);
					} else if (source.PlayerAction == VideoState.STOP) {
						_PlayerView.Stop ();
					}
				} else if (e.PropertyName == MyVideoPlayer.FullScreenProperty.PropertyName) {
					_PlayerView.FullScreen = source.FullScreen;
				} else if (e.PropertyName == MyVideoPlayer.FitInWindowProperty.PropertyName) {
					_PlayerView.FitToWindow = source.FitInWindow;
				} else if (e.PropertyName == MyVideoPlayer.AddVideoControllerProperty.PropertyName) {
					_PlayerView.AddController = source.AddVideoController;
				} else if (e.PropertyName == MyVideoPlayer.ActionBarHideProperty.PropertyName)
				{
					UIApplication.SharedApplication.StatusBarHidden = source.ActionBarHide;	
				} else if (e.PropertyName == MyVideoPlayer.ContentHeightProperty.PropertyName || e.PropertyName ==
				           MyVideoPlayer.ContentWidthProperty.PropertyName) {
					_PlayerView.UpdateFrame (source);
				}
			}
		}
	}
}

