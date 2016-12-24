using System;

using Android.Views;
using Android.Widget;
using Android.App;

using Mxp.Core.Business;
using Mxp.Droid.Fragments;
using Mxp.Core.Helpers;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Mxp.Core.Utils;

namespace Mxp.Droid
{
	public class ComboFieldHolder : AbstractFieldHolder
	{
		public ComboFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}
			
		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			if (!this.Field.IsEditable)
				return;

			Collection<ComboField.IComboChoice> comboChoices = ((ComboField)this.Field).Choices;
			string[] values = new string[comboChoices.Count];
			comboChoices.ForEach ((value, index) => values[index] = value.VTitle);
			Android.Support.V4.App.DialogFragment dialogFragment = new StringPickerDialogFragment (values, Labels.GetLoggedUserLabel (Labels.LabelEnum.Select), (object sender, EventArgsObject<string> e) => {
				ComboField.IComboChoice selected = comboChoices.Single (entity => entity.VTitle == e.Object);
				ComboField.IComboChoice combo = this.Field.GetValue<ComboField.IComboChoice> ();
				if (combo == null || selected.ComboId != combo.ComboId) {
					this.Field.Value = selected;
					this.ParentAdapter.NotifyDataSetChanged ();
				}
			});
			dialogFragment.Show (((IChildFragmentManager)this.ParentAdapter).GetChildFragmentManager (), null);
		}
	}
}