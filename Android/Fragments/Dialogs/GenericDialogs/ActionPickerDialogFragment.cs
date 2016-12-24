using System;
using Mxp.Droid.Helpers;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.App;
using Mxp.Core.Business;
using Mxp.Droid.Adapters;
using Android.Text;
using Android.Content;
using System.Collections.Generic;
using Mxp.Core.Utils;
using System.Linq;

namespace Mxp.Droid.Fragments
{
	public class ActionPickerDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ActionPickerDialogFragment).Name;
		#pragma warning restore 0414

		private IList<Actionable> mActions;

		private View mView;

		public ActionPickerDialogFragment (IList<Actionable> actions) {
			this.mActions = actions;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.SetStyle (StyleNoTitle, this.Theme);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.List_actions, null);
			ListView listView = mView.FindViewById<ListView> (Android.Resource.Id.List);
			mView.FindViewById (Android.Resource.Id.List);
			listView.Adapter = new ArrayAdapter<string> (this.Activity, Resource.Layout.List_actions_item, this.mActions.Select (actionable => actionable.Title).ToList ());
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				this.Dismiss ();
				mActions [e.Position].Action ();
			};
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState) {
			return new AlertDialog.Builder (this.Activity)
				.SetView (this.mView)
				.Create ();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}