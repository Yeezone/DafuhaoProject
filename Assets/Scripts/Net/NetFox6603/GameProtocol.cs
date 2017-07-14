using UnityEngine;

using System;
using System.Runtime.InteropServices;

using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.Utility;

namespace com.QH.QPGame.Services.NetFox
{
    internal class GameProtocol : Singleton<GameProtocol>
    {
        #region 协议解析

        public void OnConfigResp(Packet packet)
        {
            switch (packet.SubCmd)
            {
                case SubCommand.SUB_GR_CONFIG_SERVER:
                    {
                        OnConfigServerResp(packet);
                        break;
                    }
            }
        }

        private void OnConfigServerResp(Packet packet)
        {
            int dataStruct = Marshal.SizeOf(typeof(CMD_GR_ConfigServer));
            if (packet.DataSize < dataStruct)
            {
                Debug.LogWarning("data error!");
                return;
            }

            CMD_GR_ConfigServer config = GameConvert.ByteToStruct<CMD_GR_ConfigServer>(packet.Data);


            SGameRoomItem item = GameApp.GameListMgr.FindRoomItem(GameApp.GameData.EnterRoomID);
            if (item != null)
            {
                item.GameRule = config.dwServerRule;
                item.DeskCount = config.wTableCount;
                item.DeskPeople = config.wChairCount;

                if ((config.dwServerRule & (uint)CommonDefine.SR_ALLOW_AVERT_CHEAT_MODE) > 0)
                {
                    item.AutoSit = true;
                }

                if ((config.wServerType & (uint)CommonDefine.GAME_GENRE_EDUCATE) > 0)
                {
                    item.IsEducate = true;
                }
            }

        }


        public void OnCommandResp(Packet packet)
        {
            GameApp.Network.SendToSvr(ConnectionID.Game, MainCommand.MDM_KN_COMMAND, SubCommand.SUB_KN_DETECT_SOCKET, 0, null);
        }

        public void OnGameResp(Packet packet)
        {
            switch(packet.SubCmd)
            {
                case SubCommand.SUB_GF_MESSAGE:
                    {
                        OnGameMessageResp(packet);
                        break;
                    }
                default:
                    {
                        //Debug.LogWarning("Game Message:"+packet.ToString());
                        GameApp.GameSrv.CallGameMessageEvent(packet);

                        break;
                    }
            }
        }

        private void OnGameMessageResp(Packet packet)
        {
            CMD_CM_SystemMessage mSystemMessage = GameConvert.ByteToStruct<CMD_CM_SystemMessage>(packet.Data);
            GameApp.GameSrv.CallSystemMessageEvent(mSystemMessage.wType, mSystemMessage.szString);

			Debug.Log("CMD_CM_SystemMessage : " + mSystemMessage.szString);
        }

        public void OnStatusResp(Packet packet)
        {
            switch (packet.SubCmd)
            {
                case SubCommand.SUB_GR_TABLE_INFO:
                    {
                        ByteBuffer buffer = ByteBufferPool.PopPacket(packet.Data);
                        ushort cnt = buffer.PopUInt16();
                       for (int i = 0; i < cnt; i++)
                        {
                            //GameTable table = new GameTable();
                            byte TableLock = buffer.PopByte();
                            byte PlayStatus = buffer.PopByte();

                            SRoomDeskItem item = new SRoomDeskItem();
                            item.DeskID = (uint)i;
                            item.Status = PlayStatus;

                            SGameRoomItem room = GameApp.GameListMgr.FindRoomItem(GameApp.GameData.EnterRoomID);
                            if (room != null)
                            {
                                room.Desks.Add(item);
                            }

                            /*  table.ChairFlag = "0".PadRight((int)GameApp.GameData.EnterRoomItem.DeskPeople, '0');
                            table.ChairUsedQty = 0;*/
                        }
                       ByteBufferPool.DropPacket(buffer);

                        break;
                    }
                case SubCommand.SUB_GR_TABLE_STATUS:
                    {
                        ByteBuffer buffer = ByteBufferPool.PopPacket(packet.Data);
                        ushort tableID = buffer.PopUInt16();
                        byte locked = buffer.PopByte();
                        byte status = buffer.PopByte();
                        ByteBufferPool.DropPacket(buffer);

                        SGameRoomItem room = GameApp.GameListMgr.FindRoomItem(GameApp.GameData.EnterRoomID);
                        if (room != null)
                        {
                            SRoomDeskItem item = room.Desks.Find(deskItem => deskItem.DeskID == tableID);
                            if (item != null)
                            {
                                item.DeskID = (uint)tableID;
                                item.Status = status;
                                GameApp.GameSrv.CallDeskPlayStatuseEvent((UInt32)tableID, status);

                            }
                        }
                        if (status == (byte)TableState.TB_FREE)
                        {
                            GameApp.GameSrv.CallGameEndedEvent(tableID);

                        }
                        else
                        {
							Debug.Log("Game Start");
                            GameApp.GameSrv.CallGameStartedEvent(tableID);

                        }

                        break;
                    }
            }
        }

