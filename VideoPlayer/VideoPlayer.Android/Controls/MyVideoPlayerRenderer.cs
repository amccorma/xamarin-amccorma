using System;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.Media;
using Android.Content.Res;
using Android.Widget;
using Xamarin.Forms;
using LabSamples.Controls;
using LabSamples.Library;

[assembly: ExportRenderer(typeof(MyVideoPlayer), typeof(LabSamples.Droid.Controls.MyVideoPlayerRenderer))]
namespace LabSamples.Droid.Controls
{
	public class MyVideoPlayerRenderer : ViewRenderer<MyVideoPlayer, Android.Widget.LinearLayout>, ISurfaceHolderCallback
	{
		private MediaPlayer _MPlayer;
		private MediaController _MCController;
		private MyVideoView _MyVideoView;

		public MyVideoPlayerRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<MyVideoPlayer> e)
		{
			base.OnElementChanged (e);
			if (this.Control == null) {	
				this.Element.PlayerAction = (action) => {
					if (action == VideoState.ActionPause)
					{
						this._MyVideoView.Pause();
					}
					else if (action == VideoState.ActionPlay)
					{
						this._MyVideoView.Start();
					}
					else if (action == VideoState.ActionSTOP)
					{
						this._MyVideoView.StopPlayback();
					}
				};

				var layoutInflater = (LayoutInflater)Context.GetSystemService(global::Android.Content.Context.LayoutInflaterService);
				var video = (Android.Widget.LinearLayout)layoutInflater.Inflate (LabSamples.Droid.Resource.Layout.VideoLayout, null);			
				SetNativeControl (video);
			}

			this._MyVideoView = this.Control.FindViewById<MyVideoView>(LabSamples.Droid.Resource.Id.videoView1);

			// must set reference to root element
			this._MyVideoView.RootPlayer = this.Element;

			// pick controller
			if (this.Element.UseBuiltInMediaPlayer) {
				this._MCController = new MediaController (this.Context);
				this._MCController.SetMediaPlayer (this._MyVideoView);
				this._MyVideoView.SetMediaController (this._MCController);
			} else {
				this._MyVideoView.SetZOrderOnTop (true);
				this._MyVideoView.Holder.AddCallback (this);
				this._MPlayer = new MediaPlayer ();
			}

			// play
			play (this.Element.FileSource);
		}


		public void SurfaceCreated (ISurfaceHolder holder)
		{
			_MPlayer.SetDisplay (holder);
		}

		public void SurfaceChanged (ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
		{

		}

		public void SurfaceDestroyed (ISurfaceHolder holder)
		{

		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == MyVideoPlayer.SeekProperty.PropertyName) {
				this._MyVideoView.SeekTo ((int)this.Element.Seek);
			} else if (e.PropertyName == MyVideoPlayer.FileSourceProperty.PropertyName) {
				play (this.Element.FileSource);
			}
		}

		private void play(string fullPath)
		{ 
			if (String.IsNullOrEmpty (fullPath) == false) {
				if (this._MPlayer != null) {
					if (fullPath.StartsWith ("http")) {
						this._MPlayer.SetDataSource (fullPath);
					} else {
						AssetFileDescriptor afd = Xamarin.Forms.Forms.Context.Assets.OpenFd (fullPath);
						if (afd != null) {
							this._MPlayer.SetDataSource (afd.FileDescriptor, afd.StartOffset, afd.Length);
						}
					}

					this._MPlayer.Prepare ();
					if (this.Element.AutoPlay) {
						this._MPlayer.Start ();
						this._MyVideoView.Start ();
					}

				} else if (_MCController != null) {
					if (fullPath.StartsWith ("http")) {
						this._MyVideoView.SetVideoURI (global::Android.Net.Uri.Parse (fullPath));
					} else {					
						
						/* raw is the folder the video files are stored under Resources */
						/* fullpath must not include the extension (sample.mp4 = sample) */
						var video = global::Android.Net.Uri.Parse ("android.resource://" + this.Context.PackageName + "/raw/" + fullPath);
						this._MyVideoView.SetVideoURI(video);
					}

					if (this.Element.AutoPlay) {
						this._MyVideoView.RequestFocus ();
						this._MyVideoView.Start ();
					}
				}
			}
		}
	}
}

