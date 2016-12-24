using System;

using UIKit;

using Mxp.Core.Services;
using Foundation;
using Mxp.Core.Business;
using System.Threading.Tasks;
using CoreGraphics;

namespace Mxp.iOS
{
	public partial class LookupViewController : MXPViewController
	{

		public event EventHandler<LookupSelectedEventArgs> cellSelected = delegate {};

		private LookupField _LookupField;
		public LookupField LookupField {
			get {
				return this._LookupField;
			}
			set {
				if (this._LookupField != null) {
					this._LookupField.LookupItemsChanged -= HandlefetchItemsComplete;
				}
				this._LookupField = value;
				value.LookupItemsChanged += HandlefetchItemsComplete;
			}
		}

		void HandlefetchItemsComplete (object sender, EventArgs e)
		{
			this.TableView.ReloadData ();
		}
		public LookUpSource LookUpSource;

		public LookupViewController () : base ("LookupViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			if (this._LookupField != null) {
				this._LookupField.LookupItemsChanged -= HandlefetchItemsComplete;
			}

			base.DidReceiveMemoryWarning ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.LookUpSource = new LookUpSource (this, this.LookupField);

			this.LookUpSource.cellSelected += (sender, e) => {
				this.cellSelected (sender, e);
			};

			this.TableView.Source = this.LookUpSource;
			this.TableView.ReloadData ();


			this.SearchBar.SearchButtonClicked += (sender, e) => {
				this.processSearch ();
			};

			this.SearchBar.TextChanged += (sender, e) => {
				this.processSearch ();
			};

			this.TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

		}

		public async void processSearch() {
			this.loadingIndicator.Hidden = false;

			try {
				await this.LookupField.FetchItems (this.SearchBar.Text);
			} catch (Exception) {}

			this.loadingIndicator.Hidden = true;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.SearchBar.BecomeFirstResponder ();
			this.processSearch ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.NavigationItem.SetHidesBackButton (false, false);
		}
	}
		

	public class LookUpSource : UITableViewSource
	{

		public event EventHandler<LookupSelectedEventArgs> cellSelected = delegate {};

		public LookupField LookupField { get; set;}

		public UIViewController ViewController { get; set;}


		public LookUpSource (UIViewController viewController, LookupField lookupField)
		{
			this.ViewController = viewController;
			this.LookupField = lookupField;
		}


		public override nint NumberOfSections (UITableView tableView)

		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return this.LookupField.Results.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{

			if (this.LookupField.Results.Count == 0) {
				UITableViewCell emtyCell = tableView.DequeueReusableCell ("EmptyCell");

				if (emtyCell == null)
					emtyCell = EmptyCell.Create ();

				return emtyCell;
			}

			DefaultCell cell = (DefaultCell)tableView.DequeueReusableCell (DefaultCell.Key);

			if ( cell == null) {
				cell = DefaultCell.Create();
			}

			cell.TextLabel.Text = this.LookupField.Results[indexPath.Row].VTitle;

			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{	
			LookupSelectedEventArgs e = new LookupSelectedEventArgs();
			e.LookupItem = this.LookupField.Results [indexPath.Row];
			this.LookupField.ResetResults ();
			this.cellSelected (this, e);
		}

	}

	public class LookupSelectedEventArgs : EventArgs
	{
		public LookupItem LookupItem { get; set;}
	}


}