using System;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.App;
using Mxp.Droid.Adapters;
using Android.Text;
using Mxp.Core.Services;
using Mxp.Core.Business;
using Mxp.Core.Helpers;

namespace Mxp.Droid.Fragments
{
	public class LookupPickerDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(LookupPickerDialogFragment).Name;
		#pragma warning restore 0414

		private event EventHandler<EventArgsObject<LookupItem>> mOnClickHandler;
		private View mView;
		private LookupItems lookupItems = new LookupItems ();
		private LookupService.ApiEnum value;

		public LookupPickerDialogFragment (LookupService.ApiEnum value, EventHandler<EventArgsObject<LookupItem>> onClickHandler) {
			this.value = value;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_lookups, null);
			EditText filterText = mView.FindViewById<EditText> (Resource.Id.Search);
			ListView listView = mView.FindViewById<ListView> (Resource.Id.List);
			LookupDialogAdapter adapter = new LookupDialogAdapter (this.Activity, this.value, this.lookupItems);
			listView.Adapter = adapter;
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				this.mOnClickHandler (this, new EventArgsObject<LookupItem> (this.lookupItems [e.Position]));
			};

			filterText.TextChanged += (object sender, TextChangedEventArgs e) => {
				adapter.Filter.InvokeFilter (e.Text.ToString ());
			};
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
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