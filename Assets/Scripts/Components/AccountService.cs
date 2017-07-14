using System;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.Utility;
using com.QH.QPGame.Services.NetFox;
using System.Collections.Generic;

namespace com.QH.QPGame.Lobby
{
    // <summary>
    // 账户服务,提供客户端与大厅服务器通信的接口,为逻辑层屏蔽具体的平台，以及作为数据的缓存管理
    // 需要把Services里的HallProtocol所有事件,以及获取,发送数据接口等迁移到这里来
    // @Author: guofeng
    // </summary>
    public class AccountService
    {


      #region 事件

        public delegate void LogonResultHandler();
        public event LogonResultHandler LogonSuccessEvent;

        public delegate void LogonErrorResultHandler(int wHandleCode, string str);
        public event LogonErrorResultHandler LogonErrorEvent;

        //没有用到
        //public delegate void RegistVerityHandler(int wHandleCode);
        //public event RegistVerityHandler RegistVerityEvent;

        //没有用到
        //public delegate void RegistLogonHandler(int wHandleCode);
        //public event RegistLogonHandler RegistFinishedEvent;

        public delegate void SocketCloseNotifyHandler(int wSubCmdID);
        public event SocketCloseNotifyHandler SocketCloseNotifyEvent;

        public delegate void OperateResultHandler(bool operate);
        public event OperateResultHandler OperateResultEvent;



        public delegate void GameListFinishHandler();
        public event GameListFinishHandler GameListFinishEvent;

        public delegate void RoomListFinishHandler(uint dwKindID);
        public event RoomListFinishHandler RoomListFinishEvent;

        public delegate void UpdataOnlineCountHandler();
        public event UpdataOnlineCountHandler UpdataOnlineCountEvent;



        public delegate void VersionInfoHandler(Int32 version);
        public event VersionInfoHandler VersionInfoEvent;

        public delegate void AccountResultHandler(UInt32 dwCommanType, UInt32 dwCommanResult);
        public event AccountResultHandler AccountResultEvent;

        //没有用到
        //public delegate void UpdateOnlineHandler(uint GameID);
        //public event UpdateOnlineHandler UpdateOnlineEvent;

        public delegate void UserInformationHandler(
            string dwName,
            string dwIdentification,
            string dwCellPhone,
            string dwIM,
            UInt32 dwLogoID);
        public event UserInformationHandler UserInformationEvent;

        public delegate void UserInfoUpdateHandler();
        public event UserInfoUpdateHandler UserUpdatedEvent;

        public delegate void ChangeInformationHandler(int wHandleCode, string msg);
        public event ChangeInformationHandler ChangeInformationEvent;

        public delegate void LogonRecordHandler(List<LogonRecordItem> logonRecordList);
        public event LogonRecordHandler LogonRecordEvent;

        public delegate void GameRecordHandler(List<GameRecordItem> gameRecordList);
        public event GameRecordHandler GameRecordEvent;
       
        //没有用到
        //public delegate void MoneyRecordHandler(List<CMD_GH_MoneyRecord> MoneyRecordList);
        //public event MoneyRecordHandler MoneyRecordEvent;

        public delegate void UserSuggestionHandler(int wHandleCode);
        public event UserSuggestionHandler UserSuggestionEvent;

        public delegate void SafetyBoxMessageHandler(int wHandleCode);
        public event SafetyBoxMessageHandler SafetyBoxEvent;

        public delegate void PassWDChangeMessageHandler(int wHandleCode);
        public event PassWDChangeMessageHandler PassWDChangeEvent;

        public delegate void RechangeHandler(Int64 MinMoney, int ChangeScale);
        public event RechangeHandler RechangeEvent;

        public delegate void ExchangeHandler(Int64 MinMoney, int ChangeScale, Int64 Withdrawals);
        public event ExchangeHandler ExchangeEvent;

      
        public delegate void FrozenAccountHandler();
        public event FrozenAccountHandler FrozenAccountEvent;

