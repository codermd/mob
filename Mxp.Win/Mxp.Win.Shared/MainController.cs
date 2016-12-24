using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mxp.Win
{
    class MainController
    {
        public bool InExpenses = false;
        public bool InReports = false;
        public bool InApprovals = false;
        public bool InSettings = false;
        private static MainController instance;
        public event EventHandler expensesButtonRequest;
        public event EventHandler reportsButtonRequest;
        public event EventHandler approvalsButtonRequest;
        public event EventHandler settingsButtonRequest;
        public event EventHandler refreshButtonRequest;
        public event EventHandler StartMainProgressRingRequest;
        public event EventHandler FinishMainProgressRingRequest;
        public event EventHandler ExpensesCreationCategoryChoosedRequest;
        public event EventHandler ExpensesCreationCountryChoosedRequest;
        public event EventHandler ExpensesCreationAmountChoosedRequest;
        public event EventHandler ExpensesCreationSuccessedRequest;
        public event EventHandler ExpenseProductsAddedRequest;
        public event EventHandler ExpenseGroupedProductsAddedRequest;
        public event EventHandler ExpenseSplitRemovedRequest;
        public event EventHandler ExpenseReceiptRemovedRequest;
        public event EventHandler AddReceiptRequest;
        public event EventHandler MainPageExpensesTabRequest;
        public event EventHandler ReceiptsLoadingRequest;
        public event EventHandler ReceiptsLoadedRequest;
        public event EventHandler MainPageClearRequest;
        public event EventHandler AttendeeDeletedRequest;
        public event EventHandler CollapseWebViewRequest;
        public event EventHandler ChangeCurrencyRequest;
        //public event EventHandler AllReceiptsDownloadedRequest;
        public void ChangeCurrency()
        {
            if (ChangeCurrencyRequest != null)
                ChangeCurrencyRequest(this, EventArgs.Empty);
        }
        public void CollapseWebView()
        {
            if (CollapseWebViewRequest != null)
                CollapseWebViewRequest(this, EventArgs.Empty);
        }
        public void ReceiptsLoading()
        {
            if (ReceiptsLoadingRequest != null)
                ReceiptsLoadingRequest(this, EventArgs.Empty);
        }
        public void ReceiptsLoaded()
        {
            if (ReceiptsLoadedRequest != null)
                ReceiptsLoadedRequest(this, EventArgs.Empty);
        }

        //internal void AllReceiptsDownloaded()
        //{
        //    if (AllReceiptsDownloadedRequest != null)
        //        AllReceiptsDownloadedRequest(this, EventArgs.Empty);
        //}

        public void MainPageExpensesTab()
        {
            if (MainPageExpensesTabRequest != null)
                MainPageExpensesTabRequest(this, EventArgs.Empty);
        }

        public void MainPageClear()
        {
            if (MainPageClearRequest != null)
                MainPageClearRequest(this, EventArgs.Empty);
        }

        public void expensesButton()
        {
            if (expensesButtonRequest != null)
                expensesButtonRequest(this, EventArgs.Empty);
        }
        public void reportsButton()
        {
            if (reportsButtonRequest != null)
                reportsButtonRequest(this, EventArgs.Empty);
        }
        public void approvalsButton()
        {
            if (approvalsButtonRequest != null)
                approvalsButtonRequest(this, EventArgs.Empty);
        }
        public void settingsButton()
        {
            if (settingsButtonRequest != null)
                settingsButtonRequest(this, EventArgs.Empty);
        }
        public void refreshButton()
        {
            if (refreshButtonRequest != null)
            {
                refreshButtonRequest(this, EventArgs.Empty);
            }
        }
        public static MainController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainController();
                }
                return instance;
            }
        }


        internal void StartMainProgressRing()
        {
            if (StartMainProgressRingRequest != null)
            {
                StartMainProgressRingRequest(this, EventArgs.Empty);
            }
        }
        internal void FinishMainProgressRing()
        {
            if (FinishMainProgressRingRequest != null)
            {
                FinishMainProgressRingRequest(this, EventArgs.Empty);
            }
        }
        internal void ExpensesCreationCategoryChoosed()
        {
            if (ExpensesCreationCategoryChoosedRequest != null)
            {
                ExpensesCreationCategoryChoosedRequest(this, EventArgs.Empty);
            }
        }
        internal void ExpensesCreationCountryChoosed()
        {
            if (ExpensesCreationCountryChoosedRequest != null)
            {
                ExpensesCreationCountryChoosedRequest(this, EventArgs.Empty);
            }
        }
        internal void ExpensesCreationAmountChoosed()
        {
            if (ExpensesCreationAmountChoosedRequest != null)
            {
                ExpensesCreationAmountChoosedRequest(this, EventArgs.Empty);
            }
        }
        internal void ExpensesCreationSuccessed()
        {
            if (ExpensesCreationSuccessedRequest != null)
            {
                ExpensesCreationSuccessedRequest(this, EventArgs.Empty);
            }
        }
        internal void ExpenseProductsAdded()
        {
            if (ExpenseProductsAddedRequest != null)
            {
                ExpenseProductsAddedRequest(this, EventArgs.Empty);
            }
        }
        internal void ExpenseGroupedProductsAdded()
        {
            if (ExpenseGroupedProductsAddedRequest != null)
            {
                ExpenseGroupedProductsAddedRequest(this, EventArgs.Empty);
            }
        }
        internal void ExpenseSplitRemoved()
        {
            if (ExpenseSplitRemovedRequest != null)
            {

                ExpenseSplitRemovedRequest(this, EventArgs.Empty);
            }
        }
        internal void ExpenseReceiptRemoved()
        {
            if (ExpenseReceiptRemovedRequest != null)
            {

                ExpenseReceiptRemovedRequest(this, EventArgs.Empty);
            }
        }
        internal void AttendeeDeleted()
        {
            if (AttendeeDeletedRequest != null)
            {

                AttendeeDeletedRequest(this, EventArgs.Empty);
            }
        }



        
        internal void AddReceipt()
        {
            if (AddReceiptRequest != null)
            {
                AddReceiptRequest(this, EventArgs.Empty);
            }
        }



      
        public static int? index;
        public static Receipts receipts;
    }
}

