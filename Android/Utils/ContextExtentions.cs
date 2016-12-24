using System;
using Android.Content;

namespace Mxp.Droid
{
	public static class ContextExtentions {
		public static Java.IO.File CreateImageFile (this Context context, out string filePath) {
			string imageFileName = String.Format ("{0}", Guid.NewGuid ());
			Java.IO.File storageDir = context.GetExternalFilesDir (Android.OS.Environment.DirectoryPictures);
			Java.IO.File image = Java.IO.File.CreateTempFile (imageFileName, ".jpg", storageDir);

			filePath = image.AbsolutePath;

			return image;
		}
	}
}
