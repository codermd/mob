using System;

using Android.Views;
using Android.Widget;
using Android.App;

using Mxp.Core.Business;
using Mxp.Droid.Fragments;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Android.Content;

namespace Mxp.Droid
{
	public class PolicyRuleHolder : AbstractFieldHolder
	{
		public PolicyRuleHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override View GetView (int position, View convertView, ViewGroup parent, Field field) {
			this.Field = field;

			PolicyRyleViewHolder viewHolder = null;

			if (convertView == null || this.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_fields_policyrule_item, parent, false);
				viewHolder = new PolicyRyleViewHolder (convertView);
				convertView.Tag = new JavaObjectHolder<AbstractFieldHolder> (this);
				this.Tag = viewHolder;
			} else
				viewHolder = this.Tag as PolicyRyleViewHolder;

			viewHolder.BindView (this.Field);

			return convertView;
		}

		private class PolicyRyleViewHolder : ViewHolder<Field> {
			private TextView mValueView;
			private ImageView mPolicyIcon;

			public PolicyRyleViewHolder (View convertView) {
				this.mValueView = convertView.FindViewById<TextView> (Resource.Id.Value);
				this.mPolicyIcon = convertView.FindViewById<ImageView> (Resource.Id.PolicyIcon);
			}

			public override void BindView (Field field) {
				this.mValueView.Text = field.VValue;
				this.mPolicyIcon.SetImageResource (((ExpenseItem.PolicyRules)field.extraInfo ["Icon"]) == ExpenseItem.PolicyRules.Orange ? Resource.Drawable.expense_not_compliant_policy : Resource.Drawable.Expense_not_compliant);
			}
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			Android.Support.V4.App.DialogFragment dialogFragment = BaseDialogFragment.NewInstance (this.mActivity, BaseDialogFragment.DialogTypeEnum.MessageDialog, this.Field.VValue, "!");
			dialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
		}
	}
}