        public void OnUserResp(Packet packet)
        {
            switch (packet.SubCmd)
            {
                case SubCommand.SUB_GR_USER_ENTER:
                    {
                        OnUserEnterResp(packet);
                        break;
                    }
                case SubCommand.SUB_GR_USER_SCORE:
                    {
                        OnUserScoreResp(packet);
                        break;
                    }
                case SubCommand.SUB_GR_USER_STATUS:
                    {
                        OnUserStatusResp(packet);
                        break;
                    }
                case SubCommand.SUB_GR_REQUEST_FAILURE:
                    {
                        CMD_GR_RequestFailure failure = GameConvert.ByteToStruct<CMD_GR_RequestFailure>(packet.Data);
                        GameApp.GameSrv.CallUserSitErrorEvent((int)failure.lErrorCode, failure.szDescribeString);


                        break;
                    }
                case SubCommand.SUB_GR_USER_WAIT_DISTRIBUTE:
                    {
                        OnUserWaitDistribute(packet);
                        break;
                    }
            }
        }

        private void OnUserWaitDistribute(Packet packet)
        {
            GameApp.GameSrv.CallUserWaitDistributeEvent();

        }

        private void OnUserScoreResp(Packet packet)
        {
            int dataStruct = Marshal.SizeOf(typeof(CMD_GR_UserScore));
            if (packet.DataSize < dataStruct)
            {
                Debug.LogWarning("data error!");
                return;
            }

            CMD_GR_UserScore score = GameConvert.ByteToStruct<CMD_GR_UserScore>(packet.Data);
            PlayerInfo player = GameApp.GameSrv.FindPlayer(score.dwUserID);
            if (player == null)
            {
                Debug.LogWarning("player is not exists!");
                return;
            }
            player.Money = score.lScore;
            player.BankMoney = score.lInsure;
            player.DrawCount = score.dwDrawCount;
            player.LostCount = score.dwLostCount;
            player.WinCount = score.dwWinCount;

            bool isSelf = (score.dwUserID == GameApp.GameData.UserInfo.UserID);
            if (isSelf)
            {
                GameApp.GameData.UserInfo.CurMoney = score.lScore;
                GameApp.GameData.UserInfo.CurBank = score.lInsure;
                GameApp.GameSrv.CallGameUserUpdatedEvent();

            }
        }

        private void OnUserStatusResp(Packet packet)
        {
            int dataStruct = Marshal.SizeOf(typeof(CMD_GR_UserStatus));
            if (packet.DataSize < dataStruct)
            {
                Debug.LogWarning("data error!");
                return;
            }

            CMD_GR_UserStatus status = GameConvert.ByteToStruct<CMD_GR_UserStatus>(packet.Data);

            PlayerInfo player = GameApp.GameSrv.FindPlayer(status.dwUserID);
            if (player == null)
            {
                return;
            }

            if (status.UserStatus == (byte)UserState.US_NULL)
            {
                GameApp.GameSrv.CallUserLeftEvent(status.dwUserID);

                GameApp.GameSrv.RemovePlayer(status.dwUserID);
            }
            else
            {
                ushort wLastTableID = player.DeskNO;
                ushort wLastChairID = player.DeskStation;
                byte cbLastStatus = player.UserState;

                ushort wNowTableID = status.TableID;
                ushort wNowChairID = status.ChairID;
                byte cbNowStatus = status.UserStatus;

                GameApp.GameSrv.SetPlayerState(
                    status.dwUserID,
                    status.TableID,
                    status.ChairID,
                    status.UserStatus);

                //判断发送
                bool bNotifyGame = false;
                if (status.dwUserID == GameApp.GameData.UserInfo.UserID)
                {
                    //自己的状态
                    bNotifyGame = true;
                }
                else if ((GameApp.GameData.UserInfo.DeskNO != CommonDefine.INVALID_TABLE) 
                    && (GameApp.GameData.UserInfo.DeskNO == wNowTableID))
                {
                    //新来同桌的状态
                    bNotifyGame = true;
                }
                else if ((GameApp.GameData.UserInfo.DeskNO != CommonDefine.INVALID_TABLE) 
                    && (GameApp.GameData.UserInfo.DeskNO == wLastTableID))
                {
                    //原来同桌的状态
                    bNotifyGame = true;
                }

                //if (bNotifyGame == true)
                {
                    //站起
                    if (cbNowStatus == (byte)UserState.US_FREE)
                    {
                        GameApp.GameSrv.CallUserStandUpEvent(status.dwUserID);

                    }
                    //坐下
					//服务器结算后会从Play转为SIT,所以这里需要排除
                    if (cbLastStatus != (byte)UserState.US_PLAY 
					&& cbNowStatus == (byte)UserState.US_SIT)
                    {
                        GameApp.GameSrv.CallUserSitDownEvent(status.dwUserID, status.TableID, status.ChairID);

                    }
                    //继续服务器状态会从free直接跳到ready
                    if (cbNowStatus == (byte)UserState.US_READY)
                    {
                        if (cbLastStatus == (byte)UserState.US_FREE)
                        {
                            GameApp.GameSrv.CallUserSitDownEvent(status.dwUserID, status.TableID, status.ChairID);

                        }
                        GameApp.GameSrv.CallUserAgreeEvent(status.TableID, status.ChairID, 0);

                    }
                    if (cbNowStatus == (byte)UserState.US_OFFLINE)
                    {
                        GameApp.GameSrv.CallUserOfflineEvent(status.UserStatus, status.TableID, status.ChairID);

                    }
                    if (cbNowStatus == (byte)UserState.US_LOOKON)
                    {
                    }
                    if (cbNowStatus == (byte)UserState.US_PLAY)
                    {

                    }
                }
                //加入处理
                /*if ((wNowTableID != CommonDefine.INVALID_TABLE) && ((wNowTableID != wLastTableID) || (wNowChairID != wLastChairID)))
                {
                    bool isSelf = (status.dwUserID == GameApp.GameData.UserInfo.UserID);
                    if (isSelf)
                    {
                        if (UserSitDownEvent != null)
                        {
                            UserSitDownEvent(status.dwUserID, status.TableID, status.ChairID);
                        }
                    }
                }*/
            }
        }

