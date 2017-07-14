using UnityEngine;
using System;
using System.Collections.Generic;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.NetFox;
using System.Collections;

namespace com.QH.QPGame.Lobby.Surfaces
{
    public class SurfaceRoom : Surface 
    {
		// Use this for initialization
		void Start () 
		{
            RegisterEvent();
            Resume();
        }

		void OnDestroy()
		{
			UnRegisterEvent();
		}

        private void Resume()
        {
            if (GameApp.GameData.EnterRoomID != 0)
            {
                OnSendAllDeskInfo(GameApp.GameData.EnterRoomID, false);
            }

            if (GameApp.GameData.UserInfo.CutRoomID > 0)
            {
                OnSendUserRelogin((UInt32)GameApp.GameData.UserInfo.CutRoomID, true);
            }
        }

        //注册事件
		private void RegisterEvent()
        {
            #region 修改后
            GameApp.GameSrv.GameCreatedEvent += OnSendAllDeskInfo;//桌子信息事件
            GameApp.GameSrv.UserSitDownEvent += OnSendUserSitInfo;//玩家坐桌信息事件
            GameApp.GameSrv.UserStandUpEvent += OnSendUserUpInfo;//玩家离开桌子事件
            GameApp.GameSrv.UserLeftEvent += OnSendUserUpInfo;//玩家离开桌子事件
            GameApp.GameSrv.GameUserUpdatedEvent += OnSendUserInfo;//玩家数据事件
            GameApp.GameSrv.GameLogonErrorEvent += OnSendGameLogonError;//发送游戏登陆错误信息事件
            GameApp.GameSrv.UserReloginGameRoomEvent += OnSendUserRelogin;//注册用户已经在房间事件
            GameApp.GameSrv.SystemMessageEvent += Instance_SystemMessageEvent;

            //没有用到
            //GameApp.GameSrv.UserStandUpEvent += OnQuitRoomDesk;//玩家身上钱不够

            GameApp.GameSrv.UserSitErrorEvent += OnSitDownError;//玩家坐下错误事件
            GameApp.GameSrv.DeskPlayStatuseEvent += OnSendDeskPlayStatuse;//桌子状态事件
            #endregion

            HallTransfer.Instance.ncGameChairClick += NcGameRoomSit;//注册位置事件
            HallTransfer.Instance.ncQuitRoomDesk += NcQuitRoomDesk;//注册退出捕鱼房间事件
            HallTransfer.Instance.ncGameRoomClick += NcGameRoomClick;//注册房间按钮事件

		}

		//销毁注册事件
		private void UnRegisterEvent()
		{
            #region 修改后
            if (GameProtocol.GetInstance() != null)
		    {
                GameApp.GameSrv.GameCreatedEvent -= OnSendAllDeskInfo;//桌子信息事件
                GameApp.GameSrv.UserSitDownEvent -= OnSendUserSitInfo;//玩家坐桌信息事件
                GameApp.GameSrv.UserStandUpEvent -= OnSendUserUpInfo;//玩家离开桌子事件
                GameApp.GameSrv.UserLeftEvent -= OnSendUserUpInfo;//玩家离开桌子事件
                GameApp.GameSrv.GameUserUpdatedEvent -= OnSendUserInfo;//玩家数据事件
                GameApp.GameSrv.GameLogonErrorEvent -= OnSendGameLogonError;//发送游戏登陆错误信息事件
                GameApp.GameSrv.UserReloginGameRoomEvent -= OnSendUserRelogin;//注册用户已经在房间事件
                GameApp.GameSrv.SystemMessageEvent -= Instance_SystemMessageEvent;

                //没有用到
                //GameApp.GameSrv.UserStandUpEvent -= OnQuitRoomDesk;//玩家身上钱不够

                GameApp.GameSrv.UserSitErrorEvent -= OnSitDownError;//玩家坐下错误事件
                GameApp.GameSrv.DeskPlayStatuseEvent -= OnSendDeskPlayStatuse;//桌子状态事件
		    }
            #endregion

		    if (HallTransfer.Instance != null)
		    {
                HallTransfer.Instance.ncGameChairClick -= NcGameRoomSit;//注册位置事件
                HallTransfer.Instance.ncQuitRoomDesk -= NcQuitRoomDesk;//注册退出捕鱼房间事件
                HallTransfer.Instance.ncGameRoomClick -= NcGameRoomClick;//注册房间按钮事件
                //HallProtocol.Instance.ReconnectGameEvent -= OnSendUserRelogin;//注册用户已经在房间事件
		    }

		}

