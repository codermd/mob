using Android.Views;
using Android.Widget;
using Android.App;

using Mxp.Core.Business;
using Mxp.Droid.Fragments;
using Mxp.Droid.Helpers;
using Android.Content;
using Mxp.Core.Services.Google;
using Mxp.Droid.Fragments.Dialogs.GenericDialogs;
using System;

namespace Mxp.Droid
{
	public class StringFieldHolder : AbstractFieldHolder
	{
		public StringFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (!this.Field.IsEditable || this.Field.IsLoading)
				return;

			this.DialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
		}

		private int GetLayout (FieldTypeEnum fieldtype) {
			switch (fieldtype) {
				case FieldTypeEnum.LongString:
					return Resource.Layout.Dialog_string_multilines;
				case FieldTypeEnum.AutocompleteString:
					return Resource.Layout.Dialog_string_autocomplete;
				default:
					return Resource.Layout.Dialog_string;
			}
		}

		private void onClickHandler<T> (object sender, DialogArgsObject<T> e) {
			if (!e.Object.Equals (this.Field.GetValue<T> ()) && e.ButtonType == DialogButtonType.Positive) {
				this.Field.Value = e.Object;
				this.ParentAdapter.NotifyDataSetChanged ();
			}
		}

		private Android.Support.V4.App.DialogFragment DialogFragment {
			get {
				switch (Field.Type) {
					case FieldTypeEnum.AutocompleteString:
						return new AutocompleteStringDialogFragment (this.Field.GetValue<Prediction> (), this.GetLayout (Field.Type), Resource.Id.EditText, onClickHandler, this.Field.GetModel<ExpenseItem> ()?.Country) {
							mTitle = Labels.GetLoggedUserLabel (Labels.LabelEnum.Edit),
							mNegativeTextButton = Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel)
						};
					default:
						return new StringDialogFragment (this.Field.GetValue<String> (), this.GetLayout (Field.Type), Resource.Id.EditText, onClickHandler) {
							mTitle = Labels.GetLoggedUserLabel (Labels.LabelEnum.Edit),
							mNegativeTextButton = Labels.GetLoggedUserLabel (Labels.LabelEnum.Cancel),
							mPositiveTextButton = this.mActivity.GetString (Resource.String.ok)
						};
				}
			}
		}
	}
}