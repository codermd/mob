using System;
using Android.Support.V4.View;
using Android.Content;
using Android.Util;
using Android.Views;

namespace Mxp.Droid.Views
{
	public class NonSwipeableViewPager : ViewPager {

		public NonSwipeableViewPager (Context context) : base (context) {
			
		}

		public NonSwipeableViewPager(Context context, IAttributeSet attrs) : base (context, attrs) {
			
		}

		public override bool OnInterceptTouchEvent (MotionEvent ev) {
			return false;
		}

		public override bool OnTouchEvent (MotionEvent ev) {
			return false;
		}
	}
}