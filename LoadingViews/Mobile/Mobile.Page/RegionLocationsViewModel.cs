using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using mobile.models.MVVM.ViewModels;

namespace mobile.models
{
	public class RegionLocationsViewModel : NavigationAwareViewModel
	{
		private Xamarin.Forms.Command _ShowErrorPanel;
		private Xamarin.Forms.Command _ShowLoadingPanel;
		private Xamarin.Forms.Command _HideAll;

		public RegionLocationsViewModel ()
		{
			if (Xamarin.Forms.Device.OS == TargetPlatform.iOS) {
				this.PagePadding = new Thickness (0, 0, 0, 0);
			}
			this.Title = "Locations";
		}

		public override async Task OnAppearing (IPage CurrentPage)
		{
			await base.OnAppearing (CurrentPage);
			await Task.Delay (2000);
			CurrentPage.HideAll ();
			this.IsFirstLoad = false;
		}

		public Xamarin.Forms.Command ShowErrorPanel
		{
			get {
				return _ShowErrorPanel ?? (_ShowErrorPanel = new Command (() => {
					this.CurrentPage.ShowErrorLoading("");
				}, () => true));
			}
		}

		public Xamarin.Forms.Command ShowLoadingPanel
		{
			get {
				return _ShowLoadingPanel ?? (_ShowLoadingPanel = new Command (() => {
					this.CurrentPage.ShowLoadingPanel();
				}, () => true));
			}
		}

		public Xamarin.Forms.Command HideAll
		{
			get {
				return _HideAll ?? (_HideAll = new Command (async() => {
					this.CurrentPage.HideAll();
				}, () => true));
			}
		}
	}
}

