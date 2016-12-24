using System;
using System.Collections.ObjectModel;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class SectionFieldsSource : SectionSource
	{

		public Collection<DataFieldCell> DataFields;

		public override string Title { get; set;}



		private WeakReference<UIViewController> weakViewController;
		public UIViewController viewController {
			get {
				UIViewController target;
				this.weakViewController.TryGetTarget (out target);
				return target;
			}
			private set {
				this.weakViewController = new WeakReference<UIViewController> (value);
			}
		}


		public SectionFieldsSource (Collection<Field> fields, UIViewController viewController, string title)
		{
			this.DataFields = FieldFactory.WrapFields (fields);
			this.viewController = viewController;
			this.Title = title;
		}

		public SectionFieldsSource (TableSectionModel model, UIViewController viewController) : this (model.Fields, viewController, model.Title)
		{
		}

		public override UITableViewCell GetCell (UITableView tableView, int row){
			UITableViewCell cell =  this.DataFields [row].GetCell (tableView);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;//UITableViewCellSelectionStyleNone;
			return cell;
		}


		public override int RowsInSection (UITableView tableview){
			return this.DataFields.Count;
		}

		public override float GetHeightForRow (UITableView tableView, int row)
		{
			return this.DataFields[row].HeightForCell();
		}

		public override void RowSelected (UITableView tableView,  int row, UITableViewCell cell){
			this.DataFields [row].FieldSelected (this.viewController, tableView, cell);
		}

		public override UITableViewCellAccessory AccessoryForRow (UITableView tableView,int row) {
			return this.DataFields [row].accessory;
		}

		public override bool CanEditRow (UITableView tableView,int row) {
			return false;
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, int row)
		{
		}

		public override void AccessoryButtonTapped (UITableView tableView, int row) {
			this.DataFields [row].AccessoryButtonTapped(this.viewController, tableView);
		}

	}
}