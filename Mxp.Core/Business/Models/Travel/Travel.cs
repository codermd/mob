using System;
using Mxp.Core.Services.Responses;
using Mxp.Utils;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using Mxp.Core.Helpers;

namespace Mxp.Core.Business
{
	public partial class Travel : Model
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string EmployeeFirstname { get; set; }
		public string EmployeeLastname { get; set; }
		public double MaxAmount { get; set; }
		public string Comment { get; set; }
		public string Additional { get; set; }
		public string Status { get; set; }

		public int ProjectID { get; set; }

		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public TravelCarRentals CarRentals { get; set; }
		public TravelFlights Flights { get; set; }
		public TravelStays Stays { get; set; }

		public string TravelRequestparchar1ov { get; set; }
		public string TravelRequestparchar2ov { get; set; }
		public string TravelRequestparchar3ov { get; set; }
		public string TravelRequestparchar4ov { get; set; }
		public string TravelRequestparchar5ov { get; set; }
		public string TravelRequestparchar6ov { get; set; }
		public string TravelRequestparchar7ov { get; set; }
		public string TravelRequestparchar8ov { get; set; }
		public string TravelRequestparchar9ov { get; set; }
		public string TravelRequestparchar10ov { get; set; }
		public string TravelRequestparint1ov { get; set; }
		public string TravelRequestparint2ov { get; set; }
		public string TravelRequestparint3ov { get; set; }
		public string TravelRequestparint4ov { get; set; }
		public string TravelRequestparint5ov { get; set; }
		public string TravelRequestparint6ov { get; set; }
		public string TravelRequestparint7ov { get; set; }
		public string TravelRequestparint8ov { get; set; }
		public string TravelRequestparint9ov { get; set; }
		public string TravelRequestparint10ov { get; set; }
		public double TravelRequestparamount1ov { get; set; }
		public double TravelRequestparamount2ov { get; set; }
		public double TravelRequestparamount3ov { get; set; }
		public double TravelRequestparamount4ov { get; set; }
		public double TravelRequestparamount5ov { get; set; }
		public double TravelRequestparamount6ov { get; set; }
		public double TravelRequestparamount7ov { get; set; }
		public double TravelRequestparamount8ov { get; set; }
		public double TravelRequestparamount9ov { get; set; }
		public double TravelRequestparamount10ov { get; set; }
		public int TravelRequestparind1ov { get; set; }
		public int TravelRequestparind2ov { get; set; }
		public int TravelRequestparind3ov { get; set; }
		public int TravelRequestparind4ov { get; set; }
		public int TravelRequestparind5ov { get; set; }
		public int TravelRequestparind6ov { get; set; }
		public int TravelRequestparind7ov { get; set; }
		public int TravelRequestparind8ov { get; set; }
		public int TravelRequestparind9ov { get; set; }
		public int TravelRequestparind10ov { get; set; }

		public string VEmployeeFullname {
			get {
				return this.EmployeeFirstname + " " + this.EmployeeLastname;
			}
		}

		public string VDateRange {
			get {
				return this.FromDate.GetValueOrDefault().ToString ("d") + " - " + this.ToDate.GetValueOrDefault().ToString ("d");
			}
		}
	
		private Travel () : base () {
			this.CarRentals = new TravelCarRentals (this);
			this.Flights = new TravelFlights (this);
			this.Stays = new TravelStays (this);
		}

		public Travel (TravelApproval parent) : this () {
			this.Approval = parent;
		}

		private WeakReferenceObject<TravelApproval> _approval;
		public TravelApproval Approval {
			get {
				return this._approval != null ? this._approval.Value : null;
			}
			private set {
				this._approval = new WeakReferenceObject<TravelApproval> (value);
			}
		}
			
