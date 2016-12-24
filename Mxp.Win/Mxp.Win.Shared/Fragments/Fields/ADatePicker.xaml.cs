using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class ADatePicker : UserControl
    {
        public ADatePicker(Field field)
        {
            this.InitializeComponent();
            Field = field;
            this.DatePickerField.IsEnabled = Field.IsEditable;
            SetVisual();
        }
        public void SetVisual()
        {
            if (Field.Value != null)
            {
                //Field.Value = Field.Value;
                this.DateLabel.Text = "";
                this.DateLabel.Text = Field.VValue;
                try
                {
                    this.DatePickerField.Date = (DateTime)Field.Value;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                this.DatePickerField.Date = DateTime.Today;
                this.DateLabel.Text = "";
            }
        }
        public Field Field { get; set; }
        private void DatePickerField_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            this.Field.Value = e.NewDate.DateTime;
            this.DateLabel.Text = "";
            this.DateLabel.Text = Field.VValue;
        }
        private void PickerTapped(object sender, TappedRoutedEventArgs e)
        {
            this.DatePickerField.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
        }
        private void DatePickerField_Holding(object sender, HoldingRoutedEventArgs e)
        {
            this.DatePickerField.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
        }
        private void DatePressed(object sender, PointerRoutedEventArgs e)
        {
            this.DatePickerField.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
        }
    }
}