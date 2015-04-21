using System;
using Xamarin.Forms;
using Binding.iOS;

[assembly: ExportRenderer(typeof(Label), typeof(MarqueeLabelRenderer))]
namespace Binding.iOS
{
	// based on https://github.com/cbpowell/MarqueeLabel
	public class MarqueeLabelRenderer : Xamarin.Forms.Platform.iOS.LabelRenderer
	{
		private MarqueeLabel label;
		public MarqueeLabelRenderer ()
		{
		}

		protected override void OnElementChanged (Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.Label> e)
		{
			base.OnElementChanged (e);
			if (label == null) {
				label = new MarqueeLabel (this.Bounds);
				label.Text = "Hello World this is a Marquee Label.  It is a long text. Text End!";
				SetNativeControl (label);
			}
		}
	}
}

