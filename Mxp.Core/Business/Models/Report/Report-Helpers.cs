using System;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Mxp.Core.Business
{
	public partial class Report
	{
		public bool IsFromApproval {
			get {
				return this.Approval != null;
			}
		}
			
		public bool CanApproveExpense {
			get {
				return this.IsFromApproval;
			}
		}

		public bool HasDateRange {
			get {
				return this.FromDate.HasValue && this.ToDate.HasValue;
			}
		}

		private bool IsDeletable {
			get {
				return this.IsDraft;
			}
		}

		private bool IsSubmitable {
			get {
				return this.IsDraft;
			}
		}

		private bool IsCancelable {
			get {
				return this.IsOpen;
			}
		}

		public bool IsNew {
			get {
				return !this.Id.HasValue;
			}
		}

		public bool IsEditable {
			get {
				return this.IsNew;
			}
		}

		public bool CanRemoveExpenses {
			get {
				return this.IsNew || this.IsDraft || this.IsOpen;
			}
		}

		public bool CanAddExpenses {
			get {
				return this.IsNew || this.IsDraft;
			}
		}

		public bool IsDraft {
			get {
				return this.ReportType == Reports.ReportTypeEnum.Draft && this.ApprovalStatus != ApprovalStatusEnum.Rejected;
			}
		}

		public bool IsOpen {
			get {
				return this.ReportType == Reports.ReportTypeEnum.Open || (this.ReportType == Reports.ReportTypeEnum.Draft && this.ApprovalStatus == ApprovalStatusEnum.Rejected);
			}
		}

		public bool IsClosed {
			get {
				return this.ReportType == Reports.ReportTypeEnum.Closed;
			}
		}

		public bool CanShowHistory {
			get {
				return !this.IsDraft;
			}
		}

		public bool CanManageReceipts {
			get {
				return this.Receipts.CanManage;
			}
		}

		private void BindReceipts () {
			this.Receipts.CollectionChanged += HandleReceiptsChangedEventHandler;
		}

		public void HandleReceiptsChangedEventHandler (object sender, NotifyCollectionChangedEventArgs e) {
			this.NotifyPropertyChanged("NumberReceipts");
		}

		private void UnbindReceipts () {
			this.Receipts.CollectionChanged -= HandleReceiptsChangedEventHandler;
		}

		public bool CanShowReceipts {
			get {
				return Preferences.Instance.REPAttachment == VisibilityEnum.Show;
			}
		}

		public bool CanBeClosed => !this.IsNew && this.Expenses.Count == 0;
	}
}