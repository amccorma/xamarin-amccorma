using System;
using Xamarin.Forms;
using Masked.Controls;
using Masked.Library;
using System.Collections.Generic;

namespace Masked
{
	public class MaskValidatePage : ContentPage
	{
		private MyEntry maxLength, maxOnlyChars;

		public MaskValidatePage ()
		{
			var rules = new List<Validation> ();
			rules.Add (new Validation(Validators.MAX, "10", "Max length is 10"));
			maxLength = new MyEntry ();
			maxLength.Text = "";
			maxLength.FormatCharacters = "-";
			maxLength.ValidationRules = rules;


			var rules2 = new List<Validation> ();
			rules2.Add (new Validation(Validators.ONLYCHARS, "[1-9]", "Only enter 1,2,3,4,5,6,7,8,9"));
			maxOnlyChars = new MyEntry ();
			maxOnlyChars.Text = "";
			maxOnlyChars.FormatCharacters = "-";
			maxOnlyChars.ValidationRules = rules2;
			
			// validation of a mask.  beta.
//			maskValidation = new MyEntry ();
//			maskValidation.Text = "Asasd";
//			maskValidation.FormatCharacters = "-";
//			maskValidation.ValidCharacters = "[A-Za-z]";
//			maskValidation.Mask = new System.Collections.Generic.List<MaskRules> (
//				new[] {
//					new MaskRules {  
//							Start = 0, 
//							End = 5, 
//							Mask = "", 
//							startC = "ABC",
//							startCCase = false,
//							endC = "bC",
//							endCase = false
//					},
//					new MaskRules { Start = 5, End = 9, Mask = "{0:5}-{5:}"},
//			});

			// add event handler.
			// in production code, make sure you unsubscribe to this event
			maxLength.OnValidationError += MaxLength_OnValidationError;
			maxOnlyChars.OnValidationError += MaxLength_OnValidationError;

			this.Content = new StackLayout {
				Children = {
					new Label { 
						Text = "Max Length = 10",
						TextColor = Device.OnPlatform(Color.Blue, Color.Default, Color.Default)
					},
					maxLength,
					new Label { 
						Text = "Max Length = 10, Only [1-9]",
						TextColor = Device.OnPlatform(Color.Blue, Color.Default, Color.Default)
					},
					maxOnlyChars,
				}
			};
		}

		void MaxLength_OnValidationError (object sender, string message)
		{
			System.Diagnostics.Debug.WriteLine (message);
		}
	}
}

