using System;
using Android.Views;
using System.Threading.Tasks;
using Android.Animation;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Java.Interop;
using System.Threading;

namespace TopAlert.Droid
{
	public class TopAlertView
	{
		private AnimatorSet _Set;
		private LinearLayout _Layout;
		private ObjectAnimator _fadeOut;
		private bool _IsInit = false;
		private  Int32 _Delay = 0;
		private CancellationTokenSource _Token;

		public TopAlertView ()
		{
			
		}

		public void Kill()
		{
			Stop ();
		}

		private void LayoutTouched(object sender, View.TouchEventArgs e)
		{
			Stop ();
		}


		void _fadeOut_AnimationEnd (object sender, EventArgs e)
		{
			this._Token.Cancel ();
			this._Token.Dispose ();
			this._Token = null;

			_Layout.Touch -= LayoutTouched;
			_Layout.Visibility = ViewStates.Gone;
			_fadeOut.AnimationEnd -= _fadeOut_AnimationEnd;

			_Set.Dispose ();
			_Set = null;

			_fadeOut.RemoveAllListeners ();
			_fadeOut.Dispose ();
			_fadeOut = null;

			_Layout.Dispose ();
			_Layout = null;
		}

		private async Task Stop()
		{	
			if (_fadeOut != null && _Set != null && _Set.IsRunning)
			{
				await Task.Delay(_Delay);
			}

			if (_Layout != null) {

				if (this._Token != null) {
					this._Token.Cancel ();
					this._Token.Dispose ();
					this._Token = null;
				}

				_Layout.Touch -= LayoutTouched;

				Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
					if (this._Layout != null)
					{
						this._Layout.Visibility = ViewStates.Gone;
						this._Layout.Dispose ();
						this._Layout = null;
					}
				});
			}

			if (_Set != null) {	
				this._Set.RemoveAllListeners ();
				this._Set.End ();
				this._Set.Cancel ();
				this._Set.Dispose ();
				this._Set = null;
			}
			if (_fadeOut != null) {

				this._fadeOut.AnimationEnd -= _fadeOut_AnimationEnd;
				this._fadeOut.RemoveAllListeners ();
				this._fadeOut.Dispose ();
				this._fadeOut = null;
			}
		}

		public async Task Show(TopAlert alert)
		{
			await Stop ();

			this._Token = new CancellationTokenSource ();

			this._Delay = alert.Duration;

			var activity = Xamarin.Forms.Forms.Context as Android.App.Activity;
			IWindowManager windowManager = Xamarin.Forms.Forms.Context.GetSystemService(Android.App.Service.WindowService).JavaCast<IWindowManager>();
			this._Layout = (LinearLayout)activity.LayoutInflater.Inflate(Resource.Layout.AlertBox, null, false);
			this._Layout.LayoutParameters = new ViewGroup.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

			var submain = this._Layout.FindViewById<LinearLayout> (Resource.Id.linearLayout3);
			submain.SetBackgroundColor (alert.BackgroundColor.ToAndroid());

			var main = _Layout.FindViewById<LinearLayout> (Resource.Id.linearLayout1);

			var id = Xamarin.Forms.Forms.Context.Resources.GetIdentifier ("alertborder", "drawable", Xamarin.Forms.Forms.Context.PackageName);
			Android.Graphics.Drawables.GradientDrawable drawable = 
				Xamarin.Forms.Forms.Context.Resources.GetDrawable (id).JavaCast<Android.Graphics.Drawables.GradientDrawable>();


			var text = submain.FindViewById<TextView> (Resource.Id.textView1);
			text.SetTextColor (alert.TextColor.ToAndroid ());
			text.Text = alert.Text;

			if (alert.TextSize > 0) {
				text.TextSize = alert.TextSize;
			}

			drawable.SetColor (alert.BorderColor.ToAndroid ());
			drawable.SetCornerRadius (alert.BorderWidth);
			main.SetBackground (drawable);

			//activity.ActionBar.Hide ();

			var actionBarHeight = activity.ActionBar.Height;

			var intent = alert.Intent;

			var p = new WindowManagerLayoutParams (
				windowManager.DefaultDisplay.Width - intent * 2,
				(alert.AlertHeight < 0 ? 200 : (int)alert.AlertHeight),
				WindowManagerTypes.SystemAlert,
				0 | WindowManagerFlags.NotFocusable,
				Android.Graphics.Format.Translucent);

			var yOffset = alert.TopOffset;
			p.Gravity = GravityFlags.Top | GravityFlags.Left;
			p.X = intent;
			p.Y = alert.TopOffset + yOffset + (activity.ActionBar.IsShowing ? actionBarHeight : 0);
			p.Height = (alert.AlertHeight < 0 ? 200 : (int)alert.AlertHeight);
			windowManager.AddView (_Layout, p);

			this._Layout.Touch += LayoutTouched;

			Task.Run (async() => {

				await Task.Delay(alert.Duration, this._Token.Token);

				if (this._Token != null && this._Token.IsCancellationRequested == false)
				{
					if(alert.FadeOut)
					{
						Xamarin.Forms.Device.BeginInvokeOnMainThread( () => {
							// this works
							this._fadeOut = ObjectAnimator.OfFloat(_Layout, "alpha", 1f, 0f);
							this._fadeOut.SetDuration(1000);
							this._fadeOut.AnimationEnd += _fadeOut_AnimationEnd;
							this._Set = new AnimatorSet();
							this._Set.Play(_fadeOut);
							this._Set.Start();
						});
					}
					else
					{
						Stop();
					}
				}
			});
		}
	}
}

