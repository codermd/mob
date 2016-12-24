using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class AIntegerTextBox : UserControl
    {
        public AIntegerTextBox(Field field)
        {
            this.InitializeComponent();
            CellField = field;
            CellField.FieldChanged += HandleChange;
            CellField.PropertyChanged += HandlePropertyChanged;
            SetVisual();
            this.TextBoxField.IsEnabled = CellField.IsEditable;
        }
        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.SetVisual();
        }
        private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.SetVisual();
        }
        private void HandleChange(object sender, EventArgs e)
        {
            this.SetVisual();
        }

        public void SetVisual()
        {
            if (CellField.VValue != null && !(TextBoxField.Text.EndsWith(",") || TextBoxField.Text.EndsWith(".")))
                this.TextBoxField.Text = CellField.VValue;
        }
        public Field CellField { get; set; }
        private void TextBoxField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxField.Text != "" && TextBoxField != null)
                try
                {
                    if (CellField.Type == FieldTypeEnum.Integer)
                        if (TextBoxField.Text.Contains('-'))
                        {
                            TextBoxField.Text = TextBoxField.Text.Replace("-", "");
                        }
                        else if ((CellField.Type == FieldTypeEnum.Decimal))
                        {
                            if (TextBoxField.Text.Contains('-'))
                            {
                                TextBoxField.Text = TextBoxField.Text.Replace("-", "");
                            }
                        }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //this.CellField.Value = 0;
                    TextBoxField.Text = "0";
                }
        }
        private void TextBoxField_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CellField.Type == FieldTypeEnum.Integer)
                    this.CellField.Value = int.Parse(TextBoxField.Text);
                else if ((CellField.Type == FieldTypeEnum.Decimal))
                    this.CellField.Value = Double.Parse(TextBoxField.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //this.CellField.Value = 0;
                TextBoxField.Text = "0";
            }

        }

        private void TextBoxField_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.ToString() == "189")
                e.Handled = true;
            else if (e.Key.ToString() == "188" || e.Key.ToString() == "190")
            {
                if (CellField.Type == FieldTypeEnum.Integer)
                    e.Handled = true;
                else if ((CellField.Type == FieldTypeEnum.Decimal))
                {
                    if (TextBoxField.Text.Contains(",") || TextBoxField.Text.Contains(".") || TextBoxField.Text.Contains("-"))
                    {
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
