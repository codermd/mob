using System;
using UIKit;
using System.Collections.ObjectModel;
using Foundation;
using System.Collections.Generic;

namespace Mxp.iOS
{
	public class TableSectionsSource : UITableViewSource
	{

		public Collection<SectionSource> Sections = new Collection<SectionSource>();

		private bool isEmpty(){
			return this.Sections.Count == 0;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			if (this.isEmpty()) {
				//Empty cell
				return 1;
			}
			return Sections.Count;
		}
		public override string TitleForHeader (UITableView tableView, nint section)
		{
			if (this.isEmpty()) {
				//Empty cell
				return null;
			}
			return this.Sections [(int)section].Title;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (this.isEmpty()) {
				//Empty cell
				return 1;
			}
			this.Sections [(int)section].sectionNumber = (int)section;
			return this.Sections [(int)section].RowsInSection (tableview);
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (this.isEmpty()) {
				EmptyCell cell = (EmptyCell)tableView.DequeueReusableCell (EmptyCell.Key);
				if (cell == null) {
					cell = EmptyCell.Create ();
				}
				return cell;
			}
			return this.Sections [indexPath.Section].GetCell (tableView, indexPath.Row);
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (this.isEmpty()) {
				return 44;
			}

			return this.Sections [indexPath.Section].GetHeightForRow (tableView, indexPath.Row);
		}


		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (this.isEmpty()) {
				return;
			}
			this.Sections [indexPath.Section].RowSelected (tableView, indexPath.Row, tableView.CellAt (indexPath));
		}

		public override UITableViewCellAccessory AccessoryForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (this.isEmpty()) {
				return UITableViewCellAccessory.None;
			}
			return this.Sections [indexPath.Section].AccessoryForRow (tableView, indexPath.Row);
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (this.isEmpty()) {
				return false;
			}
			return this.Sections [indexPath.Section].CanEditRow(tableView, indexPath.Row);
		}


		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if (this.isEmpty()) {
				return;
			}
			this.Sections [indexPath.Section].CommitEditingStyle (tableView, editingStyle, indexPath.Row);
		}


		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return this.Sections [indexPath.Section].EditingStyleForRow (tableView, indexPath.Row);
		}


		public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
		{
			this.Sections [indexPath.Section].AccessoryButtonTapped (tableView, indexPath.Row);
		}
	}
}

