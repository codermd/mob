using System;
using Foundation;
using ObjCRuntime;
using System.Collections.ObjectModel;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class FieldFactory
	{
		public static UITableViewCell GetCell(UITableView tableView, DataFieldCell field){
			UITableViewCell cell =  field.GetCell (tableView);
	
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, "DEFAULT");
			}
			return cell;

		}


		public static Collection<DataFieldCell> WrapFields(Collection<Field> fields){
			Collection<DataFieldCell> result = new Collection<DataFieldCell> ();

			foreach (Field field in fields) {
				result.Add (FieldFactory.WrapField (field));
			}

			return result;
		}

		public static DataFieldCell WrapField(Field field){

			switch (field.Type) {
			case FieldTypeEnum.Boolean:
				return new BooleanDataFieldCell(field);
			case FieldTypeEnum.LongString:
				return new LongStringDataFieldCell (field);
			case FieldTypeEnum.Integer:
				return new IntegerDataFieldCell (field);
			case FieldTypeEnum.Country:
				return new CountriesDataFieldCell(field);
			case FieldTypeEnum.String:
				return new StringDataFieldCell (field);
			case FieldTypeEnum.FullText:
				return new FullTextDataFieldCell(field);		
			case FieldTypeEnum.AutocompleteString:
				return new AutocompleteStringDataFieldCell(field);
			case FieldTypeEnum.Date:
				return new DateDataFieldCell (field);
			case FieldTypeEnum.Time:
				return new DateDataFieldCell (field);
			case FieldTypeEnum.Category:
				return new CategoryDataFieldCell (field);
			case FieldTypeEnum.Amount:
				return new AmountDataFieldCell (field);
			case FieldTypeEnum.Currency:
				return new CurrencyDataFieldCell (field);
			case FieldTypeEnum.PolicyRule: 
				return new PolicyTipDataFieldCell (field);
			case FieldTypeEnum.Combo:
				return new ComboDataFieldCell (field);
			case FieldTypeEnum.Lookup:
				return new LookupDataFieldCell ((LookupField)field);
				case FieldTypeEnum.Decimal:
					return new DecimalDataFieldCell (field);
			}

			return new DataFieldCell (field);
		}
	}
}