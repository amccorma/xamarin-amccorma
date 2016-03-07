using System;

using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;

namespace DropDown.Forms
{
	public class DropDownPicker : Picker
	{
		/// <summary>
		/// remove global tap ID
		/// </summary>
		public const string RemoveTapMessage = "RemoveEvents";

		/// <summary>
		/// add global tap ID
		/// </summary>
		public const string AddTapMessage = "AddT";

		/// <summary>
		/// close dropdown
		/// </summary>
		public const string CloseDropMessage = "CloseD";

		/// <summary>
		/// event handler Forms -> Renderer
		/// </summary>
		public static event EventHandler<string> OnMessageTo;

		/// <summary>
		/// event handler Renderer -> Forms
		/// </summary>
		public static event EventHandler<DropDownTapArgs> OnTapFrom;

		/// <summary>
		/// event handler Renderer -> Forms (Selected change)
		/// </summary>
		public event EventHandler<string> OnSelected;

		public DropDownPicker ()
		{
			VerticalOptions = LayoutOptions.FillAndExpand;
			HorizontalOptions = LayoutOptions.StartAndExpand;
		}

		#region "iOS only"

		/// <summary>
		/// this removes the global Tap Handler.  Should be called on Disappear event or 
		/// when there are NO instances of this control on the Page!
		/// </summary>
		public static void RemoveEvents()
		{
			var handler = OnMessageTo;
			if (handler != null) {
				handler (null, RemoveTapMessage);
			}
		}


		public static void AddTapEvents()
		{			
			var handler = OnMessageTo;
			if (handler != null) {
				handler (null, AddTapMessage);
			}
		}

		public static void SendTapMessage(DropDownTapArgs args)
		{
			var handler = OnTapFrom;
			if (handler != null)
			{
				handler (null, args);
			}
		}


