using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Masked.Library;

namespace Masked.Controls
{
	public class MyEntry : Entry
	{

		public delegate void InValid(object sender, string prop, string message);
		public delegate void Valid(object sender, bool IsValid);
		public event InValid OnValidationError;
		public event Valid OnValid;
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
		/// The font name property.
		/// </summary>
		public static readonly BindableProperty FontNameProperty =
			BindableProperty.Create<MyEntry, string>(
				p => p.FontName, string.Empty);

		/// <summary>
		/// The is bold property.
		/// </summary>
		public static readonly BindableProperty IsBoldProperty =
			BindableProperty.Create<MyEntry, bool>(
				p => p.IsBold, false);

		/// <summary>
		/// The padding property.
		/// </summary>
		public static readonly BindableProperty PaddingProperty =
			BindableProperty.Create<MyEntry, Thickness>(
				p => p.Padding, new Thickness(-1, -1, -1, -1));

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
		/// Gets or sets the name of the font.
		/// </summary>
		/// <value>The name of the font.</value>
		public Thickness Padding
		{
			get { return (Thickness)GetValue(PaddingProperty); }
			set { SetValue(PaddingProperty, value); }
		}


		/// <summary>
		/// Gets or sets the name of the font.
		/// </summary>
		/// <value>The name of the font.</value>
		public bool IsBold
		{
			get { return (bool)GetValue(IsBoldProperty); }
			set { SetValue(IsBoldProperty, value); }
		}

		/// <summary>
		/// Gets or sets the Font
		/// </summary>
		public Font Font
		{
			get { return (Font)GetValue(FontProperty); }
			set { SetValue(FontProperty, value); }
		}

		/// <summary>
		/// Gets or sets the X alignment of the text
		/// </summary>
		public TextAlignment XAlign
		{
			get { return (TextAlignment)GetValue(XAlignProperty); }
			set { SetValue(XAlignProperty, value); }
		}

		/// <summary>
		/// Gets or sets if the border should be shown or not
		/// </summary>
		public bool HasBorder
		{
			get { return (bool)GetValue(HasBorderProperty); }
			set { SetValue(HasBorderProperty, value); }
		}

		/// <summary>
		/// Sets color for placeholder text
		/// </summary>
		public Color PlaceholderTextColor
		{
			get { return (Color)GetValue(PlaceholderTextColorProperty); }
			set { SetValue(PlaceholderTextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the name of the font.
		/// </summary>
		/// <value>The name of the font.</value>
		public string FontName
		{
			get { return (string)GetValue(FontNameProperty); }
			set { SetValue(FontNameProperty, value); }
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

		private Int32 MaxLengthFromMask = 0;


		public MyEntry()
		{
			Device.OnPlatform(
				() =>
				{
				},
				() =>
				{
					this.Padding = new Thickness(8, 0, 8, 0);
				},
				() =>
				{
				},
				() =>
				{
				});


			this.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
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
							adjustedStart = adjustedStart - 1;
							BeforeChars = this.Text.Substring(0, start - 1).Replace(this.FormatCharacters.ToCharArray(), "");
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
									BeforeChars = this.Text.Substring(0, start + 1).Replace(this.FormatCharacters.ToCharArray(), "");
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

						var rule = this.Mask.FirstOrDefault(r => r.End >= len);
						if (rule == null)
						{
							var temp = text.Substring(0, text.Length - 1);
							this.Text = temp;
							//IOS
							//this.LastText = temp;
							//native.SetSelection(native.Text.Length);
						}
						else
						{
							if (rule.Mask != "")
							{
								var temp = ReFractor(text, rule);
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

						Device.OnPlatform(() =>
							{
							},
							() =>
							{
								this.Locked = false;
							});
						this.SetSelection = new SelectionPoint(start + adjustedStart);
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

		public string ReFractor(string text, MaskRules rule)
		{
			string temp = "";
			if (rule.Mask != "")
			{
				if (text.Length > this.MaxLengthFromMask)
				{
					text.Substring (0, this.MaxLengthFromMask);
				}
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


	}
}

