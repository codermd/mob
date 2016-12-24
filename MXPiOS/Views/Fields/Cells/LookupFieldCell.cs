using System;
using CoreGraphics;

using Foundation;
using UIKit;
using System.Threading.Tasks;
using Mxp.Core.Business;
using Mxp.Core;

namespace Mxp.iOS
{
	public partial class LookupFieldCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("LookupFieldCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("LookupFieldCell");

		public static readonly UITableViewCellAccessory accessory =  UITableViewCellAccessory.None;


		private LookupField LookupField;

		public LookupFieldCell (IntPtr handle) : base (handle)
		{
		}

		public static LookupFieldCell Create ()
		{
			return (LookupFieldCell)Nib.Instantiate (null, null) [0];
		}

		public void SetField (DataFieldCell aField){

			this.TitleLabel.Text = aField.Field.VTitle;

			this.unbind ();

			this.LookupField = (LookupField)aField.Field;

			this.LookupField.LookupChanged += loadedItem;

			((Field)this.LookupField).FieldChanged += HandlelookupLoadingHandler;

			this.LookupField.FetchLookupItem ();

			this.refreshValue ();
		}

		void loadedItem (object sender, EventArgs e)
		{
			this.refreshValue ();
		}

		public void unbind() {
			if (this.LookupField != null) {
				this.LookupField.FieldChanged -= HandlelookupLoadingHandler;
				this.LookupField.LookupChanged -= loadedItem;
			}
		}

		void HandlelookupLoadingHandler (object sender, EventArgs e)
		{
			this.refreshValue ();
		}

		protected override void Dispose (bool disposing)
		{
			this.unbind ();
			base.Dispose (disposing);
		}

		public void refreshValue() {
			this.ValueLabel.Text = this.LookupField.VValue;
		}
	}
}

