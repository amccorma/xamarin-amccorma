using System;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using Masked.Controls;
using Masked.Library;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

[assembly: ExportRenderer(typeof(Masked.Controls.MyEntry), typeof(Masked.iOS.Controls.MyEntryRenderer))]
namespace Masked.iOS.Controls
{
	public class MyEntryRenderer : EntryRenderer
	{
		private MyEntry source;
		private List<MaskRules> rules;
		private SelectionPoint pt;
		private char[] FormatCharacters = null;
		private UITextField native;
		public bool isSecond = false;
		public bool ifIsInside = false;
		public int lastPos = 0;

		protected override void Dispose (bool disposing)
		{
			if (native != null) {
				native.ShouldChangeCharacters -= TextShouldChangeCharacters;
			}
			base.Dispose (disposing);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			if (native == null) {
				source = e.NewElement as MyEntry;
				var control = (MyEntry)this.Element;
				native = this.Control as UITextField;

				// INIT defaults
				rules = source.Mask;
				FormatCharacters = source.FormatCharacters.ToCharArray ();

				if (FormatCharacters != null && String.IsNullOrEmpty (source.Text) == false) {
					ApplyDefaultRule ();
				}	

				native.ShouldChangeCharacters += TextShouldChangeCharacters;
			}
		}

		private bool TextShouldChangeCharacters(UITextField textField, NSRange range, string replacementString) {
			native.BecomeFirstResponder ();
			source.Delete = false;
			if (replacementString == "") {
				source.Delete = true;
			}
			if (source.Locked) {
				return false;
			} else if (source.Mask != null) {
				source.SelectionStart = (int)textField.GetOffsetFromPosition (textField.BeginningOfDocument, textField.SelectedTextRange.Start);
				source.SelectionEnd = (int)textField.GetOffsetFromPosition (textField.BeginningOfDocument, textField.SelectedTextRange.End);
				source.TextLength = native.Text.Length;
			} else {
					return true;
			}

			return true;
		}

		private void ApplyDefaultRule()
		{
			source.Locked = true;
			var text = native.Text.Replace(FormatCharacters.ToString(), "");
			var len = text.Length;

			if (rules != null) {
				var rule = rules.FirstOrDefault (r => r.End >= len);
				if (rule == null) {
					rule = rules.Find (r => r.End == rules.Max (m => m.End));
					text = text.Substring (0, rule.End);
				}           

				// text trimmed
				if (rule.Mask != "") {
					native.Text = native.Text = source.ReFractor (text, rule);
				}
			}
			else if (source.MaxLength > -1) {
				if (len > source.MaxLength) {
					native.Text = native.Text.Substring (0, source.MaxLength);
				} else {
					native.Text = text;
				}
			}
			else			
			{
				native.Text = text;
			}

			source.Locked = false;
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == "SetSelection") {
				pt = source.SetSelection;
				if (pt != null && FormatCharacters != null) {
					if (pt.Start != -1) {
						if (pt.End != -1) {
							var positionToSet = native.GetPosition (native.BeginningOfDocument, pt.Start);
							native.SelectedTextRange = native.GetTextRange (positionToSet, positionToSet);
							//native.SetSelection (pt.Start, pt.End);
						} else {
							if (pt.Start >= native.Text.Length) {
								pt.Start = native.Text.Length;
							} else {
								var before = source.BeforeChars;
								if (before == "") {
									pt.Start = 1;
								} else {
									var text = native.Text;

									for (int i = 0; i < text.Length; i++) {
										string c = text [i].ToString ();
										if (FormatCharacters.Where (ch => ch.ToString () == c.ToString ()).Count () <= 0) {
											// no placeholder1
											//if (before [0].ToString () == c) {
											//	before = before.Substring (1);												
											//}

											//if (String.IsNullOrEmpty (before)) {
											//	pt.Start = i + 1;
											//	break;
											//}
										}
									}
								}	
							}
							//var getPos =native.GetOffsetFromPosition(native.BeginningOfDocument,native.SelectedTextRange.Start);
							//var positionToSet = native.GetPosition (native.BeginningOfDocument, getPos);
							if (source.Locked && source.Delete) {
								if (source.SelectionStart - 1 < native.Text.Length) {
									lastPos = source.SelectionStart;
									ifIsInside = true;


								}

							} else if (source.Locked && !source.Delete) {
								if (source.SelectionStart < native.Text.Length-1) {
									ifIsInside = true;
								} else {
									ifIsInside = false;
								}

							} else {

							}
							//native.SetSelection(pt.Start);
						}
					}
					pt = null;
					//source.Delete = false;
					source.Locked = false;
				}
			} else if (e.PropertyName == "FormatCharacters") {
				this.FormatCharacters = source.FormatCharacters.ToCharArray ();
			}
			else if (e.PropertyName == "Text") {
				pt = source.SetSelection;
				if (source.Delete) {
					//if (isSecond) {
					if (ifIsInside) {
						//if (lastPos < source.SelectionStart) {
						var positionToSet = native.GetPosition (native.BeginningOfDocument, source.SelectionStart - 1);
						native.SelectedTextRange = native.GetTextRange (positionToSet, positionToSet);
						//}
						//isSecond = false;
					}
				} else {
					if (ifIsInside) {
						var positionToSet = native.GetPosition (native.BeginningOfDocument, source.SelectionStart + 1);
						native.SelectedTextRange = native.GetTextRange (positionToSet, positionToSet);
					}
				}		 
			}
		}
	}
}