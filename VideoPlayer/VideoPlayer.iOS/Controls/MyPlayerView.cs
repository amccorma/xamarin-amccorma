using System;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
using System.Drawing;

namespace LabSamples.iOS
{
	public class MyPlayerView : UIView
	{
		private AVPlayer _MyAvPlayer;

		public MyPlayerView (bool noController, RectangleF frame)
		{
			this.Frame = frame;
			if (noController) {
				var asset = AVAsset.FromUrl (NSUrl.FromFilename ("sample.m4v"));
				var playerItem = new AVPlayerItem (asset);
				this._MyAvPlayer = new AVPlayer (playerItem); 

				var playerLayer = AVPlayerLayer.FromPlayer (this._MyAvPlayer);
				playerLayer.Frame = this.Frame;
				_MyAvPlayer.Play ();


				this.Layer.AddSublayer (playerLayer);
			}
		}

		public override void Draw (RectangleF rect)
		{
			base.Draw (rect);
		}

		protected internal void Start()
		{
			if (this._MyAvPlayer != null) {
				this._MyAvPlayer.Play ();
			}
		}

		protected internal void Stop()
		{
			if (this._MyAvPlayer != null) {
				this._MyAvPlayer.Pause ();
			}
		}

		protected internal void Pause()
		{
			if (this._MyAvPlayer != null) {
				this._MyAvPlayer.Pause ();
			}
		}

		protected internal void Load(string file)
		{
			if (this._MyAvPlayer != null) {
				
			}
		}
	}
}

