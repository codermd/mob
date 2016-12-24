using System;
using UIKit;

namespace Mxp.iOS
{
	public class SectionSource
	{

		public virtual string Title { get; set;}

		public int sectionNumber;

		public virtual UITableViewCell GetCell (UITableView tableView, int row) {
			return null;
		}

		public virtual int RowsInSection (UITableView tableview) {
			return  0;
		}

		public virtual float GetHeightForRow (UITableView tableView, int row) {
			return 44;
		}

		public virtual void RowSelected (UITableView tableView,  int row, UITableViewCell cell) {

		}

		public virtual UITableViewCellAccessory AccessoryForRow (UITableView tableView,int row) {
			return UITableViewCellAccessory.None;
		}

		public virtual bool CanEditRow (UITableView tableView,int row) {
			return false;
		}

		public virtual UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, int Row) {
			return UITableViewCellEditingStyle.None;
		}

		public virtual void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, int row) {

		}

		public virtual void  AccessoryButtonTapped (UITableView tableView, int row){

		}

	}
}

