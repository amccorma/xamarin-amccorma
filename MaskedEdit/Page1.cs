using System;

using Xamarin.Forms;
using Masked.Controls;
using Masked.Library;

namespace Masked
{
	public class Page1 : ContentPage
	{
		public Page1 ()
		{
			this.BindingContext = new Page1ViewModel ();

			// mask: 01/03/2015
			var maskEntry3= new MyEntry ();
			maskEntry3.Text = "";			
			//maskEntry3.Keyboard = Keyboard.Text;
			maskEntry3.Keyboard = Keyboard.Numeric;
			maskEntry3.FormatCharacters = "/";
			maskEntry3.Mask = new System.Collections.Generic.List<MaskRules> (
				new[] {
					// 01 [characters: 0,1]
					new MaskRules {  Start = 0, End = 2, Mask = "{0:2}" },
					// 01/03 [characters: 0,1]-[characters: 2,3]
					new MaskRules {  Start = 2, End = 4, Mask = "{0:2}/{2:2}"},
					// 01/01/2015 [characters: 0,1]-[characters: 2,3]-characters: 4,5,6,7]
					// max length: end=8

					// {0:2} : take substring (0, 1): 01
					// {2:2} : take substring (2, 2): 03
					// {5:}  : take substring (5)   : 2015
					new MaskRules {  Start = 4, End = 8, Mask = "{0:2}/{2:2}/{4:}"}
				});

			maskEntry3.SetBinding (MyEntry.TextProperty, new Binding("Text1", BindingMode.TwoWay));
			maskEntry3.SetBinding (MyEntry.TextUpdateProperty, new Binding("TextUpdate", BindingMode.OneWay));
			this.Content = new StackLayout {
				Children = {
					new Label { Text = "MVVM Sample" },
					new Button { 
						Text = "Update Text", 
						Command = new Command( () => {		
							Model.ButtonClick.Execute(null);
						})
					},
					maskEntry3
				}
			};
		}

		public Page1ViewModel Model => this.BindingContext as Page1ViewModel;
	}
}


