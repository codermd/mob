using System;

using Android.Widget;

using Mxp.Core.Business;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using com.refractored.components.stickylistheaders;
using Android.Util;
using Android.Content;
using System.Diagnostics;
using Mxp.Droid.Helpers;
using Android.Graphics;
using System.Net;
using System.Threading.Tasks;
using Squareup.Picasso;
using Java.IO;

namespace Mxp.Droid.Adapters
{
	public class ReceiptAdapter : BaseAdapter<Receipt>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ReceiptAdapter).Name;
		#pragma warning restore 0414

		private Receipts mReceipts;
		private Activity mActivity;

		public ReceiptAdapter (Activity activity, Receipts receipts) : base () {
			this.mActivity = activity;
			this.mReceipts = receipts;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override Receipt this[int index] {
			get {
				return this.mReceipts [index];
			}
		}

		public override int Count {
			get {
				return this.mReceipts.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			ReceiptViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (Resource.Layout.Grid_receipts_item, parent, false);
				viewHolder = new ReceiptViewHolder (this.mActivity, convertView);
				convertView.Tag = viewHolder;
			} else {
				viewHolder = convertView.Tag as ReceiptViewHolder;
			}

			viewHolder.BindView (this [position]);

			return convertView;
		}

		private class ReceiptViewHolder : ViewHolder<Receipt> {
			private Activity mActivity;

			private ImageView ImageView { get; set; }

			public ReceiptViewHolder (Activity activity, View convertView) {
				this.mActivity = activity;
				this.ImageView = convertView.FindViewById<ImageView> (Resource.Id.Image);
			}

			public override void BindView (Receipt receipt) {
				if (receipt.IsDocument)
					this.ImageView.SetImageResource (Resource.Drawable.ic_doc_file);
				else {
					if (receipt.AttachmentPath != null)
						Picasso.With (this.mActivity).Load (receipt.AttachmentPath).Resize (100, 100).CenterCrop ().Into (this.ImageView);
					else if (receipt.base64 != null)
						TaskConfigurator.Create ()
								.Finally<Bitmap> (this.ImageView.SetImageBitmap)
								.Start (BitmapHelper.DecodeBase64 (receipt.base64));
				}
			}
		}
	}
}