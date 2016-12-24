using System;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class AllowanceSegmentsSource : SectionSource
	{

		public Allowance Allowance;
		public UITableView TableView;
		public UIViewController ViewController;

		public override string Title{ get { return this.Allowance.VSegmentSectionTitle;
			} set { }}

		public AllowanceSegmentsSource(Allowance allowance, UITableView tableView, UIViewController viewController){
			this.Allowance = allowance;
			this.TableView = tableView;
			this.ViewController = viewController;
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Segments);
		}

		public override UITableViewCell GetCell (UITableView tableView, int row)
		{
			AllowanceSegmentCell cell = (AllowanceSegmentCell)tableView.DequeueReusableCell ("AllowanceSegmentCell");
			if (cell == null) {
				cell = AllowanceSegmentCell.Create ();
			}
			cell.AllowanceSegment = this.Allowance.AllowanceSegments [row];
			return cell;
		}

		public override int RowsInSection (UITableView tableview)
		{
			return this.Allowance.AllowanceSegments.Count;
		}

		public override bool CanEditRow (UITableView tableView, int row)
		{
			return false;
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, int row)
		{
		}

		public override void RowSelected (UITableView tableView,  int row, UITableViewCell cell){
			AllowanceSegmentViewController vc = new AllowanceSegmentViewController ();
			vc.segment = this.Allowance.AllowanceSegments [row];

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				UINavigationController nvc = new UINavigationController (vc);
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				this.ViewController.PresentViewController (nvc, true, null);
			} else {
				if (!this.Allowance.IsChanged || this.Allowance.IsNew) {
					this.ViewController.NavigationController.PushViewController (vc, true);
				} else {

					this.SaveAndShowViewController (vc);	
				}
			}
		}

		public async void SaveAndShowViewController(UIViewController vc){
			if (!this.Allowance.IsNew && this.Allowance.IsChanged) {
				LoadingView.showMessage(Labels.GetLoggedUserLabel (Labels.LabelEnum.Saving) + "...");
				try {
					await this.Allowance.SaveAsync ();
				} catch(Exception e){
					MainNavigationController.Instance.showError (e);
					return;
				} finally {
					LoadingView.hideMessage ();
				}
			}
			this.ViewController.NavigationController.PushViewController (vc, true);
		}


		public override float GetHeightForRow (UITableView tableView, int row) {
			return 88;
		}

		public override UITableViewCellAccessory AccessoryForRow (UITableView tableView, int row){
			return UITableViewCellAccessory.DisclosureIndicator;
		}


	}
}