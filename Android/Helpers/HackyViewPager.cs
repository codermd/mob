using System;
using Android.Support.V4.View;
using Android.Content;
using Android.Util;
using Java.Lang;
using Android.Views;

namespace Mxp.Droid.Helpers
{
	public class HackyViewPager : ViewPager
	{
		public HackyViewPager (Context context) :
			base (context) {
			
		}

		public HackyViewPager (Context context, IAttributeSet attrs) :
			base (context, attrs) {
			
		}

		public override bool OnInterceptTouchEvent (MotionEvent ev) {
			try {
				return base.OnInterceptTouchEvent (ev);
			} catch (IllegalArgumentException) {
				return false;
			}
		}
	}
}