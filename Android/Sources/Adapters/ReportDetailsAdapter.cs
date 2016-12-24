using System;
using Mxp.Core.Business;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.ObjectModel;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Adapters
{		
	public class ReportDetailsAdapter : BaseSectionAdapter<WrappedObject>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(ReportDetailsAdapter).Name;
		#pragma warning restore 0414

		private Report mReport;
		public Android.Support.V4.App.FragmentManager FragmentManager { get; set; }

		public ReportDetailsAdapter (Android.Support.V4.App.FragmentManager fragmentManager, Activity activity, Report report) : base (activity) {
			this.FragmentManager = fragmentManager;
			this.mReport = report;
		}

		public override BaseAdapter<WrappedObject> InstantiateSection (int position) {
			switch (position) {
			case 0:
					return new FieldsSectionAdapter<ReportDetailsAdapter> (this, this.mActivity, this.mReport.GetMainFields (), Labels.GetLoggedUserLabel (Labels.LabelEnum.General));
			case 1:
					return new FieldsSectionAdapter<ReportDetailsAdapter> (this, this.mActivity, this.mReport.GetAllFields (), Labels.GetLoggedUserLabel (Labels.LabelEnum.Details));
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
			((FieldsSectionAdapter<ReportDetailsAdapter>)section).OnListItemClick (listView, view, position, id);
		}

		public override Android.Support.V4.App.FragmentManager GetChildFragmentManager () {
			return this.FragmentManager;
		}
	}
}