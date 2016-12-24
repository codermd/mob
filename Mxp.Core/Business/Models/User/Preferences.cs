using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Mxp.Utils;
using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using Mxp.Core.Utils;
using System.Linq;

namespace Mxp.Core.Business
{
	public enum VisibilityEnum {
		Hidden,
		Show,
		ReadOnly,
		Other
	}

	public class Preferences : Model
	{
		public enum TravelRequestPermissionEnum {
			Unknown,
			Disabled,
			Enabled,
			Forced
		}

		public enum ProductTypeEnum {
			Default,
			Business,
			Private,
			BusinessOnly
		}

		public enum UnisSystemEnum {
			Metric,
			Imperial
		}

		public enum SpendCatcherEnum {
			Standard,
			Urgent,
			VeryUrgent,
			None
		}

		public enum HCPDataSourceEnum {
			None,
			MediSpend,
			Ucb,
			ComplianceId,
			AttendeeBook,
			HcpUniqueId,
			DemoHcp,
			OneKey
		}

		public enum BusinessMilesReadOnlyEnum {
			None,
			Editable,
			ReadOnly
		}

		public bool FilterTravelCategory { get; set; }

		public string FldCustomerName { get; set; }
		public VisibilityEnum REPAttachment { get; set; }

		public bool ITEShowReceiptTickbox { get; set;}

		public PermissionEnum ALLSIcomment { get; set; }
		public PermissionEnum ALLSIprojectId { get; set; }
		public PermissionEnum ALLSIdepartmentId { get; set; }
		public PermissionEnum ALLSItrqId { get; set; }
		public PermissionEnum ALLSIMerchantCity { get; set; }
		public PermissionEnum ALLSIMerchantName { get; set; }
		public PermissionEnum ALLSIinvoiceId { get; set; }
		public PermissionEnum ALLSIinfochar1 { get; set; }
		public PermissionEnum ALLSIinfochar2 { get; set; }
		public PermissionEnum ALLSIinfochar3 { get; set; }
		public PermissionEnum ALLSIinfochar4 { get; set; }
		public PermissionEnum ALLSIinfochar5 { get; set; }
		public PermissionEnum ALLSIinfochar6 { get; set; }
		public PermissionEnum ALLSIinfochar7 { get; set; }
		public PermissionEnum ALLSIinfochar8 { get; set; }
		public PermissionEnum ALLSIinfonum1 { get; set; }
		public PermissionEnum ALLSIinfonum2 { get; set; }

		public VisibilityEnum BreakfastVisibility { get; set; }
		public VisibilityEnum LunchVisibility { get; set; }
		public VisibilityEnum DinnerVisibility { get; set; }
		public VisibilityEnum LodgingVisibility { get; set; }
		public VisibilityEnum InfoVisibility { get; set; }
		public VisibilityEnum WorknightVisibility { get; set; }

		public VisibilityEnum HCPAllowHCO { get; set; }
		public VisibilityEnum HCPAllowHCP { get; set; }
		public VisibilityEnum HCPAllowManualEntry { get; set; }
		public VisibilityEnum HCPShowHideSpouse { get; set; }

		public PermissionEnum REPAllocationPJT { get; set; }
		public PermissionEnum REPAllocationDPT { get; set; }
		public PermissionEnum REPAllocationTRV { get; set; }
		public PermissionEnum REPInfoChar1 { get; set; }
		public PermissionEnum REPInfoChar2 { get; set; }
		public PermissionEnum REPInfoChar3 { get; set; }
		public PermissionEnum REPInfoNum1 { get; set; }
		public PermissionEnum REPInfoNum2 { get; set; }
		public PermissionEnum REPApprovalRejectionComment { get; set; }

		public TravelRequestPermissionEnum TravelRequestPermission { get; set; }

		public DynamicFields DynamicFields { get; set; }

		//Mileage pref
		public PermissionEnum MILSIcomment {get ; set; }
		public PermissionEnum MILSIprojectId {get ; set; }
		public PermissionEnum MILSIdepartmentId {get ; set; }
		public PermissionEnum MILSItrqId {get ; set; }
		public PermissionEnum MILSIinfochar1 {get ; set; }
		public PermissionEnum MILSIinfochar2 {get ; set; }
		public PermissionEnum MILSIinfochar3 {get ; set; }
		public PermissionEnum MILSIinfochar4 {get ; set; }
		public PermissionEnum MILSIinfochar5 {get ; set; }
		public PermissionEnum MILSIinfochar6 {get ; set; }
		public PermissionEnum MILSIinfochar7 {get ; set; }
		public PermissionEnum MILSIinfochar8 {get ; set; }
		public PermissionEnum MILSIinfonum1 {get ; set; }
		public PermissionEnum MILSIinfonum2 {get ; set; }

