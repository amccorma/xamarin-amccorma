using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using mobile.models.Interfaces;
using mobile.models.MVVM.ViewModels;
using mobile.models;
using System.Diagnostics;
using mobile.models.ViewModels;

namespace mobile.models.MVVM.ViewModels
{
	public class NavigationAwareViewModel : ViewModel, INavigationAware
    {
		#region INavigationAware implementation

		/// <summary>
		/// If true, checks login after each load of page
		/// </summary>
		/// <value><c>true</c> if handle loggin events; otherwise, <c>false</c>.</value>
		protected bool HandleLogginEvents { get; set; }

		public NavigationAwareViewModel()
		{
			this.HandleLogginEvents = true;
		}
			
		public virtual async Task OnNavigatingTo (IPage CurrentPage)
		{
			if (this.CurrentPage == null || CurrentPage.GetType () != this.CurrentPage.GetType ()) {
				this.SetCurrentPage = CurrentPage;
			}
		}

		public virtual async Task OnNavigatingFrom (IPage Next)
		{
		}

		public virtual async Task OnAppearing(IPage CurrentPage)
		{
			if (this.CurrentPage == null || CurrentPage.GetType () != this.CurrentPage.GetType ()) {
				this.SetCurrentPage = CurrentPage;
			}

		}

		public virtual Task OnDisappearing(IPage CurrentPage)
		{
			return Task.FromResult(default(object));
		}

		public virtual Task UnAuthorizedRequest()
		{
			return Task.FromResult(default(object));
		}
			

		public virtual Task PagePushed(IPage CurrentPage)
		{
			return Task.FromResult(default(object));
		}


		#endregion


    }
}
