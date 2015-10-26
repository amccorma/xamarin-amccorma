using System;
using Xamarin.Forms;
using LabSamples.Library;

namespace LabSamples.Controls
{
	public class MyVideoPlayer : View
	{
		public Action<string> PlayerAction;

		public delegate void Change(VideoData option);

		public delegate void State(string evt);

		public delegate void Buffer(Int32 percent);

		public delegate void Error (string msg);

		public event Buffer OnBufferUpdate;

		public event Change OnChange;

		public event Error OnError;

		public event State OnStateChanged;


		public MyVideoPlayer ()
		{

		}

		public static readonly BindableProperty AutoPlayProperty = 
			BindableProperty.Create<MyVideoPlayer,bool>(
				p => p.AutoPlay, true);

		public static readonly BindableProperty FileSourceProperty = 
			BindableProperty.Create<MyVideoPlayer,string>(
				p => p.FileSource,string.Empty);
		

		public string FileSource {
			get { return (string)GetValue (FileSourceProperty); }
			set { 
				SetValue (FileSourceProperty, value);
			}
		}

		public bool UseBuiltInMediaPlayer 
		{
			get; 
			set;
		}

		public bool AutoPlay
		{
			get { return (bool)GetValue (AutoPlayProperty); }
			set { 
				SetValue (AutoPlayProperty, value);
			}
		}


		public void Stop(){
			if (PlayerAction != null)
				PlayerAction (VideoState.ActionSTOP);
		}

		public void Play(){
			if (PlayerAction != null)
				PlayerAction (VideoState.ActionPlay);
		}

		public void Pause()
		{
			if (PlayerAction != null) {
				PlayerAction (VideoState.ActionPause);
			}
		}

		public static readonly BindableProperty SeekProperty = 
			BindableProperty.Create<MyVideoPlayer,double>(
				p => p.Seek,-1D);

		/// <summary>
		/// Position in MilliSeconds. 
		/// </summary>
		/// <value>The seek.</value>
		public double Seek {
			get { return (double)GetValue (SeekProperty); }
			set { SetValue (SeekProperty, value); }
		}

		public static readonly BindableProperty HasErrorProperty = 
			BindableProperty.Create<MyVideoPlayer,bool>(
				p => p.HasError,false);

		public bool HasError {
			get { return (bool)GetValue (HasErrorProperty); }
			set { SetValue (HasErrorProperty, value); }
		}

		// Event Handlers

		public void FireStateUpdate(string state)
		{
			var handler = OnStateChanged;
			if (handler != null) {
				handler (state);
			}
		}

		public void FireChange(VideoData e)
		{
			var handler = OnChange;
			if (handler != null) {
				handler (e);
			}
		}

		public void FireBufferUpdate(Int32 percent)
		{
			var handler = OnBufferUpdate;
			if (handler != null) {
				handler (percent);
			}
		}

		public void FireError (string msg)
		{
			var handler = OnError;
			if (handler != null) {
				handler (msg);
			}
		}

	}
}

