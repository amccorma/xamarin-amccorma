using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics.Drawables;

namespace MaskedEditAndroid.Mask.Control
{
	public class MaskEdit : EditText
	{
		private bool Locked = false;
		private string LastText = "";
		private string CV_defaultMask = "{\\d:(\\d)?}";
		private string CV_defaultOneMask = "{\\d:}";
		private bool _FirstLoad = false;
		private Drawable _ErrorIcon;

		private class TextHolder : Java.Lang.Object
		{
			public TextHolder(string t, Int32 start, Int32 end = -1)
			{
				this.Text = t;
				this.SelectionStart = start;
				this.SelectionEnd = end;
			}

			public Int32 SelectionStart { get; set; }

			public Int32 SelectionEnd { get; set; }

			public string Text { get; set; }

			public string RemovedBlock { get; set; }

			public Int32 RemovedBlockCount { get; set; }

			public override string ToString ()
			{
				return Text;
			}
		}

		public MaskEdit (Context context) : base(context)
		{
			AddEventHandlers ();
		}

		public MaskEdit(Context context, IAttributeSet attributes) : base(context, attributes)
		{
			AddEventHandlers ();
		}

		private void AddEventHandlers()
		{
			this.TextChanged += EditTextChanged;
			this.BeforeTextChanged += MaskEdit_BeforeTextChanged;
			this.KeyPress += MyEntryEditText_KeyPress;
		}

		void MaskEdit_BeforeTextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			if (this.Locked || this._FirstLoad == false || this.Properties == null)
				return;			

			var start = this.SelectionStart;
			var end = this.SelectionEnd;
			var holder = new TextHolder (e.Text.ToString (), this.SelectionStart, this.SelectionEnd);
			holder.RemovedBlockCount = end - start;
			if (holder.RemovedBlockCount > 0 && this.Delete == false)
			{
				// text block selected. needs to be removed before update
				holder.RemovedBlock = holder.Text.Substring(SelectionEnd);
			}
			// old text
			this.Tag = holder;
		}
			
		public override void Draw (Android.Graphics.Canvas canvas)
		{
			base.Draw (canvas);

			if (this._FirstLoad == false) {

				// change to drawable
				this._ErrorIcon = Android.App.Application.Context.Resources.GetDrawable(Resource.Drawable.error1);
				this._ErrorIcon.SetBounds (0, 0, this._ErrorIcon.IntrinsicHeight, this._ErrorIcon.IntrinsicWidth); 

				if (this.Properties != null) {
					// run on load.  maybe better place to put this
					ApplyDefaultRule ();
				}

				this._FirstLoad = true;
			}
		}

		/// <summary>
		/// Control Properties
		/// </summary>
		/// <value>The properties.</value>
		public MaskProperties Properties { get; set; }

		/// <summary>
		/// Length from Max Length
		/// </summary>
		/// <value>The max length from mask.</value>
		private Int32 MaxLengthFromMask { get; set; }

		/// <summary>
		/// characters before the input change
		/// </summary>
		/// <value>The before chars.</value>
		private string BeforeChars { get; set; }

		/// <summary>
		/// delete or backspace key pressed
		/// </summary>
		private bool Delete { get; set; }

		/// <summary>
		/// Text without formating characters
		/// </summary>
		/// <value>The raw text.</value>
		public string RawText { get; set; }


		#region "Event Handlers"

