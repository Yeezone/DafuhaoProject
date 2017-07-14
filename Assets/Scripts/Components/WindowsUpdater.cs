using System;
using System.Collections;
using System.Text;

using LitJson;
using UnityEngine;

using com.QH.QPGame.Utility;
using System.IO;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// windows版本更新器,目前采用整包更新,调用外部的update.exe实现更新逻辑
    /// @Author: guofeng
    /// </summary>
    public class WindowsUpdater
    {
        public static bool Run(string url, int version)
        {
            //TODO 加载update.exe.bak并重命名
#if (!UNITY_EDITOR || TEST_UPDATE) && UNITY_STANDALONE_WIN
            try
            {
                StringBuilder str = new StringBuilder(url);
                str.Append(" ");
                str.Append(version);
                str.Append(" ");
                str.Append(DateTime.Now.Ticks / GlobalConst.Update.PrecisionOfDownload);

                string args = str.ToString();
                string current = Win32Api.GetExePath();

                Debug.Log("Path: " + current);
                Debug.Log("Url: " + args);

                string fileName = current + "/SurfPatch.exe";
                if (File.Exists(fileName+".tmp"))
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    File.Move(fileName + ".tmp", fileName);
                }

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //mGameProcess.EnableRaisingEvents = true;
                //mGameProcess.Exited += new EventHandler(OnGameCrashed);
                process.StartInfo.WorkingDirectory = current;
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = args;
                process.Start();

                //process.WaitForInputIdle();

                GameApp.GetInstance().Quit();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
#endif
            return true;
        }
    }
}
