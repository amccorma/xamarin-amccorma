using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FrameBorder
{
	public partial class TestPush : ContentPage
	{
		public TestPush ()
		{
			InitializeComponent ();
		}

		protected async void ButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new FrameTest ());
		}
	}
}

