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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class SpouseViewController : Page
    {
        public SpouseViewController()
        {
            this.InitializeComponent();
            this.Title.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Spouse);
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
        Attendees Attendees { get; set; }
        Attendee Attendee;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Attendees = (Attendees)e.Parameter;
            Attendee = new Attendee(AttendeeTypeEnum.Spouse);
            FieldsListView.ItemsSource = Attendee.FormFields;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ProgressStart();
            try
            {
                await Attendees.AddAsync(Attendee);
                Frame.GoBack();
            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.GetExceptionMessage());
            }
            ProgressFinish();
        }

        public void ProgressStart()
        {
            this.ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRing.IsActive = true;
            BottomAppBar.IsEnabled = false;
        }
        public void ProgressFinish()
        {
            this.ProgressGrid.Visibility = Visibility.Collapsed;
            this.ProgressRing.IsActive = false;
            BottomAppBar.IsEnabled = true;
        }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
