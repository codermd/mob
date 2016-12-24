using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;
using Mxp.Core.Business;
using CoreGraphics;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.iOS
{
	partial class ReceiptsCollectionViewController : UICollectionViewController, IExpenseDetailsSubController
	{
		private Expense expense;

		private ReceiptsFlowLayout flowLayout;

		public ReceiptsCollectionViewController (IntPtr handle) : base (handle) {

		}
			
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.CollectionView.RegisterNibForCell (UINib.FromName ("ReceiptCell", null), new NSString("ReceiptCell"));
			this.CollectionView.RegisterNibForCell (UINib.FromName ("AddReceiptCell", null), new NSString("AddReceiptCell"));

			flowLayout = new ReceiptsFlowLayout ();
			flowLayout.viewController = this;
			flowLayout.collectionView = this.CollectionView;
			flowLayout.Receipts = this.expense.Receipts;

			this.CollectionView.Source = new ReceiptsCollectionSource (this.expense.Receipts);
		}

		public void setExpenseItem (ExpenseItem anExpenseItem) {
			this.expense = anExpenseItem.GetModelParent<ExpenseItem, Expense> ();
			this.Reload ();
		}

		async void ProcessImage(string base64) {
			if (!this.expense.IsNew)
				LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Uploading));

			try {
				await this.expense.Receipts.AddReceipt (base64);
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.CollectionView.ReloadData ();
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			flowLayout.SelectImage += FlowLayout_SelectImage;

			this.CollectionView.Delegate = flowLayout;

			this.CollectionView.ReloadData ();
		}

		public override void ViewWillDisappear (bool animated) {
			flowLayout.SelectImage -= FlowLayout_SelectImage;

			base.ViewWillDisappear (animated);
		}

		void FlowLayout_SelectImage (object sender, SelectImageEventArgs e) {
			this.ProcessImage (e.base64);
		}

		public async void Reload () {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Loading) + "...");

			try {
				await this.expense.Receipts.FetchAsync ();
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.CollectionView.ReloadData ();
		}

		public override bool CanBecomeFirstResponder {
			get {
				return true;
			}
		}
	}
}