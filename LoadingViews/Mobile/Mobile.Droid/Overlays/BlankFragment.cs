using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using mobile.models.Overlay;
using overlay.mobile.droid;

namespace PSEA.Mobile.Droid.Overlays
{
	internal class BlankFragment : OverLayFragments
	{
      public static BlankFragment NewInstance(OverlayDetails details)
		{
			var detailsFrag = new BlankFragment { Arguments = new Bundle() };
         detailsFrag.ViewDetails = details;
			return detailsFrag;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (container == null)
			{				
				return null;
			}		

			var view = inflater.Inflate(Resource.Layout.BlankLayout, null);
			var activity = Xamarin.Forms.Forms.Context as Activity;

         	SetView(view, activity); 
			return view;
		}
	}
}

