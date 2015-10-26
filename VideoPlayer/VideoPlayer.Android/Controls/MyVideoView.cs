
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using LabSamples.Library;
using LabSamples.Controls;
using System.Threading.Tasks;

namespace LabSamples.Droid
{		
	public class MyVideoView : VideoView, Android.Media.MediaPlayer.IOnCompletionListener, Android.Media.MediaPlayer.IOnErrorListener
	{
		private WeakReference _RootPlayer;

		public MyVideoView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			this.SetOnCompletionListener(this);
		}

		public MyVideoView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			this.SetOnCompletionListener(this);
		}

		public MyVideoView(Context context) : base(context)
		{
			this.SetOnCompletionListener(this);
			this.SetOnErrorListener (this);
		}

		protected internal MyVideoPlayer RootPlayer
		{
			get {
				return this._RootPlayer.Target as MyVideoPlayer;
			}
			set {
				this._RootPlayer = new WeakReference (value);
			}
		}

		public override void Pause ()
		{
			this.RootPlayer.FireStateUpdate (VideoState.ActionPause);
			base.Pause ();
		}

		public override bool IsPlaying {
			get {
				Task.Run (() => {
					this.RootPlayer.FireStateUpdate (VideoState.ActionPlay);
					UpdateParent ();
				});
				return base.IsPlaying;
			}
		}

		public override void Start ()
		{
			this.RootPlayer.FireStateUpdate (VideoState.ActionPlay);
			base.Start ();
		}

		public override int BufferPercentage {
			get {
				this.RootPlayer.FireBufferUpdate (base.BufferPercentage);
				return base.BufferPercentage;
			}
		}

		public void OnCompletion (Android.Media.MediaPlayer mp)
		{
			this.RootPlayer.FireStateUpdate (VideoState.ActionEnded);
		}

		public bool OnError (Android.Media.MediaPlayer mp, Android.Media.MediaError what, int extra)
		{
			this.RootPlayer.FireError (what.ToString ());
			return true;
		}

		public override void SeekTo (int msec)
		{
			// fired by moving slider
			base.SeekTo (msec);
		}

		private void UpdateParent()
		{			
			var data = new VideoData ();
			data.At = this.CurrentPosition;
			data.Duration = this.Duration;

			this.RootPlayer.FireChange (data);
		}
	}
}

