using System;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Collections.Generic;

namespace Mxp.Core.Business
{
	public partial class Report
	{
		public Collection<String> GetDynamicsFieldKeys () {
			return new Collection<String> {
				"Infochar1",
				"Infochar2",
				"Infochar3",
				"Infonum1",
				"Infonum2",
			};
		}

		Dictionary<string, string> FieldMapper = new Dictionary<string, string> () {
			{ "Infochar1", "REPInfoChar1" },
			{ "Infochar2", "REPInfoChar2" },
			{ "Infochar3", "REPInfoChar3" },
			{ "Infonum1", "REPInfoNum1" },
			{ "Infonum2", "REPInfoNum2" },
		};

		public Collection<Field> GetMainFields(){
			Collection<Field> result = new Collection<Field> ();

			if (this.IsFromApproval)
				result.Add (new ReportFieldEmployee (this));

			result.Add (new ReportMainFieldName(this));

			if (!this.IsNew) {
				result.Add (new ReportMainFieldDateRange(this));
				result.Add (new ReportMainFieldApprovalStatus(this));
				result.Add (new ReportMainFieldReceiptStatus(this));

				if (this.Pf != 0)
					result.Add (new ReportMainFieldPrivatelyFunded (this));

				if (this.Cf != 0)
					result.Add (new ReportMainFieldCompanyFunded (this));

				if (this.Retained != 0)
					result.Add (new ReportMainFieldPrepaymeqts (this));

				result.Add (new ReportMainFieldAmount(this));
			}

			return result;
		}

		public Collection<Field> GetAllFields () {
			Collection<Field> result = new Collection<Field> ();

			this.GetStaticFields ().ForEach (field => {
				result.Add (field);
			});
				
			this.GetDynamicFields ().ForEach (field => {
				result.Add (field);
			});

			return result;
		}

		private Collection<Field> GetStaticFields () {
			Collection<Field> result = new Collection<Field> ();

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.REPAllocationPJT))
				result.Add (new ReportFieldAllocationPJT (this));

			if (Preferences.Instance.CanShowPermission(Preferences.Instance.REPAllocationDPT))
				result.Add (new ReportFieldAllocationDPT (this));

			if (Preferences.Instance.CanShowPermission(Preferences.Instance.REPAllocationTRV))
				result.Add (new ReportFieldAllocationTRV (this));

			return result;
		}

		public Collection<Field> GetDynamicFields () {
			Collection<Field> result = new Collection<Field> ();

			this.GetDynamicsFieldKeys ().ForEach (key => {
				string mappedKey = this.FieldMapper[key];

				if (this.CanShowField(mappedKey)) {
					DynamicFieldHolder dfield = Preferences.Instance.DynamicFields.GetFieldWithKey (key, DynamicFieldHolder.LocationEnum.Report);

					if (dfield != null) {
						
						Field field = null;

						switch (dfield.LinkType) {
							case FieldTypeEnum.Lookup:
								field = new DynamicLookupField (this, dfield);
								break;
							case FieldTypeEnum.Combo:
								field = new DynamicComboField (this, dfield);
								break;
							default:
								field = new DynamicField (this, dfield);
								break;
						}

						field.IsEditable = this.IsNew || this.IsEditable;
						field.Permission = Preferences.Instance.IsMandatory (mappedKey) ? FieldPermissionEnum.Mandatory : FieldPermissionEnum.Optional;

						result.Add(field);
					}
				}
			});

			return result;
		}

		public bool CanShowField (string key) {
			return Preferences.Instance.CanShowField (key);
		}

		public DynamicFieldHolder GetDynamicField (string key) {
			return Preferences.Instance.DynamicFields.GetFieldWithKey (key, DynamicFieldHolder.LocationEnum.Report);
		}
	}
}