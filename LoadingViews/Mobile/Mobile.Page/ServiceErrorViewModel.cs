using System;
using mobile.models.MVVM.ViewModels;

namespace mobile.models
{
	public class ServiceErrorViewModel : NavigationAwareViewModel
	{
		private Xamarin.Forms.Command _RefreshCommand;
		public ServiceErrorViewModel ()
		{
		}

		public Xamarin.Forms.Command RefreshCommand
		{
			get
			{
				return _RefreshCommand ?? (_RefreshCommand = new Xamarin.Forms.Command (async() => {
					await Navigation.PopAsync ();
				}, () => true));
			}
		}
	}
}

