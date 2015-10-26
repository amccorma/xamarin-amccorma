using System;
using Xamarin.Forms;
using LabSamples.Controls;
using LabSamples.Library;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

[assembly: ExportRenderer(typeof(MyVideoPlayer), typeof(LabSamples.iOS.Controls.MyVideoPlayerRenderer))]
namespace LabSamples.iOS.Controls
{
	public class MyVideoPlayerRenderer : ViewRenderer<MyVideoPlayer, UIView>
	{
		private MyPlayerView _Player;

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
						this._Player.Pause();
					}
					else if (action == VideoState.ActionPlay)
					{
						this._Player.Start();
					}
					else if (action == VideoState.ActionSTOP)
					{
						this._Player.Stop();
					}
				};
					
				this._Player = new MyPlayerView (true, this.Frame);

				SetNativeControl (this._Player);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == MyVideoPlayer.SeekProperty.PropertyName) {
				//this._MyVideoView.SeekTo ((int)this.Element.Seek);
			} else if (e.PropertyName == MyVideoPlayer.FileSourceProperty.PropertyName) {
				this._Player.Load (this.Element.FileSource);
			}
		}
	}
}

