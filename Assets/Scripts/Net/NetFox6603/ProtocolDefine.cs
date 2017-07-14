using System;
using System.Runtime.InteropServices;

namespace com.QH.QPGame.Services.NetFox
{
    public class CommonDefine
    {
        public static int VERSION = 0x77;
        public static int TCP_HEAD_8_SIZE = 8;
        public static ushort TCP_INFO_SIZE = 4;
        public static ushort TCP_CMD_SIZE = 4;

        //设备类型
        public const byte DEVICE_TYPE_PC = 0x00;                              //PC
        public const byte DEVICE_TYPE_ANDROID = 0x10;                              //Android
        public const byte DEVICE_TYPE_IOS = 0x20;                             //iTouch
       //public const ushort DEVICE_TYPE_IPHONE = 0x40;                              //iPhone
       //public const ushort DEVICE_TYPE_IPAD = 0x80;                              //iPad

        //参数定义
        public const ushort INVALID_CHAIR = 0xFFFF;								//无效椅子
        public const ushort INVALID_TABLE = 0xFFFF;							//无效桌子

        //数据长度

        //资料数据
        public const int LEN_MD5 = 33; //加密密码

        public const int LEN_USERNOTE = 32; //备注长度

        public const int LEN_ACCOUNTS = 32; //帐号长度

        public const int LEN_NICKNAME = 32; //昵称长度

        public const int LEN_PASSWORD = 33; //密码长度

        public const int LEN_GROUP_NAME = 32; //社团名字

        public const int LEN_UNDER_WRITE = 32; //个性签名

        //数据长度
        public const int LEN_QQ = 16; //Q Q 号码

        public const int LEN_EMAIL = 33; //电子邮件

        public const int LEN_USER_NOTE = 256; //用户备注

        public const int LEN_SEAT_PHONE = 33; //固定电话

        public const int LEN_MOBILE_PHONE = 12; //移动电话

        public const int LEN_PASS_PORT_ID = 19; //证件号码

        public const int LEN_COMPELLATION = 16; //真实名字

        public const int LEN_DWELLING_PLACE = 128; //联系地址

        public const int LEN_DESCRIBE_STRING = 128; //错误描述

        //机器标识
        public const int LEN_NETWORK_ID = 13; //网卡长度

        public const int LEN_MACHINE_ID = 33; //序列长度

        //列表数据
        public const int LEN_TYPE = 32; //种类长度

        public const int LEN_KIND = 32; //类型长度

        public const int LEN_NODE = 32; //节点长度

        public const int LEN_PAGE = 32; //定制长度

        public const int LEN_SERVER = 32; //房间长度

        public const int LEN_PROCESS = 32; //进程长度


        public const ushort DTP_NULL = 0;								            //无效数据
        public const ushort DTP_GP_UI_NICKNAME = 1;						            //用户昵称
        public const ushort DTP_GP_UI_USER_NOTE = 2;								//用户说明
        public const ushort DTP_GP_UI_UNDER_WRITE = 3;								//个性签名
        public const ushort DTP_GP_UI_QQ = 4;								        //Q Q 号码
        public const ushort DTP_GP_UI_EMAIL = 5;								    //电子邮件
        public const ushort DTP_GP_UI_SEAT_PHONE = 6;								//固定电话
        public const ushort DTP_GP_UI_MOBILE_PHONE = 7;								//移动电话
        public const ushort DTP_GP_UI_COMPELLATION = 8;								//真实名字
        public const ushort DTP_GP_UI_DWELLING_PLACE = 9;							//联系地址



        //用户属性
        public const ushort DTP_GR_NICK_NAME = 10;								    //用户昵称
        public const ushort DTP_GR_GROUP_NAME = 11;								    //社团名字
        public const ushort DTP_GR_UNDER_WRITE = 12;                                //个性签名


        public const ushort SR_ALLOW_DYNAMIC_JOIN = 0x00000010;						//动态加入
        public const ushort SR_ALLOW_OFFLINE_TRUSTEE = 0x00000020;					//断线代打
        public const ushort SR_ALLOW_AVERT_CHEAT_MODE = 0x00000040;                 //隐藏信息

        //网址信息
        public const ushort DTP_GP_CDN = 1;					//更新地址
        public const ushort DTP_GP_OFFICESITE_URL = 2;		//官网地址
        public const ushort DTP_GP_BACK_STORGE_URL = 3;		//后台地址
        public const ushort DTP_GP_MODULE_INFO = 4;		    //模块信息
		
