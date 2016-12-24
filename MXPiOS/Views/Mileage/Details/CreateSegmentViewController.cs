using System;
using System.Collections.ObjectModel;

using System.Collections.Specialized;
using Foundation;
using Mxp.Core.Services;
using UIKit;
using Mxp.Core.Business;
using Mxp.Core.Services.Google;
using System.Collections.Generic;

namespace Mxp.iOS
{
	public partial class CreateSegmentViewController : MXPTableViewController
	{

		public CreateSegmentViewController (IntPtr handle) : base (handle)
		{
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.CreateMileage);
		}

		private Source source;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Searchbar.SearchButtonClicked += (object sender, EventArgs e) => this.searchBarChange();

			this.source = new Source ();

			this.TableView.Source = this.source;

			this.TableView.ReloadData ();

		}

		public async void reloadData(){
			try {
				this.source.setPredictions(await GoogleService.Instance.FetchPlacesLocationsAsync (this.Searchbar.Text));
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			}

			this.TableView.ReloadData ();
		}

		public void searchBarChange(){
			this.reloadData ();
		}


		private class Source : UITableViewSource 
		{
			private List<Prediction> predictions;

			public Source(){
				this.predictions = new List<Prediction>();
			}

			public void setPredictions(Predictions predictions){
				if (predictions == null) {
					this.predictions.Clear ();
				} else {
					this.predictions = predictions.predictions;
				}
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return this.predictions.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				DefaultCell cell = (DefaultCell)tableView.DequeueReusableCell (DefaultCell.Key);

				if ( cell == null) {
					cell = DefaultCell.Create();
				}

				cell.TextLabel.Text = this.predictions [indexPath.Row].description;
				return cell;
			}
		}

	}

}
