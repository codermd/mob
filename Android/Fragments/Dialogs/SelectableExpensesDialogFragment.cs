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
	public class SelectableExpensesDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(SelectableExpensesDialogFragment).Name;
		#pragma warning restore 0414

		private Report mReport;
		private View mView;
		private event EventHandler<EventArgsObject<Report>> mOnClickHandler;

		public SelectableExpensesDialogFragment (Report report, EventHandler<EventArgsObject<Report>> onClickHandler) {
			this.mReport = report;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.Cancelable = false;

			this.mView = this.Activity.LayoutInflater.Inflate(Resource.Layout.List_expenses_sticky_headers, null);
			ListView listView = this.mView.FindViewById<ListView> (Android.Resource.Id.List);
			ExpenseAdapter expenseAdapter = new ExpenseAdapter (this.Activity, LoggedUser.Instance.BusinessExpenses, this.mReport);
			listView.Adapter = expenseAdapter;
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState) {
			return new AlertDialog.Builder (this.Activity)
				.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Expenses))
				.SetView (this.mView)
				.SetPositiveButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Done), (object sender, DialogClickEventArgs e) => {
					this.mOnClickHandler (this, new EventArgsObject<Report> (this.mReport));
				})
				.Create();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}