        public void OnSendUserInfo()//玩家数据改变发送
        {
            HallTransfer.MySelfInfo tempInfo = new HallTransfer.MySelfInfo();
            tempInfo.dwUserId = GameApp.GameData.UserInfo.UserID;
            tempInfo.dwNickName = GameApp.GameData.UserInfo.NickName;
            tempInfo.dwMoney = GameApp.GameData.UserInfo.CurMoney;
            tempInfo.dwLockMathine = GameApp.GameData.UserInfo.MoorMachine;
            tempInfo.dwVip = GameApp.GameData.UserInfo.Vip;
            tempInfo.dwRoomID = GameApp.GameData.EnterRoomID;
            tempInfo.dwDeskNo = GameApp.GameData.UserInfo.LastDeskNO;
            tempInfo.dwDeskStation = GameApp.GameData.UserInfo.LastDeskStation;
			tempInfo.dwInsureMoney = GameApp.GameData.UserInfo.CurBank;
            if (GameApp.GameData.UserInfo.HeadId == 0)
            {
                if (!GameApp.GameData.UserInfo.IsBoy)
                {
                    tempInfo.dwHeadID = 1;
                }
                else
                {
                    tempInfo.dwHeadID = 0;
                }
            }
            else
            {
                tempInfo.dwHeadID = GameApp.GameData.UserInfo.HeadId;
            }
            HallTransfer.Instance.cnSetUserInfo(tempInfo);//发送玩家数据
        }

        public void OnSendAllDeskInfo(UInt32 wRoomId, bool playing)
        {
            if (playing)
            {
                return;
            }

            var item = GameApp.GameListMgr.FindRoomItem(wRoomId);
            if (item == null || item.AutoSit)
            {
                return;
            }

            var tempDesk = new HallTransfer.RoomDeskInfo();
            tempDesk.dwGameID = item.GameNameID;
            tempDesk.dwRoomId = item.ID;
            tempDesk.dwDeskCount = item.DeskCount;
            tempDesk.dwDeskPeople = item.DeskPeople;

            //房间桌子信息
//			HallTransfer.Instance.cnSetRoomInfo(tempDesk);//发送桌子数据
			CGameDeskManger._instance.SetGameDeskInfo(tempDesk);//发送桌子数据
            SendAllUserSitInfo();

            // 发送桌子状态
            foreach (var tempItem in item.Desks)
            {
                OnSendDeskPlayStatuse(tempItem.DeskID, tempItem.Status);
            }
        }

        //用户列表中用户坐下信息
        private void SendUserSitInfo(PlayerInfo player)
        {
            HallTransfer.RoomUserInfo tempRoomUserInfo = new HallTransfer.RoomUserInfo();
            tempRoomUserInfo.dwUserId = player.ID;
            tempRoomUserInfo.dwNickName = player.NickName;
            tempRoomUserInfo.dwLogoID = player.HeadID;
            tempRoomUserInfo.dwMoney = player.Money;
            tempRoomUserInfo.dwDesk = player.DeskNO;
            tempRoomUserInfo.dwChair = player.DeskStation;
            tempRoomUserInfo.dwGameCount = 10;//暂时写死
            HallTransfer.Instance.cnShowUserInfo(tempRoomUserInfo);//发送其他在线玩家数据*/
        }

