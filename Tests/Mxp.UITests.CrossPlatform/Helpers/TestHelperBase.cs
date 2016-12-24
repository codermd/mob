using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using TechTalk.SpecFlow;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Mxp.UITests.CrossPlatform.Helpers
{
    public abstract class TestHelperBase
    {
        protected IApp _app;

        protected Platform _platform;

        protected TestHelperBase(IApp app, Platform platform)
        {
            _app = app;
            _platform = platform;
        }

        public static TestHelperBase Create()
        {
            var platform = FeatureContext.Current.Get<Platform>("Platform");
            var app = FeatureContext.Current.Get<IApp>("App");

            if (platform == Platform.Android)
                return new AndroidTestHelper(app);

            return new iOSTestHelper(app);
        }

        protected string GetTimeStamp()
        {
            return DateTime.Now.ToString("yy-MM-dd@HH:mm:ss");
        }

        #region Base steps

        public void TapButton(TapButtonRequest request)
        {
            _app.Tap(x => x.Button(request.Marked).Index(request.Index));
        }

        public void TakeScreenshot(string text)
        {
            _app.Screenshot(text);
        }

        public void EnterText(EnterTextRequest request)
        {
            _app.EnterText(x => x.TextField(request.Marked).Index(request.Index), request.Text);
        }

        public void WaitForElement(WaitForElementRequest request)
        {
            var query = GetElementQuery(request);

            _app.WaitForElement(query, request.ErrorMessage, request.TimeOut);
        }

        public void WaitForNoElement(WaitForElementRequest request)
        {
            var query = GetElementQuery(request);

            _app.WaitForNoElement(query, request.ErrorMessage, request.TimeOut);
        }

        protected Func<AppQuery, AppQuery> GetElementQuery(WaitForElementRequest request)
        {
            // that gives us an expression like this : query => query.Class(request.Class)
            var valueparam = Expression.Parameter(typeof(AppQuery), "query");
            Expression theexpression = null;
            switch (request.ControlType)
            {
                case ControlType.Switch:
                    theexpression = Expression.Call(valueparam, typeof(AppQuery).GetMethod("Switch", new[] { typeof(string) }), Expression.Constant(request.Marked, typeof(string)));
                    break;
                case ControlType.TextField:
                    theexpression = Expression.Call(valueparam, typeof(AppQuery).GetMethod("TextField", new[] { typeof(string) }), Expression.Constant(request.Marked, typeof(string)));
                    break;
               case ControlType.Label:
                    var theclass = _platform == Platform.Android ? "TextView" : "UITextFieldLabel";
                    theexpression = Expression.Call(valueparam, typeof(AppQuery).GetMethod("Class", new[] { typeof(string) }), Expression.Constant(theclass, typeof(string)));
                    break;
                case ControlType.Button:
                    theexpression = Expression.Call(valueparam, typeof(AppQuery).GetMethod("Button", new[] { typeof(string) }), Expression.Constant(request.Marked, typeof(string)));
                    break;
                default:
                    break;
            }

            if (request.ControlType == ControlType.Any && !string.IsNullOrEmpty(request.Class))
            {
                theexpression = Expression.Call(theexpression ?? valueparam, typeof(AppQuery).GetMethod("Class", new[] { typeof(string) }), Expression.Constant(request.Class, typeof(string)));
            }

            if (!string.IsNullOrEmpty(request.Id))
            {
                theexpression = Expression.Call(theexpression ?? valueparam, typeof(AppQuery).GetMethod("Id", new[] { typeof(string) }), Expression.Constant(request.Id, typeof(string)));
            }

            if (!string.IsNullOrEmpty(request.Marked))
            {
                theexpression = Expression.Call(theexpression ?? valueparam, typeof(AppQuery).GetMethod("Marked", new[] { typeof(string) }), Expression.Constant(request.Marked, typeof(string)));
            }

            if (!string.IsNullOrEmpty(request.Text))
            {
                theexpression = Expression.Call(theexpression ?? valueparam, typeof(AppQuery).GetMethod("Text", new[] { typeof(string) }), Expression.Constant(request.Text, typeof(string)));
            }

            if (request.Index > 0)
            {
                theexpression = Expression.Call(theexpression ?? valueparam, typeof(AppQuery).GetMethod("Index", new[] { typeof(int) }), Expression.Constant(request.Index, typeof(int)));
            }

            return Expression.Lambda<Func<AppQuery, AppQuery>>(theexpression ?? valueparam, valueparam).Compile();
        }

        public void Wait(TimeSpan waittime, string timeoutmessage = "Timed out...")
        {
            var task = Task.Delay(waittime);
            _app.WaitFor(() => task.IsCompleted, timeoutmessage, TimeSpan.FromSeconds(waittime.TotalSeconds + 20));
        }

        public bool IsLabelElementExists(string text)
        {
            var theclass = _platform == Platform.Android ? "TextView" : "UITextFieldLabel";
            return _app.Query(c => c.Class(theclass).Text(text)).Any();
        }

        public bool IsElementExists(IsElementExistsRequest request)
        {
            return QueryApp(new QueryAppRequest {Class = request.Class, Text = request .Text}).Any();
        }

        public AppResult[] QueryApp(QueryAppRequest request)
        {
            var query = GetElementQuery(new WaitForElementRequest
            {
                Class = request.Class,
                Text = request.Text,
                Id = request.Id
            });
            return _app.Query(query);
        }

        public string CreateABasicExpense(bool addreceipt = false)
        {
            var request = new CreateAnExpenseRequest
            {
                AddReceipt = addreceipt,
                Category = "Car Maintenance",
                Country = "BE - Belgium",
                Quantity = 1,
                Amount = 10,
                Currency = "Euro (EUR)"
            };
            return CreateAnExpense(request);
        }

        #endregion

        public bool Loging(LogingRequest request)
        {
            var loginrequest = new EnterLoginDataRequest
            {
                UserName = request.UserName,
                Password = request.Password
            };
            EnterLoginData(loginrequest);
            TapLoginButton();

            return IsLogged();
        }

        public void ShowRepl()
        {
            _app.Repl();
        }

        #region Plaform dependent steps

        public abstract void TapFirstCell();

        public abstract void TapLoginButton();

        public abstract void EnterLoginData(EnterLoginDataRequest request);

        public abstract void EnterValue<T>(EnterValueRequest<T> request);

        public abstract void SelectValue(SelectValueRequest request);

		public abstract void SelectAutocompleteValue(SelectAutocompleteValueRequest request);

        public abstract void EnterAutocompleteValue(EnterAutocompleteValueRequest request);

        public abstract bool IsLogged();

        public abstract string CreateAnExpense(CreateAnExpenseRequest request);

        public abstract void AddReceiptToExpense();

        public abstract void SaveExpense();

        public abstract bool ExpenseIsSaved(ExpenseIsSavedRequest request);

        public abstract string CreateAReport();

        public abstract void AddReceiptToReport();

        public abstract void SaveReport();

        public abstract bool ReportIsSaved(ReportIsSavedRequest request);

        public abstract void AddExpenseToReport(AddExpenseToReportRequest request);

        public abstract bool ReportExpenseReceiptSaved(ReportExpenseReceiptSavedRequest request);

        public abstract void SelectExpense(SelectExpenseRequest request);

        public abstract void ToggleSwitch(ToggleSwitchRequest request);

        public abstract bool GetSwitchState(GetSwitchStateRequest request);
        public abstract bool GetIconState(GetIconStateRequest request);

        public abstract void DeleteExpense();
        public abstract void CancelExpense();
        public abstract void DeleteAllowance();
        public abstract void DeleteMilleagee();

        public abstract bool ExpenseIsDeleted(ExpenseIsDeletedRequest request);
        #endregion

        protected void SaveData(string key, object data)
        {
            if (FeatureContext.Current.ContainsKey(key))
                FeatureContext.Current[key] = data;
            else
                FeatureContext.Current.Add(key, data);
        }

        protected T GetData<T>(string key)
        {
            return (T)FeatureContext.Current[key];
        }

        public abstract void SelectReport(SelectReportRequest request);
        public abstract void DeleteReport();
        public abstract bool ReportIsDeleted(ReportIsDeletedRequest request);
        public abstract string CreateAMilleage(CreateAMilleageRequest request);
        public abstract void SaveMileage();
        public abstract bool MileageIsSaved(MileageIsSavedRequest request);
        public abstract int GetDistance(GetDistanceRequest request);
        public abstract void SetDistance(SetDistanceRequest request);
        public abstract string CreateAllowance(CreateAllowanceRequest request);
        public abstract void SaveAllowance();
        public abstract bool AllowanceIsSaved(AllowanceIsSavedRequest request);
        public abstract void SelectFirstMileage();
        public abstract string GetValue(GetValueRequest request);
        public abstract void AddBusinessRelation(AddBusinessRelationRequest request);
        public abstract void AddSpouse(AddSpouseRequest request);
        public abstract void AddHealthCareProvider(AddHealthCareProviderRequest request);
        public abstract void AddHealthCareOrganization(AddHealthCareOrganizationRequest request);
        public abstract void AddUnrecognizedHealthCareProvider(AddUnrecognizedHealthCareProviderRequest request);
        public abstract int GetListCount(GetListCountRequest request);
        public abstract int GetAttendeeCount(GetAttendeeCountRequest request);
        public abstract void PullToRefresh();
        public abstract bool CheckErrorDialogShowing();
        public abstract bool CheckIconVisibilityAndCounter(CheckIconVisibilityAndCounterRequest request);
        public abstract bool CheckAttendeeIcon(CheckAttendeeIconRequest request);
        public abstract bool CheckReceiptIcon(CheckReceiptIconRequest request);
    }
}
