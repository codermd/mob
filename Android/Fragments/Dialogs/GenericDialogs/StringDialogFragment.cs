using System;
using Android.OS;
using Android.Widget;
using Mxp.Droid.Fragments.Dialogs.GenericDialogs;

namespace Mxp.Droid.Fragments
{
	public class StringDialogFragment : StringBaseDialogFragment<String>
	{
		protected EditText mEditText;

		public StringDialogFragment (string value, int viewResourceId, int textResourceId, EventHandler<DialogArgsObject<String>> onClickHandler) : base (value, viewResourceId, textResourceId, onClickHandler) {

		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (this.mViewResourceId, null);

			this.mEditText = this.mView.FindViewById<EditText> (this.mTextResourceId);
			this.mEditText.Text = this.mValue;
		}

		protected override string Value => this.mEditText.Text;
	}
}