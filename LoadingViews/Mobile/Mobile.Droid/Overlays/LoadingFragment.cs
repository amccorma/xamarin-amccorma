using System;
using Android.Support.V4.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using Android.App;
using Xamarin.Forms.Platform.Android;
using overlay.mobile.droid;
using mobile.pages.Overlay;

namespace overlay.mobile.droid
{
	internal class LoadingFragment : OverLayFragments
	{	
		public static LoadingFragment NewInstance(OverlayDetails details)
		{
			var detailsFrag = new LoadingFragment { Arguments = new Bundle() };
         	detailsFrag.ViewDetails = details;
			return detailsFrag;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (container == null)
			{				
				return null;
			}		

			var view = inflater.Inflate(overlay.mobile.droid.Resource.Layout.LoadingLayout, null);

			var activity = Xamarin.Forms.Forms.Context as Activity;
         	SetView(view, activity);         
			return view;
		}
	}
}