using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using mobile.models;
using mobile.models.MVVM.Library;
using System.Threading;
using System.Collections.Generic;
using mobile.models.MVVM;
using System.ComponentModel;
using System.Linq;
using mobile.models.Overlay;

namespace mobile.models.Controls
{
	public class BasePage : ContentPage, IPage, INotifyPropertyChanged
    {
		private bool HasInitRun = false;
		protected bool ShowLoadingIndicator = true;

		public BasePage()
		{
			SetBinding(NavigationProperty, new Binding("Navigation", converter: new NavigationConverter()));
			SetBinding (Page.TitleProperty, new Binding ("Title", BindingMode.TwoWay));
		}
		

		/// <summary>
		/// fire the onAppear handler for ViewModel
		/// </summary>
		protected override async void OnAppearing ()
		{
			base.OnAppearing ();

			// run first
			if (HasInitRun == false && this.ShowLoadingIndicator == true) {	
				System.Diagnostics.Debug.WriteLine ("OnAppearing showing indicator");
				ShowLoadingPanel ();
			}
			System.Diagnostics.Debug.WriteLine ("OnAppearing " + this.GetType().Name);
		
			var navigationAware = new Helpers ().AsNavigationAware (this);
			if (navigationAware != null) {
				// To page!
				await navigationAware.OnAppearing (this as IPage);
			}

			HasInitRun = true;
		}

		protected override void OnPropertyChanging (string propertyName)
		{
			base.OnPropertyChanging (propertyName);
			if (propertyName == "Content" && Xamarin.Forms.Device.OS == TargetPlatform.Android) {
				if (HasInitRun == false && this.ShowLoadingIndicator == true) {					
					if (DependencyService.Get<IShowOverLay> ().CanRun) {
						System.Diagnostics.Debug.WriteLine ("OnPropertyChanging showing indicator");
						ShowLoadingPanel ();
						HasInitRun = true;
					}
				}
			} else if (propertyName == "Renderer" && Xamarin.Forms.Device.OS == TargetPlatform.iOS) {
				if (HasInitRun == false && this.ShowLoadingIndicator == true) {					
					if (DependencyService.Get<IShowOverLay> ().CanRun) {
						System.Diagnostics.Debug.WriteLine ("OnPropertyChanging showing indicator");
						ShowLoadingPanel ();
						HasInitRun = true;
					}
				}
			}
		}

		public void InvokeMain( Action e) 
		{
			Device.BeginInvokeOnMainThread (e);
		}

		public mobile.models.ViewModels.ViewModel View { 
			get {
				return this.BindingContext as mobile.models.ViewModels.ViewModel;
			}
		}			

		private void WriteMessage(string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		/// <summary>
		/// Handles the errors.
		/// </summary>
		/// <param name="LoadingErrorMessage">Message.</param>
		public async Task ShowErrorLoading (string LoadingErrorMessage, bool HideRefreshImage = false)
		{
	        this.Content.IsVisible = false;
	        HideAll();         
			await Navigation.PushAsync(ViewFactory.CreatePage<ServiceErrorViewModel>(new object[] { this.Title, this.View }, null));
	        this.Content.IsVisible = true;
        
		}

		/// <summary>
		/// Hide Loading Panel and Message
		/// </summary>
		public void HideAll()
		{
			DependencyService.Get<IShowOverLay> ().HideAll ();
		}

		public void ShowLoadingPanel ()
		{
         	DependencyService.Get<IShowOverLay>().ShowLoadingScreen(new OverlayDetails
            {
				HasNavigationBar = HasNavigationBar,
				HasTabbedBar = IsTabbedPage
            });
			
		}


		public void ShowDisabledPanel()
		{
	         DependencyService.Get<IShowOverLay>().ShowDisabledScreen(new OverlayDetails
	         {
	        	HasNavigationBar = HasNavigationBar,
	            HasTabbedBar = IsTabbedPage
	         });
		}


		/// <summary>
		/// blocking execute
		/// </summary>
		/// <returns>The on main thread and wait.</returns>
		/// <param name="act">Act.</param>
		public async Task InvokeOnMainThreadAndWait(Action act)
		{
			TaskCompletionSource<bool> source = new TaskCompletionSource<bool> ();
			await Task.Run (() => {
				Device.BeginInvokeOnMainThread ( () => {
					act.Invoke();
					source.SetResult(false);
				});			
			});

			System.Diagnostics.Debug.WriteLine ("before wait");
			await source.Task;
			System.Diagnostics.Debug.WriteLine ("after wait");
		}

		/// <summary>
		/// Invoke on Main Thread
		/// </summary>
		/// <param name="act">Act.</param>
		public void InvokeOnMain(Action act)
		{
			Device.BeginInvokeOnMainThread ( () => {
				act.Invoke();
			});	
		}

		private bool IsTabbedPage
		{
			get
			{
				return this.Parent is TabbedPage;
			}
		}
			
		private bool HasNavigationBar {
			get {
				if (Navigation.ModalStack == null && Navigation.NavigationStack == null) {
					return false;
				} else if (Navigation.ModalStack == null) {
					return true;
				} else {
					return Navigation.ModalStack.FirstOrDefault (p => p.GetType () == this.GetType ()) == null;
				}
			}
		}
	}
}
