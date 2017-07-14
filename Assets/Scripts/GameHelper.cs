
using System;
using System.IO;
using UnityEngine;
using com.QH.QPGame.Services.NetFox;
using com.QH.QPGame.Utility;

namespace com.QH.QPGame.Lobby
{
    public static class GameHelper
	{
        /// <summary>
        /// 生成下载URL,后面拼接随机字符串防止CDN缓存
        /// </summary>
        /// <returns></returns>
        public static string ResolveDownloadUrl(string url, int precision = 1 )
        {
            return url + "?" + (DateTime.Now.Ticks / precision).ToString();
        }

        public static string GetInternalPath(bool withPrefix = true)
        {
            string path = "";
            if (withPrefix)
            {
#if !UNITY_EDITOR && UNITY_ANDROID
#else
                path = "file:///";
#endif
            }

            path += Application.streamingAssetsPath + "/" + 
                GetPlatformName(Application.platform) + "/";
            return path;
        }

        public static string GetDownloadPath(bool withPrefix = true)
        {
            string path = "";
            if (withPrefix)
            {
                path = "file:///";
            }

            path += Application.persistentDataPath + "/" + 
                GetPlatformName(Application.platform) + "/";
            return path;
        }

        public static string GetPlatformName(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    {
                        return "Windows";
                    }
                case RuntimePlatform.Android:
                    {
                        return "Android";
                    }
                case RuntimePlatform.IPhonePlayer:
                    {
                        return "iOS";
                    }
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    {
                        return "OSX";
                    }
            }

            return null;
        }

#if UNITY_EDITOR
        public static string GetABExt(UnityEditor.BuildTarget target)
        {
            switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget)
            {
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneOSXIntel:
                case UnityEditor.BuildTarget.StandaloneOSXIntel64:
                case UnityEditor.BuildTarget.StandaloneOSXUniversal:
                    {
                        return GlobalConst.Res.PC_BundleFileExt;
                    }
                case UnityEditor.BuildTarget.Android:
                case UnityEditor.BuildTarget.iOS:
                    {
                        return GlobalConst.Res.Phone_BundleFileExt;
                    }
            }

            return null;
        }

        public static string GetBuildTargetGroupName(UnityEditor.BuildTarget target)
        {
            switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget)
            {
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneOSXIntel:
                case UnityEditor.BuildTarget.StandaloneOSXIntel64:
                case UnityEditor.BuildTarget.StandaloneOSXUniversal:
                    {
                        return "PC";
                    }
                case UnityEditor.BuildTarget.Android:
                case UnityEditor.BuildTarget.iOS:
                    {
                        return "Phone";
                    }
            }

            return null;
        }

        public static string GetBuildTargetName(UnityEditor.BuildTarget target)
        {
            switch (target)
            {
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    {
                        return "Windows";
                    }
                case UnityEditor.BuildTarget.Android:
                    {
                        return "Android";
                    }
                case UnityEditor.BuildTarget.iOS:
                    {
                        return "iOS";
                    }
                case UnityEditor.BuildTarget.StandaloneOSXIntel:
                case UnityEditor.BuildTarget.StandaloneOSXIntel64:
                case UnityEditor.BuildTarget.StandaloneOSXUniversal:
                    {
                        return "OSX";
                    }
            }

            return null;
        }
#endif

        public static string GetABExt(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    {
                        return GlobalConst.Res.PC_BundleFileExt;
                    }
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    {
                        return GlobalConst.Res.Phone_BundleFileExt;
                    }
            }

            return null;
        }

        //获取版本信息
        public static byte GetServerSidePlatform()
        {
            byte platform = CommonDefine.DEVICE_TYPE_PC;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    {
                        platform = CommonDefine.DEVICE_TYPE_ANDROID;
                        break;
                    }
                case RuntimePlatform.IPhonePlayer:
                    {
                        platform = CommonDefine.DEVICE_TYPE_IOS;
                        break;
                    }
            }

            return platform;
        }
	}
}

