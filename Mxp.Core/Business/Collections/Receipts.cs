using System;
using System.Threading.Tasks;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using Mxp.Core.Utils;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Mxp.Core.Business
{
	public class Receipts : SGCollection<Receipt>
	{
		public Receipts (Model parent) : base (parent) {

		}

		public async override Task FetchAsync () {
			if (this.ParentModel is Expense && !((Expense)this.ParentModel).IsNew) {
				await ReceiptService.Instance.FetchExpenseReceiptsAsync ((Expense) this.ParentModel);
			} else if (this.ParentModel is Report && !((Report)this.ParentModel).IsNew) {
				await ReceiptService.Instance.FetchReportReceiptsAsync ((Report) this.ParentModel);
			}

			if (this.ParentModel is Expense) {
				((Expense)this.ParentModel).NumberReceipts = this.Count;
				((Expense)this.ParentModel).NotifyPropertyChanged ("NumberReceipts");
			}
		}

		public async Task AddReceipt (string base64Format) {
			if (this.ParentModel is Expense) {
				if(!((Expense) this.ParentModel).IsNew) {
					await ReceiptService.Instance.AddReceiptAsync ((Expense) this.ParentModel, base64Format);
					await ((Expense)this.ParentModel).SaveAsync (((Expense)this.ParentModel).ExpenseItems [0], true);
				} else {
					this.AddItem (new Receipt (base64Format));
				}
			} else if (this.ParentModel is Report) {
				if (!((Report)this.ParentModel).IsNew) {
					await ReceiptService.Instance.AddReceiptAsync ((Report)this.ParentModel, base64Format);
				} else {
					this.AddItem (new Receipt (base64Format));
				}
			}

			if (this.ParentModel is Expense) {
				((Expense)this.ParentModel).NumberReceipts = this.Count;
				((Expense)this.ParentModel).NotifyPropertyChanged ("NumberReceipts");
			}

			if (this.ParentModel is Report) {
				((Report)this.ParentModel).NumberReceipts = this.Count;
				((Report)this.ParentModel).NotifyPropertyChanged ("NumberReceipts");
			}
		}

		public bool CanDelete {
			get {
				return this.CanManage;
			}
		}

		public bool CanAdd {
			get {
				return this.CanManage;
			}
		}

		public bool CanManage {
			get {
				if (this.ParentModel != null && this.ParentModel is Report && (this.GetParentModel<Report> ().IsClosed || this.GetParentModel<Report> ().IsFromApproval))
					return false;
				
				if (this.ParentModel != null && this.ParentModel is Expense && ((this.GetParentModel<Expense> ().Report?.IsClosed ?? false) || this.GetParentModel<Expense> ().IsFromApproval))
					return false;

				return true;
			}
		}

		public async Task DeleteReceipt (Receipt receipt) {
			if (!this.CanDelete)
				return;

			if (this.ParentModel is Expense && !this.GetParentModel<Expense> ().IsNew) {
				await ReceiptService.Instance.RemoveReceiptAsync (receipt);
				await ((Expense)this.ParentModel).SaveAsync (((Expense)this.ParentModel).ExpenseItems [0], true);
			} else if (this.ParentModel is Report && !this.GetParentModel<Report> ().IsNew) {
				await ReceiptService.Instance.RemoveReceiptAsync (receipt);
			}
				
			this.Remove (receipt);

			if (this.ParentModel is Expense) {
				((Expense)this.ParentModel).NumberReceipts = this.Count;
				((Expense)this.ParentModel).NotifyPropertyChanged ("NumberReceipts");
			}

			if (this.ParentModel is Report) {
				((Report)this.ParentModel).NumberReceipts = this.Count;
				((Report)this.ParentModel).NotifyPropertyChanged ("NumberReceipts");
			}
		}

		public async Task UploadBase64Images () {
			Collection<String> base64Images = new Collection<String> ();
			this.ForEach (receipt => {
				if(receipt.base64 != null) {
					base64Images.Add(receipt.base64);
				}
			});
			this.Clear ();

			//List<Task> ts = new List<Task> ();

			// FIXME Error from server

			//base64Images.ForEach (base64Str => {
			//	ts.Add(this.AddReceipt(base64Str));
			//});

			foreach (var t in base64Images)
				await AddReceipt (t);

			//await Task.WhenAll (ts.ToArray ());
		}
	}
}