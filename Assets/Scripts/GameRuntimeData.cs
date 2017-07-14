using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.Data;

using System;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 运行数据管理类,主要用于存放内存中的变更数据
    /// @Author: guofeng
    /// </summary>
    public class GameRuntimeData
    {
        public bool Initialize()
        {
            Account = null;
            Password = null;
            MAC = "";

            Host = "";
            Port = 0;

            BackStorgeUrl = "";
            CDN = "";
            OfficeSiteUrl = "";

            EnterGameID = 0;
            EnterRoomID = 0;
            UserInfo = null;

            PrivateKey = "";
            TempPassword = null;
            SavePassword = false;
            TempNickName = "";


            MAC = MacAddress.GetMacAddress();
            Host = GameApp.Options.ServerHost;
            Port = (uint) GameApp.Options.ServerPort;
            ForceSingleton = GameApp.Options.ForceSingleton;
            DownloadPath = GameHelper.GetDownloadPath(false);

            Version = GameApp.Options.Version;
            InstallVersion = "";

//#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
//            ProcessCommandline();
//#endif
            return true;
        }

//#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
//        private bool ProcessCommandline()
//        {
//            string[] commandLines = Environment.GetCommandLineArgs();
//            if (GameApp.Options.UseLauncher &&
//                commandLines.Length >= 8)
//            {
//                Host = commandLines[0];
//                Port = uint.Parse(commandLines[1]);

//                Account = commandLines[2];
//                Password = commandLines[3];

//                OfficeSiteUrl = commandLines[4];
//                BackStorgeUrl = commandLines[5];
//                CDN = commandLines[6];
//                MAC = commandLines[7];
//            }
//            else if (commandLines.Length >= 0)
//            {
//                foreach (var commandLine in commandLines)
//                {
//                    if (string.Compare(commandLine, "Mutil") == 0)
//                    {
//                        ForceSingleton = false;
//                    }
//                    else if (string.Compare(commandLine, "ByInstall") == 0 ||
//                        string.Compare(commandLine, "ByUpdate") == 0)
//                    {
//                        InstallOrUpdate = true;
//                    }
//                }
//            }

//            return true;
//        }
//#endif

        public string Account = null;
        public string Password = null;
        public string MAC = "";

        public string InstallVersion = "";
        public string Version = "";

        public string Host = "";
        public uint Port = 0;

        public string BackStorgeUrl = "";
        public string CDN = "";
        public string OfficeSiteUrl = "";

        public Int64 DiffServerTime = 0;


        public uint EnterGameID = 0;
        public uint EnterRoomID = 0;

        public UserInfo UserInfo = null;

		public string PrivateKey = "";
		public string TempPassword = null;
		public bool SavePassword = false;
		public bool ForceSingleton = true;
        public string TempNickName = "";
        public bool InstallOrUpdate = false;

        public string DownloadPath = "";

    }

}

