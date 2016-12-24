namespace Mxp.Core.Business
{
    public class SpendCatcherCountryField : Field
    {
        public SpendCatcherCountryField(SpendCatcherExpense spendCatcherExpense) : base (spendCatcherExpense) {
            this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Country);
            this.Permission = FieldPermissionEnum.Optional;
            this.Type = FieldTypeEnum.Country;
        }

        public override bool IsEditable {
            get {
                return true;
            }
        }

        public override string VValue {
            get {
                return this.GetValue<Country>()?.Name;
            }
        }

        public override object Value {
            get {
                return this.GetModel<SpendCatcherExpense>().Country;
            }
            set {
                this.GetModel<SpendCatcherExpense>().Country = (Country)value;
                base.Value = value;
            }
        }
    }

    public class SpendCatcherCategoryField : Field
    {
        public SpendCatcherCategoryField(SpendCatcherExpense spendCatcherExpense) : base (spendCatcherExpense) {
            this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Category);
            this.Permission = FieldPermissionEnum.Optional;
            this.Type = FieldTypeEnum.Category;
        }

		public override bool IsEditable {
			get {
				return true;
			}
		}

        public override string VValue {
			get {
                return this.GetValue<Product>()?.ExpenseCategory.Name;
            }
        }

        public override object Value {
            get {
                return this.GetModel<SpendCatcherExpense>().Product;
            }
            set {
                this.GetModel<SpendCatcherExpense>().Product = (Product)value;
                base.Value = value;
            }
        }
    }

	public class SpendCatcherCreditCardField : Field {
		public SpendCatcherCreditCardField (SpendCatcherExpense spendCatcherExpense) : base (spendCatcherExpense) {
			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.SpendCatcherCheckbox);
			this.Permission = FieldPermissionEnum.Optional;
			this.Type = FieldTypeEnum.Boolean;
		}

		public override bool IsEditable {
			get {
				return true;
			}
		}

		public override object Value {
			get {
				return this.GetModel<SpendCatcherExpense> ().IsPaidByCC;
			}
			set {
				this.GetModel<SpendCatcherExpense> ().IsPaidByCC = (bool)value;
				base.Value = value;
			}
		}
	}
}
