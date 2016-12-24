
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class HistoryDetailsViewController : MXPViewController
	{

		public ReportHistoryItem History;

		public HistoryDetailsViewController () : base ("HistoryDetailsViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.configure ();

			this.EdgesForExtendedLayout = UIRectEdge.None;
			this.AutomaticallyAdjustsScrollViewInsets = false;
		}

		public void configure(){
			this.TitleLabel.Text = this.History.Line;	
			this.CommentLabel.Text = this.History.Comment;
			this.DateLabel.Text = this.History.Date.ToLongDateString ();
			this.DateLabel.TextColor = UIColor.FromRGB(0,168,198);
		}
	}
}

