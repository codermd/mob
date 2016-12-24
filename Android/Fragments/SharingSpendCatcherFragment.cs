using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using Mxp.Droid.Helpers;
using Android.Graphics;

namespace Mxp.Droid
{
	public class SharingSpendCatcherFragment : Fragment
	{
		private const string EXTRA_POSITION = "com.sagacify.mxp.android.position";

		private ImageView mImageView;

		private SpendCatcherExpense mSpendCatcherExpense {
			get {
				return SpendCatcherSharingActivity.sSpendCatcherExpenses.Count > this.Arguments.GetInt (EXTRA_POSITION) ? SpendCatcherSharingActivity.sSpendCatcherExpenses [this.Arguments.GetInt (EXTRA_POSITION)] : null;
			}
		}

		public static SharingSpendCatcherFragment NewInstance (int position) {
			SharingSpendCatcherFragment sharingSpendCatcherFragment = new SharingSpendCatcherFragment ();

			Bundle extras = new Bundle ();
			extras.PutInt (EXTRA_POSITION, position);
			sharingSpendCatcherFragment.Arguments = extras;

			return sharingSpendCatcherFragment;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.spendcatcher_fragment, container, false);

			if (this.mSpendCatcherExpense != null) {
				ListView list = view.FindViewById<ListView> (Resource.Id.list);
				FieldsAdapter adapter = new FieldsAdapter (this.FragmentManager, this.Activity, this.mSpendCatcherExpense.Fields);
				list.Adapter = new FieldsAdapter (this.FragmentManager, this.Activity, this.mSpendCatcherExpense.Fields);
				list.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => adapter.OnListItemClick (list, e.View, e.Position, e.Id);
				this.mImageView = view.FindViewById<ImageView> (Resource.Id.image);

				TaskConfigurator.Create ()
				                .Finally<Bitmap> (this.mImageView.SetImageBitmap)
								.Start (BitmapHelper.DecodeBase64 (this.mSpendCatcherExpense.Base64Image));
			}

			return view;
		}
	}
}