using System;
using Android.Support.V7.Widget;
using Android.Content;
using Android.Util;
using DropDown.Forms;

namespace DropDown.Droid
{
	public class MyAppCompatSpinner : AppCompatSpinner
	{
		private bool mOpenInitiated = false;
		private WeakReference _FormsElement;

		/// <summary>
		/// Forms Element
		/// </summary>
		/// <value>The element.</value>
		public DropDownPicker FormsElement
		{
			get { return this._FormsElement.Target as DropDownPicker; }
			set {
				this._FormsElement = new WeakReference (value);
			}
		}

		public MyAppCompatSpinner(Context context, IAttributeSet attrs, int defStyleAttr, int mode) :
			base(context, attrs, defStyleAttr, mode)
		{
		}

		public MyAppCompatSpinner(Context context, IAttributeSet attrs, int defStyleAttr) :
			base(context, attrs, defStyleAttr)
		{
		}

		public MyAppCompatSpinner(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
		}

		public MyAppCompatSpinner(Context context, int mode) :
			base(context, mode)
		{
		}

		public MyAppCompatSpinner(Context context) :
			base(context)
		{
			
		}

		public override bool PerformClick ()
		{
			this.FormsElement.IsShowing = true;
			return base.PerformClick ();
		}

		public override void OnWindowFocusChanged (bool hasWindowFocus)
		{
			base.OnWindowFocusChanged (hasWindowFocus);
			if (mOpenInitiated && this.HasWindowFocus) {
				this.FormsElement.IsShowing = false;
			}
		}
	}
}

