using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using mobile.models.Interfaces;
using mobile.models;
using mobile.models.MVVM.Library;

namespace mobile.models.MVVM.Navigation
{
	public class NavigationView : NavigationPage
	{
		private const string CurrentPagePropertyName = "CurrentPage";

		private WeakReference<Page> _previousPage;
		private WeakReference<Page> _mainPage;
		private Helpers helper = new Helpers();
		public NavigationView()
		{

		}

		public NavigationView(Page root)
			: base(root)
		{
		}

		protected override void OnChildAdded(Element child)
		{
			base.OnChildAdded(child);

			Page view = (Page)child;

			Page m = null;
			if (_mainPage == null || _mainPage.TryGetTarget (out m)) {
				// is null
				_mainPage = new WeakReference<Page> (view);
			} 

			_mainPage.TryGetTarget (out m);
			// Since OnChildRemoved event is not triggered for main page.
			if (CurrentPage == m) {
				OnNavigatingFrom (m, view);
			}
			// added 3-4-2015
			else if (CurrentPage != m && CurrentPage != null)
			{
				PagePushed(CurrentPage);
			}

			OnNavigatingTo(view, CurrentPage);
		}

		protected override void OnChildRemoved(Element child)
		{
			base.OnChildRemoved(child);

			Page view = (Page)child;

			Page currentView = null;
			Page previousView = null;
			_previousPage.TryGetTarget (out currentView);
			_mainPage.TryGetTarget (out previousView);

			/* OLD BLOCK removed 3-5-2015 */
//			OnNavigatingFrom(view, currentView);
//
//			// Since OnChildAdded is not triggered for main page.
//			if (currentView == previousView) {
//
//				// added 2-11-2015
////				var test = view as IPage;
////				if (test != null)
////					test.FirePageRemoved ();
////
//				OnNavigatingTo (currentView, view);
//			}
//			// added 3-5-2015 (to remove event handlers)
//			else if (previousView != null && currentView != null && (currentView != previousView)) {
//				PagePushed (previousView);
//			}

			/* END OLD BLOCK */
			// switched to this from above comments

			// Since OnChildAdded is not triggered for main page.
			if (currentView == previousView) {

				// added 2-11-2015
				//				var test = view as IPage;
				//				if (test != null)
				//					test.FirePageRemoved ();
				//
				OnNavigatingTo (currentView, view);
			}
			// added 3-5-2015 (to remove event handlers)
			else if (previousView != null && currentView != null && (currentView != previousView)) {
				PagePushed (previousView);
			}

			OnNavigatingFrom(view, currentView);
		}

//		protected override void OnAppearing ()
//		{
//			base.OnAppearing ();
//			if (this.CurrentPage is IPage) {
//				OnNavigateAppearing (this.CurrentPage);
//			}
//		}

		protected override void OnPropertyChanging(string propertyName = null)
		{
			if (propertyName == CurrentPagePropertyName)
			{
				_previousPage = new WeakReference<Page> (CurrentPage);
			}

			base.OnPropertyChanging(propertyName);
		}

		protected void OnNavigatingTo(Page targetView, Page previousView)
		{
			Debug.WriteLine("OnNavigatingTo: targetView={0}, nextView={1}", targetView.GetType().Name, previousView != null ? previousView.GetType().Name : string.Empty);

			var navigationAware = helper.AsNavigationAware(targetView);
			if (navigationAware != null)
			{
				// To page!
				navigationAware.OnNavigatingTo(targetView as IPage);
			}
		}

		protected void OnNavigatingFrom(Page targetView, Page nextView)
		{
			Debug.WriteLine("OnNavigatingFrom: targetView={0}, previousView={1}", targetView.GetType().Name, nextView.GetType().Name);

//			var navigationAware = helper.AsNavigationAware(targetView);
//
//			if (navigationAware != null)
//			{
//				//
//				navigationAware.OnNavigatingFrom(nextView as IPage);
//			}

			// modified 3-3-2015
			var navigationAware = helper.AsNavigationAware(nextView);

			if (navigationAware != null)
			{
				//
				navigationAware.OnNavigatingFrom(targetView as IPage);
			}
		}

		protected override bool OnBackButtonPressed ()
		{
			var page = this.Navigation.NavigationStack [0];
			if (page != null) {
				var view = page.BindingContext as mobile.models.ViewModels.ViewModel;
				if (view != null) {
					return view.CanGoBack;
				}
			}
			return base.OnBackButtonPressed ();
		}



		protected void PagePushed(Page page)
		{
			Debug.WriteLine("PagePushed: Page={0}", page.GetType().Name);

			var navigationAware = helper.AsNavigationAware(page);
			if (navigationAware != null)
			{
				//
				navigationAware.PagePushed(page as IPage);
			}
		}
//
//		protected void OnNavigateDisappearing(Page page)
//		{
//			Debug.WriteLine("OnDisappearing: Page={0}", page.GetType().Name);
//
//			var navigationAware = helper.AsNavigationAware(page);
//			if (navigationAware != null)
//			{
//				//
//				navigationAware.OnDisappearing(page as IPage);
//			}
//		}
	}
}
