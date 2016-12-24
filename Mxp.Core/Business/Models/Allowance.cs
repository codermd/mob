using System;
using Mxp.Service.Responses;
using Mxp.Service.Expense;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Mxp.Business
{
	public class Allowance : Expense
	{
		public const int PAYEMENT_METHOD_ID = 13;

		public string ItemInfoChar1 { get; set; }
		public string ItemInfoChar2 { get; set; }
		public string ItemInfoChar3 { get; set; }
		public int ItemInfoNum1 { get; set; }
		public int ItemInfoNum2 { get; set; }
		public double grossAmountCC { get; set; }
//		public string transactionComments { get; set; } -> see commentd
		public Currency currency { get; set; }

		public Collection<AllowanceSegment> segments;



//		public int departmentId { get; set; }
//		public int employeeId { get; set; }
//		public string journeyName { get; set; }
//		public string journeyPurpose { get; set; }
//		public double legalAmountCC { get; set; }
//		public double netAmountCC { get; set; }
//		public int projectId { get; set; }
//		public int transactionId { get; set; }
//		public int travelRequestId { get; set; }
//		public double vatAmountCC { get; set; }
	

		public Allowance (ExpenseResponse expenseResponse) : base (expenseResponse)
		{
			this.IsSplit = false;

			this.segments = new Collection<AllowanceSegment> ();
		}

		public override bool CanShowReceipt {
			get {
				return false;
			}
		}

		public async Task populateAllowance(){
			AllowanceResponse response = await AllowanceService.Instance.FetchAllowance (this);

			this.ItemInfoChar1 = response.ItemInfoChar1;
			this.ItemInfoChar2 = response.ItemInfoChar2;
			this.ItemInfoChar3 = response.ItemInfoChar3;
			this.ItemInfoNum1 = response.ItemInfoNum1;
			this.ItemInfoNum2 = response.ItemInfoNum2;
			this.grossAmountCC = response.grossAmountCC;
//			this.transactionComments = response.transactionComments;


			this.Comment = response.transactionComments;
			this.currency = LoggedUser.Instance.Currency;

			this.segments.Clear ();

			foreach (AllowanceSegmentResponse res in response.segments) {
				this.segments.Add (new AllowanceSegment(res));
			}

//			this.departmentId = response.departmentId;
//			this.employeeId = response.employeeId;
//			this.journeyName = response.journeyName;
//			this.journeyPurpose = response.journeyPurpose;
//			this.legalAmountCC = response.legalAmountCC;
//			this.netAmountCC = response.netAmountCC;
//			this.projectId = response.projectId;
//			this.transactionId = response.transactionId;
//			this.travelRequestId = response.travelRequestId;
//			this.vatAmountCC = response.vatAmountCC;




		}

	}
}