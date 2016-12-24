using Mxp.Core.Business;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Imaging;

namespace Mxp.Win
{
    public sealed partial class AttendeeListElement : UserControl
    {
        public AttendeeListElement()
        {
            this.InitializeComponent();
        }

        private void Delete_Click(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (DeleteAttendeeRequest != null)
                DeleteAttendeeRequest(this, EventArgs.Empty);
        }
        public event EventHandler DeleteAttendeeRequest;

        private void MainGrid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(AttendeeDetailView), DataContext as Attendee);
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                IsShown = (DataContext as Attendee).IsShown;
                if (!Preferences.Instance.IsGTPEnabled)
                {
                    PicGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (IsShown)
                        PicGTP.Source = new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/AttendeeExpenseCell.png"));
                    else
                        PicGTP.Source = new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/AttendeeGTPNotShown.png"));
                }
            }
        }
        bool IsShown;
        private async void PicGrid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (IsShown)
                PicGTP.Source = new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/AttendeeGTPNotShown.png"));
            else
                PicGTP.Source = new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/AttendeeExpenseCell.png"));
            try
            {
                await (DataContext as Attendee).ChangeIsShownAsync();
                IsShown = !IsShown;
            }
            catch (ValidationError er)
            {
                if ((DataContext as Attendee).IsShown)
                    PicGTP.Source = new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/AttendeeExpenseCell.png"));
                else
                    PicGTP.Source = new BitmapImage(new Uri("ms-appx:" + "/Assets/icons/AttendeeGTPNotShown.png"));
                IsShown = (DataContext as Attendee).IsShown;
            }
        }

        public void ShowDelete(bool canManageAttendees)
        {
            if (!canManageAttendees)
                this.DeleteGrid.Visibility = Visibility.Collapsed;
        }
    }
}
