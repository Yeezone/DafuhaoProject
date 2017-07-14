using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Net;
using com.QH.QPGame.GameUtils;
using System.IO;

namespace com.QH.QPGame.Lobby
{
    public class WebClientWithTimeout : WebClient
    {
        public readonly float Timeout;

        public long ContentLength;

        public WebClientWithTimeout(float timeout)
        {
            Timeout = timeout;
            ContentLength = 0;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            try
            {
                var request = base.GetWebRequest(address) as HttpWebRequest;
                request.Timeout = (int)(Timeout * 1000);
                request.ReadWriteTimeout = (int)(Timeout * 1000);
                request.Proxy = null;
                return request;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var res = base.GetWebResponse(request);
            if (res != null)
            {
                Logger.Sys.Log("web response length:" + res.ContentLength);
                ContentLength = res.ContentLength;
            }
            return res;
        }
    }


    /// <summary>
    /// 文件下载器
    /// @Author: guofeng
    /// </summary>
    public class Downloader
    {
        private class DownloadTask
        {
            public enum TaskType
            {
                File,
                String,
                Data,
                Upload
            }

            public string Url;
            public string SaveAs;
            public object UserData;
            public int RetryCount;
            public TaskType Type;

            public Action<int, object> RetryCB;
            public Action<Exception, object> ErrorCB;
            public Action<bool, object, object> CompleteCB;
            public Action<object> BeginCB;
        }

        private readonly int retryTimes;
        private readonly float timeout;
        private Uri uri;
        private Stack<DownloadTask> tasks;
        private WebClientWithTimeout client;

        public int Progress { get; private set; }
        public string ProgressStr { get; private set; }

        public Queue<Action> mCallbacks;

        public Downloader(float timeout, int retryTimes)
        {
            this.timeout = timeout;
            this.retryTimes = retryTimes;
            this.mCallbacks = new Queue<Action>();
            this.tasks = new Stack<DownloadTask>();
        }

        public void AddFileTask(string url,
                                string saveAs,
                                object userData = null,
                                Action<Exception, object> errorCB = null,
                                Action<bool, object, object> completeCB = null,
                                Action<object> beginCB = null,
                                Action<int, object> retryCB = null)
        {
            Logger.Sys.Log("add file task. Url:" + url + " SaveAs:" + saveAs + " UserData:" + userData);

            string dir = Path.GetDirectoryName(saveAs);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            tasks.Push(new DownloadTask()
                {
                    Url = url,
                    SaveAs = saveAs,
                    UserData = userData,
                    CompleteCB = completeCB,
                    ErrorCB = errorCB,
                    BeginCB = beginCB,
                    RetryCB = retryCB,
                    Type = DownloadTask.TaskType.File
                });

        }

        public void AddStringTask(string url,
                                  object userData = null,
                                  Action<Exception, object> errorCB = null,
                                  Action<bool, object, object> completeCB = null,
                                  Action<object> beginCB = null,
                                  Action<int, object> retryCB = null)
        {
            tasks.Push(new DownloadTask()
                {
                    Url = url,
                    UserData = userData,
                    CompleteCB = completeCB,
                    ErrorCB = errorCB,
                    BeginCB = beginCB,
                    RetryCB = retryCB,
                    Type = DownloadTask.TaskType.String
                });
        }

        public void AddDataTask(string url,
                                object userData = null,
                                Action<Exception, object> errorCB = null,
                                Action<bool, object, object> completeCB = null,
                                Action<object> beginCB = null,
                                Action<int, object> retryCB = null)
        {
            tasks.Push(new DownloadTask()
                {
                    Url = url,
                    UserData = userData,
                    CompleteCB = completeCB,
                    ErrorCB = errorCB,
                    BeginCB = beginCB,
                    RetryCB = retryCB,
                    Type = DownloadTask.TaskType.Data
                });
        }


        public void Start()
        {
            if (client != null)
            {
                client.CancelAsync();
            }
            else
            {
                client = new WebClientWithTimeout(this.timeout);
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;
                client.DownloadStringCompleted += client_DownloadStringCompleted;
                client.DownloadDataCompleted += client_DownloadDataCompleted;
            }

            RunNextTask();
        }

        public void Update()
        {
            lock (((ICollection) mCallbacks).SyncRoot)
            {
                if (mCallbacks.Count > 0)
                {
                    mCallbacks.Dequeue()();
                }
            }
        }

