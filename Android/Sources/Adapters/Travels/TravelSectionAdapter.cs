using System;
using Mxp.Core.Business;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.ObjectModel;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Adapters
{		
	public class TravelSectionAdapter : BaseSectionAdapter<WrappedObject>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(TravelSectionAdapter).Name;
		#pragma warning restore 0414

		private Collection<TableSectionModel> tableSections;
		public Android.Support.V4.App.FragmentManager FragmentManager { get; set; }

		public TravelSectionAdapter (Android.Support.V4.App.FragmentManager fragmentManager, Activity activity, Collection<TableSectionModel> tableSections) : base (activity, tableSections.Count) {
			this.FragmentManager = fragmentManager;
			this.tableSections = tableSections;
		}

		public override BaseAdapter<WrappedObject> InstantiateSection (int position) {
			return new FieldsSectionAdapter<TravelSectionAdapter> (this, this.mActivity, this.tableSections [position].Fields, this.tableSections [position].Title);
		}

		public override int SectionCount {
			get {
				return this.tableSections.Count;
			}
		}

		public override void OnListItemClick (BaseAdapter<WrappedObject> section, int sectionPosition, ListView listView, View view, int position, long id) {
			((FieldsSectionAdapter<TravelSectionAdapter>)section).OnListItemClick (listView, view, position, id);
		}

		public override Android.Support.V4.App.FragmentManager GetChildFragmentManager () {
			return this.FragmentManager;
		}
	}
}