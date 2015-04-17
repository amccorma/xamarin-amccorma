using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using mobile.models.MVVM;

namespace mobile.models.ViewModels
{
    public class ViewModelNavigation
    {
        internal INavigation implementor;

        public ViewModelNavigation(INavigation implementor)
        {
            this.implementor = implementor;
        }

		/// <summary>
		/// get an instance of the Navigation object
		/// </summary>
		/// <value>The nav.</value>
		public INavigation Nav {
			get {
				return implementor;
			}
			set {
				implementor = value;
			}
		}

		public MasterDetailPage FindMasterModel()
		{
			var result = this.Nav.ModalStack.FirstOrDefault (item => item is MasterDetailPage);
			if (result != null) {
				return result as MasterDetailPage;
			}
			return null;

		}

        // This method can be considered unclean in the pure MVVM sense, 
        // however it has been handy to me at times
		public System.Threading.Tasks.Task PushAsync(Page page)
        {
            return implementor.PushAsync(page);
        }

        /// <summary>
        /// Pushes the asynchronous.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        /// <param name="activateAction">The activate action.</param>
        /// <returns>Task.</returns>
		public System.Threading.Tasks.Task PushAsync<TViewModel>(object[] PageParameters, object[] ViewModelParameters, 
			Action<TViewModel, Page> activateAction = null)
            where TViewModel : ViewModel
        {
			return PushAsync(ViewFactory.CreatePage<TViewModel>(PageParameters, ViewModelParameters, activateAction));
        }

		/// <summary>
		/// Pushes the asynchronous.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="activateAction">The activate action.</param>
		/// <returns>Task.</returns>
		public System.Threading.Tasks.Task PushAsync<TViewModel>(object[] Parameters, bool IsPageParameter = true, 
			Action<TViewModel, Page> activateAction = null)
			where TViewModel : ViewModel
		{
			if (IsPageParameter) {
				return PushAsync (ViewFactory.CreatePage<TViewModel> (Parameters, null, activateAction));
			} else {
				return PushAsync (ViewFactory.CreatePage<TViewModel> (null, Parameters, activateAction));
			}
		}

		/// <summary>
		/// Pushes the asynchronous.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="activateAction">The activate action.</param>
		/// <returns>Task.</returns>
		public System.Threading.Tasks.Task PushAsync<TViewModel>(Action<TViewModel, Page> activateAction = null)
			where TViewModel : ViewModel
		{
			return PushAsync(ViewFactory.CreatePage<TViewModel>(null, null, activateAction));
		}

		public System.Threading.Tasks.Task PopAsync()
        {
            return implementor.PopAsync();
        }

		public System.Threading.Tasks.Task PopToRootAsync()
        {
            return implementor.PopToRootAsync();
        }

        // This method can be considered unclean in the pure MVVM sense, 
        // however it has been handy to me at times
		public System.Threading.Tasks.Task PushModalAsync(Page page)
        {
            return implementor.PushModalAsync(page);
        }


        /// <summary>
        /// Pushes the modal asynchronous.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        /// <param name="activateAction">The create action.</param>
        /// <returns>Task.</returns>
		public System.Threading.Tasks.Task PushModalAsync<TViewModel>(object[] PageParameters, object[] ViewModelParameters, 
			Action<TViewModel, Page> activateAction = null)
            where TViewModel : ViewModel
        {
			return PushModalAsync(ViewFactory.CreatePage<TViewModel>(PageParameters,ViewModelParameters, activateAction));
        }

		/// Pushes the modal asynchronous.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="activateAction">The create action.</param>
		/// <returns>Task.</returns>
		public System.Threading.Tasks.Task PushModalAsync<TViewModel>(object[] Parameters, bool IsPageParameter, 
			Action<TViewModel, Page> activateAction = null)
			where TViewModel : ViewModel
		{
			if (IsPageParameter) {
				return PushModalAsync(ViewFactory.CreatePage<TViewModel>(Parameters, null, activateAction));
			} else {
				return PushModalAsync(ViewFactory.CreatePage<TViewModel>(null, Parameters, activateAction));
			}


		}

		/// Pushes the modal asynchronous.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="activateAction">The create action.</param>
		/// <returns>Task.</returns>
		public System.Threading.Tasks.Task PushModalAsync<TViewModel>(Action<TViewModel, Page> activateAction = null)
			where TViewModel : ViewModel
		{
			return PushModalAsync(ViewFactory.CreatePage<TViewModel>(null, null, activateAction));
		}

		public System.Threading.Tasks.Task PopModalAsync()
        {
            return implementor.PopModalAsync();
        }
    }
}