		private void EditTextChanged(object sender, Android.Text.TextChangedEventArgs e)
		{
			if (this._FirstLoad == false || this.Properties == null)
				return;

			var InputText = Convert.ToString (e.Text);

			if (this.Locked == false && (this.LastText != this.Text) && String.IsNullOrEmpty (InputText) == false) {
				this.Locked = true;
				Int32 adjustedStart = 0;


				if (this.Properties.FormatCharacters == null || this.Properties.Mask == null) {
					this.Locked = false;
					return;
				}

				var holder = this.Tag as TextHolder;
				var BeforeCount = (holder == null) ? 0 : holder.Text.Length - 1;
				var AfterCount = (String.IsNullOrEmpty(InputText)) ? 0 : InputText.Length - 1;

				// check for block of test
				if (holder != null && String.IsNullOrEmpty (holder.RemovedBlock) == false) {
					var pos = InputText.IndexOf (holder.RemovedBlock);
					var s = pos - holder.RemovedBlockCount;
					InputText = InputText.Substring (0, s) + InputText.Substring (s + holder.RemovedBlockCount);
				}

				// selection start is not avaiable here after the text has been saved.
				// use TextHolder to get it
				var start = (holder == null) ? this.SelectionStart : holder.SelectionStart;
				System.Diagnostics.Debug.WriteLine ("cursor position: " + start);
				System.Diagnostics.Debug.WriteLine ("before change:= " + BeforeCount + ", after change:=" + AfterCount);

				var chars = this.Properties.FormatCharacters.ToCharArray ();
				var text = InputText.Replace (chars, "");
				var len = text.Length;

				var middle = false;
				System.Diagnostics.Debug.WriteLine ("in editext");
				if (this.Delete && start > 0) {					
					adjustedStart = adjustedStart - 1;
					System.Diagnostics.Debug.WriteLine ("start:= " + start + ", adjustedStart:= " + adjustedStart);
					this.BeforeChars = InputText.Substring (0, e.Start).Replace (chars, "");

				} else {
					if (start != InputText.Length - 1)
					{
						middle = true;
						if (start > InputText.Length -1)
						{
							start = InputText.Length;
							this.BeforeChars = InputText;
						}
						else
						{
							this.BeforeChars = InputText.Substring(0, start + 1).Replace(chars, "");
						}
					}
				}

				// check MaxLength for Mask
				if (this.MaxLengthFromMask <= 0)
				{
					// check length of last Mask and set MaxLength of not set already
					// this will set a MaxLength value to stop the mask
					this.MaxLengthFromMask = this.Properties.Mask.Last ().End;
				}

				var rule = this.Properties.Mask.FirstOrDefault (r => r.End >= len);
				if (rule == null) {
					// no rules found. get last rule
					rule = this.Properties.Mask.Last();
				} 
					
				if (rule.Mask != "") {
					System.Diagnostics.Debug.WriteLine ("a");

					// run Mask
					var temp = this.ApplyMask (text, rule);
					if (!this.Delete) {
						System.Diagnostics.Debug.WriteLine ("b");
						if (middle) {
							adjustedStart = 1;
							System.Diagnostics.Debug.WriteLine ("c");
						} else {
							if (e.AfterCount > e.BeforeCount) {
								System.Diagnostics.Debug.WriteLine ("d");
								adjustedStart = temp.Length - e.BeforeCount;
							} else {
								adjustedStart = 0;
								start = temp.Length;
								System.Diagnostics.Debug.WriteLine ("e");
							}
						}
					} else {
						// delete?
						var l = temp.Length;
						temp = temp.Substring (0, l - 1) + (chars.Any (x => x == temp [l - 1]) ? "" : temp [l - 1].ToString());
					}
					this.Text = temp;
					//var next = temp[start + adjustedStart-1];
					this.LastText = temp;

					System.Diagnostics.Debug.WriteLine ("1");
				} else if (rule.Mask == "" && this.Delete) {
					this.Text = text;
					this.LastText = text;
					System.Diagnostics.Debug.WriteLine ("1");
				} else {
					if (AfterCount > BeforeCount) {
						adjustedStart++;
					} else {
						adjustedStart--;
						System.Diagnostics.Debug.WriteLine ("3");
					}
				}
				this.RawText = this.Text.Replace (chars, "");
				UpdatSelectionPoint (new SelectionPoint (start + adjustedStart));
				this.Locked = false;
			}
		}

		private void MyEntryEditText_KeyPress (object sender, KeyEventArgs args)
		{
			// do not run anything if this is not set
			if (this.Properties == null) {
				args.Handled = false;
			}
			else
			{			
				if (args.Event.Action == global::Android.Views.KeyEventActions.Down) {
					var len = this.Text.Length;
					if (args.KeyCode == global::Android.Views.Keycode.Back ||
					   args.KeyCode == global::Android.Views.Keycode.Del) {
						System.Diagnostics.Debug.WriteLine ("delete pressed");

						// do test cleanup
						if (this.Locked == false && this.Properties.Mask != null) {
							System.Diagnostics.Debug.WriteLine ("delete pressed. variable set");
							this.Delete = true;
							args.Handled = false;
						} else {
							args.Handled = false;
						}
					} else if (this.Locked == false && this.Properties.Mask != null) {		
						this.Delete = false;
						var start = this.SelectionStart;
						if (start < len) {
							var evt = args.Event;
							var act = evt.Action;
							var newChar = ((char)evt.UnicodeChar).ToString ();

							this.Text = this.Text.Insert (start, newChar);
							args.Handled = true;
						} else {
							args.Handled = false;
						}
					} else if (this.Properties.MaxLength > 0) {
						if (len + 1 > this.Properties.MaxLength) {
							args.Handled = true;
							SetErrorMessage ("Max length is " + this.Properties.MaxLength);
						} else {
							SetErrorMessage ("");
							args.Handled = false;
						}
					} else {
						args.Handled = false;
					}
				}
			}
		}

