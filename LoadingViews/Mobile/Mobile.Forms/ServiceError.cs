using System;
using Xamarin.Forms;

namespace mobile.pages
{
	public class ServiceError : ContentPage
	{
		private Label lblError;
		private Button imgRefresh;

		public ServiceError (string Title, mobile.models.ViewModels.ViewModel view)
		{		
			this.Padding = new Thickness (0, 0);
			var layout = new RelativeLayout ();

			NavigationPage.SetHasBackButton (this, false);

			this.Title = Title;

			imgRefresh = new Button {
				Text = "Refresh"
			};

			imgRefresh.IsVisible = true;

//			var gesture = new TapGestureRecognizer ();
//			gesture.NumberOfTapsRequired = 1;
//
//			gesture.SetBinding (TapGestureRecognizer.CommandProperty, "RefreshCommand");
//			imgRefresh.GestureRecognizers.Add (gesture);
			imgRefresh.SetBinding (Button.CommandProperty, "RefreshCommand");
			layout.Children.Add(imgRefresh,
				Constraint.RelativeToParent( (parent) => {
					// x
					return (parent.Width / 2) - 64;
				}),
				Constraint.RelativeToParent( (parent) => {
					// Y
					return (parent.Height / 2) - 64;
				}),
				// width
				Constraint.Constant(128),
				// height
				Constraint.Constant(128)
			);

			lblError = new Label { Text = "Error Loading" };

			this.Content = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					new StackLayout
					{
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Padding = new Thickness(10, 10, 10, 10),
						Children = {
							lblError
						}
					},
					layout
				}
			};
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			//DependencyService.Get<IShowOverLay> ().HideAll ();	
		}
	}
}


