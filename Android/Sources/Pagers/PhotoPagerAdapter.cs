using System;
using Android.Support.V4.App;
using Mxp.Droid.Fragments;
using Mxp.Core.Business;
using Android.Support.V4.View;
using Android.Views;
using UK.CO.Senab.Photoview;
using Squareup.Picasso;
using Mxp.Droid.Utils;
using Mxp.Droid.Helpers;
using Android.Graphics;

namespace Mxp.Droid.Adapters
{
	public class PhotoPagerAdapter : PagerAdapter
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(PhotoPagerAdapter).Name;
		#pragma warning restore 0414

		private readonly Receipts mReceipts;

		public PhotoPagerAdapter (Receipts receipts) : base () {
			this.mReceipts = receipts;
		}

		public override int Count {
			get {
				return this.mReceipts.Count;
			}
		}

		public override Java.Lang.Object InstantiateItem (ViewGroup container, int position) {
			PhotoView photoView = new PhotoView (container.Context);
			if (this.mReceipts [position].AttachmentPath != null)
				Picasso.With (container.Context)
					.Load (this.mReceipts [position].AttachmentPath)
					.Into (photoView);
			else if (this.mReceipts [position].base64 != null) {
				TaskConfigurator.Create ()
								.Finally<Bitmap> (photoView.SetImageBitmap)
								.Start (BitmapHelper.DecodeBase64 (this.mReceipts [position].base64));
			}

			container.AddView (photoView, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

			return photoView;
		}


		public override void DestroyItem (ViewGroup container, int position, Java.Lang.Object @object) {
			container.RemoveView ((View) @object);
		}

		public override bool IsViewFromObject (View view, Java.Lang.Object @object) {
			return view == (View) @object;
		}
	}
}