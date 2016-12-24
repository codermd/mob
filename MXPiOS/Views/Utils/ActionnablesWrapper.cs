using System;
using Mxp.Core.Utils;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class ActionnablesWrapper
	{

		private Actionables actionables;
		private UIViewController container;
		private UIView containerView;
		private UIBarButtonItem buttonItem;

		public ActionnablesWrapper (Actionables actionables, UIViewController container, UIView containerView = null)
		{
			this.actionables = actionables;
			this.container = container;
			this.containerView = containerView;
		}

		public ActionnablesWrapper (Actionables actionables, UIViewController container, UIBarButtonItem buttonItem)
		{
			this.actionables = actionables;
			this.container = container;
			this.buttonItem = buttonItem;
		}

		public void show()  {
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				//Show in popover
				this.showForIPad ();
			} else {
				this.showForIphone ();
			}
		}
		private void showForIPad() {

			var vc = new ActionsTableView (this.actionables);
			UIPopoverController popover = new UIPopoverController (vc);
			vc.actionSelected += (object sender, ActionsTableView.ActionSelectedEventArgs e) => {
				popover.Dismiss(true);
				e.selectedAction.Action();
			};

			vc.TableView.ReloadData ();
			popover.SetPopoverContentSize (new CoreGraphics.CGSize(320, (int)(vc.TableView.ContentSize.Height*1.2f)), true);

			if (this.buttonItem != null) {
				popover.PresentFromBarButtonItem (this.buttonItem, UIPopoverArrowDirection.Any, true);
				return;
			}

			popover.PresentFromRect (this.containerView.Frame, this.containerView.Superview, UIPopoverArrowDirection.Any, true);
		}
		private void showForIphone() {

			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 0)) {

				UIActionSheet actionSheet = new UIActionSheet (this.actionables.Title);

				this.actionables.Actions.ForEach (action => {
					actionSheet.Add(action.Title);

				});
				actionSheet.AddButton (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Cancel));

				
				actionSheet.Clicked += (object sender, UIButtonEventArgs e) => {
					foreach(Actionable action in this.actionables.Actions){
						if(action.Title.Equals (actionSheet.ButtonTitle (e.ButtonIndex))) {
							action.Action();
						}
					};
				};
				actionSheet.ShowInView (this.container.View.Window);

			} else {

				UIAlertController alert = UIAlertController.Create(this.actionables.Title, "", UIAlertControllerStyle.ActionSheet);

				this.actionables.Actions.ForEach (action => {
					alert.AddAction (UIAlertAction.Create (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Recent), UIAlertActionStyle.Default, (sender) => {
						action.Action();
					}));
				});
				alert.AddAction (UIAlertAction.Create (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Cancel), UIAlertActionStyle.Cancel, null));
				this.container.PresentViewController (alert, true, null);
			}


		}

	}
}