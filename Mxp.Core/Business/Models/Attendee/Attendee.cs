using System;
using System.Threading.Tasks;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using RestSharp.Portable;
using Mxp.Core.Helpers;
using Mxp.Utils;
using System.Linq;

namespace Mxp.Core.Business
{
	public enum AttendeeTypeEnum {
		None,
		Business,
		Employee,
		Personal,
		Spouse,
		HCO,
		HCP,
		UCP
	}

	public partial class Attendee : Model, ICountriesFor
	{
		public int Id { get; set; }
		public string Address { get; set; }
		private double amountLC;
		public double AmountLC {
			get {
				return Math.Round(amountLC, 2);
			}
			set {
				amountLC = value;
			}
		}

		// FIXME Refactor
		public bool FromRelatedMode = false;

		public string City { get; set; }
		public bool Company { get; set; }
		public string CompanyName { get; set; }
		public string ExternalSource { get; set; }
		public string ExternalReference { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; } 

		public string Name { get; set; }

		public string Infochar1 { get; set; }
		public string Infochar2 { get; set; }
		public string Infochar3 { get; set; }
		public int? Infonum1 { get; set; }
		public int? Infonum2 { get; set; }

		public DateTime? LastUpdateOn { get; set; }
		public string LastUpdateBy { get; set; }
		public double? Quantity { get; set; }
		public string Reference { get; set; }
		public string Specialty { get; set; }
		public string State { get; set; }
		public string Title { get; set; }

		public AttendeeTypeEnum Type { get; set; } 

		public string VersionLock { get; set; }
		public int? ZipCode { get; set; }
		public int? EmployeeId { get; set; }
		public int ItemId { get; set; }

		public int[] SelectableCountries {
			get {
				return new int[] { 20, 51, 40, 56, 100, 112, 756, 196, 203, 280, 208, 724, 246, 234, 250, 826, 304, 300, 348, 372, 380, 398, 417, 442, 492, 528, 578, 616, 620, 642, 643, 752, 703, 792, 804, 860 };
			}
		}

		private Countries _countries;
		public Countries Countries {
			get {
				if (this._countries == null)
					this._countries = new Countries (LoggedUser.Instance.Countries.Where (country => this.SelectableCountries.Contains (country.Id)));

				return this._countries;
			}
		}

		private Country _country;
		public Country Country {
			get {
				if (this._country == null)
					this._country = this.Countries.SingleOrDefault (country => country.Id == Preferences.Instance.FldCountryId);
				
				if (this._country == null)
					this._country = this.Countries.First ();

				return this._country;
			}
			set {
				this._country = value;
			}
		}

		public bool IsShown {
			get {
				return this.Infonum1.HasValue ? !Convert.ToBoolean (this.Infonum1.Value) : true;
			}
			set {
				this.Infonum1 = Convert.ToInt32 (!value);
			}
		}

		public Attendee (AttendeeTypeEnum type) {
			this.Type = type;
		}
			
		public Attendee (string firstname, string lastname, string company) : this (AttendeeTypeEnum.Business) {
			this.Firstname = firstname;
			this.Lastname = lastname;
			this.CompanyName = company;
		}

		public Attendee (int emplyeeId, string name) : this (AttendeeTypeEnum.Employee) {
			this.EmployeeId = emplyeeId;
			this.Name = name;
		}

		public Attendee (string firstname, string lastname) : this (AttendeeTypeEnum.Spouse) {
			this.Firstname = firstname;
			this.Lastname = lastname;
		}

		public Attendee (string firstname, string lastname, int? zipcode, string city, string state, string speciality, string npi) : this (AttendeeTypeEnum.HCP) {
			this.Firstname = firstname;
			this.Lastname = lastname;
			this.ZipCode = zipcode;
			this.City = city;
			this.State = state;
			this.Specialty = speciality;
			this.Reference = npi;
		}

		public Attendee (string company, int? zipcode, string city, string state, string speciality, string npi) : this (AttendeeTypeEnum.HCO) {
			this.CompanyName = company;
			this.ZipCode = zipcode;
			this.City = city;
			this.State = state;
			this.Specialty = speciality;
			this.Reference = npi;
		}

		public Attendee (string firstname, string lastname, string company, string address, int? zipcode, string city, string state, string speciality, string npi) : this (AttendeeTypeEnum.UCP) {
			this.Firstname = firstname;
			this.Lastname = lastname;
			this.CompanyName = company;
			this.Address = address;
			this.ZipCode = zipcode;
			this.City = city;
			this.State = state;
			this.Specialty = speciality;
			this.Reference = npi;
		}

		// FIXME WTF ?
		public Attendee (LookupItemResponse response) {
			this.Firstname = response.Name;
			this.Id = Convert.ToInt32(response.Id);
		}
	}
}