        public void RunNextTask()
        {
            if (tasks.Count == 0) return;
            var task = tasks.Pop();

            Progress = 0;
            ProgressStr = "";

            Logger.Sys.Log("start download begin, file:" + task.Url);

            try
            {
                switch (task.Type)
                {
                    case DownloadTask.TaskType.File:
                        {
                            string tempFile = task.SaveAs + ".tmp";
                            if (File.Exists(task.SaveAs))
                            {
                                File.Delete(task.SaveAs);
                            }

                            if (File.Exists(tempFile))
                            {
                                File.Delete(tempFile);
                            }

                            client.DownloadFileAsync(new Uri(task.Url), tempFile, task);
                            break;
                        }
                    case DownloadTask.TaskType.Data:
                        {
                            client.DownloadDataAsync(new Uri(task.Url), task);
                            break;
                        }
                    case DownloadTask.TaskType.String:
                        {
                            client.DownloadStringAsync(new Uri(task.Url), task);
                            break;
                        }
                }

                if (task.BeginCB != null)
                {
                    task.BeginCB(task.UserData);
                }

            }
            catch (Exception e)
            {
                Logger.Sys.LogException(e);
            }
        }

        public void CancelTask()
        {
            tasks.Clear();

            if (client != null)
            {
                client.DownloadProgressChanged -= client_DownloadProgressChanged;
                client.DownloadFileCompleted -= client_DownloadFileCompleted;
                client.DownloadStringCompleted -= client_DownloadStringCompleted;
                client.DownloadDataCompleted -= client_DownloadDataCompleted;

                client.CancelAsync();
                client.Dispose();
                client = null;
            }
        }


        private void OnDownloadDataCompleted(object sender, DownloadDataCompletedEventArgs args)
        {
            var task = args.UserState as DownloadTask;
            if (args.Error != null)
            {
                if (task.RetryCount++ >= retryTimes && task.ErrorCB != null)
                {
                    task.ErrorCB(args.Error, task.UserData);
                    return;
                }
                else
                {
                    if (task.RetryCB != null)
                    {
                        task.RetryCB(task.RetryCount, task.UserData);
                    }
                    tasks.Push(task);
                }
            }
            else
            {
                bool allDone = false;
                allDone = tasks.Count == 0;
                if (task.CompleteCB != null)
                {
                    task.CompleteCB(allDone, args.Result, task.UserData);
                }
                if (allDone)
                {
                    return;
                }
            }

            RunNextTask();
        }

        private void client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs args)
        {
            var task = args.UserState as DownloadTask;
            Logger.Sys.Log("data completed. Url:" + task.Url + " Error:" + args.Error);
            mCallbacks.Enqueue(() => OnDownloadDataCompleted(sender, args));
        }

        private void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs args)
        {
            var task = args.UserState as DownloadTask;
            if (args.Error != null)
            {
                if (task.RetryCount++ >= retryTimes && task.ErrorCB != null)
                {
                    task.ErrorCB(args.Error, task.UserData);
                    return;
                }
                else
                {
                    if (task.RetryCB != null)
                    {
                        task.RetryCB(task.RetryCount, task.UserData);
                    }
                    tasks.Push(task);
                }
            }
            else
            {
                bool allDone = false;
                allDone = tasks.Count == 0;
                if (task.CompleteCB != null)
                {
                    task.CompleteCB(allDone, args.Result, task.UserData);
                }
                if (allDone)
                {
                    return;
                }
            }

            RunNextTask();
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs args)
        {
            var task = args.UserState as DownloadTask;
            Logger.Sys.Log("string completed. Url:" + task.Url + " Error:" + args.Error);

            mCallbacks.Enqueue(() => OnDownloadStringCompleted(sender, args));
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs args)
        {
            var task = args.UserState as DownloadTask;
            if (args.Error == null)
            {
                var fileInfo = new FileInfo(task.SaveAs + ".tmp");
                if (fileInfo.Exists && !fileInfo.IsReadOnly)
                {
                    fileInfo.MoveTo(task.SaveAs);

                    bool allDone = false;
                    allDone = tasks.Count == 0;

                    if (task.CompleteCB != null)
                    {
                        task.CompleteCB(allDone, null, task.UserData);
                    }

                    if (!allDone)
                    {
                        RunNextTask();
                    }
                }
                else
                {
                    task.ErrorCB(new Exception("文件不存在或者被占用"), task.UserData);
                }

                return;
            }

            if (task.RetryCount++ >= retryTimes && task.ErrorCB != null)
            {
                task.ErrorCB(args.Error, task.UserData);
            }
            else
            {
                if (task.RetryCB != null)
                {
                    task.RetryCB(task.RetryCount, task.UserData);
                }

                tasks.Push(task);

                RunNextTask();
            }
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs args)
        {
            var task = args.UserState as DownloadTask;
            Logger.Sys.Log("file completed. Url:" + task.Url + " Error:" + args.Error);

            mCallbacks.Enqueue(() => OnDownloadFileCompleted(sender, args));
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
            ProgressStr = string.Format("{0}kb/{1}kb", e.BytesReceived/1024, e.TotalBytesToReceive/1024);
        }

    }
}
