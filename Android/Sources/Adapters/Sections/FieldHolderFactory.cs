using System;
using Mxp.Core.Business;
using Android.App;
using Android.Widget;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{
	public static class FieldHolderFactory
	{
		public static AbstractFieldHolder GetFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity, FieldTypeEnum type) {
			switch (type) {
				case FieldTypeEnum.Date:
					return new DateFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Category:
					return new CategoryFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Country:
					return new CountryFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Amount:
					return new AmountFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Currency:
					return new CurrencyFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.String:
				case FieldTypeEnum.LongString:
				case FieldTypeEnum.AutocompleteString:
					return new StringFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.FullText:
					return new FullTextFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Boolean:
					return new BooleanFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Lookup:
					return new LookupFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Integer:
					return new IntegerFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Decimal:
					return new DecimalFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Combo:
					return new ComboFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.Time:
					return new TimeFieldHolder (parentAdapter, activity);
				case FieldTypeEnum.PolicyRule:
					return new PolicyRuleHolder (parentAdapter, activity);
				default:
					return new StringFieldHolder (parentAdapter, activity);
			}
		}
	}
}