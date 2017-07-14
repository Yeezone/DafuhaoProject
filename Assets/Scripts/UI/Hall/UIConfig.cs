using UnityEngine;
using System;

public class UIConfig : MonoBehaviour {

	
	//public List<HallTransfer.roomUserList> roomUserList = new List<HallTransfer.roomUserList>();	//房间玩家信息容器
	public enum LobbyStyle	//游戏大厅版本
	{
		NormalBlueStyle,			//普通蓝色风格
		mNormalBlueStyle,			//普通蓝色风格_移动
		GambleStyle,				//赌博风格
		mGambleStyle,				//赌博风格_移动
		FeiFanStyle,				//非凡风格
		mFeiFanStyle,				//非凡风格_移动
		max 
	}
	public enum CurGameType
	{
		CardGame,			//扑克游戏
		FishGame,			//捕鱼游戏
		Ext8Game,			//8人分机押分游戏
		Main8Game,			//8人一机押分游戏
		DtGame,				//8人单挑游戏
		MjGame,				//4人麻将游戏
		max
	}

	public  LobbyStyle  		curLobbyStyle; 		//当前游戏大厅版本


	public	bool				MobileEdition;			//是否为移动版
	public	bool				LockOrUnLockAccount;	//账号绑定开关
	public	bool				BackToGame;				//返回游戏消息
	public  string              OrderId;
	
	public	LobbyAnimCtrl		AnimCtrl;				//动画控制器

	public	GameObject			WebNotice;				//网页公告
	public	GameObject			WebRanking;				//网页排行榜
	public	GameObject			WebRecharge;			//网页充值
	public	GameObject			WebShare;				//网页分享
	public	GameObject			WebLuckDraw;			//网页抽奖
	
	public	UIGrid				MainBtn_Grid;			//主要按钮窗器
	public	GameObject			MainBtn_Record;			//记录按钮
	public	GameObject			MainBtn_Notice;			//公告按钮
	public	GameObject			MainBtn_Recharge;		//充值按钮
	public	GameObject			MainBtn_LuckDraw;		//抽奖按钮
	public	GameObject			MainBtn_Ranking;		//排行榜按钮


	public	GameObject			Notice;					//公告栏
    public	GameObject			NoticeMarquee;			//横版公告栏
	public	GameObject			msgBox;					//测试文本框

    public	GameObject			title_security;			//安全中心按钮
	public	GameObject			page_gameRoom;			//游戏房间信息分页
	public	GameObject			page_scoreList;			//积分排名信息分页
	public	GameObject			page_roomDesk;			//房间座位信息分页
	public	GameObject			page_recharge;			//充值分页
	public	GameObject			page_gameRule;			//游戏规则分页

	public	GameObject			window_FirstPage;		//游戏首页
	public	GameObject			window_Swith;			//切换窗口
	public	GameObject			window_UserInfo;		//玩家信息窗口
	public	GameObject			window_SafeBox;			//保险柜窗口
	public	GameObject			window_SafeBoxEntry;	//保险柜入口窗口
	public	GameObject			window_LockOrUnLock;	//绑定窗口
	public	GameObject			window_Security;		//安全中心窗口
	public	GameObject			window_Feedback;		//意见反馈窗口
	public	GameObject			window_ReloginMsgBox;	//登录返回消息窗口
	public	GameObject			window_MsgBox;			//普通弹出消息窗口
	public	GameObject			window_CancelOrderBox;	//取消订单窗口
	public	GameObject			window_TipsBox;			//半透明消息窗口

	public	GameObject			window_MaskHall;		//等待动画屏蔽层(大厅界面)
	public	GameObject			window_MaskRoom;		//等待动画屏蔽层(房间界面)
	public	GameObject			window_MaskLayer;		//等待动画屏蔽层(附动画)
	public	GameObject			window_SafeBox_mask;	//保险柜窗口屏蔽层
	public	GameObject			window_Lock_mask;		//绑定窗口屏蔽层
	public	GameObject			window_Feedback_mask;	//绑定窗口屏蔽层
	public	GameObject			page_recharge_mask;	    //充值中心窗口屏蔽层
	public	GameObject			page_gameRecord_mask;	//安全中心窗口屏蔽层

	public	uint				gameRecordPageNum;		//每页拥有游戏记录个数
	public	uint				gameRecordPageCount;	//游戏记录总页数
	public	uint				curRecordPageCount;		//当前游戏记录页数
	public	string				curRecordGameType;			//当前游戏类型
	public	uint				curRechargeRecordPage;	//当前充值记录页数
	public	uint				curRechargeRPageCount;	//充值记录总页数

    public  Int64               bankMoney;				//保险柜金币数量
	public	GameObject			MarqueeLight;			//跑马灯

	public	GameObject			btn_gameHall;			//游戏大厅按钮
	public	GameObject			btn_scoreList;			//积分排行按钮
	public	Transform			mobileFishDesk;			//移动端捕鱼座位

	public	GameObject[]		gameRoomDesk;			//游戏桌子预设
	public	GameObject[]		gambleRoomDesk;			//赌博风游戏桌子预设

	public	GameObject			btn_gameBtn;			//游戏按钮预设
	public	int					gameBtn_interval;		//游戏按钮之间的上下间隔距离
	public	GameObject			btn_gameRoom;			//游戏房间按钮预设
	public	uint				curGameId;				//当前游戏ID
	public	CurGameType			curGameType;			//当前游戏种类
	public	uint				curGameKind;			//当前游戏类型
	public	string				curGameName;			//当前游戏名称
	public	string				curGameRateName;		//当前游戏倍率或底分名称
	public	uint				curRoomId;				//当前游戏房间ID
	public	string				curRoomName;			//当前游戏房间名称
	public	uint				curDeskNo;				//当前桌子号
	public	uint				curStation;				//当前座位号
	public	uint				lastGameRoomId;			//上一次进入的游戏房间ID

	public  Int32   			fish_ratio;     		//投币比例
	public	GameObject		    difen_lable;			//显示房间底分的label

	public  bool                isChangeFace;			//是否改头像
	public	bool				quickEnterDesk;			//是否点击“快速游戏”进入选桌界面
	public	int					deskColumnCount;		//房间桌子列数
	public	int					deskRowCount;			//房间桌子行数
	public	uint				deskCount;				//房间桌子数
	public	uint				chairCount;				//房间椅子数

	public	GameObject			phone_deskBtn;			//移动端桌子按钮预设
	public	GameObject			curTempChair;			//当前尝试坐下的椅子

	public	UInt32[]			hallGameIds;			//游戏ID
	public	uint[]				hallGameTypeId;			//游戏Type
	public	string[]			hallGameNames;			//游戏名称
	public	uint[]				hallGameRoomIds;		//游戏房间ID
	public	string[]			hallGameRoomNames;		//游戏房间名称






}
