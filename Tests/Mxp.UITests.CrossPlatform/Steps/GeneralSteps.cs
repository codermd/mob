using FluentAssertions;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
    [Binding]
    public class GeneralSteps : StepsBase
    {
        [Given(@"I am logged with ""(.*)"" ""(.*)""")]
        public void IAmLogged(string username, string password)
        {
            _testHelper.Loging(new LogingRequest {UserName = username, Password = password}).Should().BeTrue();
        }

        [When(@"I switch ""(.*)""")]
        public void Switch(string label)
        {
            _testHelper.ToggleSwitch(new ToggleSwitchRequest {Label = label});
        }

        [Then(@"Switch ""(.*)"" is set")]
        public void CheckSwitchIsSet(string label)
        {
            _testHelper.GetSwitchState(new GetSwitchStateRequest {Label = label}).Should().BeTrue();
        }

        [Then(@"Icon ""(.*)"" is set")]
        public void CheckIconIsSet(string label)
        {
            _testHelper.GetIconState(new GetIconStateRequest { Label = label }).Should().BeTrue();
        }

        [Then(@"Icon ""(.*)"" is not set")]
        public void CheckIconIsNotSet(string label)
        {
            _testHelper.GetIconState(new GetIconStateRequest { Label = label }).Should().BeTrue();
        }

        [Then(@"Switch ""(.*)"" is not set")]
        public void CheckSwitchIsNotSet(string label)
        {
            _testHelper.GetSwitchState(new GetSwitchStateRequest { Label = label }).Should().BeFalse();
        }

        [When(@"I enter ""(.*)"" as ""(.*)""")]
        public void EnterTextData(string value, string label)
        {
            _testHelper.EnterValue(new EnterValueRequest<string> {Label = label, Value = value});
        }

        [When(@"I select ""(.*)"" as ""(.*)""")]
        public void SelectData(string value, string label)
        {
            _testHelper.SelectValue(new SelectValueRequest { Label = label, Value = value });
        }

		[When(@"I select autocomplete ""(.*)"" as ""(.*)""")]
		public void SelectAutocompleteData(string value, string label)
		{
			_testHelper.SelectAutocompleteValue(new SelectAutocompleteValueRequest { Label = label, Value = value });
		}

        [Then(@"""(.*)"" is ""(.*)""")]
        public void CheckValue(string label, string value)
        {
            _testHelper.GetValue(new GetValueRequest { Label = label }).Should().Be(value);
        }
    }
}
