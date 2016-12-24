using System;
using UIKit;
using Mxp.Core.Business;
using Foundation;

namespace Mxp.iOS
{
	public class ReceiptsCollectionSource : UICollectionViewSource
	{


		// Starting off with an empty handler avoids pesky null checks
		public event EventHandler addSelected = delegate {};


		private Receipts Receipts;
		private Expense expense { 
			get {
				return this.Receipts.GetParentModel<Model> () as Expense;
			}
		}

		private Report report { 
			get {
				return this.Receipts.GetParentModel<Model> () as Report;
			}
		}

		public ReceiptsCollectionSource (Receipts receipts)
		{
			this.Receipts = receipts;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (this.Receipts.CanAdd && indexPath.Row == 0) {

				AddReceiptCell addCell = (AddReceiptCell) collectionView.DequeueReusableCell (AddReceiptCell.Key, indexPath);

				if (addCell == null) {
					addCell = AddReceiptCell.Create ();
				}

				if (this.Receipts.GetParentModel<Model> () is Expense) {
					addCell.setExpense (this.Receipts.GetParentModel<Expense> ());
				}

				return addCell;
			}

			ReceiptCell cell = (ReceiptCell) collectionView.DequeueReusableCell (new NSString("ReceiptCell"), indexPath);

			if (cell == null) {
				cell = ReceiptCell.Create ();
			}

			int cellIndex = this.Receipts.CanAdd ? indexPath.Row - 1 : indexPath.Row;
			cell.setReceipt(this.Receipts [cellIndex]);

			return cell;
		}

		public override nint NumberOfSections (UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			if (!this.Receipts.CanAdd) {
				return this.Receipts.Count;	
			}
			return 1 + this.Receipts.Count;
		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (this.Receipts.CanAdd && indexPath.Row == 0) {
				this.addSelected (this, null);
			}
		}

		public override bool ShouldShowMenu (UICollectionView collectionView, NSIndexPath indexPath)
		{
			return true;
		}

		public override bool CanPerformAction (UICollectionView collectionView, ObjCRuntime.Selector action, NSIndexPath indexPath, NSObject sender)
		{
			return true;
		}

		public override void PerformAction (UICollectionView collectionView, ObjCRuntime.Selector action, NSIndexPath indexPath, NSObject sender)
		{
			int i;
		}

	}
}