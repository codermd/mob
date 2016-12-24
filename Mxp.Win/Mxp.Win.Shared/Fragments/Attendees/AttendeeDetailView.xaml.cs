using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.Phone.UI.Input;

namespace Mxp.Win
{
    public sealed partial class AttendeeDetailView : Page
    {
        Attendee Attendee;
        public Collection<Field> Fields { get; private set; }
        public AttendeeDetailView()
        {
            this.InitializeComponent();
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
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            if (Attendee == null)
                Attendee = (Attendee)e.Parameter;
            Title.Text = Attendee.VName;
            Fields = Attendee.AllFields;
            this.FieldsListView.ItemsSource = Fields;
        }
    }
}
