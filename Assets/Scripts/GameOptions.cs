using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.QH.QPGame.Lobby
{
    [AttributeUsage( AttributeTargets.Field, AllowMultiple=true)]
    public class PlatformDesc : Attribute
    {
        public string Comments;
        public RuntimePlatform Platform;
        public bool AllPlatform;

        public PlatformDesc(RuntimePlatform platform, string comments)
        {
            AllPlatform = false;
            Platform = platform;
            Comments = comments;
        }

        public PlatformDesc(string comments)
        {
            AllPlatform = true;
            Comments = comments;
        }
    }

    /*public class GameOption
    {

    }*/

    /// <summary>
    /// 选项配置类,主要用于打包或调试配置参数
    /// TODO 改从配置文件里读取
    /// @Author: guofeng
    /// </summary>
    [Serializable]
    public class GameOptions
    {
        [PlatformDesc("服务器地址")]
        public string ServerHost = "";

        [PlatformDesc("服务器端口")]
        public int ServerPort = 8300;
        
        [PlatformDesc("版本号")]
        public string Version = "";

        [PlatformDesc("服务器列表地址")]
        public string ServerListUrl = "";

        [PlatformDesc("启用服务器列表")]
        public bool EnableServerList = false;

        [PlatformDesc("帧率")]
        public int FrameRate = 30;

        [PlatformDesc("是否启用协议加密")]
        public bool EnableEncryption = true;

        [PlatformDesc("是否验证包文件md5")]
        public bool EnableVerifyAB = true;
        
        [PlatformDesc("是否启用缓存包文件")]
        public bool EnableCacheAB = true;

        [PlatformDesc("发生版本更变时自动清理缓存以及下载文件")]
        public bool AutoCleanCache = true;

        [PlatformDesc(RuntimePlatform.Android, "是否启用Bugly")]
        //[PlatformDesc(RuntimePlatform.IPhonePlayer)]
        public bool EnableBugly = false;
        [PlatformDesc(RuntimePlatform.Android, "Bugly App ID")]
        //[PlatformDesc(RuntimePlatform.IPhonePlayer)]
        public string BuglyAppID = "900009322";
	
        /////////////windows only begin//////////////
        [PlatformDesc(RuntimePlatform.WindowsPlayer, "定宽")]
        public int FixedWidth = Screen.width;
        [PlatformDesc(RuntimePlatform.WindowsPlayer, "定高")]
        public int FixedHeight = Screen.height;
        [PlatformDesc(RuntimePlatform.WindowsPlayer, "是否自动调整大小")]
        public bool FixedSize = false;

        [PlatformDesc(RuntimePlatform.WindowsPlayer, "是否使用启动器")]
        public bool UseLauncher = false;

        [PlatformDesc(RuntimePlatform.WindowsPlayer, "强制单实例运行")]
        public bool ForceSingleton = true;
        /////////////windows only end//////////////

        [PlatformDesc("日志写入到文件")]
        public bool EnableLog2File = true;

        [PlatformDesc("日志等级")]
        public LogType LogLevel = LogType.Error;

        [PlatformDesc("其他配置")] 
        public Dictionary<string, object> CustomData = new Dictionary<string, object>();

    }
}
