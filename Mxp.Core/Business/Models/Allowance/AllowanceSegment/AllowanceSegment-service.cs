using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public partial class AllowanceSegment
	{
		public Dictionary<string, object> SerializeSegment () {
			Dictionary<string, object> result = new Dictionary<string, object> ();

			result["Context"] = null;// null
			result ["countryId"] = this.Country.Id.ToString ();
			result ["grossAmountCC"] = this.Amount.ToString ();
			result ["i1Display"] = this.CanShowBreakfast.ToString().ToLower();
			result["i2Display"] = this.CanShowLunch.ToString().ToLower();
			result["i3Display"] = this.CanShowDinner.ToString().ToLower();
			result ["i4Display"] = this.CanShowLodging.ToString().ToLower();
			result["i5Display"] = this.CanShowInfo.ToString().ToLower();
			result ["i6Display"] = this.CanShowWorkNight.ToString().ToLower();
//			this.DateTo.GetValueOrDefault().ToString("hh:mm:ss tt zz");
//			dd/MM/yyyy
			result["journeySegmentDateTimeFrom"] = this.SerizalizeDate(this.DateFrom.GetValueOrDefault())+ " "+this.DateFrom.GetValueOrDefault().ToString("HH:mm:ss");
//			result["journeySegmentDateTimeFrom"] = this.DateFrom.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");// "03/02/2015 09:00:00"

			result["journeySegmentDateTimeFromTicks"] = this.timeFromTicks.ToString();// 1422954000

			result["journeySegmentDateTimeTo"] = this.SerizalizeDate(this.DateTo.GetValueOrDefault())+ " "+this.DateTo.GetValueOrDefault().ToString("HH:mm:ss");
//			result["journeySegmentDateTimeTo"] = this.DateTo.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");// "03/02/2015 20:00:00"
			result["journeySegmentDateTimeToTicks"] = this.timeToTicks.ToString();// 1422993600
			result ["journeySegmentIndicator1"] = this.Breakfast.ToString().ToLower();// false
			result["journeySegmentIndicator2"] = this.Lunch.ToString().ToLower();// false
			result["journeySegmentIndicator3"] = this.Dinner.ToString().ToLower();// false
			result["journeySegmentIndicator4"] = this.Lodging.ToString().ToLower();// false
			result["journeySegmentIndicator5"] = this.Info.ToString().ToLower();// true
			result["journeySegmentIndicator6"] = this.WorkNight.ToString().ToLower();// false
			result ["journeySegmentLocation"] = this.Location;
			result ["journeySegmentPurpose"] = this.Comment;

			result["legalAmountCC"] =  this.legalAmountCC.ToString();// 0
			result["netAmountCC"] = this.netAmountCC.ToString();// 0
			result["productId"] = this.productId.ToString();// 4560
			result["quantity"] =  this.quantity.ToString();// 660
			result ["vatAmountCC"] = 0;//null;// 0

			return result;
		}
	}
}