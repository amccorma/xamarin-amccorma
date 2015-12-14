using System;

using UIKit;

namespace iOSMaskedEdit
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			/* TEST #1 */
			// OK nothing
			// no properties set

			/* TEST #2 */
			// OK! test max length
			//maskEntry.Text = "Hello World";
			//maskEntry.Properties = new MaskProperties();
			//maskEntry.Properties.MaxLength = 5;	


			/* TEST #3 */
			// OK! delete and forward and maxlength
			// first load: 12-3
//			maskEntry.KeyboardType = UIKeyboardType.NumberPad;
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
//			maskEntry.KeyboardType = UIKeyboardType.Default;
//			maskEntry.Text = "1234567";
//			maskEntry.Properties = new MaskProperties ();
//			maskEntry.Properties.FormatCharacters = "-";
//			maskEntry.Properties.Mask = new System.Collections.Generic.List<MaskRules> (
//				new[] {
//					new MaskRules {  Start = 0, End = 1, Mask = ""},
//					new MaskRules {  Start = 1, End = 6, Mask = "{0:1}-{1:}"}
//				});


			/* TEST #5 */
			// mask: 01/03/2015
			maskEntry.KeyboardType = UIKeyboardType.NumberPad;
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
					new MaskRules {  Start = 4, End = 9, Mask = "{0:2}/{2:2}/{4:}"}
				});

			maskEntry.OnError += OnMaskError;

			button1.TouchDown += Button1_TouchDown;
		}

		void Button1_TouchDown (object sender, EventArgs e)
		{
			var message = "Text:= " + maskEntry.Text + "\r\nRaw:= (no formating) " + maskEntry.RawText;
			var alert = new UIAlertView ("Mask properties", message, null, "OK");
			alert.Show ();
		}

		protected void OnMaskError(object sender, string msg)
		{
			System.Diagnostics.Debug.WriteLine (msg);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

