using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 全局常量配置
    /// 按模块划分，需要被配置文件加载的需要定义为static，否则为const
    /// @Author: guofeng
    /// </summary>
    public static class GlobalConst
    {
        public static class Res
        {
            public const string AppConfigFileName = "AppConfig.json";
            public const string ResVersionFileName = "ResConfig.json";
            public const string GameConfigFileName = "GameConfig.json";
            public const string UIConfigFileName = "UIConfig.json";
            public const string MarqueeDataFileName = "Marquees";
            public const string ServerListFileName = "server_list.txt";

            public const string SceneFileExt = ".scene";
            public const string PC_BundleFileExt = ".pc";
            public const string Phone_BundleFileExt = ".mp";

            public const string EncryptPassword = "com.DiMo";
        }

        public static class Settings
        {
            public const string Account = "Account";
            public const string Password = "Password";
            public const string SavePass = "SavePass";
            public const string Version = "Version";
            public const string InstallVersion = "InstallVersion";
        }

        public static class Update
        {
            public enum VersionDesc
            {
                MainVersion = 1,
                SubVersion,
                ProductVersion,
                BuildVersion
            }

            /// <summary>
            /// 强制更新等级
            /// </summary>
            public static VersionDesc ForceUpdateLevel = VersionDesc.ProductVersion;
            
            /// <summary>
            /// 检测更新等级
            /// </summary>
            public static VersionDesc UpdatableLevel = VersionDesc.BuildVersion;

            /// <summary>
            /// 下载重试次数
            /// </summary>
            public static int RetryTimes = 3;

            /// <summary>
            /// 下载超时
            /// </summary>
            public static float Timeout = 5.0f;

            /// <summary>
            /// CDN缓存时间
            /// </summary>
            public static int PrecisionOfDownload = 60*10;

			///
			/// 缓存警告阀值
			///
            public static int CachingSizeWarning = 40 * 1024 * 1024;

            public static string PC_AppVersionFile = "windows_update_version.txt";
            public static string Android_AppVersionFile = "android_update_version.txt";
            public static string iOS_AppVersionFile = "ios_update_version.txt";

        }


        public static class UI
        {
            public static string UI_SCENE_LOADING = "UI_Scene_Loading";
			public static string UI_PREFAB_LOADING = "Prefabs/UI_Loading_Root";
            public static string UI_PREFAB_LOADING_COMMON = "Prefabs/UI_Loading_Common";
			public static string UI_PREFAB_TIPS = "Prefabs/UI_TIPS";
            public static string UI_MARQUEE_DEFAULT = "UI_Marquee_Default";
            public const string UI_GAME_LOADING = "Prefabs/loading/loading_{0}";

            public static string UI_SCENE_MAIN = "Main";
            public static string UI_SCENE_LOGIN = "scene_login";
            public static string UI_SCENE_HALL = "scene_lobby";
            public static string UI_SCENE_GAME = "games/lv_{0}/{1}";

        }

        public static class Base
        {
            public static float LoadListTimeOut = 20.0f;
            public static float LoadingTimeOut = 120.0f;
            public static float LoginTimeOut = 120.0f;
        }

        public static class Game
        {
            public static int ReconnectTimes = 3;

        }

        public static class URL
        {
            public static string Notice = "/Message/Notice.aspx?Platform={0}";
            public static string Recharge = "/ProxyHandler/User.ashx?action=recharge";
            public static string Exchange = "/ProxyHandler/User.ashx?action=recharge";
            public static string GetRecord = "/ProxyHandler/User.ashx?action=getrecharge";
            public static string GetNotice = "/ProxyHandler/User.ashx?action=getNotice";
            public static string CancelRecharge = "/ProxyHandler/User.ashx?action=RechangeCancel";
            public static string CancelExchange = "/ProxyHandler/User.ashx?action=ExchangeCancel";
        }

    }
}
