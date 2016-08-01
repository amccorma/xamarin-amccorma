using System;
using Xamarin.Forms;

namespace FormsApp
{
    public class Page1 : ContentPage
    {
        public Page1()
        {
			this.Content = new StackLayout
			{
				Children =
				{
					new iOSMarqueeLabel
					{
						Text = "I'm guessing this project is abandoned or at least relegated to old hardware & OSes"
					}
                }
            };
        }
    }
}