        public PermissionEnum MILMap { get; set; }
		public ProductTypeEnum MILDefaultProduct { get; set; }
		public VisibilityEnum MILWithdrawCommuting { get; set; }

		public string MILCommutingOtherValue { get; set; }
		public VisibilityEnum MILCommuting { get; set; }
		public string MILPrivateOtherValue { get; set; }
		public VisibilityEnum MILPrivate { get; set; }
		public string MILOdometerOtherValue { get; set; }
		public VisibilityEnum MILOdometer { get; set; }
		public string MILVehicleOtherValue { get; set; }
		public VisibilityEnum MILVehicle { get; set; }

		public UnisSystemEnum MilUnit { get; set; }

		public BusinessMilesReadOnlyEnum MILBusinessMilesReadOnly { get; set; }

		private BusinessMilesReadOnlyEnum getMILBusinessMilesReadOnly(string type) {
			if (type == null) {
				return BusinessMilesReadOnlyEnum.ReadOnly;
			}
			if (type.Equals ("N")) {
				return BusinessMilesReadOnlyEnum.Editable;
			}
			if (type.Equals ("Y")) {
				return BusinessMilesReadOnlyEnum.ReadOnly;
			}
			return BusinessMilesReadOnlyEnum.ReadOnly;
		}

		public int CarsCount { get; set; }

		public int CustomerGermanAllowanceIndicator { get; set; }

		public string ExpMobileCCMatch { get; set; }

		public PermissionEnum TVRProject { get; set; }
		public PermissionEnum TVRCostCenter { get; set; }

		public int FldCurrencyId { get; set; }
		public VisibilityEnum ITEEnableItemAttachment { get; set; }

		public double FldEmployeeHomeWorkDistance { get; set; }

		public bool AllowanceActivationMobile { get; set; }
		public VisibilityEnum ITECountryTRXCRDUser { get; set; }

		public SpendCatcherEnum MOBSpendCatcher { get; set; }

		public HCPDataSourceEnum HCPDataSource { get; set; }
		public int FldCountryId { get; set; }
		public bool ApprovalSelectedExpenses { get; set;}
		public bool EXPGooglePlaces { get; set; }

		private HCPDataSourceEnum GetHCPDataSource (string str) {
			switch (str.ToUpperInvariant ()) {
				case "MEDISPEND":
					return HCPDataSourceEnum.MediSpend;
				case "UCB":
					return HCPDataSourceEnum.Ucb;
				case "COMPLIANCEID":
					return HCPDataSourceEnum.ComplianceId;
				case "ATTENDEEBOOK":
					return HCPDataSourceEnum.AttendeeBook;
				case "HCP_UNIQUEID":
					return HCPDataSourceEnum.HcpUniqueId;
				case "DEMOHCP":
					return HCPDataSourceEnum.DemoHcp;
				case "ONEKEY":
					return HCPDataSourceEnum.OneKey;
				default:
					return HCPDataSourceEnum.None;
			}
		}

		public bool IsSpendCatcherEnable {
			get {
				return this.MOBSpendCatcher != SpendCatcherEnum.None;
			}
		}

		public string ITEAttendeeAllocation { get; set; }

		public bool IsGTPEnabled {
			get {
				return this.ITEAttendeeAllocation != null && this.ITEAttendeeAllocation.Contains ("GTP", StringComparison.OrdinalIgnoreCase);
			}
		}

		public static Preferences Instance {
			get {
				return LoggedUser.Instance.Preferences;
			}
		}

		public Preferences () {

		}

		private UnisSystemEnum GetUnisSystemEnum (string str) {
			return str == "Kilometers" ? UnisSystemEnum.Metric : UnisSystemEnum.Imperial;
		}

		public PermissionEnum GetPermissionEnumForTravel (string str) {
			if (str == null)
				return PermissionEnum.Hidden;

			if (!str.Equals ("Show"))
				return PermissionEnum.Hidden;

			return PermissionEnum.Optional;
		}

