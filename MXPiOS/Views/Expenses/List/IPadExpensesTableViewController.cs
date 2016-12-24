using System;
using Mxp.Core.Business;
using UIKit;
using Mxp.Core;
using MXPiOS;

namespace Mxp.iOS
{
	[Foundation.Register ("IPadExpensesTableViewController")]
	partial class IPadExpensesTableViewController : ExpensesTableViewController
	{
		public class ExpenseSelectedEventArgs : EventArgs
		{
			public Model Model { get; set;}
		}

		public event EventHandler<ExpenseSelectedEventArgs> cellSelected = delegate {};

		public IPadExpensesTableViewController (IntPtr handle) : base (handle) {
			
		}

		public override void ShowModel (Model model, bool animated = true) {
			this.SetSelectedExpense (model);

			ExpenseSelectedEventArgs e = new ExpenseSelectedEventArgs ();
			e.Model = model;

			this.cellSelected (this, e);
		}

		public override void ShowCreateViewController (UIViewController vc) {
			UINavigationController nvc = new UINavigationController (vc);
			nvc.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
			this.PresentViewController (nvc, true, null);
		}
	}
}