using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Reflection;
using System.Linq;
using mobile.models.ViewModels;

namespace mobile.models.MVVM
{
	/// <summary>
	/// Class ViewFactory.
	/// </summary>
	public static class ViewFactory
	{
		/// <summary>
		///  The type dictionary.
		/// (ViewModel type, Page type, IsLogin, BindModel)
		/// </summary>
		private static readonly Dictionary<Type, Tuple<Type, bool, bool, bool>> TypeDictionary = new Dictionary<Type, Tuple<Type, bool, bool, bool>>();
	
		/// <summary>
		/// The page cache.
		/// </summary>
		private static readonly Dictionary<string, Tuple<ViewModel, Page>> PageCache =
			new Dictionary<string, Tuple<ViewModel, Page>>();
			
		public static TViewModel GetViewModelCache<TViewModel>() 
			where TViewModel : ViewModel
		{
			if (PageCache != null)
			{
				var result = (from x in PageCache
					where x.Value.Item1 is TViewModel
					select x.Value).FirstOrDefault();
				if (result != null)
				{
					return result as TViewModel;
				}
			}
			return null;
		}

		public static Type CreateLoginPageType()
		{
			var p = (from x in TypeDictionary
				where x.Value.Item3 == true
				select x).FirstOrDefault ();

			return p.Value.Item1;
		}

		public static Type GetViewModel<TPage>(TPage p)
		{
			var result = (from x in TypeDictionary
				where x.Value.Item1 == p.GetType()
				select x).FirstOrDefault();
			return result.Key;
		}

		public static Type GetPage<TViewModel>() where TViewModel : ViewModel
		{
			Type p = typeof(TViewModel);
			var result = (from x in TypeDictionary
				where x.Key == p
				select x).FirstOrDefault();
			return result.Value.Item1;
		}

		public static void Register<TViewModel, TPage>(bool BindModel = true, bool IsLogin = false, bool EnableCache = false) 
			where TPage : Page
			where TViewModel : ViewModel
		{
			lock (TypeDictionary) {
				Type v = typeof(TViewModel);
				Type p = typeof(TPage);

				if (TypeDictionary.ContainsKey (v)) {
					TypeDictionary [v] = new Tuple<Type, bool, bool, bool>(p, EnableCache, IsLogin, BindModel);
				} else {
					TypeDictionary.Add (v, new Tuple<Type, bool, bool, bool>(p, EnableCache, IsLogin, BindModel));
				}
			}
		}


		public static Page CreatePage(Type ViewModel, Action<object, Page> initialiser = null)
		{
			return CreatePage (ViewModel, null, null, initialiser);
		}

		public static Page CreatePage<TViewModel>(Action<TViewModel, Page> initialiser = null)
			where TViewModel : ViewModel
		{
			return CreatePage (null, null, initialiser);
		}

		public static Page CreateLoginPage()
		{
			var p = (from x in TypeDictionary
				where x.Value.Item3 == true
				select x).FirstOrDefault ();

			return CreatePage (p.Key, null, null);
		}

		public static Page CreateLoginPage(object[] PageParameters, object[] ViewModelParameters, 
			Action<object, Page> initialiser = null)
		{
			var p = (from x in TypeDictionary
				where x.Value.Item3 == true
				select x).FirstOrDefault ();

			return CreatePage (p.Key, PageParameters, ViewModelParameters, initialiser);
		}

		/// <summary>
		/// Creates the page.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the view model.</typeparam>
		/// <param name="initialiser">
		/// The create action.
		/// </param>
		/// <returns>
		/// Page for the ViewModel.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// Unknown View for ViewModel.
		/// </exception>
		public static Page CreatePage(Type ViewModelType, object[] PageParameters, object[] ViewModelParameters, 
			Action<object, Page> initialiser = null)
		{
			lock (TypeDictionary) {
				Tuple<Type, bool, bool, bool> viewType;
				var viewModelType = ViewModelType;

				if (TypeDictionary.ContainsKey (viewModelType)) {
					viewType = TypeDictionary [viewModelType];
				} else {
					throw new InvalidOperationException ("Unknown View for ViewModel");
				}

				var CacheThisPage = viewType.Item2;
				Page page;
				object viewModel;
				var pageCacheKey = string.Format ("{0}:{1}", viewModelType.Name, viewType.Item1.Name);

				if (CacheThisPage == true && PageCache.ContainsKey (pageCacheKey)) {
					var cache = PageCache [pageCacheKey];
					viewModel = cache.Item1;
					page = cache.Item2;
				} else {
					page = CreateObject<Page> (viewType.Item1, PageParameters);
					// viewmodel constructor?
					viewModel = CreateObject(viewModelType, ViewModelParameters) as ViewModel;

					// set navigation
					//viewModel.Navigation = new ViewModelNavigation (page.Navigation);

					// cache the page
					//if (CacheThisPage) {
					//	PageCache [pageCacheKey] = new Tuple<ViewModel, Page> (viewModel, page);
					//}
				}
					
				if (CacheThisPage == false) {
					// make sure the page is not in the cache
					if (PageCache.ContainsKey (pageCacheKey)) {
						PageCache.Remove (pageCacheKey);
					}
				}

				// run some code if passed in
				if (initialiser != null) {
					initialiser (viewModel, page);
				}

				if (viewType.Item4) {
					// forcing break reference on viewmodel in order to allow initializer to do its work
					page.BindingContext = null;
					page.BindingContext = viewModel;
				}
				return page;
			}
		}

