using System;

using Android.Views;
using Android.Widget;

using Mxp.Core.Business;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Mxp.Droid.Fragments;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Android.App;
using Android.Support.V4.App;
using Mxp.Droid.Utils;

namespace Mxp.Droid
{
	public class LookupFieldHolder : AbstractFieldHolder
	{
		public LookupFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {
		
		}

		public override View GetView (int position, View convertView, ViewGroup parent, Field field) {
			this.Field = field;

			LookupFieldViewHolder viewHolder = null;

			if (convertView == null || this.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_fields_default_item, parent, false);
				viewHolder = new LookupFieldViewHolder (convertView, (FragmentActivity)this.mActivity);
				convertView.Tag = new JavaObjectHolder<AbstractFieldHolder> (this);
				this.Tag = viewHolder;
			} else
				viewHolder = this.Tag as LookupFieldViewHolder;

			viewHolder.BindView (this.Field);

			return convertView;
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (!this.Field.IsEditable)
				return;

			Android.Support.V4.App.DialogFragment dialogFragment = new LookupFieldPickerDialogFragment ((LookupField) Field, (object sender, EventArgsObject<LookupItem> e) => {
				((Android.Support.V4.App.DialogFragment) sender).Dismiss ();
				((LookupField) this.Field).Value = e.Object;
				((LookupField) this.Field).ResetResults ();
				this.ParentAdapter.NotifyDataSetChanged ();
			});
			dialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
		}

		private class LookupFieldViewHolder : FieldViewHolder<TextView>
		{
			private FragmentActivity mActivity;
			private LookupField mField;

			public LookupFieldViewHolder (View convertView, FragmentActivity activity) : base (convertView) {
				this.mActivity = activity;
			}

			public override void BindValue (Field field) {
				if (this.mField != null)
					this.mField.LookupChanged -= LookupChangedHandler;

				this.mField = (LookupField) field;

				this.mField.LookupChanged += LookupChangedHandler;

				try {
					this.mField.FetchLookupItem ();
				} catch (Exception e) {
					Android.Support.V4.App.DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (this.mActivity, this.mActivity.GetErrorDialogRequestCode (), BaseDialogFragment.DialogTypeEnum.ErrorDialog, e is ValidationError ? ((ValidationError)e).Verbose : Mxp.Core.Services.Service.NoConnectionError);
					errorDialogFragment.Show (this.mActivity.SupportFragmentManager, null);
				}

				this.RefreshValue ();
			}

			private void RefreshValue () {
				this.mValueView.Text = this.mField.VValue;
			}

			private void LookupChangedHandler (object sender, EventArgs e) {
				this.RefreshValue ();
			}
		}
	}
}