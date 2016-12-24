
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.Generic;
using Mxp.Core.Services.Google;
using Mxp.Core.Services;

namespace Mxp.iOS
{
	public partial class SearchLocationViewController : MXPViewController
	{

		public Mileage Mileage;

		//Event management
		public class PredictionSelectedEventArgs : EventArgs
		{
			public Prediction Prediction { get; set;}
		}

		//Event management
		public class SegmentSelectedEventArgs : EventArgs
		{
			public MileageSegment MileageSegment { get; set;}
		}

		// Starting off with an empty handler avoids pesky null checks
		public event EventHandler<PredictionSelectedEventArgs> PredictionSelected = delegate {};
		public event EventHandler<SegmentSelectedEventArgs> SegmentSelected = delegate {};

		public event EventHandler addReturning = delegate {};
		public event EventHandler addCurrentLocation = delegate {};

		public SearchLocationViewController () : base ("SearchLocationViewController", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.SearchBar.SearchButtonClicked += (object sender, EventArgs e) => this.searchBarChange();

			this.SearchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => this.searchBarChange();
			this.searchBarChange ();


			this.TableView.ReloadData ();

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.CreateMileage);
		}

		void HandlecellSelected (object sender, PredictionSelectedEventArgs e)
		{
			this.PredictionSelected (sender, e);
		}

		void HandleSegmentSelected (object sender, SegmentSelectedEventArgs e)
		{
			this.SegmentSelected (sender, e);
		}

		public async void reloadData(){
			Predictions predictions;
			var ms = new MileageSegments ();

			try {
				predictions = await GoogleService.Instance.FetchPlacesLocationsAsync (this.SearchBar.Text);
				await MileageService.Instance.GetFavouriteLocations (ms, this.SearchBar.Text);
			} catch (Exception) {
				return;
			}

			((SearchSource)this.TableView.Source).setGooglePredictions(predictions, ms);
			this.TableView.ReloadData ();
		}

		public void searchBarChange(){
			if (String.IsNullOrEmpty (this.SearchBar.Text)) {
				NoTextSource source = new NoTextSource ();
				this.TableView.Source = source;
				source.addCurrentLocation+= (sender, e) => {
					this.addCurrentLocation(sender,e);
				};

				source.addReturning += (sender, e) => {
					this.addReturning(sender,e);
				};

				this.TableView.ReloadData ();
			} else {
				if (!(this.TableView.Source is SearchSource)) {
					var source = new SearchSource ();
					source.PredictionSelected += HandlecellSelected;
					source.SegmentSelected += HandleSegmentSelected;
					this.TableView.Source = source;
				}
				this.reloadData ();
			}
		}


		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.SearchBar.BecomeFirstResponder ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.ImageView.Image = UIImage.FromBundle("powered_by_google_on_white");

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem(Labels.GetLoggedUserLabel (Labels.LabelEnum.Close), UIBarButtonItemStyle.Done,(sender, e) => {
					this.DismissViewController(true, null);
				}), true);
			}
		}

		private class NoTextSource: UITableViewSource
		{

			public event EventHandler addReturning = delegate {};
			public event EventHandler addCurrentLocation = delegate {};

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return 2;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Row == 0) {

					CurrentLocationCell cell = tableView.DequeueReusableCell (CurrentLocationCell.Key) as CurrentLocationCell;
					if (cell == null) {
						cell = CurrentLocationCell.Create ();
					}
					return cell;
				}

				if (indexPath.Row == 1) {
					AddReturningCell cell = tableView.DequeueReusableCell (AddReturningCell.Key) as AddReturningCell;
					if (cell == null) {
						cell = AddReturningCell.Create ();
					}
					return cell;
				}
				return null;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Row == 0) {
					this.addCurrentLocation (this, new EventArgs());
				}
				if (indexPath.Row == 1) {
					this.addReturning (this, new EventArgs());
				}
			}

		}

		private class SearchSource : UITableViewSource 
		{

			// Starting off with an empty handler avoids pesky null checks
			public event EventHandler<PredictionSelectedEventArgs> PredictionSelected = delegate {};
			public event EventHandler<SegmentSelectedEventArgs> SegmentSelected = delegate {};

			private List<Prediction> googlePredictions;
			private MileageSegments mxpSegments;

			public bool hasFavorite {
				get { 
					return this.mxpSegments != null && this.mxpSegments.Count>0;
				}
			}

			public SearchSource(){
				this.googlePredictions = new List<Prediction>();
			}

			public void setGooglePredictions(Predictions gpredictions, MileageSegments mxppredictions){
				if (gpredictions == null) {
					this.googlePredictions.Clear ();
				} else {
					this.googlePredictions = gpredictions.predictions;
				}

				this.mxpSegments = mxppredictions;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{

				if (indexPath.Section == 0 && this.hasFavorite) {
					this.SegmentSelected (this, new SegmentSelectedEventArgs(){MileageSegment = this.mxpSegments[indexPath.Row]});
				} else {
					this.PredictionSelected (this, new PredictionSelectedEventArgs(){Prediction = this.googlePredictions[indexPath.Row]});
				}
			}

			public override nint NumberOfSections (UITableView tableView)
			{ 
				if (this.hasFavorite) {
					return 2;
				} else {
					return 1;
				}

			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				if (section == 0 && this.hasFavorite) {
					return this.mxpSegments.Count;
				} else {
					return this.googlePredictions.Count;
				}
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0 && this.hasFavorite) {
					DefaultCell cell = (DefaultCell)tableView.DequeueReusableCell (DefaultCell.Key);

					if ( cell == null) {
						cell = DefaultCell.Create();
					}

					cell.TextLabel.Text = this.mxpSegments [indexPath.Row].LocationAliasName;
					return cell;
				} else {
					DefaultCell cell = (DefaultCell)tableView.DequeueReusableCell (DefaultCell.Key);

					if ( cell == null) {
						cell = DefaultCell.Create();
					}

					cell.TextLabel.Text = this.googlePredictions [indexPath.Row].description;
					return cell;
				}
				return null;
			}

			public override string TitleForHeader (UITableView tableView, nint section)
			{
				if (section == 0 && this.hasFavorite) {
					return Labels.GetLoggedUserLabel (Labels.LabelEnum.Recent);
				} else {
					if (this.hasFavorite) {
						return "From Google";
					}
					return null;
				}
			}
		}

	}
}

