using Android.Views;
using Android.Widget;
using Android.App;
using Mxp.Droid.Helpers;

namespace Mxp.Droid
{
	public class FullTextFieldHolder : AbstractFieldHolder
	{
		public FullTextFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) : base (parentAdapter, activity) {

		}

		protected override int LayoutResourceId {
			get {
				return Resource.Layout.List_fields_fulltext_item;
			}
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			
		}
	}
}