using Mxp.Core.Business;
using Mxp.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Mxp.Win.Helpers.SchemeActions.Commands
{
    class OpenObjectCommand : OpenObjectAbstractCommand
    {
        public OpenObjectCommand(Uri uri) : base(uri)
        {
        }
        public OpenObjectCommand(Page page, int objectType, int referenceType, string reference)
        {
            
            this.openObject = new OpenObject(objectType.ToString(), referenceType.ToString(), reference);
        }
        public override void RedirectToLoginView(ValidationError error = null)
        {
            (Window.Current.Content as Frame).Navigate(typeof(LoginPage));
        }

        protected override void RedirectToDetailsView()
        {
            switch (this.MetaOpenObject.Location)
            {
                case MetaOpenObject.LocationEnum.PendingExpenses:
                    ExpenseItem expenseItem = LoggedUser.Instance.BusinessExpenses.SelectSingle(expense => expense.ExpenseItems.SingleOrDefault(item => item.Id == this.MetaOpenObject.Id));
                    (Window.Current.Content as Frame).Navigate(typeof(ExpenseDetailView),expenseItem);
                    break;
                case MetaOpenObject.LocationEnum.DraftReports:
                    if (this.MetaOpenObject.HasFatherId)
                    {
                    }
                    else
                    {
                    }
                    break;
                case MetaOpenObject.LocationEnum.OpenReports:
                    if (this.MetaOpenObject.HasFatherId)
                    {
                    }
                    else
                    {
                    }
                    break;
                case MetaOpenObject.LocationEnum.ApprovalReports:
                    break;
                case MetaOpenObject.LocationEnum.ApprovalTravelRequests:
                    break;
            }
        }

        protected override void RedirectToListView(ValidationError error)
        {
            switch (this.MetaOpenObject.Location)
            {
                case MetaOpenObject.LocationEnum.PendingExpenses:
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                    break;
                case MetaOpenObject.LocationEnum.DraftReports:
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                    break;
                case MetaOpenObject.LocationEnum.OpenReports:
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                    break;
                case MetaOpenObject.LocationEnum.ApprovalReports:
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                    break;
                case MetaOpenObject.LocationEnum.ApprovalTravelRequests:
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                    break;
                case MetaOpenObject.LocationEnum.Unknown:
                    switch (this.openObject.ObjectType)
                    {
                        case OpenObject.ObjectTypeEnum.Expense:
                            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                            break;
                        case OpenObject.ObjectTypeEnum.Report:
                            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                            break;
                        case OpenObject.ObjectTypeEnum.ApprovalReport:
                            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                            break;
                        case OpenObject.ObjectTypeEnum.ApprovalTravelRequest:
                            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                            break;
                    }
                    break;
            }
        }
    }
}