        //银行
        public delegate void CheckMoneyMessageHandler(bool wHandleCode, string Massege);
        public event CheckMoneyMessageHandler CheckMoneyEvent;
        #endregion

      #region 事件调用函数

        #region  MDM_GP_LOGON
        //登录成功
        public bool CallLogonSuccessEvent()
        {
            if (LogonSuccessEvent != null)
            {
                LogonSuccessEvent();
                return true;
            }
            return false;
        }

        //登录失败
        public bool CallLogonErrorEvent(int wHandleCode, string str)
        {
            if (LogonErrorEvent != null)
            {
                LogonErrorEvent(wHandleCode, str);
                return true;
            }
            return false;
        }

        //异地登录
        public bool CallSocketCloseNotifyEvent(int wSubCmdID)
        {
            if (SocketCloseNotifyEvent != null)
            {
                SocketCloseNotifyEvent(wSubCmdID);
                return true;
            }
            return false;
        }

        //操作返回
        public bool CallOperateResultEvent(bool operate)
        {
            if (OperateResultEvent != null)
            {
                OperateResultEvent(operate);
                return true;
            }
            return false;
        }

        public bool SetClientConfig(UInt64 serverTime, string privateKey)
        {
            TimeSpan dt = DateTime.Now - DateTime.Parse("1970-01-01 08:00:00");
            Logger.Net.Log("serverTime:" + serverTime + "  Seconds: " + dt.TotalSeconds);

            GameApp.GameData.DiffServerTime = (Int64)serverTime - (Int64)dt.TotalSeconds;
            GameApp.GameData.PrivateKey = privateKey;

            return false;

        }
   

        #endregion

        #region  MDM_GP_SERVER_LIST
        //接收游戏列表完成
        public bool CallGameListFinishEvent()
        {
            if (GameListFinishEvent != null)
            {
                GameListFinishEvent();
                return true;
            }
            return false;
        }

        //接收一个游戏的房间列表完成
        public bool CallRoomListFinishEvent(uint dwKindID)
        {
            if (RoomListFinishEvent != null)
            {
                RoomListFinishEvent(dwKindID);
                return true;
            }
            return false;
        }

        //接收房间在线人数
        public bool CallOnlineCountEvent()
        {
            if (UpdataOnlineCountEvent != null)
            {
                UpdataOnlineCountEvent();
                return true;
            }
            return false;
        }
        #endregion

        #region  MDM_GP_USER_SERVICE
        //版本信息
        public bool CallVersionInfoEvent(Int32 version)
        {
            if (VersionInfoEvent != null)
            {
                VersionInfoEvent(version);
                return true;
            }
            return false;
        }

        //账号操作结果
        public bool CallAccountResultEvent(UInt32 dwCommanType, UInt32 dwCommanResult)
        {
            if (AccountResultEvent != null)
            {
                AccountResultEvent(dwCommanType, dwCommanResult);
                return true;
            }
            return false;
        }

        //用户信息
        public bool CallUserInformationEvent(
            string dwName,
            string dwIdentification,
            string dwCellPhone,
            string dwIM,
            UInt32 dwLogoID)
        {
            if (UserInformationEvent != null)
            {
                UserInformationEvent(dwName, dwIdentification, dwCellPhone, dwIM, dwLogoID);
                return true;
            }
            return false;
        }

        //更新用户信息
        public bool CallUserUpdatedEvent()
        {
            if (UserUpdatedEvent != null)
            {
                UserUpdatedEvent();
                return true;
            }
            return false;
        }

        //修改资料返回
        public bool CallChangeInformationEvent(int wHandleCode, string msg)
        {
            if (ChangeInformationEvent != null)
            {
                ChangeInformationEvent(wHandleCode, msg);
                return true;
            }
            return false;
        }

        //登录记录
        public bool CallLogonRecordEvent(List<LogonRecordItem> logonRecordList)
        {
            if (LogonRecordEvent != null)
            {
                LogonRecordEvent(logonRecordList);
                return true;
            }
            return false;
        }

