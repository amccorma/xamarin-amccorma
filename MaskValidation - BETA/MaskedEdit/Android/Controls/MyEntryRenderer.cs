using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics;
using System.Linq;
using System.Collections.Generic;
using Masked.Controls;
using Masked.Library;

[assembly: ExportRenderer(typeof(Masked.Controls.MyEntry), typeof(Masked.Android.Controls.MyEntryRenderer))]
namespace Masked.Android.Controls
{
	public class MyEntryRenderer : EntryRenderer
	{
		private MyEntry source;
		private EntryEditText native;
		private SelectionPoint pt;

		protected override void Dispose (bool disposing)
		{
			if (native != null) {
				native.AfterTextChanged -= Native_AfterTextChanged;
				native.KeyPress -= Native_KeyPress;
				native.BeforeTextChanged -= Native_BeforeTextChanged;
			}
			base.Dispose (disposing);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			if (native == null) {
				source = e.NewElement as MyEntry;
				native = this.Control as EntryEditText;

				if (source.FormatCharacters != null && String.IsNullOrEmpty(source.Text) == false) {
					ApplyDefaultRule ();
				}	

				native.AfterTextChanged += Native_AfterTextChanged;

				native.KeyPress += Native_KeyPress;

				native.BeforeTextChanged += Native_BeforeTextChanged;

				SetNativeControl(native);
			}
		}

		void Native_AfterTextChanged (object sender, global::Android.Text.AfterTextChangedEventArgs e)
		{
			if (pt != null && source.FormatCharacters != null)
			{
				var temp = pt;
				pt = null;
				native.Text = temp.Text;
				if (temp.Start != -1) {
					if (temp.End != -1) {
						native.SetSelection (temp.Start, temp.End);
					} 
					else {
						if (temp.Start >= native.Text.Length) {
							temp.Start = native.Text.Length;
						}
						else
						{
							var before = source.BeforeChars;
							if (before == "")
							{
								temp.Start = 1;
							}
							else
							{
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
						native.SetSelection(temp.Start);
					}
				}
				pt = null;
				source.Locked = false;
			}
		}

		void Native_BeforeTextChanged (object sender, global::Android.Text.TextChangedEventArgs e)
		{
			if (source.Locked == false && source.Mask != null)
			{
				source.SelectionStart = native.SelectionStart;
				source.SelectionEnd = native.SelectionEnd;
				source.TextLength = native.Text.Length;
			}
			else if(source.Mask == null)
			{
				source.SelectionStart = native.SelectionStart;
				source.SelectionEnd = native.SelectionEnd;
				source.TextLength = native.Text.Length;
			}
		}

		void Native_KeyPress (object sender, KeyEventArgs args)
		{
			if (args.Event.Action == global::Android.Views.KeyEventActions.Down )
			{
				var len = native.Text.Length;
				var evt = args.Event;
				var act = evt.Action;

				if (args.KeyCode == global::Android.Views.Keycode.Back ||
					args.KeyCode == global::Android.Views.Keycode.Del)
				{
					// do test cleanup
					if (source.Locked == false && source.Mask != null)
					{
						source.Delete = true;
						args.Handled = false;
					}
					else if(source.MaxLength > 0)
					{
						args.Handled = false;
					}
					else
					{
						args.Handled = false;
					}
				}
				else
				{
					var newChar = ((char)evt.UnicodeChar).ToString();
					var start = native.SelectionStart;
					var newText = native.Text.Insert(start, newChar);
					var valid = source.ValidateKeyDown (newText, newChar);
					if (source.Locked == false && source.Mask != null)
					{
						source.Delete = false;

						if (valid == false) {
							args.Handled = true;
						} else {
							if (start < len) {							
								native.Text = newText;
								args.Handled = true;
							} else {
								args.Handled = false;
							}
						}
					}
					else if(valid)
					{
						args.Handled = false;
					}
				}
			}
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

			var text = native.Text.Replace (chars, "");

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
					native.Text = source.ApplyMask (text, rule, true);
					native.SetSelection (native.Text.Length);
				} else {
					var v = source.Validate (native.Text);
					if (v == false) {
						native.Text = "";
					}
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
			}
		}
	}
}