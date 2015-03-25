using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace PickerToButton
{
   public class Page1 : ContentPage
   {
      public Page1()
      {        
			this.Content = new StackLayout {
				Children = {
					new MyPickerView()
				}
			};
         
      }
   }
}
