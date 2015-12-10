using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Masked.Library;

namespace Masked.Controls
{
	public class MyEntry : Entry
	{
		public delegate void InValid(object sender, string message);

		public event InValid OnValidationError;
		//		public Task ValidatorTask;
		//		public CancellationToken token;

		private string CV_defaultMask = "{\\d:(\\d)?}";
		private string CV_defaultOneMask = "{\\d:}";
		public static readonly BindableProperty LastTextProperty =
			BindableProperty.Create<MyEntry, string>(
				p => p.LastText, "");

		/// <summary>
		/// The font property
		/// </summary>
		public static readonly BindableProperty MaxLengthProperty =
			BindableProperty.Create("MaxLength", typeof(Int32), typeof(MyEntry), -1);


		/// <summary>
		/// The font property
		/// </summary>
		public static readonly BindableProperty FontProperty =
			BindableProperty.Create("Font", typeof(Font), typeof(MyEntry), new Font());

		/// <summary>
		/// The XAlign property
		/// </summary>
		public static readonly BindableProperty XAlignProperty =
			BindableProperty.Create("XAlign", typeof(TextAlignment), typeof(MyEntry),
				TextAlignment.Start);

		/// <summary>
		/// The HasBorder property
		/// </summary>
		public static readonly BindableProperty HasBorderProperty =
			BindableProperty.Create("HasBorder", typeof(bool), typeof(MyEntry), true);

		/// <summary>
		/// The PlaceholderTextColor property
		/// </summary>
		public static readonly BindableProperty PlaceholderTextColorProperty =
			BindableProperty.Create("PlaceholderTextColor", typeof(Color), typeof(MyEntry), Color.Default);

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty CharacterMatchProperty =
			BindableProperty.Create<MyEntry, string>(
				p => p.CharacterMatch, "");

		public static readonly BindableProperty TextLengthProperty =
			BindableProperty.Create<MyEntry, Int32>(
				p => p.TextLength, -1);

		public static readonly BindableProperty SelectionEndProperty =
			BindableProperty.Create<MyEntry, Int32>(
				p => p.SelectionEnd, -1);

		public static readonly BindableProperty SetSelectionProperty =
			BindableProperty.Create<MyEntry, SelectionPoint>(
				p => p.SetSelection, new SelectionPoint(-1));

		public static readonly BindableProperty SelectionStartProperty =
			BindableProperty.Create<MyEntry, Int32>(
				p => p.SelectionStart, -1);

		public static readonly BindableProperty MaskProperty =
			BindableProperty.Create<MyEntry, List<MaskRules>>(
				p => p.Mask, null);

		//		public static readonly BindableProperty MaxLengthProperty =
		//			BindableProperty.Create<MyEntry, Int32>(
		//				p => p.MaxLength, -1);

		public static readonly BindableProperty LockedProperty =
			BindableProperty.Create<MyEntry, bool>(
				p => p.Locked, false);

		public static readonly BindableProperty DeleteProperty =
			BindableProperty.Create<MyEntry, bool>(
				p => p.Delete, false);

		public static readonly BindableProperty FormatCharactersProperty =
			BindableProperty.Create<MyEntry, string>(
				p => p.FormatCharacters, "");

		public static readonly BindableProperty RawTextProperty =
			BindableProperty.Create<MyEntry, string>(
				p => p.RawText, "");

		public List<MaskRules> Mask
		{
			get { return (List<MaskRules>)GetValue(MaskProperty); }
			set { 
				SetValue(MaskProperty, value); 
			}
		}

		public SelectionPoint SetSelection
		{
			get { return (SelectionPoint)GetValue(SetSelectionProperty); }
			set { SetValue(SetSelectionProperty, value); }
		}


		/// <summary>
		/// Gets or sets the length of the text.
		/// </summary>
		/// <value>The length of the text.</value>
		public Int32 TextLength
		{
			get { return (Int32)GetValue(TextLengthProperty); }
			set { SetValue(TextLengthProperty, value); }
		}

		public string CharacterMatch
		{
			get { return (string)GetValue(CharacterMatchProperty); }
			set { SetValue(CharacterMatchProperty, value); }
		}

		/// <summary>
		/// Gets or sets the selection start.
		/// </summary>
		/// <value>The selection start.</value>
		public Int32 SelectionStart
		{
			get { return (Int32)GetValue(SelectionStartProperty); }
			set { SetValue(SelectionStartProperty, value); }
		}

		/// <summary>
		/// Gets or sets the selection end.
		/// </summary>
		/// <value>The selection end.</value>
		public Int32 SelectionEnd
		{
			get { return (Int32)GetValue(SelectionEndProperty); }
			set { SetValue(SelectionEndProperty, value); }
		}

