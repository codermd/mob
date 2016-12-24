using System;
using Android.OS;
using Android.App;
using Mxp.Core.Business;
using Android.Content;

namespace Mxp.Droid.Fragments
{
	public class BaseDialogFragment : Android.Support.V4.App.DialogFragment
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(BaseDialogFragment).Name;
		#pragma warning restore 0414

		public enum DialogTypeEnum {
			ExceptionDialog,
			ErrorDialog,
			ConfirmDialog,
			MessageDialog
		}

		public static readonly string EXTRA_DIALOG_REQUESTCODE = "com.sagacify.mxp.android.dialog.requestcode";

		public static readonly string EXTRA_DIALOG_TITLE = "com.sagacify.mxp.android.dialog.title";
		public static readonly string EXTRA_DIALOG_MESSAGE = "com.sagacify.mxp.android.dialog.message";
		public static readonly string EXTRA_DIALOG_NEGATIVE_TEXT_BUTTON = "com.sagacify.mxp.android.dialog.negative.text.button";
		public static readonly string EXTRA_DIALOG_NEUTRAL_TEXT_BUTTON = "com.sagacify.mxp.android.dialog.neutral.text.button";
		public static readonly string EXTRA_DIALOG_POSITIVE_TEXT_BUTTON = "com.sagacify.mxp.android.dialog.positive.text.button";

		private IDialogClickListener listener;

		public interface IDialogClickListener {
			void OnClickHandler<T> (int requestCode, DialogArgsObject<T> args);
		}

		public BaseDialogFragment () {

		}

		public static BaseDialogFragment NewInstance (Context context, DialogTypeEnum dialogType, string message = null, string title = null) {
			return BaseDialogFragment.NewInstance<BaseDialogFragment> (context, null, dialogType, message, title);
		}

		public static BaseDialogFragment NewInstance (Context context, int requestCode, DialogTypeEnum dialogType, string message = null, string title = null) {
			return BaseDialogFragment.NewInstance<BaseDialogFragment> (context, requestCode, dialogType, message, title);
		}

		public static F NewInstance<F> (Context context, int? requestCode, DialogTypeEnum dialogType, string message = null, string title = null) where F : BaseDialogFragment, new () {
			F fragment = new F ();

			Bundle bundle = new Bundle ();

			if (requestCode.HasValue)
				bundle.PutInt (EXTRA_DIALOG_REQUESTCODE, requestCode.Value);

			// TODO this.SetStyle (StyleNoTitle, this.Theme);

			switch (dialogType) {
				case DialogTypeEnum.ExceptionDialog:
				case DialogTypeEnum.ErrorDialog:
				case DialogTypeEnum.MessageDialog:
					bundle.PutString (EXTRA_DIALOG_TITLE, System.String.IsNullOrWhiteSpace (title) ? "Error" : title);
					bundle.PutString (EXTRA_DIALOG_MESSAGE, message);
					bundle.PutString (EXTRA_DIALOG_POSITIVE_TEXT_BUTTON, context.GetString (Resource.String.ok));
					break;
				case DialogTypeEnum.ConfirmDialog:
					bundle.PutString (EXTRA_DIALOG_TITLE, title);
					bundle.PutString (EXTRA_DIALOG_MESSAGE, message);
					bundle.PutString (EXTRA_DIALOG_NEGATIVE_TEXT_BUTTON, Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel));
					bundle.PutString (EXTRA_DIALOG_POSITIVE_TEXT_BUTTON, context.GetString (Resource.String.ok));
					break;
			}

			fragment.Arguments = bundle;

