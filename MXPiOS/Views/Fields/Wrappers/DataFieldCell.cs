using System;
using Mxp.Core.Business;
using Mxp.Core;
using UIKit;
using Foundation;

namespace Mxp.iOS
{
	public class DataFieldCell
	{


		public Field Field;

		public DataFieldCell (Field field)
		{
			this.Field = field;
		}

		public virtual UITableViewCellAccessory accessory { get; set;} = UITableViewCellAccessory.None;

		public virtual void FieldSelected(UIViewController viewController, UITableView tableView, UITableViewCell cell){

		}

		public virtual int HeightForCell(){
			return 44;
		}

		public virtual UITableViewCell GetCell(UITableView tableView){
			UITableViewCell cell = tableView.DequeueReusableCell ("Default");


			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, "Default");
			}

			cell.TextLabel.Text = this.Field.VTitle;
			cell.DetailTextLabel.Text = this.Field.VValue;

			return cell;
		}

		public T GenerateCellWithNibName<T>(string nibName, UITableView tableView) where T : UITableViewCell{
			T cell =  (T)tableView.DequeueReusableCell (nibName);
			if (cell == null) {
				UIView view = this.generateFromNibName (nibName);
				cell = (T)view;
			}
			return cell;
		}

		private UIView generateFromNibName(string nibName){
			return (UIView)UINib.FromName (nibName, NSBundle.MainBundle).Instantiate (null, null) [0];
		}

		public virtual void AccessoryButtonTapped (UIViewController viewController,UITableView tableview){

		}

	}
}

