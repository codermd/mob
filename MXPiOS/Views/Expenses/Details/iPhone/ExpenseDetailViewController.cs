using System;
using UIKit;
using Mxp.Core.Business;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Utils;
using Foundation;

namespace Mxp.iOS
{
	partial class ExpenseDetailViewController : MXPViewController
	{
		private Expense Expense;
		private ExpenseItem expenseItem;
		private bool fromCommande;
		private UIViewController selectedViewController;
		private Actionables actionables;

		private UIImage _actionImage;
		private UIImage ActionImage {
			get {
				if (this._actionImage == null)
					this._actionImage = UIImage.FromFile ("navbar_overflow.png");

				return this._actionImage;
			}
		}

		public ExpenseDetailViewController (IntPtr handle) : base (handle) {

		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.SetupActions ();

			this.configureTopBar ();
			this.configureSegments ();
			this.SegmentedControlView.SelectedSegment = 0;
		}

		public override void ViewWillAppear (bool animated) {
			this.configureSegments ();
			this.Title = this.Expense.VDetailsBarTitle;

			bindModels ();
			this.showSection ((int)this.SegmentedControlView.SelectedSegment);

			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated) {
			base.ViewDidAppear (animated);

			this.configureTopBar ();
		}

		public override void ViewWillDisappear (bool animated) {
			unbindModels ();

			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated) {
			this._actionImage = null;

			base.ViewDidDisappear (animated);
		}

		public void bindModels () {
			this.expenseItem.PropertyChanged += HandlePropertyChanged;
			this.Expense.PropertyChanged += HandlePropertyChanged;
			this.expenseItem.Attendees.CollectionChanged += HandleCollectionChanged;
			this.Expense.Receipts.CollectionChanged += HandleCollectionChanged;
		}

		public void unbindModels () {
			this.expenseItem.PropertyChanged -= HandlePropertyChanged;
			this.Expense.PropertyChanged -= HandlePropertyChanged;
			this.expenseItem.Attendees.CollectionChanged -= HandleCollectionChanged;
			this.Expense.Receipts.CollectionChanged -= HandleCollectionChanged;
		}

		void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			this.configureTopBar ();
			this.configureSegments ();
		}

