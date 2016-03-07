using System;
using Android.Views;
using Android.OS;
using Android.App;
using Xamarin.Forms.Platform.Android;
using overlay.mobile.droid;
using mobile.pages.Overlay;

namespace overlay.mobile.droid
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

			return view;
		}
	}
}
