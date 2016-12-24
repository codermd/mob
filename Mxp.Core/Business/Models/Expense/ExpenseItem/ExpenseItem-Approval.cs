namespace Mxp.Core.Business
{
	public partial class ExpenseItem
	{
		public void Approve () {
			this.StatusForApprovalReport = Status.Accepted;
		}

		public void Reject () {
			this.StatusForApprovalReport = Status.Rejected;
		}

		public void Toggle () {
			this.StatusForApprovalReport = this.StatusForApprovalReport == Status.Accepted ? Status.Rejected : Status.Accepted;
		}

		private Status _statusForApprovalReport = Status.Other;
		public Status StatusForApprovalReport {
			get {
				if (this._statusForApprovalReport == Status.Other) {
					if (Preferences.Instance.ApprovalSelectedExpenses)
						this._statusForApprovalReport = Status.Accepted;
					else
						this._statusForApprovalReport = this.PolicyRule == PolicyRules.Green ? Status.Accepted : Status.Rejected;
				}

				return this._statusForApprovalReport;
			}
			set {
				this._statusForApprovalReport = value;
			}
		}
	}
}