		public PermissionEnum GetPermissionEnum (string str) {
			switch (str.ToUpper()) {
				case "":
				case "N":
					return PermissionEnum.Hidden;
				case "O":
					return PermissionEnum.Optional;
				case "M":
					return PermissionEnum.Mandatory;
				case "T":
					return PermissionEnum.MandatoryTravelRequest;
				case "P":
					return PermissionEnum.MandatoryProject;
				case "D":
					return PermissionEnum.MandatoryDepartment;
				default:
					return PermissionEnum.Unknown;
			}
		}

		public PermissionEnum GetReportApprovalCommentEnum (string str) {
			if (string.IsNullOrEmpty (str))
				return PermissionEnum.Optional;

			if (str.Equals ("Mandatory"))
				return PermissionEnum.Mandatory;

			return PermissionEnum.Optional;
		}

		public ProductTypeEnum GetProductTypeEnum (string str) {
			str = str.ToUpper ();

			switch (str) {
				case "B":
					return ProductTypeEnum.Business;
				case "P":
					return ProductTypeEnum.Private;
				case "X":
					return ProductTypeEnum.BusinessOnly;
				case "C":
				default:
					return ProductTypeEnum.Default;
			}
		}

		public bool AngloSaxonDateFormat;

		public PermissionEnum GetAllowancePermission (string str) {
			str = str.ToUpper();

			if (str.Equals ("")) {
				return PermissionEnum.Hidden;
			}

			if(str.Equals("O"))
				return PermissionEnum.Optional;

			if (str.Equals ("M")) {
				return PermissionEnum.Mandatory;
			}

			return this.GetPermissionEnum (str);
		}

		public VisibilityEnum GetVisibilityEnum (string str) {
			return str.ToBool () ? VisibilityEnum.Show : VisibilityEnum.Hidden;
		}

		public PreferencesResponse rawResponse;

