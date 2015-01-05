using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using GitHub.Library;

namespace GitHub.Controls
{
	public class MyEntry : Entry
	{
		public delegate void InValid(string prop, string message);
		public event InValid OnValidate;
		private string CV_defaultMask = "{\\d:(\\d)?}";
		private string CV_defaultOneMask = "{\\d:}";

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

		public MyEntry ()
		{
			this.TextChanged += (object sender, TextChangedEventArgs e) => {

				if (this.Mask != null)
				{
					if (this.Locked == false && (this.LastText != this.Text) && String.IsNullOrEmpty(this.Text) == false)
					{
						this.Locked = true;
						Int32 adjustedStart = 0;
						this.Locked = true;
						var start = this.SelectionStart;
						var text = this.Text.Replace(this.FormatCharacters.ToCharArray(), "");
						var len = text.Length;
						var middle = false;
						if (Delete && start > 0)
						{
							adjustedStart = adjustedStart -1;
							BeforeChars = this.Text.Substring(0, start-1).Replace(this.FormatCharacters.ToCharArray(), "");
						}
						else
						{
							if (start != this.Text.Length -1)
							{
								middle = true;
								BeforeChars = this.Text.Substring(0, start+1).Replace(this.FormatCharacters.ToCharArray(), "");
							}
						}


						var rule = this.Mask.FirstOrDefault(r => r.End >= len);
						if (rule == null)
						{
							var temp = text.Substring(0, text.Length - 1);
							this.Text = temp;
							//native.SetSelection(native.Text.Length);
						}
						else
						{
							if (rule.Mask != "")
							{
								var temp = ReFractor(text, rule);
								if (! Delete)
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
								this.Text = temp;
								//var next = temp[start + adjustedStart-1];
								this.LastText = temp;
							}
							else if (rule.Mask == "" && this.Delete)
							{
								this.Text = text;
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
							}
						}   

						this.Locked = false;
						this.SetSelection = new SelectionPoint(start + adjustedStart);	
					}
				}
			};
		}

		public string BeforeChars {get;set;}


		public void Validate(string prop, string message)
		{
			var method = OnValidate;
			if (method != null)
			{
				OnValidate(prop, message);
			}
		}

		public static readonly BindableProperty	LastTextProperty =
			BindableProperty.Create<MyEntry, string>( 
				p => p.LastText, "");

		public static readonly BindableProperty	TextLengthProperty =
			BindableProperty.Create<MyEntry, Int32>( 
				p => p.TextLength, -1);

		public static readonly BindableProperty	SelectionEndProperty =
			BindableProperty.Create<MyEntry, Int32>( 
				p => p.SelectionEnd, -1);

		public static readonly BindableProperty	SetSelectionProperty =
			BindableProperty.Create<MyEntry, SelectionPoint>( 
				p => p.SetSelection, new SelectionPoint(-1));

		public static readonly BindableProperty	SelectionStartProperty =
			BindableProperty.Create<MyEntry, Int32>( 
				p => p.SelectionStart, -1);

		public static readonly BindableProperty	MaskProperty =
			BindableProperty.Create<MyEntry, List<MaskRules>>( 
				p => p.Mask, null);

		public static readonly BindableProperty MaxLengthProperty =
			BindableProperty.Create<MyEntry, Int32>(
				p => p.MaxLength, -1);

		public static readonly BindableProperty	LockedProperty =
			BindableProperty.Create<MyEntry, bool>( 
				p => p.Locked, false);

		public static readonly BindableProperty DeleteProperty =
			BindableProperty.Create<MyEntry, bool>( 
				p => p.Delete, false);

		public static readonly BindableProperty FormatCharactersProperty =
			BindableProperty.Create<MyEntry, string>( 
				p => p.FormatCharacters, "");

		public List<MaskRules> Mask {
			get { return (List<MaskRules>)GetValue (MaskProperty); }
			set { SetValue (MaskProperty, value); }
		}

		public SelectionPoint SetSelection {
			get { return (SelectionPoint)GetValue (SetSelectionProperty); }
			set { SetValue (SetSelectionProperty, value); }
		}

		public Int32 MaxLength
		{
			get { return (Int32)GetValue(MaxLengthProperty); }
			set { SetValue(MaxLengthProperty, value); }
		}

		public bool Delete {
			get { return (bool)GetValue (DeleteProperty); }
			set { SetValue (DeleteProperty, value); }
		}

		public bool Locked {
			get { return (bool)GetValue (LockedProperty); }
			set { SetValue (LockedProperty, value); }
		}

		public Int32 TextLength {
			get { return (Int32)GetValue (TextLengthProperty); }
			set { SetValue (TextLengthProperty, value); }
		}

		public Int32 SelectionStart {
			get { return (Int32)GetValue (SelectionStartProperty); }
			set { SetValue (SelectionStartProperty, value); }
		}

		public Int32 SelectionEnd {
			get { return (Int32)GetValue (SelectionEndProperty); }
			set { SetValue (SelectionEndProperty, value); }
		}

		public string FormatCharacters {
			get { return (string)GetValue (FormatCharactersProperty); }
			set { SetValue (FormatCharactersProperty, value); }
		}

		public string LastText {
			get { return (string)GetValue (LastTextProperty); }
			set { SetValue (LastTextProperty, value); }
		}
	}
}

