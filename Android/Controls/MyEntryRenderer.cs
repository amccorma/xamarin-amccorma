using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics;
using System.Linq;
using System.Collections.Generic;
using GitHub.Controls;
using GitHub.Library;

[assembly: ExportRenderer (typeof (GitHub.Controls.MyEntry), typeof (GitHub.Android.MyEntryRenderer))]
namespace GitHub.Android
{
	public class MyEntryRenderer : EntryRenderer
	{
		private MyEntry source;
		private EntryEditText native;
		private List<MaskRules> rules;
		private SelectionPoint pt;
		private char[] FormatCharacters = null;

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			if (native == null) {
				source = e.NewElement as MyEntry;
				native = this.Control as EntryEditText;

				// INIT defaults
				rules = source.Mask;
				FormatCharacters = source.FormatCharacters.ToCharArray ();

				if (FormatCharacters != null && String.IsNullOrEmpty(source.Text) == false) {
					ApplyDefaultRule ();
				}	

				native.AfterTextChanged += (object sender, global::Android.Text.AfterTextChangedEventArgs e2) => {
					if (pt != null && FormatCharacters != null)
					{
						if (pt.Start != -1) {
							if (pt.End != -1) {
								native.SetSelection (pt.Start, pt.End);
							} 
							else {
								if (pt.Start >= native.Text.Length) {
									pt.Start = native.Text.Length;
								}
								else
								{
									var before = source.BeforeChars;
									if (before == "")
									{
										pt.Start = 1;
									}
									else
									{
										var text = native.Text;

										for (int i = 0; i < text.Length; i++)
										{
											string c = text[i].ToString();
											if (FormatCharacters.Where(ch => ch.ToString() == c.ToString()).Count() <= 0)
											{
												// no placeholder1
												if (before[0].ToString() == c)
												{
													before = before.Substring(1);												
												}

												if (String.IsNullOrEmpty(before))
												{
													pt.Start = i+1;
													break;
												}
											}
										}
									}	
								}
								native.SetSelection(pt.Start);
							}
						}
						pt = null;
					}
				};

				native.KeyPress += (object sender, KeyEventArgs args) => {
					var len = native.Text.Length;
					if (args.KeyCode == global::Android.Views.Keycode.Back ||
						args.KeyCode == global::Android.Views.Keycode.Del)
					{
						// do test cleanup
						if (source.Locked == false && args.Event.Action == global::Android.Views.KeyEventActions.Down && source.Mask != null)
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
					else if (source.Locked == false && args.Event.Action == global::Android.Views.KeyEventActions.Down && source.Mask != null)
					{
						source.Delete = false;
						var start = native.SelectionStart;
						if (start < len)
						{
							var evt = args.Event;
							var act = evt.Action;
							var newChar = ((char)evt.UnicodeChar).ToString();
							//var newChar = ((char)args.KeyCode.ConvertToString()).ToString();

							native.Text = native.Text.Insert(start, newChar);
							args.Handled = true;
						}
						else
						{
							args.Handled = false;
						}
					}
					else if(source.MaxLength > 0)
					{
						if (len+1 > source.MaxLength)
						{
							args.Handled = true;
							source.Validate("MAX", "Max length is " + source.MaxLength);
						}
						else
						{
							args.Handled = false;
						}
					}
					else
					{
						args.Handled = false;
					}
				};

				native.BeforeTextChanged += (object sender, global::Android.Text.TextChangedEventArgs e2) => {
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
				};

				SetNativeControl(native);
			}
		}

		private void ApplyDefaultRule()
		{
			source.Locked = true;
			var text = native.Text.Replace (FormatCharacters, "");
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
			} else if (e.PropertyName == "FormatCharacters") {
				this.FormatCharacters = source.FormatCharacters.ToCharArray ();
			}
		}
	}
}