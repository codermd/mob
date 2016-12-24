using System;
using Mxp.Core.Business;
using UIKit;
using System.Collections.Specialized;
using Foundation;
using CoreLocation;
using ObjCRuntime;
using Mxp.Core.Utils;
using System.Collections.Generic;

namespace Mxp.iOS
{
	public class SegmentsSource
	{

		public MileageSegments Segments;
		public UITableView TableView;
		public UIViewController ViewController;

		public SegmentsSource(MileageSegments segments, UITableView tableView, UIViewController viewController){
			this.Segments = segments;
			this.TableView = tableView;
			this.ViewController = viewController;

			this.Segments.CollectionChanged += (sender, e) => this.collectionChange(e);
		}

		public void collectionChange( NotifyCollectionChangedEventArgs e){
			this.TableView.ReloadData ();
		}

		public UITableViewCell generateLocationCell (UITableView tableView, MileageSegment mileageSegment){

			MileageSegmentCell cell = tableView.DequeueReusableCell ("MileageSegmentCell") as MileageSegmentCell;
			if (cell == null) {
				cell = MileageSegmentCell.Create ();
			}

			cell.MileageSegment = mileageSegment;
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			return cell;
		}

		public UITableViewCell GetCell (UITableView tableView, int row)
		{
			if (this.Segments.CanManage && row == this.Segments.Count + 1) {


				MileageAddSegmentCell addCell =  tableView.DequeueReusableCell ("MileageAddSegmentCell") as MileageAddSegmentCell;
				if(addCell == null) {
					addCell = MileageAddSegmentCell.Create ();
				}
				addCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				return addCell;
			}

			if (row == 0) {

				MileageShowMapCell addCell =  tableView.DequeueReusableCell ("MileageShowMapCell") as MileageShowMapCell;
				if(addCell == null) {
					addCell = MileageShowMapCell.Create ();
				}
				addCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				return addCell;

			}

			return this.generateLocationCell (tableView, this.Segments [row - 1]);

		}

		public int GetHeightForRow (UITableView tableView, int Row) {
			if (this.Segments.CanManage && Row == this.Segments.Count + 1) {
				return 55;
			}

			return 44;

		}

		public int RowsInSection (UITableView tableview)
		{
			int res = this.Segments.Count;

			//shwo map button
			res++;

			if (this.Segments.CanManage) {
				//Add button
				res = res + 1;
			}

			return res;
		}

		public bool CanEditRow (UITableView tableView, int row)
		{
			if (row == 0) {
				return false;
			}

			if (this.Segments.CanManage && row == this.Segments.Count+1) {
				return false;
			}
			return true;
		}

		public void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, int row)
		{
			int removeIndex = row - 1;
			this.Segments.RemoveItemAt (removeIndex, true);
			tableView.ReloadData ();
		}

		public void RowSelected (UITableView tableView,  int row){
			if (row == 0) {
				SegmentsMapViewController vc = new SegmentsMapViewController ();
				vc.segments = this.Segments;

				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {

					UINavigationController nvc = new UINavigationController (vc);
					nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
					this.ViewController.PresentModalViewController (nvc, true);
					
				} else {
					this.ViewController.NavigationController.PushViewController (vc, true);
				}

				return;
			}

			if (row == this.Segments.Count + 1) {
				this.showAddPicker ();
			}

			//show on map
		}

		public void showAddPicker(){
			this.ShowAddLocation ();
		}


		private SearchLocationViewController searchLocationViewController;

		public void ShowAddLocation(){
			//Add part
			this.searchLocationViewController = new SearchLocationViewController ();

			this.searchLocationViewController.PredictionSelected += (sender, e) => {
				this.Segments.AddPrediction (e.Prediction);
			};
			this.searchLocationViewController.SegmentSelected += (sender, e) => {
				this.Segments.AddItem(e.MileageSegment);
			};

			this.searchLocationViewController.addReturning += (sender, e) => {
				this.addFirstLocation();
			};

			this.searchLocationViewController.addCurrentLocation += (sender, e) => {
				this.addCurrentLocation();
			};

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				UINavigationController nvc = new UINavigationController (this.searchLocationViewController);
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				this.ViewController.PresentViewController (nvc, true, null);
				this.searchLocationViewController.PredictionSelected += (sender, e) => {
					this.ViewController.DismissViewController(true, null);
				};
				this.searchLocationViewController.SegmentSelected += (sender, e) => {
					this.ViewController.DismissViewController(true, null);
				};
				this.searchLocationViewController.addCurrentLocation += (sender, e) => {
					this.ViewController.DismissViewController(true, null);
				};

				this.searchLocationViewController.addReturning += (sender, e) => {
					this.ViewController.DismissViewController(true, null);
				};

			} else {
				this.ViewController.NavigationController.PushViewController (this.searchLocationViewController, true);
				this.searchLocationViewController.PredictionSelected += (sender, e) => {
					this.searchLocationViewController.NavigationController.PopViewController (true);
				};
				this.searchLocationViewController.SegmentSelected += (sender, e) => {
					this.searchLocationViewController.NavigationController.PopViewController (true);
				};
				this.searchLocationViewController.addReturning += (sender, e) => {
					this.searchLocationViewController.NavigationController.PopViewController (true);
				};

				this.searchLocationViewController.addCurrentLocation += (sender, e) => {
					this.searchLocationViewController.NavigationController.PopViewController (true);
				};
			}
		}


		public void addCurrentLocation(){
			if (!CLLocationManager.LocationServicesEnabled) {
				MainNavigationController.Instance.showError (new ValidationError ("ERROR", Labels.GetLoggedUserLabel (Labels.LabelEnum.LocationServiceDisabled)));
				return;
			}
			if(this.LocationManager.Location != null) {
				this.Segments.AddItem (new MileageSegment (locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude));
			} else {
					
				if (LocationManager.RespondsToSelector (new Selector ("requestWhenInUseAuthorization"))) 
				{
					LocationManager.RequestWhenInUseAuthorization ();
				}

				this.locationManager.StartMonitoringSignificantLocationChanges ();
				this.LocationManager.StartUpdatingLocation ();
				this.LocationManager.LocationsUpdated += HandleLocationsUpdated;
				this.LocationManager.UpdatedLocation += HandleUpdatedLocation;
			}
		}

		void HandleUpdatedLocation (object sender, CLLocationUpdatedEventArgs e)
		{
			this.Segments.AddLatLong (locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude);
			this.LocationManager.UpdatedLocation -= HandleUpdatedLocation;

			this.locationManager.StopMonitoringSignificantLocationChanges ();
			this.LocationManager.StopUpdatingLocation ();
			this.locationManager = null;

		}

		void HandleLocationsUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			this.Segments.AddLatLong (locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude);
			this.LocationManager.LocationsUpdated -= HandleLocationsUpdated;
			this.LocationManager.StopMonitoringSignificantLocationChanges ();
			this.LocationManager.StopUpdatingLocation ();
			this.locationManager = null;

		}

		private CLLocationManager locationManager;
		private CLLocationManager LocationManager {
			get {
				if (this.locationManager == null) {
					this.locationManager = new CLLocationManager ();
					this.locationManager.DesiredAccuracy = 1;
				}
				return this.locationManager;
			}
		}

			
		public void addFirstLocation(){
			this.Segments.AddReturningItem ();
		}

		public  UITableViewCellAccessory AccessoryForRow (UITableView tableView, int row) {
			if (row == 0) {
				return UITableViewCellAccessory.DisclosureIndicator;
			}
			return UITableViewCellAccessory.None;
		}
	}
}