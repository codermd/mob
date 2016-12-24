using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Mxp.Win.Fragments.Fields;

namespace Mxp.Win
{
    public sealed partial class ATextBlockToObject : UserControl
    {
        public ATextBlockToObject(Field field)
        {
            this.InitializeComponent();
            this.Field = field;




            if (Field.Type == FieldTypeEnum.Lookup)
            {
                try
                {
                    ((LookupField)Field).FetchLookupItem();
                }
                catch (Exception error)
                {
                    MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                    messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                    messageDialog.ShowAsync();
                    return;
                }
                    ((LookupField)Field).LookupChanged += HandleChange;
            }
            else if (Field.Type == FieldTypeEnum.Category)
            {
                TextBlockValue.TextTrimming = TextTrimming.None;
            }

            if (Field.VValue != null)
                TextBlockValue.Text = Field.VValue;

            Field.FieldChanged += HandleChange;
            this.Field.Model.PropertyChanged += HandleChange;
            Field.PropertyChanged += HandleChange;
        }
        private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChanged"))
                this.refresh();
        }
        private void HandleChange(object sender, EventArgs e)
        {
            this.refresh();
        }
        public void refresh()
        {
            if (Field.VValue != null)
                this.TextBlockValue.Text = this.Field.VValue;
        }
        public Field Field { get; set; }
        public void OpenObject(object sender, TappedRoutedEventArgs e)
        {
            if (Field.IsEditable)
            {
                switch (Field.Type)
                {
                    case FieldTypeEnum.Lookup:
                        if (Field.IsEditable)
                            ((Frame)Window.Current.Content).Navigate(typeof(ALookupFromTextBlock), Field);
                        break;
                    case FieldTypeEnum.Date:
                        if (Field.IsEditable)
                            ((Frame)Window.Current.Content).Navigate(typeof(ADateTimePicker), Field);
                        break;
                    case FieldTypeEnum.LongString:
                        ((Frame)Window.Current.Content).Navigate(typeof(ALongTextBox), Field);
                        break;
                    case FieldTypeEnum.Amount:
                        if (Field.IsEditable)
                            ((Frame)Window.Current.Content).Navigate(typeof(AmountPage), Field);
                        break;
                    case FieldTypeEnum.Category:
                        if (!this.Field.IsEditable)
                            break;
                        if ((this.Field.Model is ExpenseItem) && (Preferences.Instance.FilterTravelCategory)) {
                            ((Frame)Window.Current.Content).Navigate (typeof (ASplitedList), Field);
                        }
                        else
                            ((Frame)Window.Current.Content).Navigate (typeof (AListFromTextBox), Field);
                        break;
                    case FieldTypeEnum.Country:
                    case FieldTypeEnum.Currency:
                        if (Field.IsEditable)
                        {
                            ((Frame)Window.Current.Content).Navigate(typeof(AListFromTextBox), Field);
                            MainController.Instance.ChangeCurrency();
                        }
                        break;

                }
            }
        }
    }
}

