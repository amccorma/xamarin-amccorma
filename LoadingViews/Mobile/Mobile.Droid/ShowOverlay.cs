using System;
using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Xamarin.Forms;
using mobile.pages.Overlay;

[assembly: Dependency(typeof(overlay.mobile.droid.ShowOverlay))]
namespace overlay.mobile.droid
{
	public class ShowOverlay : IShowOverLay
	{
		private readonly string LoadingOverLay = "loading";
		private readonly string DisabledOverLay = "disabled";
		private readonly string BlankOverLay = "blank";

		public ShowOverlay ()
		{
		}

		private Activity Current
		{
			get
			{
				return Xamarin.Forms.Forms.Context as Activity;
			}
		}

		public void HideAll()
		{						
			using (var manager = Current.FragmentManager.BeginTransaction ()) {
				if (HideAll(manager, ""))
				{					
					manager.Commit ();			
				}
			}
		}

		private void HideAll(string KeepOverlay)
		{
         using (var manager = Current.FragmentManager.BeginTransaction())
         {
            if (HideAll(manager, KeepOverlay))
            {
               manager.Commit();
            }
         }
		}

		private bool HideAll(Android.App.FragmentTransaction manager, string KeepOverlay)
		{
			bool found = false;
			if (KeepOverlay != this.DisabledOverLay) {
				var frag = Current.FragmentManager.FindFragmentByTag (this.DisabledOverLay);
				if (frag != null) {				               
					manager.Remove (frag);
					found = true;
				}
			}

			if (KeepOverlay != this.LoadingOverLay) {
				var frag = Current.FragmentManager.FindFragmentByTag (this.LoadingOverLay);
				if (frag != null) {               
					manager.Remove (frag);
					found = true;
				}
			}

			if (KeepOverlay != this.BlankOverLay) {
				var frag = Current.FragmentManager.FindFragmentByTag (this.BlankOverLay);
				if (frag != null) {
					manager.Remove (frag);
					found = true;
				}
			}

			return found;
		}

      public void ShowLoadingScreen(OverlayDetails details)
		{	
			if (IsActive(this.LoadingOverLay) == false) {
            var frag = LoadingFragment.NewInstance(details); 
				using (var manager = Current.FragmentManager.BeginTransaction ()) {
               manager.Add(Android.Resource.Id.Content, frag, this.LoadingOverLay);
					HideAll (manager, this.LoadingOverLay);
					manager.Commit ();
				}
			}
		}


      public void ShowDisabledScreen(OverlayDetails details)
		{
			if (IsActive(this.DisabledOverLay) == false) {
            var frag = DisabledFragment.NewInstance(details); 
				using (var manager = Current.FragmentManager.BeginTransaction ()) {
               	manager.Add(Android.Resource.Id.Content, frag, this.DisabledOverLay);
					HideAll (manager, this.DisabledOverLay);
					manager.Commit ();
				}
			}
		}

      public void ShowBlankScreen(OverlayDetails details)
		{
			if (IsActive(this.BlankOverLay) == false) {
            var frag = BlankFragment.NewInstance(details); 
				using (var manager = Current.FragmentManager.BeginTransaction ()) {
               manager.Add(Android.Resource.Id.Content, frag, this.BlankOverLay);
					HideAll (manager, this.BlankOverLay);
					manager.Commit ();
				}
			}
		}

		private bool IsActive(string Overlay)
		{
			var activity = Xamarin.Forms.Forms.Context as Activity;
			var f = activity.FragmentManager.FindFragmentByTag (Overlay);
			return f != null;
		}

		public bool CanRun {
			get {
				return true;
			}
		}
	}
}

