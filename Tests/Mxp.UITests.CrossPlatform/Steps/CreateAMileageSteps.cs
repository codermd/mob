using FluentAssertions;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
    [Binding]
    public class CreateAMileageSteps : StepsBase
    {
        [When(@"I create a new mileage from ""(.*)"" to ""(.*)"""), Scope(Tag = "mileage")]
        public void CreateANewMileage(string startcity, string endcity)
        {
            _testHelper.CreateAMilleage(new CreateAMilleageRequest{StartCity = startcity, EndCity = endcity});
        }

        [When("I save it"), Scope(Tag = "mileage")]
        public void SaveMileage()
        {
            _testHelper.SaveMileage();
        }

        [Then("The mileage is saved"), Scope(Tag = "mileage")]
        public void MileageIsSaved()
        {
            _testHelper.MileageIsSaved(new MileageIsSavedRequest()).Should().BeTrue();
        }

        [Then(@"""(.*)"" distance is ""(.*)"""), Scope(Tag = "mileage")]
        public void MileageDistanceIscorrect(string label, int value)
        {
            _testHelper.GetDistance(new GetDistanceRequest {Label = label}).Should().Be(value);
        }

    }
}
