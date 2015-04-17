using System;
using Xamarin.Forms;

namespace mobile.models.Overlay
{
	public interface IShowOverLay
	{
		/// <summary>
		/// Shows the loading screen.
		/// </summary>
      /// <param name="details">OverlayDetails</param>
		void ShowLoadingScreen(OverlayDetails details);

		/// <summary>
		/// Shows the disabled screen.
		/// </summary>
      /// <param name="details">OverlayDetails</param>
      void ShowDisabledScreen(OverlayDetails details);

		/// <summary>
		/// Shows the blank screen.
		/// </summary>
      /// <param name="details">OverlayDetails</param>
      void ShowBlankScreen(OverlayDetails details);

		/// <summary>
		/// Hides all.
		/// </summary>
		void HideAll();

		/// <summary>
		/// iOS crap. Android not needed.  has issue in iOS on the first time the application is started.
		/// </summary>
		/// <value><c>true</c> if this instance can run; otherwise, <c>false</c>.</value>
		bool CanRun { get; }
	}
}

