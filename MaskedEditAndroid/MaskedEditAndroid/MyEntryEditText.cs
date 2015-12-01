using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics.Drawables;

namespace MaskedEditAndroid
{
	public class MyEntryEditText : EditText
	{
		private bool Locked = false;
		private string LastText = "";
		private string CV_defaultMask = "{\\d:(\\d)?}";
		private string CV_defaultOneMask = "{\\d:}";
		private SelectionPoint pt;
		private bool _FirstLoad = false;
		private Drawable _ErrorIcon;

		public MyEntryEditText (Context context) : base(context)
		{
		}

		public MyEntryEditText(Context context, IAttributeSet attributes) : base(context, attributes)
		{
			this.TextChanged += EditTextChanged;
			this.KeyPress += MyEntryEditText_KeyPress;
		}

		public override void Draw (Android.Graphics.Canvas canvas)
		{
			base.Draw (canvas);

			if (this._FirstLoad == false) {

				// change to drawable
				this._ErrorIcon = Android.App.Application.Context.Resources.GetDrawable(Resource.Drawable.error1);
				this._ErrorIcon.SetBounds (0, 0, this._ErrorIcon.IntrinsicHeight, this._ErrorIcon.IntrinsicWidth); 

				// run on load.  maybe better place to put this
				ApplyDefaultRule ();
				this._FirstLoad = true;
			}
		}

		protected internal void ApplyDefaultRule()
		{
			if (String.IsNullOrEmpty (this.Text)) {
				GetMaxLengthFromMask ();
				return;
			}

			if (String.IsNullOrEmpty (this.FormatCharacters)) {
				FormatCharacters = "";
			}
			
			this.Locked = true;
			var text = "";
			if (String.IsNullOrEmpty (this.FormatCharacters) == false)
				text = this.Text.Replace (FormatCharacters, "");
			else
				text = this.Text;
			
			var len = text.Length;

			var rules = this.Mask;
			if (rules != null) {

				GetMaxLengthFromMask ();

				var rule = rules.FirstOrDefault (r => r.End >= len);
				if (rule == null) {
					rule = rules.Find (r => r.End == rules.Max (m => m.End));
					text = text.Substring (0, rule.End);
				}           

				// text trimmed
				if (rule.Mask != "") {
					this.Text = this.Text = this.ReFractor (text, rule);
				}
			}
			else if (this.MaxLength > -1) {
				if (len > this.MaxLength) {
					this.Text = this.Text.Substring (0, this.MaxLength);
				} else {
					this.Text = text;
				}
			}
			else			
			{
				this.Text = text;
			}
			this.Locked = false;
		}

		void MyEntryEditText_KeyPress (object sender, KeyEventArgs args)
		{
			if (args.Event.Action == global::Android.Views.KeyEventActions.Down )
			{
				var len = this.Text.Length;
				if (args.KeyCode == global::Android.Views.Keycode.Back ||
					args.KeyCode == global::Android.Views.Keycode.Del)
				{
					// do test cleanup
					if (this.Locked == false && this.Mask != null)
					{
						this.Delete = true;
						args.Handled = false;
					}
					else if(this.MaxLength > 0)
					{
						args.Handled = false;
					}
					else
					{
						args.Handled = false;
					}
				}
				else if (this.Locked == false && this.Mask != null)
				{
					if (len + 1 > this.MaxLength) {
						args.Handled = true;
						SetErrorMessage ("Max length is " + this.MaxLength);
					} else {		
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
					}
				}
				else if(this.MaxLength > 0)
				{
					if (len+1 > this.MaxLength)
					{
						args.Handled = true;
						SetErrorMessage("Max length is " + this.MaxLength);
					}
					else
					{
						SetErrorMessage ("");
						args.Handled = false;
					}
				}
				else
				{
					args.Handled = false;
				}
			}
		}

