using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ReportsView : UserControl
    {
        private ListView DraftList;
        private ListView OpenList;
        private ListView ClosedList;
        public Reports DraftReports { get; set; }
        public Reports OpenReports { get; set; }
        public Reports ClosedReports { get; set; }
        public ReportsView()
        {
            this.InitializeComponent();
            MainController.Instance.StartMainProgressRing();
            MainController.Instance.refreshButtonRequest += MainControllerRefreshButton;
            OpenHeader.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Open));

        }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
        private async void MainControllerRefreshButton(object sender, EventArgs e)
        {
            if (MainController.Instance.InReports)
            {
                
                MainController.Instance.StartMainProgressRing();
                this.ProgressGrid.Visibility = Visibility.Visible;
                this.ProgressRefresh.IsActive = true;
                try
                {
                    await LoggedUser.Instance.DraftReports.FetchAsync();
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
                DraftReports = LoggedUser.Instance.DraftReports;
                DraftList.ItemsSource = DraftReports;
                try
                {
                    await LoggedUser.Instance.OpenReports.FetchAsync();
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
                OpenReports = LoggedUser.Instance.OpenReports;
                OpenList.ItemsSource = OpenReports;


                try
                {
                    await LoggedUser.Instance.ClosedReports.FetchAsync();
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
                ClosedReports = LoggedUser.Instance.ClosedReports;
                ClosedList.ItemsSource = ClosedReports;


                MainController.Instance.FinishMainProgressRing();
                this.ProgressGrid.Visibility = Visibility.Collapsed;
                this.ProgressRefresh.IsActive = false;
            }
        }
 
       

        private async void DraftReportsList_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoggedUser.Instance.DraftReports.Loaded)
                return;

            this.ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRefresh.IsActive = true;
            try
            {
                await LoggedUser.Instance.DraftReports.FetchAsync();
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
            DraftReports = LoggedUser.Instance.DraftReports;
            DraftList = (ListView)sender;
            DraftList.ItemsSource = DraftReports;
            this.ProgressGrid.Visibility = Visibility.Collapsed;
            this.ProgressRefresh.IsActive = false;

        }
        private async void OpenReportsList_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoggedUser.Instance.OpenReports.Loaded)
                return;

            this.ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRefresh.IsActive = true;
            try
            {
                await LoggedUser.Instance.OpenReports.FetchAsync();
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
            OpenReports = LoggedUser.Instance.OpenReports;
            OpenList = (ListView)sender;
            OpenList.ItemsSource = OpenReports;
            MainController.Instance.FinishMainProgressRing();
            this.ProgressGrid.Visibility = Visibility.Collapsed;
            this.ProgressRefresh.IsActive = false;
        }
        private async void ClosedReportsList_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoggedUser.Instance.ClosedReports.Loaded)
                return;

            this.ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRefresh.IsActive = true;
            try
            {
                await LoggedUser.Instance.ClosedReports.FetchAsync();
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
            ClosedReports = LoggedUser.Instance.ClosedReports;
            ClosedList = (ListView)sender;
            ClosedList.ItemsSource = ClosedReports;
            MainController.Instance.FinishMainProgressRing();
            this.ProgressGrid.Visibility = Visibility.Collapsed;
            this.ProgressRefresh.IsActive = false;
        }

        private void DraftHeaderLoaded(object sender, RoutedEventArgs e)
        {
            (sender as TextBlock).Text= LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Draft);
        }

        private void OpenHeader_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TextBlock).Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Open);
        }

        private void ClosedHeader_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TextBlock).Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Closed);
        }

        
    }
}