        //游戏记录
        public bool CallGameRecordEvent(List<GameRecordItem> gameRecordList)
        {
            if (GameRecordEvent != null)
            {
                GameRecordEvent(gameRecordList);
                return true;
            }
            return false;
        }

        //反馈结果
        public bool CallUserSuggestionEvent(int wHandleCode)
        {
            if (UserSuggestionEvent != null)
            {
                UserSuggestionEvent(wHandleCode);
                return true;
            }
            return false;
        }

        //保险柜解锁结果
        public bool CallSafetyBoxEvent(int wHandleCode)
        {
            if (SafetyBoxEvent != null)
            {
                SafetyBoxEvent(wHandleCode);
                return true;
            }
            return false;
        }

        //修改密码结果
        public bool CallPassWDChangeEvent(int wHandleCode)
        {
            if (PassWDChangeEvent != null)
            {
                PassWDChangeEvent(wHandleCode);
                return true;
            }
            return false;
        }

        //兑换信息
        public bool CallRechangeEvent(Int64 MinMoney, int ChangeScale)
        {
            if (RechangeEvent != null)
            {
                RechangeEvent(MinMoney, ChangeScale);
                return true;
            }
            return false;
        }

        //充值信息
        public bool CallExchangeEvent(Int64 MinMoney, int ChangeScale, Int64 Withdrawals)
        {
            if (ExchangeEvent != null)
            {
                ExchangeEvent(MinMoney, ChangeScale, Withdrawals);
                return true;
            }
            return false;
        }

        //跑马灯消息
        public bool CallMarqueeMessageEvent(UInt16 mType,
            UInt16 MsgID,
            UInt16 MsgPlayCount,
            float MsgInterval,
            string szMessage,
            UInt64 MessageStartTime,
            Int32  MsgNumberID,
            UInt16 MsgPlayTime)
        {
            MessageStartTime = (UInt64)((Int64)MessageStartTime - GameApp.GameData.DiffServerTime);
            GameApp.MarqueeMgr.AddData(mType, MsgID, MsgPlayCount, MsgInterval, szMessage, MessageStartTime, MsgNumberID, MsgPlayTime);

            return false;
        }

        //取消跑马灯
        public void CallCancleMarqueeMsgEvent(int MarqueeMsgID)
        {
            GameApp.MarqueeMgr.RemoveMessage(MarqueeMsgID);
        }

        #endregion

        #region MDM_WEB_NOTICATION
        //账号冻结
        public bool CallFrozenAccountEvent()
        {
            if (FrozenAccountEvent != null)
            {
                FrozenAccountEvent();
                return true;
            }
            return false;
        }

        #endregion

        //银行
        public void CallCheckMoneyEvent(bool wHandleCode, string Massege)
        {
            if (CheckMoneyEvent != null)
            {
                CheckMoneyEvent(wHandleCode, Massege);
            }
        }
        #endregion

        public  void Initialize()
        {         			
            GameApp.Network.NetworkStatusChangeEvent += Network_NetworkStatusChangeEvent;            

            GameApp.Network.RegisterHandler(ConnectionID.Lobby, MainCommand.MDM_GP_USER_SERVICE, BankProtocol.Instance.ReceiveSubMoneyChange);
            GameApp.Network.RegisterHandler(ConnectionID.Lobby, MainCommand.MDM_GP_LOGON, HallProtocol.Instance.OnLogonResp);
            GameApp.Network.RegisterHandler(ConnectionID.Lobby, MainCommand.MDM_GP_SERVER_LIST, HallProtocol.Instance.OnServerListResp);
            GameApp.Network.RegisterHandler(ConnectionID.Lobby, MainCommand.MDM_GP_USER_SERVICE, HallProtocol.Instance.OnUserServiceResp);
            GameApp.Network.RegisterHandler(ConnectionID.Lobby, MainCommand.MDM_KN_COMMAND, HallProtocol.Instance.OnCommandResp);
            GameApp.Network.RegisterHandler(ConnectionID.Lobby, MainCommand.MDM_WEB_NOTICATION, HallProtocol.Instance.OnWebNoticationResp);
        }

