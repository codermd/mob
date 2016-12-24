using System;

using Mxp.Core.Services.Responses;
using System.Collections.Generic;
using Mxp.Utils;
using System.Diagnostics;
using System.Globalization;

namespace Mxp.Core.Business
{
	public class DynamicFieldHolder : Model
	{
		public enum LocationEnum {
			Attendee,
			Item,
			Report,
			TVR,
			Other
		}

		private LocationEnum GetLocationEnum (string location) {
			switch (location) {
				case "Attendee":
					return LocationEnum.Attendee;
				case "Item":
					return LocationEnum.Item;
				case "Report":
					return LocationEnum.Report;
				case "TVR":
					return LocationEnum.TVR;
				default:
					return LocationEnum.Other;
			}
		}

		private FieldTypeEnum GetFieldTypeEnum (string str) {
			str = str.ToUpper ();
			switch (str) {
				case "C":
					return FieldTypeEnum.Combo;
				case "A":
				case "L":
					return FieldTypeEnum.Lookup;
				case "D":
					return FieldTypeEnum.Date;
				case "T":
					return FieldTypeEnum.Time;
				case "N":
					return FieldTypeEnum.Integer;
				case "R":
				case "F":
					return FieldTypeEnum.Decimal;
				case "S":
					return FieldTypeEnum.String;
				case "B":
					return FieldTypeEnum.FullText;
				case "LS":
					return FieldTypeEnum.LongString;
				case "V":
					return FieldTypeEnum.Boolean;
				default:
					return FieldTypeEnum.Unknown;
			}
		}

		public int LocationId { get; set; }
		public LocationEnum LocationName { get; set; }
		public string LinkName { get; set; }
		public FieldTypeEnum LinkType { get; set; }
		public int ComboTypeId { get; set; }
		public string LinkDescription { get; set; }
		public string ComboTypeStoredProc { get; set; }
		public string LinkDefaultValue { get; set; }
		public string LinkNoSelection { get; set; }
		public int LinkFieldLength { get; set; }

		public LookupItems LookupItems { get; set; }

		public DynamicFieldHolder (DynamicFieldResponse dynamicFieldResponse) {
			this.LocationId = dynamicFieldResponse.FieldLocationId;
			this.LocationName = this.GetLocationEnum (dynamicFieldResponse.FieldLocationName);
			this.LinkName = dynamicFieldResponse.FieldLinkName.Capitalize ();
			this.LinkType = this.GetFieldTypeEnum (dynamicFieldResponse.FieldLinkType);
			this.ComboTypeId = dynamicFieldResponse.ComboTypeId.ToInt ().Value;
			this.LinkDescription = dynamicFieldResponse.FieldLinkDescription;
			this.ComboTypeStoredProc = dynamicFieldResponse.ComboTypeStoredProc;
			this.LinkDefaultValue = dynamicFieldResponse.FieldLinkDefaultValue;
			this.LinkNoSelection = dynamicFieldResponse.FieldLinkNoSelection;
			this.LinkFieldLength = dynamicFieldResponse.FieldLinkFieldLength;
			this.LookupItems = new LookupItems (dynamicFieldResponse.LookupItems);
		}

		public string Title {
			get {
				return Labels.Instance.GetLabel (this);
			}
		}

		public object GetValue (Model model, string linkName) {
			object result = model.GetPropertyValue (linkName);

			if (result == null || (result is String && String.IsNullOrEmpty ((string)result)))
				result = String.IsNullOrEmpty (this.LinkDefaultValue) ? null : this.LinkDefaultValue;

			switch (this.LinkType) {
				case FieldTypeEnum.Lookup:
					if (result is String) {
						int id;
						bool isId = Int32.TryParse ((string)result, out id);
						if (isId)
							result = id;
					}
					break;
				case FieldTypeEnum.Combo:
				case FieldTypeEnum.Integer:
					if (result is String)
						result = ((String)result).ToInt ();
					break;
				case FieldTypeEnum.Decimal:
					if (result is String)
						result = Convert.ToDouble (result, CultureInfo.InvariantCulture);
					break;
				case FieldTypeEnum.Boolean:
					result = Convert.ToBoolean (Convert.ToInt32 (result));
					break;
				case FieldTypeEnum.Date:
					if (result is String)
						result = ((string)result).ToDateTime (@"dd\/MM\/yyyy");
					break;
				case FieldTypeEnum.Time:
					if (result is String)
						result = ((string)result).ToTimeSpan (@"hh\:mm");
					break;
			}

			return result;
		}

		public T GetValue<T> (Model model, string linkName) {
			return (T) this.GetValue (model, linkName);
		}

		public string SerializeValue (Model model) {
			object result = model.GetPropertyValue (this.LinkName);

			if (result == null)
				return null;
			
			if (!(result is String)) {
				switch (this.LinkType) {
					case FieldTypeEnum.Date:
						result = ((DateTime)result).ToString (@"dd\/MM\/yyyy");
						break;
					case FieldTypeEnum.Time:
						result = ((TimeSpan)result).ToString (@"hh\:mm");
						break;
					case FieldTypeEnum.Decimal:
						result = ((double)result).ToString (CultureInfo.InvariantCulture);
						break;
				}
			}

			return result.ToString ();
		}

		public void SetValue (Model model, object value) {
			if (value is String
				&& (this.LinkType != FieldTypeEnum.String
					|| this.LinkType != FieldTypeEnum.LongString)) {
				switch (this.LinkType) {
					case FieldTypeEnum.Integer:
					case FieldTypeEnum.Boolean:
						value = Convert.ToInt32 (value);
						break;
					case FieldTypeEnum.Decimal:
						value = Convert.ToDouble (value);
						break;
					case FieldTypeEnum.Date:
						value = ((string)value).ToDateTime (@"dd\/MM\/yyyy");
						break;
					case FieldTypeEnum.Time:
						value = ((string)value).ToTimeSpan (@"hh\:mm");
						break;
				}
			} else if (this.LinkType == FieldTypeEnum.Boolean && value is Boolean)
				value = (bool)value ? 1 : 0;

			model.SetPropertyValue (
				this.LinkName,
				value
			);
		}
	}
}