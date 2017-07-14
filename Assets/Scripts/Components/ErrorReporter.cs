using System;
using System.IO;

using UnityEngine;


namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 错误报告,将崩溃或者严重错误等信息收集然后以UI呈现或者发送到服务器
    /// @Author: guofeng
    /// </summary>
    public class ErrorReporter
    {
        public ErrorReporter()
        {
            Application.logMessageReceivedThreaded += Application_logMessageReceived;
        }

        public void Initialize()
        {
            Application.logMessageReceivedThreaded -= Application_logMessageReceived;

            //(UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS)
#if !DISABLE_BUGLY && !UNITY_EDITOR && (UNITY_ANDROID)
            if (GameApp.Options.EnableBugly)
            {
			    InitBugly();
                return;
            }
#endif
            if (GameApp.Options.EnableLog2File)
            {
                Application.logMessageReceivedThreaded += Application_logMessageReceived;
                RemoteOldFile();
            }
        }

        public void Reset()
        {
            if (!GameApp.Options.EnableLog2File)
            {
                return;
            }

#if !UNITY_EDITOR && !DISABLE_BUGLY && UNITY_ANDROID
            if(GameApp.Options.EnableBugly)
			{
                BuglyAgent.UnregisterLogCallback(Application_logMessageReceived);
                return;
            }
#endif
            UnityEngine.Application.logMessageReceivedThreaded -= Application_logMessageReceived;
        }

        private void InitBugly()
        {
            // 开启SDK的日志打印，发布版本请务必关闭
            BuglyAgent.ConfigDebugMode(Debug.isDebugBuild);
            BuglyAgent.InitWithAppId(GameApp.Options.BuglyAppID);
            BuglyAgent.SetScene(Application.loadedLevel);
            BuglyAgent.SetUserId(Application.companyName + "." + Application.productName);

            if (GameApp.Options.EnableLog2File)
            {
                BuglyAgent.RegisterLogCallback(Application_logMessageReceived);
                RemoteOldFile();
            }
        }

        private void RemoteOldFile()
        {
            string path = Application.persistentDataPath + "/log.txt";
            if (File.Exists(path) && 
                File.GetLastWriteTime(path) < DateTime.Now.AddDays(-1.0f))
            {
                File.Delete(path);
            }
        }

        public void AddFatal(string msg, params object[] args)
        {

        }

        public void AddWarning(string msg, params object[] args)
        {

        }

        public void AddException(Exception ex)
        {
            //Debug.LogException(ex);
/*#if !UNITY_EDITOR && !DISABLE_BUGLY && UNITY_ANDROID
            if(GameApp.Options.EnableBugly)
			{
                BuglyAgent.ReportException(ex, "");
            }
#endif*/
        }

        public static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            /*if (type > GameApp.Options.LogLevel)
            {
                return;
            }*/
            
            string path = UnityEngine.Application.persistentDataPath + "/log.txt";
            File.AppendAllText(path,
                "time:" + DateTime.Now.ToString() +
                "| " + type +
                "| " + condition +
                "|" + stackTrace + "\n"
            );
        }


    }
}
