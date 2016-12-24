using System;
using System.Collections.Generic;
using System.Text;
using Mxp.Win;
using Mxp.Core.Business;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Windows.Foundation;
using System.Diagnostics;

[assembly: Xamarin.Forms.Dependency(typeof(LoggedUserFileIO))]

namespace Mxp.Win
{
    public class LoggedUserFileIO : ILoggedUserFileIO
    {
        private static object locker = new object ();

        public LoggedUserFileIO () { }



        public void writeFile(string filename, string content)
        {
            StorageFolder appFolder =
              ApplicationData.Current.LocalFolder;
            try
            {
                lock (locker)
                {
                    IAsyncOperation<StorageFile> getFileTask = appFolder.CreateFileAsync (filename, CreationCollisionOption.ReplaceExisting);
                    getFileTask.AsTask ().Wait ();
                    StorageFile file = getFileTask.GetResults ();
                    IAsyncAction action = FileIO.WriteTextAsync(file, content);
                    action.AsTask().Wait();
                }
                Debug.WriteLine("Write File SUCCESS >> >> " + filename);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Write File ERROR >>>> " + filename + ex.Message);
            }
        }

        public string FullPath(string filename)
        {
            throw new NotImplementedException();
        }

        public string readFile(string filename)
        {
            StorageFolder appFolder =
                ApplicationData.Current.LocalFolder;
            try
            {
                IAsyncOperation<StorageFile> getFileTask = appFolder.GetFileAsync(filename);
                getFileTask.AsTask().Wait();
                IAsyncOperation<string> task = FileIO.ReadTextAsync(getFileTask.GetResults());
                task.AsTask().Wait();
                string result = task.GetResults();
                Debug.WriteLine("Read File SUCCESS >> >> " + filename);

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Read File Error >>>> " + filename + ex.Message);
                return null;
            }
        }

        public void RemoveFile(string filename)
        {
            StorageFolder appFolder =
                ApplicationData.Current.LocalFolder;
            try
            {
                IAsyncOperation<StorageFile> file = appFolder.GetFileAsync(filename);
                file.AsTask().Wait();
                Debug.WriteLine("Find to delete File SUCCESS >> >> " + filename);

                IAsyncAction action = file.GetResults().DeleteAsync();
                action.AsTask().Wait();
                Debug.WriteLine("Remove File SUCCESS >> >> " + filename);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Delete file error >>>> " + filename + ex.Message);
                return;
            }

        }
    }
}
