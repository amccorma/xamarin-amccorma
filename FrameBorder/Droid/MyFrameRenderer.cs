using System;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Xamarin.Forms;
using FrameBorder;
using Android.Views;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(MyFrame), typeof(FrameBorder.Droid.MyFrameRenderer))]
namespace FrameBorder.Droid
{
	public class MyFrameRenderer: Android.Widget.FrameLayout, IVisualElementRenderer
	{
		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		ViewGroup packed;

		public MyFrameRenderer() : base(Forms.Context) { 

		}

		public MyFrame SourceView
		{
			get { return this.Element == null ? null : (MyFrame)Element; }
		}


		#region IVisualElementRenderer implementation

		public void SetElement(VisualElement element)
		{
			var oldElement = this.Element;

			if (oldElement != null)
				oldElement.PropertyChanged -= HandlePropertyChanged;

			this.Element = element;
			if (this.Element != null) {
				this.Element.PropertyChanged += HandlePropertyChanged;			

				// ** fix any errors, make sure valid values **/
				if (SourceView.ShadowBorders == null) {
					SourceView.ShadowBorders = new FrameRect ();
				}

				if (SourceView.ShadowBorders.Top < 0)
					SourceView.ShadowBorders.Top = 0;

				if (SourceView.ShadowBorders.Bottom < 0)
					SourceView.ShadowBorders.Bottom = 0;

				if (SourceView.ShadowBorders.Right < 0)
					SourceView.ShadowBorders.Right = 0;

				if (SourceView.ShadowBorders.Left < 0)
					SourceView.ShadowBorders.Left = 0;

				// ** end error check **/

				// ** setbackground **/
				//var border2 = Android.App.Application.Context.Resources.GetDrawable (Resource.Drawable.FrameBorder, Android.App.Application.Context.Theme).GetConstantState().NewDrawable() as LayerDrawable;

				// prevent changes to other instances of the drawable
				var border2 = Android.App.Application.Context.Resources.GetDrawable (Resource.Drawable.FrameBorder1, Android.App.Application.Context.Theme).Mutate() as LayerDrawable; 

				var shadowLayer = border2.FindDrawableByLayerId (Resource.Id.shadowLayer).Mutate () as GradientDrawable;
				shadowLayer.SetCornerRadius (SourceView.Radius);
				if (SourceView.HasShadow == false) {
					// invisible
					shadowLayer.Alpha = 0;
					// no padding
					border2.SetLayerInset (0, 0, 0, 0, 0);
				} else {
					shadowLayer.SetStroke((int)SourceView.ShadowRadius, SourceView.ShadowColor.ToAndroid ());
					shadowLayer.SetCornerRadius (SourceView.Radius);
				}

				var BorderLayer = border2.FindDrawableByLayerId (Resource.Id.BorderLayer).Mutate () as GradientDrawable;
				BorderLayer.SetStroke (SourceView.StrokeThickness, SourceView.OutlineColor.ToAndroid ());

				if (SourceView.StrokeType == StrokeType.Dashed) {
					// dashes
					BorderLayer.SetStroke (SourceView.StrokeThickness, SourceView.OutlineColor.ToAndroid (), 15, 10);
				} else if (SourceView.StrokeType == StrokeType.Dotted) {
					// dots
					BorderLayer.SetStroke (SourceView.StrokeThickness, SourceView.OutlineColor.ToAndroid (), 5, 4);
				} else {
					BorderLayer.SetStroke (SourceView.StrokeThickness, SourceView.OutlineColor.ToAndroid ());
				}

				BorderLayer.SetCornerRadius (SourceView.Radius);

				var MainLayer = border2.FindDrawableByLayerId (Resource.Id.MainLayer).Mutate () as GradientDrawable;
				MainLayer.SetColor (SourceView.BackgroundColor.ToAndroid ());
				MainLayer.SetCornerRadius (SourceView.Radius);

				if (SourceView.HasShadow == false) {
					border2.SetLayerInset (1, 0, 0, 0, 0);

					//index, l,t,r,b
					border2.SetLayerInset (2, 
						SourceView.Borders.Left == 0 ? 0 : SourceView.StrokeThickness,
						SourceView.Borders.Top == 0 ? 0 : SourceView.StrokeThickness,
						SourceView.Borders.Right == 0 ? 0 : SourceView.StrokeThickness,
						SourceView.Borders.Bottom == 0 ? 0 : SourceView.StrokeThickness);
				} else {
					// shadow layer. no padding
					border2.SetLayerInset (0, 0, 0, 0, 0);

					// border layer. padding=ShadowRadius, if border turned on
					var rect = new Rect (
						SourceView.ShadowBorders.Left == 0 ? 0 : (int)SourceView.ShadowRadius,
						SourceView.ShadowBorders.Top == 0 ? 0 : (int)SourceView.ShadowRadius,
						SourceView.ShadowBorders.Right == 0 ? 0 : (int)SourceView.ShadowRadius,
						SourceView.ShadowBorders.Bottom == 0 ? 0 : (int)SourceView.ShadowRadius);

					rect.Left = rect.Left < 0 ? 0 : rect.Left;
					rect.Top = rect.Top < 0 ? 0 : rect.Top;
					rect.Right = rect.Right < 0 ? 0 : rect.Right;
					rect.Bottom = rect.Bottom < 0 ? 0 : rect.Bottom;

					border2.SetLayerInset (1, rect.Left, rect.Top, rect.Right, rect.Bottom);

					// main layer indent shadowRadius + border size/
					border2.SetLayerInset (2, 
						SourceView.Borders.Left == 0 ? rect.Left : SourceView.StrokeThickness + rect.Left,
						SourceView.Borders.Top == 0 ? rect.Right  : SourceView.StrokeThickness + rect.Top,
						SourceView.Borders.Right == 0 ? rect.Top : SourceView.StrokeThickness + rect.Right,
						SourceView.Borders.Bottom == 0 ? rect.Bottom : SourceView.StrokeThickness + rect.Bottom);


				}
				this.Background = border2;

				// ** set padding **/
				this.SetPadding (
					(int)SourceView.Padding.Left,
					(int)SourceView.Padding.Top,
					(int)SourceView.Padding.Right,
					(int)SourceView.Padding.Bottom);


				ViewGroup.RemoveAllViews ();
				Tracker = new VisualElementTracker (this);

				Packager = new VisualElementPackager (this);
				Packager.Load ();

				if (ElementChanged != null)
					ElementChanged (this, new VisualElementChangedEventArgs (oldElement, this.Element));
			}
		}

