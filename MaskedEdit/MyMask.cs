using System;
using Xamarin.Forms;
using GitHub.Controls;
using GitHub.Library;

namespace GitHub
{
	public class MyMask : ContentPage
	{
		private MyEntry entry, maxLength, maskEntry, defaultMask;

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
					new MaskRules { Start = 10, End = 20, Mask = "{0:}"}
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
					new Label { Text = "Mask" },
					maskEntry,
					new Label { Text = "Apply Mask to Text" },
					defaultMask
				}
			};
		}
	}
}

