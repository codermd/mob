using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class ApprovalsView : UserControl
    {
        public ApprovalsView()
        {
            this.InitializeComponent();
            MainController.Instance.StartMainProgressRing();
            MainController.Instance.refreshButtonRequest += MainControllerRefreshButton;

        }
        private async void MainControllerRefreshButton(object sender, EventArgs e)
        {
            if (MainController.Instance.InApprovals)
            {
                MainController.Instance.StartMainProgressRing();
                this.ProgressGrid.Visibility = Visibility.Visible;
                this.ProgressRefresh.IsActive = true;
                try
                {
                    await LoggedUser.Instance.ReportApprovals.FetchAsync();
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();
                    this.ProgressGrid.Visibility = Visibility.Collapsed;
                    this.ProgressRefresh.IsActive = false;
                    return;
                }

                MainController.Instance.StartMainProgressRing();
                this.ProgressGrid.Visibility = Visibility.Visible;
                this.ProgressRefresh.IsActive = true;
                try
                {
                    await LoggedUser.Instance.TravelApprovals.FetchAsync();
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();
                    this.ProgressGrid.Visibility = Visibility.Collapsed;
                    this.ProgressRefresh.IsActive = false;
                    return;
                }
             
              
                ReportApprovals = LoggedUser.Instance.ReportApprovals;
                TravelApprovals = LoggedUser.Instance.TravelApprovals;
                DraftList.ItemsSource = ReportApprovals;
                OpenList.ItemsSource = TravelApprovals;
                MainController.Instance.FinishMainProgressRing();
                this.ProgressGrid.Visibility = Visibility.Collapsed;
                this.ProgressRefresh.IsActive = false;
            }
        }
        public ReportApprovals ReportApprovals { get; set; }
        public TravelApprovals TravelApprovals { get; set; }
        private ListView DraftList;
        private ListView OpenList;
        private async void ReportApprovalList_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoggedUser.Instance.ReportApprovals.Loaded)
                return;

            this.ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRefresh.IsActive = true;
            try
            {
                await LoggedUser.Instance.ReportApprovals.FetchAsync();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                return;
            }
            ReportApprovals = LoggedUser.Instance.ReportApprovals;
            try
            {
                await ReportApprovals.FetchAsync();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                MainController.Instance.FinishMainProgressRing();
                this.ProgressGrid.Visibility = Visibility.Collapsed;
                this.ProgressRefresh.IsActive = false;
                return;
            }
            DraftList = (ListView)sender;
            DraftList.ItemsSource = ReportApprovals;
            this.ProgressGrid.Visibility = Visibility.Collapsed;
            this.ProgressRefresh.IsActive = false;
        }
        private async void TravelApprovalList_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoggedUser.Instance.TravelApprovals.Loaded)
                return;

            this.ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRefresh.IsActive = true;
            try
            {
                await LoggedUser.Instance.TravelApprovals.FetchAsync();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                MainController.Instance.FinishMainProgressRing();
                this.ProgressGrid.Visibility = Visibility.Collapsed;
                this.ProgressRefresh.IsActive = false;
                return;
            }
            TravelApprovals = LoggedUser.Instance.TravelApprovals;
            OpenList = (ListView)sender;
            OpenList.ItemsSource = TravelApprovals;
            MainController.Instance.FinishMainProgressRing();
            this.ProgressGrid.Visibility = Visibility.Collapsed;
            this.ProgressRefresh.IsActive = false;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void ReportsHeader_Loaded(object sender, RoutedEventArgs e)
        {
            ReportsHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Reports));
        }

        private void TravelHeader_Loaded(object sender, RoutedEventArgs e)
        {
            TravelHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Travels));
        }
    }
}
