using System;
using Xamarin.Forms;
using VideoSamples.Library;

namespace VideoSamples.Controls
{
	public class MyVideoPlayer : View
	{
		public delegate void Tap(MyVideoPlayer player, bool IsDoubleTap);

		public event Tap OnTap;

		public MyVideoPlayer ()
		{

		}

		public enum ScreenOrientation
		{
			PORTRAIT,
			LANDSCAPE
		}

		public void FireTap(bool IsDoubleTap)
		{
			var handler = this.OnTap;
			if (handler != null) {
				handler (this, IsDoubleTap);
			}
		}

		/// <summary>
		/// ndroid: ActionBar, iOS: StatusBar
		/// </summary>
		public static readonly BindableProperty ActionBarHideProperty = 
			BindableProperty.Create<MyVideoPlayer,bool>(
				p => p.ActionBarHide, false);

		/// <summary>
		/// Android only. no function on iOS
		/// </summary>
		public static readonly BindableProperty ContentHeightProperty = 
			BindableProperty.Create<MyVideoPlayer,double>(
				p => p.ContentHeight, 0);


		/// <summary>
		/// Android only. no function on iOS
		/// </summary>
		public static readonly BindableProperty ContentWidthProperty = 
			BindableProperty.Create<MyVideoPlayer,double>(
				p => p.ContentWidth, 0);


		/// <summary>
		/// Android, iOS
		/// </summary>
		public static readonly BindableProperty AutoPlayProperty = 
			BindableProperty.Create<MyVideoPlayer,bool>(
				p => p.AutoPlay, false);

		/// <summary>
		/// Android, iOS
		/// </summary>
		public static readonly BindableProperty FullScreenProperty = 
			BindableProperty.Create<MyVideoPlayer,bool>(
				p => p.FullScreen, false);
		
		/// <summary>
		/// iOS only.  Android will resize video for player size
		/// </summary>
		public static readonly BindableProperty FitInWindowProperty = 
			BindableProperty.Create<MyVideoPlayer,bool>(
				p => p.FitInWindow, true);

		public static readonly BindableProperty HasErrorProperty = 
			BindableProperty.Create<MyVideoPlayer,bool>(
				p => p.HasError,false);

		public static readonly BindableProperty ErrorMessageProperty = 
			BindableProperty.Create<MyVideoPlayer,string>(
				p => p.ErrorMessage, "");

		public static readonly BindableProperty AddVideoControllerProperty = 
			BindableProperty.Create<MyVideoPlayer,bool>(
				p => p.AddVideoController,false);
		
		public static readonly BindableProperty FileSourceProperty = 
			BindableProperty.Create<MyVideoPlayer,string>(
				p => p.FileSource, "");

		public static readonly BindableProperty StateProperty = 
			BindableProperty.Create<MyVideoPlayer,VideoState>(
				p => p.State, VideoState.NONE);

		public static readonly BindableProperty SeekProperty = 
			BindableProperty.Create<MyVideoPlayer,double>(
				p => p.Seek,-1D);

		public static readonly BindableProperty InfoProperty = 
			BindableProperty.Create<MyVideoPlayer,VideoData>(
				p => p.Info, null, BindingMode.OneWay);		

		/// <summary>
		/// Sends command to the player
		/// </summary>
		public static readonly BindableProperty PlayerActionProperty = 
			BindableProperty.Create<MyVideoPlayer,VideoState>(
				p => p.PlayerAction, VideoState.NONE);	


		/// <summary>
		/// Android only. no function on iOS
		/// </summary>
		public ScreenOrientation Orientation {
			get;
			set;
		}

		/// <summary>
		/// Android only. no function on iOS
		/// </summary>
		public void OrientationChanged()
		{
			OnPropertyChanged ("OrientationChanged");
		}

		/// <summary>
		/// Android: ActionBar, iOS: StatusBar
		/// </summary>
		/// <value><c>true</c> if action bar hide; otherwise, <c>false</c>.</value>
		public bool ActionBarHide
		{
			get { return (bool)GetValue (ActionBarHideProperty); }
			set { 
				SetValue (ActionBarHideProperty, value);
			}
		}

		public string ErrorMessage
		{
			get { return (string)GetValue (ErrorMessageProperty); }
			set { 
				SetValue (ErrorMessageProperty, value);
			}
		}

		/// <summary>
		/// Android only. no function on iOS
		/// </summary>
		public double ContentHeight
		{
			get { return (double)GetValue (ContentHeightProperty); }
			set { 
				SetValue (ContentHeightProperty, value);
			}
		}

		/// <summary>
		/// Android only. no function on iOS
		/// </summary>
		public double ContentWidth
		{
			get { return (double)GetValue (ContentWidthProperty); }
			set { 
				SetValue (ContentWidthProperty, value);
			}
		}

		public VideoData Info
		{
			get { return (VideoData)GetValue (InfoProperty); }
			set { 
				if (value != Info) {
					SetValue (InfoProperty, value);
				}
			}
		}

		/// <summary>
		/// iOS only
		/// </summary>
		public bool FitInWindow
		{
			get { return (bool)GetValue (FitInWindowProperty); }
			set { 
				SetValue (FitInWindowProperty, value);
			}
		}

		public bool AddVideoController
		{
			get { return (bool)GetValue (AddVideoControllerProperty); }
			set { 
				SetValue (AddVideoControllerProperty, value);
			}
		}

		public bool FullScreen
		{
			get { return (bool)GetValue (FullScreenProperty); }
			set { 
				SetValue (FullScreenProperty, value);
			}
		}

		public bool AutoPlay
		{
			get { return (bool)GetValue (AutoPlayProperty); }
			set { 
				SetValue (AutoPlayProperty, value);
			}
		}

		/// <summary>
		/// Position in MilliSeconds. 
		/// </summary>
		/// <value>The seek.</value>
		public double Seek {
			get { return (double)GetValue (SeekProperty); }
			set { 
				// fire change always
				if (value != Seek) {
					SetValue (SeekProperty, value); 
				} else {
					OnPropertyChanged (SeekProperty.PropertyName);
				}
			}
		}

		/// <summary>
		/// Player Action (fire always)
		/// </summary>
		/// <value>The seek.</value>
		public VideoState PlayerAction {
			get { return (VideoState)GetValue (PlayerActionProperty); }
			set { 
				// fire change always
				if (value != PlayerAction) {
					SetValue (PlayerActionProperty, value); 
				} else {
					OnPropertyChanged (PlayerActionProperty.PropertyName);
				}
			}
		}

		public bool HasError {
			get { return (bool)GetValue (HasErrorProperty); }
			set { 
				SetValue (HasErrorProperty, value); 
			}
		}

		/// <summary>
		/// Video State
		/// </summary>
		/// <value>The state.</value>
		public VideoState State {
			get { return (VideoState)GetValue (StateProperty); }
			set { SetValue (StateProperty, value); }
		}

		/// <summary>
		/// File string (http or Content or Asset)
		/// </summary>
		/// <value>The file source.</value>
		public string FileSource {
			get { return (string)GetValue (FileSourceProperty); }
			set { SetValue (FileSourceProperty, value); }
		}
	}
}

