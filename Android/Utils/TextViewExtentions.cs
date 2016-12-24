using Android.Graphics;
using Android.Widget;
namespace Mxp.Droid
{
	public static class TextViewExtentions {
		public static void SetTextColor (this TextView textView, int color) {
			textView.SetTextColor (new Color (Color.GetRedComponent (color), Color.GetGreenComponent (color), Color.GetBlueComponent (color)));
		}
	}
}