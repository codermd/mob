using FluentAssertions;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
    [Binding]
    public class LoginSteps : StepsBase
    {
        [Given("I am on Login Screen")]
        public void LoginScreen()
        {

        }

        [When(@"I enter credential ""(.*)"" ""(.*)""")]
        public void EnterCredentials(string username, string password)
        {
            _testHelper.EnterLoginData(new EnterLoginDataRequest {UserName = username, Password = password});
        }

        [When("I press Login")]
        public void IPressLogin()
        {
            _testHelper.TapLoginButton();
        }

        [Then(@"I should be logged")]
        public void ISee()
        {
            _testHelper.IsLogged().Should().BeTrue();
        }
    }
}
