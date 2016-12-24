using System;
using UIKit;
using Mxp.Core.Business;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Mxp.iOS
{
	public class ObservableTableViewSource<T> : UITableViewSource where T : Model
	{
		private ObservableCollection<T> Collection;
		private UITableView TableView;

		public ObservableTableViewSource (ObservableCollection<T> collection, UITableView tableView )
		{
			this.TableView = tableView;
			this.Collection = collection;

//			this.Collection.CollectionChanged  += new NotifyCollectionChangedEventHandler (
//				delegate(object sender, NotifyCollectionChangedEventArgs e) {
//					if(e.Action == NotifyCollectionChangedAction.Add){
//						this.TableView.BeginUpdates();
//						this.TableView.InsertRows();
//						this.TableView.EndUpdates();
//					}
//				});
//
			this.TableView.ReloadData ();
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return this.Collection.Count;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{

			return null;
		}

	}
}

