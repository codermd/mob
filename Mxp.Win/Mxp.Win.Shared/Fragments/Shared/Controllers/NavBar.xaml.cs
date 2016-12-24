using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Mxp.Win
{
    public sealed partial class NavBar : UserControl
    {
  
            public NavBar()
            {
                this.InitializeComponent();
                SetButtonsVisaul();
                MainController.Instance.expensesButtonRequest += LoadExpenses;
                MainController.Instance.reportsButtonRequest += LoadReports;
                MainController.Instance.approvalsButtonRequest += LoadApprovals;
                MainController.Instance.settingsButtonRequest += LoadSettings;

            }

            private void LoadSettings(object sender, EventArgs e)
            {
                this.ReportsColumn.Opacity = 0.5;
                this.ExpensesColumn.Opacity = 0.5;
                this.SettingsColumn.Opacity = 1;
                this.ApprovalsColumn.Opacity = 0.5;

            }

            private void LoadApprovals(object sender, EventArgs e)
            {
                this.ReportsColumn.Opacity = 0.5;
                this.ExpensesColumn.Opacity = 0.5;
                this.SettingsColumn.Opacity = 0.5;
                this.ApprovalsColumn.Opacity = 1;
            }

            private void LoadReports(object sender, EventArgs e)
            {
                this.ReportsColumn.Opacity = 1;
                this.ExpensesColumn.Opacity = 0.5;
                this.SettingsColumn.Opacity = 0.5;
                this.ApprovalsColumn.Opacity = 0.5;
            }

            private void LoadExpenses(object sender, EventArgs e)
            {
                this.ReportsColumn.Opacity = 0.5;
                this.ExpensesColumn.Opacity = 1;
                this.SettingsColumn.Opacity = 0.5;
                this.ApprovalsColumn.Opacity = 0.5;
            }

            private void SetButtonsVisaul()
            {
                this.ButtonExpenses.SetVisual("Expenses", "/Assets/Icons/Expenses_128.png");
                this.ButtonReports.SetVisual("Reports", "/Assets/Icons/Reports_128.png");
                this.ButtonApprovals.SetVisual("Approvals", "/Assets/Icons/Approvals_128.png");
                this.ButtonSettings.SetVisual("Settings", "/Assets/Icons/Settings_128.png");
            }



            private void ButtonExpenses_Tapped(object sender, TappedRoutedEventArgs e)
            {
                MainController.Instance.expensesButton();
            }

            private void ButtonReports_Tapped(object sender, TappedRoutedEventArgs e)
            {
                MainController.Instance.reportsButton();
            }

            private void ButtonApprovals_Tapped(object sender, TappedRoutedEventArgs e)
            {
                MainController.Instance.approvalsButton();
            }

            private void ButtonSettings_Tapped(object sender, TappedRoutedEventArgs e)
            {
                MainController.Instance.settingsButton();
            }
        }
    }

