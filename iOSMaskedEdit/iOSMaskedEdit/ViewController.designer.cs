// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iOSMaskedEdit
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton button1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MaskEdit maskEntry { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField textView2 { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (button1 != null) {
				button1.Dispose ();
				button1 = null;
			}
			if (maskEntry != null) {
				maskEntry.Dispose ();
				maskEntry = null;
			}
			if (textView2 != null) {
				textView2.Dispose ();
				textView2 = null;
			}
		}
	}
}
