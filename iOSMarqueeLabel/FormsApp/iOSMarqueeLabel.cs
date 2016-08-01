using System;
using Xamarin.Forms;

namespace FormsApp
{
	public class iOSMarqueeLabel : Xamarin.Forms.Label
	{
		public iOSMarqueeLabel ()
		{
			this.ScrollType = MarqueeType.Continuous;
		}

		public enum MarqueeType
		{
			LeftRight = 0,
			RightLeft,
			Continuous,
			ContinuousReverse
		}

		public MarqueeType ScrollType { get; set; }

		/// <summary>
		/// The placeholder property.
		/// </summary>
		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create ("Placeholder", typeof(string), typeof(iOSMarqueeLabel), default(string));

		/// <summary>
		/// Stop Scrolling property.
		/// </summary>
		public static readonly BindableProperty HoldScrollingProperty =
			BindableProperty.Create ("HoldScrolling", typeof(bool), typeof(iOSMarqueeLabel), false);
		

		/// <summary>
		/// Trailing Buffer (end spaces) property.
		/// </summary>
		public static readonly BindableProperty TrailingBufferProperty =
			BindableProperty.Create ("TrailingBuffer", typeof(float), typeof(iOSMarqueeLabel), 100f);

		/// <summary>
		/// Word Wrap
		/// </summary>
		public static readonly BindableProperty NoWrapProperty =
			BindableProperty.Create ("NoWrap", typeof(bool), typeof(iOSMarqueeLabel), true);

		/// <summary>
		/// Scroll Rate
		/// </summary>
		public static readonly BindableProperty RateProperty =
			BindableProperty.Create ("Rate", typeof(float), typeof(iOSMarqueeLabel), 50f);
		
		/// <summary>
		/// The is underlined property.
		/// </summary>
		public static readonly BindableProperty IsUnderlineProperty =
			BindableProperty.Create ("IsUnderline", typeof(bool), typeof(iOSMarqueeLabel), false);

		/// <summary>
		/// The is underlined property.
		/// </summary>
		public static readonly BindableProperty IsStrikeThroughProperty =
			BindableProperty.Create ("IsStrikeThrough", typeof(bool), typeof(iOSMarqueeLabel), false);
		
		/// <summary>
		/// The formatted placeholder property.
		/// </summary>
		public static readonly BindableProperty FormattedPlaceholderProperty =
			BindableProperty.Create ("FormattedPlaceholder", typeof(FormattedString), typeof(iOSMarqueeLabel), default(FormattedString));		

		public static readonly BindableProperty NavigateUriProperty =
			BindableProperty.Create ("NavigateUri", typeof(string), typeof(iOSMarqueeLabel), string.Empty, BindingMode.OneWay);

		public string NavigateUri
		{
			get { return (string)base.GetValue(NavigateUriProperty); }
			set { base.SetValue(NavigateUriProperty, value); }
		}


		/// <summary>
		/// Word Wrap
		/// </summary>
		public bool NoWrap
		{
			get
			{
				return (bool)GetValue(NoWrapProperty);
			}
			set
			{
				SetValue(NoWrapProperty, value);
			}
		}

		/// <summary>
		/// Stop Scrolling
		/// </summary>
		public bool HoldScrolling
		{
			get
			{
				return (bool)GetValue(HoldScrollingProperty);
			}
			set
			{
				SetValue(HoldScrollingProperty, value);
			}
		}

		/// <summary>
		/// Trailing Buffer between scrolls (spaces)
		/// </summary>
		public float TrailingBuffer
		{
			get
			{
				return (float)GetValue(TrailingBufferProperty);
			}
			set
			{
				SetValue(TrailingBufferProperty, value);
			}
		}

		/// <summary>
		/// Scroll Rate
		/// </summary>
		public float Rate
		{
			get
			{
				return (float)GetValue(RateProperty);
			}
			set
			{
				SetValue(IsUnderlineProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the text in the label is underlined.
		/// </summary>
		/// <value>A <see cref="bool"/> indicating if the text in the label should be underlined.</value>
		public bool IsUnderline
		{
			get
			{
				return (bool)GetValue(IsUnderlineProperty);
			}
			set
			{
				SetValue(IsUnderlineProperty, value);
			}
		}


		/// <summary>
		/// Gets or sets a value indicating whether the text in the label is underlined.
		/// </summary>
		/// <value>A <see cref="bool"/> indicating if the text in the label should be underlined.</value>
		public bool IsStrikeThrough
		{
			get
			{
				return (bool)GetValue(IsStrikeThroughProperty);
			}
			set
			{
				SetValue(IsStrikeThroughProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the string value that is used when the label's Text property is empty.
		/// </summary>
		/// <value>The placeholder string.</value>
		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set 
			{
				SetValue(FormattedPlaceholderProperty, null);
				SetValue(PlaceholderProperty, value); 
			}
		}

		/// <summary>
		/// Gets or sets the FormattedString value that is used when the label's Text property is empty.
		/// </summary>
		/// <value>The placeholder FormattedString.</value>
		public FormattedString FormattedPlaceholder
		{
			get { return (FormattedString)GetValue(FormattedPlaceholderProperty); }
			set 
			{  
				SetValue(FormattedPlaceholderProperty, null);
				SetValue(FormattedPlaceholderProperty, value);
			}
		}
	}
}