		/// <summary>
		/// Creates the page.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the view model.</typeparam>
		/// <param name="initialiser">
		/// The create action.
		/// </param>
		/// <returns>
		/// Page for the ViewModel.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// Unknown View for ViewModel.
		/// </exception>
		public static Page CreatePage<TViewModel>(object[] PageParameters, object[] ViewModelParameters, 
			Action<TViewModel, Page> initialiser = null)
			where TViewModel : ViewModel
		{
			lock (TypeDictionary) {
				Tuple<Type, bool, bool, bool> viewType;
				var viewModelType = typeof(TViewModel);

				if (TypeDictionary.ContainsKey (viewModelType)) {
					viewType = TypeDictionary [viewModelType];
				} else {
					throw new InvalidOperationException ("Unknown View for ViewModel");
				}

				var CacheThisPage = viewType.Item2;
				Page page;
				TViewModel viewModel;
				var pageCacheKey = string.Format ("{0}:{1}", viewModelType.Name, viewType.Item1.Name);

				if (CacheThisPage == true && PageCache.ContainsKey (pageCacheKey)) {
					var cache = PageCache [pageCacheKey];
					viewModel = cache.Item1 as TViewModel;
					page = cache.Item2;
				} else {
					page = CreateObject<Page> (viewType.Item1, PageParameters);
					// viewmodel constructor?
					viewModel = CreateObject<TViewModel> (viewModelType, ViewModelParameters);

					// set navigation
					//viewModel.Navigation = new ViewModelNavigation (page.Navigation);
					BindNavigation (viewModel, page.Navigation);

					// cache the page
					if (CacheThisPage) {
						PageCache [pageCacheKey] = new Tuple<ViewModel, Page> (viewModel, page);
					}
				}
					
				if (CacheThisPage == false) {
					// make sure the page is not in the cache
					if (PageCache.ContainsKey (pageCacheKey)) {
						PageCache.Remove (pageCacheKey);
					}
				}

				// run some code if passed in
				if (initialiser != null) {
					initialiser (viewModel, page);
				}

				if (viewType.Item4) {
					// forcing break reference on viewmodel in order to allow initializer to do its work
					page.BindingContext = null;
					page.BindingContext = viewModel;
				}
				return page;
			}
		}

		public static void BindNavigation<TViewModel>(TViewModel view, INavigation nav) 
			where TViewModel : ViewModel
		{
			view.Navigation = new ViewModelNavigation (nav);
		}

		private static object CreateObject(Type objectType, object[] parameters) 
		{
			ConstructorInfo constructor = null;

			if (parameters == null)
			{
				constructor = objectType.GetTypeInfo()
					.DeclaredConstructors
					.FirstOrDefault(c => !c.GetParameters().Any());

				parameters = new object[]
				{
				};

				return Convert.ChangeType (constructor.Invoke (parameters), objectType);
			}
			else
			{
				return Convert.ChangeType (Activator.CreateInstance (objectType, parameters), objectType);
			}
		}

		private static T CreateObject<T>(Type objectType, object[] parameters) 
		{
			ConstructorInfo constructor = null;

			if (parameters == null)
			{
				constructor = objectType.GetTypeInfo()
					.DeclaredConstructors
					.FirstOrDefault(c => !c.GetParameters().Any());

				parameters = new object[]
				{
				};
				return (T)constructor.Invoke(parameters);
			}
			else
			{
				return (T)Activator.CreateInstance (objectType, parameters);
			}
		}
	}
}
