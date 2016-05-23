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

				if (source.FormatCharacters != null && String.IsNullOrEmpty (source.Text) == false) {
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
			else if (textField.Text.Length > source.MaxLengthFromMask && source.MaxLengthFromMask > 0) {
				return false;
			}

			if (source.Locked) {
				return false;
			}
			else if (source.Mask != null) {
				source.SelectionStart = (int)textField.GetOffsetFromPosition (textField.BeginningOfDocument, textField.SelectedTextRange.Start);
				source.SelectionEnd = (int)textField.GetOffsetFromPosition (textField.BeginningOfDocument, textField.SelectedTextRange.End);
				source.TextLength = native.Text.Length;
			} else {
				return true;
			}

			return true;
		}

		protected internal void ApplyDefaultRule()
		{
			source.Locked = true;
			if (String.IsNullOrEmpty (native.Text)) {
				return;
			}

			if (String.IsNullOrEmpty (source.FormatCharacters)) {
				source.FormatCharacters = "";
			}

			var chars = source.FormatCharacters.ToCharArray ();

			var text = native.Text.Replace (FormatCharacters, "");

			if (String.IsNullOrEmpty (source.FormatCharacters) == false)
				text = text.Replace (chars, "");

			var len = text.Length;

			// update MaxLength
			if (source.MaxLength <= 0 && source.Mask != null) {
				source.MaxLength = source.Mask.LastOrDefault ().End;
			}

			if (source.MaxLength > -1) {
				if (len > source.MaxLength) {
					text = text.Substring (0, source.MaxLength);
				}
			}

			var rules = source.Mask;
			if (rules != null) {

				var rule = rules.FirstOrDefault (r => r.End >= len);
				if (rule == null) {
					rule = rules.Find (r => r.End == rules.Max (m => m.End));
					text = text.Substring (0, rule.End);
				}           

				// text trimmed
				if (rule.Mask != "") {

					// check max length
					if (source.MaxLengthFromMask <= 0) {
						source.MaxLengthFromMask = source.Mask.LastOrDefault ().End;
					}
					native.Text = source.ApplyMask (text, rule);
				}
			} else if (source.MaxLength > 0) {
				native.Text = text.Substring (0, source.MaxLength);
			}

			source.RawText = source.Text.Replace (chars, "");
			source.Locked = false;
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == "SetSelection") {
				pt = source.SetSelection;
				if (pt != null && source.FormatCharacters != null) {
					var temp = pt;
					pt = null;
					native.Text = temp.Text;
					if (temp.Start != -1) {
						if (temp.End != -1) {
							var positionToSet = native.GetPosition (native.BeginningOfDocument, temp.Start);
							native.SelectedTextRange = native.GetTextRange (positionToSet, positionToSet);
						} else {
							if (temp.Start >= native.Text.Length) {
								temp.Start = native.Text.Length;
							} else {
								var before = source.BeforeChars;
								if (before == "") {
									temp.Start = 1;
								} else {
									var text = native.Text;

									for (int i = 0; i < text.Length; i++)
									{
										string c = text[i].ToString();
										if (source.FormatCharacters.Where(ch => ch.ToString() == c.ToString()).Count() <= 0)
										{
											// no placeholder1
											if (before[0].ToString() == c)
											{
												before = before.Substring(1);												
											}

											if (String.IsNullOrEmpty(before))
											{
												temp.Start = i+1;
												break;
											}
										}
									}
								}	
							}
							var positionToSet = native.GetPosition (native.BeginningOfDocument, temp.Start);
							native.SelectedTextRange = native.GetTextRange (positionToSet, positionToSet);
						}
					}
				}
				source.Locked = false;
			} else if (e.PropertyName == "FormatCharacters") {
				this.FormatCharacters = source.FormatCharacters.ToCharArray ();
			}
		}
	}
}