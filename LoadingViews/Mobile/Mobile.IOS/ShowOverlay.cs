using System;
using Xamarin.Forms;
using UIKit;
using System.Linq;
using mobile.pages.Overlay;
using mobile.app.ios.Overlays;

[assembly: Dependency(typeof(mobile.app.ios.ShowOverlay))]
namespace mobile.app.ios
{
	public class ShowOverlay : IShowOverLay
	{
		private readonly Int32 LoadingOverLay = 1001;
      	private readonly Int32 DisabledOverLay = 1002;
      	private readonly Int32 BlankOverLay = 1003;

		public ShowOverlay ()
		{
		}


		private UIWindow MainWindow
		{
			get
			{
				return UIApplication.SharedApplication.KeyWindow;
			}
		}

		public void HideAll()
		{
			WriteMessage ("HideAll");
			HideAll(-1);
		}

		private void WriteMessage(string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		private bool HideAll(Int32 KeepOverlay)
		{
			bool found = false;
			var items = MainWindow.Subviews;
			if (KeepOverlay != this.DisabledOverLay) {
				var frag = items.FirstOrDefault(x => x.Tag == DisabledOverLay);
				if (frag != null) {
					WriteMessage ("Hiding: DisabledOverLay");
					(frag as OverlayView).Hide();
					found = true;
				}
			}

			if (KeepOverlay != this.LoadingOverLay) {
				var frag = items.FirstOrDefault(x => x.Tag == LoadingOverLay);
				if (frag != null) {
					WriteMessage ("Hiding: LoadingOverLay");
					(frag as OverlayView).Hide();
					found = true;
				}
			}

			if (KeepOverlay != this.BlankOverLay) {
				var frag = items.FirstOrDefault(x => x.Tag == BlankOverLay);
				if (frag != null) {
					WriteMessage ("Hiding: BlankOverLay");
					(frag as OverlayView).Hide();
					found = true;
				}
			}

			return found;
		}

		public void ShowLoadingScreen(OverlayDetails details)
		{	
			if (IsActive(this.LoadingOverLay) == false) {
				WriteMessage ("ShowLoadingScreen");
				var view = new LoadingView (details);
				view.Tag = LoadingOverLay;
				view.Alpha = details.Alpha;
				MainWindow.AddSubview(view);
				HideAll (this.LoadingOverLay);				
			}
		}


		public void ShowDisabledScreen(OverlayDetails details)
		{
			if (IsActive(this.DisabledOverLay) == false) {
				WriteMessage ("ShowDisabledScreen");
				var view = new DisabledView (details);
				view.Tag = DisabledOverLay;
				view.Alpha = details.Alpha;
				MainWindow.AddSubview(view);
				HideAll(this.DisabledOverLay);
			}
		}

		public void ShowBlankScreen(OverlayDetails details)
		{
			if (IsActive(this.BlankOverLay) == false) {
				WriteMessage ("ShowBlankScreen");
				var view = new BlankView (details);
				view.Tag = BlankOverLay;
				MainWindow.AddSubview(view);
				view.Alpha = details.Alpha;
				HideAll(this.BlankOverLay);
			}
		}

		private bool IsActive(Int32 Overlay)
		{
			var t = UIApplication.SharedApplication.KeyWindow;
			return MainWindow.Subviews.FirstOrDefault(x => x.Tag == Overlay) != null;			
		}

		public bool CanRun {
			get {
				return MainWindow != null;
			}
		}
	}
}

