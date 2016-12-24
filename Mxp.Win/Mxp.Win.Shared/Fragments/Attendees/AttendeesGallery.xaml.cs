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
using Windows.UI.Input;
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
    public sealed partial class AttendeesGallery : UserControl
    {
        public AttendeesGallery()
        {
            this.InitializeComponent();
        }
        ExpenseItem ExpenseItem { get; set; }
        Boolean Showed = false;
        public void LoadAttendees(ExpenseItem exp)
        {
            if (!Showed && exp != null)
            {
                ExpenseItem = exp;
                Collection<Attendee> attendees = ExpenseItem.Attendees;
                AttendeesSource.Source = attendees;
                Showed = true;
            }

        }
        private async void DeleteRequest(object sender, EventArgs e)
        {
            if (ExpenseItem.CanManageAttendees)
            {
                var item = ((AttendeeListElement)sender).DataContext;
                MessageDialog messageDialog = new MessageDialog(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.DoYouConfirm), LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Delete) + " " + LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Attendees));
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Yes)), (command) => { DeleteAttendee(item); }));
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.No)), (command) => { }));
                await messageDialog.ShowAsync();
            }
        }
        private async void DeleteAttendee(object heldItem)
        {
            Attendee attendee = (Attendee)heldItem;

            if (ExpenseItem.Attendees.Contains(attendee) && ExpenseItem.CanManageAttendees)
            {
                try
                {
                    await ExpenseItem.Attendees.RemoveAsync(attendee);
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                    messageDialog.ShowAsync();
                }
                MainController.Instance.AttendeeDeleted();
            }
        }

        private void AttendeeListElement_Loaded(object sender, RoutedEventArgs e)
        {
            AttendeeListElement attendee = sender as AttendeeListElement;
            attendee.ShowDelete(ExpenseItem.CanManageAttendees);
        }

        
    }
}
