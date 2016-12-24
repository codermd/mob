using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using System.Collections.ObjectModel;

namespace Mxp.iOS
{
	public partial class ComboFieldCell : UITableViewCell
	{
		public ComboFieldCell (IntPtr handle) : base (handle)
		{
		}

		public DataFieldCell _DataField;

		private ComboField ComboField {
			get { 
				return this._DataField.Field as ComboField;
			}
		}


		public override bool CanBecomeFirstResponder {
			get {
				return this._DataField.Field.IsEditable;
			}
		}

		public UIPickerView _inputView;
		public override UIView InputView {
			get {
				if (this._inputView == null) {
					this._inputView = new UIPickerView ();

				}

				this._inputView.Model = this.Model;
				this._inputView.ReloadAllComponents();

				return this._inputView;
			}
		}

		private PickerModel model;
		private PickerModel Model {
			get {
				if (this.model == null) {
					this.model = new PickerModel (this.ComboField);
					this.model.cellSelected += (sender, e) => {
						this.ComboField.Value = e.selectedChoice;
					};
				}
				return this.model;

			}
		}

		private class PickerModel : UIPickerViewModel
		{

			//Event management
			public class SelectedEventArgs : EventArgs
			{
				public Mxp.Core.Business.ComboField.IComboChoice selectedChoice { get; set;}
			}

			// Starting off with an empty handler avoids pesky null checks
			public event EventHandler<SelectedEventArgs> cellSelected = delegate {};

			private ComboField comboField;

			public PickerModel(ComboField comboField){
				this.comboField = comboField;
			}

			public override string GetTitle (UIPickerView picker, nint row, nint component)
			{
				return this.comboField.Choices [(int)row].VTitle;
			}


			public override nint GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				return this.comboField.Choices.Count;
			}

			public override void Selected (UIPickerView picker, nint row, nint component)
			{
				SelectedEventArgs e = new SelectedEventArgs ();
				e.selectedChoice = this.comboField.Choices [(int)row];
				this.cellSelected (this, e);
			}


		}



		private UIView _accessoryView;
		public override UIView InputAccessoryView {
			get {
				if (this._accessoryView == null) {
					DoneToolBar tb = new DoneToolBar ();
					this._accessoryView =  tb;
					tb.ClickOnButton += (sender, e) => {
						this.ResignFirstResponder();
					};
					this._accessoryView.Frame = new CGRect (0, 0, 320, 44);
				}
				return this._accessoryView;
			}
		}


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
			this.ValueLabel.Text = this._DataField.Field.VValue;
		}


	}
}

