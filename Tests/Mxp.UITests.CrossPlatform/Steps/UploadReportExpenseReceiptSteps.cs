using FluentAssertions;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
	[Binding, Scope(Feature = "UploadReportExpenseReceipt")]
	public class UploadReportExpenseReceiptSteps : StepsBase
	{
		[When("I create a new report with expense")]
		public void ICreateANewReportWithExpense()
		{
			_testHelper.CreateAReport();
			_testHelper.SaveReport();
		}

		[When("I add a receipt to the report expense")]
		public void IAddAReceiptToTheReportExpense()
		{
            // Create an expense and add it to the report
            _testHelper.CreateABasicExpense(true);
            _testHelper.SaveExpense();
            _testHelper.AddExpenseToReport(new AddExpenseToReportRequest());
		}

		[Then("the receipt is saved")]
		public void TheReceiptIsSaved()
		{
			_testHelper.ReportExpenseReceiptSaved(new ReportExpenseReceiptSavedRequest()).Should().BeTrue();
		}
	}
}
