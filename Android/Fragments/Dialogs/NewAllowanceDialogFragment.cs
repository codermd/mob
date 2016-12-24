using System;

using Android.Support.V4.App;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Widget;
using Android.Util;
using Mxp.Core.Business;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;

namespace Mxp.Droid.Fragments
{
	public class NewAllowanceDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(NewAllowanceDialogFragment).Name;
		#pragma warning restore 0414

		private Allowance mAllowance;
		private View mView;
		private event EventHandler<EventArgsObject<Allowance>> mOnClickHandler;

		public NewAllowanceDialogFragment (Allowance allowance, EventHandler<EventArgsObject<Allowance>> onClickHandler) {
			this.mAllowance = allowance;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.Allowance_segment_details, null);
			ListView listView = this.mView.FindViewById<ListView> (Resource.Id.List);
			FieldsAdapter fieldsAdapter = new FieldsAdapter (this.ChildFragmentManager, this.Activity, this.mAllowance.CreationAllowanceFields);
			listView.Adapter = fieldsAdapter;
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				fieldsAdapter.OnListItemClick (listView, e.View, e.Position, e.Id);
			};
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState) {
			return new AlertDialog.Builder(this.Activity)
				.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.NewAllowance))
				.SetView (this.mView)
				.SetNegativeButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), (object sender, DialogClickEventArgs e) => {})
				.SetPositiveButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Done), (object sender, DialogClickEventArgs e) => {})
				.Create();
		}

		public override void OnStart () {
			base.OnStart ();

			AlertDialog dialog = (AlertDialog) this.Dialog;

			dialog.GetButton ((int) DialogButtonType.Positive).Click += (object sender, EventArgs e) => {
				this.mOnClickHandler (this, new EventArgsObject<Allowance> (this.mAllowance));
			};
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}