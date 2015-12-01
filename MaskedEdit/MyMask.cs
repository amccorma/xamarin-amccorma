using System;
using Xamarin.Forms;
using Masked.Controls;
using Masked.Library;

namespace Masked
{
	public class MyMask : ContentPage
	{
		private MyEntry entry, maxLength, maskEntry, maskEntry2, defaultMask;

		public MyMask ()
		{
			maxLength = new MyEntry ();
			maxLength.Text = "";
			maxLength.Keyboard = Keyboard.Numeric;
			maxLength.Text = "";
			maxLength.MaxLength = 10;

			entry = new MyEntry ();
			entry.Text = "";
			entry.Keyboard = Keyboard.Numeric;
			entry.Text = "";

			maskEntry = new MyEntry ();
			maskEntry.Text = "";
			maskEntry.Keyboard = Keyboard.Numeric;
			maskEntry.Text = "";
			maskEntry.FormatCharacters = "-";
			maskEntry.Mask = new System.Collections.Generic.List<MaskRules> (
				new[] {
					new MaskRules {  Start = 0, End = 3, Mask = "" },
					new MaskRules { Start = 4, End = 6, Mask = "{0:3}-{3:}"},
					new MaskRules { Start = 7, End = 10, Mask = "{0:3}-{3:3}-{6:}"},
					new MaskRules { Start = 10, End = 12, Mask = "{0:}"}
				});

			/*

				new MaskRules {  Start = 0, End = 1, Mask = ""},
				new MaskRules {  Start = 1, End = 2, Mask = "{0:1}-{1:}"},
				new MaskRules {  Start = 2, End = 3, Mask = "{0:1}-{1:1}-{2:}" },

				{0:1}-{1:} :  		character 0 (-) character 1 to end. 
				{0:1}-{1:1}-{2:}:	character 0 (-) characters 1 and 2 {1, 1} - character (3 to end) {2:1}
				
					character (0 to 1), anything
					character (1, 2), use mask {0,1}-{1:0)
						example:  	12 (2 is the character length of the Rule)
									0 (insert -) 2 : result 0-2
									rule done

			*/
			maskEntry2 = new MyEntry ();
			maskEntry2.Text = "";
			maskEntry2.Keyboard = Keyboard.Numeric;
			maskEntry2.Text = "";
			maskEntry2.FormatCharacters = "-";
			maskEntry2.Mask = new System.Collections.Generic.List<MaskRules> (
				new[] {
					new MaskRules {  Start = 0, End = 1, Mask = ""},
					new MaskRules {  Start = 1, End = 2, Mask = "{0:1}-{1:}"},
					new MaskRules {  Start = 2, End = 3, Mask = "{0:1}-{1:1}-{2:}" },
				});

			defaultMask = new MyEntry ();
			defaultMask.Text = "";
			defaultMask.FormatCharacters = "-";
			defaultMask.Keyboard = Keyboard.Numeric;
			defaultMask.Text = "1234567890";
			defaultMask.Mask = new System.Collections.Generic.List<MaskRules> (
				new[] {
					new MaskRules {  Start = 0, End = 3, Mask = "" },
					new MaskRules { Start = 4, End = 6, Mask = "{0:3}-{3:}"},
					new MaskRules { Start = 7, End = 10, Mask = "{0:3}-{3:3}-{6:}"},
					new MaskRules { Start = 10, End = 20, Mask = "{0:}"}
				});

			this.Content = new StackLayout {
				Children = {
					new Label { Text = "Normal Entry" },
					entry,
					new Label { Text = "Max length = 10" },
					maxLength,
					new Label { Text = "Mask 1" },
					maskEntry,
					new Label { Text = "Mask 2" },
					maskEntry2,
					//new Label { Text = "Apply Mask to Text" },
					//defaultMask
				}
			};
		}
	}
}

