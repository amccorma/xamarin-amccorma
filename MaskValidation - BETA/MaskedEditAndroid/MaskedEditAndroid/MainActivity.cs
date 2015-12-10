using Android.App;
using Android.Widget;
using Android.OS;
using MaskedEditAndroid.Mask.Control;
using MaskedEditAndroid.Mask;

namespace MaskedEditAndroid
{
	[Activity (Label = "MaskedEditAndroid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);

			Button btn1 = FindViewById<Button> (Resource.Id.btn1);

			MaskEdit maskEntry = FindViewById<MaskEdit> (Resource.Id.textView1);

			/* TEST #1 */
			// OK nothing
			// no properties set

			/* TEST #2 */
			// OK! test max length
			//maskEntry.Properties = new MaskProperties();
			//maskEntry.Properties.MaxLength = 5;	 	

			/* TEST #3 */
			// OK! delete and forward and maxlength
//			maskEntry.Text = "";
//			maskEntry.InputType = Android.Text.InputTypes.ClassNumber;
//			maskEntry.Text = "1234";
//			maskEntry.Properties = new MaskProperties ();
//			maskEntry.Properties.FormatCharacters = "-";
//			maskEntry.Properties.Mask = new System.Collections.Generic.List<MaskRules> (
//				new[] {
//					new MaskRules {  Start = 0, End = 1, Mask = ""},
//					new MaskRules {  Start = 1, End = 3, Mask = "{0:1}-{1:}"}
//				});

			/* TEST #4 */
			// OK! cursor position not at end insert character
//			maskEntry.Text = "";
//			maskEntry.InputType = Android.Text.InputTypes.NumberVariationPassword;
//			maskEntry.Text = "1234567";
//			maskEntry.Properties = new MaskProperties ();
//			maskEntry.Properties.FormatCharacters = "-";
//			maskEntry.Properties.Mask = new System.Collections.Generic.List<MaskRules> (
//				new[] {
//					new MaskRules {  Start = 0, End = 1, Mask = ""},
//					new MaskRules {  Start = 1, End = 6, Mask = "{0:1}-{1:}"}
//				});

			/* TEST #5 */
			// OK! selected range text input
			// selected text delete OK
			// selected range insert OK
			// delete to 3 ok
			// delete from end to beginning ok
			// delete from end to beginning and reenter text ok
//			maskEntry.Text = "";
//			maskEntry.InputType = Android.Text.InputTypes.NumberVariationPassword;
//			maskEntry.Text = "1234567789";
//			maskEntry.Properties = new MaskProperties ();
//			maskEntry.Properties.FormatCharacters = "-/";
//			maskEntry.Properties.Mask = new System.Collections.Generic.List<MaskRules> (
//				new[] {
//					new MaskRules {  Start = 0, End = 1, Mask = ""},
//					new MaskRules {  Start = 1, End = 3, Mask = "{0:1}-{1:2}"},
//					new MaskRules {  Start = 3, End = 11, Mask = "{0:1}-{1:2}/{3:}"}
//				});


			/* TEST #6 */
			// mask: 000-00-0000
			maskEntry.InputType = Android.Text.InputTypes.NumberVariationPassword;
			maskEntry.Text = "123-45-6790";			
			maskEntry.Properties = new MaskProperties ();
			maskEntry.Properties.FormatCharacters = "-";
			maskEntry.Properties.Mask = new System.Collections.Generic.List<MaskRules> (
				new[] {
					// 123 [characters: 0,1,2]
					new MaskRules {  Start = 0, End = 2, Mask = "{0:3}"},
					// 123-45 [characters: 0,1,2]-[characters: 3,4]
					new MaskRules {  Start = 2, End = 4, Mask = "{0:3}-{3:2}"},
					// 123-45-6790 [characters: 0,1,2]-[characters: 3,4]-characters: 5,6,7,8]
					// max length: end=9

					// {0:3} : take substring (0, 2): 123
					// {3:2} : take substring (3, 2): 45
					// {5:}  : take substring (5)   : 6790
					new MaskRules {  Start = 4, End = 9, Mask = "{0:3}-{3:2}-{5:}"}
				});

			/* TEST #7 */
			// mask: 01/03/2015
			maskEntry.InputType = Android.Text.InputTypes.NumberVariationPassword;
			maskEntry.Text = "";			
			maskEntry.Properties = new MaskProperties ();
			maskEntry.Properties.FormatCharacters = "/";
			maskEntry.Properties.Mask = new System.Collections.Generic.List<MaskRules> (
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

			button.Click += delegate {
				maskEntry.SetErrorMessage("Error 1");
			};

			btn1.Click += delegate {
				var d = new AlertDialog.Builder(this)
					.SetMessage("Text:= " + maskEntry.Text + "\r\n" +
						"Raw:= " + maskEntry.RawText)
					.SetPositiveButton("OK", (o, x) => { })
					.Show();
			};
		}
	}
}


