using System;
using Mxp.Core.Business;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.ObjectModel;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Adapters
{		
	public class AllowanceDetailsAdapter : BaseSectionAdapter<WrappedObject>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(AllowanceDetailsAdapter).Name;
		#pragma warning restore 0414

		private Allowance mAllowance;
		public Android.Support.V4.App.FragmentManager FragmentManager { get; set; }

		public AllowanceDetailsAdapter (Android.Support.V4.App.FragmentManager fragmentManager, Activity activity, Allowance allowance) : base (activity) {
			this.FragmentManager = fragmentManager;
			this.mAllowance = allowance;
		}

		public override BaseAdapter<WrappedObject> InstantiateSection (int position) {
			switch (position) {
				case 0:
					return new AllowanceSegmentsAdapter<AllowanceDetailsAdapter> (this, this.mActivity, this.mAllowance.AllowanceSegments, Labels.GetLoggedUserLabel (Labels.LabelEnum.Total) + " : " + this.mAllowance.VAmountLC);
				case 1:
					return new FieldsSectionAdapter<AllowanceDetailsAdapter> (this, this.mActivity, this.mAllowance.ExpenseItems[0].GetAllFields (), Labels.GetLoggedUserLabel (Labels.LabelEnum.Details));
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
			switch (sectionPosition) {
			case 0:
				((AllowanceSegmentsAdapter<AllowanceDetailsAdapter>)section).OnListItemClick (listView, view, position, id);
				break;
			case 1:
				((FieldsSectionAdapter<AllowanceDetailsAdapter>)section).OnListItemClick (listView, view, position, id);
				break;
			default:
				break;
			}
		}

		public override Android.Support.V4.App.FragmentManager GetChildFragmentManager () {
			return this.FragmentManager;
		}
	}
}