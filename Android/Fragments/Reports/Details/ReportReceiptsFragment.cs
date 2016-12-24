using System;

using Android.OS;
using Android.Util;
using Android.Views;
using Android.Support.V4.App;
using Android.Content;
using Android.App;
using Android.Provider;
using Android.Widget;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using UK.CO.Senab.Photoview;
using Android.Database;
using Android.Graphics;
using Java.IO;
using System.IO;
using Android.Graphics.Drawables;
using Java.Text;
using Java.Util;

using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Mxp.Droid.Utils;
using Android.Support.V4.Widget;
using System.Linq;

namespace Mxp.Droid.Fragments
{
	public class ReportReceiptsFragment : ExpenseReceiptsFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ReportReceiptsFragment).Name;
		#pragma warning restore 0414

		private Report mReport {
			get {
				return ((IReportListener)this.Activity).GetReport ();
			}
		}

		protected override Receipts mReceipts {
			get {
				return this.mReport.Receipts;
			}
		}

		protected override bool CanShowActions {
			get {
				return this.mReport.CanManageReceipts;
			}
		}

		public new static ReportReceiptsFragment NewInstance () {
			ReportReceiptsFragment reportReceiptsFragment = new ReportReceiptsFragment ();

			return reportReceiptsFragment;
		}

		protected override void StartPhotoViewPagerActiviy (Receipt receipt) {
			Intent intent = new Intent (this.Activity, typeof(PhotoViewPagerActivity));

			if (this.mReport.Id.HasValue)
				intent.PutExtra (PhotoViewPagerActivity.EXTRA_REPORT_ID, this.mReport.Id.Value);
			
			intent.PutExtra (PhotoViewPagerActivity.EXTRA_REPORT_TYPE, (int) this.mReport.ReportType);

			if (receipt.base64 != null)
				intent.PutExtra (PhotoViewPagerActivity.EXTRA_RECEIPT_POSITION, receipt.GetCollectionParent<Receipts, Receipt> ().IndexOf (receipt));
			else
				intent.PutExtra (PhotoViewPagerActivity.EXTRA_RECEIPT_ID, receipt.AttachmentId);

			this.StartActivity(intent);
		}
	}
}