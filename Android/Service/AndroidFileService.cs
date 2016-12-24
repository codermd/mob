using System.IO;
using Mxp.Core.Services;
using Mxp.Droid.Service;

[assembly: Xamarin.Forms.Dependency (typeof (AndroidFileService))]

namespace Mxp.Droid.Service
{
	public class AndroidFileService : FileServiceBase {
		public override string DownloadFolder => Android.OS.Environment.GetExternalStoragePublicDirectory (Android.OS.Environment.DirectoryDownloads).AbsolutePath;

		public override string PathSeparator => "/";

		public override Stream OpenFileStream (string filepath) {
			FileStream filestream = File.OpenWrite (filepath);

			return filestream;
		}
	}
}