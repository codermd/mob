using System;
using System.Linq;

using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;

using com.refractored.components.stickylistheaders;

using Mxp.Core.Business;
using Mxp.Droid.Utils;
using Mxp.Droid.Helpers;
using Mxp.Droid.Filters;

namespace Mxp.Droid.Adapters
{
	public class CurrencyDialogAdapter : BaseSectionAdapter<Currency>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(CurrencyDialogAdapter).Name;
		#pragma warning restore 0414

		public CurrencyDialogAdapter (Activity activity) : base (activity) {

		}

		public override BaseAdapter<Currency> InstantiateSection (int position) {
			switch (position) {
			case 0:
				return new CurrencyMainSectionSource (this, this.mActivity);
			case 1:
				return new CurrencyListSectionSource (this, this.mActivity);
			default:
				return null;
			}
		}

		public override int SectionCount {
			get {
				return 2;
			}
		}

		public override int FilterOnSection {
			get {
				return 1;
			}
		}
	}
}