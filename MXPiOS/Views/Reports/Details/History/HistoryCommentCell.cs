using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using CoreGraphics;

namespace MXPiOS
{
	public partial class HistoryCommentCell : UITableViewCell
	{

		public static int MinimumCellSize = 58;

		public static readonly NSString Key = new NSString ("HistoryCommentCell");
		public static readonly UINib Nib;

		static HistoryCommentCell ()
		{
			Nib = UINib.FromName ("HistoryCommentCell", NSBundle.MainBundle);
		}

		public HistoryCommentCell (IntPtr handle) : base (handle)
		{
		}

		public static HistoryCommentCell Create ()
		{
			return (HistoryCommentCell)Nib.Instantiate (null, null) [0];
		}

		public ReportHistoryItem ReportHistoryItem {
			set {
				CGSize maxHeight = new CGSize (320, float.MaxValue);

				if (String.IsNullOrEmpty (value.Line))
					TitleHeightConstraint.Constant = 0;
				else {
					NSString nsstr = (new NSString (value.Line));
					var size = nsstr.StringSize (this.TitleLabel.Font, maxHeight, this.TitleLabel.LineBreakMode);
					TitleHeightConstraint.Constant = size.Height;
					this.TitleLabel.Text = value.Line;
				}

				if (String.IsNullOrEmpty (value.Comment))
					CommentHeightConstraint.Constant = 0;
				else {
					NSString nsstr = (new NSString (value.Comment));
					var size = nsstr.StringSize (this.CommentLabel.Font, maxHeight, this.CommentLabel.LineBreakMode);
					CommentHeightConstraint.Constant = size.Height;
					this.CommentLabel.Text = value.Comment;
				}

				this.DateLabel.TextColor = UIColor.FromRGB (0, 168, 198);
				this.DateLabel.Text = value.Date.ToLongDateString ();
			}
		}

		public nfloat computeSize(ReportHistoryItem ReportHistoryItem, nfloat width){
			nfloat res = 0;

			CGSize maxHeight = new CGSize (width, float.MaxValue);

			if (!String.IsNullOrEmpty (ReportHistoryItem.Line)) {
				NSString nsstr = (new NSString (ReportHistoryItem.Line));
				var size = nsstr.StringSize (this.TitleLabel.Font, maxHeight, this.TitleLabel.LineBreakMode);
				res += size.Height;
			}

			if (!String.IsNullOrEmpty (ReportHistoryItem.Comment)) {
				NSString nsstr = (new NSString (ReportHistoryItem.Comment));
				var size = nsstr.StringSize (this.CommentLabel.Font, maxHeight, this.CommentLabel.LineBreakMode);
				res += size.Height;
			}

			this.DateLabel.TextColor = UIColor.FromRGB (0, 168, 198);
			this.DateLabel.Text = ReportHistoryItem.Date.ToLongDateString ();
			res += 30;

			return res;
		}

		public UILabel TheCommentLabel { 
			get { 
				return this.CommentLabel;
			}
		}

	}
}
