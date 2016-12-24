using System;
using Android.App;
using Android.Widget;
using Mxp.Core.Business;
using Android.Views;
using Mxp.Droid.Helpers;
using Mxp.Core.Helpers;
using System.Collections.Generic;

namespace Mxp.Droid.Adapters
{
	public class IconsLegendAdapter : AbstractSectionAdapter<IconLegend, BaseSectionAdapter<IconLegend>>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(IconsLegendAdapter).Name;
		#pragma warning restore 0414

		private List<IconLegend> mIconsLegend;

		public IconsLegendAdapter (BaseSectionAdapter<IconLegend> parentAdapter, Activity activity, string title, List<IconLegend> iconsLegend) : base (parentAdapter, activity, title) {
			this.mIconsLegend = iconsLegend;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override IconLegend this[int index] {
			get {
				return this.mIconsLegend [index];
			}
		}

		public override int Count {
			get {
				return this.mIconsLegend.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			IconLegendItemViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.Settings_list_icons_legend_item, null);
				viewHolder = new IconLegendItemViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as IconLegendItemViewHolder;
			}

			viewHolder.BindView (this [position]);

			return convertView;
		}

		private class IconLegendItemViewHolder : ViewHolder<IconLegend> {
			private ImageView icon;
			private TextView legend;

			public IconLegendItemViewHolder (View convertView) {
				this.icon = convertView.FindViewById<ImageView> (Resource.Id.Icon);
				this.legend = convertView.FindViewById<TextView> (Resource.Id.Legend);
			}

			public override void BindView (IconLegend iconLegend) {
				switch (iconLegend.Icon) {
					case IconLegend.IconsEnum.Compliant:
						this.icon.SetImageResource (Resource.Drawable.expense_is_comliant);
						break;
					case IconLegend.IconsEnum.NotCompliantPolicy:
						this.icon.SetImageResource (Resource.Drawable.expense_not_compliant_policy);
						break;
					case IconLegend.IconsEnum.NotCompliant:
						this.icon.SetImageResource (Resource.Drawable.Expense_not_compliant);
						break;
					case IconLegend.IconsEnum.ReceiptsAttached:
						this.icon.SetImageResource (Resource.Drawable.report_pending);
						break;

					case IconLegend.IconsEnum.Accepted:
						this.icon.SetImageResource (Resource.Drawable.report_accepted);
						break;
					case IconLegend.IconsEnum.Rejected:
						this.icon.SetImageResource (Resource.Drawable.report_rejected);
						break;
					case IconLegend.IconsEnum.Pending:
						this.icon.SetImageResource (Resource.Drawable.report_pending);
						break;

					case IconLegend.IconsEnum.Approved:
						this.icon.SetImageResource (Resource.Drawable.report_approved);
						break;
					case IconLegend.IconsEnum.Refused:
						this.icon.SetImageResource (Resource.Drawable.report_refused);
						break;
					case IconLegend.IconsEnum.PendingSchedule:
						this.icon.SetImageResource (Resource.Drawable.ic_report_is_pending_schedule);
						break;
				}

				this.legend.Text = iconLegend.Legend;
			}
		}
	}
}