        public const ushort GAME_GENRE_GOLD = 0x0001;							//金币类型
        public const ushort GAME_GENRE_SCORE = 0x0002;							//点值类型
        public const ushort GAME_GENRE_MATCH = 0x0004;							//比赛类型
        public const ushort GAME_GENRE_EDUCATE = 0x0008;                        //训练类型

    }
    //请求更新 C->S
    public struct CMD_GP_RequestUpdateInfo
    {
        public Byte cbDeviceType;		//设备版本
        public UInt32 dwPlazaVersion; //广场版本
    };

    //更新信息yang S->C
    public struct CMD_GP_ClientUpdate
    {
        public int dwVersion;			//大厅版本
    };

    //数据描述
    [System.Serializable]
    public struct tagDataDescribe
    {
        public ushort wDataSize;						//数据大小
        public ushort wDataDescribe;					//数据描述
    };

    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct tagGameType
    {
        public ushort JoinID;
        public ushort SortID;
        public ushort TypeID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_TYPE)]
        public string TypeName;
    };

    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct tagGameKind
    {
        public ushort TypeID;
        public ushort JoinID;
        public ushort SortID;
        public ushort KindID;
        public ushort GameID;
        public uint OnlineCount;
        public uint FullCount;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_TYPE)]
        public string KindName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_PROCESS)]
        public string ProcessName;
    };

    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct tagGameNode
    {
        public ushort KindID;
        public ushort JoinID;
        public ushort SortID;
        public ushort NodeID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_NODE)]
        public string NodeName;
    };

    /*
    public struct tagGameProcess
    {
        public ushort SortID;
        public ushort TypeID;
        public ushort KindID;
        public ushort ServerPort;
        public uint ServerAddr;
        public uint MaxVersion;
        public uint OnLineCount;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_DESCRIBE_STRING)]
        public string KindName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_DESCRIBE_STRING)]
        public string ProcessName;
    };*/

    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public class tagGameServer
    {
        public ushort KindID;
        public ushort NodeID;
        public ushort SortID;
        public ushort ServerID;
        public ushort ServerPort;
        public uint OnlineCount;
        public uint FullCount;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_SERVER)]
        public string ServerAddr;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_SERVER)]
        public string ServerName;

        //积分信息
        public Int64 lServerScore; 		//底分
        public Int64 lMinServerScore; 	//需要积分
    };

    //在线信息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public class tagOnLineInfoServer
    {
        public Int16 wServerID;							//房间标识
        public Int32 dwOnLineCount;						//在线人数
    }

    //房间配置

    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct CMD_GR_ConfigServer
    {
        //房间属性
        public UInt16 wTableCount;						//桌子数目
        public UInt16 wChairCount;						//椅子数目

        //房间配置
        public UInt16 wServerType;						//房间类型
        public UInt32 dwServerRule;						//房间规则
    };


    //桌子状态
    struct tagTableStatus
    {
        public UInt16 cbTableLock;						//锁定标志
        public UInt16 cbPlayStatus;						//游戏标志
    };


    //用户信息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct tagUserInfo
    {
        //基本属性
        public UInt32 dwUserID;							//用户 I D
        public UInt32 dwGameID;							//游戏 I D
        public UInt32 dwGroupID;							//社团 I D

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szNickName;			//用户昵称

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szGroupName;		//社团名字

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szUnderWrite;		//个性签名

        //头像信息
        public UInt32 wFaceID;							//头像索引
        public UInt32 dwCustomID;							//自定标识

        //用户资料
        public Byte cbGender;							//用户性别
        public Byte cbMemberOrder;						//会员等级
        public Byte cbMasterOrder;						//管理等级

        //用户状态
        public UInt16 wTableID;							//桌子索引
        public UInt16 wChairID;							//椅子索引
        public Byte cbUserStatus;						//用户状态

        //积分信息
        public Int64 lScore;								//用户分数
        public Int64 lGrade;								//用户成绩
        public Int64 lInsure;							//用户银行

        //游戏信息
        public UInt32 dwWinCount;							//胜利盘数
        public UInt32 dwLostCount;						//失败盘数
        public UInt32 dwDrawCount;						//和局盘数
        public UInt32 dwFleeCount;						//逃跑盘数
        public UInt32 dwUserMedal;						//用户奖牌
        public UInt32 dwExperience;						//用户经验
        public UInt32 lLoveLiness;						//用户魅力
    };

    //用户信息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct tagUserInfoHead
    {
        //用户属性
        public UInt32 dwGameID;							//游戏 I D
        public UInt32 dwUserID;							//用户 I D
        public UInt32 dwGroupID;							//社团 I D

        //头像信息
        public UInt32 wFaceID;							//头像索引
        public UInt32 dwCustomID;							//自定标识

        //用户属性
        public Byte cbGender;							//用户性别
        public Byte cbMemberOrder;						//会员等级
        public Byte cbMasterOrder;						//管理等级

        //用户状态
        public UInt16 wTableID;							//桌子索引
        public UInt16 wChairID;							//椅子索引
        public Byte cbUserStatus;						//用户状态

        //积分信息
        public Int64 lScore;								//用户分数
        public Int64 lGrade;								//用户成绩
        public Int64 lInsure;							//用户银行

        //游戏信息
        public UInt32 dwWinCount;							//胜利盘数
        public UInt32 dwLostCount;						//失败盘数
        public UInt32 dwDrawCount;						//和局盘数
        public UInt32 dwFleeCount;						//逃跑盘数
        public UInt32 dwUserMedal;						//用户奖牌
        public UInt32 dwExperience;						//用户经验
        public UInt32 lLoveLiness;						//用户魅力
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

    public enum GameState
    {
        GS_FREE = 0,
        GS_PLAYING = 100
    };

    public enum TableState
    {
        TB_FREE = 0,
        TB_PLAYER
    };

    //主指令
    public class MainCommand
    {
        //内核
        public const ushort MDM_KN_COMMAND = 0;
        public const ushort MDM_CS_MANAGER_SERVICE = 5;
        //登陆
        public const ushort MDM_GP_LOGON = 1;
        public const ushort MDM_GP_SERVER_LIST = 2;
        public const ushort MDM_GP_USER_SERVICE = 3;
        public const ushort MDM_GP_REMOTE_SERVICE = 4;
        public const ushort MDM_MB_LOGON = 100;
        public const ushort MDM_MB_SERVER_LIST = 101;
        public const ushort MDM_GP_EXTRA = 20;
        //房间
        public const ushort MDM_GR_LOGON = 1;
        public const ushort MDM_GR_CONFIG = 2;
        public const ushort MDM_GR_USER = 3;
        public const ushort MDM_GR_STATUS = 4;
        public const ushort MDM_GR_SYSTEM = 1000;

        public const ushort MDM_GF_GAME = 200;
        public const ushort MDM_GF_FRAME = 100;

        public const ushort MDM_WEB_NOTICATION = 300;		                    // web
    };


    //
    public class SubCommand
    {
        //MDM_KN_COMMAND
        public const ushort SUB_KN_DETECT_SOCKET = 1; //C->S	
        //MDM_CS_MANAGER_SERVICE
        public const ushort SUB_CS_S_MARQUEE_MESSAGE = 300;//S->C

        //MDM_GP_LOGON
        public const ushort SUB_GP_LOGON_USERID = 1; //C->S
        public const ushort SUB_GP_LOGON_ACCOUNTS = 2; //C->S
        public const ushort SUB_GP_REGISTER_ACCOUNTS = 3; //C->S
        public const ushort SUB_GP_LOGON_PASSPORT = 4; //C->S
        public const ushort SUB_GP_REGISTER_PASSPORT = 5; //C->S
        public const ushort SUB_GR_QUERY_TABLES = 10; //C->S


        public const ushort SUB_GP_LOGON_SUCCESS = 100; //S->C
        public const ushort SUB_GP_LOGON_ERROR = 101; //S->C
        public const ushort SUB_GP_LOGON_FINISH = 102; //S->C
        public const ushort SUB_GP_COMMUNICATION_KEY = 104;//S->C
        public const ushort SUB_GP_REPEAT_LOGON = 105;//S->C
        public const ushort SUB_GP_UPDATE_NOTIFY = 200;//S->C升级提示
        public const ushort SUB_GR_PRIVATE_TABLE = 201; //S->C
        public const ushort SUB_GR_QUERY_FINISH = 202; //S->C

        //MDM_GP_SERVER_LIST
        //获取命令
        public const ushort SUB_GP_GET_LIST = 1; //获取列表
        public const ushort SUB_GP_GET_SERVER = 2; //获取房间
        public const ushort SUB_GP_GET_ONLINE = 3; //获取在线
        public const ushort SUB_GP_GET_COLLECTION = 4; //获取收藏
        //列表信息
        public const ushort SUB_GP_LIST_TYPE = 100;                                 //S->C
        public const ushort SUB_GP_LIST_KIND = 101;                                 //S->C
        public const ushort SUB_GP_LIST_NODE = 102;                                 //S->C
        public const ushort SUB_GP_LIST_SERVER = 104;                               //S->C
        public const ushort SUB_CS_S_SERVER_ONLINE = 111;	                        //房间人数

        public const ushort SUB_GP_LIST_FINISH = 200; //S->C
        public const ushort SUB_GP_SERVER_FINISH = 201; //房间完成

        //MDM_GP_USER_SERVICE
        //账号服务
        public const ushort SUB_GP_MODIFY_MACHINE = 100; //修改机器
        public const ushort SUB_GP_MODIFY_LOGON_PASS = 101; //修改密码
        public const ushort SUB_GP_MODIFY_INSURE_PASS = 102; //修改密码
        public const ushort SUB_GP_MODIFY_UNDER_WRITE = 103; //修改签名

        public const ushort SUB_GP_CHANGE_PASSWD = 104;	//修改登录密码yang
        public const ushort SUB_GP_CHANGE_BANK_PASSWD = 105;//修改银行密码yang
        public const ushort SUB_GP_LOCK_OR_UNLOCK_ACCOUNT = 106;	//绑定机器yang
        public const ushort SUB_GP_USER_SUGGESTION = 107;	//玩家反馈yang
        public const ushort SUB_GP_GAME_RECORD = 108;	//游戏记录yang
        public const ushort SUB_GP_LOGON_RECORD = 109;	//登陆记录yang
        public const ushort SUB_GP_RECHANGE_INFO = 110;	//充值信息yang
        public const ushort SUB_GP_EXCHANGE_INFO = 111;		//兑换信息yang
        public const ushort SUB_GP_REFRASH_USER_INFO = 112;		//刷新用户信息yang
        public const ushort SUB_GP_MARQUEE_MESSAGE = 113;		//跑马灯消息yang
        public const ushort SUB_GP_GET_VERSION_INFO = 114;			//更新信息yang
        public const ushort SUB_GP_CANCLE_MARQUEE = 115;			//取消跑马灯消息yang

        //修改头像
        public const ushort SUB_GP_USER_FACE_INFO = 200; //头像信息
        public const ushort SUB_GP_SYSTEM_FACE_INFO = 201; //系统头像
        public const ushort SUB_GP_CUSTOM_FACE_INFO = 202; //自定头像

        //个人资料
        public const ushort SUB_GP_USER_INDIVIDUAL = 301; //个人资料
        public const ushort SUB_GP_QUERY_INDIVIDUAL = 302; //查询信息
        public const ushort SUB_GP_MODIFY_INDIVIDUAL = 303; //修改资料

        //银行服务
        public const ushort SUB_GP_USER_SAVE_SCORE = 400; //存款操作
        public const ushort SUB_GP_USER_TAKE_SCORE = 401; //取款操作
        public const ushort SUB_GP_USER_TRANSFER_SCORE = 402; //转账操作
        public const ushort SUB_GP_USER_INSURE_INFO = 403; //银行资料
        public const ushort SUB_GP_QUERY_INSURE_INFO = 404; //查询银行
        public const ushort SUB_GP_USER_INSURE_SUCCESS = 405; //银行成功
        public const ushort SUB_GP_USER_INSURE_FAILURE = 406; //银行失败
        public const ushort SUB_GP_QUERY_USER_INFO_REQUEST = 407; //查询用户
        public const ushort SUB_GP_QUERY_USER_INFO_RESULT = 408; //用户信息
        public const ushort SUB_GP_SAFETYBOX_VERIFY = 409;  //校验银行密码

        //操作结果
        public const ushort SUB_GP_OPERATE_SUCCESS = 900; //操作成功
        public const ushort SUB_GP_OPERATE_FAILURE = 901; //操作失败

        //MDM_GP_SYSTEM
        public const ushort SUB_GP_VERSION = 100; //S->C
        public const ushort SUB_SP_SYSTEM_MSG = 101; //S->C

        //MDM_GR_LOGON 
        public const ushort SUB_GR_LOGON_USERID = 1; //帐号登录 C->S
        public const ushort SUB_GR_LOGON_ACCOUNTS = 3; //帐户登录 C->S
        public const ushort SUB_GR_LOGON_SUCCESS = 100; //登陆成功 S->C
        public const ushort SUB_GR_LOGON_ERROR = 101; //登陆失败 S->C
        public const ushort SUB_GR_LOGON_FINISH = 102; //登陆完成 S->C

        //MDM_GR_USER
        public const ushort SUB_GR_USER_RULE = 1;							//用户规则
        public const ushort SUB_GR_USER_LOOKON = 2;							//旁观请求
        public const ushort SUB_GR_USER_SITDOWN = 3;							//坐下请求
        public const ushort SUB_GR_USER_STANDUP = 4;								//起立请求
        public const ushort SUB_GR_USER_INVITE = 5;								//用户邀请
        public const ushort SUB_GR_USER_INVITE_REQ = 6;								//邀请请求
        public const ushort SUB_GR_USER_REPULSE_SIT = 7;								//拒绝玩家坐下
        public const ushort SUB_GR_USER_KICK_USER = 8;                                //踢出用户
        public const ushort SUB_GR_USER_INFO_REQ = 9;                                   //请求用户信息
        public const ushort SUB_GR_USER_CHAIR_REQ = 10;                                 //请求更换位置
        public const ushort SUB_GR_USER_CHAIR_INFO_REQ = 11;                                //请求椅子用户信息
        public const ushort SUB_GR_USER_WAIT_DISTRIBUTE = 12;							//等待分配

        public const ushort SUB_GR_USER_ENTER = 100; //用户进入 S->C
        public const ushort SUB_GR_USER_SCORE = 101; //用户分数 S->C
        public const ushort SUB_GR_USER_STATUS = 102; //用户状态 S->C
        public const ushort SUB_GR_REQUEST_FAILURE = 103; //坐下失败 S->C


        public const ushort SUB_GR_CONFIG_COLUMN = 100;								//列表配置
        public const ushort SUB_GR_CONFIG_SERVER = 101;								//房间配置
        public const ushort SUB_GR_CONFIG_PROPERTY = 102;							//道具配置
        public const ushort SUB_GR_CONFIG_FINISH = 103;								//配置完成
        public const ushort SUB_GR_CONFIG_USER_RIGHT = 104;							//玩家权限

        //MDM_GR_STATUS
        public const ushort SUB_GR_TABLE_INFO = 100; //桌子信息 S->C
        public const ushort SUB_GR_TABLE_STATUS = 101; //桌子状态 S->C
        //MDM_GR_SYSTEM
        public const ushort SUB_GR_MESSAGE = 100; //系统消息 S->C
        //MDM_GF_FRAME & MDM_GF_GAME
        public const ushort SUB_GF_INFO = 1; //游戏信息 S->C
        public const ushort SUB_GF_USER_READY = 2; //用户同意
        public const ushort SUB_GF_LOOKON_CONTROL = 3; //旁观控制
        public const ushort SUB_GF_OPTION = 100; //游戏配置
        public const ushort SUB_GF_SCENE = 101; //场景信息
        public const ushort SUB_GF_USER_CHAT = 10; //用户聊天
        public const ushort SUB_GF_MESSAGE = 200; //系统消息
        public const ushort SUB_GF_MARQUEE_MESSAGE = 202; //跑马灯消息


        //MDM_WEB_MAIN_ID
        public const int SUB_WEB_FROZEN_ACCOUNT = 25;					//	账号冻结
        public const int SUB_WEB_UP_OR_DOWN_POINT = 27;					// 上下分数

    };

    //系统消息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_CS_MarqueeMessage
    {
        public UInt64 MsgStartTime;                                                 //开始时间
        public Int32  MsgNumberID;                                                   //跑马灯消息ID
        public UInt16 MsgType;							                            //消息类型
        public UInt16 MsgID;								                        //消息ID(选择预设)
        public UInt16 MsgFlag;							                            //显示位置
        public UInt16 MsgPlayCount;				                                    //消息播放次数
        public UInt16 MsgInterval;				                                    //消息播放间隔时间
        public UInt16 MsgPlayTime;                                                  //消息播放时间
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szMessage;						//消息
    };
    //删除系统消息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_CS_CancleMarqueeMsg
    {
        public Int32 dwMarqueeMsgID;                                                 //消息编号
    };


    //帐号登录
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    internal struct CMD_GP_LogonAccounts
    {
        //系统信息
        public UInt32 dwPlazaVersion; //广场版本

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_MACHINE_ID)]
        public string szMachineID; //机器序列

        //登录信息
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_MD5)]
        public string szPassword; //登录密码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_ACCOUNTS)]
        public string szAccounts; //登录帐号

        public Byte cbValidateFlags; //校验标识
    };


    //登录成功
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_RegisterAccounts
    {
        //系统信息
        public UInt32 dwPlazaVersion; //广场版本

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szMachineID; //机器序列

        //密码变量
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szLogonPass; //登录密码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szInsurePass; //银行密码

        //注册信息
        public UInt16 wFaceID; //头像标识
        public byte cbGender; //用户性别

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szAccounts; //登录帐号

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szNickName; //用户昵称

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szSpreader; //推荐帐号

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string szPassPortID; //证件号码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string szCompellation; //真实名字

        public byte cbValidateFlags; //校验标识
    };

    //登录成功
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_LogonSuccess
    {
        //属性资料
        public UInt32 wFaceID; //头像标识
        public UInt32 dwUserID; //用户 I D
        public UInt32 dwGameID; //游戏 I D
        public UInt32 dwGroupID; //社团标识
        public UInt32 dwCustomID; //自定标识
        public UInt32 dwUserMedal; //用户奖牌
        public UInt32 dwExperience; //经验数值
        public UInt32 dwLoveLiness; //用户魅力

        //用户成绩
        public Int64 lUserScore; //用户金币
        public Int64 lUserInsure; //用户银行

        //用户信息
        public Byte cbGender; //用户性别
        public Byte cbMoorMachine; //锁定机器

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_ACCOUNTS)]
        public string szAccounts; //登录帐号

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_ACCOUNTS)]
        public string szNickName; //用户昵称

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_GROUP_NAME)]
        public string szGroupName; //社团名字

        //配置信息
        public Byte cbShowServerStatus; //显示服务器状态
        public Int32 dwLockServerID;    //锁定房间ID
    };

    public enum FailureCode
    {
        LockServerID = 4
    };

    //登录失败
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_LogonFailure
    {
        public UInt32 lResultCode; //错误代码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CommonDefine.LEN_DESCRIBE_STRING)]
        public string szDescribeString; //描述消息
    };

    //登陆完成
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_LogonFinish
    {
        public ushort wIntermitTime; //中断时间
        public ushort wOnLineCountTime; //更新时间
    };

    //安全密钥yang
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_CommunicationKey
    {
        public Int64 dwServerTime; //服务器时间
        public UInt32 dwUserID;			//用户 I D
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szToken;		//安全密钥	
    };

    //升级提示
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UpdateNotify
    {
        public Byte cbMustUpdate; //强行升级
        public Byte cbAdviceUpdate; //建议升级
        public UInt32 dwCurrentVersion; //当前版本
    };

    //修改密码
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_ModifyLogonPass
    {
        public UInt32 dwUserID; //用户 I D

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szDesPassword; //用户密码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szScrPassword; //用户密码
    };

    //修改银行密码
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_ModifyInsurePass
    {
        public UInt32 dwUserID; //用户 I D

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szDesPassword; //用户密码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szScrPassword; //用户密码
    };

    ///////////////////////////////////////////////////////

    //用户头像
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserFaceInfo
    {
        public UInt32 wFaceID; //头像标识
        public UInt32 dwCustomID; //自定标识
    };

    //修改头像
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_SystemFaceInfo
    {
        public UInt32 wFaceID; //头像标识
        public UInt32 dwUserID; //用户 I D

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szPassword; //用户密码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szMachineID; //机器序列
    };

    //修改头像
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_CustomFaceInfo
    {
        public UInt32 dwUserID; //用户 I D

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szPassword; //用户密码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szMachineID; //机器序列

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48 * 48)]
        public UInt32 dwCustomFace; //图片信息
    };

    ///////////////////////////////////////////////////////

    //绑定机器 C->S
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_ModifyMachine
    {
        public byte cbBind; //绑定标志
        public UInt32 dwUserID; //用户标识

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szPassword; //用户密码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szMachineID; //机器序列
    };

    //绑定  S->C 服务器发客户端
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_LockAndUnlock
    {
        public UInt32 dwUserID;							//用户ID                 
        public UInt32 dwCommanType;						//命令请求类型,1表示要求锁定，0表示要求解除锁定
        public UInt32 dwCommanResult;						//请求的结果 0代表成功,其他为失败
    }
    ///////////////////////////////////////////////////////


    //个人资料
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserIndividual
    {
        public UInt32 dwUserID; //用户 I D
    };

    //个人信息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_UserInformation
    {
        public string dwName;							//真实名字
        public string dwIdentification;				//身份证号码
        public string dwCellPhone;					//手机号
        public string dwIM;							//QQ号
        public UInt32 dwLogoID;			            //用户头像
    };

    //查询信息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_QueryIndividual
    {
        public UInt32 dwUserID; //用户 I D
    };

    //修改资料
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_ModifyIndividual
    {
        public byte cbGender; //用户性别
        public UInt32 dwUserID; //用户 I D

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szPassword; //用户密码
    };


    //银行资料
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserInsureInfo
    {
        public byte wRevenueTake; //税收比例
        public byte wRevenueTransfer; //税收比例
        public byte wServerID; //房间标识
        public Int64 lUserScore; //用户金币
        public Int64 lUserInsure; //银行金币
        public UInt64 lTransferPrerequisite; //转账条件
    };

    //存入金币
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserSaveScore
    {
        public UInt32 dwUserID; //用户 I D
        public Int64 lSaveScore; //存入金币
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szMachineID; //机器序列
    };

    //提取金币
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserTakeScore
    {
        public UInt32 dwUserID; 			//用户 I D
        public Int64 lTakeScore; 		//提取金币
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szPassword; 		//银行密码
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szMachineID; 		//机器序列
    };

    //转账金币
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserTransferScore
    {
        public UInt32 dwUserID; 			//用户 I D
        public byte cbByNickName; 		//昵称赠送
        public Int64 lTransferScore; 	//转账金币
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szPassword; 		//银行密码
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szNickName; 		//目标用户
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szMachineID; 		//机器序列
    };

    //银行成功
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserInsureSuccess
    {
        public UInt32 dwUserID; 			//用户 I D
        public Int64 lUserScore; 		//用户金币
        public Int64 lUserInsure; 		//银行金币
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szDescribeString; 	//描述消息
    };

    //银行失败
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserInsureFailure
    {
        public UInt32 lResultCode; 		//错误代码
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szDescribeString; 	//描述消息
    };

    //提取结果
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserTakeResult
    {
        public UInt32 dwUserID; 			//用户 I D
        public Int64 lUserScore; 		//用户金币
        public Int64 lUserInsure; 		//银行金币
    };

    //查询银行
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_QueryInsureInfo
    {
        public UInt32 dwUserID; 			//用户 I D
    };


    ///////////////////////////////////////////////////////

    //操作失败
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_OperateFailure
    {
        public UInt32 lResultCode; 		//错误代码
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szDescribeString; 	//描述消息
    };

    //操作成功
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_OperateSuccess
    {
        public UInt32 lResultCode; 		//操作代码
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szDescribeString; 	//成功消息
    };

    //游戏日志c->s
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CMD_GP_GameRecordRequest
    {
        public UInt32 dwUserID; 			//用户ID
        public UInt32 dwGameKind;			//游戏类型
        public UInt32 dwPage; 			//页码
        public UInt32 dwPageSize;			//每页条数
        public UInt64 dwTime; 			//日期
    }

    //公共向服务器发送请求c->s
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CMD_GP_UserRequest
    {
        public UInt32 dwUserID; 			//用户ID
    }

    //公共向服务器发送请求返回S->C
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CMD_GP_UserResult
    {
        public int dwCheckCode; 			//用户操作返回
    }

    //玩家反馈C->S
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_UserSuggestion
    {
        public UInt32 dwUserID; 			//用户ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dwType; 			//问题类型
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string dwUserSuggestion;	//反馈内容
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string dwCellPhone; 		//手机号
    }

    //保险柜密码验证
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_SafetyBoxVerify
    {
        public UInt32 dwUserID; 			//用户ID                 			
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string dwMD5Pass; 			//加密密码	
    }

    //登陆日志S->C
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_LogonRecord
    {
        public UInt64 dwTmlogonTime;			//登陆时间               
        public UInt32 dwLogonIP; 				//登陆IP				
    }

    //游戏日志S->C
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_GameRecord
    {
        public UInt64 dwEndTime; 				//游戏结束时间
        public UInt32 dwGameKind;				//游戏类型
        public Int64 dwAmount;				//输赢金额
        public UInt32 dwAllCount; 			//总记录
    }

    //充值信息S->C
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_RechangeInfo
    {
        public Int64 dwMinMoney;		//最少充值钱           
        public UInt32 dwChangeScale;  //兑换比例
    }

    //兑现信息S->C
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GP_ExchangeInfo
    {
        public Int64 dwMinMoney;		//最少充值钱            
        public UInt32 dwChangeScale;  //兑换比例
        public Int64 dwWithdrawals;  //待提现RMB
        public Int64 dwBankMoney;	//银行金币
    }

    //游戏日志UI信息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_GameRecordRequest
    {
        public UInt32 dwGameKind;			//游戏类型
        public UInt32 dwPage; 			//页码
        public UInt32 dwPageSize;			//每页条数
        public UInt64 dwTime; 			//日期
    }

    //金钱日志
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_MoneyRecord
    {
        public UInt64 dwCreateDate; 		//交易日期               
        public UInt32 dwSender; 			//汇款人ID
        public UInt32 dwReceiver;			//收款人ID
        public UInt32 dwChangeType;		//变化类型
        public Int64 dwAmount; 			//金额
    }

    //玩家反馈UI信息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_UserSuggestion
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string dwType; 			//问题类型
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 402)]
        public string dwUserSuggestion;	//反馈内容
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string dwCellPhone; 		//手机号
    }

    //上下分数
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_WebUpOrDownPoint
    {
        public UInt32 dwUserID;			//用户 ID              
        public Int64 dwMoney; 			//用户上下金币
    }

    //注册
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_GuestRegistQuick
    {
        public Byte dwBoy; 				// 用户性别
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string dwName;				//用户登录名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string dwNickName;			//用户昵称	
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string dwMD5Pass;			//用户加密密码	
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string dwToken;			//网卡
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string dwCPUID;			//cup序列号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string dwReferrerID; 		//推荐人ID
    }

    //注册账户名验证
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_LogonRegVerify
    {
        public int dwType; 			//0为都验证,1为用户名,2为昵称             
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 61)]
        public string dwName; 			//用户登录名
    }

    //刷新用户信息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_GH_RefrashUserInfo
    {
        public Int64 dwUserScore;		//用户金币
    }

    //房间结构

    //I D 登录
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct CMD_GR_LogonUserID
    {
        //版本信息
        public UInt32 dwPlazaVersion;						//广场版本
        public UInt32 dwFrameVersion;						//框架版本
        public UInt32 dwProcessVersion;					//进程版本

        //登录信息
        public UInt32 dwUserID;							//用户 I D

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szPassword;				//登录密码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szMachineID;		//机器序列

        public UInt16 wKindID;							//类型索引
    };


    //登录成功
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct CMD_GR_LogonSuccess
    {
        public UInt32 dwUserRight;						//用户权限
        public UInt32 dwMasterRight;						//管理权限
    };

    //登录失败
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct CMD_GR_LogonFailure
    {
        public UInt32 lErrorCode;							//错误代码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szDescribeString;				//描述消息
    };

    //坐下请求
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct CMD_GR_UserSitDown
    {
        public UInt16 wTableID;							//桌子位置
        public UInt16 wChairID;							//椅子位置

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string szPassword;			//桌子密码
    };

    //起立请求
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct CMD_GR_UserStandUp
    {
        public UInt16 wTableID;							//桌子位置
        public UInt16 wChairID;							//椅子位置
        public Byte cbForceLeave;						//强行离开
    };

    //用户分数
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct CMD_GR_UserScore
    {
        public UInt32 dwUserID;							//用户标识
        //积分信息
        public Int64 lScore;								//用户分数
        public Int64 lGrade;								//用户成绩
        public Int64 lInsure;							//用户银行

        //输赢信息
        public UInt32 dwWinCount;							//胜利盘数
        public UInt32 dwLostCount;						//失败盘数
        public UInt32 dwDrawCount;						//和局盘数
        public UInt32 dwFleeCount;						//逃跑盘数

        //全局信息
        public UInt32 dwUserMedal;						//用户奖牌
        public UInt32 dwExperience;						//用户经验
        public UInt32 lLoveLiness;						//用户魅力
    };

    //用户状态
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct CMD_GR_UserStatus
    {
        public UInt32 dwUserID;							//用户标识
        public ushort TableID;
        public ushort ChairID;
        public byte UserStatus;
    };

    //请求失败
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct CMD_GR_RequestFailure
    {
        public UInt32 lErrorCode;							//错误代码

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szDescribeString;				//描述信息
    };


    //系统消息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    struct CMD_CM_SystemMessage
    {
        public UInt16 wType;								//消息类型
        public UInt16 wLength;							//消息长度
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string szString;							//消息内容					
    };
}