using System;
using UnityEngine;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Utility;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 程序入口以及资源管理调度中心
    /// @Author: guofeng
    /// </summary>
    public class GameApp
    {
        private static GameApp app = null;
        public static GameSettings Settings = null;
        public static GameRuntimeData GameData = null;
        public static GameOptions Options = null;
        public static GameManager GameMgr = null;
        public static NetworkManager Network = null;
        public static GameListManager GameListMgr = null;
        public static SceneManager SceneMgr = null;
        public static ResourceManager ResMgr = null;
        public static PopupManager PopupMgr = null;
        public static VersionManager Updater = null;
        public static ErrorReporter ErrorRpt = null;
        public static MarqueeManager MarqueeMgr = null;
        public static ModuleManager ModuleMgr = null;
        public static ServerListManager SvrListMgr = null;

        public static AccountService Account = null;
        public static GameService GameSrv = null;
        public static BackendService BackendSrv = null;

        public static GameApp GetInstance()
        {
            if (app == null)
            {
                app = new GameApp();
            }

            return app;
        }

        public GameApp()
        {
            ResMgr = UnitySingleton<ResourceManager>.Instance;
            Network = UnitySingleton<NetworkManager>.Instance;
            SceneMgr = UnitySingleton<SceneManager>.Instance;
            BackendSrv = UnitySingleton<BackendService>.Instance;
            Updater = UnitySingleton<VersionManager>.Instance;
            SvrListMgr = UnitySingleton<ServerListManager>.Instance;

            ErrorRpt = new ErrorReporter();
            PopupMgr = new PopupManager();
            Settings = new GameSettings();
            GameData = new GameRuntimeData();
            GameMgr = new GameManager();
            GameListMgr = new GameListManager();
            MarqueeMgr = new MarqueeManager();
            ModuleMgr = new ModuleManager();

            Account = new AccountService();
            GameSrv = new GameService();
        }

        public void Initialize()
        {
            try
            {
                /*if (Debug.isDebugBuild)
                {
                    DebugConsole.GetInstance().Show();
                }*/

                //依赖ResourceManager
                //ResMgr.Initialize();

                //以下初始化依赖Options
                ErrorRpt.Initialize();
                GameMgr.Initialize();
                Network.Initialize();
                Account.Initialize();
                GameSrv.Initialize();
                MarqueeMgr.Initialize();
                Updater.Initialize();
                SvrListMgr.Initialize();

                InitializeEnv();

                SceneMgr.Initialize();

                Logger.Sys.Log("App initialized");
            }
            catch (Exception ex)
            {
                Logger.Sys.LogException(ex);
            }

        }

        public void InitializeEnv()
        {
            try
            {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN

                if(!Win32Api.GetInstance().Initialize(
                    Options.ForceSingleton, 
                    Options.FixedWidth, 
                    Options.FixedHeight))
                {
                    Logger.Sys.LogError("init windows api failed!!!!!!!!!!!!!!!!");
                    Quit();
                    return;
                }

                if (Options.FixedSize)
                {
                    Win32Api.GetInstance().ResizeWindow(Options.FixedWidth, 
                        Options.FixedHeight);
                }

#endif
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
                Application.targetFrameRate = Options.FrameRate;
                Application.runInBackground = true;
                Application.backgroundLoadingPriority = ThreadPriority.High;

            }
            catch (Exception ex)
            {
                Logger.Sys.LogException(ex);
            }
        }

        public void SwitchAccount()
        {
            try
            {
                Settings.Save();
                Network.Reset();
                Updater.Reset();
                ModuleMgr.Clear();
                MarqueeMgr.Clear();
                GameListMgr.Clear();
                SvrListMgr.Clear();
                GameMgr.Reset();
                ErrorRpt.Reset();
                // ResMgr.UnloadAll();

#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
                //切换账号
                if (Options.UseLauncher)
                {
                    System.Diagnostics.Process.Start("Launcher.exe", "--nocheck");
			        UnityEngine.Application.Quit();
                }
                else
                {
                    SceneMgr.Initialize();
                }
			
#else
                SceneMgr.Initialize();
#endif

            }
            catch (Exception ex)
            {
                Logger.Sys.LogException(ex);
            }
        }

        public void Quit()
        {
            try
            {
                Settings.Save();
                GameMgr.Reset();
                Network.Reset();

#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
                Win32Api.GetInstance().UnInitialize();
#endif

#if !UNITY_EDITOR
			    //关闭大厅
			    UnityEngine.Application.Quit();
#else
                UnityEngine.Debug.Break();
#endif

            }
            catch (Exception ex)
            {
                Logger.Sys.LogException(ex);
            }
        }

    }
}
