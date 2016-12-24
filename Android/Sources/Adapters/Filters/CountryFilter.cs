using System;

using Android.Widget;

using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Mxp.Core.Business;

namespace Mxp.Droid.Filters
{
	public class CountryFilter : Filter
	{
		private CountryListSectionSource adapter;

		public CountryFilter (CountryListSectionSource adapter) {
			this.adapter = adapter;
		}

		protected override FilterResults PerformFiltering (Java.Lang.ICharSequence constraint) {
			String filterString = constraint.ToString ().ToLowerInvariant ();

			FilterResults results = new FilterResults();

			JavaObjectHolder<Countries> countries = new JavaObjectHolder<Countries> (this.adapter.OriginalCountries.SearchWith (filterString));

			results.Values = countries;
			results.Count = countries.Instance.Count;

			return results;
		}

		protected override void PublishResults (Java.Lang.ICharSequence constraint, FilterResults results) {
			this.adapter.FilteredCountries = ((JavaObjectHolder<Countries>) results.Values).Instance;
			this.adapter.ParentAdapter.NotifyDataSetChanged ();
		}
	}
}