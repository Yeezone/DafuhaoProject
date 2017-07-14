using System;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Utility;
using UnityEngine;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 设置管理类,保存读取设置,一般的只保存玩家客户端临时数据
    /// @Author: guofeng
    /// </summary>
    public class GameSettings
    {
        public void Load()
        {
            string account = PlayerPrefs.GetString(GlobalConst.Settings.Account);
            string password = PlayerPrefs.GetString(GlobalConst.Settings.Password);
            string saveword = PlayerPrefs.GetString(GlobalConst.Settings.SavePass);
            //string version = PlayerPrefs.GetString(GlobalConst.Settings.Version);
            string installVersion = PlayerPrefs.GetString(GlobalConst.Settings.InstallVersion);

            GameApp.GameData.Account = account;
            GameApp.GameData.Password = password;
            GameApp.GameData.InstallVersion = installVersion;

            bool.TryParse(saveword, out GameApp.GameData.SavePassword);
        }

        public void Save()
        {
            PlayerPrefs.SetString(GlobalConst.Settings.Account, GameApp.GameData.Account);
            PlayerPrefs.SetString(GlobalConst.Settings.SavePass, GameApp.GameData.SavePassword.ToString());
            //PlayerPrefs.SetString(GlobalConst.Settings.Version, GameApp.GameData.Version);
            PlayerPrefs.SetString(GlobalConst.Settings.InstallVersion, GameApp.GameData.Version);

            if (GameApp.GameData.SavePassword)
            {
                PlayerPrefs.SetString(GlobalConst.Settings.Password, GameApp.GameData.Password);
            }
            else
            {
                PlayerPrefs.DeleteKey(GlobalConst.Settings.Password);
            }
        }
    }
}


