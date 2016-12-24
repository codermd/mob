using System;

using Android.Views;
using Android.Widget;
using Android.App;

using Mxp.Core.Business;
using Mxp.Droid.Fragments;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{
	public class BooleanFieldHolder : AbstractFieldHolder
	{
		public BooleanFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override View GetView (int position, View convertView, ViewGroup parent, Field field) {
			this.Field = field;

			BooleanFieldViewHolder viewHolder = null;

			if (convertView == null || this.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_fields_bool_item, parent, false);
				viewHolder = new BooleanFieldViewHolder (convertView);
				convertView.Tag = new JavaObjectHolder<AbstractFieldHolder> (this);
				this.Tag = viewHolder;
			} else
				viewHolder = this.Tag as BooleanFieldViewHolder;

			viewHolder.BindView (this.Field);

			return convertView;
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			// Do nothing
		}

		private class BooleanFieldViewHolder : FieldViewHolder<Switch>
		{
			private Field mField;

			public BooleanFieldViewHolder (View convertView) : base (convertView) {

			}

			public override void BindValue (Field field) {
				this.mValueView.CheckedChange -= CheckedChangeHandler;
				this.mField = field;

				this.mValueView.Checked = field.GetValue<bool> ();

				if (field.IsEditable) {
					this.mValueView.Enabled = true;
					this.mValueView.CheckedChange += CheckedChangeHandler;
				} else 
					this.mValueView.Enabled = false;
			}

			private void CheckedChangeHandler (object sender, CompoundButton.CheckedChangeEventArgs e) {
				this.mField.Value = e.IsChecked;
			}
		}
	}
}