using System;
using System.Collections.Generic;
using Mxp.Core.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mxp.Core.Business
{
	public static class AttendeeTypeExtensions {
		public static bool CanAddCompanyName (this AttendeeTypeEnum type) {
			switch (type) {
				case AttendeeTypeEnum.HCP:
					return Preferences.Instance.HCPDataSource != Preferences.HCPDataSourceEnum.OneKey
						&& Preferences.Instance.HCPDataSource != Preferences.HCPDataSourceEnum.HcpUniqueId;
				default:
					return true;
			}
		}

		public static bool CanAddCountry (this AttendeeTypeEnum type) {
			switch (type) {
				case AttendeeTypeEnum.HCP:
					return Preferences.Instance.HCPDataSource == Preferences.HCPDataSourceEnum.OneKey
						|| Preferences.Instance.HCPDataSource == Preferences.HCPDataSourceEnum.HcpUniqueId;
				default:
					return false;
			}
		}

		public static bool IsStateMandatory (this AttendeeTypeEnum type) {
			switch (type) {
				case AttendeeTypeEnum.HCP:
					return !(Preferences.Instance.HCPDataSource == Preferences.HCPDataSourceEnum.OneKey
						|| Preferences.Instance.HCPDataSource == Preferences.HCPDataSourceEnum.HcpUniqueId);
				case AttendeeTypeEnum.HCO:
					return true;
				default:
					return false;
			}
		}
	}

	public partial class Attendee
	{
		private static Collection<String> dynamicsFieldKeys = new Collection<String> {
			"Infochar1",
			"Infochar2",
			"Infochar3",
			"Infonum1",
			"Infonum2",
		};

		public Collection<Field> MainFields {
			get {
				Collection<Field> result = new Collection<Field> () {
					new AttendeeName (this),
					new AttendeeCompanyName (this),
					new AttendeeType (this)
				};

				if (!string.IsNullOrEmpty (this.Reference))
					result.Add (new AttendeeReference (this));

				if (!string.IsNullOrEmpty (this.Address))
					result.Add (new AttendeeAddress (this));

				if (!this.ZipCode.HasValue)
					result.Add (new AttendeeZipCode (this));
				
				if (!string.IsNullOrEmpty(this.City))
					result.Add (new AttendeeCity (this));

				if (!string.IsNullOrEmpty(this.State))
					result.Add (new AttendeeState (this));

				if (!string.IsNullOrEmpty(this.Specialty))
					result.Add (new AttendeeSpecialty (this));
				
				return result;
			}
		}

		public Collection<Field> DynamicField {
			get {
				Collection<Field> result = new Collection<Field> ();

				Attendee.dynamicsFieldKeys.ForEach (key => {
					DynamicFieldHolder dynamicField = Preferences.Instance.DynamicFields.GetFieldWithKey (key, DynamicFieldHolder.LocationEnum.Attendee);

					if (dynamicField != null) {
						Field field = null;

						switch (dynamicField.LinkType) {
							case FieldTypeEnum.Lookup:
								field = new DynamicLookupField (this, dynamicField);
								break;
							case FieldTypeEnum.Combo:
								field = new DynamicComboField (this, dynamicField);
								break;
							default:
								field = new DynamicField (this, dynamicField);
								break;
						}

						result.Add (field);
					}
				});

				return result;
			}
		}

		public Collection<Field> AllFields {
			get {
				return new Collection<Field> (this.MainFields.Concat (this.DynamicField).ToList ());
			}
		}

		public Collection<Field> FormFields {
			get {
				switch (this.Type) {
					case AttendeeTypeEnum.Business:
						return new Collection<Field> () {
							new AttendeeFormFirstname (this),
							new AttendeeFormLastname (this, true),
							new AttendeeFormCompanyName (this, true)
						};
					case AttendeeTypeEnum.Spouse:
						return new Collection<Field> () {
							new AttendeeFormFirstname (this),
							new AttendeeFormLastname (this, true)
						};
					case AttendeeTypeEnum.HCP:
						Collection<Field> collection = new Collection<Field> () {
							new AttendeeFormFirstname (this),
							new AttendeeFormLastname (this)
						};

						if (this.Type.CanAddCompanyName ())
							collection.Add (new AttendeeFormCompanyName (this));

						collection.Add (new AttendeeFormZipCode (this));
						collection.Add (new AttendeeFormCity (this));
						collection.Add (new AttendeeFormState (this));

						if (this.Type.CanAddCountry ())
							collection.Add (new AttendeeFormCountry (this));

						collection.Add (new AttendeeFormSpecialty (this));
						collection.Add (new AttendeeFormReference (this));

						return collection;
					case AttendeeTypeEnum.HCO:
						return new Collection<Field> () {
							new AttendeeFormCompanyName (this),
							new AttendeeFormZipCode (this),
							new AttendeeFormCity (this),
							new AttendeeFormState (this),
							new AttendeeFormSpecialty (this),
							new AttendeeFormReference (this)
						};
					case AttendeeTypeEnum.UCP:
						return new Collection<Field> () {
							new AttendeeFormFirstname (this, true),
							new AttendeeFormLastname (this, true),
							new AttendeeFormCompanyName (this),
							new AttendeeFormAddress (this),
							new AttendeeFormZipCode (this),
							new AttendeeFormCity (this, true),
							new AttendeeFormState (this),
							new AttendeeFormSpecialty (this),
							new AttendeeFormReference (this)
						};
					default:
						return null;
				}
			}
		}
	}
}