		public void Populate (TravelResponse travelResponse) {
			this.Id = travelResponse.travelrequestID;
			this.Name = travelResponse.travelrequestName;
			this.EmployeeFirstname = travelResponse.employeeFirstname;
			this.EmployeeLastname = travelResponse.employeeLastname;
			this.MaxAmount = travelResponse.travelrequestMaxamount;
			this.Comment = travelResponse.travelrequestComments;
			this.Additional = travelResponse.travelrequestAdditional;
			this.Status = travelResponse.travelrequeststatus;
			this.FromDate = travelResponse.travelRequestStartDate.ToDateTime();
			this.ToDate = travelResponse.travelRequestEndDate.ToDateTime();

			this.CarRentals.Populate (travelResponse.CarRentals);
			this.Flights.Populate (travelResponse.Flights);
			this.Stays.Populate (travelResponse.Stays);

			this.ProjectID = travelResponse.ProjectID;

			this.TravelRequestparchar1ov = travelResponse.TravelRequestparchar1ov;
			this.TravelRequestparchar2ov = travelResponse.TravelRequestparchar2ov;
			this.TravelRequestparchar3ov = travelResponse.TravelRequestparchar3ov;
			this.TravelRequestparchar4ov = travelResponse.TravelRequestparchar4ov;
			this.TravelRequestparchar5ov = travelResponse.TravelRequestparchar5ov;
			this.TravelRequestparchar6ov = travelResponse.TravelRequestparchar6ov;
			this.TravelRequestparchar7ov = travelResponse.TravelRequestparchar7ov;
			this.TravelRequestparchar8ov = travelResponse.TravelRequestparchar8ov;
			this.TravelRequestparchar9ov = travelResponse.TravelRequestparchar9ov;
			this.TravelRequestparchar10ov = travelResponse.TravelRequestparchar10ov;
			this.TravelRequestparint1ov = travelResponse.TravelRequestparint1ov;
			this.TravelRequestparint2ov = travelResponse.TravelRequestparint2ov;
			this.TravelRequestparint3ov = travelResponse.TravelRequestparint3ov;
			this.TravelRequestparint4ov = travelResponse.TravelRequestparint4ov;
			this.TravelRequestparint5ov = travelResponse.TravelRequestparint5ov;
			this.TravelRequestparint6ov = travelResponse.TravelRequestparint6ov;
			this.TravelRequestparint7ov = travelResponse.TravelRequestparint7ov;
			this.TravelRequestparint8ov = travelResponse.TravelRequestparint8ov;
			this.TravelRequestparint9ov = travelResponse.TravelRequestparint9ov;
			this.TravelRequestparint10ov = travelResponse.TravelRequestparint10ov;
			this.TravelRequestparamount1ov = travelResponse.TravelRequestparamount1ov;
			this.TravelRequestparamount2ov = travelResponse.TravelRequestparamount2ov;
			this.TravelRequestparamount3ov = travelResponse.TravelRequestparamount3ov;
			this.TravelRequestparamount4ov = travelResponse.TravelRequestparamount4ov;
			this.TravelRequestparamount5ov = travelResponse.TravelRequestparamount5ov;
			this.TravelRequestparamount6ov = travelResponse.TravelRequestparamount6ov;
			this.TravelRequestparamount7ov = travelResponse.TravelRequestparamount7ov;
			this.TravelRequestparamount8ov = travelResponse.TravelRequestparamount8ov;
			this.TravelRequestparamount9ov = travelResponse.TravelRequestparamount9ov;
			this.TravelRequestparamount10ov = travelResponse.TravelRequestparamount10ov;
			this.TravelRequestparind1ov = travelResponse.TravelRequestparind1ov;
			this.TravelRequestparind2ov = travelResponse.TravelRequestparind2ov;
			this.TravelRequestparind3ov = travelResponse.TravelRequestparind3ov;
			this.TravelRequestparind4ov = travelResponse.TravelRequestparind4ov;
			this.TravelRequestparind5ov = travelResponse.TravelRequestparind5ov;
			this.TravelRequestparind6ov = travelResponse.TravelRequestparind6ov;
			this.TravelRequestparind7ov = travelResponse.TravelRequestparind7ov;
			this.TravelRequestparind8ov = travelResponse.TravelRequestparind8ov;
			this.TravelRequestparind9ov = travelResponse.TravelRequestparind9ov;
			this.TravelRequestparind10ov = travelResponse.TravelRequestparind10ov;
		}
	}
}