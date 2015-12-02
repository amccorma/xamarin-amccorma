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


			button.Click += delegate {
				maskEntry.SetErrorMessage("Error 1");
			};
		}
	}
}


