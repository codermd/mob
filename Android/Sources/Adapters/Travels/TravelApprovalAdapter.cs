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
	public class TravelApprovalAdapter : BaseAdapter<TravelApproval>
	{
		private TravelApprovals travelApprovals;
		private Activity mContext;

		public TravelApprovalAdapter (Activity context, TravelApprovals travelApprovals) : base () {
			this.mContext = context;
			this.travelApprovals = travelApprovals;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override TravelApproval this[int index] {
			get {
				return this.travelApprovals [index];
			}
		}

		public override int Count {
			get {
				return this.travelApprovals.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			TravelApprovalViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mContext.LayoutInflater.Inflate (Resource.Layout.List_travel_approvals_item, parent, false);
				viewHolder = new TravelApprovalViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as TravelApprovalViewHolder;
			}

			viewHolder.BindView (this.travelApprovals[position]);

			return convertView;
		}

		private class TravelApprovalViewHolder : ViewHolder<TravelApproval> {
			private TextView Employee { get; set; }
			private TextView Title { get; set; }
			private TextView Date { get; set; }

			public TravelApprovalViewHolder (View convertView) {
				this.Employee = convertView.FindViewById<TextView> (Resource.Id.Employee);
				this.Title = convertView.FindViewById<TextView> (Resource.Id.Title);
				this.Date = convertView.FindViewById<TextView> (Resource.Id.Date);
			}

			public override void BindView (TravelApproval travelApproval) {
				this.Employee.Text = travelApproval.VEmployeeFullname;
				this.Title.Text = travelApproval.Travel.Name;
				this.Date.Text = travelApproval.VDateRange;
			}
		}
	}
}