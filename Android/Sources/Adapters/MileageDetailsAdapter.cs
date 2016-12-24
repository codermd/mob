using System;
using Mxp.Core.Business;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.ObjectModel;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Adapters
{		
	public class MileageDetailsAdapter : BaseSectionAdapter<WrappedObject>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(MileageDetailsAdapter).Name;
		#pragma warning restore 0414

		private Mileage mMileage;
		public Android.Support.V4.App.FragmentManager FragmentManager { get; set; }

		public MileageDetailsAdapter (Android.Support.V4.App.FragmentManager fragmentManager, Activity activity, Mileage mileage) : base (activity) {
			this.FragmentManager = fragmentManager;
			this.mMileage = mileage;
		}

		public override BaseAdapter<WrappedObject> InstantiateSection (int position) {
			switch (position) {
				case 0:
					return new MileageSegmentsAdapter<MileageDetailsAdapter> (this, this.mActivity, this.mMileage.MileageSegments, Labels.GetLoggedUserLabel (Labels.LabelEnum.Segment));
				case 1:
					return new FieldsSectionAdapter<MileageDetailsAdapter> (this, this.mActivity, this.mMileage.ExpenseItems[0].GetMainFields (), Labels.GetLoggedUserLabel (Labels.LabelEnum.General));
				case 2:
					return new FieldsSectionAdapter<MileageDetailsAdapter> (this, this.mActivity, this.mMileage.ExpenseItems[0].GetAllFields (), Labels.GetLoggedUserLabel (Labels.LabelEnum.Details));
				default:
					return null;
			}
		}

		public override int SectionCount {
			get {
				return 3;
			}
		}

		public override void OnListItemClick (BaseAdapter<WrappedObject> section, int sectionPosition, ListView listView, View view, int position, long id) {
			switch (sectionPosition) {
			case 0:
				((MileageSegmentsAdapter<MileageDetailsAdapter>)section).OnListItemClick (listView, view, position, id);
				break;
			case 1:
			case 2:
				((FieldsSectionAdapter<MileageDetailsAdapter>)section).OnListItemClick (listView, view, position, id);
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