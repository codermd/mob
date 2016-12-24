using System;
using Mxp.Core.Business;
using System.IO;
using Mxp.Droid.Helpers;

[assembly: Xamarin.Forms.Dependency (typeof (LoggedUserFileIO))]

namespace Mxp.Droid.Helpers
{
	public class LoggedUserFileIO : Java.Lang.Object, ILoggedUserFileIO
	{
		private static object locker = new object ();

		public LoggedUserFileIO () {}

		public void writeFile(string filename, string content) {
			// Prevent "Sharing Violation on Path" IOException
			lock (locker)
				File.WriteAllText (FullPath (filename), content);
		}

		public string FullPath(string filename) {
			string documentPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			return Path.Combine (documentPath, filename);
		}

		public string readFile(string filename) {
			if (!File.Exists (this.FullPath(filename)))
				return null;

			return File.ReadAllText (this.FullPath(filename));
		}

		public void RemoveFile (string filename) {
			File.Delete (FullPath(filename));
		}

	}
}