using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.ComponentModel;

namespace Mxp.iOS
{
	public partial class StringFieldCell : UITableViewCell {
		public static readonly UINib Nib = UINib.FromName ("StringFieldCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("StringFieldCell");

		private DoneToolBar tb;
		private DataFieldCell dataField;

		public StringFieldCell (IntPtr handle) : base (handle) {
		
		}

		public static StringFieldCell Create () {
			return (StringFieldCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib () {
			base.AwakeFromNib ();
			this.InputText.ReturnKeyType = UIReturnKeyType.Next;

			this.InputText.ShouldReturn += (textField) => {
				this.dataField.Field.Value = textField.Text;
				return true;
			};

			this.tb = new DoneToolBar ();
			this.tb.Frame = new CGRect (0, 0, 320, 44);
			tb.ClickOnButton += (sender, e) => {
				this.dataField.Field.Value = this.InputText.Text;
				this.InputText.ResignFirstResponder ();
			};

			this.InputText.InputAccessoryView = tb;

			this.InputText.EditingDidEnd += (Object sender, EventArgs e) => {
				if (!this.InputText.Text.Equals (this.dataField.Field.Value))
					this.dataField.Field.Value = this.InputText.Text;
			};
		}

		public override bool CanBecomeFirstResponder {
			get {
				return this.dataField.Field.IsEditable;
			}
		}

		public void SetField (DataFieldCell aField) {
			this.unbind ();
			this.dataField = aField as DataFieldCell;
			this.dataField.Field.FieldChanged += HandleFieldChange;
			this.dataField.Field.PropertyChanged += HandlePropertyChangedEvent;
			this.refresh ();
		}

		void HandleFieldChange (object sender, EventArgs e) {
			this.refresh ();
		}

		void HandlePropertyChangedEvent (object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName.Equals ("Loading"))
				this.refresh ();
		}

		protected override void Dispose (bool disposing) {
			this.unbind ();
			base.Dispose (disposing);
		}

		public void unbind () {
			if (this.dataField != null) {
				this.dataField.Field.FieldChanged -= HandleFieldChange;
				this.dataField.Field.PropertyChanged -= HandlePropertyChangedEvent;
			}
		}

		public void refresh () {
			if (this.dataField.Field.IsLoading)
				this.Loader.StartAnimating ();
			else
				this.Loader.StopAnimating ();
			
			this.Loader.Hidden = !this.dataField.Field.IsLoading;
			this.InputText.Hidden = this.dataField.Field.IsLoading;

			this.TitleLabel.Text = this.dataField.Field.VTitle;
			this.InputText.Text = this.dataField.Field.VValue;
			this.InputText.Enabled = this.dataField.Field.IsEditable;

			if (this.dataField.Field.IsEditable) {
				this.InputText.BorderStyle = UITextBorderStyle.RoundedRect;
			} else {
				this.InputText.BorderStyle = UITextBorderStyle.None;
			}

			this.InputText.Enabled = this.dataField.Field.IsEditable;
		}
	}
}