using System;
using Xamarin.Forms;

namespace mobile.pages
{
	public class TabPage : TabbedPage
	{	
		public TabPage ()
		{
			this.Children.Add (new Tab1Page1());		
			this.Children.Add (new Tab1Page2());
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
		}
	}
}


