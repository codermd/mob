using Mxp.Core.Business;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Mxp.Win
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AttendeesListPage : Page
    {
        public AttendeesListPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.ChooseAttendee));


        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack();
            Frame.GoBack();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            ProgressRing.IsActive = true;

            if (e.Parameter is AttendeeSelection)
            {
                AttendeeSelection select = e.Parameter as AttendeeSelection;
                Attendee = select.Attendee;
                Attendees = select.Attendees;
                LocalAttendees = new Attendees();
                try
                {
                    await LocalAttendees.FetchRelatedAttendeesAsync(Attendee);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                foreach (Attendee attendee in LocalAttendees)
                    AttendeesStackPanel.Items.Add(attendee);
            }
            ProgressRing.IsActive = false;

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

        }
        Attendee Attendee;
        Attendees Attendees;
        Attendees LocalAttendees;
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
            Frame.GoBack();
        }
      
    }
}
