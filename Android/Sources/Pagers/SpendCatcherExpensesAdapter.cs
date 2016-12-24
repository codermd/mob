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
using Squareup.Picasso;

namespace Mxp.Droid
{
	public class SpendCatcherExpensesAdapter : BaseAdapter<SpendCatcherExpense>
	{
		private static readonly string TAG = typeof(SpendCatcherExpensesAdapter).Name;

		private SpendCatcherExpenses mSpendCatcherExpenses;
		private Activity mActivity;

		public SpendCatcherExpensesAdapter (Activity activity, SpendCatcherExpenses spendCatcherExpenses) : base () {
			this.mActivity = activity;
			this.mSpendCatcherExpenses = spendCatcherExpenses;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override SpendCatcherExpense this[int index] {
			get {
				return this.mSpendCatcherExpenses [index];
			}
		}

		public override int Count {
			get {
				return this.mSpendCatcherExpenses.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			SpendCatcherExpenseViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_spendcatcher_expenses_item, parent, false);
				viewHolder = new SpendCatcherExpenseViewHolder (this.mActivity, convertView);
				convertView.Tag = viewHolder;
			} else
				viewHolder = convertView.Tag as SpendCatcherExpenseViewHolder;

			viewHolder.BindView (this.mSpendCatcherExpenses[position]);

			return convertView;
		}

		private class SpendCatcherExpenseViewHolder : ViewHolder<SpendCatcherExpense> {
			private Activity mActivity;

			private ImageView mReceiptImage;
			private TextView mDate;
			private TextView mCategory;
			private ImageView mCreditCardIcon;
			private ImageView mFlagIcon;

			public SpendCatcherExpenseViewHolder (Activity activity, View convertView) {
				this.mActivity = activity;

				this.mReceiptImage = convertView.FindViewById<ImageView> (Resource.Id.Receipt);
				this.mDate = convertView.FindViewById<TextView> (Resource.Id.Date);
				this.mCategory = convertView.FindViewById<TextView> (Resource.Id.Category);
				this.mCreditCardIcon = convertView.FindViewById<ImageView> (Resource.Id.CreditCardIcon);
				this.mFlagIcon = convertView.FindViewById<ImageView> (Resource.Id.FlagIcon);
			}

			public override void BindView (SpendCatcherExpense spendCatcherExpense) {
				Picasso.With (this.mActivity).Load (spendCatcherExpense.AttachmentPath).Resize (150, 150).CenterCrop ().Into (this.mReceiptImage);

				this.mDate.Text = spendCatcherExpense.VDate;
				this.mCategory.Text = spendCatcherExpense.Product?.ExpenseCategory.Name;

				this.mCreditCardIcon.Visibility = spendCatcherExpense.IsPaidByCC ? ViewStates.Visible : ViewStates.Gone;

				if (spendCatcherExpense.Country != null) {
					try {
						int resource = (int)typeof(Resource.Drawable).GetField(spendCatcherExpense.Country.VResourceName).GetValue(null);
						this.mFlagIcon.SetImageResource (resource);
					} catch (Exception e) {
						Log.Error (TAG, "Failure to get drawable id.", e);
						this.mFlagIcon.SetImageResource (Resource.Drawable.NoFlag);
					}
				} else 
					this.mFlagIcon.SetImageResource (Resource.Drawable.NoFlag);
			}
		}
	}
}
