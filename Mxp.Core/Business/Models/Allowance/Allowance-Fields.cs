using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Allowance
	{
		public Collection<Field> _creationAllowanceFields;
		public Collection<Field> CreationAllowanceFields {
			get {
				if (this._creationAllowanceFields == null) {
					this._creationAllowanceFields = new Collection<Field> () {
						new AllowanceCreationDateFrom (this),
						new AllowanceCreationDateTo (this),
						new AllowanceCreationCountry (this)
					};

					if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIcomment))
						this._creationAllowanceFields.Add (new AllowanceCreationComment (this));

					if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIMerchantCity))
						this._creationAllowanceFields.Add (new AllowanceCreationLocation (this));

					if (Preferences.Instance.BreakfastVisibility == VisibilityEnum.Show)
						this._creationAllowanceFields.Add (new AllowanceCreationBreakfast (this));

					if (Preferences.Instance.LunchVisibility == VisibilityEnum.Show)
						this._creationAllowanceFields.Add (new AllowanceCreationLunch (this));

					if (Preferences.Instance.DinnerVisibility == VisibilityEnum.Show)
						this._creationAllowanceFields.Add (new AllowanceCreationDinner (this));

					if (Preferences.Instance.LodgingVisibility == VisibilityEnum.Show)
						this._creationAllowanceFields.Add (new AllowanceCreationLodging (this));

					if (Preferences.Instance.InfoVisibility == VisibilityEnum.Show)
						this._creationAllowanceFields.Add (new AllowanceCreationInfo (this));
						
					if (Preferences.Instance.WorknightVisibility == VisibilityEnum.Show)
						this._creationAllowanceFields.Add (new AllowanceCreationWorkNight (this));
				}

				return this._creationAllowanceFields;
			}
		}

		public Collection<TableSectionModel> CreationAllowanceSectionFields { 
			get { 
				return new Collection<TableSectionModel> () {
					new TableSectionModel (this.CreationAllowanceFields)
				};
			}
		}

		public override Collection<String> GetDynamicFieldsKeys () {
			return new Collection<String> {
				"Infochar1",
				"Infochar2",
				"Infochar3",
				"Infochar4",
				"Infochar5",
				"Infochar6",
				"Infochar7",
				"Infochar8",
				"Infonum1",
				"Infonum2"
			};
		}

		public override PermissionEnum GetPermissionForKey (String fieldName) {
			return Preferences.Instance.GetPropertyValue<PermissionEnum> (fieldName);
		}
	
		private Collection<Field> _staticFields;
		private Collection<Field> StaticFields {
			get {
				if (this._staticFields == null) {
					this._staticFields =  new Collection<Field>();

					if (Preferences.Instance.CanShowPermission(Preferences.Instance.ALLSIprojectId))
						this._staticFields.Add (new AllowanceProjectId (this));

					if (Preferences.Instance.CanShowPermission(Preferences.Instance.ALLSIdepartmentId))
						this._staticFields.Add (new AllowanceDepartmentId (this));

					if (Preferences.Instance.CanShowPermission(Preferences.Instance.ALLSItrqId))
						this._staticFields.Add (new AllowanceTravelRequestID (this));

                    if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIMerchantName))
                        this._staticFields.Add (new AllowanceMerchantName (this));

                    if (Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinvoiceId))
                        this._staticFields.Add (new AllowanceInvoiceID (this));
                }

				return this._staticFields;
			}
		}

		public override Collection<Field> GetStaticFields (ExpenseItem anItem) {
			return this.StaticFields;
		}

		public bool CanShowField (string key) {
			switch (key) {
				case "Infochar1":
					return Preferences.Instance.CanShowPermission(Preferences.Instance.ALLSIinfochar1);
				case "Infochar2":
					return Preferences.Instance.CanShowPermission(Preferences.Instance.ALLSIinfochar2);
				case "Infochar3":
					return Preferences.Instance.CanShowPermission(Preferences.Instance.ALLSIinfochar3);
                case "Infochar4":
                    return Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar4);
                case "Infochar5":
                    return Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar5);
                case "Infochar6":
                    return Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar6);
                case "Infochar7":
                    return Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar7);
                case "Infochar8":
                    return Preferences.Instance.CanShowPermission (Preferences.Instance.ALLSIinfochar8);
                case "Infonum1":
					return Preferences.Instance.CanShowPermission(Preferences.Instance.ALLSIinfonum1);
				case "Infonum2":
					return Preferences.Instance.CanShowPermission(Preferences.Instance.ALLSIinfonum2);
				default:
					return false;
			}
		}

		public bool IsMandatory (string key) {
			switch (key) {
				case "Infochar1":
					return Preferences.Instance.ALLSIinfochar1 == PermissionEnum.Mandatory;
				case "Infochar2":
					return Preferences.Instance.ALLSIinfochar2 == PermissionEnum.Mandatory;
				case "Infochar3":
					return Preferences.Instance.ALLSIinfochar3 == PermissionEnum.Mandatory;
                case "Infochar4":
                    return Preferences.Instance.ALLSIinfochar4 == PermissionEnum.Mandatory;
                case "Infochar5":
                    return Preferences.Instance.ALLSIinfochar5 == PermissionEnum.Mandatory;
                case "Infochar6":
                    return Preferences.Instance.ALLSIinfochar6 == PermissionEnum.Mandatory;
                case "Infochar7":
                    return Preferences.Instance.ALLSIinfochar7 == PermissionEnum.Mandatory;
                case "Infochar8":
                    return Preferences.Instance.ALLSIinfochar8 == PermissionEnum.Mandatory;
                case "Infonum1":
					return Preferences.Instance.ALLSIinfonum1 == PermissionEnum.Mandatory;
				case "Infonum2":
					return Preferences.Instance.ALLSIinfonum2 == PermissionEnum.Mandatory;
				default:
					return false;
			}
		}
	}
}