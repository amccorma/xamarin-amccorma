using System.ComponentModel;
using Android.Support.V7.Widget;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;
using Android.Runtime;
using Android.App;
using System;
using DropDown.Forms;

[assembly: ExportRenderer(typeof(DropDownPicker), typeof(DropDown.Droid.MaterialDropDownRenderer))]
namespace DropDown.Droid
{
	public class MaterialDropDownRenderer : ViewRenderer<DropDownPicker, LinearLayout>, AdapterView.IOnItemSelectedListener
    {
		private MyAppCompatSpinner _SpinnerControl;

		protected SpinnerAdapter _Adapter { get; private set; }

		protected override void Dispose (bool disposing)
		{
			if (this._Adapter != null) {
				this._Adapter.Dispose ();
			}

			DropDownPicker.OnMessageTo -= AddMessageTO;
			this._SpinnerControl.LayoutChange -= SpinnerLayoutChange;
			this._SpinnerControl.Dispose ();
			this._SpinnerControl = null;

			base.Dispose (disposing);
		}

        public void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            if (this.Element.SelectedIndex != position && position != 0)
            {
				var text = this.Element.Source [position];
				((IElementController)this.Element).SetValueFromRenderer(DropDownPicker.SelectedTextProperty, text);
				this._Adapter.SelectedText = text;
				Element.FireSelectedChange ();
            }
			else if (position == 0)
			{
				var text = "";
				if (String.IsNullOrEmpty (this.Element.Title)) {
					text = this.Element.Source [0];
					this._Adapter.SelectedText = text;
				} else {
					this._Adapter.SelectedText = "";
				}
				((IElementController)this.Element).SetValueFromRenderer(DropDownPicker.SelectedTextProperty, text);
				Element.FireSelectedChange ();
			}
        }

		private void AddMessageTO(object sender, string msg) 
		{
			if (msg == DropDownPicker.CloseDropMessage) {
				var o = sender as DropDownPicker;
				if (this.Control != null && o != null && this.Element != null)
				{
					if (o.Id == this.Element.Id)
					{
						this.Control.PerformClick ();
					}
				}
			}
		}

		public void OnNothingSelected (AdapterView parent)
		{
			// not sure what this is? never fires
		}

		protected override void OnElementChanged(ElementChangedEventArgs<DropDownPicker> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
				var inflater = (global::Android.Views.LayoutInflater)Xamarin.Forms.Forms.Context.GetSystemService (global::Android.App.Service.LayoutInflaterService);
				var layout = inflater.Inflate (Resource.Layout.spinner, null).JavaCast<LinearLayout> ();

				this._SpinnerControl = layout.FindViewById<MyAppCompatSpinner> (Resource.Id.spinner2);
				this._SpinnerControl.FormsElement = this.Element;
				this._SpinnerControl.OnItemSelectedListener = this;

				this._SpinnerControl.LayoutChange += SpinnerLayoutChange;

				SetAdapter ();
				this._Adapter.SelectedText = Element.SelectedText;

				this._SpinnerControl.Adapter = this._Adapter;
				this._SpinnerControl.Clickable = true;
				this._SpinnerControl.OnItemSelectedListener = this;

				this.SetNativeControl(layout);

				DropDownPicker.OnMessageTo += AddMessageTO;
            }
        }

		private void SpinnerLayoutChange(object sender, LayoutChangeEventArgs e2)
		{
			var obj = sender as MyAppCompatSpinner;
			if (obj != null) {
				obj.DropDownVerticalOffset = obj.Height;
			}
		}

		private void SetAdapter()
		{
			if (this._Adapter != null) {
				this._Adapter.Dispose ();
			}

			if (this.Element.SelectedTextColor == Xamarin.Forms.Color.Transparent) {
				this._Adapter = new SpinnerAdapter (this.Context as Activity, 
					this.Element.Items, 
					this.Element.Title, 
					this.Element.FontSize);
			} else {
				this._Adapter = new SpinnerAdapter (this.Context as Activity, 
					this.Element.Items, 
					this.Element.Title, 
					this.Element.FontSize,
					this.Element.SelectedBackgroundColor.ToAndroid(),
					this.Element.SelectedTextColor.ToAndroid());
			}
		}

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == DropDownPicker.SourceProperty.PropertyName) {
				SetAdapter ();
				this._Adapter.SelectedText = Element.SelectedText;
				this._SpinnerControl.Adapter = this._Adapter;
			}
		}
    }
}