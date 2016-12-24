using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class Product : Model
	{
		public struct ExpenseCategoryStruct {
			public string Name;
			public int Id;

			public ExpenseCategoryStruct (string name, int id) {
				Name = name;
				Id = id;
			}
		}

		public int Id { get; set; }
		public ExpenseCategoryStruct ExpenseCategory { get; set; }

		// Expense
		public PermissionEnum Comment { get; set; }
		public PermissionEnum MerchantName { get; set; }
		public PermissionEnum MerchantCity { get; set; }
		public PermissionEnum InvoiceId { get; set; }

		// ExpenseItem statics
		public PermissionEnum ProjectId { get; set; }
		public PermissionEnum DepartmentId { get; set; }
		public PermissionEnum TravelRequestId { get; set; }

		// Mileage statics
		public PermissionEnum VehicleNumber { get; set; }

		// Allowance statics
		public PermissionEnum Allowanceline { get; set; }
		public PermissionEnum HotelLine { get; set; }
		public PermissionEnum VehicleOdometer { get; set; }

		// ExpenseItem dynamics
		public PermissionEnum Infochar1 { get; set; }
		public PermissionEnum Infochar2 { get; set; }
		public PermissionEnum Infochar3 { get; set; }
		public PermissionEnum Infochar4 { get; set; }
		public PermissionEnum Infochar5 { get; set; }
		public PermissionEnum Infochar6 { get; set; }
		public PermissionEnum Infochar7 { get; set; }
		public PermissionEnum Infochar8 { get; set; }
		public PermissionEnum Infonum1 { get; set; }
		public PermissionEnum Infonum2 { get; set; }

		public PermissionEnum Attendees { get; set; }

		public bool IsTravelProduct { get; set; }
		public bool CanShowInExpense { get; set; }

		public Product (ProductResponse productResponse) {
			this.Id = productResponse.ProductID;
			this.ExpenseCategory = new ExpenseCategoryStruct(productResponse.ExpenseCategoryName, productResponse.ExpenseCategoryID);
			this.Comment  = (PermissionEnum) productResponse.SIcomments;
			this.MerchantName  = (PermissionEnum) productResponse.SImerchantName;
			this.MerchantCity  = (PermissionEnum) productResponse.SImerchantCity;
			this.InvoiceId  = (PermissionEnum) productResponse.SIinvoiceId;
			this.ProjectId  = (PermissionEnum) productResponse.SIprojectId;
			this.DepartmentId  = (PermissionEnum) productResponse.SIdepartmentId;
			this.TravelRequestId  = (PermissionEnum) productResponse.SItrqid;
			this.VehicleOdometer  = (PermissionEnum) productResponse.SIvehicleOdometer;
			this.VehicleNumber  = (PermissionEnum) productResponse.SIvehicleNumber;
			this.Infochar1  = (PermissionEnum) productResponse.SIinfoChar1;
			this.Infochar2  = (PermissionEnum) productResponse.SIinfoChar2;
			this.Infochar3  = (PermissionEnum) productResponse.SIinfoChar3;
			this.Infochar4  = (PermissionEnum) productResponse.SIinfoChar4;
			this.Infochar5  = (PermissionEnum) productResponse.SIinfoChar5;
			this.Infochar6  = (PermissionEnum) productResponse.SIinfoChar6;
			this.Infochar7  = (PermissionEnum) productResponse.SIinfoChar7;
			this.Infochar8  = (PermissionEnum) productResponse.SIinfoChar8;
			this.Infonum1  = (PermissionEnum) productResponse.SIinfoNum1;
			this.Infonum2  = (PermissionEnum) productResponse.SIinfoNum2;
			this.Attendees  = (PermissionEnum) productResponse.SIattendees;
			this.Allowanceline  = (PermissionEnum) productResponse.Allowanceline;
			this.HotelLine  = (PermissionEnum) productResponse.HotelLine;
			this.CanShowInExpense = productResponse.List == 1;
			this.IsTravelProduct = !productResponse.NonTravelProduct;
		}
			
		public bool CanShowField (string key) {
			return this.CanShowPermission (this.GetPropertyValue<PermissionEnum> (key));
		}

		public FieldPermissionEnum GetFieldPermission (PermissionEnum permission) {
			return permission == PermissionEnum.Mandatory ? FieldPermissionEnum.Mandatory : FieldPermissionEnum.Optional;;
		}

		public bool IsMandatory (string key) {
			return this.GetPropertyValue<PermissionEnum> (key) == PermissionEnum.Mandatory;
		}

		public bool CanShowPermission (PermissionEnum permisson) {
			return permisson == PermissionEnum.Optional
				|| permisson == PermissionEnum.Mandatory;
		}

		// TODO Why compare expenseCategory ?
		public override bool Equals (object obj) {
			if (!(obj is Product)) {
				return false;
			}

			return this.Id == ((Product)obj).Id
				&& this.ExpenseCategory.Id == ((Product)obj).ExpenseCategory.Id ;
		}
	}
}