			return fragment;
		}

		public static BaseDialogFragment NewInstance (int requestCode, string title = null, string message = null, string negativeTextButton = null, string neutralTextButton = null, string positiveTextButton = null) {
			return BaseDialogFragment.NewInstance<BaseDialogFragment> (requestCode, title, message, negativeTextButton, neutralTextButton, positiveTextButton);
		}

		public static F NewInstance<F> (int requestCode, string title = null, string message = null, string negativeTextButton = null, string neutralTextButton = null, string positiveTextButton = null) where F : BaseDialogFragment, new () {
			F fragment = new F ();

			Bundle bundle = new Bundle ();
			bundle.PutInt (EXTRA_DIALOG_REQUESTCODE, requestCode);
			bundle.PutString (EXTRA_DIALOG_TITLE, title);
			bundle.PutString (EXTRA_DIALOG_MESSAGE, message);
			bundle.PutString (EXTRA_DIALOG_NEGATIVE_TEXT_BUTTON, negativeTextButton);
			bundle.PutString (EXTRA_DIALOG_NEUTRAL_TEXT_BUTTON, neutralTextButton);
			bundle.PutString (EXTRA_DIALOG_POSITIVE_TEXT_BUTTON, positiveTextButton);
			fragment.Arguments = bundle;

			return fragment;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			if (!this.Arguments.ContainsKey (EXTRA_DIALOG_REQUESTCODE))
				return;

			try {
				if (this.TargetFragment != null)
					listener = (IDialogClickListener) this.TargetFragment;
				else
					listener = (IDialogClickListener) this.Activity;
			} catch (InvalidCastException) {
				if (this.TargetFragment != null)
					throw new InvalidCastException ("Calling fragment " + this.TargetFragment.Class.SimpleName + " must implement IDialogClickListener interface");
				else
					throw new InvalidCastException ("Calling activity " + this.Activity.Class.SimpleName + " must implement IDialogClickListener interface");
			}
		}

		public virtual AlertDialog.Builder CreateDialog (Bundle savedInstanceState) {
			AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

			if (!System.String.IsNullOrWhiteSpace (this.Arguments.GetString (EXTRA_DIALOG_TITLE)))
				builder.SetTitle (this.Arguments.GetString (EXTRA_DIALOG_TITLE));

			if (!System.String.IsNullOrWhiteSpace (this.Arguments.GetString (EXTRA_DIALOG_MESSAGE)))
				builder.SetMessage (this.Arguments.GetString (EXTRA_DIALOG_MESSAGE));

			if (!System.String.IsNullOrWhiteSpace (this.Arguments.GetString (EXTRA_DIALOG_NEGATIVE_TEXT_BUTTON)))
				builder.SetNegativeButton (this.Arguments.GetString (EXTRA_DIALOG_NEGATIVE_TEXT_BUTTON), (object sender, DialogClickEventArgs e) => {
					if (listener != null)
						this.listener.OnClickHandler (this.Arguments.GetInt (EXTRA_DIALOG_REQUESTCODE), new DialogArgsObject<object> (this, DialogButtonType.Negative));
				});

			if (!System.String.IsNullOrWhiteSpace (this.Arguments.GetString (EXTRA_DIALOG_NEUTRAL_TEXT_BUTTON)))
				builder.SetNeutralButton (this.Arguments.GetString (EXTRA_DIALOG_NEUTRAL_TEXT_BUTTON), (object sender, DialogClickEventArgs e) => {
					if (listener != null)
						this.listener.OnClickHandler (this.Arguments.GetInt (EXTRA_DIALOG_REQUESTCODE), new DialogArgsObject<object> (this, DialogButtonType.Neutral));
				});

			if (!System.String.IsNullOrWhiteSpace (this.Arguments.GetString (EXTRA_DIALOG_POSITIVE_TEXT_BUTTON)))
				builder.SetPositiveButton (this.Arguments.GetString (EXTRA_DIALOG_POSITIVE_TEXT_BUTTON), (object sender, DialogClickEventArgs e) => {
					if (listener != null)
						this.listener.OnClickHandler (this.Arguments.GetInt (EXTRA_DIALOG_REQUESTCODE), new DialogArgsObject<object> (this, DialogButtonType.Positive));
				});

			return builder;
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState) {
			return this.CreateDialog (savedInstanceState).Create ();
		}
	}

	public class DialogArgsObject<T>
	{	
		public T Object { get; private set; }
		public DialogButtonType ButtonType { get; private set; }
		public object Sender { get; private set; }

		public DialogArgsObject (object sender, DialogButtonType buttonType) {
			this.Sender = sender;
			this.ButtonType = buttonType;
		}

		public DialogArgsObject (object sender, DialogButtonType buttonType, T obj) : this (sender, buttonType) {
			this.Object = obj;
		}
	}
}