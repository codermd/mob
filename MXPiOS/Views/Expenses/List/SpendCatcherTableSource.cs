using System;
using UIKit;
using Mxp.Core;
using Mxp.Core.Business;
using SDWebImage;
using Foundation;
using CoreGraphics;
using System.Drawing;

namespace Mxp.iOS
{
	public class SpendCatcherTableSource : UITableViewSource
	{
		public event EventHandler spendCatcherSelected = delegate {};

		private SpendCatcherExpenses SpendCatcherExpenses;

		public SpendCatcherTableSource (SpendCatcherExpenses spendCatcherExpenses) {
			this.SpendCatcherExpenses = spendCatcherExpenses;

			title.Lines = 0;
			title.LineBreakMode = UILineBreakMode.WordWrap;
			title.SizeToFit ();
			title.TextColor = UIColor.LightGray;
			title.Font = UIFont.FromName ("Avenir", 15);
			title.BackgroundColor = UIColor.White;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (section == 1) {
				return this.SpendCatcherExpenses.Count;
			}
			return 0;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 80;
		}

		private UILabel title =new PaddingLabel();

		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			if (section == 0) {
				var nib = UINib.FromName ("SpendCatcherHeaderCount", NSBundle.MainBundle);
				UIView view = nib.Instantiate (null, null) [0] as UIView;
				((UILabel)view.Subviews[0]).Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.SpendCatcher);
				((UILabel)view.Subviews[1]).Text = this.SpendCatcherExpenses.Count.ToString();
				return view;
			}

			title.Text = Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcherHeaderMessage);
			return title;
		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			if (section == 1) {
				CGSize maxHeight = new CGSize(tableView.Frame.Width, float.MaxValue);
				NSString nsstr = new NSString (Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcherHeaderMessage));
				var size = nsstr.StringSize (title.Font, maxHeight, title.LineBreakMode);
				return size.Height;
			}

			return 21;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 2;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			SpendCatcherExpenseCell cell = tableView.DequeueReusableCell("SpendCatcherExpenseCell") as SpendCatcherExpenseCell;
			if (cell == null) {
				cell = SpendCatcherExpenseCell.Create();
			}
			cell.Configure (this.SpendCatcherExpenses [indexPath.Row]);
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			SpendCatcherExpense spendcatcher = this.SpendCatcherExpenses [indexPath.Row];
			spendCatcherSelected (spendcatcher, EventArgs.Empty);
		}

		class PaddingLabel : UILabel
		{

			public PaddingLabel() : base(){}

			public override void DrawText (CGRect rect)
			{
				var Insets = new UIEdgeInsets (0, 5, 0, -5);
				var padded = new RectangleF((float)(rect.X + Insets.Left), (float)rect.Y, (float)(rect.Width + Insets.Left + Insets.Right), (float)rect.Height);
				base.DrawText (padded);
			}
		}
	}
}