		/// <summary>
		/// handle tap received. if tap outside bounds of control, close the dropdown
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void DoHideDropDownOnTap(DropDownTapArgs args)
		{
			if (Device.OS == TargetPlatform.iOS) {
				System.Diagnostics.Debug.WriteLine ("Tap received");
				if (this.IsShowing) {
					var test = NativeFrame.Contains (new Point (args.X, args.Y));
					if (test == false) {
						this.CloseDropDown ();
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// iOS: closes the drop down
		/// Android: closes the drop down
		/// </summary>
		public void CloseDropDown()
		{
			var handler = OnMessageTo;
			if (handler != null) {
				handler (this, CloseDropMessage);
			}
		}

		public void FireSelectedChange()
		{
			var handler = OnSelected;
			if (handler != null)
			{
				handler (this, this.SelectedText);
			}
		}

		/// <summary>
		/// iOS only, no function in Android
		/// </summary>
		public static readonly BindableProperty FrameProperty =
			BindableProperty.Create ("Frame", typeof(Rectangle), typeof(DropDownPicker), new Rectangle(0,0,0,0));

		/// <summary>
		/// iOS only. To set Android Height, use FontSize
		/// </summary>
		public static readonly BindableProperty DropDownHeightProperty =
			BindableProperty.Create ("DropDownHeight", typeof(Int32), typeof(DropDownPicker), 100);


		/// <summary>
		/// SelectedText
		/// </summary>
		public static readonly BindableProperty SelectedTextProperty =
			BindableProperty.Create ("SelectedText", typeof(string), typeof(DropDownPicker), "");
		
		/// <summary>
		/// Font Size of DropDown
		/// </summary>
		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create ("FontSize", typeof(float), typeof(DropDownPicker), 0f);
		
		/// <summary>
		/// iOS only. UICell height
		/// </summary>
		public static readonly BindableProperty CellHeightProperty =
			BindableProperty.Create ("CellHeight", typeof(float), typeof(DropDownPicker), 40f);	

		
		/// <summary>
		/// Is the DropDown open
		/// </summary>
		public static readonly BindableProperty IsShowingProperty =
			BindableProperty.Create ("IsShowing", typeof(bool), typeof(DropDownPicker), false);

		/// <summary>
		/// selectedBackground Color of picker item selected
		/// </summary>
		public static readonly BindableProperty SelectedBackgroundColorProperty =
			BindableProperty.Create ("SelectedBackgroundColor", typeof(Xamarin.Forms.Color), typeof(DropDownPicker), Xamarin.Forms.Color.Transparent);


		/// <summary>
		/// SelectedText Color of picker item selected
		/// </summary>
		public static readonly BindableProperty SelectedTextColorProperty =
			BindableProperty.Create ("SelectedTextColor", typeof(Xamarin.Forms.Color), typeof(DropDownPicker), Xamarin.Forms.Color.Transparent);
		
		/// <summary>
		/// Item source property
		/// </summary>
		public static readonly BindableProperty SourceProperty =
			BindableProperty.Create ("Source", 
				typeof(IList<string>), 
				typeof(DropDownPicker), 
				null,
				propertyChanged: DropDownPicker.OnItemsSourceChanged);

		/// <summary>
		/// Border Color
		/// </summary>
		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create ("BorderColor", typeof(Xamarin.Forms.Color), typeof(DropDownPicker), Color.Black);

		/// <summary>
		/// Arrow Color, Android: Dropdown Arrow color
		/// </summary>
		public static readonly BindableProperty ArrowColorProperty =
			BindableProperty.Create ("ArrowColor", typeof(Xamarin.Forms.Color), typeof(DropDownPicker), Color.Black);
		
		public Xamarin.Forms.Color BorderColor
		{
			get { return (Xamarin.Forms.Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}

		public Xamarin.Forms.Color ArrowColor
		{
			get { return (Xamarin.Forms.Color)GetValue(ArrowColorProperty); }
			set { SetValue(ArrowColorProperty, value); }
		}

		public IList<string> Source
		{
			get { return (IList<string>)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		/// <summary>
		/// iOS: Native Control Frame
		/// </summary>
		/// <value>The native frame.</value>
		public Rectangle NativeFrame
		{
			get;set;
		}

		/// <summary>
		/// Is Dropdown? (open/closed)
		/// </summary>
		/// <value><c>true</c> if this instance is showing; otherwise, <c>false</c>.</value>
		public bool IsShowing 
		{
			get {
				return (bool)GetValue(IsShowingProperty); 
			}
			set {
				this.SetValue(IsShowingProperty, value);                
			}
		}

		/// <summary>
		/// DataBinding Method
		/// </summary>
		/// <param name="bindable">Bindable.</param>
		/// <param name="oldvalue">Oldvalue.</param>
		/// <param name="newvalue">Newvalue.</param>
		private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			try
			{
				var items = newValue as IList<string>;
				var picker = bindable as DropDownPicker;
				picker.Items.Clear();
				if (items != null)
				{
					foreach (var item in items)
					{
						picker.Items.Add (item.ToString());
					}
				}
			}
			catch(NullReferenceException) {
				// happens when the page is being disposed.
			}
		}

		protected override void OnSizeAllocated (double width, double height)
		{
			base.OnSizeAllocated (width, height);

			var p = this.Parent as Layout;
			var y = this.Y;

			while (p != null) {
				y = y + p.Y;
				p = p.Parent as Layout;
			}
			// update the position to the renderer
			this.Frame = new Rectangle (this.X, y, this.Width, this.Height);
		}

		protected override SizeRequest OnSizeRequest (double widthConstraint, double heightConstraint)
		{
			return base.OnSizeRequest (widthConstraint, heightConstraint);
		}

		public Xamarin.Forms.Color SelectedBackgroundColor
		{
			get {
				return (Xamarin.Forms.Color)GetValue(SelectedBackgroundColorProperty); 
			}
			set {
				this.SetValue(SelectedBackgroundColorProperty, value);                
			}
		}

		public Xamarin.Forms.Color SelectedTextColor
		{
			get {
				return (Xamarin.Forms.Color)GetValue(SelectedTextColorProperty); 
			}
			set {
				this.SetValue(SelectedTextColorProperty, value);                
			}
		}

		public float CellHeight
		{
			get {
				return (float)GetValue(CellHeightProperty); 
			}
			set {
				this.SetValue(CellHeightProperty, value);                
			}
		}

		public float FontSize
		{
			get {
				return (float)GetValue(FontSizeProperty); 
			}
			set {
				this.SetValue(FontSizeProperty, value);                
			}
		}

		public string SelectedText
		{
			get {
				return (string)GetValue(SelectedTextProperty); 
			}
			set {
				this.SetValue(SelectedTextProperty, value);                
			}
		}

		public Rectangle Frame
		{
			get {
				return (Rectangle)GetValue(FrameProperty); 
			}
			set {
				this.SetValue(FrameProperty, value);                
			}
		}

		public Int32 DropDownHeight
		{
			get {
				return (Int32)GetValue(DropDownHeightProperty); 
			}
			set {
				this.SetValue(DropDownHeightProperty, value);                
			}
		}

	}
}