        void Network_NetworkStatusChangeEvent(ConnectionID socket, NetworkManager.Status wError)
        {
            if (socket == ConnectionID.Game)
            {
                //主动推送心跳避免没发送消息之前被服务器断开连接
                SendToHallSvr(MainCommand.MDM_KN_COMMAND, SubCommand.SUB_KN_DETECT_SOCKET, 0, null);
            }
        }


        #region 发送相关函数

        public void SendToHallSvr(uint wMainCmd, uint wSubCmdID, int wHandleCode, byte[] wByteBuffer)
        {
            GameApp.Network.SendToSvr(ConnectionID.Lobby, wMainCmd, wSubCmdID, wHandleCode, wByteBuffer);
        }

        //获取版本和网址信息
        public void ReqGetVersionInfo()
        {
            var info = new CMD_GP_RequestUpdateInfo();
            info.cbDeviceType = GameHelper.GetServerSidePlatform();
            info.dwPlazaVersion = (uint)GameVersion.ProcessVersion(GameApp.GameData.Version);
            byte[] dataBuffer = GameConvert.StructToByteArray(info);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_GET_VERSION_INFO, 0, dataBuffer);
        }

        public void SendRegistVerify(string userName)
        {
            //暂时的解决方案,后面要给网狐加上这条协议
            /*if (RegistVerityEvent != null)
            {
                RegistVerityEvent(0);
            }*/

            /*CMD_GH_LogonRegVerify RegistVerify = new CMD_GH_LogonRegVerify();
            RegistVerify.dwType = (int)UserRegVerity.UserNameVerity;
            RegistVerify.dwName = userName;

            byte[] dataBuffer = GameConvert.StructToByteArray(RegistVerify);
            SendToHallSvr(CMD_HallServer.MDM_GH_LOGON, CMD_HallServer.SUB_GH_LOGON_REG_VERIFY, 0, dataBuffer);*/
        }

        public void SendUserRegist(string name, Byte gender, string password, string referrerID)
        {
            string pwd = MD5Util.GetMD5Hash(password);
            var register = new CMD_GP_RegisterAccounts();
            register.cbGender = gender;
            register.wFaceID = gender;
            register.szAccounts = name;
            register.szNickName = name;
            register.szInsurePass = pwd;
            register.szLogonPass = pwd;
            register.szSpreader = referrerID;
            register.szMachineID = GameApp.GameData.MAC;
            register.cbValidateFlags = GameHelper.GetServerSidePlatform();
            register.dwPlazaVersion = (uint)GameVersion.ProcessVersion(GameApp.GameData.Version);

            //Logger.Net.Log("send regist request|username:" + name + "     |password:" + pwd);

            byte[] dataBuffer = GameConvert.StructToByteArray(register);
            SendToHallSvr(MainCommand.MDM_GP_LOGON, SubCommand.SUB_GP_REGISTER_ACCOUNTS, 0, dataBuffer);

            //TODO 移除
            GameApp.GameData.Account = name;
            GameApp.GameData.Password = pwd;
        }

        public void SendGuestRegistQuickMessage()
        {
            /*CMD_GH_GuestRegistQuick guestRegist = new CMD_GH_GuestRegistQuick();
            guestRegist.dwName = "";
            guestRegist.dwNickName = "";
            guestRegist.dwMD5Pass = "";
            guestRegist.dwCPUID = MacAddress.GetMacAddress();
            guestRegist.dwToken = MacAddress.GetMacAddress();
            guestRegist.dwBoy = (Byte)UnityEngine.Random.Range(0, 1);
            guestRegist.dwReferrerID = "";

            byte[] dataBuffer = GameConvert.StructToByteArray(guestRegist);
            SendToHallSvr(CMD_HallServer.MDM_GH_LOGON, CMD_HallServer.SUB_GH_GUEST_REGIST, 0, dataBuffer);*/
        }

