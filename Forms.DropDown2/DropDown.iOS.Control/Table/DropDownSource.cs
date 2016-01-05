using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace DropDown.iOS.Control.Table
{
	public class DropDownSource : UITableViewSource
	{	
		public delegate void Selected(string text);
		public event Selected OnSelected;

		/// <summary>
		/// data
		/// </summary>
		protected internal IList<string> _SourceData;

		/// <summary>
		/// Font Size
		/// </summary>
		private nfloat _FontSize;

		/// <summary>
		/// cell Height
		/// </summary>
		private nfloat _CellHeight;

		/// <summary>
		/// cell selected backgroundColor
		/// </summary>
		private UIColor _CellSBackgroundColor;

		/// <summary>
		/// cell selected color
		/// </summary>
		private UIColor _CellSTextColor;

		/// <summary>
		/// Default TableSource. Data: *SET* fontSize: *SET*, cellHeight: *SET*, selectedColors: *SET*
		/// </summary>
		/// <param name="data">TableSource Data.</param>
		/// <param name="fSize">Cell Font size.</param>
		/// <param name="cellHeight">Cell height.</param>
		/// <param name="cellSelectedBackgroundColor">Cell selected background color.</param>
		/// <param name="cellSelectedTextColor">Cell selected text color.</param>
		public DropDownSource(IList<string> data, nfloat fSize, nfloat cellHeight, 
			UIColor cellSelectedBackgroundColor, UIColor cellSelectedTextColor, string selectedText = "")
		{
			this.SelectedText = selectedText;
			this._SourceData = data;
			this._FontSize = fSize;
			this._CellHeight = cellHeight;
			this._CellSBackgroundColor = cellSelectedBackgroundColor;
			this._CellSTextColor = cellSelectedTextColor;
		}

		/// <summary>
		/// Default TableSource. Data: *SET* fontSize: *SET*, cellHeight: default, selectedColors: default
		/// </summary>
		/// <param name="data">TableSource Data.</param>
		/// <param name="fSize">Cell Font size.</param>
		public DropDownSource(IList<string> data, nfloat fSize) : this(data, fSize, 40, null, null)
		{
		}

		/// <summary>
		/// Default TableSource. Data: *SET*, fontSize: default, cellHeight: default, selectedColors: default
		/// </summary>
		/// <param name="data">Data.</param>
		public DropDownSource (IList<string> data) : this (data, 0, 40, null, null)
		{
		}


		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (this._SourceData == null)
				return 0;
			
			return this._SourceData.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("cell1") as DropDownCell;
			if (cell == null) {
				cell = new DropDownCell (UITableViewCellStyle.Default, "cell1");
				cell.Frame = new CoreGraphics.CGRect (cell.Frame.X, cell.Frame.Y, cell.Frame.Width, cell.Frame.Height);
			}

			// selection color
			if (this._CellSBackgroundColor != UIColor.Clear) {
				cell.SelectionStyle = UITableViewCellSelectionStyle.Default;
				UIView bgColorView = new UIView ();
				bgColorView.BackgroundColor = this._CellSBackgroundColor;
				cell.SelectedBackgroundView = bgColorView;
				cell.TextLabel.HighlightedTextColor = this._CellSTextColor;
			}
			// add text
			cell.TextLabel.Text = this._SourceData [indexPath.Row];
			if (this._FontSize > 1) {
				cell.TextLabel.Font = UIFont.SystemFontOfSize (this._FontSize);
			}
			return cell;
		}

		public string SelectedText { get; set; }

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			SelectedText = this._SourceData [indexPath.Row];
			var handler = this.OnSelected;
			if (handler != null) {
				handler (SelectedText);
			}
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return this._CellHeight;
		}
	}
}
