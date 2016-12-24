using System;
using Android.Widget;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;

namespace Mxp.Droid.Filters
{
	public class LookupFieldFilter : Filter
	{
		private readonly LookupFieldDialogAdapter mAdapter;
		private readonly LookupField mLookupField;

		public LookupFieldFilter (LookupFieldDialogAdapter adapter, LookupField lookupField) {
			this.mAdapter = adapter;
			this.mLookupField = lookupField;
		}

		protected override FilterResults PerformFiltering (Java.Lang.ICharSequence constraint) {
			return null;
		}

		protected override async void PublishResults (Java.Lang.ICharSequence constraint, FilterResults results) {
			string filterString = constraint.ToString ().ToLowerInvariant ();

			try {
				await this.mLookupField.FetchItems (filterString);
			} catch (Exception) {  }

			this.mAdapter.FilteredDetailFields = this.mLookupField.Results;
			this.mAdapter.NotifyDataSetChanged ();
		}
	}
}