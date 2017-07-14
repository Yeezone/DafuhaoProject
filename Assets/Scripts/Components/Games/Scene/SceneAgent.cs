using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services.NetFox;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.Data;
using UnityEngine;
using System;

namespace com.QH.QPGame
{
    /// <summary>
    /// 场景游戏客户端,场景游戏继承此类,加上GameDescAttribute属性描述支持的游戏
    /// @Author guofeng
    /// </summary>
    public abstract class SceneGameAgent : IGameAgent
    {
        protected SGameRoomItem roomItem = null;

        public uint GameID
        {
            get
            {
                return roomItem != null ? roomItem.GameNameID : 0;
            }
        }

        public bool AutoSit
        {
            get
            {
                return roomItem != null ? roomItem.AutoSit : false;
            }
        }

    
        private Vector2 oriSize = Vector2.zero;
        protected bool loaded = false;

        protected static SceneGameAgent instance = null;
 

        #region IGameAgent implementation

        public bool Start(SGameRoomItem room)
        {
            oriSize = new Vector2(Screen.width, Screen.height);

            instance = this;
            roomItem = room;
            GameApp.GameData.EnterRoomID = roomItem.ID;

            RegisterEvent();

            var ip = GameApp.Options.EnableServerList ?
                GameApp.SvrListMgr.PickALowestDelayServer() : roomItem.ServiceIP;
            GameApp.Network.Connect(ConnectionID.Game, ip, roomItem.ServicePort);
            return true;
        }

        public void Stop()
        {
            UnRegisterEvent();
            GameApp.Network.CloseConnect(ConnectionID.Game);

            if (loaded)
            {
                QuitGameScene();
            }

            var config = GameApp.GameMgr.GetGameConfig((int)GameID);
            if (config != null)
            {
                string scene = string.Format(GlobalConst.UI.UI_SCENE_GAME, GameID, config.SceneName);
                GameApp.ResMgr.UnloadLevel(scene, true);
            }
        
            GameApp.GameSrv.ClearPlayers();
            GameApp.GameData.UserInfo.SetSitInfo(CommonDefine.INVALID_TABLE, CommonDefine.INVALID_CHAIR);
            GameApp.GameData.EnterRoomID = 0;

            Resources.UnloadUnusedAssets();

            instance = null;
            roomItem = null;
        }

        public virtual void Quit()
        {
            PlayerInfo player = GameApp.GameSrv.FindMe();
            if (player.UserState > (byte)UserState.US_FREE)
            {
                GameApp.GameSrv.SendUserUp(
                                   player.DeskNO,
                                   player.DeskStation,
                                   (player.UserState == (byte)UserState.US_PLAY)
                                   );
            }
            if (AutoSit)
            {
                GameApp.GameMgr.DestoryGame(true);
            }
            else
            {
                QuitGameScene();
            }
        }

        protected abstract void RegisterEvent();
        protected abstract void UnRegisterEvent();
        //protected abstract void BeforeQuitScene();
        //protected abstract void BeforeSwitchScene();

        protected virtual void EnterGameScene()
        {
            //TODO 从文件中读取配置，从ab中读取场景
            /*string sceneName = GameHelper.FindSceneName(this.GetType(), GameID);
            string loadingName = "UI_Loading_" + sceneName;
            string abName = GlobalConst.AB.GameBasic.Name + GameID;
            GameApp.SceneMgr.EnterScene(sceneName, true, true, loadingName, abName);*/

            var config = GameApp.GameMgr.GetGameConfig((int)GameID);
            if (config != null)
            {
                string loading = string.Format(GlobalConst.UI.UI_GAME_LOADING, GameID);
                string scene = string.Format(GlobalConst.UI.UI_SCENE_GAME, GameID, config.SceneName);
                GameApp.SceneMgr.EnterScene(scene, true, true, loading);
            }

#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            //Win32Api.ImmDisableIME(-1);
#endif

            loaded = true;
        }

        protected virtual void QuitGameScene()
        {
            if (loaded)
            {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
                if (!Win32Api.GetInstance().IsMaxWindow())
                {
                    Win32Api.GetInstance().ResizeWindow((int)oriSize.x, (int)oriSize.y);
                }
                //Win32Api.ImmDisableIME(0);
#endif
                GameApp.SceneMgr.EnterScene(GlobalConst.UI.UI_SCENE_HALL, true, true);
            }

            loaded = false;
        }

        public bool IsRunning()
        {
            return loaded;
        }

        public bool IsPlaying()
        {
            PlayerInfo player = GameApp.GameSrv.FindMe();
            if (player != null)
            {
                return player.UserState == (byte)UserState.US_PLAY;
            }
            return false;
        }

        public bool Active()
        {
            if (!GameApp.Network.IsConnectionVaild(ConnectionID.Game))
            {
                var ip = GameApp.Options.EnableServerList ?
                    GameApp.SvrListMgr.PickALowestDelayServer() : roomItem.ServiceIP;
                GameApp.Network.Connect(ConnectionID.Game, ip, roomItem.ServicePort);
            }

            return true;
        }

        #endregion
    }

}