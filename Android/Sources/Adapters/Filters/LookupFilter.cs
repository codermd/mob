using System;

using Android.Widget;

using Mxp.Core.Services;
using Mxp.Droid.Adapters;

namespace Mxp.Droid.Filters
{
	public class LookupFilter : Filter
	{
		private readonly LookupDialogAdapter mAdapter;
		private readonly LookupService.ApiEnum value;

		public LookupFilter (LookupDialogAdapter adapter, LookupService.ApiEnum value) {
			this.value = value;
			this.mAdapter = adapter;
		}

		protected override FilterResults PerformFiltering (Java.Lang.ICharSequence constraint) {
			return null;
		}

		protected override async void PublishResults (Java.Lang.ICharSequence constraint, FilterResults results) {
			string filterString = constraint.ToString ().ToLowerInvariant ();

			try {
				await LookupService.Instance.FetchLookUp (this.value, this.mAdapter.mLookupItems, filterString);
			} catch (Exception) { }

			this.mAdapter.NotifyDataSetChanged ();
		}
	}
}