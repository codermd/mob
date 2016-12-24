using System;
using Android.Support.V7.Widget;
using Android.Content;
using Android.Util;
using Android.Views;

namespace Mxp.Droid.Helpers
{
	public class SplitToolbar : Toolbar
	{
		public SplitToolbar (Context context) : base (context) {

		}

		public SplitToolbar (Context context, IAttributeSet attrs) : base (context, attrs) {
		}

		public SplitToolbar(Context context, IAttributeSet attrs, int defStyleAttr) : base (context, attrs, defStyleAttr) {

		}
		public override void AddView (View child, int index, ViewGroup.LayoutParams @params) {
//			if (child is ActionMenuView) {
				@params.Width = LayoutParams.MatchParent;
//			}

			base.AddView (child, index, @params);
		}
	}
}