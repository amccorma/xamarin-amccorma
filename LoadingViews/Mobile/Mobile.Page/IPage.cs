using System;
using System.Threading.Tasks;
using mobile.models.ViewModels;


namespace mobile.models
{
	public interface IPage
	{

		/// <summary>
		/// Shows the loading panel.
		/// </summary>
		void ShowLoadingPanel ();

		/// <summary>
		/// Show Error Loading Panel
		/// <param name="LoadingErrorMessage">Loading error message.</param>
		Task ShowErrorLoading (string LoadingErrorMessage, bool HideRefreshImage = false);

		/// <summary>
		/// Hides all.
		/// </summary>
		void HideAll();

		/// <summary>
		/// Get the Current ViewModel
		/// </summary>
		/// <value>The view.</value>
		ViewModel View { get; }	

		void ShowDisabledPanel ();
	}
}

