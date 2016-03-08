using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Foundation;
using UIKit;
using System.Linq;

namespace iOSMaskedEdit
{
	[Register("MaskEdit"), Browsable(true), DesignTimeVisible(true)]
	public class MaskEdit : UIKit.UITextField
	{
		private bool Locked = false;
		private string LastText = "";
		private string CV_defaultMask = "{\\d+:(\\d+)?}";
		private string CV_defaultOneMask = "{\\d+:}";
		private bool _FirstLoad = false;

		public EventHandler<string> OnError;
	
		public MaskEdit(IntPtr handle) : base(handle)
		{
			AddEventHandlers ();
		}

		public MaskEdit ()
		{
			AddEventHandlers ();
		}


		private void Initialize()
		{
			AddEventHandlers ();
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			RemoveEventHandlers ();
		}

		public override void Draw (CoreGraphics.CGRect rect)
		{
			base.Draw (rect);
			if (this._FirstLoad == false) {

				if (this.Properties != null) {
					// run on load.  maybe better place to put this
					ApplyDefaultRule ();
				}

				this._FirstLoad = true;
			}
		}

		private void RemoveEventHandlers()
		{
			this.ShouldChangeCharacters -= TextShouldChangeCharacters;
			this.EditingChanged -= MaskEdit_EditingChanged;
		}

		private void AddEventHandlers()
		{
			this.ShouldChangeCharacters += TextShouldChangeCharacters;
			this.EditingChanged += MaskEdit_EditingChanged;
		}

		void MaskEdit_EditingChanged (object sender, EventArgs e)
		{
			if (this._FirstLoad == false || this.Properties == null)
				return;

			var InputText = Convert.ToString (this.Text);

			if (this.Locked == false && (this.LastText != this.Text) && String.IsNullOrEmpty (InputText) == false) {
				this.Locked = true;
				Int32 adjustedStart = 0;


				if (this.Properties.FormatCharacters == null || this.Properties.Mask == null) {
					this.Locked = false;
					return;
				}

				var holder = this.holder;
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
				var start = (holder == null) ? 0 : holder.SelectionStart;
				System.Diagnostics.Debug.WriteLine ("cursor position: " + start);
				System.Diagnostics.Debug.WriteLine ("before change:= " + BeforeCount + ", after change:=" + AfterCount);

				var chars = this.Properties.FormatCharacters.ToCharArray ();
				var text = InputText.Replace (chars, "");
				var len = text.Length;

				// check start:
				if (start > InputText.Length) {
					start = InputText.Length;
				}

				var middle = false;
				System.Diagnostics.Debug.WriteLine ("in editext");
				if (this.DeleteKey && start > 0) {					
					adjustedStart = adjustedStart - 1;
					System.Diagnostics.Debug.WriteLine ("start:= " + start + ", adjustedStart:= " + adjustedStart);
					this.BeforeChars = InputText.Substring (0, start).Replace (chars, "");

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

				if (this.Properties.Mask != null) {
					var rule = this.Properties.Mask.FirstOrDefault (r => r.End >= len);
					if (rule == null) {
						// no rules found. get last rule
						rule = this.Properties.Mask.Last ();
					} 

					if (rule.Mask != "") {
						System.Diagnostics.Debug.WriteLine ("a");

						// run Mask
						var temp = this.ApplyMask (text, rule);
						if (!this.DeleteKey) {
							System.Diagnostics.Debug.WriteLine ("b");
							if (middle) {
								adjustedStart = 1;
								System.Diagnostics.Debug.WriteLine ("c");
							} else {
								//if (e.AfterCount > e.BeforeCount) {
								if (AfterCount > BeforeCount) {
									System.Diagnostics.Debug.WriteLine ("d");
									adjustedStart = temp.Length - BeforeCount;
								} else {
									adjustedStart = 0;
									start = temp.Length;
									System.Diagnostics.Debug.WriteLine ("e");
								}
							}
						} else {
							// delete?
							var l = temp.Length;
							temp = temp.Substring (0, l - 1) + (chars.Any (x => x == temp [l - 1]) ? "" : temp [l - 1].ToString ());
						}
						this.Text = temp;
						//var next = temp[start + adjustedStart-1];
						this.LastText = temp;

						System.Diagnostics.Debug.WriteLine ("1");
					} else if (rule.Mask == "" && this.DeleteKey) {
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
				} else {
					this.RawText = this.Text;
				}
				this.Locked = false;
			}
		}

		private bool TextShouldChangeCharacters(UITextField textField, NSRange range, string replacementString) {
			this.BecomeFirstResponder ();
			this.DeleteKey = false;
			if (replacementString == "") {
				this.DeleteKey = true;
			}
			else if (textField.Text.Length > this.MaxLengthFromMask && this.MaxLengthFromMask > 0) {
				return false;
			}

			if (this.Locked) {
				return false;
			}
			else if (this.Properties != null && this.Properties.Mask != null) {
				this.holder = new TextHolder ();
				holder.Text = textField.Text;
				this.holder.SelectionStart = (int)textField.GetOffsetFromPosition (textField.BeginningOfDocument, textField.SelectedTextRange.Start);
				this.holder.SelectionEnd = (int)textField.GetOffsetFromPosition (textField.BeginningOfDocument, textField.SelectedTextRange.End);
			}
			else if (this.Properties.MaxLength > 0) {
				if (textField.Text.Length + 1 > this.Properties.MaxLength) {
					SetErrorMessage ("Max length = " + this.Properties.MaxLength);
					return false;
				}
			}
			else {
				return true;
			}

			return true;
		}

		private TextHolder holder { get; set; }

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
		private bool DeleteKey { get; set; }

		/// <summary>
		/// Text without formating characters
		/// </summary>
		/// <value>The raw text.</value>
		public string RawText { get; set; }

		protected internal void SetErrorMessage(string error)
		{
			var handler = this.OnError;
			if (handler != null) {
				handler (this, error);
			}
		}

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
					this.Text = this.ApplyMask (text, rule);
				}
			} else if (this.Properties.MaxLength > 0) {
				var m = this.Properties.MaxLength;
				if (m > this.Text.Length) {
					m = this.Text.Length;
				}
				this.Text = text.Substring (0, m);
			}

			this.RawText = this.Text.Replace (chars, "");
			this.Locked = false;
		}

		private void UpdatSelectionPoint(SelectionPoint pt)
		{
			if (pt != null && this.Properties.FormatCharacters != null)
			{
				if (pt != null && this.Properties.FormatCharacters != null) {
					var temp = pt;
					pt = null;
					if (temp.Start != -1) {
						if (temp.End != -1) {
							var positionToSet = this.GetPosition (this.BeginningOfDocument, temp.Start);
							this.SelectedTextRange = this.GetTextRange (positionToSet, positionToSet);
						} else {
							if (temp.Start >= this.Text.Length) {
								temp.Start = this.Text.Length;
							} else {
								var before = this.BeforeChars;
								if (before == "") {
									temp.Start = 1;
								} else {
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
												temp.Start = i+1;
												break;
											}
										}
									}
								}	
							}
							var positionToSet = this.GetPosition (this.BeginningOfDocument, temp.Start);
							this.SelectedTextRange = this.GetTextRange (positionToSet, positionToSet);
						}
					}
				}
				this.Locked = false;
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

