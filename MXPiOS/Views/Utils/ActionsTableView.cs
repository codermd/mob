using System;
using UIKit;
using Mxp.Core.Utils;

namespace Mxp.iOS
{
	public class ActionsTableView : MXPTableViewController
	{

		public class ActionSelectedEventArgs : EventArgs
		{
			public Actionable selectedAction { get; set;}
		}


		// Starting off with an empty handler avoids pesky null checks
		public event EventHandler<ActionSelectedEventArgs> actionSelected = delegate {};


		private Actionables actionables;
		public ActionsTableView(Actionables actionables): base(UITableViewStyle.Plain) {
			this.actionables = actionables;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.source = new Source (this.actionables);
			this.TableView.Source = source;
		}

		private Source source;
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.source.actionSelected+= HandleactionSelected;

			this.TableView.ShowsHorizontalScrollIndicator = false;
			this.TableView.ShowsVerticalScrollIndicator = false;

		}

		void HandleactionSelected (object sender, ActionSelectedEventArgs e)
		{
			this.actionSelected(this, e);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			this.source.actionSelected -= HandleactionSelected;
		}

		private class Source :UITableViewSource 
		{

			// Starting off with an empty handler avoids pesky null checks
			public event EventHandler<ActionSelectedEventArgs> actionSelected = delegate {};

			private Actionables actionables;

			public Source(Actionables actionables): base() {
				this.actionables = actionables;
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return this.actionables.Actions.Count;
			}
			public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
			{
				DefaultCell cell = (DefaultCell)tableView.DequeueReusableCell (DefaultCell.Key);

				if ( cell == null) {
					cell = DefaultCell.Create();
				}
				cell.TextLabel.Text = this.actionables.Actions [indexPath.Row].Title;
				return cell;
			}

			public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
			{
				ActionSelectedEventArgs e = new ActionSelectedEventArgs ();
				e.selectedAction = this.actionables.Actions [indexPath.Row];
				this.actionSelected (this, e);
			}
		} 

	}
}

