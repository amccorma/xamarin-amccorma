using System;
using Xamarin.Forms;
using System.Globalization;
using mobile.models.ViewModels;

namespace mobile.models.MVVM.Library
{
	/// <summary>
	/// Converts the Xamarin Forms page navigation to our <see cref="ViewModelNavigation"/> instance.
	/// </summary>
	public class NavigationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new ViewModelNavigation((INavigation)value);
		}
	}
}

