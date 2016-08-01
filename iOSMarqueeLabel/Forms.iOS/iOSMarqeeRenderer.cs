using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using MarqueeBinding;
using FormsApp;

[assembly: ExportRenderer(typeof(iOSMarqueeLabel), typeof(FormsiOS.iOSMarqeeRenderer))]
namespace FormsiOS
{
	public class iOSMarqeeRenderer : ViewRenderer<iOSMarqueeLabel, MarqueeBinding.MarqueeLabel>
    {
		private UITapGestureRecognizer tapXamarin;

		public iOSMarqeeRenderer()
        {
        }

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			if (this.Control != null) {
				this.Control.Dispose ();
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<iOSMarqueeLabel> e)
        {
            base.OnElementChanged(e);
			var view = Element as iOSMarqueeLabel;
			if (view != null) {
				if (this.Control == null) {
					var c = new MarqueeBinding.MarqueeLabel();
					SetNativeControl (c);
				}

				UpdateMarquee(this.Control);

				//AddNavigateUrl (view.NavigateUri);
				//UpdateUi ();
			}
        }

		private void UpdateUi()
		{
			if (this.Element != null && this.Control != null) {
				//Do not create attributed string if it is not necesarry
				if (!this.Element.IsUnderline && !this.Element.IsStrikeThrough) {
					return;
				}

				var underline = this.Element.IsUnderline ? NSUnderlineStyle.Single : NSUnderlineStyle.None;
				var strikethrough = this.Element.IsStrikeThrough ? NSUnderlineStyle.Single : NSUnderlineStyle.None;

				NSShadow dropShadow = null;

				// For some reason, if we try and convert Color.Default to a UIColor, the resulting color is
				// either white or transparent. The net result is the ExtendedLabel does not display.
				// Only setting the control's TextColor if is not Color.Default will prevent this issue.
				if (this.Element.TextColor != Color.Default) {
					this.Control.TextColor = this.Element.TextColor.ToUIColor ();
				}

				if (this.Control.Text != null) {
					this.Control.AttributedText = new NSMutableAttributedString (this.Control.Text,
						this.Control.Font,
						underlineStyle: underline,
						strikethroughStyle: strikethrough,
						shadow: dropShadow);
				}

			}
		}

		void UpdateMarquee(MarqueeBinding.MarqueeLabel label)
		{
			if (this.Element != null && label != null) {
				//label.HoldScrolling = true;

				if (this.Element.ScrollType == iOSMarqueeLabel.MarqueeType.Continuous) {
					label.MarqueeType = MarqueeType.Continuous;
				} else if (this.Element.ScrollType == iOSMarqueeLabel.MarqueeType.ContinuousReverse) {
					label.MarqueeType = MarqueeType.ContinuousReverse;
				} else if (this.Element.ScrollType == iOSMarqueeLabel.MarqueeType.LeftRight) {
					label.MarqueeType = MarqueeType.LeftRight;
				} else {
					label.MarqueeType = MarqueeType.RightLeft;
				}
				label.Text = this.Element.Text;
				label.TrailingBuffer = this.Element.TrailingBuffer;
				label.Rate = this.Element.Rate;			
			}
		}

		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);
			this.Control.Frame = rect;
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">The event arguments</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (iOSMarqueeLabel)Element;

			if (e.PropertyName == Label.TextProperty.PropertyName) {
				SetPlaceholder (view);
			} else if (e.PropertyName == Label.FormattedTextProperty.PropertyName) {
				SetPlaceholder (view);
			} else if (e.PropertyName == iOSMarqueeLabel.TextProperty.PropertyName) {
				SetPlaceholder (view);
			} else if (e.PropertyName == iOSMarqueeLabel.FormattedPlaceholderProperty.PropertyName) {
				SetPlaceholder (view);
			} else if (e.PropertyName == iOSMarqueeLabel.NavigateUriProperty.PropertyName) {
				AddNavigateUrl (view.NavigateUri);
			} else if (e.PropertyName == iOSMarqueeLabel.RateProperty.PropertyName ||
			         e.PropertyName == iOSMarqueeLabel.TrailingBufferProperty.PropertyName ||
			         e.PropertyName == iOSMarqueeLabel.HoldScrollingProperty.PropertyName) {
				UpdateMarquee (this.Control);
			}
		}

		private void SetPlaceholder(iOSMarqueeLabel view)
		{
			if (view != null) {
				if (!string.IsNullOrWhiteSpace (view.Text)) {
					var formattedString = view.FormattedText ?? view.Text;

					Control.AttributedText = formattedString.ToAttributed (view.Font, view.TextColor);

					LayoutSubviews ();

					return;
				}

				if (string.IsNullOrWhiteSpace (view.Placeholder) && view.FormattedPlaceholder == null) {
					return;
				}

				var formattedPlaceholder = view.FormattedPlaceholder ?? view.Placeholder;

				Control.AttributedText = formattedPlaceholder.ToAttributed (view.Font, view.TextColor);

				LayoutSubviews ();
			}
		}

		private void AddNavigateUrl(string url)
		{
			if (String.IsNullOrEmpty(url) == false)
			{
				if (tapXamarin != null) {
					this.Control.RemoveGestureRecognizer (tapXamarin);
					tapXamarin = null;
				}
				this.Control.UserInteractionEnabled = true;
				tapXamarin = new UITapGestureRecognizer ();

				tapXamarin.AddTarget (() => {
					var hyperLinkLabel = base.Element as iOSMarqueeLabel;
					UIApplication.SharedApplication.OpenUrl (new NSUrl (GetNavigationUri (hyperLinkLabel.NavigateUri)));
				});

				tapXamarin.NumberOfTapsRequired = 1;
				tapXamarin.DelaysTouchesBegan = true;
				this.Control.AddGestureRecognizer(tapXamarin);
			}
		}

		private string GetNavigationUri(string uri)
		{
			if (uri.Contains("@") && !uri.StartsWith("mailto:"))
			{
				return string.Format("{0}{1}", "mailto:", uri);
			}
			else if (uri.StartsWith("www."))
			{
				return string.Format("{0}{1}", @"http://", uri);
			}
			return uri;
		}
    }
}