using System;
using Android.Widget;
using Mxp.Core.Business;
using Android.App;
using com.refractored.components.stickylistheaders;
using Android.Views;
using Mxp.Droid.Utils;
using Mxp.Droid.Helpers;
using Android.Util;
using System.Linq;
using Mxp.Droid.Filters;

namespace Mxp.Droid.Adapters
{		
	public class CategoryDialogAdapter : BaseAdapter<Product>, IStickyListHeadersAdapter, IFilterable
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CategoryDialogAdapter).Name;
		#pragma warning restore 0414

		private Product mSelectedProduct;
		private Activity mActivity;

		public Filter mCategoryFilter { get; private set; }
		public bool mFilterIsInvoked { get; set; }

		public Products FilteredProducts { get; set; }
		public Products OriginalProducts { get; set; }

		public CategoryDialogAdapter (Activity activity, Products products, Product product = null) : base () {
			this.mActivity = activity;
			this.mSelectedProduct = product;
			this.mCategoryFilter = new CategoryFilter (this);
			this.OriginalProducts = products;
			this.FilteredProducts = this.OriginalProducts;
		}

		public override int Count {
			get {
				return this.FilteredProducts.Count;
			}
		}

		public override Product this[int index] {
			get {
				return this.FilteredProducts [index];
			}
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			CategoryViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_categories_item, parent, false);
				viewHolder = new CategoryViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as CategoryViewHolder;
			}

			viewHolder.BindView (this[position]);

			return convertView;
		}

		public View GetHeaderView(int position, View convertView, ViewGroup parent) {
			CategoryHeaderViewHolder headerViewHolder;

			if (convertView == null || convertView.Tag == null || !(convertView.Tag is CategoryHeaderViewHolder)) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.List_categories_header, parent, false);
				headerViewHolder = new CategoryHeaderViewHolder (convertView);
				convertView.Tag = headerViewHolder;
			} else {
				headerViewHolder = convertView.Tag as CategoryHeaderViewHolder;
			}

			headerViewHolder.BindView (this[position]);

			return convertView;
		}

		public long GetHeaderId(int position) {
			return this [position].ExpenseCategory.Name.Substring (0, 1)[0];
		}

		public Filter Filter {
			get {
				return this.mCategoryFilter;
			}
		}

		private class CategoryViewHolder : ViewHolder<Product>
		{
			private TextView Text { get; set; }

			public CategoryViewHolder (View convertView) {
				this.Text = convertView.FindViewById<TextView> (Android.Resource.Id.Text1);
			}

			public override void BindView (Product product) {
				this.Text.Text = product.ExpenseCategory.Name;
			}
		}

		private class CategoryHeaderViewHolder : ViewHolder<Product>
		{
			private TextView Text { get; set; }

			public CategoryHeaderViewHolder (View convertView) {
				this.Text = convertView.FindViewById<TextView> (Resource.Id.Text);
			}

			public override void BindView (Product product) {
				this.Text.Text = product.ExpenseCategory.Name.Substring (0, 1);
			}
		}
	}
}