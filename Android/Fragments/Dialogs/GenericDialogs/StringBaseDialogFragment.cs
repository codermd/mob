using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;

namespace Mxp.Droid.Fragments.Dialogs.GenericDialogs
{
	public abstract class StringBaseDialogFragment<T> : Android.Support.V4.App.DialogFragment
	{
		protected T mValue;
		protected int mViewResourceId;
		protected int mTextResourceId;

		public string mTitle;
		public string mMessage;
		public string mNegativeTextButton;
		public string mNeutralTextButton;
		public string mPositiveTextButton;

		protected View mView;

		private event EventHandler<DialogArgsObject<T>> mOnClickHandler;

		public StringBaseDialogFragment (T value, int viewResourceId, int textResourceId, EventHandler<DialogArgsObject<T>> onClickHandler) {
			this.mValue = value;

			this.mViewResourceId = viewResourceId;
			this.mTextResourceId = textResourceId;

			this.mOnClickHandler = onClickHandler;
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder (this.Activity);

			if (this.mView != null)
				builder.SetView (this.mView);

			if (this.mTitle != null)
				builder.SetTitle (this.mTitle);

			if (this.mMessage != null)
				builder.SetMessage (this.mMessage);

			if (this.mNegativeTextButton != null)
				builder.SetNegativeButton (this.mNegativeTextButton, (object sender, DialogClickEventArgs e) => {
					this.mOnClickHandler (this, new DialogArgsObject<T> (this, DialogButtonType.Negative, this.Value));
				});

			if (this.mNeutralTextButton != null)
				builder.SetNeutralButton (this.mNeutralTextButton, (object sender, DialogClickEventArgs e) => {
					this.mOnClickHandler (this, new DialogArgsObject<T> (this, DialogButtonType.Neutral, this.Value));
				});

			if (this.mPositiveTextButton != null)
				builder.SetPositiveButton (this.mPositiveTextButton, (object sender, DialogClickEventArgs e) => {
					this.mOnClickHandler (this, new DialogArgsObject<T> (this, DialogButtonType.Positive, this.Value));
				});

			return builder.Create ();
		}

		public override void OnPause () {
			this.Dismiss ();

			base.OnPause ();
		}

		public override void OnStart () {
			base.OnStart ();

			this.Dialog.Window.SetSoftInputMode (SoftInput.StateAlwaysVisible);
		}

		protected void DismissOnCompletion () {
			this.mOnClickHandler (this, new DialogArgsObject<T> (this, DialogButtonType.Positive, this.Value));
			this.Dismiss ();
		}

		protected abstract T Value { get; }
	}
}