using System;
using Mxp.Core.Business;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.ObjectModel;
using Mxp.Droid.Helpers;
using Mxp.Core.Helpers;
using System.Collections.Generic;

namespace Mxp.Droid.Adapters
{		
	public class SettingsIconsLegendAdapter : BaseSectionAdapter<IconLegend>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(SettingsIconsLegendAdapter).Name;
		#pragma warning restore 0414

		private List<IconsLegend> iconsLegendList {
			get {
				return IconsLegend.All;
			}
		}

		public SettingsIconsLegendAdapter (Activity activity) : base (activity) {

		}

		public override BaseAdapter<IconLegend> InstantiateSection (int position) {
			return new IconsLegendAdapter (this, this.mActivity, this.iconsLegendList [position].Title, this.iconsLegendList [position].IconsLegendList);
		}

		public override int SectionCount {
			get {
				return this.iconsLegendList.Count;
			}
		}
	}
}