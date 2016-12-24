using System;

using Android.Widget;

using Mxp.Core.Business;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using com.refractored.components.stickylistheaders;
using Android.Util;
using Android.Content;
using System.Diagnostics;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{
	public class ReportAdapter : BaseAdapter<Report>
	{
		private Reports reports;
		private Activity mActivity;

		public ReportAdapter (Activity activity, Reports expenses) : base () {
			this.mActivity = activity;
			this.reports = expenses;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override Report this[int index] {
			get {
				return this.reports [index];
			}
		}

		public override int Count {
			get {
				return this.reports.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			ReportViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_reports_item, parent, false);
				viewHolder = new ReportViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as ReportViewHolder;
			}

			viewHolder.BindView (this.reports[position]);

			return convertView;
		}

		private class ReportViewHolder : ViewHolder<Report> {
			private TextView title;
			private TextView price;
			private TextView date;

			private ImageView approvalIcon;
			private ImageView acceptanceIcon;
			private ImageView policyIcon;

			public ReportViewHolder (View convertView) {
				this.title = convertView.FindViewById<TextView> (Resource.Id.Title);
				this.price = convertView.FindViewById<TextView> (Resource.Id.Price);
				this.date = convertView.FindViewById<TextView> (Resource.Id.Date);

				this.approvalIcon = convertView.FindViewById<ImageView> (Resource.Id.ApprovalIcon);
				this.acceptanceIcon = convertView.FindViewById<ImageView> (Resource.Id.AcceptanceIcon);
				this.policyIcon = convertView.FindViewById<ImageView> (Resource.Id.PolicyIcon);
			}

			public override void BindView (Report report) {
				this.title.Text = report.Name;
				this.price.Text = report.VAmount;
				this.date.Text = report.VDateRange;

				this.ConfigureApprovalIcon (report);
				this.ConfigureAcceptanceIcon (report);
				this.ConfigurePolicyIcon (report);
			}

			private void ConfigureApprovalIcon (Report report) {
				this.approvalIcon.Visibility = report.CanShowApprovalStatus ? ViewStates.Visible : ViewStates.Gone;

				if (!report.CanShowApprovalStatus)
					return;

				switch (report.ApprovalStatus) {
					case Report.ApprovalStatusEnum.Accepted:
						this.approvalIcon.SetImageResource (Resource.Drawable.report_approved);
						break;
					case Report.ApprovalStatusEnum.Rejected:
						this.approvalIcon.SetImageResource (Resource.Drawable.report_refused);
						break;
					case Report.ApprovalStatusEnum.Waiting:
						this.approvalIcon.SetImageResource (Resource.Drawable.ic_report_is_pending_schedule);
						break;
				}
			}

			private void ConfigureAcceptanceIcon (Report report) {
				this.acceptanceIcon.Visibility = report.CanShowReceiptStatus ? ViewStates.Visible : ViewStates.Gone;

				if (!report.CanShowReceiptStatus)
					return;

				switch (report.ReceiptStatus) {
					case Report.ReceiptStatusEnum.Black:
						this.acceptanceIcon.SetImageResource (Resource.Drawable.report_pending);
						break;
					case Report.ReceiptStatusEnum.Green:
						this.acceptanceIcon.SetImageResource (Resource.Drawable.report_accepted);
						break;
					case Report.ReceiptStatusEnum.Orange:
						this.acceptanceIcon.SetImageResource (Resource.Drawable.ic_action_ic_report_has_been_orange);
						break;
					case Report.ReceiptStatusEnum.Red:
						this.acceptanceIcon.SetImageResource (Resource.Drawable.report_rejected);
						break;
				}
			}

			private void ConfigurePolicyIcon (Report report) {
				switch (report.PolicyRule) {
					case Report.PolicyRulesEnum.Green:
						this.policyIcon.SetImageResource (Resource.Drawable.expense_is_comliant);
						break;
					case Report.PolicyRulesEnum.Orange:
						this.policyIcon.SetImageResource (Resource.Drawable.expense_not_compliant_policy);
						break;
					case Report.PolicyRulesEnum.Red:
						this.policyIcon.SetImageResource (Resource.Drawable.Expense_not_compliant);
						break;
				}
			}
		}
	}
}