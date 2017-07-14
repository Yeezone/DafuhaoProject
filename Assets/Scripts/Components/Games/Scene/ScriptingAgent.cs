using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.QH.QPGame;

namespace com.QH.QPGame.GameCore
{
    /// <summary>
    /// 脚本游戏代理,为各种脚本游戏提供宿主接入游戏平台
    /// @Author guofeng
    /// </summary>
    public class ScriptingAgent : SceneGameAgent
    {
        //private LuaState luaState;

        protected override void RegisterEvent()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void UnRegisterEvent()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
