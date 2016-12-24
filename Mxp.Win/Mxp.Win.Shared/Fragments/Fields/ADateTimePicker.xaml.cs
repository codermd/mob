using Mxp.Core.Business;
using System;
using System.Collections.Generic;
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
    public sealed partial class ADateTimePicker : Page
    {
        public ADateTimePicker()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Title.Text = (LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Date));

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
            Field = e.Parameter as Field;
            
            TempTime = (DateTime)Field.Value;
            this.TimePickerField.Time = new TimeSpan(TempTime.Hour,TempTime.Minute,TempTime.Second);
        }
        Field Field { get; set; }
        DateTime TempTime;
     
        private void TimePickerField_DateChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            TempTime = new DateTime(TempTime.Year, TempTime.Month, TempTime.Day, e.NewTime.Hours, e.NewTime.Minutes, e.NewTime.Seconds);
        
        }

        private void DatePickerField_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            TempTime = new DateTime(e.NewDate.Year, e.NewDate.Month, e.NewDate.Day, ((DateTime)Field.Value).Hour, ((DateTime)Field.Value).Minute, ((DateTime)Field.Value).Second);
           
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Field.Value = TempTime;
            Frame.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
