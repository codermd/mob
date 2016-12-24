using System;
using UIKit;
using Mxp.Core.Business;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using System.Linq;
using Mxp.Core.Utils;

namespace Mxp.iOS
{
	public class CategoriesSectionSource : UITableViewSource
	{
		public class CategorySelectedEventArgs : EventArgs
		{
			public Product Product { get; }

			public CategorySelectedEventArgs (Product product) {
				this.Product = product;
			}
		}


		public event EventHandler<CategorySelectedEventArgs> cellSelected = delegate {};

		public Products Products { get; private set; }
		private bool whileSearching;

		public void SetProducts (Products products, bool whileSearching = false) {
			this.Products = products;
			this.whileSearching = whileSearching;

			this.Products.ResetGroups ();
		}

		private Dictionary<string, nfloat> heightForString = new Dictionary<string, nfloat>();
		private DefaultCell ghostCell = DefaultCell.Create();

		public CategoriesSectionSource (Products products) {
			this.Products = products;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{

			Product prod = this.Products.GetGroupedProducts (this.whileSearching) [indexPath.Section].ElementAt (indexPath.Row);
			string  valueStr = prod.ExpenseCategory.Name;
			int length = valueStr.Length;
			for(int i = 0; i < length ; i++) {
				valueStr += '-';
			}

			if (!heightForString.ContainsKey(valueStr)) {
				ghostCell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
				ghostCell.TextLabel.Lines = 0;
				CGSize maxHeight = new CGSize(tableView.Frame.Size.Width, float.MaxValue);
				NSString nsstr = (new NSString (valueStr));
				var size = nsstr.StringSize (ghostCell.TextLabel.Font, maxHeight, ghostCell.TextLabel.LineBreakMode);
				heightForString [valueStr] = size.Height + (ghostCell.TextLabel.Frame.Y*2);
			}

			return heightForString [valueStr];
		}

		public override nint NumberOfSections (UITableView tableView) {
			return this.Products.GetGroupedProducts (this.whileSearching).Count;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			return this.Products.GetGroupedProducts (this.whileSearching) [(int)section].Count ();
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			DefaultCell cell = (DefaultCell)tableView.DequeueReusableCell (DefaultCell.Key);

			if (cell == null)
				cell = DefaultCell.Create ();

			Product prod = this.Products.GetGroupedProducts (this.whileSearching) [indexPath.Section].ElementAt (indexPath.Row);

			cell.TextLabel.Text = prod.ExpenseCategory.Name;
			cell.TextLabel.AdjustsFontSizeToFitWidth = false;
			cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
			cell.TextLabel.Lines = 0;

			return cell;
		}

		public override string TitleForHeader (UITableView tableView, nint section) {
			return this.Products.GetGroupedProducts (this.whileSearching) [(int)section].Key;
		}

		public override String[] SectionIndexTitles (UITableView tableView) {
			return this.Products.GetGroupedProducts (this.whileSearching).Select (grouping => grouping.Key.Substring (0, 1)).ToArray ();
		}

		public override nint SectionFor (UITableView tableView, string title, nint atIndex) {
			return atIndex;
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath) {
			if (this.cellSelected == null)
				return;

			this.cellSelected (this, new CategorySelectedEventArgs (this.Products.GetGroupedProducts (this.whileSearching) [indexPath.Section].ElementAt (indexPath.Row)));
		}
	}
}