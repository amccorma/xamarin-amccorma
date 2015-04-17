using System.Threading.Tasks;
using mobile.models.MVVM.ViewModels;

namespace mobile.models
{
	public class RegionInfoViewModel : NavigationAwareViewModel
	{
		public RegionInfoViewModel()
		{
			this.Title = "Region Information";
			this.IsInit = true;
		}

		public override async Task OnAppearing (IPage CurrentPage)
		{
			await base.OnAppearing (CurrentPage);
		}
	}
}
