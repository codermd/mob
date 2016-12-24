using System;
using CoreGraphics;

using Foundation;
using UIKit;
using System.ComponentModel;

namespace Mxp.iOS
{
	public partial class IntegerFieldCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("IntegerFieldCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("IntegerFieldCell");

		private DoneToolBar tb;

		public IntegerFieldCell (IntPtr handle) : base (handle)
		{
		}

		public static IntegerFieldCell Create ()
		{
			return (IntegerFieldCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.InputText.ReturnKeyType = UIReturnKeyType.Next;

			this.InputText.ShouldReturn += (textField) => { 
				textField.ResignFirstResponder();
				return true; 
			};

			this.tb = new DoneToolBar ();
			this.tb.Frame = new CGRect (0, 0, 320, 44);
			tb.ClickOnButton += (sender, e) => {
				this.putValueOnField();
				this.InputText.ResignFirstResponder();
			};

			this.InputText.InputAccessoryView = tb;

			this.InputText.EditingDidEnd += (Object sender,EventArgs e) => {
				this.putValueOnField();
			};
		}

		public void putValueOnField () {
			if(string.IsNullOrEmpty(this.InputText.Text)){
				this._DataField.Field.Value = 0;
			} else {
				int convertToInt = this._DataField.Field.GetValue<int> ();

				try {
					convertToInt = Convert.ToInt32(this.InputText.Text);	
				} catch(Exception) {}

				this._DataField.Field.Value = convertToInt;
			}
		}

		public override bool CanBecomeFirstResponder {
			get {
				return this._DataField.Field.IsEditable;
			}
		}

		public DataFieldCell _DataField;

		public void SetField (DataFieldCell aField){

			this.unbind ();
			this._DataField =  aField as DataFieldCell;
			this._DataField.Field.FieldChanged += HandleFieldChange;
			this._DataField.Field.PropertyChanged += HandlePropertyChanged;
			this.refresh ();

		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName.Equals ("IsChanged"))
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
				this._DataField.Field.PropertyChanged -= HandlePropertyChanged;
			}
		}

		public void refresh() {
			this.TitleLabel.Text = this._DataField.Field.VTitle;
			this.InputText.Text = this._DataField.Field.VValue;
			this.InputText.Enabled = this._DataField.Field.IsEditable;

			if (this._DataField.Field.IsEditable) {
				this.InputText.BorderStyle = UITextBorderStyle.RoundedRect;
			} else {
				this.InputText.BorderStyle = UITextBorderStyle.None;
			}

		}
	}
}