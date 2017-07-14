using System;
using System.Collections.Generic;
using com.QH.QPGame.Services;
using com.QH.QPGame.Services.NetFox;
using com.QH.QPGame.Services.Utility;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 游戏服务,提供与游戏服务器通信的接口,为逻辑层屏蔽具体的平台，以及作为数据的缓存管理
    /// @Author: guofeng
    /// </summary>
    public class GameService
    {
        #region 事件

        public delegate void LogonResultHandler();
        public event LogonResultHandler GameLogonSuccessEvent;

        public delegate void GameLogonErrorHandler(int wHandleCode, string desc);
        public event GameLogonErrorHandler GameLogonErrorEvent;

        public delegate void UserReloginGameRoomHandler(UInt32 lastRoom, bool tips);
        public event UserReloginGameRoomHandler UserReloginGameRoomEvent;

        public delegate void GameCreatedHandler(UInt32 wRoomId, bool playing);
        public event GameCreatedHandler GameCreatedEvent;

        public delegate void GameStartedHandler(UInt32 desk);
        public event GameStartedHandler GameStartedEvent;

        public delegate void UserEnterHandler(PlayerInfo wUserInfo);
        public event UserEnterHandler UserEnterEvent;

        public delegate void UserLeftHandler(UInt32 wUserId);
        public event UserLeftHandler UserLeftEvent;

        public delegate void UserSitDownHandler(uint uid, ushort desk, ushort chairs);
        public event UserSitDownHandler UserSitDownEvent;

        public delegate void UserSitDownErrorHandler(int handlerCode, string Msg);
        public event UserSitDownErrorHandler UserSitErrorEvent;

        public delegate void UserStandUpHandler(UInt32 wUserId);
        public event UserStandUpHandler UserStandUpEvent;

        public delegate void UserAgreeHandler(ushort desk, ushort chairs, ushort agree);
        public event UserAgreeHandler UserAgreeEvent;

        public delegate void UserOfflineHandler(UInt32 uid, ushort desk, ushort chair);
        public event UserOfflineHandler UserOfflineEvent;

        public delegate void UserWaitDistributeHandler();
        public event UserWaitDistributeHandler UserWaitDistributeEvent;

        public delegate void UserInfoUpdateHandler();
        public event UserInfoUpdateHandler GameUserUpdatedEvent;

        public delegate void GameEndedHandler(UInt32 desk);
        public event GameEndedHandler GameEndedEvent;

        public delegate void DeskPlayStatuseHandler(UInt32 desk, byte Statuse);
        public event DeskPlayStatuseHandler DeskPlayStatuseEvent;

        public delegate void GameMessageHandler(Packet packet);
        public event GameMessageHandler GameMessageEvent;

        public delegate void SystemMessageHandler(UInt16 wType, string msg);
        public event SystemMessageHandler SystemMessageEvent;


        #endregion

        #region 事件调用函数

        #region MDM_GR_LOGON
        //游戏登录成功
        public bool CallGameLogonSuccessEvent()
        {
            if (GameLogonSuccessEvent != null)
            {
                GameLogonSuccessEvent();
                return true;
            }
            return false;
        }

        //游戏登录失败
        public bool CallGameLogonErrorEvent(int wHandleCode, string desc)
        {
            if (GameLogonErrorEvent != null)
            {
                GameLogonErrorEvent(wHandleCode, desc);
                return true;
            }
            return false;
        }

        //玩家被锁房间
        public bool CallUserReloginGameRoomEvent(UInt32 lastRoom, bool tips)
        {
            if (UserReloginGameRoomEvent != null)
            {
                UserReloginGameRoomEvent(lastRoom, tips);
                return true;
            }
            return false;
        }

        //创建场景
        public bool CallGameCreatedEvent(UInt32 wRoomId, bool playing)
        {
            if (GameCreatedEvent != null)
            {
                GameCreatedEvent(wRoomId, playing);
                return true;
            }
            return false;
        }

        //游戏开始
        public bool CallGameStartedEvent(UInt32 desk)
        {
            if (GameStartedEvent != null)
            {
                GameStartedEvent(desk);
                return true;
            }
            return false;
        }

        #endregion

        #region MDM_GR_USER
        //用户进入
        public bool CallUserEnterEvent(PlayerInfo wUserInfo)
        {
            if (UserEnterEvent != null)
            {
                UserEnterEvent(wUserInfo);
                return true;
            }
            return false;
        }

        //用户离开
        public bool CallUserLeftEvent(UInt32 wUserId)
        {
            if (UserLeftEvent != null)
            {
                UserLeftEvent(wUserId);
                return true;
            }
            return false;
        }

        //坐下
        public bool CallUserSitDownEvent(uint uid, ushort desk, ushort chairs)
        {
            if (UserSitDownEvent != null)
            {
                UserSitDownEvent(uid, desk, chairs);
                return true;
            }
            return false;
        }

        //坐下出错
        public bool CallUserSitErrorEvent(int handlerCode, string Msg)
        {
            if (UserSitErrorEvent != null)
            {
                UserSitErrorEvent(handlerCode, Msg);
                return true;
            }
            return false;
        }

        //玩家起立
        public bool CallUserStandUpEvent(UInt32 wUserId)
        {
            if (UserStandUpEvent != null)
            {
                UserStandUpEvent(wUserId);
                return true;
            }
            return false;
        }

        //玩家同意
        public bool CallUserAgreeEvent(ushort desk, ushort chairs, ushort agree)
        {
            if (UserAgreeEvent != null)
            {
                UserAgreeEvent(desk, chairs, agree);
                return true;
            }
            return false;
        }

        //玩家断线
        public bool CallUserOfflineEvent(UInt32 uid, ushort desk, ushort chair)
        {
            if (UserOfflineEvent != null)
            {
                UserOfflineEvent(uid, desk, chair);
                return true;
            }
            return false;
        }

        //排队
        public bool CallUserWaitDistributeEvent()
        {
            if (UserWaitDistributeEvent != null)
            {
                UserWaitDistributeEvent();
                return true;
            }
            return false;
        }

        //玩家数据
        public bool CallGameUserUpdatedEvent()
        {
            if (GameUserUpdatedEvent != null)
            {
                GameUserUpdatedEvent();
                return true;
            }
            return false;
        }
        #endregion

        #region MDM_GR_STATUS
        //游戏结束
        public bool CallGameEndedEvent(UInt32 desk)
        {
            if (GameEndedEvent != null)
            {
                GameEndedEvent(desk);
                return true;
            }
            return false;
        }

        //桌上玩家状态
        public bool CallDeskPlayStatuseEvent(UInt32 desk, byte Statuse)
        {
            if (DeskPlayStatuseEvent != null)
            {
                DeskPlayStatuseEvent(desk, Statuse);
                return true;
            }
            return false;
        }
        #endregion

        #region MDM_GF_GAME
        //游戏消息
        public bool CallGameMessageEvent(Packet packet)
        {
            if (GameMessageEvent != null)
            {
                GameMessageEvent(packet);
                return true;
            }
            return false;
        }

        //系统消息
        public bool CallSystemMessageEvent(UInt16 wType, string msg)
        {
            if (SystemMessageEvent != null)
            {
                SystemMessageEvent(wType, msg);
                return true;
            }
            return false;
        }
        #endregion

        #endregion

        private Dictionary<UInt32, PlayerInfo> mRoomPlayers = new Dictionary<uint, PlayerInfo>();
        //private Dictionary<UInt32, PlayerInfo> mTablePlayers = new Dictionary<uint, PlayerInfo>();

        public void Initialize()
        {
            GameApp.Network.NetworkStatusChangeEvent += Network_NetworkStatusChangeEvent;

            GameApp.Network.RegisterHandler(ConnectionID.Game, MainCommand.MDM_GR_LOGON, GameProtocol.Instance.OnLogonResp);
            GameApp.Network.RegisterHandler(ConnectionID.Game, MainCommand.MDM_GR_USER, GameProtocol.Instance.OnUserResp);
            GameApp.Network.RegisterHandler(ConnectionID.Game, MainCommand.MDM_GR_STATUS, GameProtocol.Instance.OnStatusResp);
            GameApp.Network.RegisterHandler(ConnectionID.Game, MainCommand.MDM_GF_GAME, GameProtocol.Instance.OnGameResp);
            GameApp.Network.RegisterHandler(ConnectionID.Game, MainCommand.MDM_GF_FRAME, GameProtocol.Instance.OnGameResp);
            GameApp.Network.RegisterHandler(ConnectionID.Game, MainCommand.MDM_KN_COMMAND, GameProtocol.Instance.OnCommandResp);
            GameApp.Network.RegisterHandler(ConnectionID.Game, MainCommand.MDM_GR_CONFIG, GameProtocol.Instance.OnConfigResp);
        }

        void Network_NetworkStatusChangeEvent(ConnectionID socket, NetworkManager.Status wError)
        {
            if (socket == ConnectionID.Game)
            {
                if (wError == NetworkManager.Status.Connected)
                {
                    ClearPlayers();
                    //连接成功后登陆游戏服
                    var item = GameApp.GameListMgr.FindRoomItem(GameApp.GameData.EnterRoomID);
                    if (item == null)
                    {
                        return;
                    }

                    uint uid = GameApp.GameData.UserInfo.UserID;
                    string pwd = GameApp.GameData.Password;
                    string mac = GameApp.GameData.MAC;
                    GameApp.GameSrv.SendLoginGameSvr(item.GameNameID, uid, pwd, mac);
                }
                else
                {
                    GameApp.GameData.UserInfo.SetSitInfo(CommonDefine.INVALID_TABLE, CommonDefine.INVALID_CHAIR);
                }

            }
            
        }

        public void AddPlayer(PlayerInfo player)
        {
            if (!mRoomPlayers.ContainsKey(player.ID))
            {
                mRoomPlayers.Add(player.ID, player);
            }
        }

        public void RemovePlayer(uint uid)
        {
            if (mRoomPlayers.ContainsKey(uid))
            {
                mRoomPlayers.Remove(uid);
            }
        }

        public void ClearPlayers()
        {
            mRoomPlayers.Clear();
        }

        public PlayerInfo FindPlayer(uint uid)
        {
            if (mRoomPlayers.ContainsKey(uid))
            {
                return mRoomPlayers[uid];
            }

            return null;
        }

        public void SetPlayerState(uint pid, ushort table, ushort chair, byte status)
        {
            var player = FindPlayer(pid);
            if (player != null)
            {
                player.SetState(table, chair, status);
            }

            bool isSelf = (pid == GameApp.GameData.UserInfo.UserID);
            if (isSelf)
            {
                GameApp.GameData.UserInfo.SetSitInfo(table, chair);
            }
        }

        public PlayerInfo FindMe()
        {
            uint uid = GameApp.GameData.UserInfo.UserID;
            return FindPlayer(uid);
        }

        public PlayerInfo[] GetAllPlayers()
        {
            var players = new PlayerInfo[mRoomPlayers.Values.Count];
            mRoomPlayers.Values.CopyTo(players, 0);
            return players;
        }

        #region 请求相关函数

        public void SendToGameSvr(byte[] wByteBuffer, int len)
        {
            GameApp.Network.SendToSvr(ConnectionID.Game, wByteBuffer, len);
        }

        public void SendToGameSvr(uint wMainCmd, uint wSubCmdID, int wHandleCode, byte[] wByteBuffer)
        {
            GameApp.Network.SendToSvr(ConnectionID.Game, wMainCmd, wSubCmdID, wHandleCode, wByteBuffer);
        }

        public void SendLoginGameSvr(uint nid, uint uid, string pwd, string mac)
        {
            var logon = new CMD_GR_LogonUserID();
            logon.dwUserID = uid;
            logon.szMachineID = mac;
            logon.szPassword = pwd;
            logon.wKindID = (ushort)nid;

            byte[] dataBuffer = GameConvert.StructToByteArray(logon);
            GameApp.Network.SendToSvr(ConnectionID.Game, MainCommand.MDM_GR_LOGON, SubCommand.SUB_GR_LOGON_USERID, 0, dataBuffer);
        }

        public void SendUserSitDown(UInt32 RoomID, UInt32 Desk, UInt32 Chair, string pwd)
        {
            var sitdown = new CMD_GR_UserSitDown();
            sitdown.szPassword = pwd;
            sitdown.wChairID = (ushort)Chair;
            sitdown.wTableID = (ushort)Desk;

            byte[] dataBuffer = GameConvert.StructToByteArray(sitdown);
            GameApp.Network.SendToSvr(ConnectionID.Game, MainCommand.MDM_GR_USER, SubCommand.SUB_GR_USER_SITDOWN, 0, dataBuffer);
        }

        public void SendUserUp(ushort table, ushort chair, bool force)
        {
            var standup = new CMD_GR_UserStandUp();
            standup.wTableID = table;
            standup.wChairID = chair;
            standup.cbForceLeave = (byte)(force ? 1 : 0);

            byte[] dataBuffer = GameConvert.StructToByteArray(standup);
            GameApp.Network.SendToSvr(ConnectionID.Game, MainCommand.MDM_GR_USER, SubCommand.SUB_GR_USER_STANDUP, 0, dataBuffer);
        }

        #endregion
    }
}
