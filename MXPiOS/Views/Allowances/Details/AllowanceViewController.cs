using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Collections.Generic;
using System;

namespace Mxp.iOS
{
	public partial class AllowanceViewController : MXPViewController
	{
		private Allowance allowance;
		private TableSectionsSource source;
		private NSObject _shownotification;
		private NSObject _hidenotification;
		private ExpenseItem expenseItem;
		private Actionables actionables;

		private UIImage _actionImage;
		private UIImage ActionImage {
			get {
				if (this._actionImage == null)
					this._actionImage = UIImage.FromFile ("navbar_overflow.png");

				return this._actionImage;
			}
		}

		public AllowanceViewController (Allowance allowance) : base ("AllowanceViewController", null) {
			this.allowance = allowance;
			this.expenseItem = this.allowance.ExpenseItems [0];
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.SetupActions ();

			this.Title = this.allowance.VDetailsBarTitle;
			this.fetchAndReload ();
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			this.configureTopBar ();

			this.allowance.PropertyChanged += HandlePropertyChanged;
			this.expenseItem.PropertyChanged += HandlePropertyChanged;

			_hidenotification = UIKeyboard.Notifications.ObserveDidHide (HideCallback);
			_shownotification = UIKeyboard.Notifications.ObserveWillShow (ShowCallback);
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);

			this.allowance.PropertyChanged -= HandlePropertyChanged;
			this.expenseItem.PropertyChanged -= HandlePropertyChanged;

			// Unregister the callbacks
			if (_shownotification != null)
				_shownotification.Dispose ();
			if (_hidenotification != null)
				_hidenotification.Dispose ();
		}

		void ShowCallback (object obj, UIKit.UIKeyboardEventArgs args) {
			this.TableView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, args.FrameEnd.Size.Height, 0.0f);
		}

		void HideCallback (object obj, UIKit.UIKeyboardEventArgs args) {
			this.TableView.ContentInset = new UIEdgeInsets (0.0f, 0.0f, 0.0f, 0.0f);
		}

		void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			this.configureTopBar ();

			if (e.PropertyName == "Populate") {
				this.TableView.ReloadData ();
			}
		}

		public void configureTopBar () {
			if (this.allowance.IsNew || this.allowance.IsChanged || this.expenseItem.IsChanged) {
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
						new ActionnablesWrapper(actionables, this, this.NavigationItem.RightBarButtonItem).show();
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
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.Close ();
		}

		public async void processSave () {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Saving) + "...");

			try {
				await this.allowance.SaveAsync ();
			} catch (Exception e) {
				MainNavigationController.Instance.showError (e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.Close ();
		}

		public async void processCancel () {
			if (!this.allowance.IsNew) {
				LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancelling));

				this.allowance.ResetChanged ();

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

		public async void fetchAndReload () {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Loading) + "...");

			try {
				await this.allowance.FetchAsync ();
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.source = new TableSectionsSource ();

			Collection<SectionSource> sections = new Collection<SectionSource> (){
				new AllowanceSegmentsSource (this.expenseItem.GetModelParent<ExpenseItem, Allowance> (), this.TableView, this),
				new SectionFieldsSource(this.expenseItem.GetAllFields (), this, Labels.GetLoggedUserLabel(Labels.LabelEnum.Details))
			};
			this.source.Sections = sections;
			this.TableView.Source = this.source;
			this.TableView.ReloadData ();
		}

		public void confirmDeleteExpense () {
			UIAlertView alert = new UIAlertView (Labels.GetLoggedUserLabel (Labels.LabelEnum.Delete) + " " + Labels.GetLoggedUserLabel (Labels.LabelEnum.Allowance), Labels.GetLoggedUserLabel (Labels.LabelEnum.DoYouConfirm), null, Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), new string [] { "OK" });
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
				await this.allowance.ExpenseItems [0].DeleteExpenseAsync ();
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.NavigationController.PopViewController (true);
		}
	}
}