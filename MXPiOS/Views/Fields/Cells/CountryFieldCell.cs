using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class CountryFieldCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("CountryFieldCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("CountryFieldCell");

		public static readonly UITableViewCellAccessory accessory =  UITableViewCellAccessory.None;

		public CountryFieldCell (IntPtr handle) : base (handle)
		{
		}

		public static CountryFieldCell Create ()
		{
			return (CountryFieldCell)Nib.Instantiate (null, null) [0];
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
			this.TitleLabel.Text = this._DataField.Field.VTitle;
			this.CountryLabel.Text = this._DataField.Field.VValue;
		}
	}
}