using System;
using Mxp.Core.Business;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.ObjectModel;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Adapters
{		
	public class ExpenseDetailsAdapter : BaseSectionAdapter<WrappedObject>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ExpenseDetailsAdapter).Name;
		#pragma warning restore 0414

		private ExpenseItem expenseItem;
		public Android.Support.V4.App.FragmentManager FragmentManager { get; private set; }

		public ExpenseDetailsAdapter (Android.Support.V4.App.FragmentManager fragmentManager, Activity activity, ExpenseItem expenseItem) : base (activity) {
			this.FragmentManager = fragmentManager;
			this.expenseItem = expenseItem;
		}

		public override BaseAdapter<WrappedObject> InstantiateSection (int position) {
			switch (position) {
			case 0:
					return new FieldsSectionAdapter<ExpenseDetailsAdapter> (this, this.mActivity, this.expenseItem.GetMainFields (), Labels.GetLoggedUserLabel (Labels.LabelEnum.General));
			case 1:
					return new FieldsSectionAdapter<ExpenseDetailsAdapter> (this, this.mActivity, this.expenseItem.GetAllFields (), Labels.GetLoggedUserLabel (Labels.LabelEnum.Details));
			default:
				return null;
			}
		}

		public override int SectionCount {
			get {
				return 2;
			}
		}

		public override void OnListItemClick (BaseAdapter<WrappedObject> section, int sectionPosition, ListView listView, View view, int position, long id) {
			((FieldsSectionAdapter<ExpenseDetailsAdapter>)section).OnListItemClick (listView, view, position, id);
		}

		public override Android.Support.V4.App.FragmentManager GetChildFragmentManager () {
			return this.FragmentManager;
		}
	}
}