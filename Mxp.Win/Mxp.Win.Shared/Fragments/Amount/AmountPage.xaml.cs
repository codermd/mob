using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Mxp.Win
{
    public sealed partial class AmountPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("Field"))
                Field = e.PageState["Field"] as Field;
            if (e.PageState != null && e.PageState.ContainsKey("ExpenseItem"))
                ExpenseItem = e.PageState["ExpenseItem"] as ExpenseItem;
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (Field != null)
                e.PageState["Field"] = Field;
            if (ExpenseItem != null)
                e.PageState["ExpenseItem"] = ExpenseItem;
        }
        private void InitializeNavigationHelper()
        {
            navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }
        public AmountPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            InitializeNavigationHelper();
            LoadLabels();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        private void LoadLabels()
        {
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Amount));
        }
        ExpenseItem ExpenseItem;
        Field Field;


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            Field = e.Parameter as Field;
            ExpenseItem = Field.GetModel<ExpenseItem>();
            FieldsListView.ItemsSource = ExpenseItem.AmountFields;
        }
    }
}
