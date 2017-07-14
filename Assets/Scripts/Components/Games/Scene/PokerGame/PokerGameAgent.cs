using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Services.NetFox;
using com.QH.QPGame.Utility;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.QH.QPGame
{
    public enum MsgType
    {
        MT_INFO = 0x0001,
        MT_EJECT = 0x0002,
        MT_GLOBAL = 0x0004,

        MT_CLOSE_ROOM = 0x0100,
        MT_CLOSE_GAME = 0x0200,
        MT_CLOSE_LINK = 0x0400,
    };

    public enum PlayerState
    {
        NULL = 0,
        READY = 1,
        PLAY = 2,
        GIVEUP = 3
    };

    public enum GameState
    {
        GS_FREE = 0,
        GS_PLAYING = 100
    };

    public enum UserState
    {
        US_NULL = 0,
        US_FREE,
        US_SIT,
        US_READY,
        US_LOOKON,
        US_PLAY,
        US_OFFLINE
    };

    public enum TableEvents
    {
        USER_LEAVE = 0,
        USER_COME = 1,
        USER_READY = 2,
        USER_LOOKON = 3,
        USER_OFFLINE = 4,
        USER_PLAY = 5,
        USER_SCORE = 6,
        WAIT_DISTRIBUTE = 7,
        //
        GAME_START = 8,
        GAME_FINISH = 9,
        //
        GAME_ENTER = 10,
        USER_NULL = 11,
        GAME_LOST = 12
    };

    public enum UserGender
    {
        Man = 0,
        Woman = 1
    }


    public delegate void PacketHandle2(ushort protocol, ushort subcmd, Packet packet);
    public delegate void PacketHandle(ushort protocol, ushort subcmd, NPacket packet);
    public delegate void TableEventHandle(TableEvents tevt, uint userid, object data);

    /// <summary>
    /// 抽出来专门为移植过来的各种u3d棋牌服务,简单的减少代码冗余
    /// @Author guofeng
    /// </summary>
    public class PokerGameAgent : SceneGameAgent
    {
        internal struct TableEvent
        {
            public TableEvents Events;
            public uint UserID;
            public object Data;
        }

        private Dictionary<uint, uint> TableUseList = new Dictionary<uint, uint>();

        private Dictionary<ushort, PacketHandle> PacketHandles = new Dictionary<ushort, PacketHandle>();
        private Dictionary<ushort, PacketHandle2> PacketHandles2 = new Dictionary<ushort, PacketHandle2>();
        private IList<TableEvent> TableEventList = new List<TableEvent>();
        private TableEventHandle TableHandle = null;

        protected override void RegisterEvent()
        {
            GameApp.Network.NetworkStatusChangeEvent += Network_NetworkStatusChangeEvent;
            //没有用到
            //GameApp.GameSrv.UserEnterEvent += Instance_UserEnterEvent;
            //GameApp.GameSrv.UserLeftEvent += Instance_UserLeftEvent;

            GameApp.GameSrv.GameCreatedEvent += Instance_GameCreatedEvent;
            GameApp.GameSrv.UserAgreeEvent += Instance_UserAgreeEvent;
            GameApp.GameSrv.UserOfflineEvent += Instance_UserOfflineEvent;
            GameApp.GameSrv.GameStartedEvent += Instance_GameStartedEvent;
            GameApp.GameSrv.GameEndedEvent += Instance_GameEndedEvent;
            GameApp.GameSrv.UserWaitDistributeEvent += Instance_UserWaitDistributeEvent;
            GameApp.GameSrv.UserSitDownEvent += Instance_UserSitDownEvent;
            GameApp.GameSrv.UserStandUpEvent += Instance_UserStandUpEvent;
            GameApp.GameSrv.UserLeftEvent += GameSrv_UserLeftEvent;
            GameApp.GameSrv.GameMessageEvent += Instance_GameMessageEvent;
   
        }

        void GameSrv_UserLeftEvent(uint uid)
        {
            foreach (var item in TableUseList)
            {
                if (item.Value == uid)
                {
                    TableUseList.Remove(item.Key);
                    break;
                }
            }

            PushTableEvent(TableEvents.USER_LEAVE, uid, null);
        }

        protected override void UnRegisterEvent()
        {
            GameApp.Network.NetworkStatusChangeEvent -= Network_NetworkStatusChangeEvent;
            //没有用到
            //GameApp.GameSrv.UserEnterEvent -= Instance_UserEnterEvent;
            //GameApp.GameSrv.UserLeftEvent -= Instance_UserLeftEvent;

            GameApp.GameSrv.GameCreatedEvent -= Instance_GameCreatedEvent;
            GameApp.GameSrv.UserAgreeEvent -= Instance_UserAgreeEvent;
            GameApp.GameSrv.UserOfflineEvent -= Instance_UserOfflineEvent;
            GameApp.GameSrv.GameStartedEvent -= Instance_GameStartedEvent;
            GameApp.GameSrv.GameEndedEvent -= Instance_GameEndedEvent;
            GameApp.GameSrv.UserWaitDistributeEvent -= Instance_UserWaitDistributeEvent;
            GameApp.GameSrv.UserSitDownEvent -= Instance_UserSitDownEvent;
            GameApp.GameSrv.UserStandUpEvent -= Instance_UserStandUpEvent;
            GameApp.GameSrv.UserLeftEvent -= GameSrv_UserLeftEvent;
            GameApp.GameSrv.GameMessageEvent -= Instance_GameMessageEvent;

        }

        protected override void EnterGameScene()
        {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            if (!Win32Api.GetInstance().IsMaxWindow())
            {
                Win32Api.GetInstance().ResizeWindow(1024, 738);
            }
#endif

            base.EnterGameScene();
        }

        protected override void QuitGameScene()
        {
            base.QuitGameScene();

            TableUseList.Clear();
            PacketHandles.Clear();
            PacketHandles2.Clear();
            TableEventList.Clear();
            TableHandle = null;
        }

        private void Network_NetworkStatusChangeEvent(ConnectionID socket, NetworkManager.Status wError)
        {
            /*if (socket == NetworkManager.ConnectionFlag.Game &&
                wError != NetworkManager.Status.Connected)
            {
                PushTableEvent(TableEvents.GAME_LOST, 0, null);
            }*/
        }

        private void Instance_GameCreatedEvent(uint wRoomId, bool playing)
        {
            //如果是自动坐桌子,需要发送坐下请求到服务器(由于玩家空闲着状态不用加载场景，所以删除（!roomItem.AutoSit && loaded）)
            if (playing || roomItem.AutoSit)
            {
                EnterGameScene();
            }
        }

        private void Instance_UserWaitDistributeEvent()
        {
            PushTableEvent(TableEvents.WAIT_DISTRIBUTE, 0, null);
        }

        private void Instance_GameEndedEvent(uint desk)
        {
            if (desk != GameApp.GameData.UserInfo.DeskNO)
            {
                return;
            }

            PushTableEvent(TableEvents.GAME_FINISH, 0, null);
        }

        private void Instance_GameStartedEvent(uint desk)
        {
            if (desk != GameApp.GameData.UserInfo.DeskNO)
            {
                return;
            }

            var players = GameApp.GameSrv.GetAllPlayers();
            foreach (var item in players)
            {
                if (IsSameTable(MySelf, item))
                {
                    TableUseList[item.DeskStation] = item.ID;
                }
            }

            TableUseList[MySelf.DeskStation] = MySelf.ID;
            PushTableEvent(TableEvents.GAME_START, 0, null);

        }

        private void Instance_UserOfflineEvent(uint uid, ushort desk, ushort chair)
        {
            if (desk != GameApp.GameData.UserInfo.DeskNO)
            {
                return;
            }
            PushTableEvent(TableEvents.USER_OFFLINE, uid, null);
        }

        private void Instance_UserAgreeEvent(ushort desk, ushort chair, ushort agree)
        {
            if (desk != GameApp.GameData.UserInfo.DeskNO)
            {
                return;
            }
            PushTableEvent(TableEvents.USER_READY, 0, null);
        }


        private void Instance_UserSitDownEvent(uint uid, ushort desk, ushort chair)
        {
            if (desk != GameApp.GameData.UserInfo.DeskNO)
            {
                return;
            }

            var players = GameApp.GameSrv.GetAllPlayers();
            foreach (var item in players)
            {
                if (IsSameTable(MySelf, item))
                {
                    TableUseList[item.DeskStation] = item.ID;
                }
            }

            TableUseList[chair] = uid;

            if (uid == GameApp.GameData.UserInfo.UserID)
            {
                if (!roomItem.AutoSit)
                {
                    EnterGameScene();
                }
                else
                {
                    SendUserSetting();
                }

                PushTableEvent(TableEvents.GAME_ENTER, uid, null);
            }
            else
            {
                PushTableEvent(TableEvents.USER_COME, uid, null);

            }
        }

        private void Instance_UserStandUpEvent(uint uid)
        {
            foreach (var item in TableUseList)
            {
                if (item.Value == uid)
                {
                    TableUseList.Remove(item.Key);
                    break;
                }
            }

            if (uid == GameApp.GameData.UserInfo.UserID)
            {
               /* if (!AutoSit && loaded)
                {
                    PushTableEvent(TableEvents.USER_NULL, uid, null);
                }*/
            }
            else
            {
                PushTableEvent(TableEvents.USER_LEAVE, uid, null);
            }
        }

        private NPacket ConvertToNPacket(Packet packet)
        {
            var np = NPacketPool.GetEnablePacket();
            np.CreateHead((ushort) packet.MainCmd, (ushort) packet.SubCmd);
            np.MessageVer = (byte) packet.Version;
            np.DataSize = (ushort) packet.DataSize;
            if (packet.DataSize > 0 && packet.Data != null)
            {
                np.AddBytes(packet.Data, packet.DataSize);
            }
            return np;
        }

        private void Instance_GameMessageEvent(Packet packet)
        {
            if (PacketHandles2.ContainsKey((ushort)packet.MainCmd))
            {
                PacketHandles2[(ushort)packet.MainCmd]((ushort)packet.MainCmd, (ushort)packet.SubCmd, packet);
            }
            else
            {
                NPacket np = ConvertToNPacket(packet);
                if (PacketHandles.ContainsKey(np.PacketId))
                {
                    PacketHandles[np.PacketId](np.PacketId, np.SubCmd, np);
                }
                NPacketPool.DropPacket(np);
            }
        }


        //--------------逻辑处理映射----------------
        public void AddPacketHandle(ushort protocol, PacketHandle handle)
        {
            lock (this)
            {
                if (!PacketHandles.ContainsKey(protocol))
                {
                    PacketHandles.Add(protocol, handle);
                }
                else
                {
                    PacketHandles[protocol] = handle;
                }
            }

        }

        public void AddPacketHandle2(ushort protocol, PacketHandle2 handle)
        {
            lock (this)
            {
                if (!PacketHandles2.ContainsKey(protocol))
                {
                    PacketHandles2.Add(protocol, handle);
                }
                else
                {
                    PacketHandles2[protocol] = handle;
                }
            }

        }

        public void Send(NPacket pt)
        {
            GameApp.GameSrv.SendToGameSvr(pt.Buff, pt.DataSize);

            //TODO fix it!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            NPacketPool.DropPacket(pt);
        }

        public void Send(uint wMainCmd, uint wSubCmdID, int wHandleCode, byte[] wByteBuffer)
        {
            GameApp.GameSrv.SendToGameSvr(wMainCmd, wSubCmdID, wHandleCode, wByteBuffer);
        }

        public void SendUserSitdown()
        {
            GameApp.GameSrv.SendUserSitDown(
                roomItem.ID,
                CommonDefine.INVALID_TABLE,
                CommonDefine.INVALID_CHAIR,
                GameApp.GameData.Password
                );
        }

        //换桌
        public void SendUserSwitchDesk()
        {
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCommand.MDM_GF_FRAME, SubCommand.SUB_GF_USER_READY);
            Send(packet);
        }

        public void SendUserReadyReq()
        {
            //用户准备
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCommand.MDM_GF_FRAME, SubCommand.SUB_GF_USER_READY);
            Send(packet);
        }

        public void SendUserSetting()
        {
            //是否允许旁观设置
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCommand.MDM_GF_FRAME, SubCommand.SUB_GF_INFO);
            packet.Addbyte(0);
            packet.AddUInt(0);
            packet.AddUInt(0);
            Send(packet);
        }

        public void SendChatMessage(uint dwTargetID, string strMsg, uint emotion)
        {
            //发送消息
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCommand.MDM_GF_FRAME, SubCommand.SUB_GF_USER_CHAT);
            packet.AddUShort((ushort)StringHelper.GetStringLength(strMsg));
            packet.AddUInt(emotion);
            packet.AddUInt(MySelf.ID);
            packet.AddUInt(dwTargetID);
            packet.AddString(strMsg, StringHelper.GetStringLength(strMsg));
            Send(packet);
        }

        public void SendNoticeMessage(string strMsg)
        {
            //发送消息
            NPacket packet = NPacketPool.GetEnablePacket();
            packet.CreateHead(MainCommand.MDM_GR_SYSTEM, SubCommand.SUB_GR_MESSAGE);
            packet.AddUShort((ushort) 0);
            packet.AddUShort((ushort) strMsg.Length);
            packet.AddString(strMsg, StringHelper.GetStringLength(strMsg));
            Send(packet);
        }

        public bool IsSameTable(PlayerInfo left, PlayerInfo right)
        {
            if (left.DeskNO == CommonDefine.INVALID_TABLE) return false;
            if (left.ID == right.ID) return false;
            if (left.DeskNO == right.DeskNO) return true;
            return false;
        }

        public byte UserIdToChairId(uint userid)
        {
            for (byte i = 0; i < roomItem.DeskPeople; i++)
            {
                if (TableUseList.ContainsKey(i))
                {
                    uint uid = TableUseList[i];

                    if (uid == userid)
                    {
                        return i;
                    }
                }
            }                    


            // return CommonDefine.INVALID_CHAIR;
            return 0xFF;
        }

        public PlayerInfo EnumTablePlayer(uint index)
        {
            if (!TableUseList.ContainsKey(index))
            {
                return null;
            }

            uint uid = TableUseList[index];
            return GameApp.GameSrv.FindPlayer(uid);
        }

        public PlayerInfo GetTableUserItem(ushort wChairID)
        {
            if (TableUseList.ContainsKey(wChairID))
            {
                uint uid = TableUseList[wChairID];
                return GameApp.GameSrv.FindPlayer(uid);
            }
            return null;
        }

        public PlayerInfo MySelf
        {
            get { return GameApp.GameSrv.FindMe(); }
        }

        public void SetTableEventHandle(TableEventHandle handle)
        {
            lock (this)
            {
                TableHandle = handle;
            }

            DispatchTableEvent();
        }

        private void PushTableEvent(TableEvents events, uint userid, object data)
        {
            TableEventList.Add(new TableEvent() { Events = events, UserID = userid, Data = data });

            DispatchTableEvent();
        }

        private void DispatchTableEvent()
        {
            if (TableHandle == null)
            {
                return;
            }

            foreach (var evt in TableEventList)
            {
                TableHandle(evt.Events, evt.UserID, evt.Data);
            }

            TableEventList.Clear();
        }
    }
}
