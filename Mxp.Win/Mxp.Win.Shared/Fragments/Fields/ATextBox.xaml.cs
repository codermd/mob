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
    public sealed partial class ATextBox : UserControl
    {
        public ATextBox(Field field)
        {
            this.InitializeComponent();
            CellField = field;
            this.TextBoxField.IsEnabled = CellField.IsEditable;
            SetVisual();

            CellField.PropertyChanged += this.CellField_PropertyChanged;
        }

        private void CellField_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName.Equals("Loading",StringComparison.OrdinalIgnoreCase))
                this.RefreshValue ();
        }

        private void RefreshValue () {
            this.TheProgressRing.IsActive = this.CellField.IsLoading;

            this.TextBoxField.Visibility = this.CellField.IsLoading ? Visibility.Collapsed : Visibility.Visible;
        }

        public void SetVisual()
        {
            if (CellField.VValue != null)
                this.TextBoxField.Text = CellField.VValue;
        }
        public Field CellField { get; set; }

        private void TextBoxField_TextChanged(object sender, TextChangedEventArgs e)
        {
            CellField.Value = TextBoxField.Text;
        }
    }
}

