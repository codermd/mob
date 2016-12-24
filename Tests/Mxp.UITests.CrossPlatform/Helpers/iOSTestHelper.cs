using System;
using System.Linq;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform.Helpers
{
    public class iOSTestHelper : TestHelperBase
    {
        public iOSTestHelper(IApp app) : base(app, Platform.iOS)
        {
			_app.Invoke("addImageToDevice:", "");
        }

        public override void TapFirstCell()
        {
            _app.Tap(x => x.Class("UITableViewCellContentView").Class("UIView"));
        }

        public override void TapLoginButton()
        {
            _app.DismissKeyboard();
            _app.Tap(x => x.Button("LOGIN"));
        }

        public override void EnterLoginData(EnterLoginDataRequest request)
        {
            _app.EnterText(x => x.Class("UITextField"), request.UserName);
            _app.EnterText(x => x.Class("UITextField").Index(1), request.Password);
        }

        public override void EnterAutocompleteValue(EnterAutocompleteValueRequest request)
        {
            WaitForElement(new WaitForElementRequest { Class = "UISearchBarTextField" });
            _app.EnterText(x => x.Class("UISearchBarTextField"), request.Value);
            WaitForElement(new WaitForElementRequest { Class = "UITableViewWrapperView" });

            _app.Tap(x => x.Class("UITableViewCellContentView"));
        }

        public override bool IsLogged()
        {
            // Check if there is a "Expenses" text on the page (meaning we are logged)
            WaitForElement(new WaitForElementRequest { Marked = "Expenses", ErrorMessage = "Error waiting for element" });
            return _app.Query(c => c.Text("Expenses")).Any();
        }

        public override string CreateAnExpense(CreateAnExpenseRequest request)
        {
            // Go to expense tab
            _app.Tap(x => x.Class("UITabBarButton").Marked("Expenses"));
            WaitForElement(new WaitForElementRequest { Marked = "SpendCatcher" });

            _app.Tap(x => x.Marked("Add"));
            _app.Tap(x => x.Marked("Expense"));
            // Select first expense type
            _app.ScrollDownTo(x => x.Marked(request.Category));
            _app.Tap(x => x.Marked(request.Category));
            WaitForElement(new WaitForElementRequest
            {
                Class = "UINavigationItemView",
                Marked = "2. Country"
            });
            _app.ScrollDownTo(x => x.Marked(request.Country));
            _app.Tap(x => x.Marked(request.Country));

            var amountrequest = new EnterValueRequest<decimal>
            {
                Label = "* Amount",
                Value = request.Amount
            };
            EnterValue(amountrequest);

            var currencyrequest = new SelectValueRequest
            {
                Label = "* Currency",
                Value = request.Currency
            };
            SelectValue(currencyrequest);

            var quantityrequest = new EnterValueRequest<decimal>
            {
                Label = "* Quantity",
                Value = request.Quantity
            };
            EnterValue(quantityrequest);

            _app.Tap(x => x.Text("Save"));

            //Add comment specific to be sure it is right expense we check in last step
            var timestamp = GetTimeStamp();

            if (request.IsCommentMandatory)
                _app.Tap(x => x.Marked("* Comment"));
            else
                _app.Tap(x => x.Marked("Comment"));
            WaitForElement(new WaitForElementRequest { Class = "UITextView" });
            _app.EnterText(x => x.Class("UITextView"), timestamp);
            _app.DismissKeyboard();
            _app.Tap(x => x.Text("Expense"));

            if (request.AddReceipt)
                AddReceiptToExpense();

            SaveData(Keys.ExpenseComment, timestamp);

            return timestamp;
        }


        public override void EnterValue<T>(EnterValueRequest<T> request)
        {
			_app.ScrollDownTo(x => x.Class("UILabel").Marked(request.Label).Sibling(0));
	
            _app.ClearText(x => x.Class("UILabel").Marked(request.Label).Sibling(0));
            _app.EnterText(x => x.Class("UILabel").Marked(request.Label).Sibling(0), request.Value.ToString());
            if (_app.Query(x => x.Marked("Done")).Any())
                _app.Tap(x => x.Marked("Done"));
        }

        public override void SelectValue(SelectValueRequest request)
        {
            _app.Tap(x => x.Marked(request.Label));

            if (string.IsNullOrEmpty(request.SearchControlId))
                Wait(TimeSpan.FromSeconds(2));//todo: check if can reduce it
            else
                WaitForElement(new WaitForElementRequest { Id = request.SearchControlId });

            _app.ScrollDownTo(x => x.Marked(request.Value));
            _app.DismissKeyboard();
            _app.Tap(x => x.Marked(request.Value));

            if (string.IsNullOrEmpty(request.SearchControlId))
                Wait(TimeSpan.FromSeconds(2));//todo: check if can reduce it
            else
                WaitForElement(new WaitForElementRequest { Id = request.SearchControlId });
        }

		public override void SelectAutocompleteValue(SelectAutocompleteValueRequest request)
		{
			_app.Tap(x => x.Marked(request.Label));

			if (string.IsNullOrEmpty(request.SearchControlId))
				Wait(TimeSpan.FromSeconds(2));//todo: check if can reduce it
			else
				WaitForElement(new WaitForElementRequest { Id = request.SearchControlId });
			
			EnterAutocompleteValue(new EnterAutocompleteValueRequest { Value = request.Value
			});

			Wait(TimeSpan.FromSeconds(2));//todo: check if can reduce it
		}

        public override void AddReceiptToExpense()
        {
            _app.Tap(x => x.Text("Receipts"));
            _app.Tap(x => x.Marked("+ Attach a receipt"));
            SelectImage();
            Wait(TimeSpan.FromSeconds(1));
        }

        public override void SaveExpense()
        {
            _app.Tap(x => x.Class("UIButtonLabel").Text("Save"));
            WaitForElement(new WaitForElementRequest {Class = "UIActivityIndicatorView" });
            WaitForNoElement(new WaitForElementRequest {Class = "UIActivityIndicatorView" });
        }

        public override bool ExpenseIsSaved(ExpenseIsSavedRequest request)
        {
            var timestamp = string.IsNullOrEmpty(request.ExpenseComment)
                ? GetData<string>(Keys.ExpenseComment)
                : request.ExpenseComment;

            WaitForElement(new WaitForElementRequest
            {
                Marked = "SpendCatcher",
                ErrorMessage = "Error finding SpendCatcher"
            });

            try
            {
                _app.ScrollUpTo(Guid.NewGuid().ToString());
            }
            catch (Exception e)
            {

            }

            TapFirstCell();
            WaitForElement(new WaitForElementRequest { Marked = "Details", ErrorMessage = "Error finding expense" });
            return _app.Query(c => c.Marked(timestamp)).Any();
        }

        public override string CreateAReport()
        {
            _app.Tap(x => x.Id("ReportsIconTab"));
            _app.Tap(x => x.Marked("Add"));

            //Add comment specific to be sure it is right expense we check in last step
            var timestamp = GetTimeStamp();

            WaitForElement(new WaitForElementRequest { Class = "UITextField" });
            _app.EnterText(x => x.Class("UITextField"), timestamp);
            _app.Tap(x => x.Class("UISegmentLabel").Text("Expenses").Marked("Expenses"));
            _app.Tap(x => x.Marked("Add expenses"));

            // Select first expense
            TapFirstCell();
            _app.Tap(x => x.Text("Select (1)"));

            SaveData(Keys.ReportComment, timestamp);

            return timestamp;
        }

        public override void AddReceiptToReport()
        {
            _app.Tap(x => x.Text("Receipts"));
            _app.Tap(x => x.Marked("+ Attach a receipt"));
            SelectImage();
            Wait(TimeSpan.FromSeconds(1));
        }

        public override void SaveReport()
        {
            _app.Tap(x => x.Text("Save"));
            Wait(TimeSpan.FromSeconds(1));
            _app.Tap(x => x.Text("Reports").Index(1));
        }

        public override bool ReportIsSaved(ReportIsSavedRequest request)
        {
            // Check if there is a report with the timestamp
            var timestamp = string.IsNullOrEmpty(request.ReportComment) ? GetData<string>(Keys.ReportComment) : request.ReportComment;

            WaitForElement(new WaitForElementRequest { Marked = "Draft" });

            _app.ScrollDownTo(x => x.Marked(timestamp));

            // if this line is reach, it means it scrolled to the right report
            return true;
        }

        public override void AddExpenseToReport(AddExpenseToReportRequest request)
        {
            var timestamp = string.IsNullOrEmpty(request.ReportComment) ? GetData<string>(Keys.ReportComment) : request.ReportComment;
            // Get to reports tab
            _app.Tap(x => x.Class("UITabBarButton").Marked("Reports"));
            WaitForElement(new WaitForElementRequest { Marked = "Draft" });

            _app.ScrollDownTo(timestamp);
            _app.Tap(x => x.Marked(timestamp));
            // Select expenses screen
            _app.Tap(x => x.Marked("Expenses"));
            WaitForElement(new WaitForElementRequest { Marked = "Add expenses" });
            // Add expense
            _app.Tap(x => x.Marked("Add expenses"));
            WaitForElement(new WaitForElementRequest { Id = "ExpensesIconTab" });
            _app.Tap(x => x.Id("ExpensesIconTab"));
            _app.Tap(x => x.Marked("Select (1)"));
            _app.Tap(x => x.Class("UINavigationItemButtonView").Marked("Reports"));
        }

        public override bool ReportExpenseReceiptSaved(ReportExpenseReceiptSavedRequest request)
        {
            var timestamp = string.IsNullOrEmpty(request.Reportcomment) ? GetData<string>(Keys.ReportComment) : request.Reportcomment;
            // Select report with timestamp and go to expenses
            _app.ScrollDownTo(x => x.Marked(timestamp), null, ScrollStrategy.Gesture, 0.77D); // have to use gesture and 0,77 or sometimes it stops too early and cannot tap it
            _app.Tap(x => x.Marked(timestamp));
            WaitForElement(new WaitForElementRequest { Marked = "Details" });
            _app.Tap(x => x.Class("UISegmentLabel").Text("Expenses").Marked("Expenses"));
            WaitForElement(new WaitForElementRequest { Marked = "Add expenses" });

            // Select first expense
            _app.Tap(x => x.Class("UITableViewCellContentView").Descendant().Id("ExpensesIconTab"));
            // check if there is one receipt linked
            WaitForElement(new WaitForElementRequest { Marked = "Receipts (1)" });
            return _app.Query(x => x.Text("Receipts (1)")).Any();
        }

        public override void SelectExpense(SelectExpenseRequest request)
        {
			_app.Tap(x => x.Class("UITabBarButtonLabel").Marked("Expenses"));
			WaitForElement(new WaitForElementRequest { Class = "UISegmentLabel", Marked = "SpendCatcher" });
            var timestamp = string.IsNullOrEmpty(request.ExpenseComment) ? GetData<string>(Keys.ExpenseComment) : request.ExpenseComment;
            TapFirstCell();
            WaitForElement(new WaitForElementRequest { Marked = timestamp, ErrorMessage = "Error finding expense" });
        }

        public override void ToggleSwitch(ToggleSwitchRequest request)
        {
            WaitForElement(new WaitForElementRequest
            {
                Class = "UILabel",
                Text = request.Label
            });
            _app.Tap(x => x.Class("UILabel").Text(request.Label).Sibling(0));
        }

        public override bool GetSwitchState(GetSwitchStateRequest request)
        {
            WaitForElement(new WaitForElementRequest
            {
                Class = "UILabel",
                Text = request.Label
            });

            var theswitch = _app.Query(x => x.Class("UILabel").Text(request.Label).Sibling(0).Property("On").Value<bool>());

            return theswitch.FirstOrDefault();
        }

        public override bool GetIconState(GetIconStateRequest request)
        {
            var theiconid = request.Label + (request.Label == "Dinner" ? "ICon" : "Icon"); // todo: correct ID DinnerICon in ios

            return _app.Query(x => x.Class("UIImageView").Id(theiconid).Invoke("isHighlighted").Value<int>()).FirstOrDefault() == 1;
        }

        public override void DeleteExpense()
        {
            _app.Tap(x => x.Marked("Share"));
            _app.Tap(x => x.Marked("Delete?"));
            _app.Tap(x => x.Marked("OK"));
        }

        public override void CancelExpense()
        {
            _app.Tap(x => x.Class("UIButtonLabel").Marked("Cancel"));
        }

        public override void DeleteAllowance()
        {
            _app.Tap(x => x.Marked("Delete"));
            _app.Tap(x => x.Marked("OK"));
        }

        public override void DeleteMilleagee()
        {
            DeleteAllowance();
        }

        public override bool ExpenseIsDeleted(ExpenseIsDeletedRequest request)
        {
            var timestamp = string.IsNullOrEmpty(request.ExpenseComment) ? GetData<string>(Keys.ExpenseComment) : request.ExpenseComment;

            WaitForElement(new WaitForElementRequest { Marked = "SpendCatcher", ErrorMessage = "Error finding SpendCatcher" });

            TapFirstCell();
            WaitForElement(new WaitForElementRequest { Marked = "Details", ErrorMessage = "Error finding expense" });
            return !_app.Query(c => c.Marked(timestamp)).Any();
        }

        public override void SelectReport(SelectReportRequest request)
        {
			_app.Tap(x => x.Class("UITabBarButtonLabel").Marked("Reports"));
			WaitForElement(new WaitForElementRequest { Class = "UISegmentLabel", Marked = "Draft" });
            var timestamp = string.IsNullOrEmpty(request.ReportComment) ? GetData<string>(Keys.ReportComment) : request.ReportComment;
            _app.ScrollDownTo(timestamp);
            _app.Tap(x => x.Marked(timestamp));
        }

        public override void DeleteReport()
        {
            _app.Tap(x => x.Marked("Share"));
            WaitForElement(new WaitForElementRequest { Marked = "Delete?" });
            _app.Tap(x => x.Marked("Delete?"));
            WaitForElement(new WaitForElementRequest { Marked = "OK" });
            _app.Tap(x => x.Marked("OK"));
            WaitForElement(new WaitForElementRequest { Marked = "Draft" });
        }

        public override bool ReportIsDeleted(ReportIsDeletedRequest request)
        {
            var timestamp = string.IsNullOrEmpty(request.ReportComment) ? GetData<string>(Keys.ReportComment) : request.ReportComment;
            try
            {
                _app.ScrollDownTo(timestamp);
            }
            catch (Exception e)
            {
                return true;
            }

            return false;
        }

        public override string CreateAMilleage(CreateAMilleageRequest request)
        {
            _app.Tap(x => x.Class("UITabBarButton").Marked("Expenses"));
            Wait(TimeSpan.FromSeconds(2));

            _app.Tap(x => x.Marked("Add"));
            WaitForElement(new WaitForElementRequest { Marked = "Mileage" });
            _app.Tap(x => x.Marked("Mileage"));

            WaitForElement(new WaitForElementRequest { Id = "AddMapMileage" });
            _app.Tap(x => x.Marked("Add"));

            EnterAutocompleteValue(new EnterAutocompleteValueRequest { Value = request.StartCity });

            WaitForElement(new WaitForElementRequest { Id = "AddMapMileage" });
            _app.Tap(x => x.Marked("Add"));

            EnterAutocompleteValue(new EnterAutocompleteValueRequest { Value = request.EndCity });

            WaitForElement(new WaitForElementRequest { Id = "AddMapMileage" });

            _app.ScrollDownTo("Comment");
            _app.Tap(x => x.Marked("Comment"));

            var timestamp = GetTimeStamp();
            WaitForElement(new WaitForElementRequest { Class = "UITextView" });
            _app.EnterText(x => x.Class("UITextView"), timestamp);

            _app.Tap(x => x.Text("Mileage"));

            WaitForElement(new WaitForElementRequest { Marked = "Save" });

            _app.ScrollUp(x => x.Marked("General"));

            WaitForElement(new WaitForElementRequest { Marked = "General" });

            SaveData(Keys.MileageComment, timestamp);

            return timestamp;
        }

        public override void SaveMileage()
        {
            _app.Tap(x => x.Text("Save"));
            WaitForElement(new WaitForElementRequest { Marked = "SpendCatcher" });
        }

        public override bool MileageIsSaved(MileageIsSavedRequest request)
        {
            var timestamp = string.IsNullOrEmpty(request.MileageComment) ? GetData<string>(Keys.MileageComment) : request.MileageComment;

            TapFirstCell();
            WaitForElement(new WaitForElementRequest { Marked = "Mileage", ErrorMessage = "Error finding expense" });
            return _app.Query(c => c.Marked(timestamp)).Any();
        }

        public override int GetDistance(GetDistanceRequest request)
        {
            _app.ScrollDownTo(x => x.Marked(request.Label));

            Wait(TimeSpan.FromSeconds(1));

            var cells = _app.Query(x => x.Marked(request.Label).Sibling(0));

            int cellvalue;

            if (int.TryParse(cells[0].Text, out cellvalue))
            {
                return cellvalue;
            }

            return -1;
        }

        public override void SetDistance(SetDistanceRequest request)
        {
            try
            {
                _app.ScrollDownTo(x => x.Marked(request.Label));
            }
            catch
            {
                _app.ScrollUpTo(x => x.Marked(request.Label));
            }

			Wait(TimeSpan.FromSeconds(1));

            var entervaluerequest = new EnterValueRequest<int>
            {
                Label = request.Label,
                Value = request.KM
            };
            EnterValue(entervaluerequest);
        }

        public override string CreateAllowance(CreateAllowanceRequest request)
        {
            _app.Tap(x => x.Class("UITabBarSwappableImageView").Id("ExpensesIconTab"));
            Wait(TimeSpan.FromSeconds(2));//todo: check if can reduce it

            _app.Tap(x => x.Marked("Add"));

            _app.Tap(x => x.Marked("Allowances"));

            WaitForElement(new WaitForElementRequest { Marked = "* Country" });
            _app.Tap(x => x.Marked("* Country"));

            _app.ScrollDownTo(request.Country);
            _app.Tap(x => x.Marked(request.Country));

            WaitForElement(new WaitForElementRequest { Marked = "* Country" });

            var timestamp = GetTimeStamp();
            _app.Tap(x => x.Marked("Comment"));
            WaitForElement(new WaitForElementRequest { Class = "UITextView" });
            _app.EnterText(x => x.Class("UITextView"), timestamp);
            _app.Tap(x => x.Text("New allowance"));
            WaitForElement(new WaitForElementRequest { Marked = "* Country" });

            SaveData(Keys.AllowanceComment, timestamp);
            return timestamp;
        }

        public override void SaveAllowance()
        {
            if (_app.Query(x => x.Text("Done")).Any())
                _app.Tap(x => x.Text("Done"));

            WaitForElement(new WaitForElementRequest { Marked = "Save" });
            _app.Tap(x => x.Text("Save"));
        }

        public override bool AllowanceIsSaved(AllowanceIsSavedRequest request)
        {
            TapFirstCell();

            var timestamp = string.IsNullOrEmpty(request.AllowanceComment) ? GetData<string>(Keys.AllowanceComment) : request.AllowanceComment;

            try
            {
                WaitForElement(new WaitForElementRequest { Id = "BE.png" });
                _app.Tap(x => x.Id("BE.png"));
                WaitForElement(new WaitForElementRequest { Marked = "Belgium" });
                WaitForElement(new WaitForElementRequest
                {
                    Class = "UILabel",
                    Text = timestamp
                });
                _app.Tap(x => x.Class("UINavigationItemButtonView")); // go back to main allowance screen
                WaitForElement(new WaitForElementRequest
                {
                    Class = "UINavigationItemButtonView",
                    Marked = "Expenses"
                });
                Wait(TimeSpan.FromSeconds(1)); // have to wait 1s or the screen is not well loaded and some queries can fail
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public override void SelectFirstMileage()
        {
			_app.Tap(x => x.Class("UITabBarButtonLabel").Marked("Expenses"));
			WaitForElement(new WaitForElementRequest { Class = "UISegmentLabel", Marked = "SpendCatcher" });
			TapFirstCell();
			WaitForElement(new WaitForElementRequest { Marked="SHOW MAP"});
        }

        public override string GetValue(GetValueRequest request)
        {
            try
            {
                _app.ScrollDownTo(x => x.Marked(request.Label));
            }
            catch (Exception e)
            {
                _app.ScrollUpTo(x => x.Marked(request.Label));
            }
            Wait(TimeSpan.FromSeconds(1)); // Seems sometimes text returns "", but it goes better after a pause
            return _app.Query(x => x.Class("UILabel").Marked(request.Label).Sibling(0)).First().Text;
        }

        public override void AddBusinessRelation(AddBusinessRelationRequest request)
        {
            // Don't know how many attendee yet, so not sure the text of the tab ("Attendee (0)", "Attendee (1)",...?)
            WaitForElement(new WaitForElementRequest { Class = "UISegment" });
            var attendeetab = _app.Query(x => x.Class("UISegment")).First(x => x.Label.StartsWith("Attendees"));
            _app.Tap(x => x.Marked(attendeetab.Label));
            Wait(TimeSpan.FromSeconds(1));
            _app.ScrollDownTo(x => x.Class("AddAttendeeButtonCell"));
            _app.Tap(x => x.Class("AddAttendeeButtonCell"));
            WaitForElement(new WaitForElementRequest { Marked = "Business Relation" });
            _app.Tap(x => x.Marked("Business Relation"));
            WaitForElement(new WaitForElementRequest { Marked = "First name" });
            _app.EnterText(x => x.Class("UITextField").Index(0), request.FirstName);
            _app.EnterText(x => x.Class("UITextField").Index(1), request.LastName);
            _app.EnterText(x => x.Class("UITextField").Index(2), request.Company);
            _app.Tap(x => x.Marked("Create"));
            WaitForElement(new WaitForElementRequest
            {
                Class = "UIButtonLabel",
                Marked = "Save"
            });
        }

        public override void AddSpouse(AddSpouseRequest request)
        {
            // Don't know how many attendee yet, so not sure the text of the tab ("Attendee (0)", "Attendee (1)",...?)
            WaitForElement(new WaitForElementRequest { Class = "UISegment" });
            var attendeetab = _app.Query(x => x.Class("UISegment")).First(x => x.Label.StartsWith("Attendees"));
            _app.Tap(x => x.Marked(attendeetab.Label));
            Wait(TimeSpan.FromSeconds(1));
            _app.ScrollDownTo(x => x.Class("AddAttendeeButtonCell"));
            _app.Tap(x => x.Class("AddAttendeeButtonCell"));
            WaitForElement(new WaitForElementRequest { Marked = "Business Relation" });
            _app.Tap(x => x.Marked("Spouse"));
            WaitForElement(new WaitForElementRequest { Marked = "First name" });
            _app.EnterText(x => x.Class("UITextField").Index(0), request.FirstName);
            _app.EnterText(x => x.Class("UITextField").Index(1), request.LastName);
            _app.Tap(x => x.Marked("Create"));
            WaitForElement(new WaitForElementRequest
            {
                Class = "UIButtonLabel",
                Marked = "Save"
            });
        }

        public override void AddHealthCareProvider(AddHealthCareProviderRequest request)
        {
            // Don't know how many attendee yet, so not sure the text of the tab ("Attendee (0)", "Attendee (1)",...?)
            WaitForElement(new WaitForElementRequest { Class = "UISegment" });
            var attendeetab = _app.Query(x => x.Class("UISegment")).First(x => x.Label.StartsWith("Attendees"));
            _app.Tap(x => x.Marked(attendeetab.Label));
            Wait(TimeSpan.FromSeconds(1));
            _app.ScrollDownTo(x => x.Class("AddAttendeeButtonCell"));
            _app.Tap(x => x.Class("AddAttendeeButtonCell"));
            WaitForElement(new WaitForElementRequest { Marked = "Business Relation" });
            _app.Tap(x => x.Marked("Health Care Provider"));
            WaitForElement(new WaitForElementRequest { Marked = "First name" });
            EnterText(request.FirstName, 0);
            EnterText(request.LastName, 1);
            EnterText(request.Address, 2);
            EnterText(request.Zip, 3);
            EnterText(request.City, 4);
            EnterText(request.State, 5);
            EnterText(request.Speciality, 6);
            EnterText(request.NPINumber, 7);
            _app.Tap(x => x.Marked("Search"));
            WaitForElement(new WaitForElementRequest { Class = "IPhoneHCPAttendeeCell" });

            _app.Tap(x => x.Class("UITableViewCellContentView").Descendant("UILabel"));


            WaitForElement(new WaitForElementRequest
            {
                Class = "UIButtonLabel",
                Marked = "Save"
            });
        }

        public override void AddHealthCareOrganization(AddHealthCareOrganizationRequest request)
        {
            // Don't know how many attendee yet, so not sure the text of the tab ("Attendee (0)", "Attendee (1)",...?)
            WaitForElement(new WaitForElementRequest { Class = "UISegment" });
            var attendeetab = _app.Query(x => x.Class("UISegment")).First(x => x.Label.StartsWith("Attendees"));
            _app.Tap(x => x.Marked(attendeetab.Label));
            Wait(TimeSpan.FromSeconds(1));
            _app.ScrollDownTo(x => x.Class("AddAttendeeButtonCell"));
            _app.Tap(x => x.Class("AddAttendeeButtonCell"));
            WaitForElement(new WaitForElementRequest { Marked = "Business Relation" });
            _app.Tap(x => x.Marked("Health Care Organization"));
            WaitForElement(new WaitForElementRequest { Marked = "Company" });
            EnterText(request.Company, 0);
            EnterText(request.Zip, 1);
            EnterText(request.City, 2);
            EnterText(request.State, 3);
            EnterText(request.Speciality, 4);
            EnterText(request.NPINumber, 5);
            _app.Tap(x => x.Marked("Search"));
            WaitForElement(new WaitForElementRequest {Id = "EmptyIcon" });
            WaitForNoElement(new WaitForElementRequest {Id = "EmptyIcon" });

            _app.Tap(x => x.Class("UITableViewCellContentView").Descendant("UILabel"));

            WaitForElement(new WaitForElementRequest
            {
                Class = "UIButtonLabel",
                Marked = "Save"
            });
        }

        public override void AddUnrecognizedHealthCareProvider(AddUnrecognizedHealthCareProviderRequest request)
        {
            // Don't know how many attendee yet, so not sure the text of the tab ("Attendee (0)", "Attendee (1)",...?)
            WaitForElement(new WaitForElementRequest { Class = "UISegment" });
            var attendeetab = _app.Query(x => x.Class("UISegment")).First(x => x.Label.StartsWith("Attendees"));
            _app.Tap(x => x.Marked(attendeetab.Label));
            Wait(TimeSpan.FromSeconds(1));
            _app.ScrollDownTo(x => x.Class("AddAttendeeButtonCell"));
            _app.Tap(x => x.Class("AddAttendeeButtonCell"));
            WaitForElement(new WaitForElementRequest { Marked = "Business Relation" });
            _app.Tap(x => x.Marked("Unrecognized Health Care Provider"));
            WaitForElement(new WaitForElementRequest { Marked = "*First name" });
            EnterText(request.FirstName, 0);
            EnterText(request.LastName, 1);
            EnterText(request.Company, 2);
            EnterText(request.Address, 3);
            EnterText(request.Zip, 4);
            EnterText(request.City, 5);
            EnterText(request.State, 6);
            EnterText(request.Speciality, 7);
            EnterText(request.NPINumber, 8);
            _app.Tap(x => x.Marked("Create"));
            WaitForElement(new WaitForElementRequest
            {
                Class = "UIButtonLabel",
                Marked = "Save"
            });
        }

        private void EnterText(string text, int textfieldindex)
        {
            if (string.IsNullOrEmpty(text)) return;

            _app.EnterText(x => x.Class("UITextField").Index(textfieldindex),text);
            _app.DismissKeyboard();
        }

        public override int GetListCount(GetListCountRequest request)
        {
            if (!string.IsNullOrEmpty(request.Id))
                return _app.Query(x => x.Class("UITableView").Id(request.Id).Invoke("numberOfRowsInSection", 0).Value<int>()).FirstOrDefault();

            return _app.Query(x => x.Class("UITableView").Invoke("numberOfRowsInSection", 0).Value<int>()).FirstOrDefault();
        }

        public override int GetAttendeeCount(GetAttendeeCountRequest request)
        {
            var attendeetab = _app.Query(x => x.Class("UISegment")).First(x => x.Label.StartsWith("Attendees"));
            _app.Tap(x => x.Marked(attendeetab.Label));
            WaitForElement(new WaitForElementRequest { Class = "AttendeeItemCell" });

            return GetListCount(new GetListCountRequest()) - 1; // remove one because of the "add attendee" button
        }

        public override void PullToRefresh()
        {
            WaitForElement(new WaitForElementRequest {Class = "ExpenseCell" });
            var rect = _app.Query(x => x.Class("ExpenseCell")).First().Rect;
            _app.DragCoordinates(rect.CenterX, rect.Y + 20, rect.CenterX, rect.Height * 5);
            _app.WaitForElement(x => x.Class("UIRefreshControl"));
            _app.WaitForNoElement(x => x.Class("UIRefreshControl"));
        }

        public override bool CheckErrorDialogShowing()
        {
            return _app.Query(x => x.Class("UIAlertView")).Any();
        }

        public override bool CheckIconVisibilityAndCounter(CheckIconVisibilityAndCounterRequest request)
        {
            var icon = _app.Query(x => x.Class("UIImageView").Id(request.Id)).FirstOrDefault();
            if ((icon == null) || (icon.Rect.Width == 0))
                return false;

            var counter = _app.Query(x => x.Class("UIImageView").Id(request.Id).Sibling("UILabel").Id(request.Counter.ToString()).Text(request.Counter.ToString())).FirstOrDefault();

            return counter != null;
        }

        public override bool CheckAttendeeIcon(CheckAttendeeIconRequest request)
        {
            return CheckIconVisibilityAndCounter(new CheckIconVisibilityAndCounterRequest
            {
                Id = "AttendeeExpenseCell.png",
                Counter = request.Counter
            });
        }

        public override bool CheckReceiptIcon(CheckReceiptIconRequest request)
        {
            return CheckIconVisibilityAndCounter(new CheckIconVisibilityAndCounterRequest
            {
                Id = "ExpensesIconTab",
                Counter = request.Counter
            });
        }

        private void SelectImage()
        {
            // Select first image in Camera Roll
            _app.Tap(x => x.Marked("Choose From Library"));
            _app.Tap(x => x.Class("UITextFieldLabel").Marked("Camera Roll"));
            _app.Tap(x => x.Class("PUPhotoView").Index(1));
        }

	}
}
