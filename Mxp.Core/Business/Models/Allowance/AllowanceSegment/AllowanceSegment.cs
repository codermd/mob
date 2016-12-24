using System;
using System.Globalization;
using Mxp.Utils;
using Mxp.Core.Services.Responses;
using System.Linq;

namespace Mxp.Core.Business
{
	public partial class AllowanceSegment : Model, ICountriesFor
	{
		public override void NotifyPropertyChanged (string name) {
			base.NotifyPropertyChanged (name);

			if (this.GetCollectionParent<AllowanceSegments, AllowanceSegment> () != null) {
				if (!name.Equals ("ResetChanged"))
					this.AddModifiedObject (name);
				
				this.GetCollectionParent<AllowanceSegments, AllowanceSegment> ().NotifyPropertyChanged (name);
			}
		}

		public string Comment { get; set; }
		public string Location { get; set; }
		public double Amount { get; set; }

		public bool Breakfast { get; set; }
		public bool Lunch { get; set; }
		public bool Dinner { get; set; }
		public bool Lodging { get; set; }
		public bool Info { get; set; }
		public bool WorkNight { get; set; }

		public DateTime? DateFrom { get; set; }
		public string TimeFrom {
			get {
				return this.DateFrom.Value.ToString("t", DateTimeFormatInfo.InvariantInfo);
			}
		}
		public DateTime? DateTo { get; set; }
		public string TimeTo {
			get {
				return this.DateTo.Value.ToString("t", DateTimeFormatInfo.InvariantInfo);
			}
		}

		public Currency Currency { 
			get {
				return this.GetModelParent<AllowanceSegment, Allowance> ().Currency;
			}
		}

		public int CountryId { get; set; }
		public Country Country { get; set; }
		public double netAmountCC { get; set; }
		public double vatAmountCC { get; set; }
		public double legalAmountCC { get; set; }
		public int productId { get; set; }
		public int quantity { get; set; }

		public bool CanShowBreakfast { get; set; }
		public bool CanShowLunch { get; set; }
		public bool CanShowDinner { get; set; }
		public bool CanShowLodging { get; set; }
		public bool CanShowInfo { get; set; }
		public bool CanShowWorkNight { get; set; }

		public int timeFromTicks { get; set;}
		public int timeToTicks { get; set;}

		public AllowanceSegment (AllowanceSegmentResponse allowanceSegmentResponse) {
			this.Amount = Math.Round (allowanceSegmentResponse.grossAmountCC, 2);

			this.Country = LoggedUser.Instance.Countries.SingleOrDefault (country => country.Id == allowanceSegmentResponse.countryId);
			this.CountryId = allowanceSegmentResponse.countryId;

			this.Comment = allowanceSegmentResponse.journeySegmentPurpose;
			this.Location = allowanceSegmentResponse.journeySegmentLocation;
			this.Breakfast = allowanceSegmentResponse.journeySegmentIndicator1;
			this.Lunch = allowanceSegmentResponse.journeySegmentIndicator2;
			this.Dinner = allowanceSegmentResponse.journeySegmentIndicator3;
			this.Lodging = allowanceSegmentResponse.journeySegmentIndicator4;
			this.Info = allowanceSegmentResponse.journeySegmentIndicator5;
			this.WorkNight = allowanceSegmentResponse.journeySegmentIndicator6;

			this.DateFrom = allowanceSegmentResponse.journeySegmentDateTimeFrom.ToDateTime();
			this.DateTo = allowanceSegmentResponse.journeySegmentDateTimeTo.ToDateTime();

			this.CanShowBreakfast = allowanceSegmentResponse.i1Display;
			this.CanShowLunch = allowanceSegmentResponse.i2Display;
			this.CanShowDinner = allowanceSegmentResponse.i3Display;
			this.CanShowLodging = allowanceSegmentResponse.i4Display;
			this.CanShowInfo = allowanceSegmentResponse.i5Display;
			this.CanShowWorkNight = allowanceSegmentResponse.i6Display;

			this.netAmountCC = allowanceSegmentResponse.netAmountCC;
			this.vatAmountCC = allowanceSegmentResponse.vatAmountCC;
			this.legalAmountCC = allowanceSegmentResponse.legalAmountCC;
			this.productId = allowanceSegmentResponse.productId;
			this.quantity = allowanceSegmentResponse.quantity;

			this.timeFromTicks = allowanceSegmentResponse.journeySegmentDateTimeFromTicks;
			this.timeToTicks = allowanceSegmentResponse.journeySegmentDateTimeToTicks;

			this.ResetChanged ();
		}

		public override bool Equals (object obj) {
			if (obj == null || !(obj is AllowanceSegment))
				return false;

			AllowanceSegment allowanceSegment = (AllowanceSegment)obj;

			return this.DateFrom.Value == allowanceSegment.DateFrom.Value
				&& this.DateTo.Value == allowanceSegment.DateTo.Value
				&& this.Country == allowanceSegment.Country
				&& this.Location == allowanceSegment.Location
				&& this.Comment == allowanceSegment.Comment
				&& this.Breakfast == allowanceSegment.Breakfast
				&& this.Lunch == allowanceSegment.Lunch
				&& this.Dinner == allowanceSegment.Dinner
				&& this.Lodging == allowanceSegment.Lodging
				&& this.Info == allowanceSegment.Info
				&& this.WorkNight == allowanceSegment.WorkNight;
		}

		#region ICountriesFor

		public Countries Countries {
			get {
				return LoggedUser.Instance.Countries.ForAllowance;
			}
		}

		#endregion
	}
}