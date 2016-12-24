using System;

using Android.OS;
using Android.App;
using Android.Support.V4.View;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using System.Linq;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using Android.Views;
using Android.Support.V7.App;
using UK.CO.Senab.Photoview;
using Squareup.Picasso;

namespace Mxp.Droid
{
	public class SpendCatcherPhotoViewActivity : BaseActivity
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(SpendCatcherPhotoViewActivity).Name;
		#pragma warning restore 0414

		public static readonly string EXTRA_EXPENSE_ID = "com.sagacify.mxp.expense.id";

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.SetContentView (Resource.Layout.SpendCatcher_photo_view);

			Android.Support.V7.Widget.Toolbar toolbar = this.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Receipt);

			PhotoView photoView = this.FindViewById<PhotoView> (Resource.Id.PhotoView);

			int expenseId = this.Intent.GetIntExtra (EXTRA_EXPENSE_ID, -1);

			Picasso.With (this.BaseContext)
				.Load (LoggedUser.Instance.SpendCatcherExpenses.Single (expense => expense.Id == expenseId).AttachmentPath)
				.Into (photoView);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					this.Finish ();
					return true;
			}

			return base.OnOptionsItemSelected (item);
		}
	}
}