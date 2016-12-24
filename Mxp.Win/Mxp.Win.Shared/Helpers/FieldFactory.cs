using Mxp.Core.Business;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Mxp.Win.Controls;

namespace Mxp.Win
{
    public static class FieldFactory
    {

        public static UserControl WrapField (Field field) {
            switch (field.Type) {
                case FieldTypeEnum.Lookup:
                case FieldTypeEnum.Amount:
                case FieldTypeEnum.Category:
                case FieldTypeEnum.Country:
                case FieldTypeEnum.LongString:
                case FieldTypeEnum.Time:
                case FieldTypeEnum.Currency:
                    return new ATextBlockToObject (field);
                case FieldTypeEnum.Combo:
                    return new AComboBox (field);
                case FieldTypeEnum.PolicyRule:
                    return new APolicyRule (field);
                case FieldTypeEnum.Integer:
                case FieldTypeEnum.Decimal:
                    return new AIntegerTextBox (field);
                case FieldTypeEnum.Boolean:
                    return new ACheckBox (field);
                case FieldTypeEnum.Date:
                    object[] values = new object[2];
                    field.extraInfo.Values.CopyTo (values, 0);
                    if (values[0] as String == "DATE-TIME" || values[1] as String == "DATE-TIME")
                        return new ATextBlockToObject (field);
                    else
                        return new ADatePicker (field);
                case FieldTypeEnum.String:
                    return new ATextBox (field);
                case FieldTypeEnum.AutocompleteString:
                    return new GooglePlacesTextbox (field) { HorizontalAlignment = HorizontalAlignment.Stretch };
                case FieldTypeEnum.FullText:
                    return new AFullTextBlock (field);
            }
            return null;
        }

    }
}

