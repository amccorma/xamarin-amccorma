using System;
using Xamarin.Forms;
using Masked.Controls;
using Masked.Library;
using System.Diagnostics;

namespace Masked
{
	public class MyMask : ContentPage
	{
		private MyEntry entry, maxLength, maskZipCode, maskEntry2, maskEntry3, defaultMask;

		public MyMask ()
		{
			entry = new MyEntry ();
			entry.Text = "";
			entry.Keyboard = Keyboard.Numeric;
			entry.Text = "";

			// maxlength text
			maxLength = new MyEntry ();
			maxLength.Text = "";
			maxLength.Keyboard = Keyboard.Numeric;
			maxLength.Text = "";
			maxLength.MaxLength = 10;

			// zip code mask:  19201 or 12901-2010
			maskZipCode = new MyEntry ();
			maskZipCode.Text = "";
			maskZipCode.Keyboard = Keyboard.Numeric;
			maskZipCode.Text = "";
			maskZipCode.FormatCharacters = "-";
			maskZipCode.Mask = new System.Collections.Generic.List<MaskRules> (
				new[] {
					new MaskRules {  Start = 0, End = 5, Mask = "" },
					new MaskRules { Start = 5, End = 9, Mask = "{0:5}-{5:}"},
				});
			
			// mask: 0-1-3
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


			// mask: 01/03/2015
			maskEntry3= new MyEntry ();
			maskEntry3.Text = "";			
			maskEntry3.Keyboard = Keyboard.Text;
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

			// mask: 123-345-4444 or 1233454444900 (overflow mask)
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

			var editTextUsuario = new MyEntry ();
			editTextUsuario.Text = "";
			editTextUsuario.FormatCharacters = ".-/";
			editTextUsuario.Mask = new System.Collections.Generic.List<MaskRules>(
				new[] {
					new MaskRules { Start = 0, End = 3, Mask = ""},
					new MaskRules { Start = 4, End = 6, Mask = "{0:3}.{3:}"},
					new MaskRules { Start = 6, End = 9, Mask = "{0:3}.{3:3}.{6:}"},
					new MaskRules { Start = 9, End = 11, Mask = "{0:3}.{3:3}.{6:3}-{9:2}"},
					new MaskRules { Start = 11, End = 12, Mask = "{0:2}.{2:3}.{5:3}/{8:4}"},
					new MaskRules { Start = 12, End = 14, Mask = "{0:2}.{2:3}.{5:3}/{8:4}{12:2}"}
			});

			// +(99) (999) 999-9999"
			var editSample4 = new MyEntry ();
			editSample4.Text = "";
			editSample4.Keyboard = Keyboard.Numeric;
			editSample4.FormatCharacters = "+ )(-";
			editSample4.Mask = new System.Collections.Generic.List<MaskRules>(
				new[] {
					new MaskRules { Start = 0, End = 11, Mask = "{0:11}"},
					new MaskRules { Start = 11, End = 12, Mask = "+({0:2}) ({2:3}) {5:3}-{8:4}"},
				});
			
			//

			this.Content = new StackLayout {
				Children = {
					new Label  { Text = "+(99) (999) 999-9999" },
					editSample4,

					new Label  { Text = "editTextUsuario" },
					editTextUsuario,

					new Label { Text = "Normal Entry" },
					entry,
					new Label { 
						Text = "Max length = 10" ,
						TextColor = Device.OnPlatform(Color.Blue, Color.Default, Color.Default)
					},
					maxLength,
					new Label { 
						Text = "Zip code Mask",
						TextColor = Device.OnPlatform(Color.Blue, Color.Default, Color.Default)
					},
					maskZipCode,
					new Label { 
						Text = "0-1-2 Mask",
						TextColor = Device.OnPlatform(Color.Blue, Color.Default, Color.Default)
					},
					maskEntry2,
					new Label { 
						Text = "DOB Mask",
						TextColor = Device.OnPlatform(Color.Blue, Color.Default, Color.Default)
					},
					maskEntry3,
					new Label { 
						Text = "123-345-4444 or 1233454444900 (overflow mask)",
						TextColor = Device.OnPlatform(Color.Blue, Color.Default, Color.Default)
					},
					defaultMask,
					new Button
					{
						Text = "Mask Values (overflow)",
						Command = new Command(async () => {
							var text = defaultMask.Text;
							var raw = defaultMask.RawText;
							await DisplayAlert("Overflow Mask values", 
								"Text:=" + text + "\r\nRaw:=" + raw, "Cancel");
						})
					}
				}
			};
		}
	}
}