		/// <summary>
		/// Gets or sets the format characters.
		/// </summary>
		/// <value>The format characters.</value>
		public string FormatCharacters
		{
			get { return (string)GetValue(FormatCharactersProperty); }
			set { SetValue(FormatCharactersProperty, value); }
		}

		/// <summary>
		/// Gets or sets the last text.
		/// </summary>
		/// <value>The last text.</value>
		public string LastText
		{
			get { return (string)GetValue(LastTextProperty); }
			set { SetValue(LastTextProperty, value); }
		}

		/// <summary>
		/// Gets or sets the before chars.
		/// </summary>
		/// <value>The before chars.</value>
		public string BeforeChars { get; set; }

		/// <summary>
		/// Gets or sets the delete.
		/// </summary>
		/// <value>The delete.</value>
		public bool Delete
		{
			get { return (bool)GetValue(DeleteProperty); }
			set { SetValue(DeleteProperty, value); }
		}

		/// <summary>
		/// Gets or sets the locked.
		/// </summary>
		/// <value>The locked.</value>
		public bool Locked
		{
			get { return (bool)GetValue(LockedProperty); }
			set { SetValue(LockedProperty, value); }
		}

		/// <summary>
		/// Text without formating characters
		/// </summary>
		/// <value>The raw text.</value>
		public string RawText
		{
			get { return (string)GetValue(RawTextProperty); }
			set { SetValue(RawTextProperty, value); }
		}


		public List<Validation> ValidationRules { get; set; }

		public Int32 MaxLengthFromMask { get; set; }

		public void ValidationError(string message)
		{
			var handler = OnValidationError;
			if (handler != null) {
				handler (this, message);
			}
		}

		public bool ValidateKeyDown(string text, string newCharacter)
		{
			bool v = true;
			if (String.IsNullOrEmpty(text) || ValidationRules == null)
				return true;				

			var temp = StripCharacters(text);

			var validCharacters = ValidationRules.FirstOrDefault (x => x.Operation == Validators.ONLYCHARS);
			if (String.IsNullOrEmpty (newCharacter) == false && validCharacters != null) {
				v = System.Text.RegularExpressions.Regex.Match (newCharacter, validCharacters.Arg).Success;
				if (v == false) {
					ValidationError (validCharacters.ErrorMessage);
				}
			}

			if (v) {
				var max = ValidationRules.FirstOrDefault (x => x.Operation == Validators.MAX);
				if (max != null && text.Length > max.ArgAsInt) {
					v = false;
					ValidationError (max.ErrorMessage);
				}

				if (v && this.Mask != null) {
					var l = text.Length;
					var t = "";
					foreach (var rule in this.Mask) {
						if (l - 1 > rule.End) {
							t = temp.Substring (rule.Start, rule.End - rule.Start);
							v = Validate (t, rule);
							if (v == false)
								return false;
						} else if (l > rule.Start && l < rule.End) {
							t = temp.Substring (rule.Start);
							return Validate (t, rule);
						}
					}
				}
			}

			return v;
		}

		public bool Validate(string text)
		{
			bool v = true;
			if (String.IsNullOrEmpty(text))
				return true;				

			if (this.Mask != null) {
				var rule = this.Mask.FirstOrDefault(r => r.End >= text.Length);
				if (rule == null)
				{
					// no rules found. get last rule
					rule = this.Mask.Last();
				}
				return Validate (StripCharacters(text), rule);
			}

			return v;
		}

		public bool Validate(string text, MaskRules rule)
		{
			bool v = true;
			if (String.IsNullOrEmpty(text))
				return true;

			var validCharacters = ValidationRules.FirstOrDefault (x => x.Operation == Validators.ONLYCHARS);
			if (ValidationRules != null) {
				v = System.Text.RegularExpressions.Regex.Match (text, validCharacters.Arg).Success;
				ValidationError (validCharacters.ErrorMessage);
			}

			if (v) {
				var max = ValidationRules.FirstOrDefault (x => x.Operation == Validators.MAX);
				if (max != null && text.Length > max.ArgAsInt) {
					v = false;
					ValidationError (max.ErrorMessage);
				}
			}

			if (v && rule != null && rule.Rules != null) {
				var startC = rule.Rules.FirstOrDefault (x => x.Operation == Validators.STARTC);
				if (startC != null) {
					if (startC.Arg.Length > text.Length - rule.Start) {
						v = String.Compare (text.Substring (rule.Start), startC.Arg.Substring (0, text.Length - rule.Start), 
							(startC.CaseCheck == false ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture)) == 0;
						ValidationError (startC.ErrorMessage);
					} else if (startC.Arg.Length > text.Length - rule.Start) {
						v = String.Compare (text.Substring (rule.Start, startC.Arg.Length), startC.Arg, 
							(startC.CaseCheck == false ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture)) == 0;
						ValidationError (startC.ErrorMessage);
					}
				} 

				var endC = rule.Rules.FirstOrDefault (x => x.Operation == Validators.ENDC);
				if (endC != null) {
					var d = rule.End - endC.Arg.Length;
					if (text.Length + endC.Arg.Length > rule.End) {
						var test = text.Substring (d);
						v = String.Compare (test, endC.Arg.Substring (0, test.Length), 
							(endC.CaseCheck == false ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture)) == 0;
						ValidationError (endC.ErrorMessage);
					}
				}
			}

			return v;
		}

