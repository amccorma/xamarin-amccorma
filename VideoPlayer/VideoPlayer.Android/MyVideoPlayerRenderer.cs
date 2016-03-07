using System;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.Media;
using Android.Content.Res;
using Android.Widget;
using Xamarin.Forms;
using VideoSamples.Controls;
using VideoSamples.Library;
using Android.Runtime;
using Android.App;
using Android.Util;
using Android.Graphics;

[assembly: ExportRenderer(typeof(MyVideoPlayer), typeof(VideoSamples.Droid.MyVideoPlayerRenderer))]
namespace VideoSamples.Droid
{
	public class MyVideoPlayerRenderer : ViewRenderer<MyVideoPlayer, Android.Widget.RelativeLayout>
	{
		private MediaController _MCController;
		private MyVideoView _MyVideoView;
		private bool _AttachedController;
		private Android.Widget.RelativeLayout _MainLayout;

		public MyVideoPlayerRenderer ()
		{
			this._AttachedController = false;
		}

		protected override void Dispose (bool disposing)
		{
			if (this._MCController != null && this._MyVideoView != null) {
				this._MyVideoView.SetMediaController (null);
			}
			if (this._MCController != null) {
				this._MCController.Dispose ();
				this._MCController = null;
			}

			if (this._MyVideoView != null) {
				this._MyVideoView.StopPlayback ();
				this._MyVideoView.Dispose ();
				this._MyVideoView = null;
			}
			base.Dispose (disposing);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<MyVideoPlayer> e)
		{
			base.OnElementChanged (e);
			if (this.Control == null) {	
				var layoutInflater = (LayoutInflater)Context.GetSystemService(global::Android.Content.Context.LayoutInflaterService);
				this._MainLayout = (Android.Widget.RelativeLayout)layoutInflater.Inflate (VideoSamples.Droid.Resource.Layout.VideoLayout, null);	
				SetNativeControl (this._MainLayout);
			}

			this._MyVideoView = this.Control.FindViewById<MyVideoView>(VideoSamples.Droid.Resource.Id.videoView1);


			// full screen hack?  
			ResizeScreen (true); //this.Element.FullScreen);

			// must set reference to root element
			this._MyVideoView.ParentElement = this.Element;

			// pick controller
			this._MCController = new MediaController (this.Context);
			this._MCController.SetMediaPlayer (this._MyVideoView);

			if (this.Element.AddVideoController) {				
				this._AttachedController = true;
				this._MyVideoView.SetMediaController (this._MCController);
			} else {
				this._AttachedController = false;
			}

			// load file
			this._MyVideoView.LoadFile (this.Element.FileSource);

			if (this.Element.AutoPlay) {
				// play if set to autoplay on load
				this._MyVideoView.Play();
			}
		}
			
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			var source = this.Element;
			if (source != null && this._MyVideoView != null) {
				if (e.PropertyName == MyVideoPlayer.SeekProperty.PropertyName) {
					this._MyVideoView.SeekTo ((int)this.Element.Seek);
				} else if (e.PropertyName == MyVideoPlayer.FileSourceProperty.PropertyName) {

					// load the play file
					this._MyVideoView.LoadFile (this.Element.FileSource);
					this._MyVideoView.Play ();
				} else if (e.PropertyName == MyVideoPlayer.AddVideoControllerProperty.PropertyName) {
					if (source.AddVideoController && this._AttachedController == false) {
						this._MyVideoView.SetMediaController (this._MCController);
					} else {
						this._MyVideoView.SetMediaController (null);
					}
				} else if (e.PropertyName == MyVideoPlayer.FullScreenProperty.PropertyName) {
					ResizeScreen (source.FullScreen);
				} else if (e.PropertyName == "OrientationChanged") {
					ResizeScreen (source.FullScreen);
				} else if (e.PropertyName == MyVideoPlayer.ActionBarHideProperty.PropertyName) {
					ResizeScreen (source.FullScreen);
				} else if (e.PropertyName == MyVideoPlayer.PlayerActionProperty.PropertyName) {
					if (source.PlayerAction == VideoState.PAUSE) {
						this._MyVideoView.Pause ();
					} else if (source.PlayerAction == VideoState.PLAY) {
						this._MyVideoView.Play ();
					} else if (source.PlayerAction == VideoState.RESTART) {
						this._MyVideoView.SeekTo (0);
						this._MyVideoView.Play ();
					} else if (source.PlayerAction == VideoState.STOP) {
						this._MyVideoView.StopPlayback ();
					}
				}
			}
		}

		private void ResizeScreen(bool fullscreen)
		{
			var a = this.Context as Activity;
			//getWindow().requestFeature(Window.FEATURE_ACTION_BAR);
			//getActionBar().hide();

			//a.Window.RequestFeature (WindowFeatures.ActionBar);
			if (a.ActionBar != null) {
				if (this.Element.ActionBarHide) {
					a.ActionBar.Hide ();
				} else {
					a.ActionBar.Show ();
				}
			} else {
				this.Element.ActionBarHide = true;
			}
			if (fullscreen) {
				var p = this._MyVideoView.LayoutParameters as Android.Widget.RelativeLayout.LayoutParams;
				p.Height = Android.Widget.RelativeLayout.LayoutParams.FillParent;

				// added works ok for rotation
				var view = a.Window.DecorView;
				Rect rect = new Rect ();
				view.GetWindowVisibleDisplayFrame (rect);

				var width = (int)this.Element.ContentWidth;
				var height = (this.Element.ActionBarHide) ? rect.Height() : (int)this.Element.ContentHeight; 
				var holder = this._MyVideoView.Holder;

				p.Height = height;
				p.Width = width;

				holder.SetFixedSize (width, height);
				// end

				p.AlignWithParent = true;
				this._MyVideoView.LayoutParameters = p;

			} else {
				var p = this._MyVideoView.LayoutParameters as Android.Widget.RelativeLayout.LayoutParams;
				if (this.Element.HeightRequest > 0 || this.Element.WidthRequest > 0) {
					if (this.Element.HeightRequest > 0) {
						p.Height = (int)this.Element.HeightRequest;
					}
					if (this.Element.WidthRequest > 0) {
						p.Width = (int)this.Element.WidthRequest;
					}
					this._MyVideoView.LayoutParameters = p;
				}
				p.AlignWithParent = false;
				this._MyVideoView.LayoutParameters = p;
			}

			InvalidLayout ();
		}

		private void InvalidLayout()
		{
			if (this.Element.Orientation == MyVideoPlayer.ScreenOrientation.LANDSCAPE) {

			}
			Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {

				this._MyVideoView.ForceLayout();
				this._MyVideoView.RequestLayout ();
				this._MyVideoView.Holder.SetSizeFromLayout();
				this._MyVideoView.Invalidate ();

			});
		}
	}
}

