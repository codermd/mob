using System;
using System.Threading.Tasks;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using RestSharp.Portable;
using Mxp.Utils;
using System.Diagnostics;

namespace Mxp.Core.Business
{
	public partial class Attendee
	{
		public AttendeeResponse rawResponse;

		public Attendee (AttendeeResponse attendeeResponse) {
			this.rawResponse = attendeeResponse;

			this.Id = attendeeResponse.AttendeeID;
			this.Address = attendeeResponse.AttendeeAddress;
			this.AmountLC = attendeeResponse.AttendeeAmountLC.GetValueOrDefault ();
			this.City = attendeeResponse.AttendeeCity;
			this.Company = attendeeResponse.AttendeeCompany.GetValueOrDefault ();
			this.CompanyName = attendeeResponse.AttendeeCompanyName;
			this.ExternalSource = attendeeResponse.AttendeeExternalSource;
			this.ExternalReference = attendeeResponse.AttendeeExternalreference;
			this.Firstname = attendeeResponse.AttendeeFirstName;
			this.Lastname = attendeeResponse.AttendeeLastName;
			this.Name = attendeeResponse.AttendeeName;
			this.Infochar1 = attendeeResponse.AttendeeInfoChar1;
			this.Infochar2 = attendeeResponse.AttendeeInfoChar2;
			this.Infochar3 = attendeeResponse.AttendeeInfoChar3;
			this.Infonum1 = attendeeResponse.AttendeeInfoNum1;
			this.Infonum2 = attendeeResponse.AttendeeInfoNum2;
			this.LastUpdateOn = attendeeResponse.AttendeeLastupdateOn.ToDateTime ();
			this.LastUpdateBy = attendeeResponse.AttendeeLastupdateby;
			this.Quantity = attendeeResponse.AttendeeQuantity;
			this.Reference = attendeeResponse.AttendeeReference;
			this.Specialty = attendeeResponse.AttendeeSpecialty;
			this.State = attendeeResponse.AttendeeState;
			this.Title = attendeeResponse.AttendeeTitle;

			this.Type = this.ConfigureType (attendeeResponse.AttendeeType);

			this.VersionLock = attendeeResponse.AttendeeVersionLock;
			this.ZipCode = attendeeResponse.AttendeeZipCode.ToInt ();
			this.EmployeeId = attendeeResponse.EmployeeId;
			this.ItemId = attendeeResponse.ItemID;
//			this.IsShown = !attendeeResponse.NoShow;
		}

		public String AttendeeTypeSerializer () {
			switch (this.Type) {
				case AttendeeTypeEnum.Business:
					return "BUS";
				case AttendeeTypeEnum.Spouse:
					return "SPO";
				case AttendeeTypeEnum.UCP:
					return "HCU";
				case AttendeeTypeEnum.Employee:
					return "EMP";
				default:
					return null;
			}
		}

		public bool IsNew {
			get { 
				return this.Id == 0;
			}
		}

