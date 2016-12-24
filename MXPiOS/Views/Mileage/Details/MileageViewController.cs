using System;
using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.Specialized;
using System.Collections.Generic;
using Mxp.Core.Utils;

namespace Mxp.iOS
{
	public partial class MileageViewController : MXPViewController
	{
		private Mileage mileage;
		private bool mileageIsLoaded = false;
		private NSObject _shownotification;
		private NSObject _hidenotification;
		private Actionables actionables;

		private UIImage _actionImage;
		private UIImage ActionImage {
			get {
				if (this._actionImage == null)
					this._actionImage = UIImage.FromFile ("navbar_overflow.png");

				return this._actionImage;
			}
		}

		private ExpenseItem expenseItem {
			get {
				return this.mileage.ExpenseItems [0];
			}
		}

		public MileageViewController (Mileage mileage) : base ("MileageViewController", null) {
			this.mileage = mileage;
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.SetupActions ();

			this.Title = this.mileage.VDetailsBarTitle;

			this.TableView.Source = new Source (this.mileage, this.TableView, this);
		}

		public override void ViewDidAppear (bool animated) {
			base.ViewDidAppear (animated);
			loadMileage ();
		}

		private void loadMileage () {
			if (mileageIsLoaded)
				return;

			if (!this.mileage.IsNew)
				this.LoadSegments ();
			else
				this.LoadVehicles ();

			mileageIsLoaded = true;
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			this.configureTopBar ();
			this.mileage.PropertyChanged += HandlePropertyChanged;
			this.mileage.MileageSegments.CollectionChanged += (sender, e) => this.collectionChange (e);

			_hidenotification = UIKeyboard.Notifications.ObserveDidHide (HideCallback);
			_shownotification = UIKeyboard.Notifications.ObserveWillShow (ShowCallback);

			this.AutomaticallyAdjustsScrollViewInsets = false;
			this.EdgesForExtendedLayout = UIRectEdge.None;
		}

		void ShowCallback (object obj, UIKeyboardEventArgs args) {
			this.TableView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, args.FrameEnd.Size.Height, 0.0f);
		}

