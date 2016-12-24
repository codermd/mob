using System;
using Android.App;
using Mxp.Core.Business;
using Android.Views;
using Android.Widget;
using Mxp.Droid.Fragments;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Mxp.Droid.Pagers;
using Android.Content;

namespace Mxp.Droid
{
	public class CategoryFieldHolder : AbstractFieldHolder
	{
		public CategoryFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {
			
		}

		protected override int LayoutResourceId {
			get {
				return Resource.Layout.List_fields_category_item;
			}
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (!this.Field.IsEditable)
				return;

			Android.Support.V4.App.DialogFragment dialogFragment = new CategoryPickerDialogFragment (LoggedUser.Instance.Products.ExpenseProducts, (object sender, EventArgsObject<Product> e) => {
				((Android.Support.V4.App.DialogFragment)sender).Dismiss ();
				// TODO Compare Category -> override == or use Equals method
				this.Field.Value = e.Object;

				if (this.mActivity is IPagerAdapter)
					((IPagerAdapter)this.mActivity).PagerAdapter.NotifyDataSetChanged ();
				
				this.ParentAdapter.NotifyDataSetChanged ();
			});
			dialogFragment.Show (((IChildFragmentManager) this.ParentAdapter).GetChildFragmentManager (), null);
		}
	}
}