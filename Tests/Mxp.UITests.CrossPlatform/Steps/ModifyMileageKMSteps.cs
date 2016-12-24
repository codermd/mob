using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
    [Binding]
    public class ModifyMileageKMSteps : StepsBase
    {
        [Given(@"there is a mileage from ""(.*)"" to ""(.*)"" with ""(.*)"" ""(.*)""")]
        public void ICreateANewMileage(string startcity, string endcity, int km, string label)
        {
            _testHelper.CreateAMilleage(new CreateAMilleageRequest {StartCity = startcity, EndCity = endcity});
            _testHelper.SetDistance(new SetDistanceRequest { Label = label, KM = km });
            _testHelper.SaveMileage();
            _testHelper.SelectFirstMileage();
        }

        [When(@"I set ""(.*)"" to ""(.*)""")]
        public void SetDistance(string label,int km)
        {
            _testHelper.SetDistance(new SetDistanceRequest {Label = label, KM = km});
        }
    }
}