		void HideCallback (object obj, UIKeyboardEventArgs args) {
			this.TableView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, 0.0f, 0.0f);
		}

		void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName.Equals ("IsChanged")) {
				this.configureTopBar ();
				this.TableView.ReloadData ();
			}
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			this.mileage.PropertyChanged -= HandlePropertyChanged;
			this.mileage.MileageSegments.CollectionChanged -= (sender, e) => this.collectionChange (e);

			// Unregister the callbacks
			if (_shownotification != null)
				_shownotification.Dispose ();
			if (_hidenotification != null)
				_hidenotification.Dispose ();
		}

		private async void LoadVehicles () {
			LoadingView.showMessage ();

			try {
				await this.mileage.Vehicles.FetchAsync ();
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.TableView.Source = new Source (this.mileage, this.TableView, this);
		}

		public async void LoadSegments () {
			LoadingView.showMessage ();

			try {
				await this.mileage.MileageSegments.FetchAsync ();
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}
		}

		public void collectionChange (NotifyCollectionChangedEventArgs e) {
			this.configureTopBar ();
			this.TableView.ReloadData ();
		}

		public void configureTopBar () {
			if (this.mileage.IsNew || this.mileage.IsChanged || this.expenseItem.IsChanged) {
				this.NavigationItem.SetHidesBackButton (true, true);

				this.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Plain, (sender, e) => {
					this.processCancel ();
				}), true);

				this.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Save), UIBarButtonItemStyle.Done, (sender, e) => {
					this.processSave ();
				}), true);

			} else {
				this.NavigationItem.SetHidesBackButton (false, true);

				List<UIBarButtonItem> rightButtons = new List<UIBarButtonItem> ();

				if (this.actionables.HasActions) {
					rightButtons.Add (new UIBarButtonItem (this.ActionImage, UIBarButtonItemStyle.Plain, (sender, e) => {
						new ActionnablesWrapper (actionables, this, this.NavigationItem.RightBarButtonItem).show();
					}));
				}

				if (this.expenseItem.CanShowDelete) {
					rightButtons.Add (new UIBarButtonItem (UIBarButtonSystemItem.Trash, (sender, e) => {
						this.confirmDeleteExpense ();
					}));
				}

				this.NavigationItem.SetRightBarButtonItems (rightButtons.ToArray (), true);
				this.NavigationItem.SetLeftBarButtonItem (null, true);
			}
		}

		public void SetupActions () {
			List<Actionable> actions = new List<Actionable> ();

			if (this.expenseItem.CanChangeAccountType)
				actions.Add (new Actionable (this.expenseItem.VChangeAccountType, this.ChangeExpenseTypeAsync));

			this.actionables = new Actionables (Labels.GetLoggedUserLabel (Labels.LabelEnum.Actions), actions);
		}

		public async void ChangeExpenseTypeAsync () {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Waiting) + "...");

			try {
				await this.expenseItem.ChangeAccountTypeAsync ();
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.Close ();
		}

		public async void processSave () {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Saving) + "...");

			try {
				if (this.mileage.IsNew) {
					await this.mileage.CreateAsync ();
				} else {
					await this.mileage.SaveAsync ();
				}
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.Close ();
		}

		public async void processCancel () {
			if (!this.mileage.IsNew) {
				LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancelling));

				this.mileage.ResetChanged ();

				try {
					await LoggedUser.Instance.BusinessExpenses.FetchAsync ();
				} catch (Exception e) {
					MainNavigationController.Instance.showError (e);
					return;
				} finally {
					LoadingView.hideMessage ();
				}
			}

			this.Close ();
		}

		public void confirmDeleteExpense () {
			UIAlertView alert = new UIAlertView (Labels.GetLoggedUserLabel (Labels.LabelEnum.Delete) + " " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Mileage), Labels.GetLoggedUserLabel (Labels.LabelEnum.DoYouConfirm), null, Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), new string [] { "OK" });
			alert.Show ();
			alert.Clicked += (object sender, UIButtonEventArgs e) => {
				if (e.ButtonIndex == 1) {
					this.deleteAction ();
				}
			};
		}

		public async void deleteAction () {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Deleting) + "...");

			try {
				await this.expenseItem.DeleteExpenseAsync ();
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.NavigationController.PopViewController (true);
		}

		private class Source : UITableViewSource
		{
			public Mileage Mileage;
			public UITableView TableView;

			public SegmentsSource SegmentsSource;

			public SectionFieldsSource StaticsSource;
			public SectionFieldsSource DynamicFieldsSource;

			public Source (Mileage mileage, UITableView tableView, UIViewController viewController) {
				this.Mileage = mileage;

				this.TableView = tableView;

				this.SegmentsSource = new SegmentsSource (this.Mileage.MileageSegments, tableView, viewController);
				this.StaticsSource = new SectionFieldsSource (this.Mileage.GetMainFields (this.Mileage.ExpenseItems [0]), viewController, Labels.GetLoggedUserLabel (Labels.LabelEnum.General));
				this.DynamicFieldsSource = new SectionFieldsSource (this.Mileage.ExpenseItems [0].GetAllFields (), viewController, Labels.GetLoggedUserLabel (Labels.LabelEnum.Details));
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
				if (indexPath.Section == 0) {
					return this.SegmentsSource.GetCell (tableView, indexPath.Row);
				}
				if (indexPath.Section == 1) {
					return this.StaticsSource.GetCell (tableView, indexPath.Row);
				}

				if (indexPath.Section == 2) {
					return this.DynamicFieldsSource.GetCell (tableView, indexPath.Row);
				}

				return null;
			}

			public override nint NumberOfSections (UITableView tableView) {
				return 3;
			}

			public override nint RowsInSection (UITableView tableview, nint section) {
				if (section == 0) {
					return this.SegmentsSource.RowsInSection (tableview);
				}

				if (section == 1) {
					return this.StaticsSource.RowsInSection (tableview);
				}

				if (section == 2) {
					return this.DynamicFieldsSource.RowsInSection (tableview);
				}

				return 0;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath) {
				if (indexPath.Section == 0) {
					return this.SegmentsSource.CanEditRow (tableView, indexPath.Row);
				}

				if (indexPath.Section == 1) {
					return this.StaticsSource.CanEditRow (tableView, indexPath.Row);
				}

				if (indexPath.Section == 2) {
					return this.DynamicFieldsSource.CanEditRow (tableView, indexPath.Row);
				}

				return false;
			}

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath) {
				if (indexPath.Section == 0) {
					this.SegmentsSource.CommitEditingStyle (tableView, editingStyle, indexPath.Row);
				}

				if (indexPath.Section == 1) {
					this.StaticsSource.CommitEditingStyle (tableView, editingStyle, indexPath.Row);
				}

				if (indexPath.Section == 2) {
					this.StaticsSource.CommitEditingStyle (tableView, editingStyle, indexPath.Row);
				}
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath) {
				if (indexPath.Section == 0) {
					this.SegmentsSource.RowSelected (tableView, indexPath.Row);
				}
				if (indexPath.Section == 1) {
					this.StaticsSource.RowSelected (tableView, indexPath.Row, tableView.CellAt (indexPath));
				}
				if (indexPath.Section == 2) {
					this.DynamicFieldsSource.RowSelected (tableView, indexPath.Row, tableView.CellAt (indexPath));
				}
			}

			public override UITableViewCellAccessory AccessoryForRow (UITableView tableView, NSIndexPath indexPath) {
				if (indexPath.Section == 0) {
					return this.SegmentsSource.AccessoryForRow (tableView, indexPath.Row);
				}
				if (indexPath.Section == 1) {
					return this.StaticsSource.AccessoryForRow (tableView, indexPath.Row);
				}

				if (indexPath.Section == 2) {
					return this.DynamicFieldsSource.AccessoryForRow (tableView, indexPath.Row);
				}

				return UITableViewCellAccessory.None;
			}

			public override string TitleForHeader (UITableView tableView, nint section) {

				if (section == 1) {
					return Labels.GetLoggedUserLabel (Labels.LabelEnum.General);
				}

				if (section == 2) {
					return Labels.GetLoggedUserLabel (Labels.LabelEnum.Details);
				}
				return null;
			}

			public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath) {
				if (indexPath.Section == 0) {
					return this.SegmentsSource.GetHeightForRow (tableView, indexPath.Row);
				}
				if (indexPath.Section == 1) {
					return this.StaticsSource.GetHeightForRow (tableView, indexPath.Row);
				}

				if (indexPath.Section == 2) {
					return this.DynamicFieldsSource.GetHeightForRow (tableView, indexPath.Row);
				}

				return 44;
			}
		}
	}
}