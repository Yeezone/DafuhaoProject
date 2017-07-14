using System;
using System.Collections.Generic;

namespace com.QH.QPGame.Services.Data
{
    [Serializable]
    public class GameUIConfig
    {
        public string Name;
        public bool Activation;

        public Dictionary<string, string> CustomData;

        public Dictionary<string, GameUIConfig> SubConfigMap;

        /*public Vector3 Position;
        public Vector3 Scale;*/
    }

    public enum ModuleKind
    {
        Base,
        UI,
        Game
    }

    [Serializable]
    public class ModuleInfo
    {
        public int ID;
        public ModuleKind K;
        //Status
        public int S;
        public int[] Sub;
        //public Dictionary<string, string> CustomData;
    }
}

