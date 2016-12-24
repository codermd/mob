using System;

using Android.Webkit;

namespace Mxp.Droid
{
	public static class FileExtentions {
		public static string GetMimetype (this Java.IO.File file) {
			string type = null;
			string extension = MimeTypeMap.GetFileExtensionFromUrl (Android.Net.Uri.FromFile (file).ToString ());
			if (!String.IsNullOrEmpty (extension))
				type = MimeTypeMap.Singleton.GetMimeTypeFromExtension (extension);
			return type;
		}

		public static bool IsImage (this Java.IO.File file) {
			string type = GetMimetype (file);
			return String.IsNullOrEmpty (type) ? false : type.Contains ("image");
		}
	}
}