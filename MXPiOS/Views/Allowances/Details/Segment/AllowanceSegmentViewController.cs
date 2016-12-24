
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Mxp.iOS
{
	public partial class AllowanceSegmentViewController : MXPViewController
	{
		public AllowanceSegmentViewController () : base ("AllowanceSegmentViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		public AllowanceSegment segment;


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			Collection<TableSectionModel> sections = new Collection<TableSectionModel> ();
			sections.Add (new TableSectionModel (this.segment.GetAllowanceFields ()));

			this.TableView.Source = new SectionedFieldsSource (sections, this);
			this.TableView.ReloadData ();
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Segment);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (Labels.GetLoggedUserLabel (Labels.LabelEnum.Close), UIBarButtonItemStyle.Done, (sender, e) => {
					this.RecomputeResult ();
				}), true);
			}

		}

		public async void RecomputeResult() {
			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Loading) + "...");

			try {
				await this.segment.GetModelParent<AllowanceSegment, Allowance> ().RecalculateAsync ();
			} catch (Exception error) {
				MainNavigationController.Instance.showError (error);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			this.DismissViewController (true, null);
		}
	}
}