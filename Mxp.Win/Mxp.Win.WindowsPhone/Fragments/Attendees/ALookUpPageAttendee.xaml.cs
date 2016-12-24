using Mxp.Core.Business;
using Mxp.Core.Services;
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
    public sealed partial class ALookUpPageAttendee : Page
    {
        public ALookUpPageAttendee()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.Title.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Employee);

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
        private LookupField Field { get; set; }
        private LookupItems LUItems { get; set; }
        public ObservableCollection<LookupItem> SuggestionFrom { get; set; }
        private void SearchTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTB.Text == Labels.GetLoggedUserLabel(Labels.LabelEnum.Search))
            {
                SearchTB.Text = "";
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Windows.UI.Colors.Blue;
                SearchTB.Foreground = Brush1;
            }
        }
        private void SearchTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTB.Text == String.Empty)
            {
                SearchTB.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Search);
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Windows.UI.Colors.Gray;
                SearchTB.Foreground = Brush2;
            }
        }
        public Attendees Attendees { get; set; }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Attendees)
                Attendees = e.Parameter as Attendees;
            else if (e.Parameter is Attendee)
            {
                Attendee = e.Parameter as Attendee;
                Attendees = new Attendees();
                try
                {
                    await Attendees.FetchRelatedAttendeesAsync(Attendee);

                }catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
        Attendee Attendee;
        private async void SearchTB_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (SearchTB.Text != Labels.GetLoggedUserLabel(Labels.LabelEnum.Search))
            {
                try
                {
                    if (!String.IsNullOrWhiteSpace(sender.Text))
                    {
                        List<Attendee> result = await LookupService.Instance.FetchEmployee(sender.Text);
                        LookupResultAttendee packagesResult = new LookupResultAttendee();
                        foreach (Attendee attendee in result)
                        {
                            packagesResult.items.Add(new Mxp.Win.LookupResultAttendee.Item(attendee));
                        }
                        sender.ItemsSource = packagesResult.items;
                    }
                }
                catch (ValidationError error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.Verbose);
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();
                    return;
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);

                    return;
                }

            }
        }
        private async void SearchTB_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            try {
                ProgressRing.IsActive = true;
                SearchTB.IsEnabled = false;
                Attendee item = ((LookupResultAttendee.Item)args.SelectedItem).LUItem;
                await Attendees.AddAsync (item);
            }
            catch (Exception error) {
                MessageDialog messageDialog = new MessageDialog ((error is ValidationError) ? ((ValidationError)error).Verbose : error.Message);
                messageDialog.Commands.Add (new UICommand ("OK", (command) => { }));
                messageDialog.ShowAsync ();
                ProgressRing.IsActive = false;
                SearchTB.IsEnabled = true;
            }
            Frame.GoBack ();
        }
    }
    public class LookupResultAttendee
    {
        public LookupResultAttendee()
        {
            items = new List<Item>();
        }
        public class Item
        {
            public Item(Attendee item) { LUItem = item; }
            public Attendee LUItem { get; set; }

            public override string ToString()
            {
                return LUItem.Firstname;
            }
        }
        public List<Item> items { get; set; }
    }
}