		#endregion

		#region "Update Cursor"

		private void UpdatSelectionPoint(SelectionPoint pt)
		{
			if (pt != null && this.Properties.FormatCharacters != null)
			{
				if (pt.Start != -1) {
					if (pt.End != -1) {
						this.SetSelection (pt.Start, pt.End);
					} 
					else {
						if (pt.Start >= this.Text.Length) {
							pt.Start = this.Text.Length;
						}
						else
						{
							var before = this.BeforeChars;
							if (before == "")
							{
								pt.Start = 1;
							}
							else
							{
								var text = this.Text;

								for (int i = 0; i < text.Length; i++)
								{
									string c = text[i].ToString();
									if (this.Properties.FormatCharacters.Where(ch => ch.ToString() == c.ToString()).Count() <= 0)
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
						this.SetSelection(pt.Start);
					}
				}
				pt = null;
			}
		}

		#endregion

		#region "Startup Rule"

		protected internal void ApplyDefaultRule()
		{
			if (this.Properties == null)
				return;
			
			if (String.IsNullOrEmpty (this.Text)) {
				return;
			}

			if (String.IsNullOrEmpty (this.Properties.FormatCharacters)) {
				this.Properties.FormatCharacters = "";
			}

			var chars = this.Properties.FormatCharacters.ToCharArray ();
			this.Locked = true;
			var text = "";
			if (String.IsNullOrEmpty (this.Properties.FormatCharacters) == false)
				text = this.Text.Replace (chars, "");
			else
				text = this.Text;

			var len = text.Length;

			// update MaxLength
			if (this.Properties.MaxLength <= 0 && this.Properties.Mask != null) {
				this.Properties.MaxLength = this.Properties.Mask.LastOrDefault ().End;
			}

			if (this.Properties.MaxLength > -1) {
				if (len > this.Properties.MaxLength) {
					text = this.Text.Substring (0, this.Properties.MaxLength);
				}
			}

			var rules = this.Properties.Mask;
			if (rules != null) {

				var rule = rules.FirstOrDefault (r => r.End >= len);
				if (rule == null) {
					rule = rules.Find (r => r.End == rules.Max (m => m.End));
					text = text.Substring (0, rule.End);
				}           

				// text trimmed
				if (rule.Mask != "") {

					// check max length
					if (this.MaxLengthFromMask <= 0) {
						this.MaxLengthFromMask = this.Properties.Mask.LastOrDefault ().End;
					}
					this.Text = this.Text = this.ApplyMask (text, rule);
					this.SetSelection (this.Text.Length);
				}
			} else if (this.Properties.MaxLength > 0) {
				this.Text = text.Substring (0, this.Properties.MaxLength);
			}

			this.RawText = this.Text.Replace (chars, "");
			this.Locked = false;
		}

		#endregion

		protected internal void SetErrorMessage(string error)
		{
			if (String.IsNullOrEmpty (error)) {
				this.SetError ("", null);
			} else {
				this.SetError (error, this._ErrorIcon);
			}
		}

		private Int32[] ConvertMatch(string match)
		{
			match = match.Replace("{", "").Replace("}", "");
			var result = match.Split(new char[] { ':' }).Select(s => (s == "") ? 0 : int.Parse(s)).ToArray();		
			return result;
		}

		public string ApplyMask(string text, MaskRules rule)
		{
			string temp = "";
			if (rule.Mask != "")
			{
				// adjust length of greater than last mask entry
				if (text.Length > this.MaxLengthFromMask)
				{
					text = text.Substring (0, this.MaxLengthFromMask);
				}

				var result = System.Text.RegularExpressions.Regex.Match(rule.Mask, CV_defaultMask);
				temp = rule.Mask;
				do
				{
					if (System.Text.RegularExpressions.Regex.Match(result.Value, CV_defaultOneMask).Success || String.IsNullOrEmpty(result.Value))
					{
						// result is an empty string. return. done parsing
						if (String.IsNullOrEmpty(result.Value)) break;

						// end match                            
						var obj = ConvertMatch(result.Value);
						temp = temp.Replace(result.Value, text.Substring(obj[0]));
						break;
					}
					else
					{						
						var obj = ConvertMatch(result.Value);
						if (obj[0] + obj[1] >= text.Length)
						{
							// keeping format character at end??
							var t = result.Value[result.Value.Length-1];
							temp = temp.Replace(result.Value, text.Substring(obj[0])); 
						}
						else
						{
							temp = temp.Replace(result.Value, text.Substring(obj[0], obj[1]));
						}
						result = result.NextMatch();
					}
				} while (true);
				return temp;
			}
			return text;
		}

	}
}

