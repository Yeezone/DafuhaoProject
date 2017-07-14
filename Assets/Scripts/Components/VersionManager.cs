using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.Data;


namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 版本管理器，实现程序，包资源等更新
    /// 主要流程为：
    ///     1，通过发送socket到登录服务器，查询当前版本以及各项配置
    ///     2，根据返回的版本号与本地的版本号比较
    ///     3，拉取服务器更新配置文件，强制更新则直接调用平台接口
    ///     4，否则，拉取服务器资源配置文件，与本地配置文件比较，获得需要更新的列表，然后下载
    ///     5，重新加载本地配置文件
    /// @Author: guofeng
    /// </summary>
    public class VersionManager : MonoBehaviour
    {
        /*private struct DownloadReq
        {
            private int CurrentGameID;
            private Stack<int> Tasks;
            public string Url;
        }*/

        public string Status { get; private set; }
        public bool Error { get; private set; }
        public string ErrorStr { get; private set; }
        public bool Done { get; private set; }
        public bool RemoteConfigLoaded { get; private set; }

        public int TotalFileCount;
        public int DownloadedCount;


        public float Progress
        {
            get
            {
                if (downloader != null)
                {
                    return downloader.Progress/100.0f;
                }

                return 0.0f;
            }
        }

        public string ProgressStr
        {
            get
            {
                if (downloader != null)
                {
                    return downloader.ProgressStr;
                }

                return "";
            }
        }

        public int RemoteVersion { get; set; }

        private bool forceUpdate;
        private string downloadUrl;
        private Downloader downloader;
       
        public static string RemoteAppVersionFileUrl =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || DISABLE_AB
            GlobalConst.Update.PC_AppVersionFile;
#elif UNITY_ANDROID
            GlobalConst.Update.Android_AppVersionFile;
#elif UNITY_IPHONE
            GlobalConst.Update.iOS_AppVersionFile;
#endif

        public bool Initialize()
        {
            Reset();
            return true;
        }

        public void Reset()
        {
            if (downloader != null)
            {
                downloader.CancelTask();
                downloader = null;
            }

            Error = false;
            ErrorStr = null;
            Status = null;
            Done = false;
            DownloadedCount = 0;
            TotalFileCount = 0;
            RemoteVersion = 0;
            forceUpdate = false;
            downloadUrl = null;
            RemoteConfigLoaded = false;
        }

        public bool IsForceUpdate()
        {
            int localVersion = GameVersion.ProcessVersion(GameApp.GameData.Version);
            return GameVersion.IsUpdateNeeded(localVersion, RemoteVersion,
                                              (int)GlobalConst.Update.ForceUpdateLevel);
        }

        public bool IsUpdateNeeded()
        {
            int localVersion = GameVersion.ProcessVersion(GameApp.GameData.Version);
            return localVersion != RemoteVersion;
        }

        public bool Run()
        {
            RemoteConfigLoaded = false;
            Error = false;
            ErrorStr = null;
            Done = false;
            Status = "";
            DownloadedCount = 0;
            TotalFileCount = 0;

            var resolvedUrl = GameHelper.ResolveDownloadUrl(
                GameApp.GameData.CDN + RemoteAppVersionFileUrl, 
                GlobalConst.Update.PrecisionOfDownload);

            downloader = new Downloader(GlobalConst.Update.Timeout, 
                GlobalConst.Update.RetryTimes);

            downloader.AddStringTask(
                resolvedUrl,
                null,
                OnDownloadError,
                OnDownloadAppFile,
                OnDownloadBegin,
                OnDownloadRetry
                );

            downloader.Start();

            Status = "获取远程程序配置文件";
            Logger.UI.Log(Status);
            return true;
        }

        void Update()
        {
            if (downloader != null)
            {
                downloader.Update();
            }
        }

        private void OnDownloadRetry(int times, object obj)
        {
            Status = string.Format("下载失败 {0}/{1}，再次重试", times, 
                GlobalConst.Update.RetryTimes);
            Logger.Net.Log(Status);
        }

        public void FullUpdate()
        {
//#if TEST_UPDATE
			WindowsUpdater.Run(downloadUrl, RemoteVersion);
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
			WindowsUpdater.Run(downloadUrl, RemoteVersion);
#elif !UNITY_EDITOR && UNITY_ANDROID
			AndroidUpdater.Run(downloadUrl, RemoteVersion);
#elif !UNITY_EDITOR && UNITY_IOS
			iOSUpdater.Run(downloadUrl, RemoteVersion);
#endif
        }

        
        private void ErrorOccurred(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                Logger.Sys.Log("download failed. Error:" + error);
            }

            Error = !string.IsNullOrEmpty(error);
            ErrorStr = error;
            Status = error;
            Done = true;
            DownloadedCount = 0;
            TotalFileCount = 0;
            //Progress = 0.0f;
        }

        #region 回调

        private void OnDownloadAppFile(bool b, object result, object arg3)
        {
            string text = result as string;

            Status = "程序配置文件下载完成";
            Logger.Sys.Log(Status);

            forceUpdate = IsForceUpdate();

            var remoteAppVer = JsonMapper.ToObject<AppVersionDesc>(text);
            var remoteConfig = remoteAppVer.FindConfig(new[] { forceUpdate ? GameApp.GameData.Version : "res", "other" });
            if (remoteConfig == null)
            {
                ErrorOccurred("服务器版本配置错误，没有找到客户端版本对应的下载配置");
                return;
            }

			downloadUrl = remoteConfig.DownloadUrl;

            if (forceUpdate)
            {
                ErrorOccurred(null);
            }
            else
            {
                var files = new[] { GlobalConst.Res.GameConfigFileName, GlobalConst.Res.ResVersionFileName };
                //var files = new[] { GlobalConst.Res.ResVersionFileName };
                foreach (var file in files)
                {
                    var resolvedUrl = GameHelper.ResolveDownloadUrl(downloadUrl + file, 
                        GlobalConst.Update.PrecisionOfDownload);
                    downloader.AddStringTask(
                        resolvedUrl,
                        file,
                        OnDownloadError,
                        OnDownloadResFile,
                        OnDownloadBegin,
                        OnDownloadRetry
                        );

                }
                downloader.Start();
            }
        }

        private void OnDownloadError(Exception exception, object userData)
        {
            Logger.Sys.LogException(exception);

            if (exception is WebException)
            {
                var webEx = exception as WebException;
                switch (webEx.Status)
                {
                    case WebExceptionStatus.Timeout:
                    case WebExceptionStatus.ConnectionClosed:
                        {
                            ErrorOccurred("连接超时，请检查网络后重试");
                            return;
                        }
                    case WebExceptionStatus.ConnectFailure:
                        {
                            ErrorOccurred(webEx.Message);
                            return;
                        }
                    case WebExceptionStatus.ProtocolError:
                        {
                            var res = webEx.Response as HttpWebResponse;
                            switch (res.StatusCode)
                            {
                                case HttpStatusCode.NotFound:
                                    {
                                        ErrorOccurred("文件不存在: "+userData);
                                        return;
                                    }
                                default:
                                    {
                                        ErrorOccurred("协议错误: " + webEx.Message);
                                        return;
                                    }
                            }
                        }
                    default:
                        {
                            ErrorOccurred("下载失败，请稍后重试:"+webEx.Message);
                            return;
                        }
                }
            }

            if (exception is InvalidOperationException)
            {
                ErrorOccurred("下载失败，请稍后重试");
                return;
            }

            ErrorOccurred(exception.Message);
        }


        private void OnDownloadResFile(bool allDone, object result, object userState)
        {
            string fileName = userState as string;
            string text = result as string;

            switch (fileName)
            {
                case GlobalConst.Res.GameConfigFileName:
                    {
                        Status = "游戏配置读取完毕";
                        Logger.Sys.Log(Status);

                        GameApp.GameMgr.UpdateExistsGameConfig(text);
                        GameApp.GameMgr.SaveGameConfig();
                        GameApp.GameMgr.Initialize();

                        break;
                    }
                case GlobalConst.Res.ResVersionFileName:
                    {
                        Status = "资源配置读取完毕";
                        Logger.Sys.Log(Status);

                        GameApp.ResMgr.LoadRemoteResConfigFromString(text);

                        break;
                    }
            }

            Logger.Sys.Log(Status);

            if (allDone)
            {
                RemoteConfigLoaded = true;

                var names = GameApp.ResMgr.FindUpdatableNames(false);
                var files = GameApp.ResMgr.FindUpdatableRes(names.ToArray());
                if (files == null || files.Count == 0)
                {
                    /*if (IsUpdateNeeded() && !IsForceUpdate())
                    {
                        GameApp.GameData.Version = GameVersion.Version2Str(RemoteVersion);
                    }*/
                    ErrorOccurred(null);
                    return;
                }

                DownloadFiles(files);
            }
        }

        private void OnDownloadBegin(object userState)
        {
            Status = "正在下载";
            /*if (userState is string)
           {
               Status = "下载中."; //+ System.IO.Path.GetFileName(userState as string);
           }
           else if (userState is AssetBundleInfo)
           {
               var ab = userState as AssetBundleInfo;
               Status = "下载中."; //+ System.IO.Path.GetFileName(ab.FileName);
           }*/
        }

        private void OnDownloadCompleted(bool allDone, object o, object userState)
        {
            DownloadedCount++;
            Status = "文件下载完毕";
            Logger.Sys.Log(Status);

            var ab = userState as AssetBundleInfo;
            //Status = ab.FileName + "下载完毕";

            string str = string.Format("file downloaded:{0} task:{1}/{2}", 
                ab.FileName, DownloadedCount, TotalFileCount);
            Logger.Sys.Log(str);

            //TODO MD5校验
            if (GameApp.Options.EnableVerifyAB)
            {
                Status = "正在校验文件";
                Logger.Sys.Log(Status);

                string fileName = GameApp.GameData.DownloadPath + ab.FileName;
                string md5 = MD5Util.GetFileMD5(fileName);
                if (string.Compare(md5, ab.Hash, true) != 0)
                {
                    ErrorOccurred("文件校验失败");
                    return;
                }
            }

            GameApp.ResMgr.UpdateResConfig(ab.FileName, ab);
            GameApp.ResMgr.SaveResConfig();

            if (allDone)
            {
                /*if (IsUpdateNeeded() && !IsForceUpdate())
                {
                    GameApp.GameData.Version = GameVersion.Version2Str(RemoteVersion);
                }*/

                GameApp.ResMgr.ScanDownloadedFiles();
                GameApp.GameMgr.Initialize();

                //Progress = 1.0f;
                Status = "所有文件下载完毕";
                Logger.Sys.Log(Status);

                Done = true;
            }
        }
        
        #endregion

        private void DownloadFiles(ICollection<AssetBundleInfo> files)
        {
            DownloadedCount = 0;
            TotalFileCount = files.Count;

            foreach (AssetBundleInfo assetBundleInfo in files)
            {
                string url = GameHelper.ResolveDownloadUrl(downloadUrl + assetBundleInfo.FileName, 
                    GlobalConst.Update.PrecisionOfDownload);

                string saveAs = GameApp.GameData.DownloadPath + assetBundleInfo.FileName;

                Logger.Sys.Log("add download task. Url:" + url);

                downloader.AddFileTask(
                    url,
                    saveAs,
                    assetBundleInfo,
                    OnDownloadError,
                    OnDownloadCompleted,
                    OnDownloadBegin,
                    OnDownloadRetry
                    );
            }
            downloader.Start();
        }

        public void DownloadGame(int gameID)
        {
            Logger.Sys.Log("add download task. GameID:" + gameID);

            Error = false;
            ErrorStr = null;
            Done = false;
            Status = "";
            DownloadedCount = 0;
            TotalFileCount = 0;
            //Progress = 0.0f;

            var config = GameApp.GameMgr.GetGameConfig(gameID);
            if (config == null)
            {
                Logger.Sys.LogError("no match game's config for GameID:" + gameID);
                return;
            }

            var files = GameApp.ResMgr.FindUpdatableRes(config.AssetBundles);
            if (files == null || files.Count == 0)
            {
                ErrorOccurred("没有发现需要下载的文件");
                return;
            }

            TotalFileCount = files.Count;
            DownloadFiles(files);
        }


        public void CancelDownload(int gameID)
        {
            if (downloader != null)
            {
                Logger.Sys.Log("cancel download. GameID:" + gameID);
                downloader.CancelTask();
                ErrorOccurred("用户取消");
            }
        }

        public void CancelAll()
        {
            CancelDownload(-1);
        }

    }
}
