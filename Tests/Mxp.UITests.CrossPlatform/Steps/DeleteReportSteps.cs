using FluentAssertions;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
    [Binding, Scope(Feature = "DeleteReport")]
    public class DeleteReportSteps : StepsBase
    {
        [When("There is a report")]
        public void ThereIsAReport()
        {
            _testHelper.CreateAReport();
            _testHelper.SaveReport();
        }

        [When("I select it")]
        public void ISelectIt()
        {
            _testHelper.SelectReport(new SelectReportRequest());
        }

        [When("I delete it")]
        public void IDeleteIt()
        {
            _testHelper.DeleteReport();
        }

        [Then("The report is deleted")]
        public void ReportIsDeleted()
        {
            _testHelper.ReportIsDeleted(new ReportIsDeletedRequest()).Should().BeTrue();
        }
    }
}
