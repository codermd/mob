using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class IconsLegentTableViewController : MXPTableViewController
	{
		public IconsLegentTableViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			this.TableView.Source = new IconLegendsTableSource ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.IconsLegend);
		}
	}
}