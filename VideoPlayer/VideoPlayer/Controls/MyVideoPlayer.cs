using VideoSamples.Library;
using Xamarin.Forms;

namespace VideoSamples.Controls
{
	public class MyVideoPlayer : View
	{
		public delegate void Tap(MyVideoPlayer player, bool IsDoubleTap);

		public event Tap OnTap;


		public enum ScreenOrientation
		{
			PORTRAIT,
			LANDSCAPE
		}

		public void FireTap(bool IsDoubleTap)
		{
			var handler = this.OnTap;
		    handler?.Invoke (this, IsDoubleTap);
		}

		/// <summary>
		/// Android: ActionBar, iOS: StatusBar
		/// </summary>
		public static readonly BindableProperty ActionBarHideProperty = 
			BindableProperty.Create("ActionBarHide", typeof(bool), typeof(MyVideoPlayer), false);
		
		/// <summary>
		/// Android only. no function on iOS
		/// </summary>
		public static readonly BindableProperty ContentHeightProperty = 
			BindableProperty.Create ("ContentHeight", typeof(double), typeof(MyVideoPlayer), 0D);
		

		/// <summary>
		/// Android only. no function on iOS
		/// </summary>
		public static readonly BindableProperty ContentWidthProperty = 
			BindableProperty.Create("ContentWidth", typeof(double), typeof(MyVideoPlayer), 0D);

		/// <summary>
		/// Android, iOS
		/// </summary>
		public static readonly BindableProperty AutoPlayProperty = 
			BindableProperty.Create("AutoPlay", typeof(bool), typeof(MyVideoPlayer), false);	

		/// <summary>
		/// iOS only.  Android will resize video for player size
		/// </summary>
		public static readonly BindableProperty FitInWindowProperty = 
			BindableProperty.Create("FitInWindow", typeof(bool), typeof(MyVideoPlayer), true);

		public static readonly BindableProperty FullScreenProperty = 
			BindableProperty.Create("FullScreen", typeof(bool), typeof(MyVideoPlayer), false);

		public static readonly BindableProperty HasErrorProperty = 
			BindableProperty.Create("HasError", typeof(bool), typeof(MyVideoPlayer), false);

		public static readonly BindableProperty ErrorMessageProperty = 
			BindableProperty.Create("ErrorMessage", typeof(string), typeof(MyVideoPlayer), "");		
		
		public static readonly BindableProperty AddVideoControllerProperty = 
			BindableProperty.Create("AddVideoController", typeof(bool), typeof(MyVideoPlayer), false);

		public static readonly BindableProperty FileSourceProperty = 
			BindableProperty.Create("FileSource", typeof(string), typeof(MyVideoPlayer), "");
		
		public static readonly BindableProperty StateProperty = 
			BindableProperty.Create("State", typeof(VideoState), typeof(MyVideoPlayer), VideoState.NONE);

		public static readonly BindableProperty SeekProperty = 
			BindableProperty.Create("Seek", typeof(double), typeof(MyVideoPlayer), -1D);

		public static readonly BindableProperty InfoProperty = 
			BindableProperty.Create("Info", typeof(VideoData), typeof(MyVideoPlayer),  null, BindingMode.OneWay);

		/// <summary>
		/// Sends command to the player
		/// </summary>
		public static readonly BindableProperty PlayerActionProperty = 
			BindableProperty.Create("PlayerAction", typeof(VideoState), typeof(MyVideoPlayer), VideoState.NONE);

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

