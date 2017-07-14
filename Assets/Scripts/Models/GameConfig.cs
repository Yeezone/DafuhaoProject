using System;
using System.Collections.Generic;

namespace com.QH.QPGame.Services.Data
{
    public class NoUpdateNeededAttribute : Attribute
    {
    }

    /// <summary>
    /// 从单一文件改成从每个游戏一个文件
    /// </summary>
    [Serializable]
    public class GameConfig
    {
        [NoUpdateNeeded]
        public int ID;
        public string Version;
        public string Assembly;
        public string SceneName;
        public string[] AssetBundles;

        [NoUpdateNeeded]
        public bool Packed;

        /*[NoUpdateNeeded]
        public Dictionary<string, object> CustomData;*/

        [NoUpdateNeeded]
        [NonSerialized]
        public bool Installed;

        [NoUpdateNeeded]
        [NonSerialized]
        public bool NeedUpdate;

    }

}
