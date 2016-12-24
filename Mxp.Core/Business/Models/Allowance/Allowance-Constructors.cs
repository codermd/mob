using System;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public partial class Allowance
	{
		private Allowance () {
			this.DateFrom = DateTime.Now;
			this.DateFrom = this.DateFrom.Date + new TimeSpan (9, 00, 00);

			this.DateTo = DateTime.Now;
			this.DateTo = this.DateTo.Date + new TimeSpan (20, 00, 00);

			this.AllowanceSegments = new AllowanceSegments (this);
		}

		public Allowance (ExpenseResponse expenseResponse) : this () {
			this.Populate (expenseResponse);
		}

		public static new Allowance NewInstance () {
			Allowance allowance = new Allowance ();
			allowance.ExpenseItems.AddItem (new ExpenseItem ());
			return allowance;
		}

		public override void Populate (ExpenseResponse expenseResponse) {
			base.Populate (expenseResponse);

			this.GrossAmountCC = expenseResponse.fldItemGrossAmountCC;

			this.ResetChanged ();
		}

		public void Populate (AllowanceResponse allowanceResponse, bool silent = false) {
			this.ALLSIprojectId = allowanceResponse.projectId;
			this.ALLSIdepartmentId = allowanceResponse.departmentId;
			this.ALLSItrqId = allowanceResponse.travelRequestId;

			this.ExpenseItems [0].Infochar1 = allowanceResponse.ItemInfoChar1;
			this.ExpenseItems [0].Infochar2 = allowanceResponse.ItemInfoChar2;
			this.ExpenseItems [0].Infochar3 = allowanceResponse.ItemInfoChar3;
			this.ExpenseItems [0].Infochar4 = allowanceResponse.ItemInfoChar4;
			this.ExpenseItems [0].Infochar5 = allowanceResponse.ItemInfoChar5;
			this.ExpenseItems [0].Infochar6 = allowanceResponse.ItemInfoChar6;
			this.ExpenseItems [0].Infochar7 = allowanceResponse.ItemInfoChar7;
			this.ExpenseItems [0].Infochar8 = allowanceResponse.ItemInfoChar8;
			this.ExpenseItems [0].Infonum1 = allowanceResponse.ItemInfoNum1;
			this.ExpenseItems [0].Infonum2 = allowanceResponse.ItemInfoNum2;

			this.GrossAmountCC = allowanceResponse.grossAmountCC;
			this.journeyId = allowanceResponse.journeyId;
			this.transactionId = allowanceResponse.transactionId;
			this.itemId = allowanceResponse.itemId;
			this.employeeId = allowanceResponse.employeeId;
			this.journeyName = allowanceResponse.journeyName;
			this.journeyPurpose = allowanceResponse.journeyPurpose;
			this.netAmountCC = allowanceResponse.netAmountCC;
			this.vatAmountCC = allowanceResponse.vatAmountCC;
			this.legalAmountCC = allowanceResponse.legalAmountCC;
			this.transactionComments = allowanceResponse.transactionComments;
			this.Comment = allowanceResponse.transactionComments;
			this.AllowanceSegments.Populate (allowanceResponse.segments);

			if (!silent)
				this.ResetChanged ();

			this.NotifyPropertyChanged ("Populate");
		}
	}
}