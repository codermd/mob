using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using Mxp.Core.Business;
using Mxp.iOS;
using UIKit;

namespace MXPiOS
{
	public partial class ProductsViewController : UIViewController {
		public event EventHandler<CategoriesSectionSource.CategorySelectedEventArgs> cellSelected = delegate { };

		public bool ShowTravelProducts { get; private set; }

		private Product product;
		private Products products;

		public void setDataField (DataFieldCell dataField) {
			this.product = dataField.Field.GetValue<Product> ();
		}

		public ProductsViewController () : base ("ProductsViewController", null) {
			
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			//this.AutomaticallyAdjustsScrollViewInsets = true;
			this.EdgesForExtendedLayout = UIRectEdge.None;

			this.SegmentedControl.SelectedSegment = 0;
			this.ShowTravelProducts = true;
			this.SegmentedControl.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.TravelFilter), 0);
			this.SegmentedControl.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.NonTravelFilter), 1);

			if (Preferences.Instance.FilterTravelCategory) {
				this.SegmentedControl.ValueChanged += (sender, e) => {
					this.ShowTravelProducts = !this.ShowTravelProducts;
					this.SearchBar.Text = "";
					this.configureSource ();
				};
			} else {
				this.SegmentedControl.Hidden = true;
				this.SegmentHeight.Constant = 0;
				this.SegmentTopMargin.Constant = 0;
				this.SegmentBottomMargin.Constant = 0;
			}

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Categories);

			this.SearchBar.SearchButtonClicked += (object sender, EventArgs e) => this.searchWith (this.SearchBar.Text);
			this.SearchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => this.searchWith (this.SearchBar.Text);

			this.configureSource ();

			((CategoriesSectionSource)TableView.Source).cellSelected += ConfigureCellSelection;
			this.highlightSelectedCategory ();
		}

		public virtual void ConfigureCellSelection (object sender, CategoriesSectionSource.CategorySelectedEventArgs e) {
			if (this.cellSelected != null)
				this.cellSelected (this, e);
		}

		void configureSource () {
			if (!Preferences.Instance.FilterTravelCategory) {
				this.products = LoggedUser.Instance.Products.ExpenseProducts;
			} else {
				if (this.ShowTravelProducts) {
					this.products = LoggedUser.Instance.Products.TravelProduct;
				} else {
					this.products = LoggedUser.Instance.Products.NonTravelProduct;
				}
			}

			if (this.TableView.Source == null) {
				this.TableView.Source = new CategoriesSectionSource (this.products);
			} else {
				((CategoriesSectionSource)this.TableView.Source).SetProducts (this.products);
			}

			this.TableView.ReloadData ();
		}

		public void searchWith (string text) {
			((CategoriesSectionSource)this.TableView.Source).SetProducts (this.products.SearchWith (text), !String.IsNullOrEmpty (text));
			this.TableView.ReloadData ();
		}

		private void highlightSelectedCategory () {
			if (this.product == null)
				return;

			IGrouping<string, Product> selectedGroup = ((CategoriesSectionSource)this.TableView.Source).Products.GroupedProducts.LastOrDefault (grouping => grouping.Contains (this.product));
			if (selectedGroup != null) {
				NSIndexPath indexPath = NSIndexPath.FromRowSection (selectedGroup.ToList ().IndexOf (this.product), ((CategoriesSectionSource)this.TableView.Source).Products.GroupedProducts.IndexOf (selectedGroup));
				this.TableView.SelectRow (indexPath, false, UITableViewScrollPosition.Middle);
			}
		}
	}
}
