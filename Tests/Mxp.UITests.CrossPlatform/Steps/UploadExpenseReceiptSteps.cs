using FluentAssertions;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
    [Binding,Scope(Feature = "UploadExpenseReceipt")]
    public class UploadExpenseReceiptSteps : StepsBase
    {
        [When("I create an expense")]
        public void ICreateAnExpense()
        {
            _testHelper.CreateABasicExpense();
        }

        [When("I upload an image")]
        public void IUploadAnImage()
        {
            _testHelper.AddReceiptToExpense();
        }

        [When("I press save")]
        public void IPressSave()
        {
            _testHelper.SaveExpense();
        }

        [Then("the receipt is saved")]
        public void TheReceiptIsSaved()
        {
            _testHelper.ExpenseIsSaved(new ExpenseIsSavedRequest()).Should().BeTrue();
        }
    }
}
