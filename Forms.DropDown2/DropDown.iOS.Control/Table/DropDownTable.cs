using UIKit;
using Foundation;
using System.Collections.Generic;
using CoreGraphics;
using CoreAnimation;
using System;

namespace DropDown.iOS.Control.Table
{
	public class DropDownTable : UITableView
	{
		public Action<string> SelectedText;

		private nfloat _FontSize;
		private nfloat _CellHeight;
		private UIColor _CellSBackgroundColor, _CellSTextColor;

		/// <summary>
		/// Initializes a new instance of UITableView.
		/// </summary>
		/// <param name="data">Source Data.</param>
		/// <param name="cellHeight">cellHeight, use 0 for default.</param>
		/// <param name="fSize">Cell Font size.</param>
		/// <param name="cellSelectedBackgroundColor">Cell selected background color, Use Clear for default</param>
		/// <param name="cellSelectedTextColor">Cell selected text color, Use Clear for default</param>
		public DropDownTable(IList<string> data, nfloat cellHeight,
			nfloat fSize, UIColor cellSelectedBackgroundColor, UIColor cellSelectedTextColor) : base()
		{
			CellLayoutMarginsFollowReadableWidth = false;
			this.MultipleTouchEnabled = true;
			this._FontSize = fSize;
			this._CellHeight = cellHeight;
			this._CellSBackgroundColor = cellSelectedBackgroundColor;
			this._CellSTextColor = cellSelectedTextColor;

			Source = new DropDownSource (data, this._FontSize, this._CellHeight, this._CellSBackgroundColor, this._CellSTextColor);
			//ContentInset =  new UIEdgeInsets(0, -10, 0, 0);
			LayoutMargins = UIEdgeInsets.Zero;
			SeparatorInset = UIEdgeInsets.Zero;

			(Source as DropDownSource).OnSelected += RowSelected;
		}

		/// <summary>
		/// reload the data
		/// </summary>
		/// <param name="data">Data.</param>
		public void ReloadSource(IList<string> data)
		{
			// events must be removed and reattached
			(Source as DropDownSource).OnSelected -= RowSelected;

			Source = new DropDownSource (data, this._FontSize, this._CellHeight, this._CellSBackgroundColor, this._CellSTextColor);

			(Source as DropDownSource).OnSelected += RowSelected;
			this.ReloadData ();
		}

		/// <summary>
		/// Has Data
		/// </summary>
		/// <returns><c>true</c> if this instance has data; otherwise, <c>false</c>.</returns>
		public bool HasData()
		{
			var d = Source as DropDownSource;
			if (d != null && d._SourceData != null) {
				return d._SourceData.Count > 0;
			}
			return false;
		}

		protected override void Dispose (bool disposing)
		{
			var obj = Source as DropDownSource;
			if (obj != null) {
				obj.OnSelected -= RowSelected;
				Source.Dispose ();
				Source = null;
			}
			base.Dispose (disposing);
		}

		/// <summary>
		/// Row Selected Action
		/// </summary>
		/// <param name="text">Text.</param>
		void RowSelected (string text)
		{
			if (SelectedText != null) {
				SelectedText (text);
			}
		}
	}
}