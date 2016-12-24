using System;
using Android.Widget;
using Android.App;
using Android.Views;
using Mxp.Droid.Adapters;
using com.refractored.components.stickylistheaders;
using System.Collections.Generic;
using Mxp.Droid.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mxp.Droid.Adapters
{
	public abstract class BaseSectionAdapter<O> : BaseAdapter<O>, IStickyListHeadersAdapter, IFilterable, IChildFragmentManager
	{
		protected Activity mActivity;
		public bool mFilterIsInvoked { get; set; }
		private BaseAdapter<O>[] mAdapters;

		public BaseSectionAdapter (Activity activity) {
			this.mActivity = activity;
			this.mAdapters = new BaseAdapter<O> [this.SectionCount];
		}

		public BaseSectionAdapter (Activity activity, int sectionCount) {
			this.mActivity = activity;
			this.mAdapters = new BaseAdapter<O> [sectionCount];
		}

		// TODO Use this property
		public virtual bool IsFilterable {
			get {
				return this.FilterOnSection != -1;
			}
		}

		public virtual int FilterOnSection {
			get {
				return 0;
			}
		}

		public Filter Filter {
			get {
				return ((IFilterable) this.GetSection(this.FilterOnSection)).Filter;
			}
		}

		public override sealed O this[int index] {
			get {
				if (this.mFilterIsInvoked) {
					return this.GetSection (this.FilterOnSection) [index];
				}
					
				int sectionPosition;
				index = this.GetIndexForSection (index, out sectionPosition);

				return this.GetSection(sectionPosition) [index];
			}
		}

		public override sealed int Count {
			get {
				return this.mFilterIsInvoked ? this.GetSection(this.FilterOnSection).Count : this.TotalCount;
			}
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			if (this.mFilterIsInvoked) {
				return this.GetSection (this.FilterOnSection).GetView (position, convertView, parent);
			}

			int sectionPosition;
			position = this.GetIndexForSection (position, out sectionPosition);

			return this.GetSection(sectionPosition).GetView (position, convertView, parent);
		}

		// TODO Recursive
		private int TotalCount {
			get {
				int count = 0;

				for (int i = 0; i < this.SectionCount; i++) {
					count += this.GetSection (i).Count;
				}

				return count;
			}
		}
			
		public int GetIndexForSection (int index, out int positionSection) {
			BaseAdapter<O> section = null;

			for (positionSection = 0; positionSection < this.SectionCount; positionSection++) {
				section = this.GetSection (positionSection);
				if (index < section.Count) {
					return index;
				}

				index -= section.Count;
			}

			return -1;
		}

		public int GetPositionForSection (BaseAdapter<O> section) {
			return Array.IndexOf (this.mAdapters, section);
		}

		public BaseAdapter<O> GetSection (int position) {
			if (this.mAdapters [position] == null) {
				this.mAdapters [position] = this.InstantiateSection (position);
			}

			return this.mAdapters [position];
		}

		public abstract BaseAdapter<O> InstantiateSection (int position);
		public abstract int SectionCount { get; }
		public virtual Android.Support.V4.App.FragmentManager GetChildFragmentManager () {
			return null;
		}

		public virtual View GetHeaderView(int position, View convertView, ViewGroup parent) {
			if (this.mFilterIsInvoked) {
				return ((IStickyListHeadersAdapter) this.GetSection (this.FilterOnSection)).GetHeaderView (position, convertView, parent);
			}

			int sectionPosition;
			position = this.GetIndexForSection (position, out sectionPosition);

			return ((IStickyListHeadersAdapter) this.GetSection(sectionPosition)).GetHeaderView (position, convertView, parent);
		}

		public virtual long GetHeaderId(int position) {
			if (this.mFilterIsInvoked) {
				return ((IStickyListHeadersAdapter) this.GetSection (this.FilterOnSection)).GetHeaderId (position);
			}

			int sectionPosition;
			position = this.GetIndexForSection (position, out sectionPosition);

			return ((IStickyListHeadersAdapter) this.GetSection(sectionPosition)).GetHeaderId (position);
		}

		public virtual void OnListItemClick (BaseAdapter<O> section, int positionSection, ListView listView, View view, int position, long id) {

		}

		public virtual void OnListItemClick (ListView listView, View view, int position, long id) {
			int sectionPosition;
			position = this.GetIndexForSection (position, out sectionPosition);

			this.OnListItemClick (this.GetSection(sectionPosition), sectionPosition, listView, view, position, id);
		}

		public override bool IsEnabled (int position) {
			int sectionPosition;
			position = this.GetIndexForSection (position, out sectionPosition);

			return this.GetSection (sectionPosition).IsEnabled (position);
		}
	}
}