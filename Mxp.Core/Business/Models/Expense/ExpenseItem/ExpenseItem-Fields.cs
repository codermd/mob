using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Diagnostics;

namespace Mxp.Core.Business
{
	public partial class ExpenseItem
	{
		public virtual Collection<Field> GetAllFields () {
			return this.GetModelParent<ExpenseItem, Expense> ().GetAllFields (this);
		}
			
		public virtual Collection<Field> GetMainFields () {
			Collection<Field> result = this.ParentExpense.GetMainFields (this);

			if (this.CanShowPolicyTip)
				result.Insert (0, new ExpenseItemPolicyRule (this));

			return result;
		}

		public Collection<Field> AmountFields {
			get {
				return new Collection<Field> () {
					new ExpenseItemPrice (this),
					new ExpenseItemCurrency (this),
					new ExpenseItemQuantity (this)
				};
			}
		}

		private bool CanShowField (string key) {
			if (this.ParentExpense != null && this.ParentExpense is Mileage)
				return ((Mileage)this.ParentExpense).CanShowField (key);
			
			if (this.ParentExpense != null && this.ParentExpense is Allowance)
				return ((Allowance)this.ParentExpense).CanShowField (key);
			
			return this.Product.CanShowField (key);
		}

		private bool IsMandatory (string key) {
			if (this.ParentExpense != null && this.ParentExpense is Mileage)
				return ((Mileage)this.ParentExpense).IsMandatory (key);

			if (this.ParentExpense != null && this.ParentExpense is Allowance)
				return ((Allowance)this.ParentExpense).IsMandatory (key);

			return this.Product.IsMandatory (key);
		}

		public void ClearCachedFields () {
			this._detailsFields = null;
			this._dynamicFields = null;
		}

		private Collection<Field> _dynamicFields;
		public Collection<Field> DynamicFields {
			get {
				if (this._dynamicFields == null) {
					this._dynamicFields = new Collection<Field> ();

					this.ParentExpense.GetDynamicFieldsKeys ().ForEach (key => {
						if (this.CanShowField (key)) {
							DynamicFieldHolder dfield = this.ParentExpense.GetDynamicField (key);

							if (dfield == null)
								return;

							Field field = null;

							switch (dfield.LinkType) {
								case FieldTypeEnum.Lookup:
									field = new DynamicLookupField (this, dfield);
									break;
								case FieldTypeEnum.Combo:
									field = new DynamicComboField (this, dfield);
									break;
								default:
									field = new DynamicField (this, dfield);
									break;
							}

							field.IsEditable = this.IsNew || (this.ParentExpense.IsEditable && this.CanEdit > 0);
							field.Permission = this.IsMandatory (key) ? FieldPermissionEnum.Mandatory : FieldPermissionEnum.Optional;

							this._dynamicFields.Add (field);
						}
					});
				}

				return this._dynamicFields;
			}
		}

		private Collection<TableSectionModel> _detailsFields;
		public Collection<TableSectionModel> DetailsFields {
			get {
				this._detailsFields = new Collection<TableSectionModel> ();
				this._detailsFields.Add(new TableSectionModel(Labels.GetLoggedUserLabel (Labels.LabelEnum.General), this.GetMainFields ()));
				this._detailsFields.Add(new TableSectionModel(Labels.GetLoggedUserLabel (Labels.LabelEnum.Details), this.GetAllFields ()));
				return this._detailsFields;
			}
		}
	}
}