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
using VideoSamples.Library;
using VideoSamples.Controls;
using System.Threading.Tasks;
using System.Threading;

namespace VideoSamples.Droid
{		
	public class MyVideoView : VideoView, Android.Media.MediaPlayer.IOnCompletionListener, Android.Media.MediaPlayer.IOnErrorListener
	{
		private WeakReference _ParentElement;
		private DateTime _TouchStart = DateTime.MinValue;
		private bool _DidDouble = false;
		private bool _HasEnded = false;

		private CancellationTokenSource _Token;

		public MyVideoView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init ();
		}

		public MyVideoView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Init ();
		}

		public MyVideoView(Context context) : base(context)
		{
			Init ();
		}

		protected internal MyVideoPlayer ParentElement
		{
			get {
				return this._ParentElement.Target as MyVideoPlayer;
			}
			set {
				this._ParentElement = new WeakReference (value);
			}
		}

		private void Init()
		{
			this.SetOnCompletionListener(this);
			this.SetOnErrorListener (this);

			this._Token = new CancellationTokenSource ();

			Task.Run (async() => {
				while(true && this._Token.Token.IsCancellationRequested == false)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(200), this._Token.Token);
					if (this._HasEnded)
					{
						this.ParentElement.State = VideoSamples.Library.VideoState.ENDED;
						this.ParentElement.Info = new VideoData
						{
							State = VideoSamples.Library.VideoState.ENDED,
							At = this.CurrentPosition,
							Duration = this.Duration
						};
						this._HasEnded = false;
					}
					else if (this.ParentElement.State != VideoState.ENDED)
					{
						if (this.IsPlaying)
						{
							this.ParentElement.State = VideoSamples.Library.VideoState.PLAY;
						}
						else if (this.ParentElement.State != VideoState.PAUSE)
						{
							this.ParentElement.State = VideoSamples.Library.VideoState.STOP;
						}
						if (this.Duration > 1.000)
						{
							this.ParentElement.Info = new VideoData
							{
								State = this.ParentElement.State,
								At = this.CurrentPosition,
								Duration = this.Duration
							};
						}
					}
				}

			}, this._Token.Token);
		}


		/// <summary>
		/// File FileSource (string)
		/// </summary>
		/// <param name="fullPath">Full path.</param>
		protected internal void LoadFile(string fullPath)
		{ 
			if (String.IsNullOrEmpty (fullPath) == false) 
			{
				if (fullPath.StartsWith ("http")) {
					this.SetVideoURI (global::Android.Net.Uri.Parse (fullPath));
				} else {
					/* raw is the folder the video files are stored under Resources */
					/* fullpath must not include the extension (sample.mp4 = sample) */
					var video = global::Android.Net.Uri.Parse ("android.resource://" + this.Context.PackageName + "/raw/" + fullPath);
					this.SetVideoURI(video);
				}

				if (this.ParentElement.AutoPlay) {
					Play ();
				}
			}
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			if (e.Action == MotionEventActions.Down) {

				if (DateTime.Now.Subtract(this._TouchStart).Milliseconds <= 500 && this._DidDouble == false) {	
					this._DidDouble = true;
					this._TouchStart = DateTime.MinValue;
					// double tap
					ParentElement.FireTap(true);
				} else {
					this._DidDouble = false;
					this._TouchStart = DateTime.Now;
					ParentElement.FireTap(false);
				}

			} 
			return base.OnTouchEvent (e);
		}

		protected internal void Play()
		{
			this._HasEnded = false;
			this.ParentElement.State = VideoState.PLAY;
			this.RequestFocus ();
			this.Start ();
		}


		public override void Pause ()
		{
			ParentElement.State = VideoState.PAUSE;
			base.Pause ();
		}

		public override bool IsPlaying {
			get {					
				return base.IsPlaying;
			}
		}

		public override void StopPlayback ()
		{
			ParentElement.State = VideoState.STOP;
			base.StopPlayback ();
		}

		public override void Resume ()
		{
			this.ParentElement.State = VideoState.PLAY;
			base.Resume ();
		}

		public void OnCompletion (Android.Media.MediaPlayer mp)
		{
			this.ParentElement.State = VideoState.ENDED;
			this._HasEnded = true;
		}

		public bool OnError (Android.Media.MediaPlayer mp, Android.Media.MediaError what, int extra)
		{
			this.ParentElement.HasError = true;
			this.ParentElement.ErrorMessage = what.ToString ();

			this._HasEnded = true;
			// true handle, false allow pass
			return false;
		}

		public override void SeekTo (int msec)
		{
			// fired by moving slider if added
			this.ParentElement.State = VideoState.PLAY;
			this._HasEnded = false;
			base.SeekTo (msec);
		}

		private void UpdateParent()
		{			
			var data = new VideoData ();
			data.At = this.CurrentPosition;
			data.Duration = this.Duration;
			data.State = this.ParentElement.State;

			this.ParentElement.Info = data;
		}

		protected override void Dispose (bool disposing)
		{
			this._Token.Cancel ();
			this._Token.Dispose ();
			this._Token = null;
			base.Dispose (disposing);
		}
	}
}

