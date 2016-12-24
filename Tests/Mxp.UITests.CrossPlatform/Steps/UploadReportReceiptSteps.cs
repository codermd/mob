using FluentAssertions;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
	[Binding,Scope(Feature = "UploadReportReceipt")]
	public class UploadReportReceiptSteps : StepsBase
	{
		[When("I create a report")]
		public void ICreateAReport()
		{
			_testHelper.CreateAReport();
		}

		[When("I upload an image")]
		public void IUploadAnImage()
		{
			_testHelper.AddReceiptToReport();
		}

		[When("I press save")]
		public void IPressSave()
		{
			_testHelper.SaveReport();
		}

		[Then("the report is saved")]
		public void TheReportIsSaved()
		{
			_testHelper.ReportIsSaved(new ReportIsSavedRequest()).Should().BeTrue();
		}
	}
}
