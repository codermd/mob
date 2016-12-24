using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Mxp.Core.Business;
using Mxp.Core.Services;
using Mxp.Core.Services.Google;
using UIKit;

namespace Mxp.iOS
{
	public partial class AutocompleteStringViewController : UIViewController
	{
		public AutocompleteStringDataFieldCell DataField { get; set; }

		private NSObject _shownotification;
		private NSObject _hidenotification;
		private Source source;

		public class SelectedEventArgs : EventArgs
		{
			public Prediction Prediction { get; set; }
		}

		public AutocompleteStringViewController () : base ("AutocompleteStringViewController", null) {
		
		}

		public override void ViewDidAppear (bool animated) {
			base.ViewDidAppear (animated);

			this.SearchBar.BecomeFirstResponder ();
			this.SearchString (this.SearchBar.Text);
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			_hidenotification = UIKeyboard.Notifications.ObserveDidHide (HideCallback);
			_shownotification = UIKeyboard.Notifications.ObserveWillShow (ShowCallback);

			this.source = new Source (new List<Prediction> ());
			this.TableView.Source = source;

			this.SearchBar.Text = this.DataField.Field.GetValue<Prediction> ()?.description;
			this.SearchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => this.SearchString (this.SearchBar.Text);

			this.source.cellSelected += (sender, args) => {
				this.DataField.Field.Value = args.Prediction;
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					this.DismissViewController (true, null);
				} else {
					this.NavigationController.PopViewController (true);
				}
			};
		}

		private async void SearchString (String searchText) {
			try {
				source.Predictions = (await GoogleService.Instance.FetchPlacesLocationsAsync (searchText, GoogleService.PlaceTypeEnum.Merchant, DataField.Field.GetModel<ExpenseItem> ()?.Country)).predictions;
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			}

			this.TableView.ReloadData ();
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			if (_shownotification != null)
				_shownotification.Dispose ();
			if (_hidenotification != null)
				_hidenotification.Dispose ();
		}

		void ShowCallback (object obj, UIKit.UIKeyboardEventArgs args) {
			if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad) {
				this.TableView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, args.FrameEnd.Size.Height, 0.0f);
			}
		}

		void HideCallback (object obj, UIKit.UIKeyboardEventArgs args) {
			if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad) {
				this.TableView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, 0.0f, 0.0f);
			}
		}

		private class Source : UITableViewSource
		{
			public event EventHandler<SelectedEventArgs> cellSelected = delegate { };

			public List<Prediction> Predictions;

			public Source (List<Prediction> predicions) {
				this.Predictions = predicions;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
				DefaultCell cell = (DefaultCell)tableView.DequeueReusableCell (DefaultCell.Key);

				if (cell == null) {
					cell = DefaultCell.Create ();
				}
				cell.TextLabel.Text = this.Predictions [indexPath.Row].description;
				return cell;
			}

			public override nint RowsInSection (UITableView tableview, nint section) {
				return this.Predictions.Count;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath) {
				this.cellSelected (this, new SelectedEventArgs () { Prediction = this.Predictions [indexPath.Row] });
			}
		}
	}
}