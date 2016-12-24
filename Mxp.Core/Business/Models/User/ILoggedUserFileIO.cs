using System;
using Mxp.Core.Business;

namespace Mxp.Core.Business
{
	public interface ILoggedUserFileIO
	{
		
		void writeFile(string filename, string content);
		string FullPath(string filename);
		string readFile(string filename);
		void RemoveFile (string filename);

	}
}