using System;
using System.Linq;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreAnimation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.CoreGraphics;
using System.Collections.Generic;
using MonoTouch.Foundation;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Masked.Controls;
using Masked.Library;

[assembly: ExportRenderer(typeof(MyEntry), typeof(Masked.IOS.Controls.MyEntryRenderer))]
namespace Masked.IOS.Controls
{
    public class MyEntryRenderer : EntryRenderer
    {
        private MyEntry source;
        private UITextField native;
        private List<MaskRules> rules;
        private SelectionPoint pt;
        private char[] FormatCharacters = null;
        public UIEdgeInsets EdgeInseys { get; set; }
        public bool isSecond = false;
        public bool ifIsInside = false;
        public int lastPos = 0;
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            //var native = (UITextField)this.Control;

            if (native == null)
            {
                source = e.NewElement as MyEntry;
                var control = (MyEntry)this.Element;
                native = this.Control as UITextField;


                // INIT defaults
                rules = source.Mask;
                FormatCharacters = source.FormatCharacters.ToCharArray();

                // Events
                //if (FormatCharacters != null && String.IsNullOrEmpty(source.Text) == false) {
                //	ApplyDefaultRule ();
                //}	

                SetFont(control);
                SetTextAlignment(control);
                SetBorder(control);
                SetPlaceholderTextColor(control);

                ResizeHeight();

                native.ShouldChangeCharacters += new UITextFieldChange(delegate(UITextField textField, NSRange range, string replacementString)
                {
                    native.BecomeFirstResponder();
                    if (source.MaxLength > 0)
                    {
                        var newlength = source.Text.Length + replacementString.Length - range.Length;
                        return newlength <= source.MaxLength;
                    }

                    if (source.Locked)
                    {
                        return false;
                    }
                    if (source.Locked == false && source.Mask != null)
                    {
                        source.Delete = false;
                        source.SelectionStart = textField.GetOffsetFromPosition(textField.BeginningOfDocument, textField.SelectedTextRange.Start);
                        source.SelectionEnd = textField.GetOffsetFromPosition(textField.BeginningOfDocument, textField.SelectedTextRange.End);
                        source.TextLength = native.Text.Length;
                    }
                    if (source.Mask != null)
                    {
                        source.Delete = false;
                        if (replacementString == "")
                        {
                            source.Delete = true;
                        }
                        source.SelectionStart = textField.GetOffsetFromPosition(textField.BeginningOfDocument, textField.SelectedTextRange.Start);
                        source.SelectionEnd = textField.GetOffsetFromPosition(textField.BeginningOfDocument, textField.SelectedTextRange.End);
                        source.TextLength = native.Text.Length;
                    }

                    return true;
                });



                //SetNativeControl(native);
            }
        }




        private void ApplyDefaultRule()
        {
            source.Locked = true;
            var text = native.Text.Replace(FormatCharacters.ToString(), "");
            var len = text.Length;

            if (rules != null)
            {
                var rule = rules.FirstOrDefault(r => r.End >= len);
                if (rule == null)
                {
                    rule = rules.Find(r => r.End == rules.Max(m => m.End));
                    text = text.Substring(0, rule.End);
                }

                // text trimmed
                if (rule.Mask != "")
                {
                    native.Text = native.Text = source.ReFractor(text, rule);
                }
            }
            else if (source.MaxLength > -1)
            {
                if (len > source.MaxLength)
                {
                    native.Text = native.Text.Substring(0, source.MaxLength);
                }
                else
                {
                    native.Text = text;
                }
            }
            else
            {
                native.Text = text;
            }
            source.Locked = false;
        }


