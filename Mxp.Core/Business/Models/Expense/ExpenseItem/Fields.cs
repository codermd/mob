using System;
using Mxp.Core.Services;
using Mxp.Core.Services.Google;
using System.Threading.Tasks;

namespace Mxp.Core.Business
{
	public class ExpenseItemCreditCard : Field
	{
		public ExpenseItemCreditCard (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.CreditCard);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.Boolean;
		}

		public override string VValue {
			get {
				return this.GetModel<ExpenseItem> ().CCMatch.ToString ();
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().CCMatch;
			}
			set {
				this.GetModel<ExpenseItem> ().CCMatch = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemComment : Field
	{
		public ExpenseItemComment (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Comment);
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.Comment);
			this.Type = FieldTypeEnum.LongString;
		}

		public override string VValue {
			get {
				return this.GetValue<string> ();
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.Comment;
			}
			set {
				this.GetModel<ExpenseItem> ().ParentExpense.Comment = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemMerchantName : Field
	{
		public ExpenseItemMerchantName (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.MerchantName);
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.MerchantName);
			this.Type = Preferences.Instance.EXPGooglePlaces ? FieldTypeEnum.AutocompleteString : FieldTypeEnum.String;
		}

		public override bool IsEditable {
			get {
				if (this.Type == FieldTypeEnum.AutocompleteString)
					return true;
				
				if (this.GetModel<ExpenseItem> ().ParentExpense.IsNew
					|| this.GetModel<ExpenseItem> ().IsTransactionBySpendCatcher
					|| (this.GetModel<ExpenseItem> ().IsTransactionByCard
						&& String.IsNullOrEmpty (this.GetModel<ExpenseItem> ().ParentExpense.MerchantName)))
					return true;

				return this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit == 2;
			}
		}

		public override string VValue {
			get {
				if (Type == FieldTypeEnum.String)
					return this.GetValue<string> ();

				return this.GetValue<Prediction> ()?.description;
			}
		}

		public override object Value {
			get {
				Expense expense = this.GetModel<ExpenseItem> ().ParentExpense;

				if (Type == FieldTypeEnum.String)
					return expense.MerchantName;
				
				if (String.IsNullOrWhiteSpace (expense.MerchantName) && String.IsNullOrWhiteSpace (expense.GooglePlacesId))
					return null;
				
				return new Prediction { description = expense.MerchantName, place_id = expense.GooglePlacesId };
			}
			set {
				Expense expense = this.GetModel<ExpenseItem> ().ParentExpense;

				if (Type == FieldTypeEnum.AutocompleteString) {
					Prediction prediction = (Prediction)value;
					expense.GooglePlacesId = prediction.place_id;
					expense.MerchantName = prediction.description;

					if (!String.IsNullOrEmpty (expense.GooglePlacesId))
						expense.NotifyPropertyChanged ("RefreshMerchantCity");
				} else
					expense.MerchantName = (string)value;

				base.Value = value;
			}
		}
	}

	public class ExpenseItemMerchantCity : Field
	{
		public ExpenseItemMerchantCity (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.MerchantCity);
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.MerchantCity);
			this.Type = FieldTypeEnum.String;

			expenseItem.ParentExpense.PropertyChanged += (sender, e) => {
				if (e.PropertyName.Equals ("RefreshMerchantCity"))
					this.RefreshMerchantCityAsync ();
			};
		}

		private async void RefreshMerchantCityAsync () {
			Expense expense = this.GetModel<ExpenseItem> ().ParentExpense;

			if (String.IsNullOrEmpty (expense.GooglePlacesId)
			    || !this.GetModel<ExpenseItem> ().Product.CanShowPermission (this.GetModel<ExpenseItem> ().Product.MerchantCity))
				return;

			this.IsLoading = true;

			try {
				PlaceDetails place = await GoogleService.Instance.FetchPlaceLocationAsync (expense.GooglePlacesId);
				this.Value = place?.Locality;
			} catch (Exception) {}

			this.IsLoading = false;
		}

		public override bool IsEditable {
			get {
				if (this.GetModel<ExpenseItem> ().ParentExpense.IsNew
					|| this.GetModel<ExpenseItem> ().IsTransactionBySpendCatcher
					|| (this.GetModel<ExpenseItem> ().IsTransactionByCard
						&& String.IsNullOrEmpty (this.GetModel<ExpenseItem> ().ParentExpense.MerchantCity)))
					return true;
				else
					return this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit == 2;
			}
		}

