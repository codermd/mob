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
	public class ApprovalCommentDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ApprovalCommentDialogFragment).Name;
		#pragma warning restore 0414

		private Report mReport;
		private View mView;
		private EditText mEditText;
		private event EventHandler<EventArgsObject<String>> mOnClickHandler;

		public ApprovalCommentDialogFragment (Report report, EventHandler<EventArgsObject<String>> onClickHandler) {
			this.mReport = report;
			this.mOnClickHandler = onClickHandler;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (Resource.Layout.Dialog_string_multilines, null);

			this.mEditText = mView.FindViewById<EditText> (Resource.Id.EditText);
			this.mEditText.Text = this.mReport.Approval.Comment;
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState) {
			return new AlertDialog.Builder(this.Activity)
				.SetTitle (Labels.GetLoggedUserLabel (Labels.LabelEnum.Approve))
				.SetMessage (String.Format ("{0} :\n\n{1} {2}\n{3} {4}",
					Labels.GetLoggedUserLabel (Labels.LabelEnum.Expenses),
					this.mReport.Approval.VNumberOfAccepted,
					Labels.GetLoggedUserLabel (Labels.LabelEnum.Accepted),
					this.mReport.Approval.VNumberOfRejected,
					Labels.GetLoggedUserLabel (Labels.LabelEnum.Rejected)))
				.SetView (this.mView)
				.SetNegativeButton (Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel), (object sender, DialogClickEventArgs e) => {})
				.SetPositiveButton (this.GetString (Resource.String.ok), (object sender, DialogClickEventArgs e) => {})
				.Create();
		}

		public override void OnStart () {
			base.OnStart ();

			AlertDialog dialog = (AlertDialog) this.Dialog;

			dialog.GetButton ((int) DialogButtonType.Positive).Click += (object sender, EventArgs e) => {
				this.mOnClickHandler (this, new EventArgsObject<String> (this.mEditText.Text));
			};
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}
	}
}