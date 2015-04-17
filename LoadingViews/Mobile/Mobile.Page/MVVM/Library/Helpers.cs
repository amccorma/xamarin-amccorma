using System;
using mobile.models.Interfaces;
using Xamarin.Forms;

namespace mobile.models.MVVM.Library
{
	public class Helpers
	{
		public INavigationAware AsNavigationAware(VisualElement element)
		{
			if (element == null || element.BindingContext == null) {
				return null;
			}
			var navigationAware = element.BindingContext as INavigationAware;
			if (navigationAware == null)
			{
				navigationAware = element as INavigationAware;
			}

			return navigationAware;
		}
	}
}

