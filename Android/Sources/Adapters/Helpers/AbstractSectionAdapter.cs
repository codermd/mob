using System;
using Android.Views;
using Android.App;
using Android.Widget;
using com.refractored.components.stickylistheaders;
using Mxp.Core.Helpers;
using Mxp.Droid.ViewHolders;

namespace Mxp.Droid.Adapters
{
	public abstract class AbstractSectionAdapter<O, P> : BaseAdapter<O>, IStickyListHeadersAdapter where P : BaseSectionAdapter<O>
	{
		public Activity Activity { get; }

		protected string mTitle;
		private int? _mPosition;
		protected int mPosition {
			get {
				if (this._mPosition == null)
					this._mPosition = this.ParentAdapter.GetPositionForSection (this);

				return this._mPosition.Value; 
			}
		}

		private WeakReferenceObject<P> weakParentAdapter;
		public P ParentAdapter {
			get {
				return this.weakParentAdapter == null ? null : this.weakParentAdapter.Value;
			}
			private set {
				this.weakParentAdapter = new WeakReferenceObject<P> (value);
			}
		}

		public AbstractSectionAdapter (P parentAdapter, Activity activity) : base () {
			this.ParentAdapter = parentAdapter;
			this.Activity = activity;
		}

		public AbstractSectionAdapter (P parentAdapter, Activity activity, string title) : this (parentAdapter, activity) {
			this.mTitle = title;
		}

		public virtual View GetHeaderView (int position, View convertView, ViewGroup parent) {
			SectionHeaderViewHolder headerViewHolder;

			if (convertView == null || convertView.Tag == null || !(convertView.Tag is SectionHeaderViewHolder)) {
				convertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_section_header, parent, false);
				headerViewHolder = new SectionHeaderViewHolder (convertView);
				convertView.Tag = headerViewHolder;
			} else {
				headerViewHolder = convertView.Tag as SectionHeaderViewHolder;
			}

			headerViewHolder.BindView (this.mTitle);

			return convertView;
		}

		public virtual long GetHeaderId (int position) {
			return this.mPosition;
		}

		public virtual void OnListItemClick (ListView listView, View view, int position, long id) {

		}

		public override void NotifyDataSetInvalidated () {
			this.NotifyDataSetChanged (false);
		}

		public override void NotifyDataSetChanged () {
			this.NotifyDataSetChanged (false);
		}

		public void NotifyDataSetChanged (bool force) {
			if (this.ParentAdapter == null)
				throw new InvalidOperationException ();

			if (force)
				base.NotifyDataSetChanged ();
			else
				this.ParentAdapter.NotifyDataSetChanged ();
		}
	}
}