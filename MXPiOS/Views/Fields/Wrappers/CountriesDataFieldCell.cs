using System;
using Mxp.Core.Business;
using UIKit;

namespace Mxp.iOS
{
	public class CountriesDataFieldCell : DataFieldCell
	{
		public CountriesDataFieldCell (Field field): base(field)
		{
			if (field.IsEditable) {
				this.accessory = UITableViewCellAccessory.DisclosureIndicator;
			} else {
				this.accessory = UITableViewCellAccessory.None;
			}
		}

		public override void FieldSelected (UIViewController viewController,UITableView tableview, UITableViewCell cell)
		{
			if (!this.Field.IsEditable) {
				return;
			}

			CountriesTableViewController vc = new CountriesTableViewController (((ICountriesFor)this.Field.Model).Countries);
			vc.setDataField(this);
			vc.cellSelected += (object sender, CountriesSectionSource.CountrySelectedEventArgs e) => {
				this.Field.Value = e.Country;
			};
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				UIPopoverController popover = new UIPopoverController (vc);


				popover.PresentFromRect (cell.ConvertRectToView (cell.Subviews [0].Subviews [1].Frame, viewController.View), viewController.View, UIPopoverArrowDirection.Any, true);
				vc.cellSelected += (sender, e) => {
					popover.Dismiss(true);
				};
			} else {
				viewController.NavigationController.PushViewController (vc, true);
				vc.cellSelected += (sender, e) => {
					vc.NavigationController.PopViewController(true);
				};
			}
		}

		public override UITableViewCell GetCell (UITableView tableView)
		{
			CountryFieldCell cell = this.GenerateCellWithNibName<CountryFieldCell> ("CountryFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}

	}
}