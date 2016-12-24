using System.IO;
using System.Threading.Tasks;

namespace Mxp.Core.Services
{
	public interface IFileService {
		Task<string> SaveBinaryAsync (string filename, FileServiceBase.FileDirectory directory, byte [] buffer);
		string GetDirectory (FileServiceBase.FileDirectory directory);
		string DownloadFolder { get; }
		string PathSeparator { get; }
		Stream OpenFileStream (string filepath);
	}
}