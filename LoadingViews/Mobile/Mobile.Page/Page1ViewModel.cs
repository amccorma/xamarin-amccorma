using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using mobile.models.MVVM.ViewModels;
using mobile.models.MVVM;

namespace mobile.models
{
	public class Page1ViewModel : NavigationAwareViewModel
	{
		private Xamarin.Forms.Command _ShowErrorPanel;
		private Xamarin.Forms.Command _ShowLoadingPanel;
		private Xamarin.Forms.Command _HideAll;
		private Xamarin.Forms.Command _PushPage;
		private Xamarin.Forms.Command _DisabledPanel;

		public Page1ViewModel ()
		{
			this.Title = "Page1";
			this.IsInit = true;
		}

		public override async Task OnAppearing (IPage CurrentPage)
		{
			await base.OnAppearing (CurrentPage);
		//	await Task.Delay (4000);
			//await CurrentPage.HideAll ();
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
				return _HideAll ?? (_HideAll = new Command (() => {
					this.CurrentPage.HideAll();
				}, () => true));
			}
		}

		public Xamarin.Forms.Command PushPage
		{
			get {
				return _PushPage ?? (_PushPage = new Command (async() => {
					await this.Navigation.PushAsync(ViewFactory.CreatePage<RegionInfoViewModel>());
				}, () => true));
			}
		}

		public Xamarin.Forms.Command DisabledPanel
		{
			get {
				return _DisabledPanel ?? (_DisabledPanel = new Command (() => {
					this.CurrentPage.ShowDisabledPanel();
				}, () => true));
			}
		}
	}
}

