using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using System.Linq;
using DropDown.iOS;
using DropDown.Forms;
using DropDown.iOS.Control;

[assembly: ExportRenderer (typeof(DropDownPicker), typeof(DropDownViewRenderer))]
namespace DropDown.iOS
{
	public class DropDownViewRenderer : ViewRenderer<DropDownPicker, DropDownView>
	{
		private static UITapGestureRecognizer _Tap;
		private static bool _WindowTapped = false;

		public DropDownViewRenderer ()
		{
			
		}

		protected override void Dispose (bool disposing)
		{
			DropDownPicker.OnMessageTo -= AddMessageTO;
			this.Control.OnChanged -= Control_OnChanged;
			base.Dispose (disposing);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<DropDownPicker> e)
		{
			base.OnElementChanged (e);
			if (this.Control == null) {
				DropDownView view;
				if (this.Element.SelectedBackgroundColor != Xamarin.Forms.Color.Transparent) {
					view = new DropDownView (this.Element.Title, this.Element.Source, this.Element.FontSize, this.Element.CellHeight,
						this.Element.SelectedBackgroundColor.ToUIColor (), this.Element.SelectedTextColor.ToUIColor (),
						this.Element.BorderColor.ToUIColor(),
						this.Element.iOSHeaderText,
						this.Element.SelectedText);
				} else {
					view = new DropDownView (this.Element.Title, this.Element.Source, this.Element.FontSize, this.Element.CellHeight,
						this.Element.BorderColor.ToUIColor(),
						this.Element.iOSHeaderText,
						this.Element.SelectedText);
				}
				view.HeaderHeight = this.Element.iOSHeaderHeight;
				view.HeaderFontHeight = this.Element.iOSHeaderFontSize;
				view.PopupHeight = this.Element.DropDownHeight;
				view.SelectedText = (x) => {
					this.Element.SelectedText = x;
					this.Element.FireSelectedChange();
				};
				SetNativeControl (view);

				this.Control.OnChanged += Control_OnChanged;

				DropDownPicker.OnMessageTo += AddMessageTO;
			}
		}

		private void AddMessageTO(object sender, string msg) 
		{
			if (msg == DropDownPicker.RemoveTapMessage) {
				if (_Tap != null)
				{
					var v = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
					v.RemoveGestureRecognizer(_Tap);
					_WindowTapped = false;
					_Tap = null;
				}
			} else if (msg == DropDownPicker.CloseDropMessage) {
				var o = sender as DropDownPicker;
				if (this.Control != null && o != null && this.Element != null)
				{
					if (o.Id == this.Element.Id)
					{
						this.Control.HideDropDown();
					}
				}
			}
		}

		void Control_OnChanged (object sender, DropDownViewArgs e)
		{
			if (this.Element != null) {
				this.Element.IsShowing = e.IsShowing;
				this.Element.NativeFrame = new Rectangle (e.Frame.X, e.Frame.Y, e.Frame.Width, e.Frame.Height);
			}
		}

		private void HandleWindowTapped()
		{
//			if (_Tap == null && _WindowTapped == false) {
//
//				_WindowTapped = true;
//				_Tap = new UITapGestureRecognizer ((x) => {
//				});
//
//				// handle click outside control to close
//				var w = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
//				_Tap.ShouldReceiveTouch = (recognizer, touch) => {
//					var view = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
//					var pos = touch.LocationInView(view);
//
//					var args = new DropDownTapArgs((float)pos.X, (float)pos.Y);
//					DropDownPicker.SendTapMessage(args);
//					return false;
//				};
//				w.AddGestureRecognizer(_Tap);
//			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == DropDownPicker.FrameProperty.PropertyName) {
				// check for navigation bar and append y
				this.Control.FormsFrame = new CGRect (this.Element.Frame.X, this.Element.Frame.Y, this.Element.Frame.Width, this.Element.Frame.Height);
				this.Control.ControlHeight = (nfloat)this.Element.Height;
				System.Diagnostics.Debug.WriteLine (this.Element.Frame);
			} else if (e.PropertyName == DropDownPicker.TitleProperty.PropertyName) {
				this.Control.Title = this.Element.Title;
			} else if (e.PropertyName == DropDownPicker.SourceProperty.PropertyName) {
				this.Control.UpdateSource (this.Element.Source);
			}
		}
	}
}

