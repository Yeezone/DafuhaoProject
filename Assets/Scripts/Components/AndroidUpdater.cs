using com.QH.QPGame.Utility;
using com.QH.QPGame.GameUtils;
using System;
using UnityEngine;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 安卓更新器
    /// </summary>
    public class AndroidUpdater
    {
        public static bool Run(string downloadUrl, int version)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            try
            {
                string url = GameHelper.ResolveDownloadUrl(downloadUrl, GlobalConst.Update.PrecisionOfDownload);
                string saveFileName = Application.companyName + "_" + GameVersion.Version2Str(version) + ".apk";

                //TODO 监听广播安装然后直接启动自身
                AndroidJavaClass installClass = new AndroidJavaClass("com.DiMo.ToolChain.UpdateManager");
                installClass.CallStatic(
                    "DownloadAndUpdate",
                    url,
                    saveFileName,
                    new AndroidJavaObject("java.lang.Boolean", true)
                    );
            }
            catch (Exception e)
            {
                Logger.Sys.LogException(e);
            }
#endif
            return true;
        }

    }
}
