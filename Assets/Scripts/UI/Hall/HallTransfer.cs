using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Text;
using com.QH.QPGame.Services.Data;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Lobby;
using com.QH.QPGame.Lobby.Surfaces;


public class HallTransfer : MonoBehaviour
{
	public	UIConfig			uiConfig;

	#region 事件委托声明
	//登录完成
	public delegate void _ncLoginComplete();
	public _ncLoginComplete ncLoginComplete;
	
	//点击游戏按钮事件(游戏ID)
	public delegate void _ncGameBtnClick( UInt32 gameId );
	public _ncGameBtnClick ncGameBtnClick;
	//点击游戏房间按钮事件(房间ID)
	public delegate void _ncGameRoomClick( UInt32 roomId );
	public _ncGameRoomClick ncGameRoomClick;
	//点击房间座位按钮事件(桌子ID,椅子ID)
	public delegate void _ncGameChairClick( UInt32 roomId, UInt32 deskId, UInt32 chairId );
	public _ncGameChairClick ncGameChairClick;
	//点击快速游戏按钮事件(房间ID)
	public delegate void _ncGameQuickEnterClick( UInt32 roomId );
	public _ncGameQuickEnterClick ncGameQuickEnterClick;
	//点击保险柜事件
	public delegate void _ncSafetyBoxRequest();
	public _ncSafetyBoxRequest ncSafetyBoxRequest;
	//点击保险柜事件(保险柜密码)
	public delegate void _ncSafetyBoxClick( string password );
	public _ncSafetyBoxClick ncSafetyBoxClick;
	//点击保险柜"取钱"事件(取钱数量,保险柜密码)
	public delegate void _ncCheckOutMoney( Int64 money, string password );
	public _ncCheckOutMoney ncCheckOutMoney;
	//点击保险柜"存钱"事件(存钱数量)
	public delegate void _ncCheckInMoney( Int64 money );
	public _ncCheckInMoney ncCheckInMoney;
	//点击保险柜"修改密码"事件(密码类型,旧密码,新密码)
	public delegate void _ncChangePassWD( int passType, string oldPass, string newPass );
	public _ncChangePassWD ncChangePassWD;
	//点击"绑定"按钮事件(密码,旧密码,新密码)
	public delegate void _ncLockOrUnLockAccount( string pass, UInt32 commanType );
	public _ncLockOrUnLockAccount ncLockOrUnLockAccount;
	//点击"资料"按钮事件()
	public delegate void _ncUserInformationRequest( );
	public _ncUserInformationRequest ncUserInformationRequest;
	//点击"修改资料"按钮事件()
	public delegate void _ncChangeUserInformation( UserInfomation userInfo );
	public _ncChangeUserInformation ncChangeUserInformation;
	//点击"修改资料"按钮事件2(修改头像)
	public delegate void _ncChangeUserFace( UserInfomation userInfo );
	public _ncChangeUserFace ncChangeUserFace;
	//点击"官网"按钮事件()
	public delegate void _ncOfficialSite( );
	public _ncOfficialSite ncOfficialSite;
	//窗口"最小化"事件()
	public delegate void _ncMiniWindow( );
	public _ncMiniWindow ncMiniWindow;
	//窗口"最大化"事件()
	public delegate void _ncMaxWindow( );
	public _ncMaxWindow ncMaxWindow;
	//"切换账号"事件()
	public delegate void _ncChangeAccount( );
	public _ncChangeAccount ncChangeAccount;
	//窗口"关闭"事件()
	public delegate void _ncCloseHall( );
	public _ncCloseHall ncCloseHall;
	//点击"复制"按钮事件()
	public delegate void _ncCopyUserIDAndName( );
	public _ncCopyUserIDAndName ncCopyUserIDAndName;
	//点击"安全中心"按钮事件()
	public delegate void _ncSafetyCenter( );
	public _ncSafetyCenter ncSafetyCenter;
	//点击"游戏记录"按钮事件()
	public delegate void _ncGameRecord( GameRecordRequest gameRecord );
	public _ncGameRecord ncGameRecord;
	//点击"意见反馈"事件()
	public delegate void _ncUserSuggestion( UserSuggestion	suggestion );
	public _ncUserSuggestion ncUserSuggestion;
	//玩家登录错误返回事件()
	public delegate void _ncLogonError(  );
	public _ncLogonError ncLogonError;
	//退出捕鱼坐桌界面事件()
	public delegate void _ncQuitRoomDesk(  );
	public _ncQuitRoomDesk ncQuitRoomDesk;
	
	//充值请求事件
	public delegate void _ncRechargeBtnClick( RechargeRequest recharge);
	public _ncRechargeBtnClick ncRechargeBtnClick;
	//充值记录事件
	public delegate void _ncRechargeRecord( RecordRequest record);
	public _ncRechargeRecord ncRechargeRecord;
	//兑奖请求事件
	public delegate void _ncAwardBtnClick( ExchangeRequest recharge);
	//	public delegate void _ncAwardBtnClick( RechargeRequest recharge);
	public _ncAwardBtnClick ncAwardBtnClick;	
	//兑奖记录事件
	public delegate void _ncAwardRecord( RecordRequest record);
	public _ncAwardRecord ncAwardRecord;
	//充值界面事件
	public delegate void _ncRechargeEventClick();
	public _ncRechargeEventClick ncRechargeEventClick;
	//兑奖界面事件
	public delegate void _ncAwardEventClick();
	public _ncAwardEventClick ncAwardEventClick;
	//取消充值订单事件
	public delegate void _ncCancelRechargeClick(OrderMsg msg);
	public _ncCancelRechargeClick ncCancelRechargeClick;
	//取消提现订单事件
	public delegate void _ncCancelAwardClick(OrderMsg msg);
	public _ncCancelAwardClick ncCancelAwardClick;
	#endregion

	#region 结构体声明
	public class GameInfoS
	{
		public UInt32 TypeID;
		public UInt32 ID;
		public UInt32 SortID;
		public	bool		Installed;			//是否安装
		public	bool		NeedUpdate;			//是否需要更新
		public string Name;
		public UInt32 OnlineCnt;
		public bool IsHot;
		public bool IsOpen;
	}
	
	public class RoomInfoS
	{
		public	UInt32		roomId;				//房间ID
//		public	UInt32		gameId;				//房间ID
//		public	bool		Installed;			//是否安装
//		public	bool		NeedUpdate;			//是否需要更新
		public	string		roomName;			//房间名称
		public	UInt32		roomPeopleCnt;		//房间人数
		public	UInt32		roomPeopleUplimit;	//房间人数上限
		public	UInt32		roomDifen;			//房间底分
		public	UInt32		roomRuchang;		//房间入场金币
	}
	public class RoomUserInfo
	{
		public	UInt32		dwUserId;		///用户ID
		public  string		dwNickName;		///用户昵称
		public  UInt32		dwLogoID;		///用户头像
		public	UInt32		dwDesk;			///桌子号
		public	UInt32		dwChair;		///椅子号
		public 	Int64		dwMoney;		///金币
		public  UInt32		dwGameCount;	///游戏局数
	}
	public class MySelfInfo
	{
		public	UInt32		dwUserId;		///用户ID
		public  string		dwNickName;     ///用户昵称
		public  UInt32      dwVip;			///vip等级
		public  Int64		dwMoney;		///用户金币
		public	Int64		dwInsureMoney;  ///银行金币
		public  UInt32      dwLockMathine;	///当前帐号是否锁定了某台机器，1为锁定，0为未锁定
		public	UInt32		dwHeadID;       ///用户头像ID

        public UInt32       dwRoomID;		//非正常退出后,房间ID
		public UInt32       dwDeskNo;		//非正常退出后,桌子ID
		public UInt32       dwDeskStation;	//非正常退出后,桌子状态
	}

	public class RoomDeskInfo
	{
	    public UInt32       dwGameID;           ///游戏ID
		public UInt32		dwRoomId;			///房间ID	
		public UInt32		dwDeskCount;		///桌子数
		public UInt32		dwDeskPeople;		///每桌人数
	}
	
	public class UserInfomation
	{
		public  string		dwName;				///真实名字
		public  string		dwIdentification;	///身份证号码
		public  string		dwNickname;			///用户昵称
		public  string		dwCellPhone;		///手机号
		public  string		dwIM;				///QQ号
		public  string		dwSign;				///个人签名
		public  UInt32		dwLogoID;			///用户头像
	}
	
	public class LogonRecord
	{
		public  UInt64     			dwTmlogonTime;							///登陆时间               
		public  UInt32     			dwLogonIP;								///登陆IP	
	}
	public class GameRecord 
	{
		public  UInt64				dwEndTime;								///游戏结束时间
		public  UInt32     			dwGameKind;								///游戏类型
		public  Int64				dwAmount;								///输赢金额
		public	UInt32				dwAllCount;								///总记录
	}
	public class MoneyRecord 
	{
		public  UInt64     			dwCreateDate;							///交易日期               
		public  UInt32     			dwSender;								///汇款人ID
		public  UInt32				dwReceiver;								///收款人ID
		public	UInt32				dwChangeType;							///变化类型
		public	Int64				dwAmount;								///金额
	}
	///玩家反馈
	public class UserSuggestion
	{              
		public  string         		dwType;							///问题类型
		public  string         		dwUserSuggestion;				///反馈内容
		public  string         		dwCellPhone;					///手机号
	}
	
	///游戏日志c->s
	public class GameRecordRequest 
	{              
		public  UInt32     			dwGameKind;						///游戏类型
		public  UInt32				dwPage;							///页码
		public	UInt32				dwPageSize;						///每页条数
		public	UInt64				dwTime;							///日期
	}
	
	///记录请求
	public class RecordRequest 
	{              
		public  UInt32				dwPage;							///页码
		public	UInt32				dwPageSize;						///每页条数
		public	DateTime		    dwTime;							///日期
	}
	
	///充值请求
	public class RechargeRequest  
	{ 
		public	Int64   dwMoney;				///金额
		public  string   dwRemark;				///备注
	}
	
	public class ExchangeRequest
	{
		public	Int64   dwMoney;				///金额
		public 	string   dwPassword;			///密码
		public  string   dwRemark;				///备注
	}
	
	///提现界面信息
	public class AwardMsg  
	{ 
		public  Int64 	dwSafeMoney;			//保险柜金额
		public  Int64 	dwAwardMoney;			//带提现金额
		public  Int64 	dwLowestMoney;			//最低提现金额
	}
	
	///取消信息
	public class OrderMsg  
	{ 
		public  string 	dwCancelReason;			//取消原因
		public  string 	dwApplyNumber;			//订单号
		
	}
	
	///充值记录
	public class RechargeRecord
	{
		public  string   dwApplyNumber;   		///申请单号
		public  Int64   dwMoney;				///金额
		public  DateTime dwTime;				///充值时间
		public  string   dwRemark;				///备注
		public  Int32    dwState;				///状态
		public  UInt32   dwAllCount;  			///记录总数
		public  long   dwOrderId;
	}
	

        public enum UIModuleID
        {
			Record = 0,
            Notice,
            Recharge,
            Lottery,
			Ranking
            
        }

		
	#endregion
    
	public List<GameObject> gameBtnList = new List<GameObject>();		//游戏按钮容器
	public List<GameObject> gameRoomList = new List<GameObject>();		//游戏房间按钮容器
	public List<GameObject> roomDeskList = new List<GameObject>();		//房间桌子容器
	public List<RoomUserInfo> roomUserList = new List<RoomUserInfo>();	//房间玩家信息容器
	public List<LogonRecord> logonRecordList = new List<LogonRecord>();	//登录游戏信息容器
	public List<GameRecord> gameRecordList = new List<GameRecord>();	//登录游戏信息容器
	public List<uint> gameIdsRecordList = new List<uint>();				//游戏记录ID容器
	public bool[] DeskState;											//桌子"游戏中"状态数组
//	public List<string> ipAddresses = new List<string>();				//IP地址
//	public List<string> ipCity = new List<string>();					//IP地址对应的城市

	private IpCitySearch citySearch;
	public  UserInfomation userInfos = new UserInfomation ();
	public  List<RechargeRecord> rechargeLogonList = new List<RechargeRecord>();	  //充值记录容器

	public static HallTransfer _instance=null;
	
	public static HallTransfer Instance
	{
		get{
				if(_instance == null)  
				{
				    if (UIRoot.list.Count > 0 && UIRoot.list[0].isActiveAndEnabled)
				    {
				        _instance=UIRoot.list[0].gameObject.GetComponentInChildren<HallTransfer>();
				    }
				}

			  return _instance;
			}
	}
	void OnDestroy()
	{
		_instance = null;
	}


	// Use this for initialization
	private UIGrid			GameGrid = null;
	private UIScrollView	GameScrollView = null;
	private UIScrollBar		GameScrollBar = null;
	private	UIScrollView	RoomScrollView = null;
    void Start()
    {
        citySearch = gameObject.AddComponent<IpCitySearch>() as IpCitySearch;

		uiConfig.curRecordGameType = "全部";

		if (ncLoginComplete != null) {
			ncLoginComplete ();
		}

		GameGrid = GameObject.Find("GameGrid").GetComponent<UIGrid>();
//		GameScrollView = GameObject.Find("Game_ScrollView").GetComponent<UIScrollView>();
//		GameScrollBar = GameObject.Find("Game_ScrollBar").GetComponent<UIScrollBar>();
//		RoomScrollView = (uiConfig.page_gameRoom.transform.FindChild("Room_ScrollView")!=null?uiConfig.page_gameRoom.transform.FindChild("Room_ScrollView").GetComponent<UIScrollView>():null);
    }

    // Update is called once per frame
    //void Update()
    //{

	//}

	#region MsgBox 消息框
	public void cnMsgBox(string val)
	{
		//		Debug.LogWarning("cnMsgBox============================== ");
		SurfaceContainer container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
		var msgBox = container.GetSurface<MessageBoxPopup>();
        msgBox.Confirm("",val);

	}

	public void cnTipsBox(string val)
	{
		if (uiConfig.window_TipsBox != null) 
		{
			uiConfig.window_TipsBox.SetActive (true);
			Transform tranObject = uiConfig.window_TipsBox.transform.FindChild("front_panel");
			tranObject.FindChild ("value_label").GetComponent<UILabel> ().text = val;
			StartCoroutine (updateTimeLabel (3));
		} 
		else {
			cnMsgBox(val);
		}
	}

	IEnumerator updateTimeLabel(int sec)
	{
		string tempSec = "";

		while (sec > 0) {
			tempSec = "("+ sec +"秒后关闭)";
			uiConfig.window_TipsBox.transform.FindChild("front_panel").FindChild("time_label").gameObject.GetComponent<UILabel>().text = tempSec;
			yield return new WaitForSeconds (1f);
			sec--;
		}
		uiConfig.window_TipsBox.SetActive(false);
	}

	#endregion

