using System;
using System.Collections.Generic;
using System.Reflection;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.Lobby
{
    public interface IGameAgent
    {
        //启动一个代理
        bool Start(SGameRoomItem room);

        //停止一个代理,退出当前房间
        void Stop();

        //退出当前游戏
        void Quit();

        //判断是否正在游戏中(客户端逻辑,也就是游戏进程还在)
        bool IsRunning();

        //判断是否正在游戏中(服务器逻辑,游戏开始到游戏结束都是真),主要用于限制不可多开
        bool IsPlaying();

        //激活
        bool Active();
    }


    /// <summary>
    /// 游戏控制器,控制游戏进程或游戏上下文
    /// @Author: guofeng
    /// </summary>
    public class GameManager
    {
        private List<IGameAgent> mGameAgentList = new List<IGameAgent>();
        private Dictionary<int, Type> mGameAssemblies = new Dictionary<int, Type>();
        private Dictionary<int, GameConfig> gamesConfig = new Dictionary<int, GameConfig>();

        public bool Initialize()
        {
            Logger.Sys.Log("GameManager Init");

            Reset();
            
            LoadGameConfig();
            ScanGameClasses();

            return true;
        }

        public void Reset()
        {
            DestoryGame(true);
            mGameAssemblies.Clear();
            mGameAgentList.Clear();
            gamesConfig.Clear();
        }

        public void LoadGameConfig()
        {
            string text = GameApp.ResMgr.GetCachedTextFile(GlobalConst.Res.GameConfigFileName);
            if (string.IsNullOrEmpty(text))
            {
                Logger.Sys.LogError("empty game config file");
                return;
            }

            var dataList = LitJson.JsonMapper.ToObject<List<GameConfig>>(text);
            foreach (var config in dataList)
            {
                config.NeedUpdate = false;
                config.Installed = config.Packed;
                config.SceneName = System.IO.Path.GetFileNameWithoutExtension(config.SceneName);

                foreach (var ab in config.AssetBundles)
                {
                    if (!config.Packed)
                    {
                        config.Installed = GameApp.ResMgr.IsResDownloaded(ab);
                        if (!config.Installed)
                        {
                            //Logger.Sys.Log("the game is not installed.file:"+ab);
                            break;
                        }
                    }

                    if (GameApp.ResMgr.IsResNeedsUpdate(ab))
                    {
                        Logger.Sys.Log("the game is needs update.file:" + ab);
                        config.NeedUpdate = true;
                        break;
                    }
                }
                Logger.Sys.Log("add game:" + config.ID+" Installed:"+config.Installed+" UpdateNeeds:"+config.NeedUpdate);
                gamesConfig.Add(config.ID, config);
            }
        }

        public void UpdateExistsGameConfig(string text)
        {
            var fields = typeof (GameConfig).GetFields(BindingFlags.Instance | BindingFlags.Public);
            var remoteGames = LitJson.JsonMapper.ToObject<List<GameConfig>>(text);
            foreach (var config in remoteGames)
            {
                if (!gamesConfig.ContainsKey(config.ID))
                {
                    continue;
                }

                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];
                    if(field.IsDefined(typeof(NoUpdateNeededAttribute), false))
                    {
                        continue;
                    }

                    var value = field.GetValue(config);
                    field.SetValue(gamesConfig[config.ID], value);
                }
            }
        }

        public void ScanGameClasses()
        {
            try
            {
                foreach (var gameConfig in gamesConfig)
                {
                    var type = Type.GetType(gameConfig.Value.Assembly, true);
                    mGameAssemblies.Add(gameConfig.Key, type);
                }
            }
            catch (Exception e)
            {
                Logger.Sys.LogException(e);
            }
        }


        public void SaveGameConfig()
        {
            var data = new GameConfig[gamesConfig.Count];
            gamesConfig.Values.CopyTo(data, 0);
            string text = LitJson.JsonMapper.ToJson(data);
            GameApp.ResMgr.SaveTextFile(GlobalConst.Res.GameConfigFileName, text);
        }

        public bool IsGameExists(int gameID)
        {
            return gamesConfig.ContainsKey(gameID);
        }

        public bool IsGameInstalled(int gameID)
        {
            if (!gamesConfig.ContainsKey(gameID))
            {
                return false;
            }

            var config = gamesConfig[gameID];
            return config.Installed;
        }

        public bool IsGameNeedUpdate(int gameID)
        {
            if (!gamesConfig.ContainsKey(gameID))
            {
                return false;
            }

            var config = gamesConfig[gameID];
            return config.NeedUpdate;
        }

        public GameConfig GetGameConfig(int gameID)
        {
            if (!gamesConfig.ContainsKey(gameID))
            {
                return null;
            }

            return gamesConfig[gameID];
        }

        public bool CreateGame(SGameRoomItem item)
        {
            Logger.Sys.Log("Create game:" + item.GameNameID);

            var agent = GetARunningAgent();
            if (agent != null)
            {
                return false;
            }

            DestoryAgents();
            //ClearPlayers();
            GC.Collect();

            agent = CreateGameAgent(item);
            if (agent == null)
            {
                return false;
            }

            mGameAgentList.Add(agent);
            if (!agent.Start(item))
            {
                return false;
            }


            return true;
        }

        public void DestoryGame(bool force)
        {
            if (!force)
            {
                var agent = GetARunningAgent();
                if (agent != null)
                {
                    agent.Quit();
                    return;
                    //mAgentManager.StopAndRemoveAgent(agent);
                }
            }

            DestoryAgents();

            //ClearPlayers();

            //GameApp.GameData.EnterRoomItem = null;
            /*if (GameApp.GameData.UserInfo != null)
            {
                GameApp.GameData.UserInfo.SetSitInfo(CommonDefine.INVALID_TABLE,
                    CommonDefine.INVALID_CHAIR);
            }*/

            GC.Collect();
        }

        public void ActiveGame()
        {
            var agent = GetARunningAgent();
            if (agent == null)
            {
                return;
            }

            agent.Active();
        }

        public bool IsInGame()
        {
            return GetAPlayingAgent() != null;
        }

        private IGameAgent CreateGameAgent(SGameRoomItem item)
        {
            IGameAgent agent = null;
            switch (item.HostType)
            {
                case enGameHostType.IPC:
                    {
                        break;
                    }
                case enGameHostType.Scene:
                    {
                        string strPrefix = item.GameNameID.ToString();
                        if (strPrefix.Length > 6)
                        {
                            strPrefix = strPrefix.Substring(0, 6);
                        }

                        int prefix = int.Parse(strPrefix);
                        if (mGameAssemblies.ContainsKey(prefix))
                        {
                            var impl = mGameAssemblies[prefix];
                            var obj = Activator.CreateInstance(impl);
                            agent = obj as IGameAgent;
                        }
                        else
                        {
                            throw new NotImplementedException("no implemented found by gameid:" + item.GameNameID);
                        }

                        break;
                    }
                case enGameHostType.Socket:
                    {
                        break;
                    }
            }

            return agent;
        }

        private void StopAndRemoveAgent(IGameAgent agent)
        {
            foreach (var agentItem in mGameAgentList)
            {
                if (agent == agentItem)
                {
                    agent.Stop();
                    mGameAgentList.Remove(agent);
                    break;
                }
            }
        }

        private void DestoryAgents()
        {
            foreach (var agent in mGameAgentList)
            {
                agent.Stop();
            }

            mGameAgentList.Clear();
        }


        public IGameAgent GetARunningAgent()
        {
            foreach (var agent in mGameAgentList)
            {
                if (agent.IsRunning())
                {
                    return agent;
                }
            }

            return null;
        }

        public IGameAgent GetAPlayingAgent()
        {
            foreach (var agent in mGameAgentList)
            {
                if (agent.IsPlaying())
                {
                    return agent;
                }
            }

            return null;
        }

        private bool HasPlayingAgent()
        {
            return GetAPlayingAgent() != null;
        }






    }
}
