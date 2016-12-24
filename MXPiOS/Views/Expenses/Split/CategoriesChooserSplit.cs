using System;
using UIKit;
using Foundation;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class CategoriesChooserSplit : MXPTableViewController
	{

		private ExpenseItem ExpenseItem;

		public CategoriesChooserSplit (ExpenseItem expenseItem): base()
		{
			this.ExpenseItem = expenseItem;
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = "1. " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Category);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.configureTopBar();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.loadProducts ();
			this.NavigationItem.SetHidesBackButton (true, false);
		}

		public void configureTopBar(){

			UIBarButtonItem cancelButton = new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Plain, (sender, args) => {

				if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					if(this.NavigationController.ViewControllers.Length == 1) {
						((ExpenseItem)this.ExpenseItem.GetModelParent<ExpenseItem>()).ClearSplittedItems();
						this.DismissViewController(true, null);
					}
					this.NavigationController.PopViewController(true);
					return ;
				}

				if(!(this.NavigationController.ViewControllers [this.NavigationController.ViewControllers.Length - 2] is SplitTableViewController)){
					((ExpenseItem)this.ExpenseItem.GetModelParent<ExpenseItem>()).ClearSplittedItems();
				};
				this.NavigationController.PopViewController(true);

			}); 


			this.NavigationItem.SetLeftBarButtonItem (cancelButton, true);

		}

		private Products products;

		private async void loadProducts(){
			if (this.products == null) {
				LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Loading) + "...");

				try {
					this.products = await this.ExpenseItem.GetModelParent<ExpenseItem, ExpenseItem> ().FetchAvailableProductsAsync ();
				} catch (Exception e) {
					this.Close ();
					MainNavigationController.Instance.showError (e);
					return;
				} finally {
					LoadingView.hideMessage ();
				}
			}

			CategoriesSectionSource source = new CategoriesSectionSource (products);
			source.cellSelected += (sender, e) => {
				this.ExpenseItem.Product = e.Product;
				this.NavigationController.PushViewController(new ChooseAmoutSplitViewController(this.ExpenseItem), true);
			};
			this.TableView.Source = source;

			this.TableView.ReloadData ();
			if (this.ExpenseItem.Product != null) {
					this.TableView.SelectRow(NSIndexPath.FromRowSection(this.products.IndexOf(this.ExpenseItem.Product), 0), true, UITableViewScrollPosition.Middle); 
			}



		}
	}
}

