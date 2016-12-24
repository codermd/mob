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
using Mxp.Core.Services.Responses;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{
	public class ReportApprovalAdapter : BaseAdapter<ReportApproval>
	{
		private ReportApprovals reportApprovals;
		private Activity mContext;

		public ReportApprovalAdapter (Activity context, ReportApprovals reportApprovals) : base () {
			this.mContext = context;
			this.reportApprovals = reportApprovals;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override ReportApproval this[int index] {
			get {
				return this.reportApprovals [index];
			}
		}

		public override int Count {
			get {
				return this.reportApprovals.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			ReportApprovalViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mContext.LayoutInflater.Inflate (Resource.Layout.List_report_approvals_item, parent, false);
				viewHolder = new ReportApprovalViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as ReportApprovalViewHolder;
			}

			viewHolder.BindView (this.reportApprovals[position]);

			return convertView;
		}

		private class ReportApprovalViewHolder : ViewHolder<ReportApproval> {
			private TextView Employee { get; set; }
			private TextView Title { get; set; }
			private TextView Price { get; set; }
			private TextView Date { get; set; }

			public ReportApprovalViewHolder (View convertView) {
				this.Employee = convertView.FindViewById<TextView> (Resource.Id.Employee);
				this.Title = convertView.FindViewById<TextView> (Resource.Id.Title);
				this.Price = convertView.FindViewById<TextView> (Resource.Id.Price);
				this.Date = convertView.FindViewById<TextView> (Resource.Id.Date);
			}

			public override void BindView (ReportApproval reportApproval) {
				this.Employee.Text = reportApproval.VEmployeeFullname;
				this.Title.Text = reportApproval.Report.Name;
				this.Price.Text = reportApproval.Report.VAmount;
				this.Date.Text = reportApproval.VDateRange;
			}
		}
	}
}