using System;
using Mxp.iOS;
using Mxp.Core.Business;
using Foundation;
using System.IO;

[assembly: Xamarin.Forms.Dependency (typeof (LoggedUserFileIO))]

namespace Mxp.iOS
{
	public class LoggedUserFileIO : ILoggedUserFileIO
	{
		public LoggedUserFileIO () {}

		public void writeFile(string filename, string content) {
			NSData data = NSData.FromString (content);
			data.Save (FullPath (filename), true);
		}

		public string FullPath(string filename) {
			string documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			return Path.Combine (documents, filename);
		} 

		public string readFile(string filename) {
			if (!NSFileManager.DefaultManager.FileExists (FullPath(filename)))
				return null;

			NSData data = NSData.FromFile (FullPath(filename));
			return NSString.FromData (data, NSStringEncoding.UTF8);
		}

		public void RemoveFile (string filename) {
			NSError error;
			NSFileManager.DefaultManager.Remove (FullPath(filename), out error);
		}
	}
}