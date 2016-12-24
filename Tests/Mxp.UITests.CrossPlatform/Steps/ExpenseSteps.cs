using FluentAssertions;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;

namespace Mxp.UITests.CrossPlatform.Steps
{
    [Binding,Scope(Tag = "expense")]
    public class ExpenseSteps : StepsBase
    {
        [When(@"I create a new expense with category ""(.*)"" and country ""(.*)"" and ""(.*)"" quantity of ""(.*)"" ""(.*)""")]
        public void ICreateANewExpense(string category, string country, int quantity, decimal amount, string currency)
        {
            var iscommentmandatory = FeatureContext.Current.FeatureInfo.Title == "CreateRestaurantExpenseWithAttendees";
            var request = new CreateAnExpenseRequest
            {
                Category = category,
                Country = country,
                Quantity = quantity,
                Amount = amount,
                Currency = currency,
                IsCommentMandatory = iscommentmandatory
            };
            _testHelper.CreateAnExpense(request);
        }

        [When(@"I add a business relation ""(.*)"" ""(.*)"" from ""(.*)""")]
        public void AddBusinessRelation(string firstname, string lastname, string company)
        {
            var request = new AddBusinessRelationRequest
            {
                FirstName = firstname,
                LastName = lastname,
                Company = company
            };
            _testHelper.AddBusinessRelation(request);
        }

        [When(@"I add a spouse ""(.*)"" ""(.*)""")]
        public void AddSpouse(string firstname, string lastname)
        {
            var request = new AddSpouseRequest()
            {
                FirstName = firstname,
                LastName = lastname
            };
            _testHelper.AddSpouse(request);
        }

        [When(@"I add a HCP ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)""")]
        public void AddHCP(string firstname, string lastname, string address,string zip, string city, string state, string speciality, string npi)
        {
            var request = new AddHealthCareProviderRequest()
            {
                FirstName = firstname,
                LastName = lastname,
                Address = address,
                Zip = zip,
                City = city,
                State = state,
                Speciality = speciality,
                NPINumber = npi
            };
            _testHelper.AddHealthCareProvider(request);
        }

        [When(@"I add a HCO ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)""")]
        public void AddHCO(string company, string zip, string city, string state, string speciality, string npi)
        {
            var request = new AddHealthCareOrganizationRequest()
            {
                Company = company,
                Zip = zip,
                City = city,
                State = state,
                Speciality = speciality,
                NPINumber = npi
            };
            _testHelper.AddHealthCareOrganization(request);
        }

        [When(@"I add a UHCP ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)""")]
        public void AddUHCP(string firstname, string lastname, string company, string zip, string city, string state, string speciality, string npi)
        {
            var request = new AddUnrecognizedHealthCareProviderRequest()
            {
                FirstName = firstname,
                LastName = lastname,
                Company = company,
                Zip = zip,
                City = city,
                State = state,
                Speciality = speciality,
                NPINumber = npi
            };
            _testHelper.AddUnrecognizedHealthCareProvider(request);
        }

        [When("I save it")]
        public void SaveExpense()
        {
            _testHelper.SaveExpense();
        }

        [Then("The expense is saved")]
        public void ExpenseIsSaved()
        {
            _testHelper.ExpenseIsSaved(new ExpenseIsSavedRequest()).Should().BeTrue();
        }

        [When("There is an expense")]
        public void ThereIsAnExpense()
        {
            _testHelper.CreateABasicExpense();
            _testHelper.SaveExpense();
        }

        [When("I select it")]
        public void ISelectIt()
        {
            _testHelper.SelectExpense(new SelectExpenseRequest());
        }

        [When("I delete it")]
        public void IDeleteIt()
        {
            _testHelper.DeleteExpense();
        }

        [Then("The expense is deleted")]
        public void ExpenseIsDeleted()
        {
            _testHelper.ExpenseIsDeleted(new ExpenseIsDeletedRequest()).Should().BeTrue();
        }

        [Then(@"There are ""(.*)"" attendees")]
        public void CheckNumberOfAttendees(int number)
        {
            _testHelper.GetAttendeeCount(new GetAttendeeCountRequest()).Should().Be(number);
        }

        [When("I pull to refresh expenses list")]
        public void PullToRefreshExpensesList()
        {
            _testHelper.PullToRefresh();
        }
        [Then("Expenses list is refreshed")]
        public void ExpensesListIsRefreshed()
        {
            _testHelper.CheckErrorDialogShowing().Should().BeFalse();
        }

        [Then(@"The ""(.*)"" icon is set with ""(.*)"" counter on expense cell")]
        public void CheckExpenseIcon(string icon, int counter)
        {
            switch (icon)
            {
                case "attendee":
                    _testHelper.CheckAttendeeIcon(new CheckAttendeeIconRequest { Counter = counter });
                    break;
                case "receipt":
                    _testHelper.CheckReceiptIcon(new CheckReceiptIconRequest { Counter = counter });
                    break;
                default:
                    break;
            }
        }
    }
}
