using System;
using UIKit;
using Mxp.Core.Business;
using CoreGraphics;

namespace Mxp.iOS
{
	public class ChooseCountryTableViewController : MXPTableViewController
	{
		private CountriesSectionSource CountriesSectionSource;

		public Expense Expense { get; set;}

		public ChooseCountryTableViewController () : base (UITableViewStyle.Plain) {
			
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();
			this.Title = "2. "+ Labels.GetLoggedUserLabel (Labels.LabelEnum.Country);

			UISearchBar sb = new UISearchBar(CGRect.FromLTRB(0,0,320,44));

			this.TableView.TableHeaderView = sb;

			sb.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => {
				this.searchWith(sb.Text);
			};

			sb.SearchButtonClicked += (object sender, EventArgs e) => {
				this.searchWith(sb.Text);
			};
		}

		public void searchWith(String text){
			this.CountriesSectionSource.SetCountries (this.Expense.Countries.SearchWith (text), !String.IsNullOrEmpty (text));
			this.TableView.ReloadData ();
		}

		public void configureTopBar(){
			UIBarButtonItem cancelButton = new UIBarButtonItem (Labels.GetLoggedUserLabel(Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Done, (action, args)=>this.popToRoot());
			this.NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[]{ cancelButton }, true);
		}

		public void popToRoot(){
			this.NavigationController.PopToRootViewController(true);
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);
			this.configureTopBar ();
			this.CountriesSectionSource = new CountriesSectionSource (this.Expense.Countries);

			this.TableView.Source = this.CountriesSectionSource;
			this.CountriesSectionSource.cellSelected += (object sender, Mxp.iOS.CountriesSectionSource.CountrySelectedEventArgs e) => {
				Expense.ExpenseItems[0].Country = e.Country;
				this.showAmount();
			};
		}

		public void showAmount() {
			ChooseAmoutViewController vc =new ChooseAmoutViewController(this.Expense.ExpenseItems[0]);
			this.NavigationController.PushViewController (vc, true);
		}
	}
}