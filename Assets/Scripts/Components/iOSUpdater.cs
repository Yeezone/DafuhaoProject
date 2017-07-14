using UnityEngine;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// @Author: guofeng
    /// </summary>
    public static class iOSUpdater
    {
        public static bool Run(string url, int version)
        {
            Application.OpenURL(url);
            return true;
        }
    }

}
