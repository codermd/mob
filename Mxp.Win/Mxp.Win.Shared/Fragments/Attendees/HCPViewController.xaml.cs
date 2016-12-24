using System;
using Mxp.Core.Business;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class HCPViewController
    {
        public HCPViewController()
        {
            this.InitializeComponent();
            this.Title.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Hcp);

	        this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
	        this.Frame.GoBack();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= this.HardwareButtons_BackPressed;
        }
        Attendees Attendees { get; set; }
        Attendee Attendee;

        protected override void OnNavigatedTo(NavigationEventArgs e) {
	        if (e.NavigationMode != NavigationMode.Back || this.Attendee == null) {
		        this.Attendees = (Attendees)e.Parameter;
		        this.Attendee = new Attendee(AttendeeTypeEnum.HCP);
            }

            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;

            this.FieldsListView.ItemsSource = this.Attendee.FormFields;
	        this.FieldsListView.ScrollIntoView (this.Attendee.FormFields[0]);

		}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
	        this.ProgressStart();
            try
            {
	            this.Attendee.TryValidate();
                AttendeeSelection attendeeselect = new AttendeeSelection(this.Attendees, this.Attendee);
	            this.Frame.Navigate(typeof(HCPAttendeesListPage), attendeeselect);
            }
            catch (Exception error)
            {
                PopMessages.AsyncMessage(error.GetExceptionMessage());
            }


	        this.ProgressFinish();
        }
        public void ProgressStart()
        {
            this.ProgressGrid.Visibility = Visibility.Visible;
            this.ProgressRing.IsActive = true;
	        this.BottomAppBar.IsEnabled = false;
        }
        public void ProgressFinish()
        {
            this.ProgressGrid.Visibility = Visibility.Collapsed;
            this.ProgressRing.IsActive = false;
	        this.BottomAppBar.IsEnabled = true;
        }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
