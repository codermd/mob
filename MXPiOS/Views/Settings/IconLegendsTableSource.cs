using System;
using CoreGraphics;

using Foundation;
using UIKit;
using ObjCRuntime;
using Mxp.Core.Business;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using Mxp.Core.Helpers;

namespace Mxp.iOS
{
	public class IconLegendsTableSource : UITableViewSource
	{
		private List<IconsLegend> iconsLegendList {
			get {
				return IconsLegend.All;
			}
		}

		public override nint NumberOfSections (UITableView tableView) {
			return this.iconsLegendList.Count;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			return (nint)this.iconsLegendList [(int)section].IconsLegendList.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			IconLegend iconLegend = this.iconsLegendList [indexPath.Section].IconsLegendList [indexPath.Row];

			IconLegendCell cell = tableView.DequeueReusableCell ("IconLegendCell") as IconLegendCell;

			if (cell == null) {
				NSArray views = NSBundle.MainBundle.LoadNib ("IconLegendCell", cell, null);
				cell = Runtime.GetNSObject (views.ValueAt (0)) as IconLegendCell;
			}

			cell.IconLegend = iconLegend;

			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath) {
			return 80;
		}

		public override string TitleForHeader (UITableView tableView, nint section) {
			return this.iconsLegendList [(int)section].Title;
		}
	}
}