        private void SendAllUserSitInfo()
        {
            PlayerInfo[] players = GameApp.GameSrv.GetAllPlayers();

            List<HallTransfer.RoomUserInfo> AllUserInfo = new List<HallTransfer.RoomUserInfo>();
            foreach (PlayerInfo temp in players)
            {
                if (temp.DeskNO != CommonDefine.INVALID_TABLE && temp.DeskStation != CommonDefine.INVALID_TABLE)
                {
                    HallTransfer.RoomUserInfo tempRoomUserInfo = new HallTransfer.RoomUserInfo();
                    tempRoomUserInfo.dwUserId = temp.ID;
                    tempRoomUserInfo.dwNickName = temp.NickName;
                    tempRoomUserInfo.dwLogoID = temp.HeadID;
                    tempRoomUserInfo.dwMoney = temp.Money;
                    tempRoomUserInfo.dwDesk = temp.DeskNO;
                    tempRoomUserInfo.dwChair = temp.DeskStation;
                    tempRoomUserInfo.dwGameCount = 10;//暂时写死
                    AllUserInfo.Add(tempRoomUserInfo);
                }
            }
            HallTransfer.Instance.cnShowUserInfo(AllUserInfo);
        }

        //游戏中用户坐下
        public void OnSendUserSitInfo(uint uid, ushort desk, ushort chairs)
        {
            if (GameApp.GameData.EnterRoomID == 0)
            {
                return;
            }


            SGameRoomItem item = GameApp.GameListMgr.FindRoomItem(GameApp.GameData.EnterRoomID);
            if (item == null || item.AutoSit)
            {
                return;
            }

            if (desk != CommonDefine.INVALID_TABLE && chairs != CommonDefine.INVALID_CHAIR)
            {
                PlayerInfo temp = GameApp.GameSrv.FindPlayer(uid);
                HallTransfer.RoomUserInfo tempRoomUserInfo = new HallTransfer.RoomUserInfo();
                tempRoomUserInfo.dwUserId = uid;
                tempRoomUserInfo.dwNickName = temp.NickName;
                tempRoomUserInfo.dwLogoID = temp.HeadID;
                tempRoomUserInfo.dwMoney = temp.Money;
                tempRoomUserInfo.dwDesk = desk;
                tempRoomUserInfo.dwChair = chairs;
                tempRoomUserInfo.dwGameCount = 10;//暂时写死
                HallTransfer.Instance.cnShowUserInfo(tempRoomUserInfo);//发送进入玩家数据
            }
        }

        public void OnSitDownError(int HandlerCode, string Msg)
        {
            HallTransfer.Instance.cnUserSitError(HandlerCode, Msg);//玩家坐下错误
        }

        //游戏中用户起立
        public void OnSendUserUpInfo(UInt32 wUserId)
        {
            if (GameApp.GameData.EnterRoomID == 0)
            {
                return;
            }

            var item = GameApp.GameListMgr.FindRoomItem(GameApp.GameData.EnterRoomID);
            if (item == null || item.AutoSit)
            {
                return;
            }

            HallTransfer.Instance.cnHideUserInfo(wUserId);//发送离开玩家数据
        }

        public void OnSendGameLogonError(int wHandleCode, string desc)
        {
            //发送游戏登陆错误信息
            HallTransfer.Instance.cnGameLogonError(wHandleCode, desc);
            GameApp.Network.CloseConnect(ConnectionID.Game);
        }

        public void OnSendUserRelogin(UInt32 roomID, bool showTips)
        {
            var item = GameApp.GameListMgr.FindRoomItem(roomID);
            if (item == null)
            {
				HallTransfer.Instance.cnCloseWebpage();
                GameApp.PopupMgr.Confirm("提示",
                    "您的账号被锁定在房间：" + roomID + "，请联系客服。", 
                    style => GameApp.GetInstance().SwitchAccount());
                return;
            }


#if !UNITY_EDITOR || TEST_AB
            var gameID = (int)item.GameNameID;
            bool installed = GameApp.GameMgr.IsGameInstalled(gameID);
            bool needUpdate = GameApp.GameMgr.IsGameNeedUpdate(gameID);
            if (!installed || needUpdate)
            {
				HallTransfer.Instance.cnCloseWebpage();
                GameApp.PopupMgr.MsgBox(
                    !installed ? "游戏未安装" : "游戏需要更新",
                    "您正在" + item.Name + "，是否下载继续游戏?",
                    ButtonStyle.Cancel | ButtonStyle.OK,
                    delegate(MessageBoxResult result)
                    {
                        switch (result)
                        {
                            case MessageBoxResult.Timeout:
                            case MessageBoxResult.Cancel:
                                {
                                    break;
                                }
                            case MessageBoxResult.OK:
                                {
                                    var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
                                    SurfaceDownload download = container.GetSurface<SurfaceDownload>();
                                    download.AddTask(gameID, delegate(int i)
                                    {
                                        NcGameRoomClick(roomID);
                                    });
                                    break;
                                }
                        }
                    },
                    10f);

                return;
            }
#endif

            //发送房间号
            if (showTips)
            {
				HallTransfer.Instance.cnCloseWebpage();
                GameApp.PopupMgr.MsgBox("提示", "您正在" + item.Name + "，是否继续游戏？",
                            ButtonStyle.Cancel | ButtonStyle.OK,
                            delegate(MessageBoxResult result)
                            {
                                switch (result)
                                {
                                    case MessageBoxResult.Timeout:
                                    case MessageBoxResult.Cancel:
                                        {
                                            break;
                                        }
                                    case MessageBoxResult.OK:
                                        {
                                            NcGameRoomClick(roomID);
                                            break;
                                        }
                                }
                            },
                            10f);
            }
            else
            {
                NcGameRoomClick(roomID);
            }
        }


