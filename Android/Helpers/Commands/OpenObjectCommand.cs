using System;
using System.Threading.Tasks;
using Mxp.Core.Business;
using Android.Content;
using Android.App;
using Android.Widget;

namespace Mxp.Droid.Helpers
{
	public class OpenObjectCommand : OpenObjectAbstractCommand
	{
		private Activity mActivity;

		public OpenObjectCommand (Activity activity, Uri uri) : base (uri) {
			this.mActivity = activity;
		}

		public OpenObjectCommand (Activity activity, int objectType, string referenceType, string reference) {
			this.mActivity = activity;
			this.openObject = new OpenObject (objectType, referenceType, reference);
		}

		#region SAMLAbstractCommand

		protected override void RedirectToListView (ValidationError error) {
			Intent intent = new Intent (this.mActivity, typeof (MainActivity));

			switch (this.MetaOpenObject?.Location) {
				case MetaOpenObject.LocationEnum.PendingExpenses: {
						intent.PutExtra (MainActivity.EXTRA_SELECTED_TAB, 0);
						break;
					}
				case MetaOpenObject.LocationEnum.DraftReports: {
						intent.PutExtra (MainActivity.EXTRA_SELECTED_TAB, 1);
						intent.PutExtra (MainActivity.EXTRA_SELECTED_CATEGORY, (int)Reports.ReportTypeEnum.Draft);
						break;
					}
				case MetaOpenObject.LocationEnum.OpenReports: {
						intent.PutExtra (MainActivity.EXTRA_SELECTED_TAB, 1);
						intent.PutExtra (MainActivity.EXTRA_SELECTED_CATEGORY, (int)Reports.ReportTypeEnum.Open);
						break;
					}
				case MetaOpenObject.LocationEnum.ClosedReports: {
					intent.PutExtra (MainActivity.EXTRA_SELECTED_TAB, 1);
					intent.PutExtra (MainActivity.EXTRA_SELECTED_CATEGORY, (int)Reports.ReportTypeEnum.Closed);
					break;
				}
				case MetaOpenObject.LocationEnum.ApprovalReports: {
						intent.PutExtra (MainActivity.EXTRA_SELECTED_TAB, 2);
						intent.PutExtra (MainActivity.EXTRA_SELECTED_CATEGORY, 0);
						break;
					}
				case MetaOpenObject.LocationEnum.ApprovalTravelRequests: {
						intent.PutExtra (MainActivity.EXTRA_SELECTED_TAB, 2);
						intent.PutExtra (MainActivity.EXTRA_SELECTED_CATEGORY, 1);
						break;
					}
			}

			intent.PutExtra (MainActivity.EXTRA_MESSAGE, error.Verbose);

			intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);

			this.mActivity.StartActivity (intent);
			this.mActivity.Finish ();
		}

		protected override void RedirectToDetailsView () {
			Intent intent = null;

			switch (this.MetaOpenObject.Location) {
				case MetaOpenObject.LocationEnum.PendingExpenses: {
						intent = new Intent (this.mActivity, typeof(ExpenseDetailsActivity));
						intent.PutExtra (ExpenseDetailsActivity.EXTRA_EXPENSE_ITEM_ID, this.MetaOpenObject.Id);
						break;
					}
				case MetaOpenObject.LocationEnum.DraftReports: {
						if (this.MetaOpenObject.HasFatherId) {
							intent = new Intent (this.mActivity, typeof(ExpenseDetailsActivity));
							intent.PutExtra (ExpenseDetailsActivity.EXTRA_EXPENSE_ITEM_ID, this.MetaOpenObject.Id);
							intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_ID, this.MetaOpenObject.FatherId);
							intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_TYPE, (int)Reports.ReportTypeEnum.Draft);
						} else {
							intent = new Intent (this.mActivity, typeof(ReportDetailsActivity));
							intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_ID, this.MetaOpenObject.Id);
							intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_TYPE, (int)Reports.ReportTypeEnum.Draft);
						}
						break;
					}
				case MetaOpenObject.LocationEnum.OpenReports: {
						if (this.MetaOpenObject.HasFatherId) {
							intent = new Intent (this.mActivity, typeof(ExpenseDetailsActivity));
							intent.PutExtra (ExpenseDetailsActivity.EXTRA_EXPENSE_ITEM_ID, this.MetaOpenObject.Id);
							intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_ID, this.MetaOpenObject.FatherId);
							intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_TYPE, (int)Reports.ReportTypeEnum.Open);
						} else {
							intent = new Intent (this.mActivity, typeof(ReportDetailsActivity));
							intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_ID, this.MetaOpenObject.Id);
							intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_TYPE, (int)Reports.ReportTypeEnum.Open);
						}
						break;
					}
				case MetaOpenObject.LocationEnum.ClosedReports: {
					if (this.MetaOpenObject.HasFatherId) {
						intent = new Intent (this.mActivity, typeof (ExpenseDetailsActivity));
						intent.PutExtra (ExpenseDetailsActivity.EXTRA_EXPENSE_ITEM_ID, this.MetaOpenObject.Id);
						intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_ID, this.MetaOpenObject.FatherId);
						intent.PutExtra (ExpenseDetailsActivity.EXTRA_REPORT_TYPE, (int)Reports.ReportTypeEnum.Closed);
					} else {
						intent = new Intent (this.mActivity, typeof (ReportDetailsActivity));
						intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_ID, this.MetaOpenObject.Id);
						intent.PutExtra (ReportDetailsActivity.EXTRA_REPORT_TYPE, (int)Reports.ReportTypeEnum.Closed);
					}
					break;
				}
				case MetaOpenObject.LocationEnum.ApprovalReports:
					break;
				case MetaOpenObject.LocationEnum.ApprovalTravelRequests:
					break;
			}
					
			intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);

			this.mActivity.StartActivity (intent);
			this.mActivity.Finish ();
		}

		public override void RedirectToLoginView (ValidationError error = null) {
			Intent intent = new Intent (this.mActivity, typeof(LoginActivity));
			intent.PutExtra (LoginActivity.EXTRA_OPEN_OBJECT_TYPE, (int)this.openObject.ObjectType);
			intent.PutExtra (LoginActivity.EXTRA_OPEN_REFERENCE_TYPE, this.openObject.ReferenceType);
			intent.PutExtra (LoginActivity.EXTRA_OPEN_REFERENCE, this.openObject.Reference);

			if (error != null)
				intent.PutExtra (LoginActivity.EXTRA_MESSAGE, error.Verbose);

			this.mActivity.StartActivity (intent);
		}

		#endregion
	}
}