        private void OnUserEnterResp(Packet packet)
        {
            int dataStruct = Marshal.SizeOf(typeof(tagUserInfoHead));
            if (packet.DataSize < dataStruct)
            {
                Debug.LogWarning("data error!");
                return;
            }

            tagUserInfoHead userInfo = GameConvert.ByteToStruct<tagUserInfoHead>(packet.Data);
            PlayerInfo player = ProtoHelper.InitPlayerInfo(userInfo);
            if (player.ID == GameApp.GameData.UserInfo.UserID)
            {
                GameApp.GameData.UserInfo.SetSitInfo(player.DeskNO, player.DeskStation);
            }

            ByteBuffer buffer = ByteBufferPool.PopPacket(packet.Data);
            buffer.Position = dataStruct;

            while (true)
            {
                string str = "";
                ushort type = ProtoHelper.ReadDescDataString(ref buffer, ref str);
                if (type == 0)
                {
                    break;
                }

                switch (type)
                {
                    case CommonDefine.DTP_GR_NICK_NAME:
                        {
                            player.NickName = str;
                            break;
                        }
                }
            }
            ByteBufferPool.DropPacket(buffer);

            GameApp.GameSrv.AddPlayer(player);
            GameApp.GameSrv.CallUserEnterEvent(player);

        }

        public void OnLogonResp(Packet packet)
        {
            switch (packet.SubCmd)
            {
                case SubCommand.SUB_GR_LOGON_ERROR:
                    {
                        CMD_GP_LogonFailure failure = GameConvert.ByteToStruct<CMD_GP_LogonFailure>(packet.Data);
                        if (failure.lResultCode == (UInt32)FailureCode.LockServerID)
                        {
                            GameApp.GameSrv.CallUserReloginGameRoomEvent((UInt32)GameApp.GameData.UserInfo.CutRoomID, true);
                        }
                        GameApp.GameSrv.CallGameLogonErrorEvent((int)failure.lResultCode, failure.szDescribeString);

                        
                       
                        break;
                    }
                case SubCommand.SUB_GR_LOGON_SUCCESS:
                    {
                        GameApp.GameSrv.CallGameLogonSuccessEvent();

                        if (GameApp.GameData.UserInfo.CutRoomID > 0)
                        {
                            GameApp.GameData.UserInfo.CutRoomID = -1;
                        }

                        break;
                    }
                case SubCommand.SUB_GR_LOGON_FINISH:
                    {

                        var playing = false;
                        var player = GameApp.GameSrv.FindMe();
                        if (player != null)
                        {
                            playing = player.UserState > (byte)UserState.US_FREE;
                        }

                        GameApp.GameSrv.CallGameCreatedEvent(GameApp.GameData.EnterRoomID, playing);

                        if (playing && GameApp.GameData.UserInfo.DeskNO != CommonDefine.INVALID_TABLE)
                        {
                            GameApp.GameSrv.CallGameStartedEvent(GameApp.GameData.UserInfo.DeskNO);
                        }
                        /*
                        if (GameApp.GameData.UserInfo != null && GameApp.GameData.UserInfo.DeskNO != CommonDefine.INVALID_TABLE)
                        {
                            //TODO 断线重连状态需要整理
                            
                            if (UserSitDownEvent != null)
                            {
                                UserSitDownEvent(GameApp.GameData.UserInfo.UserID, GameApp.GameData.UserInfo.DeskNO, GameApp.GameData.UserInfo.DeskStation);
                            }
                            if (GameStartedEvent != null)
                            {
                                GameStartedEvent(GameApp.GameData.UserInfo.DeskNO);
                            }
                        }
                        else
                        {
                            if (GameCreatedEvent != null)
                            {
                                GameCreatedEvent(GameApp.GameData.EnterRoomItem.ID);
                            }

                        }*/

                        break;
                    }
            }
        }
        #endregion

    }
}


