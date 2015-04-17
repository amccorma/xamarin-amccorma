using System;
using Android.Views;
using Android.OS;
using Android.App;
using Xamarin.Forms.Platform.Android;
using mobile.models.Overlay;
using overlay.mobile.droid;

namespace PSEA.Mobile.Droid.Overlays
{
	internal class DisabledFragment: OverLayFragments
	{
      	public static DisabledFragment NewInstance(OverlayDetails details)
		{
			var detailsFrag = new DisabledFragment { Arguments = new Bundle() };
	     	detailsFrag.ViewDetails = details;
			return detailsFrag;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (container == null)
			{				
				return null;
			}		

			var view = inflater.Inflate(Resource.Layout.DisabledLayout, null);
			var activity = Xamarin.Forms.Forms.Context as Activity;

         	SetView(view, activity); 

			var prog = view.FindViewById (Resource.Id.progressBar1);
			return view;
		}
	}
}
