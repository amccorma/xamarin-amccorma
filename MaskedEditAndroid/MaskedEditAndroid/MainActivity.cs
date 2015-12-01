using Android.App;
using Android.Widget;
using Android.OS;

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

			MyEntryEditText maskEntry = FindViewById<MyEntryEditText> (Resource.Id.textView1);

			// test max length
			//maskEntry.MaxLength = 5;	 	// works

			maskEntry.Text = "";
			maskEntry.InputType = Android.Text.InputTypes.NumberVariationPassword;
			maskEntry.Text = "";
			maskEntry.FormatCharacters = "-";
			maskEntry.Mask = new System.Collections.Generic.List<MaskRules> (
				new[] {
					new MaskRules {  Start = 0, End = 1, Mask = ""},
					new MaskRules {  Start = 1, End = 2, Mask = "{0:1}-{1:}"},
					new MaskRules {  Start = 2, End = 3, Mask = "{0:1}-{1:1}-{2:}" },
				});

//			maskEntry.Text = "";
//			maskEntry.InputType = Android.Text.InputTypes.NumberVariationNormal;
//			maskEntry.Text = "";
//			maskEntry.FormatCharacters = "-";
//			maskEntry.Mask = new System.Collections.Generic.List<MaskRules> (
//				new[] {
//					new MaskRules {  Start = 0, End = 3, Mask = "" },
//					new MaskRules { Start = 4, End = 6, Mask = "{0:3}-{3:}"},
//					new MaskRules { Start = 7, End = 10, Mask = "{0:3}-{3:3}-{6:}"},
//					new MaskRules { Start = 10, End = 12, Mask = "{0:}"}
//				});


			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
				maskEntry.SetErrorMessage("Error 1");
			};
		}
	}
}


