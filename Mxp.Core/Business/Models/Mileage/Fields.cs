using System;
using Mxp.Core.Services;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public class MileageExpenseItemComment : Field
	{
		public MileageExpenseItemComment(ExpenseItem expenseItem) : base(expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Comment);
			this.Permission = Preferences.Instance.MILSIcomment == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.LongString;
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().Comment;
			}
			set {
				this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().Comment = (string)value;
				base.Value = value;
			}
		}
	}

	public class MileageExpenseItemProjectId : LookupField
	{
		public MileageExpenseItemProjectId(ExpenseItem expenseItem) : base(expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ProjectID);
			this.Permission = Preferences.Instance.MILSIprojectId == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
			this.LookupKey = LookupService.ApiEnum.GetLookupProject;
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ProjectId;
			}
			set {
				this.GetModel<ExpenseItem> ().ProjectId = ((LookupItem)value).ComboId;
				base.Value = value;
			}
		}
	}

	public class MileageExpenseItemDepartmentId : LookupField
	{
		public MileageExpenseItemDepartmentId(ExpenseItem expenseItem) : base(expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DepartmentID);
			this.Permission = Preferences.Instance.MILSIdepartmentId == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
			this.LookupKey = LookupService.ApiEnum.GetLookupDepartment;
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().DepartmentId;
			}
			set {
				this.GetModel<ExpenseItem> ().DepartmentId = ((LookupItem)value).ComboId;
				base.Value = value;
			}
		}
	}

	public class MileageExpenseItemTravelRequestId : LookupField
	{
		public MileageExpenseItemTravelRequestId(ExpenseItem expenseItem) : base(expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.TravelRequestID);
			this.Permission = Preferences.Instance.MILSItrqId == PermissionEnum.Optional ? FieldPermissionEnum.Optional : FieldPermissionEnum.Mandatory;
			this.LookupKey = LookupService.ApiEnum.GetLookupTravelRequests;
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().TravelRequestId;
			}
			set {
				this.GetModel<ExpenseItem> ().TravelRequestId = ((LookupItem)value).ComboId;
				base.Value = value;
			}
		}
	}

	public class CalculatedDistanceC : Field
	{
		public CalculatedDistanceC (Mileage mileage) : base (mileage) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.CalculatedDistance) + " (" + Preferences.Instance.MilUnit.GetString () + ")";
			this.Type = FieldTypeEnum.Integer;

			mileage.PropertyChanged += HandlePropertyChanged;
		}

		private void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "CalculatedDistance")
				this.NotifyPropertyChanged("IsChanged");
		}

		public static bool CanCreate (Mileage mileage) {
			return Preferences.Instance.MILMap == PermissionEnum.Mandatory;
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}

		public override string VValue {
			get {
				return this.GetModel<Mileage> ().CalculatedDistance.ToString ();
			}
		}

		public override object Value {
			get {
				return this.GetModel<Mileage> ().CalculatedDistance;
			}
		}
	}

	public class BusinessDistanceC : Field
	{
		public BusinessDistanceC (Mileage mileage) : base (mileage) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.BusinessDistance) + " (" + Preferences.Instance.MilUnit.GetString () + ")";
			this.Type = FieldTypeEnum.Integer;

			mileage.PropertyChanged += HandlePropertyChanged;
		}

		private void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "BusinessDistance")
				this.NotifyPropertyChanged("IsChanged");
		}

		public static bool CanCreate (Mileage mileage) {
			return Preferences.Instance.MILMap == PermissionEnum.Mandatory;
		}

		public override bool IsEditable {
			get {
				if (this.GetModel<Mileage> ().IsEditable && LoggedUser.Instance.Preferences.MILDefaultProduct != Preferences.ProductTypeEnum.Business)
					return true;
				
				if (LoggedUser.Instance.Preferences.MILBusinessMilesReadOnly == Preferences.BusinessMilesReadOnlyEnum.Editable)
					return true;
				
				return false;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Mileage> ().BusinessDistance;
			}
			set {
				this.GetModel<Mileage> ().BusinessDistance = (int) value;
				base.Value = value;
			}
		}
			
		public override string VValue {
			get {
				return this.GetModel<Mileage> ().BusinessDistance.ToString ();
			}
		}
	}

	public class CommutingDistanceC : Field
	{
		public CommutingDistanceC (Mileage mileage) : base (mileage) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Commute)  + " (" + Preferences.Instance.MilUnit.GetString () + ")";
			this.Type = FieldTypeEnum.Integer;

			mileage.PropertyChanged += HandlePropertyChanged;
		}

		private void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "CommuteDistance")
				this.NotifyPropertyChanged("IsChanged");
		}

		public static bool CanCreate (Mileage mileage) {
			return mileage.CanDisplayCommutePrivate (Preferences.Instance.MILCommuting, Preferences.Instance.MILCommutingOtherValue)
				&& Preferences.Instance.MILMap == PermissionEnum.Mandatory;
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Mileage> ().IsEditable
					&& Preferences.Instance.MILWithdrawCommuting != VisibilityEnum.Show
					&& Preferences.Instance.MILCommuting != VisibilityEnum.ReadOnly
					&& Preferences.Instance.MILDefaultProduct != Preferences.ProductTypeEnum.Default;
			}
		}
			
		public override object Value {
			get {
				return this.GetModel<Mileage> ().CommuteDistance;
			}
			set {
				this.GetModel<Mileage> ().CommuteDistance = (int)value;
				base.Value = value;
			}
		}

		public override string VValue {
			get {
				return this.GetModel<Mileage> ().CommuteDistance.ToString();
			}
		}
	}

	public class PrivateDistanceC : Field
	{
		public PrivateDistanceC (Mileage mileage) : base (mileage) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Private) + " (" + Preferences.Instance.MilUnit.GetString () + ")";
			this.Type = FieldTypeEnum.Integer;

			mileage.PropertyChanged += HandlePropertyChanged;
		}
			
		private void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "PrivateDistance")
				this.NotifyPropertyChanged("IsChanged");
		}

		public static bool CanCreate (Mileage mileage) {
			return mileage.CanDisplayCommutePrivate (Preferences.Instance.MILPrivate, Preferences.Instance.MILPrivateOtherValue)
				&& Preferences.Instance.MILMap == PermissionEnum.Mandatory;
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Mileage> ().IsNew || this.GetModel<Mileage> ().IsEditable;
			}
		}
			
		public override string VValue {
			get {
				return this.GetModel<Mileage> ().PrivateDistance.ToString();
			}
		}

		public override object Value {
			get {
				return this.GetModel<Mileage> ().PrivateDistance;
			}
			set {
				this.GetModel<Mileage> ().PrivateDistance = (int)value;
				base.Value = value;
			}
		}
	}

	public class OdometerFromC : Field
	{
		public OdometerFromC (Mileage mileage) : base (mileage) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.OdometerFrom);
			this.Type = FieldTypeEnum.Integer;
		}

		public static bool CanCreate (Mileage mileage) {
			return mileage.CanDisplayOdometer;
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Mileage> ().IsNew || this.GetModel<Mileage> ().IsEditable;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Mileage> ().OdometerFrom;
			}
			set {
				this.GetModel<Mileage> ().OdometerFrom = (int) value;
				base.Value = value;
			}
		}

		public override string VValue {
			get {
				return this.GetModel<Mileage> ().OdometerFrom.ToString();
			}
		}
	}

	public class OdometerToC : Field
	{
		public OdometerToC (Mileage mileage) : base (mileage) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.OdometerTo);
			this.Type = FieldTypeEnum.Integer;

			mileage.PropertyChanged += HandlePropertyChanged;
		}

		private void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "OdometerTo")
				this.NotifyPropertyChanged("IsChanged");
		}

		public static bool CanCreate (Mileage mileage){
			return mileage.CanDisplayOdometer;
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Mileage> ().IsNew || this.GetModel<Mileage> ().IsEditable;
			}
		}

		public override string VValue {
			get {
				return this.GetModel<Mileage> ().OdometerTo.ToString();
			}
		}

		public override object Value {
			get {
				return this.GetModel<Mileage> ().OdometerTo;
			}
			set {
				this.GetModel<Mileage> ().OdometerTo = (int) value;
				base.Value = value;
			}
		}
	}

	public class DateC : Field
	{
		public DateC (Mileage mileage) : base (mileage) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Date);
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Date;
			this.extraInfo ["Max-Range"] = DateTime.Now;
		}

		public static bool CanCreate (Mileage mileage) {
			return true;
		}

		public override string VValue {
			get {
				return this.GetValue<DateTime> ().ToString ("d");
			}
		}

		public override object Value {
			get {
				return this.GetModel<Mileage> ().Date.Value;
			}
			set {
				this.GetModel<Mileage>().Date = (DateTime) value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Mileage> ().IsNew || this.GetModel<Mileage> ().IsEditable;
			}
		}
	}

	public class VehicleC : ComboField
	{
		public VehicleC (Mileage mileage) : base (mileage) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Vehicle);
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Combo;
		}

		public override Collection<IComboChoice> Choices {
			get {
				if (base.Choices == null)
					base.Choices = new Collection<IComboChoice> ();

				base.Choices.Clear ();

				this.GetModel<Mileage> ().Vehicles.ForEach (v => base.Choices.Add (v));

				return base.Choices;
			}
			set {
				base.Choices = value;
			}
		}

		public static bool CanCreate (Mileage mileage) {
			return !(mileage.IsFromApproval && mileage.Vehicle == null);
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Mileage> ().IsNew || this.GetModel<Mileage> ().IsEditable;
			}
		}
			
		public override string VValue {
			get {
				if (this.GetValue<Vehicle> () == null)
					return "";

				return this.GetValue<Vehicle> ().LicensePlateNumber;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Mileage> ().Vehicle;
			}
			set {
				this.GetModel<Mileage>().Vehicle = value as Vehicle;
				base.Value = value;
			}
		}
	}
		
	public class VehicleCategoryC : ComboField
	{
		public VehicleCategoryC (Mileage mileage) : base (mileage) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.VehicleCategory);
			this.Type = FieldTypeEnum.Combo;

			Collection<IComboChoice> choices =  new Collection<IComboChoice>();

			LoggedUser.Instance.VehicleCategories.ForEach (v => {
				choices.Add(v);
			});

			this.Choices = choices;
		}

		public static bool CanCreate (Mileage mileage) {
			return mileage.Vehicle != null
				&& (Preferences.Instance.MILVehicle == VisibilityEnum.Show
					|| (Preferences.Instance.MILVehicle == VisibilityEnum.Other
						&& (mileage.Vehicle != null
							&& !Preferences.Instance.MILVehicleOtherValue.Contains (mileage.Vehicle.Ownership))));
		}

		public override bool IsEditable {
			get {
				return this.GetModel<Mileage> ().IsNew || this.GetModel<Mileage> ().IsEditable;
			}
		}

		public override string VValue {
			get {
				return this.GetValue<VehicleCategory> ().VTitle;
			}
		}

		public override object Value {
			get {
				return this.GetModel<Mileage>().VehicleCategory;
			}
			set {
				this.GetModel<Mileage>().VehicleCategory = value as VehicleCategory;
				base.Value = value;
			}
		}
	}
}