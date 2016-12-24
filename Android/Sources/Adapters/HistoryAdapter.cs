using System;
using Android.App;
using Android.Widget;
using Mxp.Core.Business;
using Android.Views;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Adapters
{
	public class HistoryAdapter : BaseAdapter<ReportHistoryItem>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(HistoryAdapter).Name;
		#pragma warning restore 0414

		private ReportHistoryItems mReportHistoryItems;
		private Activity mActivity;

		public HistoryAdapter (Activity activity, ReportHistoryItems reportHistoryItems) : base () {
			this.mActivity = activity;
			this.mReportHistoryItems = reportHistoryItems;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override ReportHistoryItem this[int index] {
			get {
				return this.mReportHistoryItems [index];
			}
		}

		public override int Count {
			get {
				return this.mReportHistoryItems.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			ReportHistoryItemViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_report_history_item, parent, false);
				viewHolder = new ReportHistoryItemViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else
				viewHolder = convertView.Tag as ReportHistoryItemViewHolder;

			viewHolder.BindView (this [position]);

			return convertView;
		}

		private class ReportHistoryItemViewHolder : ViewHolder<ReportHistoryItem> {
			private TextView Line { get; set; }
//			private TextView Date { get; set; }
			private TextView Comment { get; set; }

			public ReportHistoryItemViewHolder (View convertView) {
				this.Line = convertView.FindViewById<TextView> (Resource.Id.Line);
//				this.Date = convertView.FindViewById<TextView> (Resource.Id.Date);
				this.Comment = convertView.FindViewById<TextView> (Resource.Id.Comment);
			}

			public override void BindView (ReportHistoryItem reportHistoryItem) {
				this.Line.Text = reportHistoryItem.Line;
//				this.Date.Text = reportHistoryItem.VDate;
				this.Comment.Text = reportHistoryItem.Comment;
			}
		}
	}
}