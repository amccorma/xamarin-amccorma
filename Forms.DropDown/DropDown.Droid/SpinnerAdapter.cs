using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;

namespace DropDown.Droid
{
	public class SpinnerHolder : Java.Lang.Object
	{
		public LinearLayout Layout { get; set; }
		public TextView Text { get; set; }
	}

	public class SpinnerAdapter : BaseAdapter<string>
	{
		private IList<string> _Items;
		private Activity _Context;
		private string _Title;
		private float _FontSize;
		private global::Android.Graphics.Color _SelectedBackColor, _SelectedTextColor;
		private bool _NoSelectedColor;

		public SpinnerAdapter (Activity context, IList<string> items, 
			string Title, float FSize, global::Android.Graphics.Color backcolor, global::Android.Graphics.Color selectedColor) : base ()
		{
			this._Items = items;
			this._Context = context;
			this._Title = Title;
			this._FontSize = FSize;
			this._SelectedBackColor = backcolor;
			this._SelectedTextColor = selectedColor;
			this._NoSelectedColor = false;
		}

		public SpinnerAdapter (Activity context, IList<string> items, 
			string Title, float FSize) : base ()
		{
			this._Items = items;
			this._Context = context;
			this._Title = Title;
			this._FontSize = FSize;
			this._NoSelectedColor = true;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override string this [int index] {
			get {
				return this._Items [index];
			}
		}

		public override int Count {
			get {
				return this._Items.Count;
			}
		}

		public string SelectedText { get; set; }

		public override global::Android.Views.View GetView (int position, global::Android.Views.View convertView, global::Android.Views.ViewGroup parent)
		{
			SpinnerHolder holder = null;
			var view = convertView;
			if (view != null) {
				holder = view.Tag as SpinnerHolder;
			}

			if (holder == null) {
				holder = new SpinnerHolder ();
				//view = this._Context.LayoutInflater.Inflate (Resource.Layout.SpinnerItemLayout, null);
				view = this._Context.LayoutInflater.Inflate(Resource.Layout.SpinnerItemLayout, null);
				holder.Layout = view.FindViewById<LinearLayout> (Resource.Id.SpinnerLayout);
				holder.Text = view.FindViewById<TextView> (Resource.Id.SpinnerTextView);
				holder.Text.TextSize = this._FontSize;
				view.Tag = holder;
			}

			var item = this._Items [position];
			if (position == 0) {
				holder.Text.Text = this._Title;
			} else {
				if (SelectedText == item && this._NoSelectedColor == false) {									
					holder.Layout.SetBackgroundDrawable (new RectBorder (3, 
						global::Android.Graphics.Color.Black, this._SelectedBackColor));
					holder.Text.SetTextColor (this._SelectedTextColor);
				} 
				holder.Text.Text = this._Items [position];

			}
				

			return view;
		}
	}
}

