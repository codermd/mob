using System;
using Android.App;
using Android.Widget;
using Mxp.Core.Business;
using Android.Views;
using Mxp.Droid.Helpers;
using com.refractored.components.stickylistheaders;
using Mxp.Droid.ViewHolders;
using Mxp.Core.Helpers;

namespace Mxp.Droid.Adapters
{
	public class SplitExpenseItemAdapter : BaseAdapter<ExpenseItem>, IStickyListHeadersAdapter
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(SplitExpenseItemAdapter).Name;
		#pragma warning restore 0414

		private ExpenseItems mExpenseItems;
		private Activity mActivity;

		private event EventHandler<EventArgsObject<ExpenseItem>> mOnClickHandler;

		public SplitExpenseItemAdapter (Activity activity, ExpenseItems expenseItems, EventHandler<EventArgsObject<ExpenseItem>> onClickHandler) : base () {
			this.mActivity = activity;
			this.mExpenseItems = expenseItems;
			this.mOnClickHandler = onClickHandler;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override ExpenseItem this[int index] {
			get {
				return this.mExpenseItems [index];
			}
		}

		public override int Count {
			get {
				return this.mExpenseItems.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			SplitExpenseItemViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_expenses_item_split, parent, false);
				viewHolder = new SplitExpenseItemViewHolder (convertView, this.mOnClickHandler);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as SplitExpenseItemViewHolder;
			}

			viewHolder.BindView (this [position]);

			return convertView;
		}

		public virtual View GetHeaderView (int position, View convertView, ViewGroup parent) {
			SectionHeaderViewHolder headerViewHolder;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_section_header, parent, false);
				headerViewHolder = new SectionHeaderViewHolder (convertView);
				convertView.Tag = headerViewHolder;
			} else {
				headerViewHolder = convertView.Tag as SectionHeaderViewHolder;
			}

			headerViewHolder.BindView (this [position].GetModelParent<ExpenseItem, ExpenseItem> ().VTitleSplit);

			return convertView;
		}

		public virtual long GetHeaderId (int position) {
			return 0;
		}

		private class SplitExpenseItemViewHolder : ViewHolder<ExpenseItem> {
			private TextView TitleView;
			private TextView ValueView;

			private ExpenseItem currentExpenseItem;

			public SplitExpenseItemViewHolder (View convertView, EventHandler<EventArgsObject<ExpenseItem>> onClickHandler) {
				this.TitleView = convertView.FindViewById<TextView> (Resource.Id.Key);
				this.ValueView = convertView.FindViewById<TextView> (Resource.Id.Value);
				convertView.FindViewById<ImageButton> (Resource.Id.DeleteButton).Click += (object sender, EventArgs e) => {
					onClickHandler (this, new EventArgsObject<ExpenseItem> (this.currentExpenseItem));
				};
			}

			public override void BindView (ExpenseItem expenseItem) {
				this.currentExpenseItem = expenseItem;

				this.TitleView.Text = expenseItem.VCategoryName;
				this.ValueView.Text = expenseItem.VAmountLC;
			}
		}
	}
}