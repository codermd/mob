using System;

using Android.OS;
using Android.App;
using Android.Text;
using Android.Views;
using Android.Widget;

using Mxp.Core.Business;
using Mxp.Droid.Adapters;
using Mxp.Core.Helpers;

namespace Mxp.Droid.Fragments
{
	public class CurrencyPickerDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CurrencyPickerDialogFragment).Name;
		#pragma warning restore 0414

		private Currency mCurrency;
		private View mView;
		private event EventHandler<EventArgsObject<Currency>> mOnClickHandler;
		public CurrencyDialogAdapter mAdapter { get; private set; }

		public CurrencyPickerDialogFragment (Currency currency, EventHandler<EventArgsObject<Currency>> onClickHandler) {
			this.mCurrency = currency;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_currencies, null);

			EditText filterText = mView.FindViewById<EditText> (Resource.Id.Search);
			ListView listView = mView.FindViewById<ListView> (Resource.Id.List);
			this.mAdapter = new CurrencyDialogAdapter (this.Activity);
			listView.Adapter = this.mAdapter;
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				this.mOnClickHandler (this, new EventArgsObject<Currency> (this.mAdapter [e.Position]));
			};

			filterText.TextChanged += (object sender, TextChangedEventArgs e) => {
				this.mAdapter.mFilterIsInvoked = !String.IsNullOrWhiteSpace (e.Text.ToString ());
				this.mAdapter.Filter.InvokeFilter (e.Text.ToString ());
			};
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

			builder.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Select))
				.SetView (this.mView);

			return builder.Create();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}