		void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			this.configureSegments ();
		}

		public async void cancelProcess () {
			if (!this.Expense.IsNew) {
				LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancelling));

				this.Expense.ResetChanged ();

				try {
					await LoggedUser.Instance.BusinessExpenses.FetchAsync ();
				} catch (Exception error) {
					MainNavigationController.Instance.showError (error);
					return;
				} finally {
					LoadingView.hideMessage ();
				}
			}

			this.Close ();
		}

		public void configureTopBar () {
			UIBarButtonItem leftButton = null;
			List<UIBarButtonItem> rightButtons = new List<UIBarButtonItem> ();

			if (this.fromCommande) {
				leftButton = new UIBarButtonItem (UIBarButtonSystemItem.Done, (sender, e) => {
					this.DismissViewController (true, null);
				});
			}

			if (this.actionables.HasActions
				&& (this.expenseItem.IsNew
					|| (!this.expenseItem.IsNew && !this.expenseItem.ParentExpense.IsChanged))) {
				rightButtons.Add (new UIBarButtonItem (this.ActionImage, UIBarButtonItemStyle.Plain, (sender, e) => {
					new ActionnablesWrapper (this.actionables, this, this.NavigationItem.RightBarButtonItem).show ();
				}));
			}

			if (this.expenseItem.CanShowSavingOptions) {
				rightButtons.Add (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Save), UIBarButtonItemStyle.Done, (sender, e) => {
					this.saveProcess ();
				}));

				leftButton = new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Plain, (sender, e) => {
					this.cancelProcess ();
				});
			}

			if (this.expenseItem.CanShowDelete) {
				rightButtons.Add (new UIBarButtonItem (UIBarButtonSystemItem.Trash, (sender, e) => {
					this.confirmDeleteExpense ();
				}));
			}

			this.NavigationItem.SetLeftBarButtonItem (leftButton, true);
			this.NavigationItem.SetRightBarButtonItems (rightButtons.ToArray (), true);

			this.NavigationItem.SetHidesBackButton (this.fromCommande, true);
		}

		private void SetupActions () {
			List<Actionable> actions = new List<Actionable> ();

			if (this.expenseItem.CanShowSplit)
				actions.Add (new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.Split), this.startSplitProcess));

			if (this.expenseItem.CanShowUnsplit)
				actions.Add (new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.Unsplit),this.ConfirmUnSplit));

			if (this.expenseItem.CanCopy)
				actions.Add (new Actionable (Labels.GetLoggedUserLabel (Labels.LabelEnum.SaveAndCopy), this.CopyExpenseAsync));

			if (this.expenseItem.CanChangeAccountType)
				actions.Add (new Actionable (this.expenseItem.VChangeAccountType, this.ChangeExpenseTypeAsync));

			this.actionables = new Actionables (Labels.GetLoggedUserLabel (Labels.LabelEnum.Actions), actions);
		}

		public async void saveProcess () {
			if (this.Expense.IsNew) {
				try {
					await this.createExpense ();
				} catch (Exception) {
					return;
				}

				return;
			}

			try {
				await this.Expense.SaveAsync (this.expenseItem);
			} catch (Exception) {
				return;
			}

			this.configureTopBar ();

			this.Close ();
		}

		private async void CopyExpenseAsync () {
			bool wasNew = this.Expense.IsNew;

			try {
				if (wasNew)
					await this.Expense.CreateAsync ();
				else if (this.Expense.IsChanged)
					await this.Expense.SaveAsync (this.expenseItem);
			} catch (Exception) {
				return;
			}

			Expense expense = (Expense)this.Expense.Clone ();

			UIStoryboard storyBoard = UIStoryboard.FromName ("ExpenseDetailsStoryboard", NSBundle.MainBundle);
			ExpenseDetailViewController vc = storyBoard.InstantiateInitialViewController () as ExpenseDetailViewController;
			vc.setExpenseItem (expense.ExpenseItems [0]);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad && !wasNew) {
				UINavigationController nvc = new UINavigationController (vc);
				nvc.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
				this.PresentViewController (nvc, true, null);
			} else {
				UIViewController [] viewControllers = new UIViewController [] { this.NavigationController.ViewControllers [0], vc };
				this.NavigationController.SetViewControllers (viewControllers, true);
			}
		}

		public async Task createExpense(){
			try {
				await this.Expense.CreateAsync ();
			} catch (Exception) {
				return;
			}

			this.configureTopBar ();

			this.Close ();
		}	

		public void startSplitProcess () {
			this.HidesBottomBarWhenPushed = true;
			CategoriesChooserSplit vc = new CategoriesChooserSplit (this.expenseItem.AddExpenseItem ());

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				UINavigationController nvc = new UINavigationController (vc);
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				this.PresentViewController (nvc, true, null);
			} else {
				this.NavigationController.PushViewController (vc, true);
			}
		}

		public void ConfirmUnSplit () {
			UIAlertView alert = new UIAlertView (Labels.GetLoggedUserLabel (Labels.LabelEnum.Unsplit) + " " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Expense), Labels.GetLoggedUserLabel (Labels.LabelEnum.DoYouConfirm), null, Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), new string [] { "OK" });
			alert.Show ();
			alert.Clicked += (object sender, UIButtonEventArgs e) => {
				if (e.ButtonIndex == 1) {
					this.startUnSplitProcess ();
				}
			};
		}

		public async void startUnSplitProcess () {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Unsplitting) + "...");

			try {
				await this.expenseItem.UnsplitAsync ();
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.NavigationController.PopToRootViewController (true);
		}

		public void confirmDeleteExpense () {
			UIAlertView alert = new UIAlertView (Labels.GetLoggedUserLabel (Labels.LabelEnum.Delete) + " " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Expense), Labels.GetLoggedUserLabel (Labels.LabelEnum.DoYouConfirm), null, Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), new string [] { "OK" });
			alert.Show ();
			alert.Clicked += (object sender, UIButtonEventArgs e) => {
				if (e.ButtonIndex == 1) {
					this.deleteAction ();
				}
			};
		}

		public async void ChangeExpenseTypeAsync () {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Waiting) + "...");

			try {
				await this.expenseItem.ChangeAccountTypeAsync ();
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.Close ();
		}

		public async void deleteAction () {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Deleting) + "...");

			try {
				await this.expenseItem.DeleteExpenseAsync ();
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.Close ();
		}

		public void configureSegments () {
			int selectedSegment = (int)this.SegmentedControlView.SelectedSegment;
			this.SegmentedControlView.RemoveAllSegments ();

			this.SegmentedControlView.InsertSegment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Details), 0, false);
			if (this.Expense.CanShowReceipts) {
				if (this.Expense.HasReceipts) {
					this.SegmentedControlView.InsertSegment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Receipts) + " (" + this.Expense.NumberReceipts + ")", 1, false);
				} else {
					this.SegmentedControlView.InsertSegment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Receipts), 1, false);
				}

				if (this.expenseItem.CanShowAttendees) {
					this.SegmentedControlView.InsertSegment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Attendees) + " (" + this.expenseItem.Attendees.Count + ")", 2, false);
				}
			} else {
				if (this.expenseItem.CanShowAttendees) {
					this.SegmentedControlView.InsertSegment (Labels.GetLoggedUserLabel (Labels.LabelEnum.Attendees) + " (" + this.expenseItem.Attendees.Count + ")", 1, false);
				}
			}

			this.SegmentedControlView.SelectedSegment = selectedSegment;
		}

		public void setExpenseItem (ExpenseItem expenseItem, bool fromCommand = false) {
			this.expenseItem = expenseItem;
			this.Expense = this.expenseItem.ParentExpense;
			this.fromCommande = fromCommand;
		}

		partial void selectionChange (UISegmentedControl sender) {
			this.showSection ((int)this.SegmentedControlView.SelectedSegment);
		}

		private void showSection (int index) {
			UIViewController viewController = null;

			switch (index) {
				case 0:
					viewController = this.Storyboard.InstantiateViewController ("ExpenseDetailsTableViewController");
					break;
				case 1:
					if (this.Expense.CanShowReceipts) {
						viewController = this.Storyboard.InstantiateViewController ("ReceiptsCollectionViewController");
					} else {
						viewController = this.Storyboard.InstantiateViewController ("AttendeesTableViewController");
					}
					break;
				case 2:
					viewController = this.Storyboard.InstantiateViewController ("AttendeesTableViewController");
					break;
				default:
					return;
			}

			if (this.selectedViewController != null && viewController != null && (this.selectedViewController.GetType ().Equals (viewController.GetType ()))) {
				return;
			}

			if (this.selectedViewController != null) {
				this.selectedViewController.RemoveFromParentViewController ();
			}

			viewController.WillMoveToParentViewController (this);

			if (this.MainView.Subviews.Length != 0) {
				for (int i = 0; i < this.MainView.Subviews.Length; i++) {
					this.MainView.Subviews [i].RemoveFromSuperview ();
				}
			}

			((IExpenseDetailsSubController)viewController).setExpenseItem (this.expenseItem);

			this.AddChildViewController (viewController);

			viewController.View.Frame = this.MainView.Bounds;
			viewController.DidMoveToParentViewController (this);
			this.MainView.AddSubview (viewController.View);

			this.selectedViewController = viewController;
		}
	}
}