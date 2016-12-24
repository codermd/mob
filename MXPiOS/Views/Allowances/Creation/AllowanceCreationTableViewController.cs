using System;
using UIKit;
using Mxp.Core.Business;
using Foundation;

namespace Mxp.iOS
{
	public class AllowanceCreationTableViewController : MXPTableViewController
	{
		public AllowanceCreationTableViewController () : base(UITableViewStyle.Plain)
		{
		}

		private Allowance Allowance;

		private SectionedFieldsSource  source;




		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Allowance = Allowance.NewInstance ();

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.NewAllowance);

			this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.source = new SectionedFieldsSource (this.Allowance.CreationAllowanceSectionFields, this);
			this.TableView.Source = this.source;
			this.TableView.ReloadData ();

			this.NavigationItem.SetHidesBackButton (true, false);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			this.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), UIBarButtonItemStyle.Plain, (sender, e) => {
				if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					this.DismissViewController(true, null);
				} else {
					this.NavigationController.PopViewController(true);
				}

			}), true);

			this.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Done), UIBarButtonItemStyle.Done, (sender, e) => {
				this.saveProcess();
			}), true);
		}

		public async void saveProcess(){
			LoadingView.showMessage(Labels.GetLoggedUserLabel (Labels.LabelEnum.Preparing) + "...");

			try {
				await this.Allowance.CreateAsync ();
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			AllowanceViewController vc = new AllowanceViewController (this.Allowance);
			UIViewController[] viewControllers = new UIViewController[]{this.NavigationController.ViewControllers[0], vc};

			this.NavigationController.SetViewControllers (viewControllers, true);
		}
	}
}