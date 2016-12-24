using System;
using Android.Widget;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Mxp.Core.Business;
using System.Linq;

namespace Mxp.Droid.Filters
{
	public class CurrencyFilter : Filter
	{
		private CurrencyListSectionSource adapter;

		public CurrencyFilter (CurrencyListSectionSource adapter) {
			this.adapter = adapter;
		}

		protected override FilterResults PerformFiltering (Java.Lang.ICharSequence constraint) {
			String filterString = constraint.ToString ().ToLowerInvariant ();

			FilterResults results = new FilterResults();

			JavaObjectHolder<Currencies> currencies = new JavaObjectHolder<Currencies> (this.adapter.OriginalCurrencies.SearchWith (filterString));

			results.Values = currencies;
			results.Count = currencies.Instance.Count;

			return results;
		}

		protected override void PublishResults (Java.Lang.ICharSequence constraint, FilterResults results) {
			this.adapter.FilteredCurrencies = ((JavaObjectHolder<Currencies>) results.Values).Instance;
			this.adapter.ParentAdapter.NotifyDataSetChanged ();
		}
	}
}