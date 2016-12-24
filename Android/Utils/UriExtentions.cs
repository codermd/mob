using Android.Content;
using Android.Database;
using Android.Provider;

namespace Mxp.Droid
{
	public static class UriExtentions {
		public static string GetPath (this Android.Net.Uri uri, Context context) {
			string [] projection = new [] { MediaStore.Images.Media.InterfaceConsts.Data };
			using (ICursor cursor = context.ContentResolver.Query (uri, projection, null, null, null)) {
				if (cursor != null) {
					int columnIndex = cursor.GetColumnIndexOrThrow (MediaStore.Images.Media.InterfaceConsts.Data);
					cursor.MoveToFirst ();

					if (cursor.GetString (columnIndex) != null)
						return cursor.GetString (columnIndex);
				}
			}

			return null;
		}
	}
}