using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class CurrencyFieldCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("CurrencyLabel", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("CurrencyLabel");

		public static readonly UITableViewCellAccessory accessory =  UITableViewCellAccessory.None;

		public CurrencyFieldCell (IntPtr handle) : base (handle)
		{
		}

		public static CurrencyFieldCell Create ()
		{
			return (CurrencyFieldCell)Nib.Instantiate (null, null) [0];
		}

		public DataFieldCell _DataField;

		public void SetField (DataFieldCell aField){

			this.unbind ();
			this._DataField =  aField as DataFieldCell;
			this._DataField.Field.FieldChanged += HandleFieldChange;
			this.refresh ();
		}

		void HandleFieldChange (object sender, EventArgs e)
		{
			this.refresh ();
		}

		protected override void Dispose (bool disposing)
		{
			this.unbind ();
			base.Dispose (disposing);
		}

		public void unbind(){
			if (this._DataField != null) {
				this._DataField.Field.FieldChanged -= HandleFieldChange;
			}
		}

		public void refresh() {
			this.CurrencyTitle.Text = this._DataField.Field.VTitle;
			this.CurrencyValue.Text = this._DataField.Field.VValue;
		}
	}
}