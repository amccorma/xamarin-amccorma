using System;
using Xamarin.Forms;
using FrameBorder;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreAnimation;
using CoreGraphics;
using System.ComponentModel;
using FrameBorder.iOS.Control;

[assembly: ExportRenderer(typeof(MyFrame), typeof(FrameBorder.iOS.MyFrameRenderer))]
namespace FrameBorder.iOS
{
	public class MyFrameRenderer: FrameView, IVisualElementRenderer
	{
		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		enum BorderPosition
		{
			Left,
			Top,
			Right,
			Bottom
		}

		public MyFrameRenderer ()
		{
			
		}

		public VisualElementTracker Tracker
		{
			get;
			private set;
		}

		public VisualElementPackager Packager
		{
			get;
			private set;
		}

		public VisualElement Element
		{
			get;
			private set;
		}

		public UIKit.UIView NativeView
		{
			get { return this as UIView; }
		}

		public UIKit.UIViewController ViewController
		{
			get
			{
				return null;
			}
		}

		public void SetElement (VisualElement element)
		{
			var oldElement = this.Element;

			if (oldElement != null)
			{
				oldElement.PropertyChanged -= this.HandlePropertyChanged;
			}

			this.Element = element;

			if (this.Element != null) {
				this.Element.PropertyChanged += this.HandlePropertyChanged;	
				this.SourceView = this.Element as MyFrame;

				if (SourceView.Radius == 0) {					
					this.BackgroundColor = TranslateFormsColor (SourceView.BackgroundColor);
				} else {
					this.BackgroundColor = UIColor.Clear;
				}

				this.LayoutMargins = new UIEdgeInsets (
					(float)SourceView.Padding.Top,
					(float)SourceView.Padding.Left,
					(float)SourceView.Padding.Bottom,
					(float)SourceView.Padding.Right);

				this.Tracker = new VisualElementTracker(this);
				this.Packager = new VisualElementPackager(this);

				this.Packager.Load();

				if (ElementChanged != null)
				{
					this.ElementChanged(this, new VisualElementChangedEventArgs(oldElement, this.Element));
				}
			}
		}

		private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Content") {
				//this.SetNeedsDisplay ();
			} else if (e.PropertyName == MyFrame.WidthProperty.PropertyName ||
			           e.PropertyName == MyFrame.HeightProperty.PropertyName ||
			           e.PropertyName == MyFrame.XProperty.PropertyName ||
			           e.PropertyName == MyFrame.YProperty.PropertyName) {
				
				this.Frame = new CGRect (this.Element.Bounds.X, 
					this.Element.Bounds.Y, 
					this.Element.Bounds.Width, this.Element.Bounds.Height);
				this.SetNeedsDisplay ();
			} else if (e.PropertyName == MyFrame.PaddingProperty.PropertyName) {
				this.LayoutMargins = new UIEdgeInsets (
					(float)SourceView.Padding.Top,
					(float)SourceView.Padding.Left,
					(float)SourceView.Padding.Bottom,
					(float)SourceView.Padding.Right);
			}
		}

		public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var size = UIViewExtensions.GetSizeRequest(this.NativeView, widthConstraint, heightConstraint, 44.0, 44.0);
			return size;
		}

		public void SetElementSize(Size size)
		{
			this.Element.Layout(new Rectangle(this.Element.X, this.Element.Y, size.Width, size.Height));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.Packager == null)
				{
					return;
				}

				this.SetElement((VisualElement) null);
				this.Packager.Dispose();
				this.Packager = (VisualElementPackager) null;
				this.Tracker.Dispose();
				this.Tracker = (VisualElementTracker) null;
			}

			base.Dispose(disposing);
		}
	}
}

