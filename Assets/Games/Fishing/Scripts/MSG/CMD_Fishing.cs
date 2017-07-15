using System;
using System.Runtime.InteropServices;

namespace com.QH.QPGame.Fishing
{
	public class CMD_Fishing
	{
		public const string ActorIdConfigFilePath="data.actorId";
		public const string NavigationConfigFilePath="game.navigPak";

		public const string GAME_NAME="猎鱼高手OnLine";

		public const int GAME_ID=41500000;

		public const int GAME_PLAYER_COUNT=6;

        public const int NAME_STRING_LEN = 61;
        public const int MAX_COUNTRY_LEN = 32;

		public const int MDM_GF_HEART=7;                            ///心跳
		public const int SUB_GF_HEART=1;

//		public const int MDM_GF_ROOM=8;
//		public const int SUB_GF_BASE_INFO=0;
//		public const int SUB_GF_DATA_OVER=1;
//		public const int SUB_GF_RESET=2;

		public const int MDM_GF_FRAME=10;                           ///用户信息消息
		public const int SUB_GF_USER_INFO=0;                        ///客户端发送来的用户信息()
		public const int SUB_GF_USER_AGREE=1;                       ///用户同意
		public const int SUB_GF_USER_STANDUP=3;                     ///用户离开
		public const int SUB_GF_USER_INFO_UPDATE=7;                 ///用户信息更新
		public const int SUB_GF_GAME_TRUSTEE=10;                    ///游戏的托管


//		public const int MDM_GF_QUEUE=13;
//		public const int SUB_GF_QUEUE_JION=1;

		public const int MDM_GF_BANK=17;                            ///游戏中的银行功能
		public const int SUB_GF_DRAW_MONEY=2;                       ///取钱
		public const int SUB_GF_DRAW_MONEY_ERROR=5;                 ///取款失败
		public const int SUB_GF_DRAW_MONEY_ERROR_PASSWORD=6;        ///二级密码错误
		public const int SUB_GF_ACCOUNT_LOCK=7;                     ///账号被锁
		public const int SUB_GF_BANK_EMPTY=8;                       ///银行中没有乐豆

		public const int MDM_GF_GAME_PROP=140;                      ///大厅道具
		public const int SUB_GF_GAME_PROP_LIST=11;                  ///道具列表

//		public const int MDM_GF_PLANTFORM=150;                      ///平台消息
//		public const int SUB_GF_NORMAL_TAKE=0;

		public const int MDM_GF_ROOM_PROP=160;                      ///房间和游戏中道具相关的消息
		public const int SUB_GF_ROOM_PROP_BUY=3;

		public const int MDM_GF_USER_STANDUP=161;                   ///玩家站起

		public const int MDM_GF_MAQ_MSG=0x01000004;                 ///跑马灯消息
		public const int SUB_GF_MAQ_MSG=1;                          

		public const int MDM_GF_INFO=200;							///游戏信息
        public const int MDM_GF_GAME_FRAME = 100;						///框架消息
                                                                        ///
        public const int SUB_GF_GAME_OPTION = 1;						///游戏配置

		public const int SUB_CS_GF_USER_READY=4150;					///游戏玩家准备开始游戏C->S
		public const int SUB_CS_GF_USER_BUY_BULLET=4151;			///游戏玩家购买子弹C->S
		public const int SUB_CS_GF_USER_GUN_POWER_UP=4152;			///游戏玩家武器加炮C->S
		public const int SUB_CS_GF_USER_GUN_FIRE=4153;				///游戏玩家开炮C->S
//		public const int SUB_CS_GF_USER_GUN_POWER_MULTI=4154;		///游戏玩家准武器翻倍C->S
		public const int SUB_CS_GF_BULLET_ATTACK=4155;				///游戏子弹击中鱼C->S
		public const int SUB_CS_GF_WIN_FROM_CACHE=4156;				///请求服务器累加缓存C->S
		public const int SUB_CS_GF_BALANCE=4157;				   	///玩家准备要离开了，通知结算缓存数据C->S
//		public const int SUB_CS_GF_TASK=4158;						///申请任务C->S
//		public const int SUB_CS_GF_TASK_COMPLETE=4159;				///申请任务完成C->S

