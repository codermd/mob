using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public sealed partial class RecentlyUserTableViewController : Page
    {
        public RecentlyUserTableViewController()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.ChooseAttendee));
            this.TBListFilter.PlaceholderText = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Search));
            RecentAttendees = new Attendees();
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
        Attendees Attendees { get; set; }
        Attendees RecentAttendees { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Attendees = (Attendees)e.Parameter;
            try
            {
                ProgressRing.IsActive = true;
                FetchAttendees();
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                ProgressRing.IsActive = false;
                return;
            }
            ProgressRing.IsActive = false;
        }
        private async void FetchAttendees()
        {
            try
            {
                ProgressRing.IsActive = true;
                await RecentAttendees.FetchRecentlyUsedAttendeesAsync();
                foreach (Attendee attendee in RecentAttendees)
                    AttendeesStackPanel.Items.Add(attendee);
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                ProgressRing.IsActive = false;
                return;
            }

            ProgressRing.IsActive = false;
        }
        private async void AttendeeClicked(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                ProgressRing.IsActive = true;
                Attendee attendee = ((RecentlyUserListElement)sender).DataContext as Attendee;
                await Attendees.AddAsync(attendee, null, true);
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                ProgressRing.IsActive = false;
            }
            Frame.GoBack();
        }
        private void TBListFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            AttendeesStackPanel.Items.Clear();
            foreach (Attendee attendee in RecentAttendees)
            {
                if (attendee.VName.ToUpper().Contains(((TextBox)sender).Text.ToUpper()))
                {
                    AttendeesStackPanel.Items.Add(attendee);
                }
            }
        }
    }
}
