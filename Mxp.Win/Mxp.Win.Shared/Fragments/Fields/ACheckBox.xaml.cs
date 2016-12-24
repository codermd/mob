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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class ACheckBox : UserControl
    {

        public ACheckBox(Field field)
        {
            this.InitializeComponent();
            Field = field;
            this.CheckBox.IsEnabled = Field.IsEditable;
            Field.PropertyChanged += HandlePropertyChanged;
            SetVisual();
            this.CheckBox.IsEnabled = field.IsEditable;
        }

        private void SetVisual()
        {
            this.CheckBox.IsChecked = (bool)Field.Value;
        }

        private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetVisual();
        }

        public Field Field { get; set; }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(Field.IsEditable)
                this.Field.Value = this.CheckBox.IsChecked;
        }

        

    }
}