		void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Content")
			{
				Tracker.UpdateLayout();
			}
		}

		public SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			packed.Measure(widthConstraint, heightConstraint);
			return new SizeRequest(new Size(packed.MeasuredWidth, packed.MeasuredHeight));
		}

		public void UpdateLayout()
		{
			if (Tracker == null)
				return;

			Tracker.UpdateLayout();
		}

		public VisualElementPackager Packager { get; private set; }

		public VisualElementTracker Tracker { get; private set; }

		public Android.Views.ViewGroup ViewGroup { get { return this; } }

		public VisualElement Element { get; private set; }

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.SetElement((VisualElement) null);
				if (this.Packager != null) {

					try
					{
						// exception.  Bug?  
						this.Packager.Dispose ();
					}
					catch {
					}
					this.Packager = null;
				}

				this.Tracker.Dispose();
				this.Tracker = (VisualElementTracker) null;
			}

			base.Dispose(disposing);
		}


		private Android.Graphics.Color getShade(Android.Graphics.Color color, float shade) {
			var red = Convert.ToInt32 (color.R * (1 - shade));
			var blue = Convert.ToInt32 (color.B * (1 - shade));
			var green = Convert.ToInt32 (color.G * (1 - shade));
			return new Android.Graphics.Color (red, green, blue);
		}
	}
}

