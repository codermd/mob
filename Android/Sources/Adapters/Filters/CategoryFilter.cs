using System;
using Android.Widget;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Mxp.Core.Business;
using System.Linq;

namespace Mxp.Droid.Filters
{
	public class CategoryFilter : Filter
	{
		private CategoryDialogAdapter adapter;

		public CategoryFilter (CategoryDialogAdapter adapter) {
			this.adapter = adapter;
		}

		protected override FilterResults PerformFiltering (Java.Lang.ICharSequence constraint) {
			String filterString = constraint.ToString ().ToLowerInvariant ();

			FilterResults results = new FilterResults();

			JavaObjectHolder<Products> products = new JavaObjectHolder<Products> (this.adapter.OriginalProducts.SearchWith (filterString));

			results.Values = products;
			results.Count = products.Instance.Count;

			return results;
		}

		protected override void PublishResults (Java.Lang.ICharSequence constraint, FilterResults results) {
			this.adapter.FilteredProducts = ((JavaObjectHolder<Products>) results.Values).Instance;
			this.adapter.NotifyDataSetChanged ();
		}
	}
}