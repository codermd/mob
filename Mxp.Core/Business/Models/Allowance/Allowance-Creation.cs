using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public partial class Allowance
	{
		private DateTime _dateFrom;
		public DateTime DateFrom { 
			get {
				return _dateFrom;
			} 
			set {
				this._dateFrom = value;
				this.NotifyPropertyChanged ("DateFrom");
			}
		}

		private DateTime _dateTo;
		public DateTime DateTo { 
			get {
				return _dateTo;
			} 
			set {
				this._dateTo = value;
				this.NotifyPropertyChanged ("DateTo");
			}
		}

		public string Location;
		public bool Breakfast;
		public bool Lunch;
		public bool Dinner;
		public bool Lodging;
		public bool Info;
		public bool WorkNight;

		public int journeyId { get; set; }
		public int transactionId { get; set; }
		public int itemId { get; set; }
		public int employeeId { get; set; }
		public string journeyName { get; set; }
		public string journeyPurpose { get; set; }
		public double netAmountCC { get; set; }
		public double vatAmountCC { get; set; }
		public double legalAmountCC { get; set; }
		public string transactionComments { get; set; }
	}
}