using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using mobile.models;

namespace mobile.models.Interfaces
{
    public interface INavigationAware
    {
		/// <summary>
		/// Called when being navigated to.
		/// </summary>
		/// <remarks>
		/// Can be implemented on either viewmodel or view.
		/// </remarks>
		/// <param name="Current">The view being navigated away from.</param>
		Task OnNavigatingTo(IPage Current);

		/// <summary>
		/// Called when being navigated away from.
		/// </summary>
		/// <remarks>
		/// Can be implemented on either viewmodel or view.
		/// </remarks>
		/// <param name="Previous">The view being navigated to.</param>
		Task OnNavigatingFrom(IPage Previous);

		/// <summary>
		/// Called when the page is Appearing
		/// </summary>
		/// <remarks>
		/// Can be implemented on either viewmodel or view.
		/// </remarks>
		/// <param name="Current">The view being navigated to.</param>
		Task OnAppearing (IPage Current);

		/// <summary>
		/// Called when the page is Disappearing
		/// </summary>
		/// <remarks>
		/// Can be implemented on either viewmodel or view.
		/// </remarks>
		/// <param name="Current">The view being navigated to.</param>
		Task OnDisappearing (IPage Current);

		/// <summary>
		/// Page not visible in Stack
		/// </summary>
		/// <returns>The pushed.</returns>
		/// <param name="Current">Current.</param>
		Task PagePushed(IPage Current);
    }
}