		private string StripCharacters(string text)
		{
			if (String.IsNullOrEmpty (this.FormatCharacters))
				return text;
			
			return text.Replace(this.FormatCharacters.ToCharArray(), "");
		}

		public MyEntry()
		{

			this.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				if (this.Mask != null)
				{
					if (this.Locked == false && (this.LastText != this.Text) && String.IsNullOrEmpty(this.Text) == false)
					{
						this.Locked = true;

						if (this.FormatCharacters == null) {
							this.Locked = false;
							return;
						}

						var chars = this.FormatCharacters.ToCharArray ();
						Int32 adjustedStart = 0;
						this.Locked = true;
						var start = this.SelectionStart;
						var text = this.Text.Replace(chars, "");
						var len = text.Length;
						var middle = false;
						if (Delete && start > 0)
						{
							adjustedStart = adjustedStart - 1;
							BeforeChars = this.Text.Substring(0, start - 1).Replace(chars, "");
						}
						else
						{
							if (start != this.Text.Length - 1)
							{
								middle = true;
								if (start > this.Text.Length -1)
								{
									start = this.Text.Length;
									BeforeChars = this.Text;
								}
								else
								{
									BeforeChars = this.Text.Substring(0, start + 1).Replace(chars, "");
								}
							}
						}

						// check MaxLength for Mask
						if (this.MaxLengthFromMask <= 0)
						{
							// check length of last Mask and set MaxLength of not set already
							// this will set a MaxLength value to stop the mask
							this.MaxLengthFromMask = this.Mask.Last ().End;
						}

						var finalText = "";
						var rule = this.Mask.FirstOrDefault(r => r.End >= len);
						if (rule == null)
						{
							// no rules found. get last rule
							rule = this.Mask.Last();
						}

						
						if (rule.Mask != "")
						{
							var temp = ApplyMask(text, rule);
							if (!Delete)
							{
								if (middle)
								{
									adjustedStart = 1;
								}
								else
								{
									if (temp.Length > e.OldTextValue.Length)
									{
										adjustedStart = temp.Length - e.OldTextValue.Length;
									}
									else
									{
										adjustedStart = 0;
										start = temp.Length;
									}
								}
							}
							finalText = temp;
							//var next = temp[start + adjustedStart-1];
							this.LastText = temp;
						}
						else if (rule.Mask == "" && this.Delete)
						{
							finalText = text;
							this.LastText = text;
						}
						else
						{
							if (e.NewTextValue.Length > e.OldTextValue.Length)
							{
								adjustedStart++;
							}
							else
							{
								adjustedStart--;
							}
							finalText = this.Text.Replace (chars, "");
						}

						this.RawText = this.Text.Replace (chars, "");
						var pt = new SelectionPoint(start + adjustedStart);
						pt.Text = finalText;
						SetSelection = pt;
					}
				}
			};
		}


		public Int32 MaxLength
		{
			get { return (Int32)GetValue(MaxLengthProperty); }
			set { SetValue(MaxLengthProperty, value); }
		}

		private Int32[] ConvertMatch(string match)
		{
			match = match.Replace("{", "").Replace("}", "");
			return match.Split(new char[] { ':' }).Select(s => (s == "") ? 0 : int.Parse(s)).ToArray();
		}

		public string ApplyMask(string text, MaskRules rule, bool IsFirstLoad = false)
		{
			string temp = "";
			if (rule.Mask != "")
			{
				// adjust length of greater than last mask entry
				if (text.Length > this.MaxLengthFromMask)
				{
					text = text.Substring (0, this.MaxLengthFromMask);
				}
					
				var test = "";
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
						test += text.Substring(obj[0]);
						if (IsFirstLoad && Validate(text) == false)
						{
							return "";
						}
						temp = temp.Replace(result.Value, text.Substring(obj[0]));
						break;
					}
					else
					{						
						var obj = ConvertMatch(result.Value);
						if (obj[0] + obj[1] >= text.Length)
						{
							test += text.Substring(obj[0]);
							if (IsFirstLoad && Validate(text) == false)
							{
								return "";
							}
							var t = result.Value[result.Value.Length-1];
							temp = temp.Replace(result.Value, test); 
						}
						else
						{
							test += text.Substring(obj[0], obj[1]);
							if (IsFirstLoad && Validate(text) == false)
							{
								return "";
							}
							temp = temp.Replace(result.Value, test);
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

