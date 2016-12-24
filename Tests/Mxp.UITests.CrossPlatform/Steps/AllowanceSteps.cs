using FluentAssertions;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
    [Binding]
    public class AllowanceSteps : StepsBase
    {
        [When(@"I create a new allowance in ""(.*)""")]
        public void ICreateANewAllowance(string country)
        {
            _testHelper.CreateAllowance(new CreateAllowanceRequest {Country = country});
        }

        [When("I save it"), Scope(Feature = "CreateAllowance")]
        public void SaveAllowance()
        {
            _testHelper.SaveAllowance();
        }

        [Then("The allowance is saved")]
        public void AllowanceIsSaved()
        {
            _testHelper.AllowanceIsSaved(new AllowanceIsSavedRequest()).Should().BeTrue();
        }
    }
}