		public override string VValue {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.MerchantCity;
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.MerchantCity;
			}
			set {
				this.GetModel<ExpenseItem> ().ParentExpense.MerchantCity = (string)value;
				base.Value = value;
			}
		}
	}

	public class ExpenseItemInvoiceId : Field
	{
		public ExpenseItemInvoiceId (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.InvoiceID);
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.InvoiceId);
			this.Type = FieldTypeEnum.String;
		}

		public override string VValue {
			get {
				return this.GetValue<string> ();
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.InvoiceId;
			}
			set {
				this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().InvoiceId = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemAllowancelineFrom : Field
	{
		public ExpenseItemAllowancelineFrom (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = "From";
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.Allowanceline);
			this.Type = FieldTypeEnum.Date;
			this.extraInfo ["Type"] = "DATE-TIME";
		}

		public override string VValue {
			get {
				return this.GetValue<DateTime> ().ToString ("g");
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().PeriodFromDateTime;
			}
			set {
				this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().PeriodFromDateTime = (DateTime)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemAllowancelineTo : Field
	{
		public ExpenseItemAllowancelineTo (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = "To";
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.Allowanceline);
			this.Type = FieldTypeEnum.Date;
			this.extraInfo ["Type"] = "DATE-TIME";
		}

		public override string VValue {
			get {
				return LoggedUser.Instance.Preferences.VDate (this.GetValue<DateTime> ()) + " " + this.GetValue<DateTime> ().ToString ("t");
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().PeriodToDateTime;
			}
			set {
				this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().PeriodToDateTime = (DateTime)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemHotelLineFrom : Field
	{
		public ExpenseItemHotelLineFrom (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = "From";
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.HotelLine);
			this.Type = FieldTypeEnum.Date;
			this.extraInfo ["Type"] = "DATE";
		}

		public override string VValue {
			get {
				return LoggedUser.Instance.Preferences.VDate (this.GetValue<DateTime> ());
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.PeriodFromDateTime;
			}
			set {
				this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().PeriodFromDateTime = (DateTime)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemHotelLineTo : Field
	{
		public ExpenseItemHotelLineTo (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = "To";
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.HotelLine);
			this.Type = FieldTypeEnum.Date;
			this.extraInfo ["Type"] = "DATE";
		}

		public override string VValue {
			get {
				return LoggedUser.Instance.Preferences.VDate (this.GetValue<DateTime> ());
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().PeriodToDateTime;
			}
			set {
				this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().PeriodToDateTime = (DateTime)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemFuelMileage : Field
	{
		public ExpenseItemFuelMileage (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Mileage);
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.VehicleOdometer);
			this.Type = FieldTypeEnum.Integer;
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.FuelMileage;
			}
			set {
				this.GetModel<ExpenseItem> ().ParentExpense.FuelMileage = (int)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemVehicleNumber : Field
	{
		public ExpenseItemVehicleNumber (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.LicencePlate);
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.VehicleNumber);
			this.Type = FieldTypeEnum.String;
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.VehicleNumber;
			}
			set {
				this.GetModel<ExpenseItem> ().ParentExpense.VehicleNumber = (string)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemProjectId : LookupField
	{
		public ExpenseItemProjectId (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ProjectID);
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.ProjectId);
			this.LookupKey = LookupService.ApiEnum.GetLookupProject;
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

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemDepartmentId : LookupField
	{
		public ExpenseItemDepartmentId (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.DepartmentID);
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.DepartmentId);
			this.LookupKey = LookupService.ApiEnum.GetLookupDepartment;
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

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemTravelRequestId : LookupField
	{
		public ExpenseItemTravelRequestId (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.TravelRequestID);
			this.Permission = expenseItem.Product.GetFieldPermission (expenseItem.Product.TravelRequestId);
			this.LookupKey = LookupService.ApiEnum.GetLookupTravelRequests;
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().TravelRequestId;
			}
			set {
				this.GetModel<ExpenseItem> ().SetTravelRequestIdAsync (((LookupItem)value).ComboId);
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemDate : Field
	{
		public ExpenseItemDate (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Date);
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Date;
			this.extraInfo ["Max-Range"] = DateTime.Now;
		}

		public override string VValue {
			get {
				DateTime date = this.GetValue<DateTime> ();
				return date.ToString ("dd MMM yyyy");
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.Date.Value;
			}
			set {
				this.GetModel<ExpenseItem> ().GetModelParent<ExpenseItem, Expense> ().Date = (DateTime)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit == 2 && !this.GetModel<ExpenseItem> ().ParentExpense.IsPaidByCreditCard);
			}
		}
	}

	public class ExpenseItemCountry : Field
	{
		public ExpenseItemCountry (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Country);
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Country;
		}

		public override string VValue {
			get {
				if (this.GetValue<Country> () == null)
					return null;

				return this.GetValue<Country> ().Name;
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.Country;
			}
			set {
				this.GetModel<ExpenseItem> ().Country = (Country)value;

				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEditCountry);
			}
		}
	}

	public class ExpenseItemCategory : Field
	{
		public ExpenseItemCategory (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Category);
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Category;
		}

		public override string VValue {
			get {
				if (this.GetValue<Product> () == null)
					return null;

				return this.GetValue<Product> ().ExpenseCategory.Name;
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().Product;
			}
			set {
				this.GetModel<ExpenseItem> ().Product = (Product)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemAmount : Field
	{
		public ExpenseItemAmount (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Amount);

			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Amount;

			expenseItem.PropertyChanged += (sender, e) => {
				if (e.PropertyName.Equals ("AmountLC")
					|| e.PropertyName.Equals ("Currency"))
					this.EmitChangeEvent ();
			};
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}

		public override string VValue {
			get {
				return this.GetModel<ExpenseItem> ().VAmountLC;
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().AmountLC;
			}
			set {
				this.GetModel<ExpenseItem> ().AmountLC = (double)value;
				base.Value = value;
			}
		}
	}

	public class ExpenseItemPrice : Field
	{
		public ExpenseItemPrice (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Amount);
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Decimal;
		}

		public override bool IsEditable {
			get {
				if (this.GetModel<ExpenseItem> ().ParentExpense == null)
					return true;

				return (this.GetModel<ExpenseItem> ().ParentExpense.IsNew
					|| !this.GetModel<ExpenseItem> ().IsTransactionByCard)
					&& this.GetModel<ExpenseItem> ().ParentExpense.IsEditable;
			}
		}

		public override string VValue {
			get {
				return this.GetModel<ExpenseItem> ().VRoundedAmountLC;
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().AmountLC;
			}
			set {
				this.GetModel<ExpenseItem> ().AmountLC = (double)value;
				base.Value = value;
			}
		}
	}

	public class ExpenseItemCurrency : Field
	{
		public ExpenseItemCurrency (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Currency);
			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Currency;
		}

		public override bool IsEditable {
			get {
				if (this.GetModel<ExpenseItem> ().ParentExpense == null)
					return true;

				return (this.GetModel<ExpenseItem> ().ParentExpense.IsNew
					|| !this.GetModel<ExpenseItem> ().IsTransactionByCard)
					&& this.GetModel<ExpenseItem> ().ParentExpense.IsEditable
					&& this.GetModel<ExpenseItem> ().CanEditCurrency;
			}
		}

		public override string VValue {
			get {
				return this.GetModel<ExpenseItem> ().Currency.Iso;
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().Currency;
			}
			set {
				this.GetModel<ExpenseItem> ().Currency = (Currency)value;
				base.Value = value;
			}
		}
	}

	public class ExpenseItemQuantity : Field
	{
		public ExpenseItemQuantity (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Quantity);

			this.Permission = FieldPermissionEnum.Mandatory;
			this.Type = FieldTypeEnum.Integer;
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}

		public override string VValue {
			get {
				return this.GetModel<ExpenseItem> ().Quantity.ToString ();
			}
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().Quantity;
			}
			set {
				this.GetModel<ExpenseItem> ().Quantity = (int)value;
				base.Value = value;
			}
		}
	}

	public class ExpenseItemReceiptTickBox : Field
	{
		public ExpenseItemReceiptTickBox (ExpenseItem expenseItem) : base (expenseItem) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Receipt);
			this.Type = FieldTypeEnum.Boolean;
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().ParentExpense.ReceiptPresent;
			}
			set {
				this.GetModel<ExpenseItem> ().ParentExpense.ReceiptPresent = (bool)value;
				base.Value = value;
			}
		}

		public override bool IsEditable {
			get {
				return this.GetModel<ExpenseItem> ().IsNew || (this.GetModel<ExpenseItem> ().ParentExpense.IsEditable && this.GetModel<ExpenseItem> ().CanEdit > 0);
			}
		}
	}

	public class ExpenseItemPolicyRule : Field
	{
		public ExpenseItemPolicyRule (ExpenseItem expenseItem) : base (expenseItem) {
			this.Type = FieldTypeEnum.PolicyRule;

			string title = null;
			if (expenseItem.PolicyRule == ExpenseItem.PolicyRules.Orange)
				title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ExpensePolicyOrange);
			else
				title = Labels.GetLoggedUserLabel (Labels.LabelEnum.ExpensePolicyRed);

			this.extraInfo ["Icon"] = expenseItem.PolicyRule;
			this.extraInfo ["Title"] = title;
			this.extraInfo ["Message"] = expenseItem.PolicyRuleTip;
		}

		public override object Value {
			get {
				return this.GetModel<ExpenseItem> ().PolicyRuleTip;
			}
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
	}
}