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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class AComboBox : UserControl
    {
        public AComboBox(Field field)
        {
            this.InitializeComponent();

            Field = field;
            SetVisual();
            this.IsEnabled = Field.IsEditable;

            Field.FieldChanged += HandleChange;
            Field.PropertyChanged += HandlePropertyChanged;
            Field.Model.PropertyChanged += HandlePropertyChanged;
        }

        private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.SetVisual();
        }
        private void HandleChange(object sender, EventArgs e)
        {
            this.SetVisual();
        }
        public async void SetVisual()
        {
            Collection<MenuFlyoutItem> items = new Collection<MenuFlyoutItem>();
            
            foreach (var choice in ((ComboField)Field).Choices)
            {
                MenuFlyoutItem item = new MenuFlyoutItem();
                item.Text = choice.VTitle;
                item.DataContext = choice;
                item.Foreground = new SolidColorBrush(Colors.Black);
                item.MinWidth = 600;
                item.Click += ItemClicked;
                items.Add(item);
            }
            MenuFlyout.ItemsSource = items;
            
            var choices = ((ComboField)Field).Choices;
            if (!choices.Contains(Field.Value) && choices.Count > 0)
            {
                Field.Value = ((ComboField)Field).Choices[0];
            }
            ChoiceButton.Content = Field.VValue;

        }
        private void ItemClicked(object sender, RoutedEventArgs e)
        {
            Field.Value = ((MenuFlyoutItem)sender).DataContext;
            ChoiceButton.Content = Field.VValue;
            this.MenuFlyout.Hide();
        }
        public Field Field { get; set; }
    }
}