		public void HealtSerialize (RestRequest request, Attendees attendees) {
			request.AddParameter("AttendeeAddress", this.rawResponse.AttendeeAddress != null ? this.rawResponse.AttendeeAddress.ToString() : "");
			request.AddParameter("AttendeeAmountLC", this.rawResponse.AttendeeAmountLC != null ? this.rawResponse.AttendeeAmountLC.ToString() : "");
			request.AddParameter("AttendeeCity", this.rawResponse.AttendeeCity != null ? this.rawResponse.AttendeeCity.ToString() : "");
			request.AddParameter("AttendeeCompany", this.rawResponse.AttendeeCompany != null ? this.rawResponse.AttendeeCompany.ToString() : "");
			request.AddParameter("AttendeeCompanyName", this.rawResponse.AttendeeCompanyName != null ? this.rawResponse.AttendeeCompanyName.ToString() : "");
			request.AddParameter("AttendeeExternalSource", this.rawResponse.AttendeeExternalSource != null ? this.rawResponse.AttendeeExternalSource.ToString() : "");
			request.AddParameter("AttendeeExternalreference", this.rawResponse.AttendeeExternalreference != null ? this.rawResponse.AttendeeExternalreference.ToString() : "");
			request.AddParameter("AttendeeFirstName", this.rawResponse.AttendeeFirstName != null ? this.rawResponse.AttendeeFirstName.ToString() : "");
			request.AddParameter("AttendeeID", this.rawResponse.AttendeeID);
			request.AddParameter("AttendeeInfoChar1", this.rawResponse.AttendeeInfoChar1 != null ? this.rawResponse.AttendeeInfoChar1.ToString() : "");
			request.AddParameter("AttendeeInfoChar2", this.rawResponse.AttendeeInfoChar2 != null ? this.rawResponse.AttendeeInfoChar2.ToString() : "");
			request.AddParameter("AttendeeInfoChar3", this.rawResponse.AttendeeInfoChar3 != null ? this.rawResponse.AttendeeInfoChar3.ToString() : "");
			request.AddParameter("AttendeeInfoNum1", this.rawResponse.AttendeeInfoNum1 != null ? this.rawResponse.AttendeeInfoNum1.ToString() : "");
			request.AddParameter("AttendeeInfoNum2", this.rawResponse.AttendeeInfoNum2 != null ? this.rawResponse.AttendeeInfoNum2.ToString() : "");
			request.AddParameter("AttendeeLastName", this.rawResponse.AttendeeLastName != null ? this.rawResponse.AttendeeLastName.ToString() : "");
			request.AddParameter("AttendeeLastupdateOn", this.rawResponse.AttendeeLastupdateOn != null ? this.rawResponse.AttendeeLastupdateOn.ToString() : "");
			request.AddParameter("AttendeeLastupdateby", this.rawResponse.AttendeeLastupdateby != null ? this.rawResponse.AttendeeLastupdateby.ToString() : "");
			request.AddParameter("AttendeeName", this.rawResponse.AttendeeName != null ? this.rawResponse.AttendeeName.ToString() : "");
			request.AddParameter("AttendeeQuantity", this.rawResponse.AttendeeQuantity != null ? this.rawResponse.AttendeeQuantity.ToString() : "");
			request.AddParameter("AttendeeReference", this.rawResponse.AttendeeReference != null ? this.rawResponse.AttendeeReference.ToString() : "");
			request.AddParameter("AttendeeSpecialty", this.rawResponse.AttendeeSpecialty != null ? this.rawResponse.AttendeeSpecialty.ToString() : "");
			request.AddParameter("AttendeeState", this.rawResponse.AttendeeState != null ? this.rawResponse.AttendeeState.ToString() : "");
			request.AddParameter("AttendeeTitle", this.rawResponse.AttendeeTitle != null ? this.rawResponse.AttendeeTitle.ToString() : "");
			request.AddParameter("AttendeeType", this.rawResponse.AttendeeType != null ? this.rawResponse.AttendeeType.ToString() : "");
			request.AddParameter("AttendeeVersionLock", this.rawResponse.AttendeeVersionLock != null ? this.rawResponse.AttendeeVersionLock.ToString() : "");
			request.AddParameter("AttendeeZipCode", this.rawResponse.AttendeeZipCode != null ? this.rawResponse.AttendeeZipCode.ToString() : "");
			request.AddParameter("Context", this.rawResponse.Context != null ? this.rawResponse.Context.ToString() : "");
			request.AddParameter("EmployeeId", this.rawResponse.EmployeeId != null ? this.rawResponse.EmployeeId.ToString() : "");

			request.AddParameter("ItemID", attendees.GetParentModel<ExpenseItem>().Id.ToString());
		}

