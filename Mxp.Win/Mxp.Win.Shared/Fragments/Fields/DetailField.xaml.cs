using Mxp.Core.Business;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Popups;
using Mxp.Win.Controls;

namespace Mxp.Win
{
    public sealed partial class DetailField
    {
        public DetailField()
        {
            this.InitializeComponent();
            DataContextChanged += DetailField_DataContextChanged;
        }
        private async void DetailField_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue != null)
            {
                if (args.NewValue is FieldGroup)
                {
                    FieldGrp = args.NewValue as FieldGroup;
                    Field = FieldGrp.Field;
                }
                else if (args.NewValue is Field)
                {
                    Field = args.NewValue as Field;
                }
                FillValue();
            }
        }

        public UserControl Title { get; set; }
        public UserControl Value { get; set; }
        private async void FillValue()
        {
            if (Field != null) /*  &&*/
            {
                Value = FieldFactory.WrapField(Field);

                if (Value != null) //&& Field.Title!="Chargeable"
                {
                    Value.HorizontalAlignment = Value is GooglePlacesTextbox ? HorizontalAlignment.Stretch : HorizontalAlignment.Right;
                    if (Value is APolicyRule)
                    {
                        LeftGrid.Visibility = Visibility.Collapsed;
                        RightGrid.Visibility = Visibility.Collapsed;
                        PolicyGrid.Visibility = Visibility.Visible;
                        Value.SetValue(Grid.MaxHeightProperty, 100);
                        ColumnDefinition c = new ColumnDefinition();
                        Value.HorizontalAlignment = HorizontalAlignment.Left;
                        PolicyGrid.Children.Add(Value);
                    }
                    else
                    {
                        RightGrid.Children.Add(Value);
                        Title = new ATextBlock(Field);
                        Title.SetValue(Grid.ColumnProperty, 0);
                        Title.HorizontalAlignment = HorizontalAlignment.Left;
                        LeftGrid.Children.Add(Title);
                        Title.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0, 0));
                    }
                }
                else
                {
                }
            }
            else
            {
            }
        }
        private FieldGroup FieldGrp { get; set; }
        private Field Field { get; set; }
        public Object ItemsField { get; set; }
        private void FieldsGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Value is ATextBlockToObject)
                ((ATextBlockToObject)Value).OpenObject(null, null);
            if (Value is APolicyRule)
                Policy_Tapped();
        }
        private async void Policy_Tapped()
        {
            MessageDialog messageDialog = new MessageDialog(((APolicyRule)Value).Message);
            messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
            await messageDialog.ShowAsync();
        }
    }
}