        //protected void EditingDidBegin(object sender, EventArgs e) 
        //{ 
        //		var x=0;
        //}


        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            //if (source.Delete && isSecond==false ){
            //	pt = source.SetSelection;
            //	isSecond=true;
            //	lastPos = pt.Start;
            //} 
            var view = (MyEntry)Element;
            var PN = e.PropertyName;
            if (PN == "SetSelection")
            {
                pt = source.SetSelection;
                if (pt != null && FormatCharacters != null)
                {
                    if (pt.Start != -1)
                    {
                        if (pt.End != -1)
                        {
                            var positionToSet = native.GetPosition(native.BeginningOfDocument, pt.Start);
                            native.SelectedTextRange = native.GetTextRange(positionToSet, positionToSet);
                            //native.SetSelection (pt.Start, pt.End);
                        }
                        else
                        {
                            if (pt.Start >= native.Text.Length)
                            {
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
                            if (source.Locked && source.Delete)
                            {
                                if (source.SelectionStart - 1 < native.Text.Length)
                                {
                                    lastPos = source.SelectionStart;
                                    ifIsInside = true;


                                }

                            }
                            else if (source.Locked && !source.Delete)
                            {
                                if (source.SelectionStart < native.Text.Length - 1)
                                {
                                    ifIsInside = true;
                                }
                                else
                                {
                                    ifIsInside = false;
                                }

                            }
                            else
                            {

                            }
                            //native.SetSelection(pt.Start);
                        }
                    }
                    pt = null;
                    //source.Delete = false;
                    source.Locked = false;
                }
            }
            else if (e.PropertyName == "FormatCharacters")
            {
                this.FormatCharacters = source.FormatCharacters.ToCharArray();
            }
            else if (e.PropertyName == "Text")
            {
                pt = source.SetSelection;
                if (source.Delete)
                {
                    //if (isSecond) {
                    if (ifIsInside)
                    {
                        //if (lastPos < source.SelectionStart) {
                        var positionToSet = native.GetPosition(native.BeginningOfDocument, source.SelectionStart - 1);
                        native.SelectedTextRange = native.GetTextRange(positionToSet, positionToSet);
                        //}
                        //isSecond = false;
                    }
                }
                else
                {
                    if (ifIsInside)
                    {
                        var positionToSet = native.GetPosition(native.BeginningOfDocument, source.SelectionStart + 1);
                        native.SelectedTextRange = native.GetTextRange(positionToSet, positionToSet);
                    }
                }
            }
            else if (e.PropertyName == MyEntry.FontProperty.PropertyName)
                SetFont(view);
            else if (e.PropertyName == MyEntry.XAlignProperty.PropertyName)
                SetTextAlignment(view);
            else if (e.PropertyName == MyEntry.HasBorderProperty.PropertyName)
                SetBorder(view);
            else if (e.PropertyName == MyEntry.PlaceholderTextColorProperty.PropertyName)
                SetPlaceholderTextColor(view);

            ResizeHeight();
        }

        void SetPlaceholderTextColor(MyEntry view)
        {
            /*
UIColor *color = [UIColor lightTextColor];
YOURTEXTFIELD.attributedPlaceholder = [[NSAttributedString alloc] initWithString:@"PlaceHolder Text" attributes:@{NSForegroundColorAttributeName: color}];
            */
            if (string.IsNullOrEmpty(view.Placeholder) == false && view.PlaceholderTextColor != Xamarin.Forms.Color.Default)
            {
                NSAttributedString placeholderString = new NSAttributedString(view.Placeholder, new UIStringAttributes() { ForegroundColor = view.PlaceholderTextColor.ToUIColor() });
                Control.AttributedPlaceholder = placeholderString;
            }
        }

        private void SetTextAlignment(MyEntry view)
        {
            switch (view.XAlign)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                case TextAlignment.Start:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }

        private void SetFont(MyEntry view)
        {
            UIFont uiFont;
            if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
                Control.Font = uiFont;
            else if (view.Font == Font.Default)
                Control.Font = UIFont.SystemFontOfSize(17f);
        }


        private void SetBorder(MyEntry view)
        {
            Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
        }

        private void ResizeHeight()
        {
            if (Element.HeightRequest >= 0) return;

            var height = Math.Max(Bounds.Height,
                new UITextField { Font = Control.Font }.IntrinsicContentSize.Height);

            Control.Frame = new System.Drawing.RectangleF(0.0f, 0.0f, (float)Element.Width, height);

            Element.HeightRequest = height;
        }
    }

}