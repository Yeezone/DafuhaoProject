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
	// 充值
	[System.Serializable]
	public class Recharge
	{
		public UILabel Label_money;
	}

	// 充值记录
	[System.Serializable]
	public class RechargeRecords
	{
		public UILabel page_label;
		public UILabel count_label;
		public SingleRecordConfig[] singleRecordConfigs; 	
	}

	// 兑奖
	[System.Serializable]
	public class Award
	{
		public UILabel Label_money;
		public UILabel Label_safebox;
		public UILabel Label_order;
	}

	// 兑奖记录
	[System.Serializable]
	public class AwardRecords
	{
		public UILabel page_label;
		public UILabel count_label;
		public SingleRecordConfig[] singleRecordConfigs; 
	}

	public class SurfaceHall : Surface 
    {
		/// <summary>
		/// 充值
		/// </summary>
		public Recharge recharge;
		/// <summary>
		/// 充值记录
		/// </summary>
		public RechargeRecords rechargeRecords;
		/// <summary>
		/// 兑奖
		/// </summary>
		public Award award;
		/// <summary>
		/// 兑奖记录
		/// </summary>
		public AwardRecords awardRecords;

		// Use this for initialization
		void Start () 
		{
            RegisterEvent();
		    Resume();
		}

        private void Resume()
	    {
            //刷新用户信息
            RefrashUserInfo();

            OnSendModulesInfo();
            //OnSendGameItem();     //通过GameID分类
            OnSendNodeItem();       //通过NodeID分类
            OnSendUserInfo();
            NcNotice();

            if (GameApp.GameData.EnterGameID != 0)
            {
                NcGameBtnClick(GameApp.GameData.EnterGameID);
            }
	    }

	    void OnDestroy()
		{
			UnRegisterEvent();
		}

		//注册事件
		private void RegisterEvent()
        {
            GameApp.Network.NetworkStatusChangeEvent += NetworkStatusChangeEvent;//Socket断链
            #region 修改后
            GameApp.Account.AccountResultEvent += OnSendAccountResult;//账号绑定结果事件
            GameApp.Account.UserSuggestionEvent += OnSendSuggestionResult;//意见反馈结果事件
            GameApp.Account.SocketCloseNotifyEvent += OnSendSocketCloseNotify;//异地登陆事件
            GameApp.Account.FrozenAccountEvent += OnFrozenAccount;//冻结账号

            //没有用到
            //GameApp.Account.RoomListFinishEvent += OnSendAllRoomInfo;
            //GameApp.Account.UpdateOnlineEvent += OnSendRoomInfo;//更新房间人数
            //GameApp.Account.MoneyRecordEvent += OnSendMoneyRecord;//金币日志事件
            //GameApp.Account.ReconnectGameEvent += OnSendUserRelogin;//注册用户已经在房间事件
            GameApp.Account.GameListFinishEvent += OnGameListFinished;

            GameApp.Account.RechangeEvent += OnSendRechange; // 充值事件
            GameApp.Account.ExchangeEvent += OnSendExchange; // 兑换事件
            GameApp.Account.UpdataOnlineCountEvent += OnSendOnlineCount;//发送房间人数事件
          
            #endregion

			HallTransfer.Instance.ncGameBtnClick += NcGameBtnClick;//注册游戏按钮事件
			HallTransfer.Instance.ncLockOrUnLockAccount += NcLockOrUnLockAccount;//注册绑定事件
			HallTransfer.Instance.ncUserSuggestion += NcUserSuggestion;//注册意见反馈
			HallTransfer.Instance.ncOfficialSite += NcOfficialSite;//注册链接官网
			HallTransfer.Instance.ncMiniWindow += NcMiniWindow;//注册最小化事件
			HallTransfer.Instance.ncMaxWindow += NcMaxWindow;//注册最大化事件
			HallTransfer.Instance.ncCloseHall += NcCloseHall;//注册关闭事件
			HallTransfer.Instance.ncChangeAccount += NcChangeAccount;//注册切换账号事件
			HallTransfer.Instance.ncLogonError += NcChangeAccount;//注册登陆错误返回事件
			HallTransfer.Instance.ncLoginComplete += NcLoadingComplete;//注册加载完成事件
			HallTransfer.Instance.ncRechargeBtnClick += NcRechargeRequest;//注册充值事件
			HallTransfer.Instance.ncAwardBtnClick += NcExchargeRequest;//注册兑奖事件
			HallTransfer.Instance.ncRechargeRecord += NcRechargeRecord;//注册充值记录事件
			HallTransfer.Instance.ncAwardRecord += NcExchargeRecord;//注册充值记录事件
			HallTransfer.Instance.ncRechargeEventClick += NcRechange;//注册充值按钮点击事件
			HallTransfer.Instance.ncAwardEventClick += NcExchange;//注册兑换按钮点击事件
			HallTransfer.Instance.ncCancelRechargeClick += NcRechangeCancel;//注册取消充值订单事件
			HallTransfer.Instance.ncCancelAwardClick += NcExchangeCancel;//注册取消兑奖订单事件
		}

		//销毁注册事件
		private void UnRegisterEvent()
		{
            GameApp.Network.NetworkStatusChangeEvent -= NetworkStatusChangeEvent;//Socket断链

            #region 修改后
            GameApp.Account.AccountResultEvent -= OnSendAccountResult;//账号绑定结果事件
   

            GameApp.Account.UserSuggestionEvent -= OnSendSuggestionResult;//意见反馈结果事件
            GameApp.Account.SocketCloseNotifyEvent -= OnSendSocketCloseNotify;//异地登陆事件
            GameApp.Account.FrozenAccountEvent -= OnFrozenAccount;//冻结账号
            GameApp.Account.GameListFinishEvent -= OnGameListFinished;

            //没有用到
            //GameApp.Account.RoomListFinishEvent -= OnSendAllRoomInfo;
            //GameApp.Account.UpdateOnlineEvent -= OnSendRoomInfo;//更新房间人数
            //GameApp.Account.MoneyRecordEvent -= OnSendMoneyRecord;//金币日志事件
            //GameApp.Account.ReconnectGameEvent -= OnSendUserRelogin;//注册用户已经在房间事件

            GameApp.Account.RechangeEvent -= OnSendRechange; // 充值事件
            GameApp.Account.ExchangeEvent -= OnSendExchange; // 兑换事件
            GameApp.Account.UpdataOnlineCountEvent -= OnSendOnlineCount;//发送房间人数事件
            #endregion
           
			if (HallTransfer.Instance != null)
			{
				HallTransfer.Instance.ncGameBtnClick -= NcGameBtnClick;//注册游戏按钮事件
				HallTransfer.Instance.ncLockOrUnLockAccount -= NcLockOrUnLockAccount;//注册绑定事件
				HallTransfer.Instance.ncUserSuggestion -= NcUserSuggestion;//注册意见反馈
				HallTransfer.Instance.ncOfficialSite -= NcOfficialSite;//注册链接官网
				HallTransfer.Instance.ncMiniWindow -= NcMiniWindow;//注册最小化事件
				HallTransfer.Instance.ncMaxWindow -= NcMaxWindow;//注册最大化事件
				HallTransfer.Instance.ncCloseHall -= NcCloseHall;//注册关闭事件
				HallTransfer.Instance.ncChangeAccount -= NcChangeAccount;//注册切换账号事件
				HallTransfer.Instance.ncLogonError -= NcChangeAccount;//注册登陆错误返回事件
				HallTransfer.Instance.ncLoginComplete -= NcLoadingComplete;//注册加载完成事件
				HallTransfer.Instance.ncRechargeBtnClick -= NcRechargeRequest;//注册充值事件
				HallTransfer.Instance.ncAwardBtnClick -= NcExchargeRequest;//注册兑奖事件
				HallTransfer.Instance.ncRechargeRecord -= NcRechargeRecord;//注册充值记录事件
				HallTransfer.Instance.ncAwardRecord -= NcExchargeRecord;//注册充值记录事件
				HallTransfer.Instance.ncRechargeEventClick -= NcRechange;//注册充值按钮点击事件
				HallTransfer.Instance.ncAwardEventClick -= NcExchange;//注册兑换按钮点击事件
				HallTransfer.Instance.ncCancelRechargeClick -= NcRechangeCancel;//注册取消充值订单事件
				HallTransfer.Instance.ncCancelAwardClick -= NcExchangeCancel;//注册取消兑奖订单事件
			}
		}

		private void RefrashUserInfo()
		{
            GameApp.Account.SendRefreshUserInfo();
		}

		public void NcLoadingComplete()
		{
			/*OnSendGameItem();
			OnSendUserInfo();
			NcNotice();*/
		}


        //通过NodeID分类
        public void OnSendNodeItem()
        {
            List<SGameNodeItem> tempGameList = GameApp.GameListMgr.FindNodeList();

            List<HallTransfer.GameInfoS> gameIDList = new List<HallTransfer.GameInfoS>();

            foreach (SGameNodeItem tempName in tempGameList)
            {
                //pc
                if (((tempName.JoinID & 0x01) != 0) && !Application.isMobilePlatform ||
                   ((tempName.JoinID & 0x02) != 0) && Application.isMobilePlatform)
                {
                    HallTransfer.GameInfoS tempNameInfo = new HallTransfer.GameInfoS();
                    tempNameInfo.TypeID = tempName.KindID;
                    tempNameInfo.ID = tempName.NodeID;
					tempNameInfo.SortID = tempName.SortID;
					tempNameInfo.Name = tempName.Name;
                    tempNameInfo.Installed = true;
                    tempNameInfo.NeedUpdate = false;
                    //tempNameInfo.Installed = GameApp.GameMgr.IsGameInstalled((int)tempName.KindID);
                   // tempNameInfo.NeedUpdate = GameApp.GameMgr.IsGameNeedUpdate((int)tempName.KindID);
				    gameIDList.Add(tempNameInfo);
				}
			}

//			HallTransfer.Instance.cnSetGameIDs(gameIDList);//发送游戏列表
			CGameManager._instance.SetGameList(gameIDList);
		}

        ////通过GameID分类
        //public void OnSendGameItem()
        //{
        //    List<SGameKindItem> tempGameList = GameApp.GameListMgr.FindKindList();

        //    List<HallTransfer.GameInfoS> gameIDList = new List<HallTransfer.GameInfoS>();

        //    foreach (SGameKindItem tempName in tempGameList)
        //    {
        //        //pc
        //        if (((tempName.JoinID & 0x01) != 0) && !Application.isMobilePlatform ||
        //           ((tempName.JoinID & 0x02) != 0) && Application.isMobilePlatform)
        //        {
        //            HallTransfer.GameInfoS tempNameInfo = new HallTransfer.GameInfoS();
        //            tempNameInfo.TypeID = tempName.KindID;
        //            tempNameInfo.ID = tempName.ID;
        //            tempNameInfo.SortID = tempName.SortID;
        //            tempNameInfo.Name = tempName.Name;
        //            gameIDList.Add(tempNameInfo);
        //        }
        //    }
        //    HallTransfer.Instance.cnSetGameIDs(gameIDList);//发送游戏列表
        //}

        //通过NodeID分类
        public void OnSendAllRoomInfo(uint NodeID)
        {
            List<SGameRoomItem> GameNameList = GameApp.GameListMgr.FindRoomListByNodeID(NodeID);

            List<HallTransfer.RoomInfoS> RoomInfo = new List<HallTransfer.RoomInfoS>();
            foreach (var GameItem in GameNameList)
            {
                SGameKindItem tempKind = GameApp.GameListMgr.FindKindItem(GameItem.GameNameID);
				if(tempKind == null)
				{
					continue;
				}
				
                //过滤配置
                if (GameApp.GameMgr.GetGameConfig((int) GameItem.GameNameID) == null)
                {
                    continue;
                }

                //过滤平台
                if (((tempKind.JoinID & 0x01) != 0) && !Application.isMobilePlatform ||
                    ((tempKind.JoinID & 0x02) != 0) && Application.isMobilePlatform)
                {
                    HallTransfer.RoomInfoS tempRoomInfo = new HallTransfer.RoomInfoS();
                    tempRoomInfo.roomId = GameItem.ID;
                    tempRoomInfo.roomName = GameItem.Name;
                    tempRoomInfo.roomPeopleCnt = GameItem.OnlineCnt;
                    tempRoomInfo.roomPeopleUplimit = GameItem.FullCount;
                    tempRoomInfo.roomDifen = GameItem.BasePoint;
                    tempRoomInfo.roomRuchang = GameItem.LessMoney2Enter;
                    RoomInfo.Add(tempRoomInfo);
                }
            }
            //HallTransfer.Instance.cnSetGameRoomInfo(NodeID, RoomInfo);//发送房间信息
			CGameRoomManger._instance.SetRoomList(NodeID,RoomInfo);
        }

       //// 通过GameID分类
       // public void OnSendAllRoomInfo( uint gameId )
       // {
       //     List<SGameRoomItem> tempGameName = GameApp.GameListMgr.FindRoomList ( gameId );
       //     //if(tempGameName == null) return;

       //     List<HallTransfer.RoomInfoS> RoomInfo = new List<HallTransfer.RoomInfoS>();

       //     foreach (SGameRoomItem tempInfo in tempGameName) 
       //     {
       //         HallTransfer.RoomInfoS tempRoomInfo=new HallTransfer.RoomInfoS();
       //         tempRoomInfo.roomId = tempInfo.ID;
       //         tempRoomInfo.roomName = tempInfo.Name;
       //     //	Debug.LogWarning ("tempInfo.Name"+tempInfo.Name);
       //         tempRoomInfo.roomPeopleCnt = tempInfo.OnlineCnt;
       //         tempRoomInfo.roomPeopleUplimit = tempInfo.FullCount;
       //         tempRoomInfo.roomDifen = tempInfo.BasePoint;
       //         tempRoomInfo.roomRuchang = tempInfo.LessMoney2Enter;
       //         RoomInfo.Add( tempRoomInfo );
       //     }
       //     HallTransfer.Instance.cnSetGameRoomInfo(gameId, RoomInfo);//发送房间信息
       // }

		public void OnSendAccountResult( UInt32 dwCommanType, UInt32 dwCommanResult )
		{
			//发送绑定结果给UI,解绑定
			HallTransfer.Instance.cnAccountResult( dwCommanType, dwCommanResult);
		}

        public void OnSendMoneyRecord(List<CMD_GH_MoneyRecord> MoneyRecordList)
		{
			//发送金钱流水
//			HallTransfer.Instance.cnMoneyRecord(MoneyRecordList);
		}


		public void OnSendSuggestionResult(int wHandleCode)
		{
			//发送反馈结果
			HallTransfer.Instance.cnSuggestionResult(wHandleCode);
		}

        public void OnGameListFinished()
        {
            OnSendNodeItem();

            if (GameApp.GameData.EnterGameID != 0)
            {
                NcGameBtnClick(GameApp.GameData.EnterGameID);
            }
        }

	    public void OnFrozenAccount()
		{
			//账号被冻结
            GameApp.GameMgr.DestoryGame(true);
            HallTransfer.Instance.cnCloseWebpage();
            GameApp.PopupMgr.Confirm("提示", "账号被冻结，请通知客服解冻!", delegate(MessageBoxResult style)
            {
                GameApp.GetInstance().SwitchAccount();
            }, 15);
		}

		private bool bShowSocketTips = true;
		public void OnSendSocketCloseNotify( int wSubCmdID)
		{
			//在其他地方登陆
			bShowSocketTips = false;

            GameApp.GameMgr.DestoryGame(true);
            HallTransfer.Instance.cnCloseWebpage();
            GameApp.PopupMgr.Confirm("提示", "账号在别处登录，请检查账户安全性!", delegate(MessageBoxResult style)
            {
                GameApp.GetInstance().SwitchAccount();
            }, 15);
		}

        private void NetworkStatusChangeEvent(ConnectionID socket, NetworkManager.Status wError)
		{
            if (wError != NetworkManager.Status.Connected)
		    {
                GameApp.GameMgr.DestoryGame(true);

		        if (bShowSocketTips == false)
                {
                    return;
                }

                if (socket == ConnectionID.Lobby)
                {
                    HallTransfer.Instance.cnCloseWebpage();
                    GameApp.PopupMgr.Confirm("提示", "为了您的账户安全,请重新登录!", delegate(MessageBoxResult style)
                    {
                        GameApp.GetInstance().SwitchAccount();
                    }, 15);
                }
                else
                {
                    HallTransfer.Instance.cnCloseWebpage();
                    GameApp.PopupMgr.Confirm("提示", 
                        wError == NetworkManager.Status.TimeOut ? "连接超时,请重试!" : "与游戏服务器断开连接,请重试!", 
                        delegate(MessageBoxResult style)
                    {
                        GameApp.GetInstance().SwitchAccount();
                    }, 15);
                }
		    }
		}


		public void OnSendRechange( Int64 MinMoney, int ChangeScale )
		{
			cnRechargeMsg( MinMoney );
		}

		public void OnSendExchange(  Int64 MinMoney, int ChangeScale, Int64 Withdrawals )
		{
			HallTransfer.AwardMsg temp = new HallTransfer.AwardMsg();
			temp.dwAwardMoney = Withdrawals;
			temp.dwLowestMoney = MinMoney;
			Int64 tempBankMoney = GameApp.GameData.UserInfo.CurBank / ChangeScale;
			temp.dwSafeMoney = tempBankMoney;
			HallTransfer.Instance.cnAwardMsg( temp );
		}

        public void OnSendOnlineCount()
        {
            List<SGameKindItem> tempNameItem = GameApp.GameListMgr.FindKindList();

            foreach (var GameNameItem in tempNameItem)
            {
                List<SGameRoomItem> tempGameName = GameApp.GameListMgr.FindRoomList(GameNameItem.ID);
                List<HallTransfer.RoomInfoS> RoomInfo = new List<HallTransfer.RoomInfoS>();
                foreach (SGameRoomItem tempInfo in tempGameName)
                {
                    var tempRoomInfo = new HallTransfer.RoomInfoS();
                    tempRoomInfo.roomId = tempInfo.ID;
                    tempRoomInfo.roomName = tempInfo.Name;
                    tempRoomInfo.roomPeopleCnt = tempInfo.OnlineCnt;
                    tempRoomInfo.roomPeopleUplimit = tempInfo.FullCount;
                    tempRoomInfo.roomDifen = tempInfo.BasePoint;
                    tempRoomInfo.roomRuchang = tempInfo.LessMoney2Enter;
                    RoomInfo.Add(tempRoomInfo);
                }
                HallTransfer.Instance.cnOnlineCount(GameNameItem.ID, RoomInfo);
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

	    public void OnSendModulesInfo()
	    {
	        bool fromLogin = GameApp.SceneMgr.LastScene == GlobalConst.UI.UI_SCENE_LOGIN;
            var modules = GameApp.ModuleMgr.GetModulesOfKind(ModuleKind.UI);
            HallTransfer.Instance.cnSetModulesInfo(modules, fromLogin);
	    }


//UI	------------------------------------------------------
	    public void NcGameBtnClick(UInt32 nodeID)
	    {
	        if (GameApp.GameData.EnterGameID != nodeID)
	        {
                GameApp.GameMgr.DestoryGame(true);
	        }

            var node = GameApp.GameListMgr.FindNodeItem(nodeID);
            var rooms = GameApp.GameListMgr.FindRoomListByNodeID(nodeID);
            if (node == null || rooms.Count == 0)
	        {
                HallTransfer.Instance.cnCloseWebpage();
                GameApp.PopupMgr.Confirm("提示", "游戏尚未开放，请稍后重试");
	            return;
	        }

#if !UNITY_EDITOR || TEST_AB
            var needsDownloadGames = new List<int>();
            var needsUpdateGames = new List<int>();
	        foreach (var item in rooms)
	        {
                int gameID = (int)item.GameNameID;
	            if (!GameApp.GameMgr.IsGameExists(gameID))
	            {
	                continue;
	            }

                bool installed = GameApp.GameMgr.IsGameInstalled(gameID);
                bool needUpdate = GameApp.GameMgr.IsGameNeedUpdate(gameID);
	            if (!installed)
	            {
                    Logger.UI.Log("add download needs game:"+gameID);
                    needsDownloadGames.Add(gameID);
	            }
                else if(needUpdate)
                {
                    Logger.UI.Log("add updatable game:" + gameID);
                    needsUpdateGames.Add(gameID);
                }
	        }

	        //if (!installed || needUpdate)
	        if (needsUpdateGames.Count > 0 || needsDownloadGames.Count > 0)
	        {
                string title = needsDownloadGames.Count > 0 ? "游戏 " + node.Name + " 未安装,是否下载?" : "游戏 " + node.Name + "需要更新,是否更新?";
                HallTransfer.Instance.cnCloseWebpage();
                GameApp.PopupMgr.MsgBox("提示", title, ButtonStyle.Cancel | ButtonStyle.OK, 
                    delegate(MessageBoxResult result)
                    {
                        switch (result)
                        {
                            case MessageBoxResult.OK:
                                {
                                    var allGameIDs = new List<int>();
                                    allGameIDs.AddRange(needsUpdateGames);
                                    allGameIDs.AddRange(needsDownloadGames);

                                    var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
                                    var download = container.GetSurface<SurfaceDownload>();
                                    download.AddTasks(allGameIDs, delegate(int i)
                                    {
                                        GameApp.GameData.EnterGameID = nodeID;
                                        OnSendAllRoomInfo(nodeID);
                                    });

                                    break;
                                }
                        }
                    }, 
                    10f);
	        }
	        else
            {
                GameApp.GameData.EnterGameID = (uint)nodeID;
                OnSendAllRoomInfo((uint)nodeID);
	        }
#else
            GameApp.GameData.EnterGameID = (uint)nodeID;
            OnSendAllRoomInfo((uint)nodeID);
#endif
        }


		public void NcLockOrUnLockAccount( string dwPassword, UInt32 dwCommanType )
		{
			//绑定与解绑定
            GameApp.Account.SendLockAccountRequest(dwPassword, dwCommanType);
		}

		public void NcMiniWindow()
		{
            //最小化窗口
            MinimizeWindow();
        }

		public void NcMaxWindow()
		{
            //最大化窗口
            SwitchFullScreen();
		}

		public void NcCloseHall()
		{
            GameApp.GetInstance().Quit();
		}

		public void NcChangeAccount()
		{
            GameApp.GetInstance().SwitchAccount();
		}

        public void NcUserSuggestion(HallTransfer.UserSuggestion suggestion)
		{
			//问题反馈
            GameApp.Account.SendUserSuggestion(
                suggestion.dwType, 
                suggestion.dwUserSuggestion, 
                suggestion.dwCellPhone);
		}

		public void NcOfficialSite()
		{
			//连接官网
            GotoOfficeSite();
		}


		
		public void NcRechange()
		{
            GameApp.Account.SendRechange();
		}

		public void NcExchange()
		{
            GameApp.Account.SendExchange();
		}

		//充值
		public void NcRechargeRequest( HallTransfer.RechargeRequest tempRecharge )
	    {
            uint UserID = GameApp.GameData.UserInfo.UserID;
			Int64 Money = tempRecharge.dwMoney;
			string 	dwNote = tempRecharge.dwRemark;
			string	Key = GameApp.GameData.PrivateKey;
			GameApp.BackendSrv.Recharge(UserID, Money, dwNote,Key, b =>
				                                {
				if( b != null )
				{
					HallTransfer.Instance.cnRechargeResult( b.Code, b.Msg);
				}
				});
	    }

		//兑奖
		public void NcExchargeRequest( HallTransfer.ExchangeRequest tempRecharge )
		{
            uint UserID = GameApp.GameData.UserInfo.UserID;
			Int64 Money = tempRecharge.dwMoney;
			string 	dwNote = tempRecharge.dwRemark;
			string  passWord = MD5Util.GetMD5Hash(tempRecharge.dwPassword);
			string	Key = GameApp.GameData.PrivateKey;
			GameApp.BackendSrv.Exchange(UserID, Money, dwNote, passWord,Key, b =>
			                                {
				if( b != null)
				{
					HallTransfer.Instance.cnAwardResult( b.Code, b.Msg);
				}
			});	
		}
		
		//充值记录
		public void NcRechargeRecord( HallTransfer.RecordRequest rechargeRequest)
		{
			uint UserID = GameApp.GameData.UserInfo.UserID;
			uint Page = rechargeRequest.dwPage;
			uint PageSize = rechargeRequest.dwPageSize;
			DateTime tempTime = rechargeRequest.dwTime;
			string	Key = GameApp.GameData.PrivateKey;
			GameApp.BackendSrv.GetRechargeRecord(UserID, Page, PageSize,tempTime,Key, b =>
			                                         {
				if( b != null && b.Code == 0 )
				{
					List<HallTransfer.RechargeRecord> rechargelist = new List<HallTransfer.RechargeRecord>();

					if(b.Data != null)
					{
						foreach( var temp in b.Data )
						{
							HallTransfer.RechargeRecord tempRecharge = new HallTransfer.RechargeRecord();
							tempRecharge.dwApplyNumber = temp.OrderNo;
							tempRecharge.dwMoney = (Int64)temp.PayAmount;
							tempRecharge.dwState = temp.OrderStatus;
							tempRecharge.dwOrderId = temp.OrderId;
							if(temp.OrderStatus == -1)
							{
								tempRecharge.dwRemark = temp.CancelRemark;
							}
							else
							{
								tempRecharge.dwRemark = temp.SubmitRemark;
							}
							tempRecharge.dwTime = temp.OrderTime;
							tempRecharge.dwAllCount = b.AllCount;
							rechargelist.Add(tempRecharge);
						}
					}
					HallTransfer.Instance.cnRechargeRecond( b.Code, rechargelist );
				}

			});
		}	

		//兑奖记录
		public void NcExchargeRecord( HallTransfer.RecordRequest rechargeRequest)
		{
			uint UserID = GameApp.GameData.UserInfo.UserID;
			uint Page = rechargeRequest.dwPage;
			uint PageSize = rechargeRequest.dwPageSize;
			DateTime tempTime = rechargeRequest.dwTime;
			string	Key = GameApp.GameData.PrivateKey;
			GameApp.BackendSrv.GetExchangeRecord(UserID, Page, PageSize,tempTime,Key, b =>
				                                         {
				if( b != null && b.Code == 0 )
				{
					List<HallTransfer.RechargeRecord> exchargelist = new List<HallTransfer.RechargeRecord>();
					if(b.Data != null)
					{
						foreach( var temp in b.Data )
						{
							HallTransfer.RechargeRecord tempExcharge = new HallTransfer.RechargeRecord();
							tempExcharge.dwApplyNumber = temp.OrderNo;
							tempExcharge.dwMoney = (Int64)temp.PayAmount;
							tempExcharge.dwState = temp.OrderStatus;
							tempExcharge.dwOrderId = temp.OrderId;
							if(temp.OrderStatus == -1)
							{
								tempExcharge.dwRemark = temp.CancelRemark;
							}
							else
							{
								tempExcharge.dwRemark = temp.SubmitRemark;
							}
							tempExcharge.dwTime = temp.OrderTime;
							tempExcharge.dwAllCount = b.AllCount;
							exchargelist.Add(tempExcharge);
						}
					}
					HallTransfer.Instance.cnAwardRecond( b.Code ,exchargelist );
				}	
			});
		}

		//充值订单取消
		public void NcRechangeCancel( HallTransfer.OrderMsg Msg )
		{
			uint UserID = GameApp.GameData.UserInfo.UserID;
			string  CancelReason = Msg.dwCancelReason;
			string  ApplyNumber = Msg.dwApplyNumber;
			string	Key = GameApp.GameData.PrivateKey;
			GameApp.BackendSrv.RechangeCancel( UserID, CancelReason, ApplyNumber,Key, b =>
			                                       {
				if( b!= null )
				{
					HallTransfer.Instance.cnCancelOrderResult( b.Code );
				}
			});
		}

		//兑换订单取消
		public void NcExchangeCancel( HallTransfer.OrderMsg Msg )
		{
			uint UserID = GameApp.GameData.UserInfo.UserID;
			string  CancelReason = Msg.dwCancelReason;
			string  ApplyNumber = Msg.dwApplyNumber;
			string	Key = GameApp.GameData.PrivateKey;
			GameApp.BackendSrv.ExchangeCancel( UserID, CancelReason, ApplyNumber,Key, b =>
			                                       {
				if( b!= null)
				{
					HallTransfer.Instance.cnCancelOrderResult( b.Code );
				}
			});
		}

		public void NcNotice()
		{
			GameApp.BackendSrv.GetNotice( b =>
			                                {
				if( b != null && b.Msg != null )
				{
					HallTransfer.Instance.cnNoticeMsg( b.Code, b.Msg);
				}
			});
		}


        public void GotoOfficeSite()
        {
#if !UNITY_EDITOR
			UnityEngine.Application.OpenURL(GameApp.GameData.OfficeSiteUrl);
#endif
        }

        public void MinimizeWindow()
        {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            Win32Api.GetInstance().ShowMinWindow();
#endif
        }

        public void SwitchFullScreen()
        {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            Win32Api.GetInstance().SwitchMaxWindow();
#endif
        }		
//=================================================================================================================================
		/// <summary>
		/// 接收 充值界面信息
		/// </summary>
		public	void cnRechargeMsg( Int64 lowestMoney )
		{
			Debug.LogWarning ("RechargeMsg~~~~:" + lowestMoney);
			if(!HallTransfer.Instance.uiConfig.MobileEdition)
			{
				recharge.Label_money.text = lowestMoney.ToString();
			}			
		}

		/// <summary>
		/// 提现界面消息返回
		/// </summary>
		public	void cnAwardMsg( HallTransfer.AwardMsg msg)
		{
			Debug.LogWarning ("AwardMsg~~~~:" + msg.dwLowestMoney);
			award.Label_money.text = msg.dwLowestMoney.ToString();
			award.Label_safebox.text = msg.dwSafeMoney.ToString();
			award.Label_order.text = msg.dwAwardMoney.ToString();
		}

		/// <summary>
		/// 接收 充值记录接收
		/// </summary>
		public	void cnRechargeRecond( int code, List<HallTransfer.RechargeRecord> recondlist )
		{
			Debug.LogWarning ("RechargeRecond~~~~~~~~~~~" + recondlist.Count);
			
			if(HallTransfer.Instance.canReceiveRecord_Rc)
			{
				if (HallTransfer.Instance.uiConfig.page_recharge_mask != null) 
				{
					HallTransfer.Instance.uiConfig.page_recharge_mask.SetActive(false);
				}
				HallTransfer.Instance.msgTooLate_RcLogs = false;
				HallTransfer.Instance.rechargeLogonList = recondlist;
				int startIndex = (int)HallTransfer.Instance.uiConfig.curRechargeRecordPage;
				//HallTransfer.Instance.resetRechargeRecord((startIndex-1)*10);	
				resetRechargeRecord((startIndex-1)*10);	
			}
		}
		public	void resetRechargeRecord( int index )
		{
			string tempIndex,tempTime,tempMoney,tempPage,tempRemark,tempState,tempNum,stdt,tempOrderId;
			int recordCount;
			
			tempPage = HallTransfer.Instance.uiConfig.curRechargeRecordPage.ToString();
			
			try
			{
				recordCount = int.Parse(HallTransfer.Instance.rechargeLogonList[0].dwAllCount.ToString());
			}
			catch{
				recordCount = 0;
			}
			
			int pageCount = recordCount / 10;
			if( (recordCount % 10) != 0 ) pageCount += 1;
			HallTransfer.Instance.uiConfig.curRechargeRPageCount = (uint)pageCount;
			rechargeRecords.page_label.text = "第"+tempPage+"页" + " 共"+pageCount+"页";
			rechargeRecords.count_label.text = "共"+recordCount+"条";
			HallTransfer.Instance.uiConfig.curRechargeRPageCount = (uint)pageCount;
			
			for (int i = 0; i < 10; i++) 
			{
				if(!HallTransfer.Instance.uiConfig.MobileEdition)
				{
					rechargeRecords.singleRecordConfigs[i].cancelBtn.SetActive(false);
				}
				
				if (HallTransfer.Instance.rechargeLogonList.Count > i) {
					tempIndex = (i + index + 1).ToString();
					
					tempNum = HallTransfer.Instance.rechargeLogonList [i].dwApplyNumber.ToString();
					tempMoney = HallTransfer.Instance.rechargeLogonList [i].dwMoney.ToString("N0");//HallTransfer.Instance.transMoney(HallTransfer.Instance.rechargeLogonList [i].dwMoney);
					tempRemark = HallTransfer.Instance.rechargeLogonList [i].dwRemark.ToString();
					tempOrderId = HallTransfer.Instance.rechargeLogonList [i].dwOrderId.ToString();
					
					if(tempRemark.Length>8)
					{
						string s = tempRemark.Substring(0,6);
						tempRemark = s + "...";
					}
					int state =  HallTransfer.Instance.rechargeLogonList [i].dwState;
					tempState = "";
					if(state == 0)
					{
						tempState = "待处理";
						
						
					}else if(state == 1)
					{
						tempState = "成功";
					}
					else if(state == -1)
					{
						tempState = "撤销";
					}

					rechargeRecords.singleRecordConfigs[i].cancelBtn.SetActive(true);
					rechargeRecords.singleRecordConfigs[i].cancelBtn.GetComponent<cancelBtnTag>().tags = tempOrderId;

					//if(!(tempState == "待处理"))
					if( state != 0 )
					{
						rechargeRecords.singleRecordConfigs[i].cancelBtn.SetActive(false);
					}
					
				} else {
					tempIndex = tempTime = tempMoney = tempRemark = tempState= tempNum = "";
				}

				rechargeRecords.singleRecordConfigs[i].index_label.text = tempIndex;
				rechargeRecords.singleRecordConfigs[i].applyNum_lable.text = tempNum;
				rechargeRecords.singleRecordConfigs[i].money_label.text = tempMoney;
				rechargeRecords.singleRecordConfigs[i].remark_label.text = tempRemark;
				rechargeRecords.singleRecordConfigs[i].state_label.text = tempState;
			}	
		}

		/// <summary>
		/// 兑奖记录
		/// </summary>		
		//public bool  msgTooLate_AwLogs = false; 	//提现消息接收超时
		//public bool  canReceiveRecord_Aw = true;	//可接收提现记录信息		
		public	void cnAwardRecond(int code, List<HallTransfer.RechargeRecord> recondlist )
		{
			Debug.LogWarning ("AwardRecond~~~~~~~~~~~" + recondlist.Count);
			
			if (HallTransfer.Instance.canReceiveRecord_Aw) 
			{
				if (HallTransfer.Instance.uiConfig.page_recharge_mask != null) 
				{
					HallTransfer.Instance.uiConfig.page_recharge_mask.SetActive(false);
				}
				HallTransfer.Instance.msgTooLate_AwLogs = false;
				HallTransfer.Instance.rechargeLogonList = recondlist;
				int startIndex = (int)HallTransfer.Instance.uiConfig.curRechargeRecordPage;
				//HallTransfer.Instance.resetAwardRecord((startIndex-1)*10);	
				resetAwardRecord((startIndex-1)*10);
			}			
		}

		public	void resetAwardRecord( int index )
		{
			string tempIndex,tempTime,tempMoney,tempPage,tempRemark,tempState,tempNum,stdt,tempOrderId;
			int recordCount;
			
			//GameObject tempLogs = uiConfig.page_recharge.transform.FindChild("front_panel").FindChild("content").FindChild("awardRecords").gameObject;
			tempPage = HallTransfer.Instance.uiConfig.curRechargeRecordPage.ToString();
			
			try
			{
				recordCount = int.Parse(HallTransfer.Instance.rechargeLogonList[0].dwAllCount.ToString());
			}
			catch{
				recordCount = 0;
			}
			
			int pageCount = recordCount / 10;
			if( (recordCount % 10) != 0 ) pageCount += 1;
			HallTransfer.Instance.uiConfig.curRechargeRPageCount = (uint)pageCount;
//			tempLogs.transform.FindChild("page_label").GetComponent<UILabel>().text = "第"+tempPage+"页" + " 共"+pageCount+"页";
//			tempLogs.transform.FindChild("count_label").GetComponent<UILabel>().text = "共"+recordCount+"条";
			awardRecords.page_label.text = "第"+tempPage+"页" + " 共"+pageCount+"页";
			awardRecords.count_label.text = "共"+recordCount+"条";;

			HallTransfer.Instance.uiConfig.curRechargeRPageCount = (uint)pageCount;
			for (int i = 0; i < 10; i++) 
			{
				//Transform tempLog = tempLogs.transform.FindChild ("log" + i);
				
				if(!HallTransfer.Instance.uiConfig.MobileEdition)
				{
					awardRecords.singleRecordConfigs[i].cancelBtn.SetActive(false);
				}
				
				if (HallTransfer.Instance.rechargeLogonList.Count > i) {
					tempIndex = (i + index + 1).ToString();
					
					tempNum = HallTransfer.Instance.rechargeLogonList [i].dwApplyNumber;
					//tempMoney = rechargeLogonList [i].dwMoney.ToString();
					tempMoney = HallTransfer.Instance.rechargeLogonList [i].dwMoney.ToString("N0");//HallTransfer.Instance.transMoney( HallTransfer.Instance.rechargeLogonList [i].dwMoney );
					tempRemark = HallTransfer.Instance.rechargeLogonList [i].dwRemark.ToString();
					tempOrderId = HallTransfer.Instance.rechargeLogonList [i].dwOrderId.ToString();
					
					if(tempRemark.Length>8)
					{
						string s = tempRemark.Substring(0,6);
						tempRemark = s + "...";
					}
					int state =  HallTransfer.Instance.rechargeLogonList [i].dwState;
					tempState = "";
					if(state == 0)
					{
						tempState = "待处理";
						
						
					}else if(state == 1)
					{
						tempState = "成功";
					}
					else if(state == -1)
					{
						tempState = "撤销";
					}	

					awardRecords.singleRecordConfigs[i].cancelBtn.SetActive(true);					
					awardRecords.singleRecordConfigs[i].cancelBtn.GetComponent<cancelBtnTag>().tags = tempOrderId;
					
					if( state != 0 )
					{
						awardRecords.singleRecordConfigs[i].cancelBtn.SetActive(false);	
					}
					
				} else {
					tempIndex = tempTime = tempMoney = tempRemark = tempState = tempNum = "";
				}				
			
				awardRecords.singleRecordConfigs[i].index_label.text = tempIndex;
				awardRecords.singleRecordConfigs[i].applyNum_lable.text = tempNum;
				awardRecords.singleRecordConfigs[i].money_label.text = tempMoney;
				awardRecords.singleRecordConfigs[i].remark_label.text = tempRemark;
				awardRecords.singleRecordConfigs[i].state_label.text = tempState;
			}	
		}

	}

}

