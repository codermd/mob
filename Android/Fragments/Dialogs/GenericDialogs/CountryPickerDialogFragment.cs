using System;
using Mxp.Core.Business;
using Mxp.Core.Helpers;
using Android.Views;
using Mxp.Droid.Adapters;
using Android.OS;
using Android.Widget;
using Mxp.Droid.Utils;
using Android.App;

namespace Mxp.Droid.Fragments
{
	public class CountryPickerDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CountryPickerDialogFragment).Name;
		#pragma warning restore 0414

		private Country mCountry;
		private Countries mCountries;
		private event EventHandler<EventArgsObject<Country>> mOnClickHandler;
		public CountryDialogAdapter mAdapter { get; private set; }
		private View mView;

		public CountryPickerDialogFragment (Country country, Countries countries, EventHandler<EventArgsObject<Country>> onClickHandler) {
			this.mCountry = country;
			this.mCountries = countries;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_countries, null);
			EditText filterText = mView.FindViewById<EditText> (Resource.Id.Search);
			ListView listView = mView.FindViewById<ListView> (Resource.Id.List);
			this.mAdapter = new CountryDialogAdapter (this.Activity, this.mCountries);
			listView.Adapter = this.mAdapter;
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				Country country;
				if (this.mAdapter.mFilterIsInvoked) {
					country = this.mAdapter.GetSection (this.mAdapter.FilterOnSection).GetItem (e.Position).Cast<Country> ();
				} else {
					int sectionIndex;
					int position = this.mAdapter.GetIndexForSection (e.Position, out sectionIndex);
					country = this.mAdapter.GetSection (sectionIndex).GetItem (position).Cast<Country> ();
				}

				this.mOnClickHandler (this, new EventArgsObject<Country> (country));
			};

			filterText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				this.mAdapter.mFilterIsInvoked = !String.IsNullOrWhiteSpace (e.Text.ToString ());
				this.mAdapter.Filter.InvokeFilter (e.Text.ToString ());
			};
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

			builder.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.PickupCountry))
				.SetView (this.mView);

			return builder.Create();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}