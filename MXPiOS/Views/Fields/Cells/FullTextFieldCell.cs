using System;
using Foundation;
using UIKit;

namespace Mxp.iOS
{
	public partial class FullTextFieldCell : UITableViewCell {
		public static readonly UINib Nib = UINib.FromName ("FullTextFieldCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("FullTextFieldCell");

		public DataFieldCell _DataField;

		public FullTextFieldCell (IntPtr handle) : base (handle) {

		}

		public static FullTextFieldCell Create () {
			return (FullTextFieldCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib () {
			this.TitleLabel.PreferredMaxLayoutWidth = (nfloat)280;
		}

		public void SetField (DataFieldCell aField) {
			this.Unbind ();
			this._DataField = aField;
			this._DataField.Field.FieldChanged += HandleFieldChanged;
			this.Refresh ();
		}

		private void HandleFieldChanged (object sender, EventArgs e) {
			this.Refresh ();
		}

		protected override void Dispose (bool disposing) {
			this.Unbind ();

			base.Dispose (disposing);
		}

		public void Unbind () {
			if (this._DataField != null)
				this._DataField.Field.FieldChanged -= HandleFieldChanged;
		}

		public void Refresh () {
			this.TitleLabel.Text = this._DataField.Field.VTitle;
			this.TextLabel.Text = this._DataField.Field.VValue;
		}
	}
}