using System;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.App;
using Mxp.Droid.Adapters;
using Android.Text;
using Mxp.Core.Business;
using Mxp.Core.Helpers;
using Android.Content;

namespace Mxp.Droid.Fragments
{
	public class LookupFieldPickerDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(LookupFieldPickerDialogFragment).Name;
		#pragma warning restore 0414

		private readonly LookupField mLookupField;
		private event EventHandler<EventArgsObject<LookupItem>> mOnClickHandler;
		public LookupFieldDialogAdapter mAdapter { get; private set; }
		private View mView;

		public LookupFieldPickerDialogFragment (LookupField lookupField, EventHandler<EventArgsObject<LookupItem>> onClickHandler) {
			this.mLookupField = lookupField;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_lookups, null);

			EditText filterText = mView.FindViewById<EditText> (Resource.Id.Search);

			ListView listView = mView.FindViewById<ListView> (Resource.Id.List);
			this.mAdapter = new LookupFieldDialogAdapter (this.Activity, this.mLookupField);
			listView.Adapter = this.mAdapter;
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => this.mOnClickHandler (this, new EventArgsObject<LookupItem> (this.mAdapter [e.Position]));

			filterText.TextChanged += (object sender, TextChangedEventArgs e) => this.mAdapter.Filter.InvokeFilter (e.Text.ToString ());

			this.mAdapter.Filter.InvokeFilter (String.Empty);
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

			builder.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Select))
				.SetView (this.mView);

			return builder.Create();
		}

		public override void OnDismiss (IDialogInterface dialog) {
			this.mLookupField.ResetResults ();

			base.OnDismiss (dialog);
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}