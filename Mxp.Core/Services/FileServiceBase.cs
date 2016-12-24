using System.IO;
using System.Threading.Tasks;

namespace Mxp.Core.Services
{
	public abstract class FileServiceBase : IFileService {
		public virtual async Task<string> SaveBinaryAsync (string filename, FileDirectory directory, byte [] buffer) {
			string fullpath = GetDirectory (directory) + filename;
			using (Stream stream = OpenFileStream (fullpath)) {
				await stream.WriteAsync (buffer, 0, buffer.Length);
				await stream.FlushAsync ();
			}

			return fullpath;
		}

		public string GetDirectory (FileDirectory directory) {
			switch (directory) {
				case FileDirectory.Download:
					return this.DownloadFolder + PathSeparator;
				default:
					return string.Empty;
			}
		}

		public abstract string DownloadFolder { get; }
		public abstract string PathSeparator { get; }

		public abstract Stream OpenFileStream (string filepath);

		public enum FileDirectory {
			Download
		}
	}
}