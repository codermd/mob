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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class AllowanceSegmentDetails : Page
    {
        public AllowanceSegmentDetails()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.SegmentDetails));
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
            AllowanceSegment = (AllowanceSegment)e.Parameter;
            Fields = AllowanceSegment.GetAllowanceFields();
        }
        AllowanceSegment AllowanceSegment { get; set; }
        ExpenseItem ExpenseItem { get; set; }
        public Collection<Field> Fields { get; set; }

        public List<DetailField> SourceFields { get; set; }

        private void SegmentLoaded(object sender, RoutedEventArgs e)
        {
            FieldsListView.ItemsSource = Fields;
        }
    }
}