		public const int SUB_CS_GF_PROP_USER=4160;					///道具使用C->S
		public const int SUB_CS_GF_TIME_SYNC=4161;					///时间同步C->S
		public const int SUB_CS_GF_ROBOT_GUN_FIRE=4162;				///机器人开炮C->S
		public const int SUB_CS_GF_ROBOT_BULLET_ATTACK=4163;		///机器人子弹击中鱼C->S

		public const int SUB_SC_GF_GAME_STATION=4164;				///游戏状态S->C
//		public const int SUB_SC_GF_TALK_NORMAL=4165;				///聊天S->C
		public const int SUB_SC_GF_TIME_SYNC=4166;					///同步时间S->C
		public const int SUB_SC_GF_MAP_CHANGE=4167;					///切换地图S->C
		public const int SUB_SC_GF_USER_JOIN=4168;					///玩家加入游戏S->C
		public const int SUB_SC_GF_USER_LEAVE=4169;					///玩家离开游戏S->C

		public const int SUB_SC_GF_USER_PROP_GET=4170;				///玩家获得道具S->C
		public const int SUB_SC_GF_BUY_BULLET=4171;				    ///购买子弹S->C
		public const int SUB_SC_GF_GUN_POWER_UP=4172;				///武器加炮S->C
		public const int SUB_SC_GF_GUN_FIRE=4173;				    ///武器开炮S->C
//		public const int SUB_SC_GF_GUN_POWER_MULTI=4174;			///武器翻倍S->C
		public const int SUB_SC_GF_NPC_KILL=4175;				    ///成功击杀NPC S->C
		public const int SUB_SC_GF_WIN_FROM_CACHE=4176;				///服务器缓存奖励S->C
		public const int SUB_SC_GF_BALANCE=4177;				    ///结算S->C
		public const int SUB_SC_GF_NPC_GENERATOR=4178;				///生成NPC   S->C
//		public const int SUB_SC_GF_CAIJIN_GET=4179;					///彩金中奖S->C

//		public const int SUB_SC_GF_CAIJIN_UPDATE=4180;				///彩金更新S->C
		public const int SUB_SC_GF_TASK=4181;				        ///任务S->C
		public const int SUB_SC_GF_TASK_COMPLETE=4182;				///任务完成S->C
		public const int SUB_SC_GF_PROP_USE=4183;				    ///道具使用S->C
//		public const int SUB_SC_GF_HAPPY_TIME=4184;				    ///开心时间S->C
		public const int SUB_SC_GF_UPDATE_USER_GOLD=4185;			///玩家金币更新S->C
		public const int SUB_SC_GF_ROBOT_NOTIFY_GUN_FIRE=4186;		///机器人 服务器通知填充开炮数据S->C
		public const int SUB_SC_GF_ROBOT_GUN_FIRE=4187;				///机器人 服务器群发开炮S->C

	}

	public enum BUY_PROP_MONEY_TYPE 
	{
		MONEY_TYPE_VIR=0,                                           ///0虚拟货币（购买道具得到的）
		MONEY_TYPE_MONEY,                                           ///1元宝（充值得到的）
		MONEY_TYPE_EXCHANGE_BILL,                                   ///奖券
		MONEY_TYPE_MAX
	}

    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CMD_GF_GameOption
    {
        public byte cbAllowLookon;						//旁观标志
        public UInt32 dwFrameVersion;						//框架版本
        public UInt32 dwClientVersion;					//游戏版本
    }