		protected internal void SetErrorMessage(string error)
		{
			if (String.IsNullOrEmpty (error)) {
				this.SetError ("", null);
			} else {
				this.SetError (error, this._ErrorIcon);
			}
		}

		void UpdatSelectionPoint(SelectionPoint pt)
		{
			if (pt != null && FormatCharacters != null)
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
						this.SetSelection(pt.Start);
					}
				}
				pt = null;
			}
		}

		private void EditTextChanged(object sender, Android.Text.TextChangedEventArgs e)
		{
			if (this.Locked == false && (this.LastText != this.Text) && String.IsNullOrEmpty (this.Text) == false) {
				this.Locked = true;
				Int32 adjustedStart = 0;
				this.Locked = true;
				var start = this.SelectionStart;
				if (this.FormatCharacters == null || this.Mask == null)
					return;

				var text = this.Text.Replace (this.FormatCharacters.ToCharArray (), "");
				var len = text.Length;
				var middle = false;
				if (Delete && start > 0) {
					adjustedStart = adjustedStart - 1;
					BeforeChars = this.Text.Substring (0, start - 1).Replace (this.FormatCharacters.ToCharArray (), "");
				} else {
					if (start != this.Text.Length - 1) {
						middle = true;
						if (start > this.Text.Length - 1) {
							start = this.Text.Length;
							BeforeChars = this.Text;
						} else {
							BeforeChars = this.Text.Substring (0, start + 1).Replace (this.FormatCharacters.ToCharArray (), "");
						}
					}

				}

				var rule = this.Mask.FirstOrDefault (r => r.End >= len);
				if (rule == null) {
					var temp = text.Substring (0, text.Length - 1);
					this.Text = temp;

				} else {
					if (rule.Mask != "") {
						var temp = ReFractor (text, rule);
						if (!Delete) {
							if (middle) {
								adjustedStart = 1;
							} else {
								if (e.AfterCount > e.BeforeCount) {
									adjustedStart = temp.Length - e.BeforeCount;
								} else {
									adjustedStart = 0;
									start = temp.Length;
								}
							}
						}
						this.Text = temp;
						//var next = temp[start + adjustedStart-1];
						this.LastText = temp;
					} else if (rule.Mask == "" && this.Delete) {
						this.Text = text;
						this.LastText = text;
					} else {
						if (e.AfterCount > e.BeforeCount) {
							adjustedStart++;
						} else {
							adjustedStart--;
						}
					}
				}
					
				this.Locked = false;
				UpdatSelectionPoint (new SelectionPoint (start + adjustedStart));
			}
		}

		private Int32[] ConvertMatch(string match)
		{
			match = match.Replace("{", "").Replace("}", "");
			return match.Split(new char[] { ':' }).Select(s => (s == "") ? 0 : int.Parse(s)).ToArray();
		}

		public string ReFractor(string text, MaskRules rule)
		{
			string temp = "";
			if (rule.Mask != "")
			{
				var result = System.Text.RegularExpressions.Regex.Match(rule.Mask, CV_defaultMask);
				temp = rule.Mask;
				do
				{
					if (System.Text.RegularExpressions.Regex.Match(result.Value, CV_defaultOneMask).Success)
					{
						// end match                            
						var obj = ConvertMatch(result.Value);
						temp = temp.Replace(result.Value, text.Substring(obj[0]));
						break;
					}
					else
					{
						var obj = ConvertMatch(result.Value);
						temp = temp.Replace(result.Value, text.Substring(obj[0], obj[1]));
						result = result.NextMatch();
					}
				} while (true);
				return temp;
			}
			return text;
		}


		public List<MaskRules> Mask { get; set; }

		public Int32 MaxLength
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the before chars.
		/// </summary>
		/// <value>The before chars.</value>
		public string BeforeChars { get; set; }

		public bool Delete {
			get;
			set;
		}

		public string CharacterMatch
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the format characters.
		/// </summary>
		/// <value>The format characters.</value>
		public string FormatCharacters
		{
			get;
			set;
		}
	}
}