        public void OnSendDeskPlayStatuse(UInt32 deskID, byte status)
        {
            //判断是否在游戏场景中
            if (GameApp.GameData.EnterRoomID == 0)
            {
                return;
            }

            var item = GameApp.GameListMgr.FindRoomItem(GameApp.GameData.EnterRoomID);
            if (item == null || item.AutoSit)
            {
                return;
            }

            if (GameApp.GameMgr.GetARunningAgent() != null)
            {
                return;
            }
            HallTransfer.Instance.cnSetDeskStatus(deskID, status);
			CGameDeskManger._instance.cnSetDeskStatus(deskID, status);
        }


        public void NcGameRoomClick(UInt32 roomID)
        {
            Logger.UI.Log("NcGameRoomClick:" + roomID);

            GameApp.GameMgr.DestoryGame(true);

            var item = GameApp.GameListMgr.FindRoomItem(roomID);
            if (item == null)
            {
                Logger.UI.LogError("no match room id:" + roomID);
                return;
            }

#if !UNITY_EDITOR || TEST_AB
            if (!GameApp.GameMgr.IsGameInstalled((int)item.GameNameID) ||
                GameApp.GameMgr.IsGameNeedUpdate((int)item.GameNameID))
            {
                Logger.UI.LogError("the game needs update or download GameID:" + item.GameNameID);
                return;
            }
#endif

            GameApp.GameMgr.CreateGame(item);
        }

        public void NcGameRoomSit(UInt32 RoomID, UInt32 Desk, UInt32 Chair)
        {
            //判断是否在游戏中
            if (GameApp.GameMgr.IsInGame())
            {
                return;
            }
            GameApp.GameSrv.SendUserSitDown(RoomID, Desk, Chair, GameApp.GameData.Password);
        }

        public void NcQuitRoomDesk()
        {
            GameApp.GameMgr.DestoryGame(false);
        }


        private void Instance_SystemMessageEvent(ushort wType, string msg)
        {
            if (((wType & (ushort)MsgType.MT_INFO) != 0) ||
                ((wType & (ushort)MsgType.MT_GLOBAL) != 0) ||
                ((wType & (ushort)MsgType.MT_EJECT) != 0))
            {
                if ((wType & (ushort)MsgType.MT_CLOSE_ROOM) != 0 ||
                (wType & (ushort)MsgType.MT_CLOSE_LINK) != 0)
                {
                    GameApp.PopupMgr.Confirm("系统消息", msg, delegate(MessageBoxResult result)
                    {
                        GameApp.GameMgr.DestoryGame(true);
                    }, 3.0f);
                }
                else if ((wType & (ushort)MsgType.MT_CLOSE_GAME) != 0)
                {
                    GameApp.PopupMgr.Confirm("系统消息", msg, delegate(MessageBoxResult result)
                    {
                        GameApp.GameMgr.DestoryGame(false);
                    });
                }
                else
                {
                    GameApp.PopupMgr.Confirm("系统消息", msg, null, 3.0f);
                }
            }
        }
    }

}