		public void Populate(PreferencesResponse customerPrefsResponse) {
			this.rawResponse = customerPrefsResponse;

			this.FldCustomerName = customerPrefsResponse.FldCustomerName;
			this.REPAttachment = customerPrefsResponse.REPAttachment.ToUpper () != "N" ? VisibilityEnum.Show : VisibilityEnum.Hidden;

			this.ALLSIcomment = this.GetAllowancePermission(customerPrefsResponse.ALLSIcomment);
			this.ALLSIprojectId = this.GetAllowancePermission(customerPrefsResponse.ALLSIprojectId);
			this.ALLSIdepartmentId = this.GetAllowancePermission(customerPrefsResponse.ALLSIdepartmentId);
			this.ALLSItrqId = this.GetAllowancePermission(customerPrefsResponse.ALLSItrqId);
			this.ALLSIMerchantCity = this.GetAllowancePermission (customerPrefsResponse.ALLSIMerchantCity);
			this.ALLSIMerchantName = this.GetAllowancePermission (customerPrefsResponse.ALLSIMerchantName);
			this.ALLSIinvoiceId = this.GetAllowancePermission (customerPrefsResponse.ALLSIinvoiceId);
			this.ALLSIinfochar1 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfochar1);
			this.ALLSIinfochar2 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfochar2);
			this.ALLSIinfochar3 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfochar3);
			this.ALLSIinfochar4 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfochar4);
			this.ALLSIinfochar5 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfochar5);
			this.ALLSIinfochar6 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfochar6);
			this.ALLSIinfochar7 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfochar7);
			this.ALLSIinfochar8 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfochar8);
			this.ALLSIinfonum1 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfonum1);
			this.ALLSIinfonum2 = this.GetAllowancePermission(customerPrefsResponse.ALLSIinfonum2);

			this.HCPAllowHCO = this.GetVisibilityEnum (customerPrefsResponse.HCPAllowHCO);
			this.HCPAllowHCP = this.GetVisibilityEnum (customerPrefsResponse.HCPAllowHCP);
			this.HCPAllowManualEntry = this.GetVisibilityEnum (customerPrefsResponse.HCPAllowManualEntry);
			this.HCPShowHideSpouse = this.GetVisibilityEnum (customerPrefsResponse.HCPShowHideSpouse);
			this.ITEAttendeeAllocation = customerPrefsResponse.ITEAttendeeAllocation;

			this.DynamicFields = new DynamicFields (customerPrefsResponse.DynamicFields);

			this.FldCurrencyId = customerPrefsResponse.FldcurrencyID;
			this.ITEEnableItemAttachment = this.GetVisibilityEnum (customerPrefsResponse.ITEEnableItemAttachment);
			this.FldEmployeeHomeWorkDistance = customerPrefsResponse.FldEmployeeHomeWorkDistance;

			this.REPAllocationPJT = this.GetPermissionEnum(customerPrefsResponse.REPAllocationPJT);
			this.REPAllocationDPT = this.GetPermissionEnum(customerPrefsResponse.REPAllocationDPT);
			this.REPAllocationTRV = this.GetPermissionEnum(customerPrefsResponse.REPAllocationTRV);
			this.REPInfoChar1 = this.GetPermissionEnum(customerPrefsResponse.REPInfoChar1);
			this.REPInfoChar2 = this.GetPermissionEnum(customerPrefsResponse.REPInfoChar2);
			this.REPInfoChar3 = this.GetPermissionEnum(customerPrefsResponse.REPInfoChar3);
			this.REPInfoNum1 = this.GetPermissionEnum(customerPrefsResponse.REPInfoNum1);
			this.REPInfoNum2 = this.GetPermissionEnum(customerPrefsResponse.REPInfoNum2);

			this.REPApprovalRejectionComment = this.GetReportApprovalCommentEnum (customerPrefsResponse.REPApprovalRejectionComment);

			this.CustomerGermanAllowanceIndicator = customerPrefsResponse.CustomerGermanAllowanceIndicator;

			this.ExpMobileCCMatch = customerPrefsResponse.EXPMobileCCMatch;

			this.MILSIcomment = this.GetPermissionEnum (customerPrefsResponse.MILSIcomment);
			this.MILSIprojectId = this.GetPermissionEnum (customerPrefsResponse.MILSIprojectId);
			this.MILSIdepartmentId = this.GetPermissionEnum (customerPrefsResponse.MILSIdepartmentId);
			this.MILSItrqId = this.GetPermissionEnum (customerPrefsResponse.MILSItrqId);
			this.MILSIinfochar1 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfochar1);
			this.MILSIinfochar2 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfochar2);
			this.MILSIinfochar3 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfochar3);
			this.MILSIinfochar4 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfochar4);
			this.MILSIinfochar5 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfochar5);
			this.MILSIinfochar6 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfochar6);
			this.MILSIinfochar7 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfochar7);
			this.MILSIinfochar8 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfochar8);
			this.MILSIinfonum1 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfonum1);
			this.MILSIinfonum2 = this.GetPermissionEnum (customerPrefsResponse.MILSIinfonum2);

			this.MILMap = this.GetPermissionEnum (customerPrefsResponse.MILMap);
			this.MILDefaultProduct = this.GetProductTypeEnum (customerPrefsResponse.MILDefaultProduct);
			this.MILWithdrawCommuting = this.GetVisibilityEnum (customerPrefsResponse.MILWithdrawCommuting);

			this.MILCommuting = this.GetMileageVisibility (customerPrefsResponse.MILCommuting, (string value) => this.MILCommutingOtherValue = value);
			this.MILPrivate = this.GetMileageVisibility (customerPrefsResponse.MILPrivate, (string value) => this.MILPrivateOtherValue = value);
			this.MILOdometer = this.GetMileageVisibility (customerPrefsResponse.MILOdometer, (string value) => this.MILOdometerOtherValue = value);

			this.MILBusinessMilesReadOnly = this.getMILBusinessMilesReadOnly (customerPrefsResponse.MILBusinessMilesReadOnly);

			customerPrefsResponse.MILVehicle = customerPrefsResponse.MILVehicle.ToUpper ();
			if (customerPrefsResponse.MILVehicle == "Y") {
				this.MILVehicle = VisibilityEnum.Show;
			} else {
				this.MILVehicleOtherValue = customerPrefsResponse.MILVehicle;
				this.MILVehicle = VisibilityEnum.Other;
			}

			this.MilUnit = this.GetUnisSystemEnum (customerPrefsResponse.MilUnit);
			this.CarsCount = customerPrefsResponse.CarsCount;

			this.TVRCostCenter = this.GetPermissionEnumForTravel ((string)customerPrefsResponse.TVRCostCenter);
			this.TVRProject = this.GetPermissionEnumForTravel ((string)customerPrefsResponse.TVRProject);

			this.BreakfastVisibility = !customerPrefsResponse.AllIndicator1.Equals("0") ? VisibilityEnum.Show : VisibilityEnum.Hidden;
			this.LunchVisibility = !customerPrefsResponse.AllIndicator2.Equals("0") ? VisibilityEnum.Show : VisibilityEnum.Hidden;
			this.DinnerVisibility = !customerPrefsResponse.AllIndicator3.Equals("0") ? VisibilityEnum.Show : VisibilityEnum.Hidden;
			this.LodgingVisibility = !customerPrefsResponse.AllIndicator4.Equals("Hide") ? VisibilityEnum.Show : VisibilityEnum.Hidden;
			this.InfoVisibility = !customerPrefsResponse.AllIndicator5.Equals("Hide") ? VisibilityEnum.Show : VisibilityEnum.Hidden;
			this.WorknightVisibility = !customerPrefsResponse.AllIndicator6.Equals("Hide") ? VisibilityEnum.Show : VisibilityEnum.Hidden;

			this.AngloSaxonDateFormat = !customerPrefsResponse.GENDateFormat.Equals ("0");

			if (customerPrefsResponse.ITEShowReceiptTickbox != null)
				this.ITEShowReceiptTickbox = customerPrefsResponse.ITEShowReceiptTickbox.Equals ("Show");
			else
				this.ITEShowReceiptTickbox = false;

			this.AllowanceActivationMobile = customerPrefsResponse.MOBAllowanceActivationMobile == 1;
			this.ITECountryTRXCRDUser = this.GetVisibilityEnum (customerPrefsResponse.ITECountryTRXCRDUser);
			this.MOBSpendCatcher = this.GetSpendCatcherEnum (customerPrefsResponse.MOBSpendCatcher);
			this.TravelRequestPermission = customerPrefsResponse.REPTravelRequestName.ToEnum (TravelRequestPermissionEnum.Unknown);
			this.HCPDataSource = this.GetHCPDataSource (customerPrefsResponse.HCPDataSource);
			this.FldCountryId = customerPrefsResponse.FldcountryID;
			this.ApprovalSelectedExpenses = !customerPrefsResponse.MOBApprovalSelectedExpenses?.Equals ("0") ?? false;
			this.EXPGooglePlaces = customerPrefsResponse.EXPGooglePlaces.ToBool ();
			this.FilterTravelCategory = customerPrefsResponse.ITEFilterCategory.Equals ("ABINBEV");
		}

		private SpendCatcherEnum GetSpendCatcherEnum (string str) {
			switch (str) {
				case "A":
					return SpendCatcherEnum.Standard;
				case "B":
					return SpendCatcherEnum.Urgent;
				case "C":
					return SpendCatcherEnum.VeryUrgent;
				default:
					return SpendCatcherEnum.None;
			}
		}

		public VisibilityEnum GetMileageVisibility (string str, Action<string> selector) {
			str = str.ToUpper ();

			switch (str) {
				case "READONLY":
					return VisibilityEnum.ReadOnly;
				case "N":
					return VisibilityEnum.Hidden;
				case "Y":
					return VisibilityEnum.Show;
				default:
					selector (str);
					return VisibilityEnum.Other;
			}
		}

		public async Task FetchAsync () {
			this.Populate (await SystemService.Instance.FetchPreferencesAsync ());
		}

		public bool CanShowField (string propertyName) {
			PermissionEnum permission = this.GetPropertyValue<PermissionEnum> (propertyName);

			return this.CanShowPermission (permission);
		}

		public bool CanShowPermission (PermissionEnum permission) {
			return permission == PermissionEnum.Optional
				|| permission == PermissionEnum.Mandatory;
		}

		public bool IsMandatory (string propertyName) {
			return this.GetPropertyValue<PermissionEnum> (propertyName) == PermissionEnum.Mandatory;
		}

		public Collection<DynamicFieldHolder> GetTravelDynamicFields () {
			Collection<DynamicFieldHolder> collection = new Collection<DynamicFieldHolder> ();

			this.DynamicFields.ForEach (dynamicField => {
				if(dynamicField.LocationName == DynamicFieldHolder.LocationEnum.TVR)
					collection.Add(dynamicField);
			});

			return collection;
		}

		public string VDate (DateTime date) {
			return date.ToString (this.AngloSaxonDateFormat ? @"MM\/dd\/yyyy" : @"dd\/MM\/yyyy");
		}
	}
}
