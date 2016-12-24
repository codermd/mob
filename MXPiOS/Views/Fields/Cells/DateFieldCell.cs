
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class DateFieldCell : UITableViewCell
	{
		public DateFieldCell (IntPtr handle) : base (handle)
		{
		}

		public DataFieldCell _DataField;

		public override bool CanBecomeFirstResponder {
			get {
				return this._DataField.Field.IsEditable;
			}
		}

		public UIDatePicker _inputView;
		public override UIView InputView {
			get {
				if (this._inputView == null) {
					this._inputView = new UIDatePicker ();
					this._inputView.Mode = UIDatePickerMode.DateAndTime;
//					this._inputView.ValueChanged += (sender, e) => {
//
//					};
				}

				if (this._DataField.Field.extraInfo.ContainsKey ("Max-Range")) {
					DateTime date = (DateTime)this._DataField.Field.extraInfo ["Max-Range"];

					((UIDatePicker)this._inputView).MaximumDate = date.DateTimeToNSDate ();

				} else {
					((UIDatePicker)this._inputView).MaximumDate = null;
				}

				if (this._DataField.Field.extraInfo.ContainsKey ("Min-Range")) {
					DateTime date = (DateTime)this._DataField.Field.extraInfo ["Min-Range"];
					((UIDatePicker)this._inputView).MinimumDate = date.DateTimeToNSDate ();
				} else {
					((UIDatePicker)this._inputView).MinimumDate = null;
				}

				if (this._DataField.Field.extraInfo.ContainsKey ("Type")) {
					if (this._DataField.Field.extraInfo ["Type"].Equals ("DATE-TIME")) {
						((UIDatePicker)this._inputView).Mode = UIDatePickerMode.DateAndTime;
					}
					if (this._DataField.Field.extraInfo ["Type"].Equals ("DATE")) {
						((UIDatePicker)this._inputView).Mode = UIDatePickerMode.Date;
					}

					if (this._DataField.Field.extraInfo ["Type"].Equals ("TIME")) {
						((UIDatePicker)this._inputView).Mode = UIDatePickerMode.Time;
					}

				} else {

					if (this._DataField.Field.Type == FieldTypeEnum.Time) {
						((UIDatePicker)this._inputView).Mode = UIDatePickerMode.Time;
					} else {
						((UIDatePicker)this._inputView).Mode = UIDatePickerMode.Date;
					}
				}

				return this._inputView;
			}
		}

		public override bool BecomeFirstResponder ()
		{
			if (this._DataField.Field.Type == FieldTypeEnum.Time) {
				TimeSpan time = this._DataField.Field.GetValue<TimeSpan> ();
				((UIDatePicker)this.InputView).SetDate (new DateTime (2001, 1, 1, time.Hours, time.Minutes, time.Seconds).DateTimeToNSDate (), true);
			} else
				((UIDatePicker)this.InputView).SetDate (this._DataField.Field.GetValue<DateTime> ().DateTimeToNSDate (), true);

			return base.BecomeFirstResponder ();
		}

		private UIView _accessoryView;
		public override UIView InputAccessoryView {
			get {
				if (this._accessoryView == null) {
					DoneToolBar tb = new DoneToolBar ();
					this._accessoryView =  tb;
					tb.ClickOnButton += (sender, e) => {
						DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0) );
						newDate = newDate.AddSeconds(this._inputView.Date.SecondsSinceReferenceDate);
						if(newDate.IsDaylightSavingTime()) {
							newDate = newDate.AddHours(1);
						}

						if(this._DataField.Field.Type == FieldTypeEnum.Time) {
							this._DataField.Field.Value = new TimeSpan(newDate.Hour, newDate.Minute, 0);
						} else {
							this._DataField.Field.Value = newDate;
						}
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
			this.Title.Text = this._DataField.Field.VTitle;
			this.DateLabel.Text = this._DataField.Field.VValue;

		}
	

	}
}