        public void SendLoginHallSvr(string wName, string wPassword, string mac)
        {
            var logon = new CMD_GP_LogonAccounts();
            logon.szAccounts = wName;
            logon.szPassword = wPassword;
            logon.szMachineID = mac;
            logon.dwPlazaVersion = (uint)GameVersion.ProcessVersion(GameApp.GameData.Version);
            logon.cbValidateFlags = GameHelper.GetServerSidePlatform();
            byte[] dataBuffer = GameConvert.StructToByteArray(logon);

            //Logger.Net.Log("send login request|username:" + wName + "|password:" + wPassword);

            SendToHallSvr(MainCommand.MDM_GP_LOGON, SubCommand.SUB_GP_LOGON_ACCOUNTS, 0, dataBuffer);
        }

        public void SendGetRoomInfoRequest(UInt32 wKindId, UInt32 wNameId)
        {
            ushort kind = (ushort)wNameId;
            byte[] dataBuffer = BitConverter.GetBytes(kind);
            SendToHallSvr(MainCommand.MDM_GP_SERVER_LIST, SubCommand.SUB_GP_GET_SERVER, 0, dataBuffer);
        }

        public void SendGameListRequest()
        {
            /*SendToHallSvr(CMD_HallServer.MDM_GH_GAME_LIST, CMD_HallServer.SUB_GH_LIST_KIND, 0, null);*/
        }
        //绑定请求
        public void SendLockAccountRequest(string dwPassword, UInt32 dwCommanType)
        {
            var LockAccount = new CMD_GP_ModifyMachine();
            LockAccount.cbBind = (byte)dwCommanType;
            LockAccount.dwUserID = GameApp.GameData.UserInfo.UserID;
            LockAccount.szPassword = MD5Util.GetMD5Hash(dwPassword);
            LockAccount.szMachineID = GameApp.GameData.MAC;

            byte[] dataBuffer = GameConvert.StructToByteArray(LockAccount);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_LOCK_OR_UNLOCK_ACCOUNT, 0, dataBuffer);
        }

        //发送游戏流水日志
        public void SendGameRecordRequest(uint kind, uint page, uint pageSize, ulong time)
        {
            var Request = new CMD_GP_GameRecordRequest();
            Request.dwUserID = GameApp.GameData.UserInfo.UserID;
            Request.dwGameKind = kind;
            Request.dwPage = page;
            Request.dwPageSize = pageSize;
            Request.dwTime = time;

            byte[] dataBuffer = GameConvert.StructToByteArray(Request);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_GAME_RECORD, 0, dataBuffer);
        }

        //发送登陆流水日志
        public void SendLogonRecordRequest()
        {
            var Logon = new CMD_GP_UserRequest();
            Logon.dwUserID = GameApp.GameData.UserInfo.UserID;
            byte[] dataBuffer = GameConvert.StructToByteArray(Logon);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_LOGON_RECORD, 0, dataBuffer);
        }

        //发送资料请求
        public void SendGetUserInfoRequest()
        {
            var req = new CMD_GP_QueryIndividual();
            req.dwUserID = GameApp.GameData.UserInfo.UserID;
            byte[] dataBuffer = GameConvert.StructToByteArray(req);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_QUERY_INDIVIDUAL, 0, dataBuffer);
        }

        //修改资料
        public void SendChangeUserInformation(string name, string nickName, string phone, string im, uint head,string UnderWrite)
        {
            var req = new CMD_GP_ModifyIndividual();
            req.cbGender = GameApp.GameData.UserInfo.Gender;
            req.dwUserID = GameApp.GameData.UserInfo.UserID;
            req.szPassword = GameApp.GameData.Password;

            GameApp.GameData.TempNickName = nickName;

            byte[] dataBuffer = GameConvert.StructToByteArray(req);
            var buffer = ByteBufferPool.PopPacket(dataBuffer);
            if (!string.IsNullOrEmpty(nickName))
            {
                ProtoHelper.AppendDescDataString(ref buffer, CommonDefine.DTP_GP_UI_NICKNAME, nickName/*GameApp.GameData.UserInfo.NickName*/);                
            }
            //ProtoHelper.AppendDescDataString(ref buffer, CommonDefine.DTP_GP_UI_USER_NOTE, ident);
            if (!string.IsNullOrEmpty(name))
            {
                ProtoHelper.AppendDescDataString(ref buffer, CommonDefine.DTP_GP_UI_COMPELLATION, name);
            }
            if (!string.IsNullOrEmpty(im))
            {
                ProtoHelper.AppendDescDataString(ref buffer, CommonDefine.DTP_GP_UI_QQ, im);
            }
            if (!string.IsNullOrEmpty(phone))
            {
                ProtoHelper.AppendDescDataString(ref buffer, CommonDefine.DTP_GP_UI_MOBILE_PHONE, phone);  
            }
            if (!string.IsNullOrEmpty(UnderWrite))
			{
            	ProtoHelper.AppendDescDataString(ref buffer, CommonDefine.DTP_GP_UI_UNDER_WRITE, UnderWrite);
			}

            var data = buffer.ToByteArray();
            ByteBufferPool.DropPacket(buffer);

            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_MODIFY_INDIVIDUAL, 0, data);
        }

        //修改头像
        public void SendChangeUserFace(string name, string ident, string phone, string im, uint head)
        {
            var req = new CMD_GP_SystemFaceInfo();
            req.wFaceID = (UInt16)head;
            req.dwUserID = GameApp.GameData.UserInfo.UserID;
            req.szPassword = GameApp.GameData.Password;
            req.szMachineID = GameApp.GameData.MAC;


            byte[] dataBuffer = GameConvert.StructToByteArray(req);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_SYSTEM_FACE_INFO, 0, dataBuffer);
        }

        //发送玩家反馈
        public void SendUserSuggestion(string type, string suggest, string phone)
        {
            var UserSuggestion = new CMD_GP_UserSuggestion();
            UserSuggestion.dwUserID = GameApp.GameData.UserInfo.UserID;
            UserSuggestion.dwType = type;
            UserSuggestion.dwUserSuggestion = suggest;
            UserSuggestion.dwCellPhone = phone;

            byte[] dataBuffer = GameConvert.StructToByteArray(UserSuggestion);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_USER_SUGGESTION, 0, dataBuffer);
        }

        //发送获取充值信息请求
        public void SendRechange()
        {
            var Request = new CMD_GP_UserRequest();
            Request.dwUserID = GameApp.GameData.UserInfo.UserID;
            byte[] dataBuffer = GameConvert.StructToByteArray(Request);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_RECHANGE_INFO, 0, dataBuffer);
        }

        //发送获取兑换信息请求
        public void SendExchange()
        {
            var Request = new CMD_GP_UserRequest();
            Request.dwUserID = GameApp.GameData.UserInfo.UserID;
            byte[] dataBuffer = GameConvert.StructToByteArray(Request);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_EXCHANGE_INFO, 0, dataBuffer);
        }

        //发送刷新用户信息请求
        public void SendRefreshUserInfo()
        {
            var Request = new CMD_GP_UserRequest();
            Request.dwUserID = GameApp.GameData.UserInfo.UserID;
            byte[] dataBuffer = GameConvert.StructToByteArray(Request);
            SendToHallSvr(MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_REFRASH_USER_INFO, 0, dataBuffer);
        }
        #endregion

        #region 请求函数

        ///发送打开保险柜请求
        public void SendOpenSafetyBoxRequest(string dwPassword)
        {
            var safetyBoxVerity = new CMD_GH_SafetyBoxVerify();
            safetyBoxVerity.dwUserID = GameApp.GameData.UserInfo.UserID;
            safetyBoxVerity.dwMD5Pass = MD5Util.GetMD5Hash(dwPassword);

            byte[] dataBuffer = GameConvert.StructToByteArray(safetyBoxVerity);
            GameApp.Network.SendToSvr(ConnectionID.Lobby, MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_SAFETYBOX_VERIFY, 0, dataBuffer);
        }

        ///发送赠送金币请求
        public void SendPressGlodRequest(string dwID, int dwMoney)
        {
            /*CMD_GH_TransMonbey transMoney = new CMD_GH_TransMonbey();
            transMoney.dwDestName = dwID;
            transMoney.dwMoney = dwMoney;
            transMoney.dwMD5PassNew = MD5Util.GetMD5Hash("123456");//暂时写死

            byte[] dataBuffer = GameConvert.StructToByteArray(transMoney);

            GameApp.Network.SendToHallSvr(CMD_HallServer.MDM_GH_MONEY, CMD_HallServer.SUB_GP_TRANS_MONEY, 0, dataBuffer);*/
        }

        ///发送存钱请求
        public void SendCheckInMoneyRequest(Int64 dwMoney)
        {
            var saveScore = new CMD_GP_UserSaveScore();
            saveScore.lSaveScore = dwMoney;
            saveScore.szMachineID = GameApp.GameData.MAC;
            saveScore.dwUserID = GameApp.GameData.UserInfo.UserID;

            byte[] dataBuffer = GameConvert.StructToByteArray(saveScore);
            GameApp.Network.SendToSvr(ConnectionID.Lobby, MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_USER_SAVE_SCORE, 0,
                                          dataBuffer);
        }

        ///发送取钱请求
        public void SendCheckOutMoneyRequest(Int64 dwMoney, string dwPassword)
        {
            var takeScore = new CMD_GP_UserTakeScore();
            takeScore.lTakeScore = dwMoney;
            takeScore.dwUserID = GameApp.GameData.UserInfo.UserID;
            takeScore.szPassword = MD5Util.GetMD5Hash(dwPassword);
            takeScore.szMachineID = GameApp.GameData.MAC;

            byte[] dataBuffer = GameConvert.StructToByteArray(takeScore);
            GameApp.Network.SendToSvr(ConnectionID.Lobby, MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_USER_TAKE_SCORE, 0, dataBuffer);
        }

        ///发送修改密码请求
        public void SendChangePassWDRequest(int type, string dwOldPassword, string dwPassword)
        {
            if (type == 0)//0表示修改登录密码 1表示修改银行密码
            {
                var modifyPasswd = new CMD_GP_ModifyLogonPass();
                modifyPasswd.dwUserID = GameApp.GameData.UserInfo.UserID;
                modifyPasswd.szScrPassword = MD5Util.GetMD5Hash(dwOldPassword);
                modifyPasswd.szDesPassword = MD5Util.GetMD5Hash(dwPassword);

                GameApp.GameData.TempPassword = MD5Util.GetMD5Hash(dwPassword);

                byte[] dataBuffer = GameConvert.StructToByteArray(modifyPasswd);
                GameApp.Network.SendToSvr(ConnectionID.Lobby, MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_CHANGE_PASSWD, 0, dataBuffer);

            }
            else
            {
                var modifyPasswd = new CMD_GP_ModifyInsurePass();
                modifyPasswd.dwUserID = GameApp.GameData.UserInfo.UserID;
                modifyPasswd.szScrPassword = MD5Util.GetMD5Hash(dwOldPassword);
                modifyPasswd.szDesPassword = MD5Util.GetMD5Hash(dwPassword);
                byte[] dataBuffer = GameConvert.StructToByteArray(modifyPasswd);
                GameApp.Network.SendToSvr(ConnectionID.Lobby, MainCommand.MDM_GP_USER_SERVICE, SubCommand.SUB_GP_CHANGE_BANK_PASSWD, 0, dataBuffer);
            }
        }

        ///发送金币记录数据 暂时没用
        public void SendMoneyRecordRequest()
        {
            // GameApp.Network.SendToHallSvr(CMD_HallServer.MDM_GP_USERREFLASH, CMD_HallServer.SUB_GH_FETCH_MONEY_RECORD, 0, null);
        }

        #endregion
    }
}
