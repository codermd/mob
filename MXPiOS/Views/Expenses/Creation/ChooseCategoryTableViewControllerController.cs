
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using MXPiOS;

namespace Mxp.iOS
{
	public class ChooseCategoryTableViewControllerController : ProductsViewController
	{
		private Expense Expense;

		public ChooseCategoryTableViewControllerController () : base () {}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.Title = "1. "+ Labels.GetLoggedUserLabel (Labels.LabelEnum.Category);

			this.configureTopBar ();

			this.Expense = Expense.NewInstance ();

		}

		public override void ConfigureCellSelection(object sender, CategoriesSectionSource.CategorySelectedEventArgs e)
		{
			this.Expense.ExpenseItems[0].Product = e.Product;
			this.pushNextViewController();
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);
			this.NavigationItem.SetHidesBackButton (true, false);
		}

		public void configureTopBar () {
			UIBarButtonItem cancelButton = new UIBarButtonItem (Labels.GetLoggedUserLabel(Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Done, (action, args)=>this.popToRoot());
			this.NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[]{ cancelButton }, true);
		}

		public void popToRoot () {
			if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				this.DismissViewControllerAsync (true);
			else
				this.NavigationController.PopToRootViewController(true);
		}

		public void pushNextViewController () {
			ChooseCountryTableViewController vc = new ChooseCountryTableViewController();
			vc.Expense = this.Expense;
			this.NavigationController.PushViewController(vc, true);
		}
	}
}