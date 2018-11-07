using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveAsyncWorker
{

    //public static class FileDownloaderHelper
    //{
    //    public static IObservable<DownloadFileProgressChangedArgs> GetProgress(this IFileDownloader fileDownloader) =>
    //        Observable.FromEventPattern<EventHandler<FileDownloader.DownloadFileProgressChangedArgs>, FileDownloader.DownloadFileProgressChangedArgs>(h => fileDownloader.DownloadProgressChanged += h, h => fileDownloader.DownloadProgressChanged -= h)
    //      .Select(e => e.EventArgs);

    //    public static IObservable<DownloadFileCompletedArgs> GetCompletion(this IFileDownloader fileDownloader) =>
    //        Observable.FromEventPattern<FileDownloader.DownloadFileCompletedArgs>(h => fileDownloader.DownloadFileCompleted += h, h => fileDownloader.DownloadFileCompleted -= h)
    //            .Select(_ => _.EventArgs);

    //}


    public static class FileHelper
    {

        public static bool CheckFile(string sink)
        {

            System.IO.FileInfo info = new System.IO.FileInfo(sink);
            if (info.Length > 0)
            {
                return true;
                //Kaliko.Logger.Write(string.Format("File {0} downloaded to {1} @ {2}", source, sink, DateTime.Now), Kaliko.Logger.Severity.Info);
            }
            else
            {
                System.IO.File.Delete(sink);
                return false;
            }

        }
    }

}