	#region 接收游戏ID
	public void cnSetGameIDs( List<GameInfoS> gameIDs)
	{
		Debug.LogWarning("-------------------------------------cnSetGameIDs------------   ::  " + gameIDs.Count);
		gameIdsRecordList.Clear();
		uiConfig.hallGameIds = new UInt32[gameIDs.Count];
		uiConfig.hallGameNames = new string[gameIDs.Count];
		uiConfig.hallGameTypeId = new uint[gameIDs.Count];

		MousePointerCtr tempMouse = GetComponent<MousePointerCtr>();
		if(tempMouse != null) tempMouse.Lobby_SetMouse_exit();//还原鼠标

		for(int i = 0; i < gameIDs.Count; i++)
		{
			gameIdsRecordList.Add( gameIDs[i].ID );
			uiConfig.hallGameNames[i] = gameIDs[i].Name ;
			if(gameIDs[i].IsOpen)
			{
				uiConfig.hallGameIds[i] = gameIDs[i].ID ;
				uiConfig.hallGameTypeId[i] = gameIDs[i].TypeID;
			}
		}
		setGameRecordNameList();
		if(!uiConfig.MobileEdition)
		{
			uiConfig.page_roomDesk.SetActive(false);
			uiConfig.page_recharge.SetActive(false);
			if(uiConfig.page_gameRule != null) uiConfig.page_gameRule.SetActive(false);
			if(uiConfig.btn_scoreList != null) uiConfig.btn_scoreList.SetActive(false);
			uiConfig.btn_gameHall.SetActive(true);
		}

		uiConfig.window_FirstPage.SetActive(true);
		for(var i = 0; i < gameBtnList.Count; i++)
		{
			Destroy(gameBtnList[i]);//销毁游戏按钮
		}
		gameBtnList.Clear();//清空游戏按钮容器
		int gameIndex = 0;
		int tempIndex = 0;
		for(int i = 0; i < gameIDs.Count; i++)
		{
			//创建游戏按钮
			gameBtnList.Add( Instantiate(uiConfig.btn_gameBtn,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject );
			if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
			{
				//赌场风格_移动
				gameBtnList[gameIndex].transform.parent = GameObject.Find("GameGrid").transform;
				string	tempSpriteName = gameBtnList[gameIndex].transform.FindChild("button_img").GetComponent<UISprite>().spriteName;
				tempSpriteName = tempSpriteName.Substring(0,tempSpriteName.Length-1) + tempIndex.ToString();
				tempIndex = (tempIndex==2?0:tempIndex+1);
				gameBtnList[gameIndex].transform.FindChild("button_img").GetComponent<UISprite>().spriteName = tempSpriteName;
			}else{
				//正常版本
				gameBtnList[gameIndex].transform.parent = GameGrid.transform;
				GameGrid.cellHeight = gameBtnList[gameIndex].GetComponent<UIWidget>().height + uiConfig.gameBtn_interval;
				int gameCellHeight = (int)GameGrid.cellHeight;
				gameBtnList[gameIndex].transform.localPosition = new Vector3(0,(i * -(gameCellHeight)),0);
			}

			gameBtnList[gameIndex].transform.localScale = new Vector3(1f,1f,1f);


			if(uiConfig.MobileEdition)
			{
				//移动版
				gameBtnList[gameIndex].transform.FindChild("button_img").GetComponent<gameBtnClick>().gameid = gameIDs[i].ID;//初始化gameId
				gameBtnList[gameIndex].transform.FindChild("button_img").GetComponent<gameBtnClick>().gameKind = gameIDs[i].TypeID;
				gameBtnList[gameIndex].transform.FindChild("button_img").GetComponent<gameBtnClick>().installed = gameIDs[i].Installed;
				gameBtnList[gameIndex].transform.FindChild("button_img").GetComponent<gameBtnClick>().needUpdate = gameIDs[i].NeedUpdate;
			}else{
				//PC版
				gameBtnList[gameIndex].GetComponent<gameBtnClick>().gameid = gameIDs[i].ID;//初始化gameId
				gameBtnList[gameIndex].GetComponent<gameBtnClick>().gameKind = gameIDs[i].TypeID;
				gameBtnList[gameIndex].GetComponent<gameBtnClick>().installed = gameIDs[i].Installed;
				gameBtnList[gameIndex].GetComponent<gameBtnClick>().needUpdate = gameIDs[i].NeedUpdate;
			}

			Transform gameName = gameBtnList[gameIndex].transform.FindChild("gameName_label");
			gameName.gameObject.GetComponent<UILabel>().text = gameIDs[i].Name;
			Transform gameName2 = gameBtnList[gameIndex].transform.FindChild("toggled_img").FindChild("gameName_label");
			if(gameName2 != null) gameName2.gameObject.GetComponent<UILabel>().text =  gameIDs[i].Name;//初始化gameName
			Transform gameIco = gameBtnList[gameIndex].transform.FindChild("game_ico");
			gameIco.gameObject.GetComponent<UISprite>().spriteName = "gameico_" + gameIDs[i].ID ;//初始化游戏图标
			bool	hotEnable = false;
			if( gameIndex < 2 ) hotEnable = true;
			Transform hotIco = gameBtnList[gameIndex].transform.FindChild("hot_ico");
			hotIco.gameObject.SetActive(hotEnable);//设置火爆标签
			gameIndex++;
		}
		Invoke("resetGamesScrollView",0.1f);
    }


	public void resetGamesScrollView()
	{
		//Debug.LogWarning("resetGamesScrollView!");
		int gamesHeight = 0;
		int gameBgHeight = 0;
		
		if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
		{
			//赌博风格
			GameObject.Find("GameGrid").GetComponent<UIWrapContent>().SortBasedOnScrollMovement();
			for(int i = 0; i< gameBtnList.Count; i++)
			{
				gameBtnList[i].transform.localPosition = new Vector3((float)((i-1)*GameObject.Find("GameGrid").GetComponent<UIWrapContent>().itemSize),0,0);
			}
			ScrollViewCtrl tempViewCtrl = gameBtnList[0].transform.parent.parent.parent.GetComponent<ScrollViewCtrl>();
			if(tempViewCtrl != null) tempViewCtrl.ScrollViewInit();
		}else if(!uiConfig.MobileEdition)
		{
			//PC端
			
			gamesHeight = gameBtnList.Count * (int)GameGrid.cellHeight;
			gameBgHeight = GameObject.Find("GAMES").GetComponent<UIWidget>().height - GameObject.Find("BG_gamelist_bg").GetComponent<UISprite>().height;
			int tempBottomAnchor = gameBgHeight - gamesHeight;
			if(tempBottomAnchor < 1) tempBottomAnchor = 1;

// 			if((float)gamesHeight < GameScrollView.GetComponent<UIPanel>().GetViewSize().y)
// 			{
// 				GameScrollBar.value = 0;
// 				GameScrollBar.enabled = GameScrollView.enabled = false;
// 			}else{
// 				GameScrollBar.enabled = GameScrollView.enabled = true;
// 				GameScrollView.GetComponent<UIPanel>().bottomAnchor.absolute = tempBottomAnchor;
// 			}
		}else
		{
			//移动端
			GameGrid.transform.localPosition = Vector2.zero;
			gamesHeight = gameBtnList.Count * (int)GameGrid.cellHeight;
			gameBgHeight = GameObject.Find("GAMES").GetComponent<UIWidget>().height;
		}
		if(!GameScrollBar.enabled) return;
		if(uiConfig.curGameId != 0)
		{
			for(var i = 0; i < uiConfig.hallGameIds.Length; i++)
			{
				if(uiConfig.curGameId == uiConfig.hallGameIds[i])
				{
					if(uiConfig.MobileEdition)
					{
						gameBtnList[i].transform.FindChild("button_img").GetComponent<UIToggle>().value = true;
					}else{
						gameBtnList[i].GetComponent<UIToggle>().value = true;
					}
					GameObject.Find("Game_ScrollBar").GetComponent<UIScrollBar>().value = ((float)i)/((float)uiConfig.hallGameIds.Length);
					break;
				}
			}
		}
	}

	public void setGameRecordNameList()
	{
		//初始化游戏类型下拉菜单
		if(!uiConfig.MobileEdition)
		{
			string tempGameName = "";
			Transform gamePopupList =  uiConfig.window_Security.transform.FindChild("front_panel").FindChild("content").FindChild("gamePopup_list");
			gamePopupList.GetComponent<UIPopupList>().Clear();
			gamePopupList.GetComponent<UIPopupList>().AddItem("全部");
			for(int i = 0; i < gameIdsRecordList.Count; i++)
			{
				tempGameName = "";
				for(int j = 0; j < uiConfig.hallGameIds.Length; j++)
				{
					if(gameIdsRecordList[i] == uiConfig.hallGameIds[j])
					{
						tempGameName = uiConfig.hallGameNames[j];
						break;
					}
				}
				if(tempGameName != "") gamePopupList.GetComponent<UIPopupList>().AddItem( tempGameName );
			}
		}
	}
	#endregion

	#region 接收玩家自己信息
	private UIAtlas userFace_atlas;
	public void cnSetUserInfo(MySelfInfo mySelfInfo)
	{
		Debug.LogWarning("cnSetUserInfo============================== " + mySelfInfo.dwMoney );
		SurfaceUserInfo.Instance.SetUserInfo(mySelfInfo);

//		GameObject nickName = GameObject.Find("userNickName_label");
//		nickName.GetComponent<UILabel>().text = mySelfInfo.dwNickName;
//		if(!uiConfig.MobileEdition)
//		{
//			GameObject vipLevel = GameObject.Find("userVipLevel_label");
//			string tempVip = mySelfInfo.dwVip.ToString();
//			if(mySelfInfo.dwVip == 0)
//			{
//				tempVip = "非会员";
//			}else{
//				tempVip = "会员";
//			}
//			vipLevel.GetComponent<UILabel>().text = tempVip;
//		}
//		GameObject gameId = GameObject.Find("userGameId_label");
//		gameId.GetComponent<UILabel>().text = mySelfInfo.dwUserId.ToString();
//		GameObject money = GameObject.Find("userMoney_label");
//		money.GetComponent<UILabel>().text = transMoney( mySelfInfo.dwMoney );
//		if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
//			uiConfig.page_gameRoom.transform.FindChild("UserMoney").FindChild("UserMoney_label").GetComponent<UILabel>().text = transMoney( mySelfInfo.dwMoney );
//		GameObject face = GameObject.Find("userFace_img");
//		userFace_atlas = face.GetComponent<UISprite>().atlas;
//		face.GetComponent<UISprite>().spriteName = "face_" + mySelfInfo.dwHeadID;
//		bool tempLock = false;
//		if(mySelfInfo.dwLockMathine == 1) tempLock = true;
//		uiConfig.LockOrUnLockAccount = tempLock;
//		if(true)//(!uiConfig.MobileEdition)
//		{
//			for(var j = 0; j < uiConfig.hallGameRoomIds.Length; j++)
//			{
//				if(mySelfInfo.dwRoomID == uiConfig.hallGameRoomIds[j])
//				{
//					uiConfig.curRoomName = uiConfig.hallGameRoomNames[j];
//					break;
//				}
//			}
//			uiConfig.curRoomId = mySelfInfo.dwRoomID;
//			uiConfig.curDeskNo = mySelfInfo.dwDeskNo;
//			uiConfig.curStation = mySelfInfo.dwDeskStation;
//		}
	}
	public int MainBtnGridChildCount;
    public void cnSetModulesInfo(List<ModuleInfo> info, bool fromLogin)
	{
		if(uiConfig.MainBtn_Grid!=null)
		{
			MainBtnGridChildCount = uiConfig.MainBtn_Grid.GetComponentsInChildren<UIButton>().Length-1;
		}
		if(uiConfig.MainBtn_Record!=null) uiConfig.MainBtn_Record.SetActive(false);
		if(uiConfig.MainBtn_Notice!=null) uiConfig.MainBtn_Notice.SetActive(false);
		if(uiConfig.MainBtn_Recharge!=null) uiConfig.MainBtn_Recharge.SetActive(false);
		if(uiConfig.MainBtn_LuckDraw!=null) uiConfig.MainBtn_LuckDraw.SetActive(false);	
		if(uiConfig.MainBtn_Ranking!=null) uiConfig.MainBtn_Ranking.SetActive(false);	
        foreach (var uiModuleInfo in info)
        {
            if (uiModuleInfo.S != 0)
            {
                switch ((UIModuleID)uiModuleInfo.ID)
                {
                    case UIModuleID.Notice:
                        {
                            //公告
							bool msgBoxShown = GameApp.PopupMgr.IsMsgBoxShown();
							if(uiConfig.MainBtn_Notice!=null) uiConfig.MainBtn_Notice.SetActive(true);
							if (fromLogin && !msgBoxShown)
                            {
								if(uiConfig.WebNotice!=null)
								{
									uiConfig.WebNotice.SetActive(false);
									uiConfig.WebNotice.SetActive(true);
								}
                                //Win32Api.Instance.OpenUrlInBrowser("公告",GameApp.GameData.BackStorgeUrl+"/ReferencePage/Notice.aspx?Platform="+Application.platform, 600, 400);
//                          	Win32Api.Instance.OpenUrlInBrowser("公告", "http://172.16.10.252:8090", 600, 400);
                            }
                            break;
                        }
                    case UIModuleID.Lottery:
                        {
							//抽奖
							if(uiConfig.MainBtn_LuckDraw!=null) uiConfig.MainBtn_LuckDraw.SetActive(true);
                        	break;
                        }
                    case UIModuleID.Record:
                        {
							//记录
							if(uiConfig.MainBtn_Record!=null) uiConfig.MainBtn_Record.SetActive(true);
                            break;
                        }
					case UIModuleID.Recharge:
						{
							//充值
							if(uiConfig.MainBtn_Recharge!=null) uiConfig.MainBtn_Recharge.SetActive(true);
							break;
						}
					case UIModuleID.Ranking:
						{
							//排行榜
							if(uiConfig.MainBtn_Ranking!=null) uiConfig.MainBtn_Ranking.SetActive(true);
							break;
						}
				}
			}
		}
		if(uiConfig.MainBtn_Grid!=null)
		{
			if(uiConfig.MainBtn_Record!=null && !uiConfig.MainBtn_Record.activeSelf)
				DestroyImmediate(uiConfig.MainBtn_Record);
			if(uiConfig.MainBtn_Notice!=null && !uiConfig.MainBtn_Notice.activeSelf)
				DestroyImmediate(uiConfig.MainBtn_Notice);
			if(uiConfig.MainBtn_Recharge!=null && !uiConfig.MainBtn_Recharge.activeSelf)
				DestroyImmediate(uiConfig.MainBtn_Recharge);
			if(uiConfig.MainBtn_LuckDraw!=null && !uiConfig.MainBtn_LuckDraw.activeSelf)
				DestroyImmediate(uiConfig.MainBtn_LuckDraw);
			if(uiConfig.MainBtn_Ranking!=null && !uiConfig.MainBtn_Ranking.activeSelf)
				DestroyImmediate(uiConfig.MainBtn_Ranking);
			Invoke("resetMainBtnGrid",0f);
		}
    }
	void resetMainBtnGrid()
	{
		if(uiConfig.MainBtn_Grid.GetComponent<UILabel>()!=null)
		{
			if(uiConfig.MainBtn_Grid.GetComponent<UILabel>().text == "center")
			{
				//均匀排列
				int gridChildMinCount = uiConfig.MainBtn_Grid.GetComponentsInChildren<UIButton>().Length-1;
				Debug.LogWarning("MainBtnGridChildCount:"+MainBtnGridChildCount+"    gridChildMinCount:"+gridChildMinCount);
				uiConfig.MainBtn_Grid.cellWidth*=((float)MainBtnGridChildCount)/((float)gridChildMinCount);
				uiConfig.MainBtn_Grid.Reposition();
			}
		}else{
			//居右排列
			UIButton[] tempButtons = uiConfig.MainBtn_Grid.GetComponentsInChildren<UIButton>();
			int cellWidth = (int)uiConfig.MainBtn_Grid.cellWidth;
			int tempIndex = 0;
			for(int i = tempButtons.Length-1; i >= 0; i--)
			{
				if(tempButtons[i].GetComponent<TweenPosition>()!=null)
				{
					tempButtons[i].GetComponent<TweenPosition>().from = new Vector3((float)(MainBtnGridChildCount+3)*cellWidth,0,0);
					tempButtons[i].GetComponent<TweenPosition>().to = new Vector3((float)(MainBtnGridChildCount-tempIndex++)*cellWidth,0,0);
					tempButtons[i].GetComponent<TweenPosition>().duration = (float)tempIndex*0.1f;
					tempButtons[i].GetComponent<TweenPosition>().Play();
				}else{
					tempButtons[i].transform.localPosition = new Vector2((float)(MainBtnGridChildCount-tempIndex++)*cellWidth,0);
				}
			}
		}
	}
    public void cnCloseWebpage()
    {
		if (uiConfig.WebLuckDraw != null) uiConfig.WebLuckDraw.SetActive (false);
		if (uiConfig.WebNotice != null) uiConfig.WebNotice.SetActive (false);
		if (uiConfig.WebRanking != null) uiConfig.WebRanking.SetActive (false);
		if (uiConfig.WebRecharge != null) uiConfig.WebRecharge.SetActive (false);
		if (uiConfig.WebShare != null) uiConfig.WebShare.SetActive (false);
    }

    public string transMoney( Int64 money )
	{
		string tempMoney, str;
		tempMoney = str = money.ToString ();

		for (int i = 0; i<(str.Length-1)/3; i++)
		{
			tempMoney = tempMoney.Insert((tempMoney.Length-3*(i+1)-i),",");
		}

		return tempMoney;
	}

	#endregion

	#region 接收房间信息
	private	string resetNum(string num)
	{
		int zeroCount = 0;
		foreach(char i in num)
		{
			if(i == '0')
			{
				zeroCount++;
			}else{
				zeroCount = 0;
			}
		}
		if(zeroCount>=4)
		{
			return num.Substring(0,num.Length-4)+"w";
		}else if(zeroCount==3)
		{
			return num.Substring(0,num.Length-3)+"q";
		}else
		{
			return num;
		}
	}
	public void cnSetGameRoomInfo( uint gameId, List<RoomInfoS> gameRoomInfoList )
	{
		Debug.LogWarning("cnSetGameRoomInfo============================== " + gameRoomInfoList.Count);
		//设置当前游戏ID
		uiConfig.curGameId = gameId;
		for(var i = 0; i < uiConfig.hallGameIds.Length; i++)
		{
			if(gameId == uiConfig.hallGameIds[i])
			{
				uiConfig.curGameName = uiConfig.hallGameNames[i];
				break;
			}
		}
		Debug.LogWarning("uiConfig.curGameId:  " + uiConfig.curGameId);
		SetCurGameType();//设置游戏种类
		if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
		{
			for(var i = 0; i < gameRoomList.Count; i++) 
			{
				Debug.LogWarning("mGambleStyle____Destroy:   " + gameRoomList[i].name);
				DestroyImmediate(gameRoomList[i].gameObject);	//销毁房间按钮
			}
		}else{
			for(var i = 1; i < gameRoomList.Count; i++) 
			{
				Debug.LogWarning("Destroy:   " + gameRoomList[i].name);
				Destroy(gameRoomList[i]);	//销毁房间按钮
			}
		}
		gameRoomList.Clear();//清空房间按钮容器



		if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
		{
			//赌博风格
//			GameObject.Find("Room_Logo").GetComponent<UISprite>().spriteName = "gamelabel_" + gameId.ToString();
			GameObject.Find("Room_Logo").GetComponent<UILabel>().text = uiConfig.curGameName;
			uiConfig.AnimCtrl.EnterGameRoom();						//播放进入房间动画
			for(int i= 0; i < (int)UIConfig.CurGameType.max; i++)
			{
				//隐藏其他游戏类型
				Transform tempGameType = uiConfig.page_gameRoom.transform.FindChild(((UIConfig.CurGameType)i).ToString());
				if(tempGameType == null) continue;
				tempGameType.GetComponent<TweenAlpha>().PlayReverse();
				Transform[] tempTrans = tempGameType.FindChild("Room_ScrollView").FindChild("RoomGrid").GetComponentsInChildren<Transform>();
				for(int j = 1; j < tempTrans.Length; j++) Destroy(tempTrans[j]);
				tempGameType.gameObject.SetActive(false);
			}
			Transform tempCurGameType = uiConfig.page_gameRoom.transform.FindChild(uiConfig.curGameType.ToString());
			if(tempCurGameType != null)
			{
				//显示当前游戏类型
				tempCurGameType.gameObject.SetActive(true);
				tempCurGameType.GetComponent<TweenAlpha>().PlayForward();
			}
		}else{
			//隐藏座位页面
			if(uiConfig.page_roomDesk != null) uiConfig.page_roomDesk.SetActive(false);
			//隐藏金券游戏页面
			if(uiConfig.page_scoreList != null) uiConfig.page_scoreList.SetActive(false);
			//隐藏充值页面
			if(uiConfig.page_recharge != null) uiConfig.page_recharge.SetActive(false);
			//隐藏游戏规则页面
			if(uiConfig.page_gameRule != null) uiConfig.page_gameRule.SetActive(false);
			//隐藏FirstPage
			if(uiConfig.window_FirstPage !=null) uiConfig.window_FirstPage.SetActive(false);
			//显示游戏房间页面
			if(uiConfig.page_gameRoom != null) uiConfig.page_gameRoom.SetActive(true);
			if(!uiConfig.MobileEdition)
			{
				uiConfig.btn_gameHall.transform.FindChild("toggled_img").gameObject.SetActive(true);
				if(uiConfig.btn_scoreList != null) uiConfig.btn_scoreList.transform.FindChild("toggled_img").gameObject.SetActive(false);
			}
		}


		if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
		{
			foreach(GameObject tempRoomDesk in uiConfig.gambleRoomDesk) tempRoomDesk.GetComponent<TweenPosition>().PlayReverse();//.SetActive(false);
			uiConfig.page_gameRoom.transform.FindChild("Desks").GetComponent<TweenPosition>().PlayReverse();//.gameObject.SetActive(false);
			//赌博风格
			int tempSpriteNameIndex = 0;
			Transform tempGrid = uiConfig.page_gameRoom.transform.FindChild(uiConfig.curGameType.ToString()).FindChild("Room_ScrollView").FindChild("RoomGrid");
			tempGrid.GetComponentInParent<UIScrollView>().ResetPosition();
//			tempGrid.GetComponent<UIWrapContent>().maxIndex = gameRoomInfoList.Count-1;
			int tempItemWidth = tempGrid.GetComponent<UIWrapContent>().itemSize;
//			tempGrid.GetComponent<UIWrapContent>().enabled = false;
			for(var i = 0; i < gameRoomInfoList.Count; i++)
			{
				gameRoomList.Add ( Instantiate(uiConfig.btn_gameRoom.transform.FindChild(uiConfig.curGameType.ToString()).gameObject,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject );
				gameRoomList[i].SetActive(true);
				if(gameRoomList[i].GetComponent<UISprite>()!=null)
				{
					string tempStr = gameRoomList[i].GetComponent<UISprite>().spriteName;
					gameRoomList[i].GetComponent<UISprite>().spriteName = tempStr.Substring(0,tempStr.Length-1) + tempSpriteNameIndex.ToString();
				}
				tempSpriteNameIndex = (tempSpriteNameIndex==2?0:tempSpriteNameIndex+1);
				gameRoomList[i].name = "GameRoom" + i.ToString();
				if(tempGrid.GetComponent<UIUpdateChild>()!=null)
				{
					tempGrid.GetComponent<UIUpdateChild>().Init();
					tempGrid.GetComponent<UIWrapContent>().enabled = false;
				}
				gameRoomList[i].transform.parent = tempGrid;
				gameRoomList[i].transform.localScale = new Vector3(1f,1f,1f);
				gameRoomList[i].transform.localPosition = new Vector2((float)i*tempItemWidth,0f);
				string tempRoomDiFen = gameRoomInfoList[i].roomDifen.ToString();
				switch(uiConfig.curGameId)
				{
				case 104:
					tempRoomDiFen = resetNum(((int.Parse(tempRoomDiFen))/10).ToString()) + "/" + resetNum(tempRoomDiFen);
					break;
				case 400:
					tempRoomDiFen = resetNum(((int.Parse(tempRoomDiFen))*10).ToString());
					break;
				case 402:
					tempRoomDiFen = resetNum(((int.Parse(tempRoomDiFen))*10).ToString());
					break;
				case 403:
					tempRoomDiFen = resetNum(((int.Parse(tempRoomDiFen))*10).ToString());
					break;
				case 404:
					tempRoomDiFen = resetNum(((int.Parse(tempRoomDiFen))*10).ToString());
					break;
				case 405:
					tempRoomDiFen = resetNum(((int.Parse(tempRoomDiFen))*10).ToString());
					break;
				case 406:
					tempRoomDiFen = resetNum(((int.Parse(tempRoomDiFen))*10).ToString());
					break;
				default:
					break;
				}
				gameRoomList[i].GetComponent<gameRoomClick>().SetRoomInfo(gameRoomInfoList[i].roomId,gameRoomInfoList[i].roomName,
				                                                          resetNum(gameRoomInfoList[i].roomPeopleCnt.ToString()),resetNum(uiConfig.curGameRateName),
				                                                          resetNum(gameRoomInfoList[i].roomRuchang.ToString()));
			}
			Invoke("resetRoomScrollView",0.1f);
			return;
		}
		if(gameRoomInfoList.Count == 0) uiConfig.btn_gameRoom.SetActive(false);
		for(var i = 0; i < gameRoomInfoList.Count; i++)
		{
			//创建房间按钮
			if(i == 0)
			{
				uiConfig.btn_gameRoom.SetActive(true);
				gameRoomList.Add(uiConfig.btn_gameRoom);
			}else{
				gameRoomList.Add ( Instantiate(uiConfig.btn_gameRoom,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject );
				gameRoomList[i].transform.parent = RoomScrollView.transform.FindChild("RoomGrid");
				gameRoomList[i].transform.localScale = new Vector3(1f,1f,1f);
			}
			gameRoomList[i].SetActive(false);
			if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.NormalBlueStyle && i % 2 == 1)
			{
				gameRoomList[i].GetComponent<UISprite>().spriteName = "hall_center_roombar1";//每隔一个改变颜色
			}

			if(gameRoomInfoList[i].roomId == uiConfig.lastGameRoomId)
			{
				gameRoomList[i].GetComponent<UISprite>().spriteName = "hall_gameroom_highlight";//高亮显示上次进入的游戏
			}

			if(uiConfig.MobileEdition)
			{
				//移动版
				gameRoomList[i].GetComponent<gameRoomClick>().RoomId = gameRoomInfoList[i].roomId;//初始化roomid
			}else{
				//PC版
				gameRoomList[i].GetComponent<gameRoomClick>().RoomId = gameRoomInfoList[i].roomId;//初始化roomid
				Transform tempQuickEnter = gameRoomList[i].transform.FindChild("quickEnter_btn");
				tempQuickEnter.gameObject.GetComponent<gameQuickEnterClick>().roomid = gameRoomInfoList[i].roomId;//初始化快速进入roomid
				tempQuickEnter.gameObject.SetActive(true);
			}
			Transform nameLabel = gameRoomList[i].transform.FindChild("name_label");
			nameLabel.gameObject.GetComponent<UILabel>().text = gameRoomInfoList[i].roomName;//初始化房间名称
			Transform numLabel = gameRoomList[i].transform.FindChild("num_label");
			numLabel.gameObject.GetComponent<UILabel>().text = gameRoomInfoList[i].roomPeopleCnt.ToString();//初始化房间人数
			Color roomColor,outColor;
			string roomState;
			Transform peopleStateLabel = gameRoomList[i].transform.FindChild("peopleState_label");
			Transform peopleState0Img = gameRoomList[i].transform.FindChild("peopleState0_img");
			Transform peopleState1Img = gameRoomList[i].transform.FindChild("peopleState1_img");
			Transform peopleState2Img = gameRoomList[i].transform.FindChild("peopleState2_img");
			if( (double)gameRoomInfoList[i].roomPeopleCnt / (double)gameRoomInfoList[i].roomPeopleUplimit < 0.3f )
			{
				roomColor = Color.green;
				outColor = Color.black;
				roomState = "(流畅)";
				peopleState0Img.gameObject.SetActive(true);
				peopleState1Img.gameObject.SetActive(false);
				peopleState2Img.gameObject.SetActive(false);
			}else if( (double)gameRoomInfoList[i].roomPeopleCnt / (double)gameRoomInfoList[i].roomPeopleUplimit < 0.6f )
			{
				roomColor = Color.yellow;
				outColor = Color.black;
				roomState = "(良好)";
				peopleState0Img.gameObject.SetActive(false);
				peopleState1Img.gameObject.SetActive(true);
				peopleState2Img.gameObject.SetActive(false);
			}else
			{
				roomColor = Color.red;
				outColor = Color.white;
				roomState = "(爆满)";
				peopleState0Img.gameObject.SetActive(false);
				peopleState1Img.gameObject.SetActive(false);
				peopleState2Img.gameObject.SetActive(true);
			}
			peopleStateLabel.gameObject.GetComponent<UILabel>().color = roomColor;
			peopleStateLabel.gameObject.GetComponent<UILabel>().effectColor = outColor;
			peopleStateLabel.gameObject.GetComponent<UILabel>().text = roomState;//初始化房间人数状态
			Transform difenLabel = gameRoomList[i].transform.FindChild("difen_label");
			difenLabel.gameObject.GetComponent<UILabel>().text = gameRoomInfoList[i].roomDifen.ToString();//初始化房间底分
			
			if( uiConfig.curGameId != 0 && uiConfig.curGameId > 100 )
			{
				//string strId = uiConfig.curGameId.ToString();
				if(uiConfig.curGameKind == 3)
				{
					difenLabel.gameObject.GetComponent<UILabel>().text = ( gameRoomInfoList[i].roomDifen * 0.001f).ToString();
				}
			}


			Transform ruchangLabel = gameRoomList[i].transform.FindChild("ruchang_label");
			//ruchangLabel.gameObject.GetComponent<UILabel>().text = gameRoomInfoList[i].roomRuchang.ToString();//初始化房间入场费
			ruchangLabel.gameObject.GetComponent<UILabel>().text = gameRoomInfoList[i].roomRuchang.ToString("N0");//transMoney(gameRoomInfoList[i].roomRuchang);
		}
		Invoke("resetRoomScrollView",0.1f);
	//}else{
		//从游戏返回大厅时,设置
		if(!uiConfig.MobileEdition)
		{
			//PC端
			for(var i = 0; i < uiConfig.hallGameIds.Length; i++)
			{
				if(gameId == uiConfig.hallGameIds[i])
				{
					uiConfig.curGameName = uiConfig.hallGameNames[i];
					uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("gameName_btn").FindChild("Label").GetComponent<UILabel>().text = uiConfig.curGameName;
					uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("gameName_btn").GetComponent<gameBtnClick>().enabled = true;
					uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("gameName_btn").GetComponent<gameBtnClick>().gameid = gameId;
					uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("gameName_btn").GetComponent<gameBtnClick>().gameKind = uiConfig.hallGameTypeId[i];
					break;
				}
			}
			uiConfig.hallGameRoomIds = new uint[gameRoomInfoList.Count];
			uiConfig.hallGameRoomNames = new string[gameRoomInfoList.Count];
			for(var i = 0; i < gameRoomInfoList.Count; i++)
			{
				uiConfig.hallGameRoomIds[i] = gameRoomInfoList[i].roomId;
				uiConfig.hallGameRoomNames[i] = gameRoomInfoList[i].roomName;
				if(gameRoomInfoList[i].roomId == uiConfig.curRoomId)
				{
					uiConfig.curRoomName = gameRoomInfoList[i].roomName;
					uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("roomName_label").GetComponent<UILabel>().text = "/" + uiConfig.curRoomName;
				}
			}
			//移动到最后一次进入的桌子处
			Invoke("FindLastDesk",0.1f);
		}else{
			//手机端
			for(var i = 0; i < uiConfig.hallGameIds.Length; i++)
			{
				if(gameId == uiConfig.hallGameIds[i])
				{
					uiConfig.curGameName = uiConfig.hallGameNames[i];
//						uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("title_label").GetComponent<UILabel>().text = uiConfig.curGameName;
					uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("close_btn").GetComponent<gameBtnClick>().enabled = true;
					uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("close_btn").GetComponent<gameBtnClick>().gameid = gameId;
					uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("close_btn").GetComponent<gameBtnClick>().gameKind = uiConfig.hallGameTypeId[i];
					uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("return_btn").GetComponent<gameBtnClick>().enabled = true;
					uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("return_btn").GetComponent<gameBtnClick>().gameid = gameId;
					uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("return_btn").GetComponent<gameBtnClick>().gameKind = uiConfig.hallGameTypeId[i];
					break;
				}
			}
			uiConfig.hallGameRoomIds = new uint[gameRoomInfoList.Count];
			uiConfig.hallGameRoomNames = new string[gameRoomInfoList.Count];
			for(var i = 0; i < gameRoomInfoList.Count; i++)
			{
				uiConfig.hallGameRoomIds[i] = gameRoomInfoList[i].roomId;
				uiConfig.hallGameRoomNames[i] = gameRoomInfoList[i].roomName;
				if(gameRoomInfoList[i].roomId == uiConfig.curRoomId)
				{
					uiConfig.curRoomName = gameRoomInfoList[i].roomName;
					uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("title_label").GetComponent<UILabel>().text = "/" + uiConfig.curRoomName;
				}
			}
			//移动到最后一次进入的桌子处
			Invoke("FindLastDesk",0.1f);
		}
	}
	public void cnOnlineCount(uint gameId, List<RoomInfoS> gameRoomInfoList)
	{
		if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
		{
			return;
		}
		for(var i = 0; i < gameRoomList.Count; i++)
		{
			for(var j = 0; j < gameRoomInfoList.Count; j++)
			{
				if(gameRoomInfoList[j].roomId == gameRoomList[i].GetComponent<gameRoomClick>().RoomId)
				{
					gameRoomList[i].transform.FindChild("num_label").GetComponent<UILabel>().text = gameRoomInfoList[j].roomPeopleCnt.ToString();
				}
			}
		}
	}
	private moblieRoomDeskBtnClick[] MoblieRoomBtns;
	private void SetCurGameType()
	{
		switch(uiConfig.curGameId)
		{
		case 100:
			//斗地主
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底分";
			break;
		case 101:
			//斗地主
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底分";
			break;
		case 102:
			//诈金花
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底分";
			break;
		case 103:
			//斗地主
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底分";
			break;
		case 104:
			//牛牛
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底注/顶注";
			break;
		case 105:
			//梭哈
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底分";
			break;
		case 106:
			//麻将
			uiConfig.curGameType = UIConfig.CurGameType.MjGame;
			uiConfig.curGameRateName = "底分";
			break;
		case 107:
			//龙虎斗
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底分";
			break;
		case 108:
			//扯旋
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底分";
			break;
		case 109:
			//超级六
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底分";
			break;
		case 110:
			//ATT
			uiConfig.curGameType = UIConfig.CurGameType.Ext8Game;
			uiConfig.curGameRateName = "底注/顶注";
			break;
		case 111:
			//单挑
			uiConfig.curGameType = UIConfig.CurGameType.DtGame;
			uiConfig.curGameRateName = "底注/顶注";
			break;
		case 116:
			//金鲨银鲨
			uiConfig.curGameType = UIConfig.CurGameType.Ext8Game;
			uiConfig.curGameRateName = "底分";
			break;
		case 117:
			//幸运六狮
			uiConfig.curGameType = UIConfig.CurGameType.Ext8Game;
			uiConfig.curGameRateName = "底注/顶注";
			break;
		case 118:
			//水浒传
			uiConfig.curGameType = UIConfig.CurGameType.Ext8Game;
			uiConfig.curGameRateName = "底注/顶注";
			break;
		case 120:
			//跑马
			uiConfig.curGameType = UIConfig.CurGameType.Ext8Game;
			uiConfig.curGameRateName = "底注/顶注";
			break;
		case 400:
			//海洋之星
			uiConfig.curGameType = UIConfig.CurGameType.FishGame;
			uiConfig.curGameRateName = "最大炮值";
			break;
		case 402:
			//东方神龙
			uiConfig.curGameType = UIConfig.CurGameType.FishGame;
			uiConfig.curGameRateName = "最大炮值";
			break;
		case 403:
			//金蟾捕鱼
			uiConfig.curGameType = UIConfig.CurGameType.FishGame;
			uiConfig.curGameRateName = "最大炮值";
			break;
		case 404:
			//李逵劈鱼
			uiConfig.curGameType = UIConfig.CurGameType.FishGame;
			uiConfig.curGameRateName = "最大炮值";
			break;
		case 405:
			//大圣闹海
			uiConfig.curGameType = UIConfig.CurGameType.FishGame;
			uiConfig.curGameRateName = "最大炮值";
			break;
		case 406:
			//摇钱树
			uiConfig.curGameType = UIConfig.CurGameType.FishGame;
			uiConfig.curGameRateName = "最大炮值";
			break;
		default:
			uiConfig.curGameType = UIConfig.CurGameType.CardGame;
			uiConfig.curGameRateName = "底分";
			break;
		}
	}
	private void FindLastDesk()
	{
		Debug.LogWarning("FindLastDesk!!!!!!!!!!!!");
		if(!uiConfig.MobileEdition)
		{
			uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("findPlayer_btn").GetComponent<findUserBtnClick>().findDesk(uiConfig.curDeskNo,999999);
		}else{
			MoblieRoomBtns = uiConfig.page_roomDesk.GetComponentsInChildren<moblieRoomDeskBtnClick>();
			foreach(moblieRoomDeskBtnClick _temp in MoblieRoomBtns)
			{
				if(_temp.deskId == uiConfig.curDeskNo)
				{
					_temp.OnClick();
					break;
				}
			}
			Debug.LogWarning("curDeskNo:  " + uiConfig.curDeskNo + "  deskCount: " + uiConfig.deskCount);
			uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("desk_scrollBar").GetComponent<UIScrollBar>().value = ((float)uiConfig.curDeskNo)/((float)uiConfig.deskCount);
		}
	}
	public void resetRoomScrollView()
	{
		if(gameRoomList.Count == 0)
		{
			return;
		}

		if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
		{
			//赌博风
			gameRoomList[0].transform.parent.GetComponent<UIWrapContent>().SortBasedOnScrollMovement();
			gameRoomList[0].transform.parent.parent.GetComponent<UIScrollView>().ResetPosition();
			gameRoomList[0].transform.parent.GetComponent<UIWrapContent>().WrapContent();
			ScrollViewCtrl tempViewCtrl = gameRoomList[0].transform.parent.parent.parent.GetComponent<ScrollViewCtrl>();
			if(tempViewCtrl != null) tempViewCtrl.ScrollViewInit();
			return;
		}
		int roomCount = gameRoomList.Count;
		int cellHeight = 0;
		int roomsHeight = 0;

		for(int i = 0 ; i<HallTransfer.Instance.gameBtnList.Count; i++)
		{
			if(HallTransfer.Instance.gameBtnList [i].transform.FindChild ("waiting").gameObject!=null)
			{
				HallTransfer.Instance.gameBtnList [i].transform.FindChild ("waiting").gameObject.SetActive (false);
			}
		}
		if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
		{
			GameObject.Find("RoomGrid").GetComponent<UIWrapContent>().SortBasedOnScrollMovement();
			for(int i = 0; i< gameRoomList.Count; i++)
			{
				gameRoomList[i].transform.localPosition = new Vector3((float)(i*GameObject.Find("RoomGrid").GetComponent<UIWrapContent>().itemSize),0,0);
			}
		}else if(uiConfig.MobileEdition)
		{
			//移动版
			for(int i = 0; i < gameRoomList.Count; i++)
			{
				gameRoomList[i].transform.localPosition = new Vector3(0,( -69*i ),0);
			}
			cellHeight = uiConfig.btn_gameRoom.GetComponent<UISprite>().height + 12;
			roomsHeight = roomCount * cellHeight;
		}else{
			//PC版
			for(int i = 0; i < gameRoomList.Count; i++)
			{
				gameRoomList[i].transform.localPosition = new Vector3(0,(i * -(gameRoomList[i].GetComponent<UIWidget> ().height)),0);
			}
			cellHeight = uiConfig.btn_gameRoom.GetComponent<UISprite>().height;
			roomsHeight = 2 + roomCount * cellHeight;
			int tempBottomAnchor = uiConfig.page_gameRoom.GetComponent<UIWidget>().height - roomsHeight;
			if(tempBottomAnchor < 1) tempBottomAnchor = 1;
			RoomScrollView.GetComponent<UIPanel>().bottomAnchor.absolute = tempBottomAnchor;
		}
		for(int i = 0; i < gameRoomList.Count; i++)
		{
			gameRoomList[i].SetActive(true);
		}
		uiConfig.page_gameRoom.transform.FindChild("room_ScrollBar").GetComponent<UIScrollBar>().value = 0;
		RoomScrollView.ResetPosition();
		if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
		{

		}else{
			if(roomsHeight < RoomScrollView.GetComponent<UIPanel>().GetViewSize().y)
			{
				RoomScrollView.enabled = false;
			}else{
				RoomScrollView.enabled = true;
			}
		}
	}
	#endregion

	#region 接收座位信息
	public void cnSetRoomInfo( RoomDeskInfo deskInfo )
	{
		Debug.LogWarning("cnSetRoomInfo++++++++++++++++++++" + deskInfo.dwDeskPeople);
		roomUserList.Clear();//清空座位上玩家信息
		
		if (HallTransfer.Instance.uiConfig.window_MaskHall != null)
		{
			HallTransfer.Instance.uiConfig.window_MaskHall.SetActive(false);
		}
		if (HallTransfer.Instance.uiConfig.window_MaskRoom != null)
		{
			HallTransfer.Instance.uiConfig.window_MaskRoom.SetActive(false);
		}
		if (HallTransfer.Instance.gameRoomList.Count != 0) {
			for(int i = 0 ; i<HallTransfer.Instance.gameRoomList.Count; i++)
			{
				HallTransfer.Instance.gameRoomList[i].transform.FindChild("wait").gameObject.SetActive(false);
			}
		}

		//===============================================================================
		DeskState = new bool[deskInfo.dwDeskCount];
		for(var i = 0; i < deskInfo.dwDeskCount; i++)
		{
			DeskState[i] = false;		//设置桌子初始状态
		}
		uiConfig.deskCount = deskInfo.dwDeskCount;
		uiConfig.chairCount = deskInfo.dwDeskPeople;
		uiConfig.curRoomId = deskInfo.dwRoomId;
		for(var i = 0; i < roomDeskList.Count; i++)
		{
			Destroy(roomDeskList[i]);	//销毁桌子
		}
		roomDeskList.Clear();//清空房间按钮容器
		if(!uiConfig.MobileEdition)
		{
			//PC端
			uiConfig.window_FirstPage.SetActive(false);	//隐藏首页界面
			uiConfig.page_gameRoom.SetActive(false);	//隐藏游戏大厅界面
			if(uiConfig.page_scoreList != null) uiConfig.page_scoreList.SetActive(false);	//隐藏积分排行界面
			if(uiConfig.page_gameRule != null) uiConfig.page_gameRule.SetActive(false);	//隐藏游戏规则界面
			uiConfig.page_roomDesk.SetActive(true);
			uiConfig.page_roomDesk.transform.FindChild("desk_scrollBar").GetComponent<UIScrollBar>().value = 0;
			uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("gameName_btn").FindChild("Label").GetComponent<UILabel>().text = uiConfig.curGameName;
			uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("roomName_label").GetComponent<UILabel>().text = "/"+uiConfig.curRoomName;
			Debug.LogWarning("uiConfig.curRoomName:  " + uiConfig.curRoomName);
			//选几人桌
			uiConfig.curGameId.ToString();
//			string abName = string.Format(GlobalConst.Res.GameBasic.Name, deskInfo.dwGameID);
			GameObject curDeskType = null;
			for(var i = 0; i < uiConfig.gameRoomDesk.Length; i++)
			{
				if(uiConfig.gameRoomDesk[i].name == deskInfo.dwGameID.ToString() + "_desk")
				{
					curDeskType = uiConfig.gameRoomDesk[i];
					break;
				}
			}
//			GameObject curDeskType = (GameObject)Resources.Load(deskInfo.dwGameID.ToString() + "_desk");
//			games/lv_4000/lv_4000_basic_____4000desktype
//            GameObject curDeskType = GameApp.ResMgr.LoadAsset<GameObject>(abName, "basic_" + deskInfo.dwGameID.ToString() + "_desk");
			if(curDeskType == null)
			{
				Debug.LogError(deskInfo.dwGameID.ToString() + "desktype is not found...");
				return;
			}
			Transform tempDeskGrid = GameObject.Find("deskGrid").transform;
			if(tempDeskGrid.GetComponent<UISprite>()!=null) tempDeskGrid.GetComponent<UISprite>().alpha = 0;
			for(UInt32 i = 0; i < deskInfo.dwDeskCount; i++)//deskInfo.dwDeskCount
			{
				//创建桌子
				roomDeskList.Add( Instantiate(curDeskType,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject );

				GameObject desk = roomDeskList[ (int)i ];
				desk.transform.parent = tempDeskGrid;
				desk.transform.localScale = new Vector3(1f,1f,1f);
				Transform tempLabel = desk.transform.FindChild("desk_label");
				tempLabel.GetComponent<UILabel>().text = (i + 1) + "号桌";
//				desk.transform.FindChild("deskIco_img").gameObject.SetActive(false);
				desk.transform.FindChild("gaming_img").gameObject.SetActive(DeskState[i]);

				for(UInt32 j = 0; j < deskInfo.dwDeskPeople; j++)
				{
					if(j>7) break;
					Transform tempChair = desk.transform.FindChild("chair" + j);
					tempChair.gameObject.GetComponent<CGameChairItem>().roomid = deskInfo.dwRoomId;
					tempChair.gameObject.GetComponent<CGameChairItem>().deskid = i;
					tempChair.gameObject.GetComponent<CGameChairItem>().chairid = j;

					tempChair.gameObject.GetComponent<CGameChairItem>().empty = true;//状态空

					tempChair.gameObject.GetComponent<UIButton>().enabled = true;//启用按钮
					Transform tempChairLabel = tempChair.FindChild("label");
					tempChairLabel.gameObject.SetActive(true);//"座位"显示
					Transform tempChairFace = tempChair.transform.FindChild("face_img");
					tempChairFace.gameObject.SetActive(false);
					Transform tempChairFindUser = tempChair.transform.FindChild("findUser");
					tempChairFindUser.gameObject.SetActive(false);
					Transform tempChairName = tempChair.transform.FindChild("name_label");
					tempChairName.gameObject.GetComponent<UILabel>().text = "";
				}
			}
			Invoke("deskReposition",0.5f);// deskReposition();
			if(uiConfig.quickEnterDesk)
			{
				uiConfig.quickEnterDesk = false;
				uiConfig.page_roomDesk.transform.FindChild("topBar").FindChild("quickEnter_btn").GetComponent<quickEnterBtnClick>().OnClick();
			}

		}else{

			if(uiConfig.curLobbyStyle == UIConfig.LobbyStyle.mGambleStyle)
			{
				//赌场
				Debug.LogWarning(uiConfig.curLobbyStyle.ToString());
				string tempGameID = deskInfo.dwGameID.ToString();
				tempGameID = tempGameID.Substring(0,1)+tempGameID.Substring(2,2);
				uiConfig.curGameId = uint.Parse(tempGameID);
				SetCurGameType();
				uiConfig.page_gameRoom.transform.FindChild(uiConfig.curGameType.ToString()).GetComponent<TweenAlpha>().PlayReverse();//.gameObject.SetActive(false);
				uiConfig.mobileFishDesk = uiConfig.page_gameRoom.transform.FindChild( uiConfig.curGameType.ToString()+"Desk"+uiConfig.chairCount.ToString() );
				uiConfig.mobileFishDesk.GetComponent<TweenPosition>().ResetToBeginning();
				uiConfig.mobileFishDesk.GetComponent<TweenPosition>().PlayForward();//.gameObject.SetActive(true);
				uiConfig.page_gameRoom.transform.FindChild("Desks").GetComponent<TweenPosition>().ResetToBeginning();
				uiConfig.page_gameRoom.transform.FindChild("Desks").GetComponent<TweenPosition>().PlayForward();//.gameObject.SetActive(true);
				uiConfig.page_gameRoom.transform.FindChild("UserMoney").GetComponent<TweenPosition>().PlayForward();
				for(UInt32 j = 0; j < deskInfo.dwDeskPeople; j++)
				{
					Transform tempChair = uiConfig.mobileFishDesk.transform.FindChild("chair" + j).transform.FindChild("chair_btn");
					tempChair.gameObject.GetComponent<CGameChairItem>().roomid = deskInfo.dwRoomId;
					tempChair.gameObject.GetComponent<CGameChairItem>().deskid = 0;
					tempChair.gameObject.GetComponent<CGameChairItem>().chairid = j;
					tempChair.gameObject.GetComponent<CGameChairItem>().empty = true;//状态空
					Transform tempChairFace = tempChair.parent.FindChild("face");
					tempChairFace.FindChild("face_img").GetComponent<UISprite>().atlas = userFace_atlas;
					tempChairFace.gameObject.SetActive(false);
					if(tempChair.parent.FindChild("game_img")!=null)
					{
						string tempSpriteName = tempChair.parent.FindChild("game_img").GetComponent<UISprite>().spriteName;
						tempChair.parent.FindChild("game_img").GetComponent<UISprite>().spriteName = tempSpriteName.Substring(0,tempSpriteName.Length-3)+uiConfig.curGameId.ToString();
					}
				}
				for(int i = 0; i < deskInfo.dwDeskCount; i++)
				{
					//创建桌子按钮
					roomDeskList.Add( Instantiate(uiConfig.phone_deskBtn,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject );
					GameObject desk = roomDeskList[i];
					Transform tempGrid = uiConfig.mobileFishDesk.parent.FindChild("Desks").FindChild("Desk_ScrollView").FindChild("DeskGrid");
					desk.transform.parent = tempGrid;
					desk.transform.localScale = new Vector3(1f,1f,1f);
					desk.transform.localPosition = new Vector3(0f, -(tempGrid.GetComponent<UIGrid>().cellHeight*i), 0f);
					desk.GetComponent<moblieRoomDeskBtnClick>().deskId = (uint)i;
					Transform tempLabel = desk.transform.FindChild("deskId_label");
					tempLabel.GetComponent<UILabel>().text = (i + 1) + "号桌";
					desk.transform.FindChild("manCount_label").GetComponent<UILabel>().text = "0/" + uiConfig.chairCount;
					desk.transform.FindChild("toggle_img").gameObject.SetActive(false);
					desk.transform.FindChild("manSlider").GetComponent<UISlider>().value = 0;
					if(i == 0)
					{
						desk.transform.FindChild("toggle_img").gameObject.SetActive(true);
					}
				}
				uiConfig.mobileFishDesk.parent.FindChild("Desks").FindChild("Desk_ScrollView").GetComponent<UIScrollView>().ResetPosition();
			}else{
				//移动端
				uiConfig.page_roomDesk.SetActive(true);
				
				Transform tempTitle = uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("title_label");
				tempTitle.GetComponent<UILabel>().text= uiConfig.curRoomName;		//设置标题
				//选几人桌
//				string abName = string.Format(GlobalConst.Res.GameBasic.Name, deskInfo.dwGameID);
//				GameObject curDeskType = GameApp.ResMgr.LoadAsset<GameObject>(abName, "basic_" + deskInfo.dwGameID.ToString() + "_desk");
				GameObject curDeskType = null;
				for(var i = 0; i < uiConfig.gameRoomDesk.Length; i++)
				{
					if(uiConfig.gameRoomDesk[i].name == deskInfo.dwGameID.ToString() + "_desk")
					{
						curDeskType = uiConfig.gameRoomDesk[i];
						break;
					}
				}
				if(curDeskType == null)
				{
					Debug.LogError("desktype is not found..." + deskInfo.dwGameID.ToString());
					return;
				}
				
				if( uiConfig.mobileFishDesk == null || uiConfig.mobileFishDesk.name != curDeskType.name)
				{
					//创建大桌子
					if(uiConfig.mobileFishDesk != null) Destroy(uiConfig.mobileFishDesk.gameObject);
					uiConfig.mobileFishDesk = Instantiate(curDeskType.transform,Vector3.zero,Quaternion.Euler(new Vector3(0f,0f,0f)))as Transform;
					uiConfig.mobileFishDesk.parent = uiConfig.page_roomDesk.transform.FindChild("front_panel").transform;
					uiConfig.mobileFishDesk.localPosition = new Vector2(-193f,-60f);
					uiConfig.mobileFishDesk.localScale = Vector3.one;
				}
				for(UInt32 j = 0; j < deskInfo.dwDeskPeople; j++)
				{
					Transform tempChair = uiConfig.mobileFishDesk.transform.FindChild("chair" + j).transform.FindChild("chair_btn");
					tempChair.gameObject.GetComponent<CGameChairItem>().roomid = deskInfo.dwRoomId;
					tempChair.gameObject.GetComponent<CGameChairItem>().deskid = 0;
					tempChair.gameObject.GetComponent<CGameChairItem>().chairid = j;
					tempChair.gameObject.GetComponent<CGameChairItem>().empty = true;//状态空
					tempChair.gameObject.GetComponent<UIButton>().enabled = true;//启用按钮
					Transform tempChairLabel = tempChair.parent.FindChild("click_label");
					tempChairLabel.gameObject.SetActive(true);//"座位"显示
					Transform tempChairFace = tempChair.parent.FindChild("face");
					tempChairFace.FindChild("face_img").GetComponent<UISprite>().atlas = userFace_atlas;
					tempChairFace.gameObject.SetActive(false);
				}
				for(UInt32 i = 0; i < deskInfo.dwDeskCount; i++)//deskInfo.dwDeskCount
				{
					//Debug.LogWarning("桌子" + i);
					//创建桌子按钮
					roomDeskList.Add( Instantiate(uiConfig.phone_deskBtn,Vector3.one,Quaternion.Euler(new Vector3(0f,0f,0f)))as GameObject );
					GameObject desk = roomDeskList[ (int)i ];
					desk.transform.parent = GameObject.Find("desk_scrollView").transform;
					desk.transform.localScale = new Vector3(1f,1f,1f);
					desk.transform.localPosition = new Vector3(25f, -45f-67f*i, 0f);
					desk.GetComponent<moblieRoomDeskBtnClick>().deskId = i;
					Transform tempLabel = desk.transform.FindChild("deskId_label");
					tempLabel.GetComponent<UILabel>().text = (i + 1) + "号桌";
					desk.transform.FindChild("manCount_label").GetComponent<UILabel>().text = "0/" + uiConfig.chairCount;
					desk.transform.FindChild("toggle_img").gameObject.SetActive(false);
					desk.transform.FindChild("manSlider").GetComponent<UISlider>().value = 0;
					if(i == 0)
					{
						desk.transform.FindChild("toggle_img").gameObject.SetActive(true);
					}
				}
				
				deskReposition();
			}

		}
	}

	public void deskReposition()
	{
		//重新排列桌子
		if (HallTransfer.Instance.uiConfig.window_MaskHall != null)
		{
			HallTransfer.Instance.uiConfig.window_MaskHall.SetActive(false);
		}

		if (!uiConfig.MobileEdition) {
			//PC端
			GameObject bg = uiConfig.page_roomDesk;//GameObject.Find ("BG_center_bg");
			Transform _tempDeskGrid = bg.GetComponentInChildren<UIScrollView>().transform.FindChild("deskGrid");
			_tempDeskGrid.localPosition = new Vector2((float)bg.GetComponentInChildren<UIScrollView>().GetComponent<UIPanel>().leftAnchor.absolute,0f);
			int viewWidth, viewHeight, cellWidth, cellHeight, widthInterval, heightInterval, cellColumn, cellRow;
			int miniWidthInterval = 5;
			int miniHeightInterval = 40;
			cellWidth = roomDeskList [0].GetComponent<UIWidget> ().width;
			cellHeight = roomDeskList [0].GetComponent<UIWidget> ().height;
			viewWidth = (int)bg.GetComponentInChildren<UIScrollView>().transform.GetComponent<UIPanel>().GetViewSize().x;//GetComponent<UISprite> ().width - 40;
			viewHeight = (int)bg.GetComponentInChildren<UIScrollView>().transform.GetComponent<UIPanel>().GetViewSize().y;//.GetComponent<UISprite> ().height - 60;
			uiConfig.deskColumnCount = cellColumn = (viewWidth - miniWidthInterval) / (cellWidth + miniWidthInterval);
			uiConfig.deskRowCount = cellRow = System.Convert.ToInt32 (roomDeskList.Count / cellColumn);
			widthInterval = (viewWidth - cellWidth * cellColumn) / (cellColumn + 1);
			int tempColumn = 0;
			int tempRow = 0;
			for (int i = 0; i < roomDeskList.Count; i++) {
				float tempX = widthInterval + (tempColumn * (widthInterval +  cellWidth));
				float tempY = (miniHeightInterval + (tempRow * (miniHeightInterval + cellHeight))) * -1;
				roomDeskList [i].transform.localPosition = new Vector3 (tempX, tempY, 0f);
				if (++tempColumn == cellColumn) {
					tempColumn = 0;
					tempRow++;
				}
			}
			Debug.LogWarning( "deskReposition!!!!!!!" + viewWidth.ToString() + "  ");
			uiConfig.page_roomDesk.transform.FindChild ("desk_scrollBar").GetComponent<UIScrollBar> ().value = 0;
			uiConfig.page_roomDesk.transform.FindChild ("desk_ScrollView").GetComponent<UIScrollView> ().ResetPosition();
			Transform tempDeskGrid = GameObject.Find("deskGrid").transform;
			if(tempDeskGrid.GetComponent<UISprite>()!=null) tempDeskGrid.GetComponent<UISprite>().alpha = 1;
		}
		else 
		{	
			Transform scrollBar =  HallTransfer.Instance.uiConfig.page_roomDesk.transform.FindChild("front_panel").FindChild("desk_scrollBar");	
			scrollBar.GetComponent<UIScrollBar>().value = 0f;

		}
	}
	#endregion

	#region 玩家坐下信息


	public void cnShowUserInfo(List<HallTransfer.RoomUserInfo> users)
	{
		StopCoroutine("BuildUserInfoList");
		StartCoroutine("BuildUserInfoList",users);
	}
	private  IEnumerator BuildUserInfoList( List<HallTransfer.RoomUserInfo> users )
	{
		foreach(var item in users)
		{
			cnShowUserInfo(item);
			yield return new WaitForFixedUpdate();
		}
	}
	public void cnShowUserInfo( RoomUserInfo userInfo )
	{
//		string tempGameId = "";
//		string tempDeskNeme = "";
//
		for(int i=0; i<roomUserList.Count; i++)
		{
			if(roomUserList[i].dwUserId == userInfo.dwUserId)
			{
				roomUserList.RemoveAt(i);
				break;
			}
		}
		roomUserList.Add(userInfo);//丢进容器

		int tempManCount = 0;
		for(int i = 0; i < roomUserList.Count; i++)
		{
			if(roomUserList[i].dwDesk == userInfo.dwDesk) tempManCount++;
		}
		CGameDeskManger._instance.m_lstGameDeskList[(int)userInfo.dwDesk].GetComponent<CGameDeskItem>()
			.UpdateGameDeskInfo((int)userInfo.dwDesk,tempManCount,CGameDeskManger._instance.m_RoomDeskInfo);//更新桌子数据
		int tempDeskIndex = (int)userInfo.dwDesk;
		CGameDeskManger._instance.m_lstGameDeskList[tempDeskIndex].m_lstUserInfos.Add(userInfo);

		if(CChairManger._instance!=null)
		{
			for(int i = 0; i < CChairManger._instance.m_lRoomUChairList.Count; i++)
			{
				if(userInfo.dwDesk == CChairManger._instance.m_lRoomUChairList[i].deskid && userInfo.dwChair == CChairManger._instance.m_lRoomUChairList[i].chairid)
				{
					CChairManger._instance.m_lRoomUChairList[i].UserSitDown(userInfo);//玩家坐下
				}
			}
		}else{
			var desk = CGameDeskManger._instance.m_lstGameDeskList[(int)userInfo.dwDesk];
			var chair = desk.m_ChairItems[(int)userInfo.dwChair];
			chair.UserSitDown(userInfo);//玩家坐下
		}
	}

	#endregion

	#region 设置桌子游戏中状态
	public void cnSetDeskStatus( UInt32 deskId, byte status)
	{
//		DeskState[deskId] = (status == 0?false:true);
//		if(!uiConfig.MobileEdition)
//		{
//			if(roomDeskList[(int)deskId] != null) roomDeskList[(int)deskId].transform.FindChild("gaming_img").gameObject.SetActive(DeskState[deskId]);
//		}else{
//			if(roomDeskList[(int)deskId] != null)
//			{
//				roomDeskList[(int)deskId].transform.FindChild("ruchang_label").GetComponent<UILabel>().text = (DeskState[deskId]?"游戏中":"可进入");
//				roomDeskList[(int)deskId].transform.FindChild("ruchang_label").GetComponent<UILabel>().color = (DeskState[deskId]?Color.yellow:Color.green);
//			}
//		}
	}

	#endregion
	
	#region 玩家起立信息

	public void cnHideUserInfo( UInt32 userId )
	{
		uint tempDeskIndex = 0;
		for(int i=0; i<roomUserList.Count; i++)
		{
			if(roomUserList[i].dwUserId == userId)
			{
				tempDeskIndex = roomUserList[i].dwDesk;
				if(CChairManger._instance!=null)
				{
					for(int j = 0; j < CChairManger._instance.m_lRoomUChairList.Count; j++)
					{
						if(roomUserList[i].dwDesk == CChairManger._instance.m_lRoomUChairList[j].deskid && roomUserList[i].dwChair == CChairManger._instance.m_lRoomUChairList[j].chairid)
						{
							CChairManger._instance.m_lRoomUChairList[j].ClearUserInfo();//玩家起立
							CChairManger._instance.m_lRoomUChairList.RemoveAt(j);
							break;
						}
					}
				}else{
					CGameDeskManger._instance.m_lstGameDeskList[(int)roomUserList[i].dwDesk].m_ChairItems[(int)roomUserList[i].dwChair].ClearUserInfo();
				}
				roomUserList.RemoveAt(i);
				break;
			}
		}
		int tempManCount = 0;
		for(int i = 0; i < roomUserList.Count; i++)
		{
			if(roomUserList[i].dwDesk == tempDeskIndex) tempManCount++;
		}
		CGameDeskManger._instance.m_lstGameDeskList[(int)tempDeskIndex].GetComponent<CGameDeskItem>()
			.UpdateGameDeskInfo((int)tempDeskIndex,tempManCount,CGameDeskManger._instance.m_RoomDeskInfo);//更新桌子数据
//		if(!uiConfig.MobileEdition)
//		{
//			//PC端
//			for(int i = 0; i < roomUserList.Count; i++)
//			{
//				if(roomUserList[i].dwUserId == userId)
//				{
//					int tempDeskId = (int)roomUserList[i].dwDesk;
//					int tempChairId = (int)roomUserList[i].dwChair;
//					Transform tempChair =  roomDeskList[tempDeskId].transform.FindChild("chair" + tempChairId);
//					tempChair.gameObject.GetComponent<CGameChairItem>().empty = true;//状态空
//					tempChair.gameObject.GetComponent<UIButton>().enabled = true;//启用按钮
//					tempChair.FindChild("wait").gameObject.SetActive(false);
//					Transform tempLabel = tempChair.FindChild("label");
//					tempLabel.gameObject.SetActive(true);//"座位"显示
//					Transform tempName = tempChair.FindChild("name_label");
//					tempName.gameObject.GetComponent<UILabel>().text = "";//隐藏名称
//					Transform tempFace = tempChair.FindChild("face_img");
//					tempFace.gameObject.SetActive(false);//隐藏头像
//					roomUserList.RemoveAt(i);//收拾掉
//					break;
//				}
//			}
//		}else{
//			//移动端
//
//			int tempDesk = 0;
//			for(int i = 0; i < roomUserList.Count; i++)
//			{
//				if(roomUserList[i].dwUserId == userId)
//				{
//					tempDesk = i;
//					break;
//				}
//			}
//			int tempRoomDesk = (int)roomUserList[tempDesk].dwDesk;
//			Transform tempChair = uiConfig.mobileFishDesk.transform.FindChild("chair" + roomUserList[tempDesk].dwChair);
//			if(roomUserList[tempDesk].dwDesk == tempChair.FindChild("chair_btn").GetComponent<CGameChairItem>().deskid)
//			{
//				//隐藏头像
//				tempChair.FindChild("face").gameObject.SetActive(false);
//				//设置椅子为空
//				tempChair.FindChild("chair_btn").GetComponent<CGameChairItem>().empty = true;
//				tempChair.FindChild("wait").gameObject.SetActive(false);
//			}
//
//			roomUserList.RemoveAt(tempDesk);//收拾掉
//			roomDeskList[tempRoomDesk].GetComponent<moblieRoomDeskBtnClick>().RefreshManCount();//刷新人数
//		}

	}

	#endregion

	#region 接收进入保险柜返回消息
	public	void	cnSafetyBoxResult( int value )
	{
		Debug.LogWarning("cnSafetyBoxResult====================== " + value) ;
		if(value == 0)
		{
			//成功
//			Transform tempSubmitBtn = uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("submit_btn");
//			tempSubmitBtn.gameObject.GetComponent<UIButton>().isEnabled = true;
//			Transform tempCloseBtn = uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("close_btn");
//			tempCloseBtn.gameObject.GetComponent<UIButton>().isEnabled = true;
//			uiConfig.window_SafeBoxEntry.SetActive(false);
			uiConfig.window_SafeBox.SetActive(true);
			TweenPosition tempTween = uiConfig.window_SafeBox.GetComponent<TweenPosition>();
			if(tempTween != null ) tempTween.PlayForward();

//			if(uiConfig.page_roomDesk.activeSelf)
//			{
//				uiConfig.page_roomDesk.SetActive(false);//关闭坐桌界面
//				uiConfig.page_gameRoom.SetActive(true);//打开房间界面
//				ncQuitRoomDesk();
//			}

		}else
		{
			//失败
			Transform tempSubmitBtn = uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("submit_btn");
			tempSubmitBtn.gameObject.GetComponent<UIButton>().isEnabled = true;
			Transform tempCloseBtn = uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("close_btn");
			tempCloseBtn.gameObject.GetComponent<UIButton>().isEnabled = true;
			Transform tempLog = uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("log_label");
			tempLog.gameObject.GetComponent<UILabel>().text = "密码错误,请重新输入!";
			Transform tempInput = uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("pass_Input");
			if(!uiConfig.MobileEdition) tempInput.gameObject.GetComponent<UIInput>().isSelected = true;
			Invoke("cleanLogLabel",2.0f);
		}
	}
	private void cleanLogLabel()
	{
		Transform tempLog = uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("log_label");
		tempLog.gameObject.GetComponent<UILabel>().text = "";
	}
	#endregion

	#region 接收保险柜数据
	public	void	cnUpdataBankMoney( Int64 money, Int64 bank)
	{
		Debug.LogWarning("cnUpdataBankMoney====================== " + money + " + " + bank);
		uiConfig.bankMoney = bank>=0 ? bank : 0;
		Transform tempPageSave = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_save&take");
		Transform tempSaveContent = tempPageSave.FindChild("content");
		Transform tempMoneyLabel = tempSaveContent.FindChild("money_label");
		Transform tempSafeBoxMoneyLabel = tempSaveContent.FindChild("safeBoxMoney_label");
		//tempMoneyLabel.gameObject.GetComponent<UILabel>().text = money.ToString();			//存取界面 金钱赋值
		tempMoneyLabel.gameObject.GetComponent<UILabel> ().text = money>=0 ? money.ToString("N0") : "0"; //;transMoney (money);
		//tempSafeBoxMoneyLabel.gameObject.GetComponent<UILabel>().text = bank.ToString();	//存取界面 银行赋值
		tempSafeBoxMoneyLabel.gameObject.GetComponent<UILabel>().text = bank>=0 ? bank.ToString("N0") : "0";//;transMoney (bank);
		Transform tempInput = tempSaveContent.FindChild("money_Input");
		if(!uiConfig.MobileEdition) tempInput.gameObject.GetComponent<UIInput>().isSelected = true;

//		Transform tempPageSave1 = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_give");
//		Transform tempSafeBoxMoneyLabel1 = tempPageSave1.FindChild("content").FindChild("safeMoney_label");
//		tempSafeBoxMoneyLabel1.GetComponent<UILabel>().text = bank.ToString();				//赠送界面 银行赋值

		GameObject.Find("userMoney_label").GetComponent<UILabel>().text = money.ToString();			//玩家信息 金钱赋值
//		uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_give").FindChild("content").FindChild("money_Input").FindChild("capitalMoney").gameObject.GetComponent<capitalMoney>().maxCount = bank;

		double maxCount;
		if(money >= bank)
		{
			maxCount = (double)money;
		}else{
			maxCount = (double)bank;
		}
		Debug.LogWarning("maxCount:" + maxCount);
		uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_save&take").FindChild("content").FindChild("money_Input").FindChild("capitalMoney").gameObject.GetComponent<capitalMoney>().maxCount = maxCount;
		if (!uiConfig.MobileEdition) 
		{
			uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_save&take").FindChild("content").FindChild("moneyAll_btn").gameObject.GetComponent<showWindow>().input_txt[0] = maxCount.ToString();
		}
	}
	#endregion

	#region 接收保险柜 赠送 返回消息
	public	void	cnGiveMoneyResult( int HandleCode )
	{
		Debug.LogWarning("cnGiveMoneyResult====================== " + HandleCode);
		if(HandleCode == 2)
		{
			//成功
//			Transform tempPageGive = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_give");
//			Transform tempLog0 = tempPageGive.FindChild("content").FindChild("log0_label");
//			tempLog0.GetComponent<UILabel>().color = Color.blue;
//			tempLog0.GetComponent<UILabel>().text = "赠送成功!";
		}else{
			//失败
//			Transform tempPageGive = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_give");
//			Transform tempLog0 = tempPageGive.FindChild("content").FindChild("log0_label");
//			tempLog0.GetComponent<UILabel>().color = Color.red;
//			tempLog0.GetComponent<UILabel>().text = "赠送失败!";
		}
		Invoke("cleanGiveMoneyResultLog",2.0f);
	}
	void cleanGiveMoneyResultLog()
	{
//		Transform tempPageGive = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_give");
//		Transform tempLog0 = tempPageGive.FindChild("content").FindChild("log0_label");
//		Transform tempLog1 = tempPageGive.FindChild("content").FindChild("log1_label");
//		tempLog0.GetComponent<UILabel>().text = "";
//		tempLog1.GetComponent<UILabel>().text = "";
	}
	#endregion


	#region 接收保险柜 取钱 存钱 返回消息
	public	void	cnCheckInOrOUtMoneyResult(/*int HandleCode,*/ string msg )
	{
		if(uiConfig.window_SafeBox_mask != null)
		{
			uiConfig.window_SafeBox_mask.SetActive(false);
		}

		Debug.LogWarning("cnCheckInOrOUtMoneyResult====================== "  + " + " + msg);
		Transform tempPageSave = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_save&take");
		GameObject tempSave = tempPageSave.FindChild("content").FindChild("save_btn").gameObject;
		tempSave.GetComponent<UIButton>().isEnabled = true;
		GameObject tempTake = tempPageSave.FindChild("content").FindChild("take_btn").gameObject;
		tempTake.GetComponent<UIButton>().isEnabled = true;
		GameObject tempToggle = tempSave.GetComponent<safeBox_saveTake_btnClick>().toggleBtn;
		if(tempToggle != null) tempToggle.GetComponent<UIButton>().isEnabled = true;
		if(tempToggle != null) tempToggle.GetComponent<showWindow>().enabled = true;
		GameObject tempToggle1 = tempSave.GetComponent<safeBox_saveTake_btnClick>().toggleBtn1;
		if(tempToggle1 != null) tempToggle.GetComponent<UIButton>().isEnabled = true;
		if(tempToggle1 != null) tempToggle.GetComponent<showWindow>().enabled = true;
		GameObject tempToggle2 = tempSave.GetComponent<safeBox_saveTake_btnClick>().toggleBtn2;
		if(tempToggle2 != null) tempToggle.GetComponent<UIButton>().isEnabled = true;
		if(tempToggle2 != null) tempToggle.GetComponent<showWindow>().enabled = true;
		GameObject tempClose = tempPageSave.parent.FindChild("close_btn").gameObject;
		tempClose.GetComponent<UIButton>().isEnabled = true;

		GameObject tempLog0 = tempPageSave.FindChild("content").FindChild("log0_label").gameObject;
		GameObject tempLog1 = tempPageSave.FindChild("content").FindChild("log1_label").gameObject;
		tempLog0.GetComponent<UILabel>().text = "";
		tempLog1.GetComponent<UILabel>().text = "";
		//SubCmdID:		2:取钱 3:存钱 9:游戏中取钱 11:游戏中存钱
		//HandleCode: 	1:取钱\存钱失败 2:取钱\存钱成功 3:游戏中存钱 4:密码错误 10:密码错误多次,账号锁定 11:游戏中取钱 12:钱不够
//		if(HandleCode == 2)
//		{
//			tempLog0.GetComponent<UILabel>().color = Color.blue;
//			tempLog0.GetComponent<UILabel>().text = "成功";
//		}else if(HandleCode == 1)
//		{
//			tempLog0.GetComponent<UILabel>().color = Color.red;
//			tempLog0.GetComponent<UILabel>().text = "失败";
//		}else if(HandleCode == 4)
//		{
//			tempLog0.GetComponent<UILabel>().color = Color.red;
//			tempLog0.GetComponent<UILabel>().text = "密码错误";
//		}else if(HandleCode == 10)
//		{
//			tempLog0.GetComponent<UILabel>().color = Color.red;
//			tempLog0.GetComponent<UILabel>().text = "密码错误多次,账号已锁定";
//		}else if(HandleCode == 3)
//		{
//			tempLog0.GetComponent<UILabel>().color = Color.red;
//			tempLog0.GetComponent<UILabel>().text = "游戏中不能存钱,请退出游戏重试";
//		}else if(HandleCode == 11)
//		{
//			tempLog0.GetComponent<UILabel>().color = Color.red;
//			tempLog0.GetComponent<UILabel>().text = "游戏中不能取钱,请退出游戏重试";
//		}else if(HandleCode == 12)
//		{
//			tempLog0.GetComponent<UILabel>().color = Color.red;
//			tempLog0.GetComponent<UILabel>().text = "金钱不足";
//		}
		if (msg != "") {
			cnTipsBox (msg);
		}

		Invoke("cleanCheckInOrOUtMoneyResultLog",2.0f);
	}

	void cleanCheckInOrOUtMoneyResultLog()
	{
		Transform tempPageSave = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_save&take");
		GameObject tempLog0 = tempPageSave.FindChild("content").FindChild("log0_label").gameObject;
		tempLog0.GetComponent<UILabel>().text = "";
	}
	#endregion
	
	#region 接收保险柜 修改密码 返回消息
	public	void	cnChangePassWDResult( int HandleCode )
	{
		if(uiConfig.window_SafeBox_mask != null)
		{
			uiConfig.window_SafeBox_mask.SetActive(false);
		}
		Debug.LogWarning("cnChangePassWDResult====================== "  + " + " + HandleCode);
		Transform tempPageChange = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_changePass");
		tempPageChange.FindChild("content").FindChild("change_btn").gameObject.GetComponent<UIButton>().isEnabled = true;
		GameObject tempLog0 = tempPageChange.FindChild("content").FindChild("log0_label").gameObject;
		GameObject tempLog1 = tempPageChange.FindChild("content").FindChild("log1_label").gameObject;
		GameObject tempLog2 = tempPageChange.FindChild("content").FindChild("log2_label").gameObject;
		GameObject change_btn = tempPageChange.FindChild("content").FindChild("change_btn").gameObject;
		GameObject close_btn = tempPageChange.parent.FindChild("close_btn").gameObject;
		GameObject ridio0_btn = tempPageChange.FindChild("content").FindChild("ridio0_btn").gameObject;
		GameObject ridio1_btn = tempPageChange.FindChild("content").FindChild("ridio1_btn").gameObject;
		GameObject toggleBtn = tempPageChange.FindChild("btns").FindChild("1save&take_btn").gameObject;

		change_btn.GetComponent<UIButton>().isEnabled = true;
		close_btn.GetComponent<UIButton>().isEnabled = true;
		ridio0_btn.GetComponent<UIButton>().isEnabled = true;
		ridio1_btn.GetComponent<UIButton>().isEnabled = true;
		ridio0_btn.GetComponent<showWindow>().enabled = true;
		ridio1_btn.GetComponent<showWindow>().enabled = true;
		toggleBtn.GetComponent<UIButton>().isEnabled = true;
		toggleBtn.GetComponent<showWindow>().enabled = true;

		tempLog0.GetComponent<UILabel>().text = "";
		tempLog1.GetComponent<UILabel>().text = "";
		tempLog2.GetComponent<UILabel>().text = "";
		//HandleCode:	0:操作成功 1:操作失败 20:锁定或者解锁账号绑定
		if(HandleCode == 0)
		{
			tempLog0.GetComponent<UILabel>().color = Color.blue;
			tempLog0.GetComponent<UILabel>().text = "修改成功";
			Invoke("cleanChangePassWDResultLog",2.0f);
//			cnMsgBox("修 改 成 功 !");
		}else if(HandleCode == 1)
		{
			tempLog0.GetComponent<UILabel>().color = Color.red;
			tempLog0.GetComponent<UILabel>().text = "旧密码输入错误";
			Invoke("cleanChangePassWDResultLog",2.0f);
		}else if(HandleCode == 2)
		{
			tempLog0.GetComponent<UILabel>().color = Color.red;
			tempLog0.GetComponent<UILabel>().text = "账号有误";
			Invoke("cleanChangePassWDResultLog",2.0f);
		}
		else if(HandleCode == 3)
		{
			tempLog0.GetComponent<UILabel>().color = Color.red;
			tempLog0.GetComponent<UILabel>().text = "密码有误";
			Invoke("cleanChangePassWDResultLog",2.0f);
		}
		else if(HandleCode == 20)
		{
			tempLog0.GetComponent<UILabel>().color = Color.red;
			tempLog0.GetComponent<UILabel>().text = "账号已锁定";
			Invoke("cleanChangePassWDResultLog",2.0f);
		}
	}
	void cleanChangePassWDResultLog()
	{
		Transform tempPageChange = uiConfig.window_SafeBox.transform.FindChild("front_panel").FindChild("page_changePass");
		GameObject tempLog0 = tempPageChange.FindChild("content").FindChild("log0_label").gameObject;
		tempLog0.GetComponent<UILabel>().text = "";
	}
	#endregion
	
	#region 接收 绑定\解绑 返回消息
	public	void	cnAccountResult( UInt32 commanType, UInt32 commanResult )
	{
		Debug.LogWarning("cnAccountResult====================== " + commanType + " + " + commanResult);

		if (uiConfig.window_Lock_mask != null) {
			uiConfig.window_Lock_mask.SetActive(false);
		}

		//commanResult  0:成功  1:失败
		if(commanResult == 0)
		{
			string ctrlType = "解除绑定";
			if(commanType == 1) ctrlType = "绑定";
			GameObject tempLog =  uiConfig.window_LockOrUnLock.transform.FindChild("front_panel").FindChild("log_label").gameObject;
			tempLog.GetComponent<UILabel>().color = Color.green;
			tempLog.GetComponent<UILabel>().text = ctrlType + "成功";
			tempLog.transform.parent.FindChild("label0").gameObject.SetActive(!tempLog.transform.parent.FindChild("label0").gameObject.activeSelf);
			tempLog.transform.parent.FindChild("label1").gameObject.SetActive(!tempLog.transform.parent.FindChild("label1").gameObject.activeSelf);
			tempLog.transform.parent.FindChild("pass_Input").gameObject.GetComponent<UIInput>().value = "";
			tempLog.transform.parent.FindChild("lock_btn").gameObject.GetComponent<UIButton>().isEnabled = true;
			tempLog.transform.parent.FindChild("unlock_btn").gameObject.GetComponent<UIButton>().isEnabled = true;
			tempLog.transform.parent.FindChild("cancel_btn").gameObject.GetComponent<UIButton>().isEnabled = true;
			tempLog.transform.parent.FindChild("close_btn").gameObject.GetComponent<UIButton>().isEnabled = true;
			tempLog.transform.parent.FindChild("lock_btn").gameObject.SetActive(!tempLog.transform.parent.FindChild("lock_btn").gameObject.activeSelf);
			tempLog.transform.parent.FindChild("unlock_btn").gameObject.SetActive(!tempLog.transform.parent.FindChild("unlock_btn").gameObject.activeSelf);
			uiConfig.LockOrUnLockAccount = !uiConfig.LockOrUnLockAccount;
		}else{
			GameObject tempLog =  uiConfig.window_LockOrUnLock.transform.FindChild("front_panel").FindChild("log_label").gameObject;
			tempLog.GetComponent<UILabel>().color = Color.red;
			tempLog.GetComponent<UILabel>().text = "密码错误";
			tempLog.transform.parent.FindChild("lock_btn").gameObject.GetComponent<UIButton>().isEnabled = true;
			tempLog.transform.parent.FindChild("unlock_btn").gameObject.GetComponent<UIButton>().isEnabled = true;
			tempLog.transform.parent.FindChild("cancel_btn").gameObject.GetComponent<UIButton>().isEnabled = true;
			tempLog.transform.parent.FindChild("close_btn").gameObject.GetComponent<UIButton>().isEnabled = true;
		}
		Invoke ("cleanLog_LockOrUnLock", 3.0f);
	}

	void cleanLog_LockOrUnLock()
	{
		GameObject tempLog =  uiConfig.window_LockOrUnLock.transform.FindChild("front_panel").FindChild("log_label").gameObject;
		tempLog.GetComponent<UILabel>().text = "";
	}

	#endregion



	#region 接收 点击"资料"后 玩家信息
	public bool	msgTooLate_UIF = false;		//返回用户资料是否消息超时
	public bool canExecuteUIF  = true;		//是否可以接收用户资料返回信息

	public	void	cnUserInformation( UserInfomation userInfo )
	{
		SurfaceUserInfo.Instance.ShowUserInfo(userInfo);
//		if (canExecuteUIF) 
//		{	
//			if(uiConfig.window_MaskLayer != null){
//				uiConfig.window_MaskLayer.SetActive(false);
//			}
//			this.msgTooLate_UIF = false;
//			this.userInfos = userInfo;
//			GameObject tempInfo = uiConfig.window_UserInfo;
//			tempInfo.SetActive(true);
//			if(uiConfig.MobileEdition)
//			{
//				tempInfo.transform.FindChild("front_panel").localPosition = Vector3.zero;
//				tempInfo.transform.FindChild("front_panel").FindChild("userInfo").FindChild("nameInput").GetComponent<UIInput>().value = userInfo.dwName;
//				tempInfo.transform.FindChild("front_panel").FindChild("userInfo").FindChild("nicknameInput").GetComponent<UIInput>().value = userInfo.dwNickname;
//				tempInfo.transform.FindChild("front_panel").FindChild("userInfo").FindChild("phoneInput").GetComponent<UIInput>().value = userInfo.dwCellPhone;
//				tempInfo.transform.FindChild("front_panel").FindChild("userInfo").FindChild("qqInput").GetComponent<UIInput>().value = userInfo.dwIM;
//				Transform tempSignInput = tempInfo.transform.FindChild("front_panel").FindChild("userInfo").FindChild("signInput");
//				if(tempSignInput!=null) tempSignInput.GetComponent<UIInput>().value = userInfo.dwSign;
//				tempInfo.transform.FindChild("front_panel").FindChild("userInfo").FindChild("face_bg").FindChild("face_img").GetComponent<UISprite>().spriteName = "face_" + userInfo.dwLogoID;
//				tempInfo.transform.FindChild("front_panel").FindChild("userInfo").FindChild("submit_btn").GetComponent<changeUserInfoBtnClick>().faceId = userInfo.dwLogoID;
//			}else{
//				tempInfo.transform.FindChild("front_panel").localPosition = Vector3.zero;
//				tempInfo.transform.FindChild("front_panel").FindChild("nameInput").gameObject.GetComponent<UIInput>().value = userInfo.dwName;
//				tempInfo.transform.FindChild("front_panel").FindChild("nicknameInput").gameObject.GetComponent<UIInput>().value = userInfo.dwNickname;
//				tempInfo.transform.FindChild("front_panel").FindChild("phoneInput").gameObject.GetComponent<UIInput>().value = userInfo.dwCellPhone;
//				tempInfo.transform.FindChild("front_panel").FindChild("qqInput").gameObject.GetComponent<UIInput>().value = userInfo.dwIM;
//				tempInfo.transform.FindChild("front_panel").FindChild("face_img").gameObject.GetComponent<UISprite>().spriteName = "face_" + userInfo.dwLogoID;
//				tempInfo.transform.FindChild("front_panel").FindChild("submit_btn").GetComponent<changeUserInfoBtnClick>().faceId = userInfo.dwLogoID;
//			}
//		}	
	}
	#endregion
	
	#region 接收 修改资料 返回消息
	public	void	cnChangeUserInformation( int wHandleCode,string msg )
	{
		Debug.LogWarning("cnChangeUserInformation-----------------" + wHandleCode);
		//wHandleCode		0:成功	1:失败
		GameObject tempInfo = uiConfig.window_UserInfo;
		GameObject tempCloseBtn = tempInfo.transform.FindChild("front_panel").FindChild("close_btn").gameObject;
		GameObject tempSubmitBtn = null;
		GameObject tempFace = null;
		if(uiConfig.MobileEdition)
		{
			tempSubmitBtn = tempInfo.transform.FindChild("front_panel").FindChild("userInfo").FindChild("submit_btn").gameObject;
			tempFace = tempInfo.transform.FindChild("front_panel").FindChild("userInfo").FindChild("face_bg").FindChild("face_img").gameObject;
		}else{
			GameObject tempChangeFaceBtn = tempInfo.transform.FindChild("front_panel").FindChild("changeFace_btn").gameObject;
			tempChangeFaceBtn.GetComponent<UIButton>().isEnabled = true;
			tempSubmitBtn = tempInfo.transform.FindChild("front_panel").FindChild("submit_btn").gameObject;
			tempFace = tempInfo.transform.FindChild("front_panel").FindChild("face_img").gameObject;
		}

		tempSubmitBtn.GetComponent<UIButton>().isEnabled = true;
		tempCloseBtn.GetComponent<UIButton>().isEnabled = true;

		if(wHandleCode == 0)
		{
			GameObject face = GameObject.Find("userFace_img");
			face.GetComponent<UISprite>().spriteName = tempFace.GetComponent<UISprite>().spriteName;
			cnTipsBox("资 料 修 改 成 功 !");
		}else{
            cnTipsBox(msg);
		}

		if (!uiConfig.isChangeFace) 
		{
			uiConfig.isChangeFace = false;
			uiConfig.window_UserInfo.SetActive(false);
		}
	}

	#endregion

	#region 接收 安全中心 登录信息 消息
	public	void	cnLogonRecord( List<LogonRecord> tempLogonRecord )
	{
		Debug.LogWarning("cnLogonRecord-----------------"+tempLogonRecord.Count);

		if(uiConfig.page_gameRecord_mask != null)
		{
			uiConfig.page_gameRecord_mask.SetActive(false);
		}

		if(true)
		{
			logonRecordList.Clear();
			logonRecordList = tempLogonRecord;
//			citySearch.StopAllCoroutines();
			resetLogonRecord(0);
		}
		if (uiConfig.title_security != null) {
			uiConfig.title_security.GetComponent<UIButton> ().isEnabled = true;
			uiConfig.title_security.transform.FindChild("wait").gameObject.SetActive(false);
		}
	}

	public	void	resetLogonRecord(int startIndex)
	{
		string tempIndex,tempTime,tempIp,stdt,tempAddress = "";
		Transform tempLogonLogs = uiConfig.window_Security.transform.FindChild("front_panel").FindChild("content").FindChild("logonLogs");
		int recordCount = logonRecordList.Count;
		int pageCount = recordCount / 8;
		if( (recordCount % 8) != 0 ) pageCount += 1;
		tempLogonLogs.FindChild("page&count_label").GetComponent<UILabel>().text = "总页数:" + pageCount + "  总记录:" + recordCount;
		int logIndex = 0;
		for(int i = startIndex; i < startIndex + 8; i++)
		{
			Transform tempLog = tempLogonLogs.FindChild("log" + logIndex.ToString());
			logIndex++;
			if(logonRecordList.Count > i)
			{
				tempIndex = (i + 1).ToString();
				DateTime dt = new DateTime(1970,1,1,0,0,0,DateTimeKind.Local).AddSeconds( logonRecordList[i].dwTmlogonTime + 28800 );
				stdt = dt.ToString("yyyy-MM-dd HH:mm:ss");
				tempTime = stdt;
				IPAddress add =  new IPAddress( logonRecordList[i].dwLogonIP);//数字变为IP地址
				tempIp = add.ToString();
				tempLog.FindChild("index_label").GetComponent<UILabel>().text = tempIndex;
				tempLog.FindChild("time_label").GetComponent<UILabel>().text = tempTime;
				tempLog.FindChild("ip_label").GetComponent<UILabel>().text = tempIp;
				tempLog.FindChild("address_label").GetComponent<UILabel>().text = tempAddress;
				citySearch.StartCoroutine("GetCityByIp", new IpCitySearch.GetCityParam{ Index = i, IpAddress = tempIp, Callback = (city, index)=>
					{
						GameObject tempLogonLogs0 = uiConfig.window_Security.transform.FindChild("front_panel").FindChild("content").FindChild("logonLogs").gameObject;
						Transform tempLog0 = tempLogonLogs0.transform.FindChild("log" + index);
						if(tempLog0 != null) tempLog0.FindChild("address_label").GetComponent<UILabel>().text = city;
					}
				});
			}else{
				tempIndex = tempTime = tempIp = tempAddress = "";
				tempLog.FindChild("index_label").GetComponent<UILabel>().text = tempIndex;
				tempLog.FindChild("time_label").GetComponent<UILabel>().text = tempTime;
				tempLog.FindChild("ip_label").GetComponent<UILabel>().text = tempIp;
				tempLog.FindChild("address_label").GetComponent<UILabel>().text = tempAddress;
			}
		}
	}
	#endregion

	#region 接收 安全中心 游戏信息 消息
	public	void cnGameRecord( List<GameRecord> tempGameRecord )
	{
		Debug.LogWarning("cnGameRecord-----------------" + tempGameRecord.Count);

		if(uiConfig.page_gameRecord_mask != null)
		{
			uiConfig.page_gameRecord_mask.SetActive(false);
		}

//		if(true)
//		{
		gameRecordList = tempGameRecord;
		int startIndex = (int)uiConfig.curRecordPageCount;
		resetGameRecord((startIndex-1)*8);

//		}
	}

	public	void  resetGameRecord(int startIndex)
	{
		Int64 tempPageScore = 0;
		string tempIndex,tempStartTime,tempEndTime,tempGame,tempScore,stdt;
		Color tempTextColor = Color.green,tempOutColor = Color.white;
		GameObject tempGameLogs = uiConfig.window_Security.transform.FindChild("front_panel").FindChild("content").FindChild("gameLogs").gameObject;
		int recordCount;
	
		try
		{
			recordCount = int.Parse(gameRecordList[0].dwAllCount.ToString());
		}
		catch{
			recordCount = 0;
		}


		int pageCount = recordCount / 8;
		if( (recordCount % 8) != 0 ) pageCount += 1;
		uiConfig.gameRecordPageCount = (uint)pageCount;
		tempGameLogs.transform.FindChild("page&count_label").GetComponent<UILabel>().text = "总页数:" + pageCount + "  总记录:" + recordCount;
		
		for(int i = 0; i < 8 ; i++)
		{
			if(gameRecordList.Count > i)
			{
				tempIndex = (i + 1 + startIndex).ToString();
				DateTime dt = new DateTime(1970,1,1,0,0,0,DateTimeKind.Local).AddSeconds( gameRecordList[i].dwEndTime + 28800 );
				stdt = dt.ToString("yyyy-MM-dd HH:mm:ss");
				tempEndTime = stdt;
				tempGame = gameRecordList[i].dwGameKind.ToString();
				tempGame = tempGame.Substring(0,1) + tempGame.Substring(2,2);
//				for(int j = 0; j < uiConfig.hallGameIds.Length; j++)
				for(int j = 0; j < CGameManager._instance.m_lstGameInfoList.Count; j++)
				{
					if(tempGame == "101"){
						tempGame = "欢乐斗地主";
						continue;
					}
//					if(tempGame == uiConfig.hallGameIds[j].ToString())
					if(tempGame == CGameManager._instance.m_lstGameInfoList[j].ID.ToString())
					{
//						tempGame = uiConfig.hallGameNames[j];
						tempGame = CGameManager._instance.m_lstGameInfoList[j].Name;;
					}
				}
				tempScore = gameRecordList[i].dwAmount.ToString();
				tempPageScore += gameRecordList[i].dwAmount;
			}else{
				tempIndex = tempStartTime = tempEndTime = tempGame = tempScore = " ";
			}
			Transform tempLog = tempGameLogs.transform.FindChild("log" + i);
			tempLog.FindChild("index_label").GetComponent<UILabel>().text = tempIndex;
			tempLog.FindChild("endTime_label").GetComponent<UILabel>().text = tempEndTime;
			tempLog.FindChild("game_label").GetComponent<UILabel>().text = tempGame;

			if(tempScore != " ")
			{
				if(Int64.Parse(tempScore) >= 0)

				{
					tempTextColor = Color.red;
					tempOutColor = Color.white;
				}else
				{
					tempTextColor = Color.green;
					tempOutColor = Color.black;
				}
			}
            UILabel uscoreTemp = tempLog.FindChild("score_label").GetComponent<UILabel>();

			uscoreTemp.effectColor = tempOutColor;
			uscoreTemp.color = tempTextColor;
			uscoreTemp.text = tempScore;
		}
       UILabel uiTmp = tempGameLogs.transform.FindChild("totalScore_label").GetComponent<UILabel>();

		if(tempPageScore >= 0)
		{
			uiTmp.color = Color.red;
			uiTmp.effectColor = Color.white;
		}else
		{
			uiTmp.color = Color.green;
			uiTmp.effectColor = Color.black;
		}
        uiTmp.text = tempPageScore.ToString();
	}

	#endregion
	
	
	#region 接收 提交反馈 返回消息
	public	void	cnSuggestionResult( int wHandleCode )
	{
		Debug.LogWarning("cnSuggestionResult-----------------:" + wHandleCode);
		//wHandleCode	0:成功
		if (uiConfig.window_Lock_mask != null) {
			uiConfig.window_Lock_mask.SetActive(false);
		}
		uiConfig.window_Feedback.GetComponent<feedbackBtnClick>().CancelFeedbackInvoke();
		uiConfig.window_Feedback.SetActive(false);

		if(wHandleCode == 0)
		{
			cnTipsBox("反 馈 成 功 ! 我 们 会 尽 快 处 理 !");
		}else{
			cnTipsBox("反 馈 失 败 ! 请 稍 候 重 试 !");
		}
	}
	#endregion

	#region 接收 账号登录 返回消息
	public	void	cnLogonError( int wHandleCode )
	{
		Debug.LogWarning("cnLogonError-----------------:" + wHandleCode);
		//wHandleCode	
		switch (wHandleCode)
		{
		case	0:
			cnMsgBox( "登 录 失 败 !" );
			break;
		case	1:
			//cnMsgBox( "登 录 成 功 !" );
			break;
		case	2:
			cnMsgBox( "用 户 名 错 误 !" );
			break;
		case	3:
			cnMsgBox( "密 码 错 误 !" );
			break;
		case	4:
			cnMsgBox( "用 户 名 被 禁 用 !" );
			break;
		case	5:
			cnMsgBox( "登 录 I P 被 禁 止 !" );
			break;
		case	6:
			cnMsgBox( "用 户 已 存 在 !" );
			break;
		case	7:
			cnMsgBox( "密 码 禁 止 校 验 !" );
			break;
		case	8:
			cnMsgBox( "不 是 指 定 地 址 !" );
			break;
		default:
			cnMsgBox( "登 录 失 败 !" );
			break;
		}
	}
	#endregion

	#region 接收 游戏登录 返回消息
	public	void	cnGameLogonError( int wHandleCode,string desc )
	{
		Debug.LogWarning("cnGameLogonError-----------------:" + wHandleCode);
		//wHandleCode	
//		if (uiConfig.MobileEdition) {
		if (HallTransfer.Instance.gameRoomList.Count != 0) {
			for(int i = 0 ; i<HallTransfer.Instance.gameRoomList.Count; i++)
			{
				HallTransfer.Instance.gameRoomList[i].transform.FindChild("wait").gameObject.SetActive(false);
			}
		}
//		}

		if (uiConfig.window_MaskHall != null) {			
			uiConfig.window_MaskHall.SetActive(false);
		}

		if( !string.IsNullOrEmpty(desc) )
		{
			cnTipsBox( desc );
		}
		else
		{
			switch (wHandleCode) 
			{
			case	0:
				cnMsgBox ("未 知 错 误 !");
				break;
			case	2:
				cnMsgBox ("用 户 不 存 在 !");
				break;
			case	3:
				cnMsgBox ("用 户 密 码 错 误 !");
				break;
			case	4:
				cnMsgBox ("用 户 账 号 禁 用 !");
				break;
			case	5:
				cnMsgBox ("登 录 IP 禁 止 !");
				break;
			case	6:
				cnMsgBox ("不 是 指 定 地 址 !");
				break;
			case	7:
				cnMsgBox ("会 员 游 戏 房 间 !");
				break;
			case	8:
				cnMsgBox ("正 在 其 他 房 间 !");
				break;
			case	9:
				cnMsgBox ("账 号 正 在 使 用 !");
				break;
			case	10:
				cnMsgBox ("人 数 已 满 !");
				break;
			case	13:
				cnMsgBox ("暂 停 登 录 服 务 !");
				break;
			case	14:
				cnMsgBox( "金 币 不 足, 不 能 进 入 !" );
				break;
			case	15:
				cnMsgBox ("比 赛 游 戏 房 间 ! !");
				break;
			case	18:
				cnMsgBox ("用 户 被 GM 禁 用 !");
				break;
			case	19:
				cnMsgBox( "非 法 登 录 房 间 !" );
				break;
			}
		}
	}
	#endregion

	#region 接收 玩家坐下 返回消息

	public	void	cnUserSitError( int HandlerCode, string Msg)
	{
		Debug.LogWarning("cnUserSitError-----------------:" + HandlerCode.ToString());

		if (uiConfig.window_MaskRoom != null) {			
			uiConfig.window_MaskRoom.SetActive(false);
		}
		if (uiConfig.curTempChair != null) {
			uiConfig.curTempChair.transform.FindChild("wait").gameObject.SetActive(false);
		}

		if( !string.IsNullOrEmpty( Msg ))
		{
			cnTipsBox( Msg );
		}
		else
		{
			///用户坐下错误码	
			switch (HandlerCode)
			{
			case	50:
				cnMsgBox( "成 功 坐 下 !" );
				break;
			case	51:
				cnMsgBox( "游 戏 已 经 开 始 !" );
				break;
			case	52:
				cnMsgBox( "已 经 有 人 存 在 !" );
				break;
			case	53:
				cnMsgBox( "密 码 错 误 !" );
				break;
			case	54:
				cnMsgBox( "IP 相 同 !" );
				break;
			case	55:
				cnMsgBox( "断 线 率 太 高 !" );
				break;
			case	56:
				cnMsgBox( "经 验 值 太 低 !" );
				break;
			case	57:
				cnMsgBox( "经 验 值 太 高 !" );
				break;
			case	58:
				cnMsgBox( "不 受 欢 迎 !" );
				break;
			case	59:
				cnMsgBox( "经 验 值 不 够 !" );
				break;
			case	60:
				cnMsgBox( "不 能 离 开 !" );
				break;
			case	61:
				cnMsgBox( "不 是 这 位 置 !" );
				break;
			case	62:
				cnMsgBox( "比 赛 结 束 !" );
				break;
			case	63:
				cnMsgBox( "金 币 太 低 !" );
				break;
			case	64:
				cnMsgBox( "比赛场排队提示" );
				break;
			case	65:
				cnMsgBox( "IP 前 3 相 同 !" );
				break;
			case	66:
				cnMsgBox( "IP 前4 相 同" );
				break;
			case	67:
				cnMsgBox( "不 允 许 旁 观" );
				break;
			case	68:
				cnMsgBox( "百家乐桌子座位满了,无法分配座位给玩家" );
				break;
			}
		}
		
	}

	#endregion


	#region 接收 改变窗口大小 消息
	public	void	cnResizeWindow ()
	{
		Debug.LogWarning("cnResizeWindow-----------------:");
		resetGamesScrollView();//重置游戏列表scrollview
		if(uiConfig.page_roomDesk.activeSelf) deskReposition();//重置桌子列表scrollview
		if(uiConfig.page_gameRoom.activeSelf) resetRoomScrollView();//重置房间列表scrollview
	}
	#endregion

	#region 接收 保险柜请求 返回消息
	public	void	cnSafetyBoxAnswer( int wHandleCode )
	{
		Debug.LogWarning("cnSafetyBoxAnswer-----------------:" + wHandleCode.ToString());
		//wHandleCode	
		switch (wHandleCode)
		{
		case	0:
			cnTipsBox( "游 戏 正 在 进 行 中 !" );
			break;
		case	1:
			uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("pass_Input").GetComponent<UIInput>().value ="";
			uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("pass_Input").FindChild("Label").GetComponent<UILabel>().text="";
			uiConfig.window_SafeBoxEntry.transform.FindChild("front_panel").FindChild("pass_Input").FindChild("tempLabel").gameObject.SetActive(true);
			uiConfig.window_SafeBoxEntry.SetActive(true);
			break;
		}
	}
	#endregion

	#region 接收 充值界面信息

	public	void cnRechargeMsg( Int64 lowestMoney )
	{
		Debug.LogWarning ("RechargeMsg~~~~:" + lowestMoney);
		if(!uiConfig.MobileEdition)
		{
			Transform tr = uiConfig.page_recharge.transform.FindChild ("front_panel").FindChild ("content").FindChild ("recharge").FindChild ("content").FindChild ("Label_money");
			tr.gameObject.GetComponent<UILabel> ().text = lowestMoney.ToString();
		}

	}

	
	///提现界面消息返回
	public	void cnAwardMsg( AwardMsg msg)
	{
		Debug.LogWarning ("AwardMsg~~~~:" + msg.dwLowestMoney);
		uiConfig.page_recharge.transform.FindChild ("front_panel").FindChild ("content").FindChild ("award").FindChild ("content").FindChild ("Label_money")
			.gameObject.GetComponent<UILabel> ().text = msg.dwLowestMoney.ToString();
		uiConfig.page_recharge.transform.FindChild ("front_panel").FindChild ("content").FindChild ("award").FindChild ("content").FindChild ("Label_safebox")
			.gameObject.GetComponent<UILabel> ().text = msg.dwSafeMoney.ToString();
		uiConfig.page_recharge.transform.FindChild ("front_panel").FindChild ("content").FindChild ("award").FindChild ("content").FindChild ("Label_order")
			.gameObject.GetComponent<UILabel> ().text = msg.dwAwardMoney.ToString();
	}


	#endregion

	#region 接收 充值记录接收

	public bool  isRecharge = false;			//是否充值消息
	public bool  msgTooLate_RcLogs = false; 	//充值消息接收超时
	public bool  canReceiveRecord_Rc = true;	//可接收记录信息

	public	void cnRechargeRecond( int code, List<RechargeRecord> recondlist )
	{
		Debug.LogWarning ("RechargeRecond~~~~~~~~~~~" + recondlist.Count);
		
		if(canReceiveRecord_Rc)
		{
			if (uiConfig.page_recharge_mask != null) 
			{
				uiConfig.page_recharge_mask.SetActive(false);
			}
			msgTooLate_RcLogs = false;
			rechargeLogonList = recondlist;
			int startIndex = (int)uiConfig.curRechargeRecordPage;
			resetRechargeRecord((startIndex-1)*10);		
		}
	}

	public	void resetRechargeRecord( int index )
	{
		string tempIndex,tempTime,tempMoney,tempPage,tempRemark,tempState,tempNum,stdt,tempOrderId;
		int recordCount;

		GameObject tempLogs = uiConfig.page_recharge.transform.FindChild("front_panel").FindChild("content").FindChild("rechargeRecords").gameObject;
		tempPage = uiConfig.curRechargeRecordPage.ToString();

		try
		{
			recordCount = int.Parse(rechargeLogonList[0].dwAllCount.ToString());
		}
		catch{
			recordCount = 0;
		}
				
		int pageCount = recordCount / 10;
		if( (recordCount % 10) != 0 ) pageCount += 1;
		uiConfig.curRechargeRPageCount = (uint)pageCount;
		tempLogs.transform.FindChild("page_label").GetComponent<UILabel>().text = "第"+tempPage+"页" + " 共"+pageCount+"页";
		tempLogs.transform.FindChild("count_label").GetComponent<UILabel>().text = "共"+recordCount+"条";
		HallTransfer.Instance.uiConfig.curRechargeRPageCount = (uint)pageCount;

		for (int i = 0; i < 10; i++) 
		{
			Transform tempLog = tempLogs.transform.FindChild ("log" + i);
			if(!uiConfig.MobileEdition)
			{
				tempLog.FindChild("cancelBtn").gameObject.SetActive(false);
			}

			if (rechargeLogonList.Count > i) {
				tempIndex = (i + index + 1).ToString();

				tempNum = rechargeLogonList [i].dwApplyNumber.ToString();
				//tempMoney = rechargeLogonList [i].dwMoney.ToString();
				tempMoney = rechargeLogonList [i].dwMoney.ToString("N0");//transMoney(rechargeLogonList [i].dwMoney);
				tempRemark = rechargeLogonList [i].dwRemark.ToString();
				tempOrderId = rechargeLogonList [i].dwOrderId.ToString();

				if(tempRemark.Length>8)
				{
					string s = tempRemark.Substring(0,6);
					tempRemark = s + "...";
				}
				int state =  rechargeLogonList [i].dwState;
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

				tempLog.FindChild("cancelBtn").gameObject.SetActive(true);
				tempLog.FindChild("cancelBtn").gameObject.GetComponent<cancelBtnTag>().tags = tempOrderId;
				//if(!(tempState == "待处理"))
				if( state != 0 )
				{
					tempLog.FindChild("cancelBtn").gameObject.SetActive(false);
				}

			} else {
				tempIndex = tempTime = tempMoney = tempRemark = tempState= tempNum = "";
			}


			tempLog.FindChild ("index_label").GetComponent<UILabel> ().text = tempIndex;
			tempLog.FindChild ("applyNum_lable").GetComponent<UILabel>().text = tempNum;
		//	tempLog.FindChild ("time_label").GetComponent<UILabel> ().text = tempTime;
			tempLog.FindChild ("money_label").GetComponent<UILabel> ().text = tempMoney;
			tempLog.FindChild ("remark_label").GetComponent<UILabel> ().text = tempRemark;
			tempLog.FindChild ("state_label").GetComponent<UILabel> ().text = tempState;
		}	
	}

	///兑奖记录

	public bool  msgTooLate_AwLogs = false; 	//提现消息接收超时
	public bool  canReceiveRecord_Aw = true;	//可接收提现记录信息

	public	void cnAwardRecond(int code,  List<RechargeRecord> recondlist )
	{
		Debug.LogWarning ("AwardRecond~~~~~~~~~~~" + recondlist.Count);

		if (canReceiveRecord_Aw) 
		{
			if (uiConfig.page_recharge_mask != null) 
			{
				uiConfig.page_recharge_mask.SetActive(false);
			}
			msgTooLate_AwLogs = false;
			rechargeLogonList = recondlist;
			int startIndex = (int)uiConfig.curRechargeRecordPage;
			resetAwardRecord((startIndex-1)*10);	
		}			
	}

	public	void resetAwardRecord( int index )
	{
		string tempIndex,tempTime,tempMoney,tempPage,tempRemark,tempState,tempNum,stdt,tempOrderId;
		int recordCount;

		GameObject tempLogs = uiConfig.page_recharge.transform.FindChild("front_panel").FindChild("content").FindChild("awardRecords").gameObject;
		tempPage = uiConfig.curRechargeRecordPage.ToString();
		
		try
		{
			recordCount = int.Parse(rechargeLogonList[0].dwAllCount.ToString());
		}
		catch{
			recordCount = 0;
		}
		
		int pageCount = recordCount / 10;
		if( (recordCount % 10) != 0 ) pageCount += 1;
		uiConfig.curRechargeRPageCount = (uint)pageCount;
		tempLogs.transform.FindChild("page_label").GetComponent<UILabel>().text = "第"+tempPage+"页" + " 共"+pageCount+"页";
		tempLogs.transform.FindChild("count_label").GetComponent<UILabel>().text = "共"+recordCount+"条";
		HallTransfer.Instance.uiConfig.curRechargeRPageCount = (uint)pageCount;
		for (int i = 0; i < 10; i++) 
		{
			Transform tempLog = tempLogs.transform.FindChild ("log" + i);

			if(!uiConfig.MobileEdition)
			{
				tempLog.FindChild("cancelBtn").gameObject.SetActive(false);
			}

			if (rechargeLogonList.Count > i) {
				tempIndex = (i + index + 1).ToString();

				tempNum = rechargeLogonList [i].dwApplyNumber;
				//tempMoney = rechargeLogonList [i].dwMoney.ToString();
				tempMoney = rechargeLogonList [i].dwMoney.ToString("N0");//transMoney( rechargeLogonList [i].dwMoney );
				tempRemark = rechargeLogonList [i].dwRemark.ToString();
				tempOrderId = rechargeLogonList [i].dwOrderId.ToString();

				if(tempRemark.Length>8)
				{
					string s = tempRemark.Substring(0,6);
					tempRemark = s + "...";
				}
				int state =  rechargeLogonList [i].dwState;
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
				
				tempLog.FindChild("cancelBtn").gameObject.SetActive(true);
				tempLog.FindChild("cancelBtn").gameObject.GetComponent<cancelBtnTag>().tags = tempOrderId;

				if( state != 0 )
				{
					tempLog.FindChild("cancelBtn").gameObject.SetActive(false);
				}

			} else {
				tempIndex = tempTime = tempMoney = tempRemark = tempState = tempNum = "";
			}
			

			tempLog.FindChild ("index_label").GetComponent<UILabel> ().text = tempIndex;
			tempLog.FindChild ("applyNum_lable").GetComponent<UILabel>().text = tempNum;
			//	tempLog.FindChild ("time_label").GetComponent<UILabel> ().text = tempTime;
			tempLog.FindChild ("money_label").GetComponent<UILabel> ().text = tempMoney;
			tempLog.FindChild ("remark_label").GetComponent<UILabel> ().text = tempRemark;
			tempLog.FindChild ("state_label").GetComponent<UILabel> ().text = tempState;
		}	
	}
	
	#endregion

	#region 接收 充值结果返回
	public	void cnRechargeResult( int wHandleCode , string msg)
	{
		if (uiConfig.page_recharge_mask != null) {
			uiConfig.page_recharge_mask.SetActive(false);
		}

		if (msg != "") {
			cnMsgBox( msg );
		}	
	}

	///提现结果返回
	public	void cnAwardResult( int wHandleCode, string msg)
	{
		if (uiConfig.page_recharge_mask != null) {
			uiConfig.page_recharge_mask.SetActive(false);
		}

		if (msg != "") {
			cnMsgBox( msg );
		}

		if(ncAwardEventClick != null){
			ncAwardEventClick();
		}
	}

	private void clean_rechargeLabel()
	{
		GameObject tempLogs = uiConfig.page_recharge.transform.FindChild("front_panel").FindChild("content").FindChild("recharge").FindChild("content").gameObject;
		tempLogs.transform.FindChild ("Label_log").GetComponent<UILabel> ().text = "";
	}

	private void clean_awardLabel()
	{
		GameObject tempLogs = uiConfig.page_recharge.transform.FindChild("front_panel").FindChild("content").FindChild ("award").FindChild("content").gameObject;
		tempLogs.transform.FindChild ("Label_log").GetComponent<UILabel> ().text = "";
	}

	#endregion

	#region 接受 取消订单结果

	public	void cnCancelOrderResult( int wHandleCode )	
	{
		Debug.LogWarning ("CancelOrder~~~:"+wHandleCode);

		switch(wHandleCode)
		{
		case 0:
			cnMsgBox( "取消订单成功" );

			if(HallTransfer.Instance.uiConfig.window_CancelOrderBox.activeSelf)
			{
				//关闭取消订单窗口
				HallTransfer.Instance.uiConfig.window_CancelOrderBox.SetActive(false);
			}

//			if (HallTransfer.Instance.ncRechargeRecord != null) 
			{
				RecordRequest record = new RecordRequest();
				record.dwPage = uiConfig.curRechargeRecordPage;
				record.dwPageSize = 10;
				record.dwTime = DateTime.Now;

				if(isRecharge)
				{
					HallTransfer.Instance.ncRechargeRecord(record);
				}else
				{
					HallTransfer.Instance.ncAwardRecord(record);
				}
			}
			break;
		case 1:
			cnMsgBox( "取消订单失败" );

			if(HallTransfer.Instance.uiConfig.window_CancelOrderBox.activeSelf)
			{
				//关闭取消订单窗口
				HallTransfer.Instance.uiConfig.window_CancelOrderBox.SetActive(false);
			}
			break;
		}
	}

	#endregion

	#region 接收 公告消息

	public	void cnNoticeMsg( int wHandleCode, string msg)
	{
		if (msg == "")
			return;

		if (!uiConfig.MobileEdition)
		{
			GameObject notice = uiConfig.Notice.transform.FindChild ("noticeBg").FindChild ("content_label").gameObject;
			//notice.GetComponent<UILabel> ().text = "";
			string tempColor = notice.GetComponent<UILabel>().effectColor.ToHexStringRGBA();
			string number = "0123456789";
			string tempMsg = "";

			StringBuilder sb = new StringBuilder ();

			for (int i = 0; i < msg.Length; i++) 
			{
				if (!number.Contains ("" + msg [i])) 
				{
					tempMsg = "[" + tempColor + "]" + msg [i] + "[-]";
//					tempMsg = "[035283ff]" + msg [i] + "[-]";
					sb.Append (tempMsg);
				} else {
					tempMsg = "[ff0000ff]" + msg [i] + "[-]";
					sb.Append (tempMsg);
				}					
			}
			
			notice.GetComponent<UILabel> ().text = sb.ToString ();

		}
	}
	
	#endregion

	#region 接收 跑马灯 消息
	public	void	cnMarqueeLight ( string val )
	{
		Debug.LogWarning("cnMarqueeLight-----------------:" + val);
		//uiConfig.MarqueeLight.GetComponent<marqueeEffect>().AddItem(val);
		if(	uiConfig.MarqueeLight != null )
		{
//			if(!notice.GetComponent<UITweener>().enabled)
//			{
//				notice.GetComponent<UITweener>().enabled = true;
//			}
			uiConfig.MarqueeLight.SetActive (true);
			GameObject notice = uiConfig.MarqueeLight.transform.FindChild ("noticeBg").FindChild ("content").gameObject;
			notice.GetComponent<UILabel> ().text = val;

			Invoke("hideMarquee",6.0f);
		}
	}

	void hideMarquee()
	{
		uiConfig.MarqueeLight.SetActive (false);
	}
	#endregion

	#region 确认 退出房间
	
	public	void cnComfirmQuitRoom( )
	{
		if (!uiConfig.MobileEdition) {
			uiConfig.page_roomDesk.transform.FindChild ("topBar").FindChild ("return_btn").GetComponent<quitRoomDeskBtnClick> ().quickRoom ();	
		} else {
			uiConfig.page_roomDesk.transform.FindChild ("front_panel").FindChild ("close_btn").GetComponent<quitRoomDeskBtnClick>().quickRoom ();	
		}
	}
	
	#endregion


	#region 接收 进入房间成功 消息
	public	void	cnEnterRoom ( uint roomId )
	{
		Debug.LogWarning("cnEnterRoom-----------------:" + roomId);
		uiConfig.lastGameRoomId = roomId;
	}
	#endregion

	#region 打开游戏规则外链
	public void OpenWebIntroduction()
	{
		string tempUrl = GameApp.GameData.OfficeSiteUrl + "/Games/IntroductionList.aspx";
		Application.OpenURL(tempUrl);
	}
	#endregion

}
