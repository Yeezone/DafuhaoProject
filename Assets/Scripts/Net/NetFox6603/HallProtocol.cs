using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Services.Utility;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Lobby;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.Services.NetFox
{
    internal class HallProtocol : Singleton<HallProtocol>
    {
        #region 协议解析

        void OnLogonSuccess(Packet packet)
        {
            int dataStruct = Marshal.SizeOf(typeof(CMD_GP_LogonSuccess));
            Logger.Net.Log("logon success. size:"+packet.DataSize+"/"+dataStruct);
            if (packet.DataSize < dataStruct)
            {
                Logger.Net.LogError(" CMD_GP_LogonSuccess data error!");
                return;
            }

            ByteBuffer buffer = ByteBufferPool.PopPacket(packet.Data);
            byte[] descData = buffer.PopByteArray(Marshal.SizeOf(typeof(CMD_GP_LogonSuccess)));
            CMD_GP_LogonSuccess loginData = GameConvert.ByteToStruct<CMD_GP_LogonSuccess>(descData);
            GameApp.GameData.UserInfo = ProtoHelper.InitUserInfo(loginData);
			Logger.Net.Log("User:" + loginData.szAccounts + " Loged|Money:" + loginData.lUserScore + "|Bank:" + loginData.lUserInsure);
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
                    case CommonDefine.DTP_GP_UI_UNDER_WRITE:
                        {
                            GameApp.GameData.UserInfo.UnderWrite = str;
                            break;
                        }
                }
            }
            ByteBufferPool.DropPacket(buffer);

            GameApp.Account.CallLogonSuccessEvent();

        }


        public void OnLogonResp(Packet packet)
        {
            switch (packet.SubCmd)
            {
                case SubCommand.SUB_GP_LOGON_ERROR:
                    {
                        var failure = GameConvert.ByteToStruct<CMD_GP_LogonFailure>(packet.Data);
                        GameApp.Account.CallLogonErrorEvent((int) failure.lResultCode, failure.szDescribeString);

                        return;
                    }
                case SubCommand.SUB_GP_LOGON_SUCCESS:
                    {
                        OnLogonSuccess(packet);
                        return;
                    }
                case SubCommand.SUB_GP_LOGON_FINISH:
                    {
                        return;
                    }
                case SubCommand.SUB_GP_COMMUNICATION_KEY:
                    {
                        var CommunicationKey = GameConvert.ByteToStruct<CMD_GP_CommunicationKey>(packet.Data);
                        GameApp.Account.SetClientConfig((UInt64) CommunicationKey.dwServerTime, CommunicationKey.szToken);
                        return;
                    }
                case SubCommand.SUB_GP_UPDATE_NOTIFY:
                    {
                        GameApp.Account.CallLogonErrorEvent(12, "版本过低需要升级！！！！");
                        return;
                    }
                case SubCommand.SUB_GP_REPEAT_LOGON:
                    {
                        GameApp.Account.CallSocketCloseNotifyEvent(SubCommand.SUB_GP_REPEAT_LOGON);
                        return;
                    }
                default:
                    return;
            }

        }

        public void OnServerListResp(Packet packet)
        {
            switch (packet.SubCmd)
            {
                case SubCommand.SUB_GP_LIST_TYPE:
                    {
                        int dataStruct = Marshal.SizeOf(typeof(tagGameType));
                        if (packet.DataSize < dataStruct)
                        {
                            Logger.Net.LogError(" tagGameType Game List data error!");
                            return;
                        }

                        int dataSzie = Marshal.SizeOf(typeof(tagGameType));
                        int dataCount = packet.DataSize / dataSzie;

                        ByteBuffer listDataBuff = ByteBufferPool.PopPacket(packet.Data);
                        for (int i = 0; i < dataCount; i++)
                        {
                            byte[] tempDataBB = listDataBuff.PopByteArray(dataSzie);
                            tagGameType kindDataE = GameConvert.ByteToStruct<tagGameType>(tempDataBB);
                            GameApp.GameListMgr.AppendTypeItem(ProtoHelper.InitGameTypeItem(kindDataE));

                            //Logger.Sys.Log("Append Game Type===========" + kindDataE.TypeID + " Name:" + kindDataE.TypeName);
                        }
                        ByteBufferPool.DropPacket(listDataBuff);

                        return;
                    }
                case SubCommand.SUB_GP_LIST_KIND:
                    {
                        int dataStruct = Marshal.SizeOf(typeof(tagGameKind));
                        if (packet.DataSize < dataStruct)
                        {
                            Logger.Net.LogError(" tagGameKind Game name list data error!");
                            return;
                        }

                        int dataSzie = Marshal.SizeOf(typeof(tagGameKind));
                        int dataCount = packet.DataSize / dataSzie;

                        ByteBuffer listDataBuff = ByteBufferPool.PopPacket(packet.Data);
                        for (int i = 0; i < dataCount; i++)
                        {
                            byte[] tempDataBB = listDataBuff.PopByteArray(dataSzie);
                            tagGameKind tempNameS = GameConvert.ByteToStruct<tagGameKind>(tempDataBB);
                            GameApp.GameListMgr.AppendKindItem(ProtoHelper.InitGameKindItem(tempNameS));
                            //Logger.Sys.Log("Append Game Kind==========="+ tempNameS.KindID +"  Name:"+tempNameS.KindName + " Process:"+tempNameS.ProcessName);
                        }
                        ByteBufferPool.DropPacket(listDataBuff);

                        //GameApp.Account.CallGameListFinishEvent();
                        return;
                    }
                case SubCommand.SUB_GP_LIST_NODE:
                    {
						int dataStruct = Marshal.SizeOf(typeof(tagGameNode));
						if (packet.DataSize < dataStruct)
						{
                            Logger.Net.LogError(" tagGameNode Game List data error!");
							return;
						}
				
						int dataSzie = Marshal.SizeOf(typeof(tagGameNode));
						int dataCount = packet.DataSize / dataSzie;
				
						ByteBuffer listDataBuff = ByteBufferPool.PopPacket(packet.Data);
						for (int i = 0; i < dataCount; i++)
						{
							byte[] tempDataBB = listDataBuff.PopByteArray(dataSzie);
							tagGameNode nodeDataE = GameConvert.ByteToStruct<tagGameNode>(tempDataBB);
							GameApp.GameListMgr.AppendNodeItem(ProtoHelper.InitGameNodeItem(nodeDataE));
						}
                        ByteBufferPool.DropPacket(listDataBuff);

                        GameApp.Account.CallGameListFinishEvent();
                        return;
                    }
                case SubCommand.SUB_GP_LIST_SERVER:
                    {
                        int dataStruct = Marshal.SizeOf(typeof(tagGameServer));
                        if (packet.DataSize < dataStruct)
                        {
                            Logger.Net.LogError(" tagGameServer Room list data error!");
                            return ;
                        }

                        uint kindID = 0;
                        int dataSzie = Marshal.SizeOf(typeof(tagGameServer));
                        int dataCount = packet.DataSize / dataSzie;
                        ByteBuffer listDataBuff = ByteBufferPool.PopPacket(packet.Data);

                        for (int i = 0; i < dataCount; i++)
                        {
                            byte[] tempData = listDataBuff.PopByteArray(dataSzie);
                            tagGameServer roomInfoS = GameConvert.ByteToStruct<tagGameServer>(tempData);

                            //Logger.Net.Log("--------------ROOM:" + roomInfoS.KindID + " " + roomInfoS.ServerID + " " + roomInfoS.ServerName+" "+roomInfoS.ServerAddr);

                            kindID = roomInfoS.KindID;

                            GameApp.GameListMgr.AppendRoomItem(ProtoHelper.InitGameRoomItem(roomInfoS));

                           /* if (GameApp.GameData.ReEnter &&
                                GameApp.GameData.ReEnterRoomID == roomInfoS.ServerID)
                            {
                                GameApp.GameData.ReEnterNameID = roomInfoS.KindID;
                                if (ReconnectGameEvent != null)
                                {
                                    ReconnectGameEvent(roomInfoS.ServerID, false);
                                }
                            }*/
                        }

                        ByteBufferPool.DropPacket(listDataBuff);

                        GameApp.Account.CallRoomListFinishEvent(kindID);
                        return;
                    }
                case SubCommand.SUB_CS_S_SERVER_ONLINE:
                    {
                        int dataSzie = Marshal.SizeOf(typeof(tagOnLineInfoServer));
                        if (packet.DataSize < dataSzie)
                        {
                            Logger.Net.LogError(" tagGameServer Room list data error! size:" + packet.DataSize);
                            return;
                        }

                        int dataCount = packet.DataSize / dataSzie;

                        ByteBuffer listDataBuff = ByteBufferPool.PopPacket(packet.Data);
                        for (int i = 0; i < dataCount; i++)
                        {
                            byte[] tempDataBB = listDataBuff.PopByteArray(dataSzie);
                            tagOnLineInfoServer OnlineDataE = GameConvert.ByteToStruct<tagOnLineInfoServer>(tempDataBB);

                            SGameRoomItem temRoomItem = GameApp.GameListMgr.FindRoomItem((uint)OnlineDataE.wServerID);
                            if (temRoomItem != null)
                            {
                                temRoomItem.UpdateOnlineCnt((uint)OnlineDataE.dwOnLineCount);
                            }
                        }
                        ByteBufferPool.DropPacket(listDataBuff);

                        GameApp.Account.CallOnlineCountEvent();

                        return;
                    }
            }

        }

        public void OnCommandResp(Packet obj)
        {
            GameApp.Network.SendToSvr(ConnectionID.Lobby, MainCommand.MDM_KN_COMMAND, SubCommand.SUB_KN_DETECT_SOCKET, 0, null);
        }

        public void OnWebNoticationResp(Packet packet)
        {
            switch (packet.SubCmd)
            {
                case SubCommand.SUB_WEB_FROZEN_ACCOUNT:
                    {
                        GameApp.Account.CallFrozenAccountEvent();
                        break;
                    }
                case SubCommand.SUB_WEB_UP_OR_DOWN_POINT:
                    {
                        AnalysisUpOrDownPoint(packet.Data);
                        break;
                    }
            }

        }

        private bool AnalysisUpOrDownPoint(byte[] wByteBuffer)
        {
            int dataStruct = Marshal.SizeOf(typeof(CMD_GH_WebUpOrDownPoint));
            if (wByteBuffer == null || wByteBuffer.Length < dataStruct)
            {
                Logger.Net.LogError("CMD_GH_WebUpOrDownPoint UpOrDownPoint Success Data Error !");
                return false;
            }
            CMD_GH_WebUpOrDownPoint UserUpOrDownPoint = GameConvert.ByteToStruct<CMD_GH_WebUpOrDownPoint>(wByteBuffer);
            if (UserUpOrDownPoint.dwUserID == GameApp.GameData.UserInfo.UserID)
            {
                GameApp.GameData.UserInfo.AddBankMoney(UserUpOrDownPoint.dwMoney);
				GameApp.Account.CallUserUpdatedEvent();
            }
            return true;
        }

        public void OnUserServiceResp(Packet packet)
        {
            switch (packet.SubCmd)
            {
                case SubCommand.SUB_GP_USER_INDIVIDUAL:
                    {
                        OnQueryIndividualResp(packet);
                        break;
                    }
                case SubCommand.SUB_GP_OPERATE_FAILURE:
                    {
                        GameApp.Account.CallOperateResultEvent(false);
                        CMD_GP_OperateSuccess CheckCode = GameConvert.ByteToStruct<CMD_GP_OperateSuccess>(packet.Data);
                        GameApp.Account.CallChangeInformationEvent((int)CheckCode.lResultCode, CheckCode.szDescribeString);
                        break;
                    }
                case SubCommand.SUB_GP_OPERATE_SUCCESS:
                    {
                        GameApp.Account.CallOperateResultEvent(true);
                        CMD_GP_OperateSuccess CheckCode = GameConvert.ByteToStruct<CMD_GP_OperateSuccess>(packet.Data);
                        if (!string.IsNullOrEmpty(GameApp.GameData.TempNickName))
                        {
                            GameApp.GameData.UserInfo.NickName = GameApp.GameData.TempNickName;
                            GameApp.GameData.TempNickName = "";
                        }
                        GameApp.Account.CallChangeInformationEvent((int) CheckCode.lResultCode,
                                                                   CheckCode.szDescribeString);
                        break;
                    }
				case SubCommand.SUB_GP_SAFETYBOX_VERIFY:
					{
						RecevieSafetyBox(packet);
						break;
					}
				case SubCommand.SUB_GP_CHANGE_PASSWD:
					{
						CMD_GP_UserResult CheckCode = GameConvert.ByteToStruct<CMD_GP_UserResult>(packet.Data);
						if(CheckCode.dwCheckCode == 0)
						{
							GameApp.GameData.Password = GameApp.GameData.TempPassword;
						}
                        GameApp.Account.CallPassWDChangeEvent(CheckCode.dwCheckCode);
						break;
					}
				case SubCommand.SUB_GP_CHANGE_BANK_PASSWD:
					{
						CMD_GP_UserResult CheckCode = GameConvert.ByteToStruct<CMD_GP_UserResult>(packet.Data);
					    GameApp.Account.CallPassWDChangeEvent(CheckCode.dwCheckCode);
						break;
					}
				case SubCommand.SUB_GP_LOCK_OR_UNLOCK_ACCOUNT:
					{
						AnalysisLockAccount(packet);
						break;
					}
				case SubCommand.SUB_GP_USER_SUGGESTION:
					{
						CMD_GP_UserResult CheckCode = GameConvert.ByteToStruct<CMD_GP_UserResult>(packet.Data);
					    GameApp.Account.CallUserSuggestionEvent(CheckCode.dwCheckCode);
						break;
					}
				case SubCommand.SUB_GP_GAME_RECORD:
					{
						AnalysisGameRecord(packet);
						break;
					}
				case SubCommand.SUB_GP_LOGON_RECORD:
					{
						AnalysisLogonRecord(packet);
						break;
					}
				case SubCommand.SUB_GP_RECHANGE_INFO:
					{
						AnalysisRechange(packet);
						break;
					}
				case SubCommand.SUB_GP_EXCHANGE_INFO:
					{
						AnalysisExcahnge(packet);
						break;
					}
				case SubCommand.SUB_GP_USER_FACE_INFO:
					{
						CMD_GP_UserFaceInfo Data = GameConvert.ByteToStruct<CMD_GP_UserFaceInfo>(packet.Data);
						GameApp.GameData.UserInfo.HeadId = Data.wFaceID;
                        GameApp.Account.CallChangeInformationEvent(0, "修改成功！");
						break;
					}
				case SubCommand.SUB_GP_REFRASH_USER_INFO:
					{
						AnalysisRefrashUserInfo(packet);
						break;
					}
				case SubCommand.SUB_GP_MARQUEE_MESSAGE:
					{
						AnalysisSystemMessage(packet);
						break;
					}
				case SubCommand.SUB_GP_GET_VERSION_INFO:
					{
						AnalysisVersionInfo(packet);
						break;
					}
                case   SubCommand.SUB_GP_CANCLE_MARQUEE:
                    {
                        AnalysisCancleMarqueeMsg(packet);
                        break;
                    }
            }
        }

        private void AnalysisVersionInfo(Packet packet)
        {
            ByteBuffer buffer = ByteBufferPool.PopPacket(packet.Data);
            //不需要只包含用户信息的数据包
            byte[] descData = buffer.PopByteArray(Marshal.SizeOf(typeof(CMD_GP_ClientUpdate)));
            CMD_GP_ClientUpdate version = GameConvert.ByteToStruct<CMD_GP_ClientUpdate>(descData);
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
                    case CommonDefine.DTP_GP_CDN:
                        {
                            GameApp.GameData.CDN = str;
                            break;
                        }
                    case CommonDefine.DTP_GP_OFFICESITE_URL:
                        {
                            GameApp.GameData.OfficeSiteUrl = str;
                            break;
                        }
                    case CommonDefine.DTP_GP_BACK_STORGE_URL:
                        {
                            GameApp.GameData.BackStorgeUrl = str;
                            break;
                        }
                    case CommonDefine.DTP_GP_MODULE_INFO:
                        {
                            GameApp.ModuleMgr.ApplyDataFromStr(str);
                            break;
                        }
                }

            }
            ByteBufferPool.DropPacket(buffer);

            GameApp.Account.CallVersionInfoEvent(version.dwVersion);
        }

        private void OnQueryIndividualResp(Packet packet)
        {
            ByteBuffer buffer = ByteBufferPool.PopPacket(packet.Data);
            //不需要只包含用户信息的数据包
            buffer.Position = Marshal.SizeOf(typeof(CMD_GP_UserIndividual));

            var info = new CMD_GH_UserInformation();
            info.dwLogoID = GameApp.GameData.UserInfo.HeadId;

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
                    case CommonDefine.DTP_GP_UI_COMPELLATION:
                        {
                            info.dwName = str;
                            break;
                        }
                    case CommonDefine.DTP_GP_UI_QQ:
                        {
                            info.dwIM = str;
                            break;
                        }
                    case CommonDefine.DTP_GP_UI_USER_NOTE:
                        {
                            info.dwIdentification = str;
                            break;
                        }
                    case CommonDefine.DTP_GP_UI_MOBILE_PHONE:
                        {
                            info.dwCellPhone = str;
                            break;
                        }
                }

            }

            ByteBufferPool.DropPacket(buffer);

            GameApp.Account.CallUserInformationEvent(
                info.dwName, 
                info.dwIdentification, 
                info.dwCellPhone, 
                info.dwIM, 
                info.dwLogoID);
        }


        private bool RecevieSafetyBox(Packet packet)
        {
			CMD_GP_UserResult CheckCode = GameConvert.ByteToStruct<CMD_GP_UserResult>(packet.Data);
            GameApp.Account.CallSafetyBoxEvent(CheckCode.dwCheckCode);

            //保险柜解锁结果,直接发给UI
            return true;
        }

        private bool AnalysisLogonRecord(Packet packet)
        {
            int dataStruct = Marshal.SizeOf(typeof(CMD_GH_LogonRecord));
            if (packet.DataSize < dataStruct)
            {
                Logger.Net.LogError("LogonRecord Success Data Error  CMD_GH_LogonRecord!");
                return false;
            }

            ByteBuffer listDataBuff = ByteBufferPool.PopPacket(packet.Data);
            int dataSzie = Marshal.SizeOf(typeof(CMD_GH_LogonRecord));
            int dataCount = packet.DataSize / dataSzie;
          
            List<LogonRecordItem>  logonRecordList = new List<LogonRecordItem>();
            for (int i = 0; i < dataCount; i++)
            {
                byte[] tempData = listDataBuff.PopByteArray(dataSzie);
                CMD_GH_LogonRecord logonRecord = GameConvert.ByteToStruct<CMD_GH_LogonRecord>(tempData);
                LogonRecordItem logonItem = new LogonRecordItem();
                logonItem.dwLogonIP = logonRecord.dwLogonIP;
                logonItem.dwTmlogonTime = logonRecord.dwTmlogonTime;
                logonRecordList.Add(logonItem);
            }
            ByteBufferPool.DropPacket(listDataBuff);
            GameApp.Account.CallLogonRecordEvent(logonRecordList);

            return true;
        }

		//游戏记录
        private bool AnalysisGameRecord(Packet packet)
        {
            //int dataStruct = Marshal.SizeOf(typeof(CMD_GH_GameRecord));
            //if (packet.DataSize < dataStruct)
            //{
            //    Debug.LogError("Game Record Data Error CMD_GH_GameRecord!");
            //    return false;
            //}

            List<GameRecordItem> gameRecordList = new List<GameRecordItem>();
            if (packet.DataSize > 0)
            {
                ByteBuffer listDataBuff = ByteBufferPool.PopPacket(packet.Data);
                int dataSzie = Marshal.SizeOf(typeof(CMD_GH_GameRecord));
                int dataCount = packet.DataSize / dataSzie;
                for (int i = 0; i < dataCount; i++)
                {
                    byte[] tempData = listDataBuff.PopByteArray(dataSzie);
                    CMD_GH_GameRecord gameRecord = GameConvert.ByteToStruct<CMD_GH_GameRecord>(tempData);
                    GameRecordItem gameItem = new GameRecordItem();
                    gameItem.dwAllCount = gameRecord.dwAllCount;
                    gameItem.dwAmount = gameRecord.dwAmount;
                    gameItem.dwEndTime = gameRecord.dwEndTime;
                    gameItem.dwGameKind = gameRecord.dwGameKind;
                    gameRecordList.Add(gameItem);
                }
                ByteBufferPool.DropPacket(listDataBuff);
            }
            GameApp.Account.CallGameRecordEvent(gameRecordList);

            return true;
        }

		//锁定返回值
		private bool AnalysisLockAccount(Packet packet)
		{
			int dataStruct = Marshal.SizeOf(typeof(CMD_GP_LockAndUnlock));
			if (packet.DataSize < dataStruct)
			{
                Logger.Net.LogError("Lock   Account  Data Error CMD_GP_LockAndUnlock!");
				return false;
			}
			
			CMD_GP_LockAndUnlock lockData = GameConvert.ByteToStruct<CMD_GP_LockAndUnlock>(packet.Data);
			if (lockData.dwUserID == GameApp.GameData.UserInfo.UserID)
			{
				if (lockData.dwCommanResult == 0)
				{
                    GameApp.GameData.UserInfo.MoorMachine = lockData.dwCommanType;
				}

                GameApp.Account.CallAccountResultEvent(lockData.dwCommanType, lockData.dwCommanResult);
			}
			return true;
		}

		private bool AnalysisRechange(Packet packet)
		{
			int dataStruct = Marshal.SizeOf(typeof(CMD_GP_RechangeInfo));
			if (packet.DataSize < dataStruct)
			{
                Logger.Net.LogError("Rechange  Data Error CMD_GP_RechangeInfo!");
				return false;
			}
			
			CMD_GP_RechangeInfo RechangeData = GameConvert.ByteToStruct<CMD_GP_RechangeInfo>(packet.Data);

            GameApp.Account.CallRechangeEvent(RechangeData.dwMinMoney, (int)RechangeData.dwChangeScale);
			return true;
		}
		
		private bool AnalysisExcahnge( Packet packet )
		{
			int dataStruct = Marshal.SizeOf(typeof(CMD_GP_ExchangeInfo));
			if (packet.DataSize < dataStruct)
			{
                Logger.Net.LogError("Exchange  Data Error CMD_GP_ExchangeInfo!");
				return false;
			}
			
			CMD_GP_ExchangeInfo ExchangeData = GameConvert.ByteToStruct<CMD_GP_ExchangeInfo>(packet.Data);
			GameApp.GameData.UserInfo.CurBank = ExchangeData.dwBankMoney;
            GameApp.Account.CallExchangeEvent(ExchangeData.dwMinMoney, (int)ExchangeData.dwChangeScale, ExchangeData.dwWithdrawals);
			return true;
		}

		private bool AnalysisRefrashUserInfo( Packet packet )
		{
			int dataStruct = Marshal.SizeOf(typeof(CMD_GH_RefrashUserInfo));
			if (packet.DataSize < dataStruct)
			{
                Logger.Net.LogError("Exchange  Data Error CMD_GH_RefrashUserInfo!");
				return false;
			}
			CMD_GH_RefrashUserInfo UserInfo = GameConvert.ByteToStruct<CMD_GH_RefrashUserInfo>(packet.Data);
			GameApp.GameData.UserInfo.CurMoney = UserInfo.dwUserScore;
		    GameApp.Account.CallUserUpdatedEvent();
			return true;
		}

        private bool AnalysisSystemMessage(Packet packet)
        {
            int dataStruct = Marshal.SizeOf(typeof (CMD_CS_MarqueeMessage));
            if (packet.DataSize < dataStruct)
            {
                Logger.Net.LogError("CMD_CS_MarqueeMessage Data Error !");
                return true;
            }

            ByteBuffer listDataBuff = ByteBufferPool.PopPacket(packet.Data);
           int dataSzie = Marshal.SizeOf(typeof(CMD_CS_MarqueeMessage));
           int dataCount = packet.DataSize / dataSzie;
           for (int i = 0; i < dataCount; i++)
           {
               byte[] tempData = listDataBuff.PopByteArray(dataSzie);
               CMD_CS_MarqueeMessage msg = GameConvert.ByteToStruct<CMD_CS_MarqueeMessage>(tempData);
               GameApp.Account.CallMarqueeMessageEvent(msg.MsgType,
                                       msg.MsgID,
                                       msg.MsgPlayCount,
                                       (float)msg.MsgInterval,
                                       msg.szMessage,
                                       msg.MsgStartTime,
                                       msg.MsgNumberID,
                                       msg.MsgPlayTime);

           }
           ByteBufferPool.DropPacket(listDataBuff);

            return true;
        }

        private bool AnalysisCancleMarqueeMsg(Packet packet)
        {
            int dataStruct = Marshal.SizeOf(typeof(CMD_CS_CancleMarqueeMsg));
            if (packet.DataSize < dataStruct)
            {
                Logger.Net.LogError("Exchange  Data Error CMD_GH_RefrashUserInfo!");
                return false;
            }
            CMD_CS_CancleMarqueeMsg cancleMsg = GameConvert.ByteToStruct<CMD_CS_CancleMarqueeMsg>(packet.Data);
            GameApp.Account.CallCancleMarqueeMsgEvent(cancleMsg.dwMarqueeMsgID);

            return true;
        }

        #endregion

    }
}