		public void Serialize (RestRequest request, Attendees attendees) {
			if (this.FromRelatedMode) {
				this.HealtSerialize (request, attendees);
				return;
			}

			if (this.IsNew)
				this.TryValidate ();
			
			Expense expense = this.GetModelParent<Attendee, ExpenseItem> ().ParentExpense;

			if (!this.IsNew)
				request.AddParameter ("AttendeeID", this.Id.ToString ());

			request.AddParameter("ItemID", attendees.GetParentModel<ExpenseItem>().Id.ToString());

			if (this.Type == AttendeeTypeEnum.Employee && this.EmployeeId != null && this.EmployeeId != 0)
				request.AddParameter("EmployeeId", this.EmployeeId.ToString());

			if (!String.IsNullOrWhiteSpace(this.Firstname))
				request.AddParameter("AttendeeFirstName", this.Firstname);

			if (!String.IsNullOrWhiteSpace (this.Lastname))
				request.AddParameter ("AttendeeLastName", this.Lastname);

			if (this.Type == AttendeeTypeEnum.Business && !String.IsNullOrWhiteSpace (this.CompanyName))
				request.AddParameter("AttendeeCompanyName", this.CompanyName);
				
			if (!String.IsNullOrWhiteSpace(this.Address))
				request.AddParameter("AttendeeAddress", this.Address);

			if (this.ZipCode.HasValue)
				request.AddParameter("AttendeeZipCode", this.ZipCode.Value.ToString());

			if (!String.IsNullOrWhiteSpace (this.City))
				request.AddParameter ("AttendeeCity", this.City);

			if (!String.IsNullOrWhiteSpace (this.State))
				request.AddParameter ("AttendeeState", this.State);

			if (!String.IsNullOrWhiteSpace (this.Specialty))
				request.AddParameter ("AttendeeSpecialty", this.Specialty);

			if (!String.IsNullOrWhiteSpace (this.Reference))
				request.AddParameter ("AttendeeReference", this.Reference);

			request.AddParameter("AttendeeType", this.AttendeeTypeSerializer());
		}

		public void SerializeRelatedAttendee (RestRequest request) {
			this.TryValidate ();

			if (!this.IsNew)
				request.AddParameter ("AttendeeID", this.Id);

			request.AddParameter ("AttendeeFirstName", this.Firstname);
			request.AddParameter ("AttendeeLastName", this.Lastname);

			if (this.Type.CanAddCompanyName ())
				request.AddParameter ("AttendeeCompanyName", this.CompanyName);

			if (this.ZipCode.HasValue)
				request.AddParameter ("AttendeeZipCode", this.ZipCode.Value);
			
			request.AddParameter ("AttendeeCity", this.City);
			request.AddParameter ("AttendeeState", this.State);
			request.AddParameter ("AttendeeSpecialty", this.Specialty);
			request.AddParameter ("AttendeeReference", this.Reference);

			if (this.Type.CanAddCountry ())
				request.AddParameter ("CountryID",  this.Country?.Id);
		}

		public override void TryValidate () {
			if (this.FormFields == null)
				return;
			
			foreach (Field field in this.FormFields)
				field.TryValidate ();
		}

		public AttendeeTypeEnum ConfigureType (string type) {
			if (type != null)
				type = type.Trim ();

			switch(type) {
				case "BUS":
				case "0":
				case null:
					return AttendeeTypeEnum.Business;
				case "EMP":
				case "1":
					return AttendeeTypeEnum.Employee;
				case "SPO":
					return AttendeeTypeEnum.Personal;
				case "HCO":
					return AttendeeTypeEnum.HCO;
				case "HCP":
					return AttendeeTypeEnum.HCP;
				case "UCP":
					return AttendeeTypeEnum.UCP;
				default:
					return AttendeeTypeEnum.None;
			}
		}

		// TODO Do it into property directly. Thank you.
		public async Task ChangeIsShownAsync () {
			this.IsShown = !this.IsShown;

			try {
				await AttendeeService.Instance.ChangeSwitchNoShowAsync (this);
			} catch (Exception e) {
				this.IsShown = !this.IsShown;
				throw (e);
			}
		}
	}
}