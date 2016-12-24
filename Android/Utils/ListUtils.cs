using System;
using System.Linq;
using Android.Views;
using Android.Widget;

namespace Mxp.Droid.Utils
{
	public static class ListUtils
	{
		public static void SetListViewHeightBasedOnChildren (ListView listView) {
			if (listView.Adapter == null)
				return;

			int totalHeight = listView.PaddingTop + listView.PaddingBottom;
			int desiredWidth = Android.Views.View.MeasureSpec.MakeMeasureSpec (listView.Width, MeasureSpecMode.AtMost);
			for (int i = 0; i < listView.Count; i++) {
				View listItem = listView.Adapter.GetView (i, null, listView);
				if (listItem.GetType () == typeof(ViewGroup))
					listItem.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
//				listItem.Measure (0, 0);
				listItem.Measure (desiredWidth, 0);
				totalHeight += listItem.MeasuredHeight;
			}

			listView.LayoutParameters.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));
//			listView.RequestLayout ();
		}
	}
}