using System;
using Android.OS;
using Android.Widget;
using Mxp.Core.Services;
using Mxp.Core.Services.Google;
using Mxp.Droid.Adapters;
using Android.Views;
using Mxp.Core.Business;

namespace Mxp.Droid.Fragments.Dialogs.GenericDialogs
{
	public class AutocompleteStringDialogFragment : StringBaseDialogFragment<Prediction>
	{
		protected AutoCompleteTextView mAutoCompleteTextView;
		private Country mCountry;

		public AutocompleteStringDialogFragment (Prediction value, int viewResourceId, int textResourceId, EventHandler<DialogArgsObject<Prediction>> onClickHandler, Country country = null) : base (value, viewResourceId, textResourceId, onClickHandler) {
			this.mCountry = country;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mView = this.Activity.LayoutInflater.Inflate (this.mViewResourceId, null);

			this.mAutoCompleteTextView = this.mView.FindViewById<AutoCompleteTextView> (this.mTextResourceId);
			this.mAutoCompleteTextView.Adapter = new LocationsAdapter (this.Activity, GoogleService.PlaceTypeEnum.Merchant, this.mCountry);
			this.mAutoCompleteTextView.ItemClick += this.AutoCompleteTextViewItemClickHandler;

			this.Refresh ();
		}

		private void Refresh () {
			this.mAutoCompleteTextView.Text = this.mValue?.description;
		}

		private void AutoCompleteTextViewItemClickHandler (object sender, AdapterView.ItemClickEventArgs e) {
			this.mValue = ((LocationsAdapter)this.mAutoCompleteTextView.Adapter) [e.Position].GetInstance<Prediction> ();

			this.DismissOnCompletion ();
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			this.Dialog.Window.SetSoftInputMode (SoftInput.AdjustResize);
			this.Dialog.Window.SetGravity (GravityFlags.CenterHorizontal | GravityFlags.Top);

			return base.OnCreateView (inflater, container, savedInstanceState);
		}

		protected override Prediction Value => this.mValue;
	}
}