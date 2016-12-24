using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
    public sealed partial class AcceptApprovalPage : Page
    {
        public Report Report { get; set; }
        public Travel Travel { get; set; }
        public AcceptApprovalPage()
        {
            this.InitializeComponent();
            Title.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Approve);
            AcceptedLabel.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Accepted);
            RejectedLabel.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Rejected);
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter is Report)
            {
                Report = e.Parameter as Report;
                AcceptedLabel.Visibility = Visibility.Visible;
                AcceptedValue.Visibility = Visibility.Visible;
                RejectedLabel.Visibility = Visibility.Visible;
                RejectedValue.Visibility = Visibility.Visible;
                AcceptedValue.Text = this.Report.Approval.VNumberOfAccepted.ToString();
                RejectedValue.Text = this.Report.Approval.VNumberOfRejected.ToString();
            }
            else if (e.Parameter is Travel)
            {
                Travel = e.Parameter as Travel;
                AcceptedLabel.Visibility = Visibility.Collapsed;
                AcceptedValue.Visibility = Visibility.Collapsed;
                RejectedLabel.Visibility = Visibility.Collapsed;
                RejectedValue.Visibility = Visibility.Collapsed;
                RejectButton.Visibility = Visibility.Visible;
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxComment.Text != null)
            {
                if (Report != null)
                    Report.Approval.Comment = TextBoxComment.Text;
                if (Travel != null)
                    Travel.Approval.Comment = TextBoxComment.Text;
            }

            try
            {
                AcceptAppProgressRing.IsActive = true;
                BottomAppBar.IsEnabled = false;
                TextBoxComment.IsEnabled = false;
                if (Report != null)
                    await Report.Approval.SaveAsync();
                else if (Travel != null)
                {
                    Travel.Approval.AcceptedStatus = true;
                    await Travel.Approval.SubmitAsync();

                }
                Frame.Navigate(typeof(MainPage));
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { TextBoxComment.IsEnabled = true; })); // So the keyboard will only show after user press ok (MOB-815)
                messageDialog.ShowAsync();
                BottomAppBar.IsEnabled = true;
                AcceptAppProgressRing.IsActive = false;
                
            }
        }
        private async void Reject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AcceptAppProgressRing.IsActive = true;
                BottomAppBar.IsEnabled = false;
                    Travel.Approval.AcceptedStatus = false;
                    await Travel.Approval.SubmitAsync();
                Frame.Navigate(typeof(MainPage));
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                BottomAppBar.IsEnabled = true;
                AcceptAppProgressRing.IsActive = false;
            }
        }
    }
}