    [System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_GAME_STATION
	{
		public  UInt32              gsJiTaiLeiXing;                 ///机台类型
		public  UInt32              gsToUBiRate;	                ///投币比例
		public  UInt32              gsGameRate;	                    ///游戏倍率（房间倍率）
		public  UInt32              gsPowerMin;                     ///最小炮数
		public  UInt32              gsPowerMax;		                ///最大炮数 
		public  UInt32              gsPowerStep;		            ///加炮幅度
		public  UInt32              gsPowerMultiMax;		        ///最大炮数倍率
		public  UInt32              gsPeak;			                ///爆机分
		public  UInt32		        gsServerTime;				    ///服务器时间
		public  UInt32              gsMapID;                        ///地图ID（配置文件中地图索引）
		public  float               gsMapTime;                      ///地图进行到的时间
		public  UInt32              gsSelfDesk;                     ///自己的桌子号
		public  UInt32              gsSelfChair;					///自己的椅子号
		public  UInt32              gsSelfUserId;					///自己的USERID
		public  UInt32              gsSelfGold;						///自己的金币
		public  UInt32              gsSelfVoucher;					///自己的代金券
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_USER_JOIN
	{
		public  UInt32              gsChair;                        ///玩家座位号
		public  UInt32              gsUserId;	                    ///玩家ID
		public  UInt32              gsScore;	                    ///分数
		public  UInt32              gsLevel;                        ///等级
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst =64)]
		public  string    			dwName;	                        ///用户名
		public  UInt32              gsPropIdShip;		            ///船ID
		public  UInt32              gsPropIdBullet;		            ///子弹ID
		public  UInt32              gsPower;			            ///炮值
		public  UInt32		        gsPowerMulti;				    ///炮倍数

	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_BUY_BULLET
	{
		public  UInt32              gsScore;                        ///玩家要购买的分数
		
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_GUN_POWER_UP
	{
		public  UInt32              gsUpScore;                      ///子弹增加的分数
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_USER_FIRE
	{
		public  float              	gsRot;                          ///玩家旋转值
		public  float              	gsZ;	                        ///坐标
		public  float              	gsY;	                        ///坐标
		public  UInt32              gsServerId;                     ///锁定NPC的ServerId -1为没锁定
		public  UInt32              gsCostVal;		                ///花费
		public  UInt32              gsServerTime;		            ///时间
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_BULLET_ATTACK
	{
		public  UInt32              gsServerId;                     ///NPC  ID
		public  UInt32              gsCost;	                    	///子弹花费
		public  UInt32              gsPower;	                    ///子弹炮值
		public  UInt32              gsRate;		                    ///倍率
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_GET_SCORE_CACHE
	{
		public  UInt32              gsPower;                        ///炮值
		public  UInt32              gsRate;	                    	///倍率
		public  UInt32              gsNPCId;	                    ///NPC ID

	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_PROP_USER
	{
		public  UInt32              gsPropId;                       ///道具ID
		public  UInt32              gsChairTarget;	                ///目标座位号 -1没有目标
		
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_PROP_SHOW
	{
		public  UInt32              gsPropId;                       ///道具ID
		public  UInt32              gsChairTarget;	                ///目标座位号 -1没有目标
		
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_PROP_BUY
	{
		public  UInt64              gsUserId;                       ///购买者ID
		public  UInt32              gsPropId;	                    ///购买道具ID
		public  UInt64              gsProPayMoney;	                ///总共的元宝
		public  UInt32              gsPropBuyCount;                 ///道具数量
		public  UInt32              gsPresentGold;	                ///赠送金币
		public  UInt32              gsIPC;	                   
		
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_BANK_OPERATE
	{
		public  UInt32              gsType;                       	///类型（1表存钱，2表取钱，0表传错了）
		public  UInt32              gsMoney;	                    ///钱数（不接受0和负数）
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst =32)]
		public  string    			dwPass;	                     	///密码（取钱时用）
		public  UInt32              gsPassBit;                 		///密码位数
		public  UInt32              gsLeftMoney;	                ///取完后或存完后的钱数                
	}



	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_ROBOT_GUN_FIRE
	{
		public  UInt32              gsRoboChair;                    ///机器人座位号
		public  float               gsRot;	                        ///玩家旋转值
		public  float               gsZ;	                        ///坐标
		public  float               gsY;                            ///坐标
		public  UInt32              gsSsrverId;	                    ///锁定NPC的ServerId -1为没锁定
		public  UInt32              gsCostVal;                      ///花费
		public  UInt32              gsServerTime;                   ///时间
		
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_CS_GF_ROBOT_BULLET_ATTACK
	{
		public  UInt32              gsRobotChair;                   ///机器人座位号
		public  UInt32              gsNpcID;	                    ///NPC  ID
		public  UInt32              gsCost;	                        ///子弹花费
		public  UInt32              gsPower;                        ///子弹炮值 
		public  UInt32              gsRate;		                    ///倍率
		
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet=CharSet.Unicode)]
	public struct CMD_SC_GF_USER_INFO
	{
		public  UInt32			    gsUserID;						///ID号码     
		public  UInt32    		    gsImageNO;						//<个人形象图片文件名      
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 61)]
		public  string			    gsNickName;						//<昵称  
		public  UInt32			    gsAge;							//<年龄	
		public  Byte			    gsSex;							//<0 表示女，1 表示男
		public  UInt64				gsBirthday;						//<生日
		public  UInt32				gsStarTag;						//<星座
		public  UInt32				gsBornTag;						//<生肖		
		public  UInt32				gsBloodTag;						//<血型	
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CMD_Fishing.MAX_COUNTRY_LEN)]
		public  string          	gsCountry;                      //<国家
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CMD_Fishing.NAME_STRING_LEN)]
		public  string          	gsProvince;						//<玩家所在的省    
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CMD_Fishing.NAME_STRING_LEN)]
		public  string          	gsCity;							//<玩家所在的市
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CMD_Fishing.NAME_STRING_LEN)]
		public  string          	gsGameSign;						//<备注 
		public  UInt32				gsLevel;						//<用户等级
		public  UInt32				gsDeskNO;						//<游戏桌号   
		public  UInt32				gsChair;						//<桌子位置
		public  UInt32				gsRoomID;                       //<桌子ID
		public  UInt32				gsUserState;					//<用户状态 
		public  UInt32				gsMoney;						//用户金币								
		public  UInt32				gsBank;							//用户财富								
		public  UInt32     			gsTreasure;						//元宝                            	
		public  UInt32           	gsExchangeBill;                 //兑换券
		public  UInt32				gsFlag;							//<0表示自己， 1表示其它玩家， 2表示旁观	
		public  UInt32				gsWinCount;						//胜利数目
		public  UInt32				gsLostCount;					//输数目
		public  UInt32				gsDrawCount;					//和局数目
		public  UInt32				gsPoint;						//积分

	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_USER_LEAVE
	{
		public  UInt32              gsChair;                        ///玩家座位号
		
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_USER_GET_PROP
	{
		public  UInt32              gsChair;                       	///玩家座位号
		public  UInt32              gsPropId;	                    ///道具ID
		public  UInt32              gsPropCount;	                ///道具数量               
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_TIME_SYNC
	{
		public  UInt32              gsServerTime;                   ///服务器时间
		
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_MAP_CHANGE
	{
		public  UInt32              gsMapIndex;                     ///玩家座位号
		public  UInt32              gsServerTime;	                ///道具ID              
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_USER_BUY_BULLET
	{
		public  UInt32              gsChair;                       	///玩家座位号
		public  UInt32              gsScore;	                    ///购买的分数
		public  UInt32              gsGold;	                        ///金币余额              
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_GUN_POWER_UP
	{
		public  UInt32              gsChair;                        ///玩家座位号
		public  UInt32              gsPower;	                    ///玩家炮值            
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_GUN_FIRE
	{
		public  UInt32              gsChair;                        ///座位号
		public  float               gsRot;	                        ///玩家旋转值
		public  float               gsZ;	                        ///坐标
		public  float               gsY;                            ///坐标
		public  UInt32              gsCostVal;                      ///花费
		public  UInt32              gsSsrverId;	                    ///锁定NPC的ServerId -1为没锁定
		public  UInt32              gsServerTime;                   ///时间
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_GUN_POWER_MULTI
	{
		public  UInt32              gsChair;                        ///玩家座位号
		public  UInt32              gsMulti;	                    ///玩家威力            
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_NPC_KILL
	{
		public  UInt32              gsChairId;                      ///玩家座位号
		public  UInt32              gsNpcId;	                    ///NPC ID	若玩家客户端对应ID的NPC不存在. 直接将分加到玩家身上
		public  UInt32              gsRate;	                        ///倍率
		public  UInt32              gsPower;                        ///炮值
		public  UInt32              gsNPCType;	                    ///
		public  UInt32              gsFlag;	                        ///0 常规击杀 1 缓存击杀
		
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_WIN_FROM_CACHE
	{
		public  UInt32              gsChair;                       	///玩家座位号
		public  UInt32              gsRate;	                        ///倍率
		public  UInt32              gsPower;	                    ///炮值             
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_BALANCE
	{
		public  UInt32              gsChair;                        ///玩家座位号
		public  UInt32              gsGold;	                        ///金币            
	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_NPC_GENERATOR
	{
		public  UInt32              gsServerTime;                   ///时间
		public  UInt32              gsServerId;	                    ///服务器  ID
		public  UInt32              gsActorId;                   	///NPC ID
		public  UInt32              gsNavigId;                      ///导航
		public  float               gsNavigRot;	                    ///导航角度
		public  float               gsZ;	                        ///出生坐标
		public  float               gsY;                            ///出生坐标

	}


	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet=CharSet.Unicode)]
	public struct CMD_SC_GF_PROP_INFO
	{
		public  UInt32			    gsPropID;						///道具ID    
		public  UInt32    		    gsPrice;						//<价格  
		public  UInt32				gsVIPPRICE;						//<VIP价格
		public  UInt32				gsPropType;						//<道具类型
		public  UInt32				gsPresentGold;					//<赠送金币数	
		public  UInt32				gsPropActionAttrib;				//<道具属性
		public  UInt32			    gsPropValueAttrib;				//<	
		public  Byte			    gsIsHot;						//<0 热门，推荐
		public  Byte			    gsPropSex;						//<0女，1男，2通用
		public  Byte			    gsDelete;						//<是否能删除
		public  UInt32				gsVipLeve;						//<购买需要VIP等级
		public  UInt32				gsMoneyType;					//<货币类型
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public  string			    gsValidTime;					//<有效日期
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public  string          	gsPropName;                     //<道具名字
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
		public  string          	gsIntro;						//<具体描述   
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public  string          	gsImagePath;					//<大图路径
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public  string          	gsSamllImagePath;				//<小图路径
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public  string          	gsImagePath2;							
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public  string          	gsSamllImagePath2;						

		
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_PROP_USE
	{
		public  UInt32              gsChairId;                      ///使用者座位号
		public  UInt32              gsTargetChair;	                ///目标座位号 -1 没有目标
		public  UInt32              gsPropType;	                    ///道具类型
		public  UInt32              gsPropId;                       ///道具 ID
		public  UInt32              gsPropCount;	                ///道具剩余数量
		public  UInt32              gsHandleCode;	                ///操作码
		
	}

	//机器人 服务器通知填充开炮数据
	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_GF_ROBOT_GUN_FIRE_NOTIFY
	{
		public  UInt32              gsRobotChair;                   ///机器人座位号
		public  UInt32              gsNpcId;	                    ///NPC的服务器ID           
	}


	//	机器人 武器开炮
	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CMD_SC_Robot_GUN_FIRE
	{
		public  UInt32              gsRobotChair;                   ///机器人座位号
		public  UInt32              gsChair;                        ///处理数据玩家玩家座位号
		public  float               gsRot;	                        ///玩家旋转值
		public  float               gsZ;	                        ///坐标
		public  float               gsY;                            ///坐标
		public  UInt32              gsCostVal;                      ///花费
		public  UInt32              gsSsrverId;	                    ///锁定NPC的ServerId -1为没锁定
		public  UInt32              gsServerTime;                   ///时间
	}
	
}


