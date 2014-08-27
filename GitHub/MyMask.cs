using System;
using Xamarin.Forms;
using GitHub.Controls;

namespace GitHub
{
	public class MyMask : ContentPage
	{
		private MyEntry entry;

		public MyMask ()
		{
			entry = new MyEntry ();
			entry.Text = "";
			entry.Keyboard = Keyboard.Numeric;
			entry.MaskPlaceHolders = "-";
			entry.Text = "100200";
			entry.MaxLength = 10;
//			entry.Mask = new System.Collections.Generic.List<MaskRules> (
//				new[] {
//					new MaskRules {  Start = 0, End = 3, Mask = "" },
//					new MaskRules { Start = 4, End = 6, Mask = "{0:3}-{3:}"},
//					new MaskRules { Start = 7, End = 10, Mask = "{0:3}-{3:3}-{6:}"},
//					new MaskRules { Start = 10, End = 20, Mask = "{0:}"}
//				});


			this.Content = new StackLayout {
				Children = {
					entry
				}
			};

		}
	}
}

