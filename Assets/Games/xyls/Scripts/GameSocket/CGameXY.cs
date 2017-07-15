using UnityEngine;
using System.Collections;
using Shared;
using com.QH.QPGame.Services.NetFox;
using System;
using System.Runtime.InteropServices;
using com.QH.QPGame.Services.Utility;

namespace com.QH.QPGame.xyls
{
    //主指令
    class MainCmd : MainCommand
    {
        public const int MDM_GF_INFO = 200;							///游戏信息
    };

    //子指令
    class SubCmd : SubCommand
    {
        //客户端命令结构
        public const byte SUB_C_PLACE_JETTON = 1;		//用户下注
        public const byte SUB_C_PLAYER_CONTINUE_BET = 8;//用户续押
        public const byte SUB_C_CLEAR_JETTON = 2;       //清除下注
        public const byte SUB_C_CONTROL_SET_PRIZE = 6;  //管理员设置开奖结果

        //服务器命令结构
        public const byte GAME_STATUS_FREE = 0;        //空闲状态
        public const byte GAME_STATUS_PLAY = 100;      //游戏状态
        public const byte GAME_STATUS_WAIT = 200;      //等待状态

        public const byte GS_PLACE_JETTON = GAME_STATUS_PLAY;			//下注状态
        public const byte GS_GAME_END = GAME_STATUS_PLAY + 1;			//结束状态

        public const byte SUB_S_GAME_FREE = 99;			    //游戏空闲
        public const byte SUB_S_GAME_START = 100;           //游戏开始  
        public const byte SUB_S_PLACE_JETTON = 101;         //用户下注
        public const byte SUB_S_PLACE_JETTON_FAIL = 107;	//下注失败
        public const byte SUB_S_GAME_END = 102;			    // 游戏结束

        public const byte SUB_S_CLEAR_JETTON = 109;         //下注失败(清除下注)

        public const byte SUB_S_CREATE_TRUN = 115;	        //生成转盘
        public const byte SUB_S_PRIZE_COLOR = 116;  	    //开奖颜色
        public const byte SUB_S_PLAYER_CONTINUE_BET = 117;  //用户续押
        public const byte SUB_S_SEND_GAMERECORD = 118;		//游戏记录
        public const byte SUB_S_PLACE_CONTINUE_FAIL = 119;  //续押失败
        public const byte SUB_S_SEND_PRIZE_REWARD = 120;    //发送彩金中奖
        public const byte SUB_S_SEND_PRIZE_DATA = 121;      //发送彩金信息
        public const byte SUB_S_USEROFFLINE = 122;          //断线重连
    }

    /// <summary>
    /// 用户控制指令
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_C_Control
    {
        public byte cbCmdType;
        public ushort wChairID;
        public eAnimalPrizeMode eAnimalPrize;
        public eAnimalType eAnimal;
        public eColorType eColor;
        public eEnjoyGameType eEnjoyGame;
        public Int64 lStorageScore;
        public int nStorageDecute;
    }

    /// <summary>
    /// 进入场景_下注阶段
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_StatusJetton
    {
	    public int ContinueBetState;					//续押状态

	    // 下注限制信息	
	    public Int64 iUserScore;						//我的金币
	    public int iJetton1;					        //筹码1(配置文件读取)
	    public int iJetton2;					        //筹码2(配置文件读取)
	    public int iJetton3;					        //筹码3(配置文件读取)

	    //全局信息
	    public byte cbTimeLeave;						//剩余时间
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public Int64[] iAllUserJettonScore;			    //各个区域总注
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
	    public Int64[] iJettonScore;					//各个区域下注
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
	    public int[]  dwMul;							//各个区域倍率
    }

    /// <summary>
    /// 断线重连
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_OnActionUserOffLine
    {
        public ushort wChairID;									 //用户位置
		public int ContinueBetState;							 //续押状态
		public byte cbTimeLeave;						//剩余时间
		
		public Int64 iUserScore;						//我的金币
		public int iJetton1;					    //筹码1(配置文件读取)
		public int iJetton2;					    //筹码2(配置文件读取)
		public int iJetton3;					    //筹码3(配置文件读取)
		
		// 是否处于下注状态(0是,1否)
		public int		IsPlaceJetton;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public Int64[] iAllUserJettonScore;			        //各个区域总注
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public Int64[] iJettonScore;					    //各个区域下注
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public int[] dwMul;							        //各个区域倍率
    }

    /// <summary>
    /// 彩金中奖
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_SendPrizePoolReward
    {
        public Int64 lRewardGold;                          //中奖金币
        public ushort wTableID;                            //中奖玩家桌子ID
        public ushort wChairID;                            //中奖玩家ID
    }

    /// <summary>
    /// 彩金数据
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_PRIZE_DATA
    {
        public Int64 lPrazePool;            //彩金数
    }

    /// <summary>
    /// 续押失败
    /// </summary> 
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_ContinueJettonsFail
    {
        // 0为续押失败,1为续押未失败(包括其他多种情况)
        public Int32 ContinueJettonsFail;			// 续押状态
    }

    /// <summary>
    /// 清除下注
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_ClearJetton
    {
        // 0: 成功，1：失败
        public UInt32 dwErrorCode;
        public ushort wChairID;				    	//用户位置
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public Int64[] iTotalPlayerJetton;		    //总下注
    }

    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_ContinueJettons_temp
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int64[] iAnimalJettonNum_temp;
    }
    /// <summary>
    /// 玩家续押
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_ContinueJettons
    {
        public ushort wChairID;									//用户位置
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public CMD_S_ContinueJettons_temp[] iAnimalJettonNum;	//玩家下注

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public Int64[] iEnjoyGameJettonNum;		                //庄闲和下注

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public Int64[] iTotalPlayerJetton;                      //总下注
    }

    /// <summary>
    /// 游戏状态_空闲
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_GameFree
    {
        public Int64 iUserScore;						//我的金币
        public byte cbTimeLeave;						//剩余时间
        //BYTE							cbGameRecord;   //本次开出的结果
        //STAnimalPrize stAnimalPrize;
        //STEnjoyGamePrizeInfo stEnjoyGamePrizeInfo;
        public UInt32 qwGameTimes;                      //当前是游戏启动以来的第几局

        //public CMD_BANKER_INFO stBankerInfo;            //庄家信息
        //public byte cbCanCancelBank;					//是否可以申请下庄（0： 不能下庄，1：能下庄）
    }

    /// <summary>
    ///  游戏状态_开始
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_GameStart
    {
        public int ContinueBetState;								  //续押状态

        public Int64 iUserScore;										//我的金币
        public byte cbTimeLeave;						                //剩余时间

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public UInt32[] dwMul;											//动物开奖倍率

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int32[] arrColorRate;                                   //颜色分布概率

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public STEnjoyGameAtt[] arrSTEnjoyGameAtt;                    //庄闲和属性


        //public CMD_BANKER_INFO stBankerInfo;				          //庄家信息
        //public byte cbBankerFlag;					                  //庄家表示 0： 非庄家，1： 庄家
    }

    /// <summary>
    /// 游戏状态_结束
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_GameEnd
    {
        //下一局信息
        public UInt32 dwTimeLeave;						        //剩余时间
        public STAnimalPrize stWinAnimal;						//开奖动物
        public STEnjoyGamePrizeInfo stWinEnjoyGameType;         //开奖庄闲和
        //玩家成绩
        public Int64 iUserScore;							    //玩家成绩
        //全局信息
//         public Int64 iRevenue;							    //游戏税收
// 
//         public CMD_BANKER_INFO stBankerInfo;					//庄家信息
//         public Int64 iBankerScore;						    //庄本次得分
    }

    /// <summary>
    /// 进入场景_游戏空闲
    /// </summary>  
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_StatusFree
    {
        // 下注限制信息
        public Int64 iUserScore;						                                //我的金币
        public Int32 iJetton1;			                                    		    //筹码1(配置文件读取,服务器发送)
        public Int32 iJetton2;					                                        //筹码2(配置文件读取,服务器发送)
        public Int32 iJetton3;					                                        //筹码3(配置文件读取,服务器发送)
        //全局信息
        public byte cbTimeLeave;						                                 //剩余时间

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public STAnimalAttArray[] arrSTAnimalArray;                                  // 哪种动物

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public UInt32[] arrColorRate;                                                   //颜色分布概率

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public STEnjoyGameAtt[] arrSTEnjoyGameJettonLimit;                              //庄闲和属性
        //public CMD_BANKER_INFO stBankerInfo;				                            //庄家信息
    }

    /// <summary>
    /// 进入场景_游戏进行中
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_StatusPlay
    {
        public Int64 iUserScore;                                 //我的金币
        public byte cbTimeLeave;                                 //剩余时间
        public byte cbGameStatus;                                //游戏状态

        public Int32 iJetton1;                                  //筹码1(配置文件读取,服务器发送)
        public Int32 iJetton2;                                  //筹码2(配置文件读取,服务器发送)
        public Int32 iJetton3;                                  //筹码3(配置文件读取,服务器发送)

        // 倍率信息
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public STAnimalAttArray[] arrSTAnimalArray;             // 哪种动物

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int32[] arrColorRate;                             //颜色分布概率

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public STEnjoyGameAtt[] arrSTEnjoyGameAtt;               //庄闲和属性

        //开奖信息
        public STAnimalPrize stWinAnimal;                        //开奖动物
        public STEnjoyGamePrizeInfo stWinEnjoyGameType;          //开奖庄闲和

        //public CMD_BANKER_INFO stBankerInfo;                     //庄家信息
    }

    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct STAnimalAttArray
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public STAnimalAtt[] arrSTAnimalJettonLimit;            // 动物的不同颜色下的属性(类型,倍率,最高下注)
    }

    /// <summary>
    /// 动物属性
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct STAnimalAtt
    {
        public STAnimalInfo stAnimal;       //动物类型
        public UInt32 dwMul;                //动物开奖倍率
    }

    /// <summary>
    /// 动物类型
    /// </summary> 
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct STAnimalInfo
    {
        public Int32 eAnimal;       //动物
        public Int32 eColor;        //颜色
    }

    /// <summary>
    /// 动物类型定义
    /// </summary>
    public enum eAnimalType
    {
        eAnimalType_Invalid = -1,
        eAnimalType_Lion = 0,
        eAnimalType_Panda,
        eAnimalType_Monkey,
        eAnimalType_Rabbit,

        eAnimalType_Max,    //最大值
    };

    /// <summary>
    /// 颜色类型定义
    /// </summary>
    public enum eColorType
    {
        eColorType_Invalid = -1,
        eColorType_Red = 0,
        eColorType_Green,
        eColorType_Yellow,

        eColorType_Max,     //最大值
    };

    /// <summary>
    /// 庄闲和属性
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct STEnjoyGameAtt
    {
        public UInt32 eEnjoyGame;              //庄闲和类型
        public UInt32 dwMul;                   //倍率
    }

    /// <summary>
    /// 庄闲和游戏定义
    /// </summary>
    public enum eEnjoyGameType
    {
        eEnjoyGameType_Invalid = -1,
        eEnjoyGameType_Zhuang = 0,
        eEnjoyGameType_Xian,
        eEnjoyGameType_He,

        eEnjoyGameType_Max,    //最大值
    }

    /// <summary>
    /// 庄家信息
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_BANKER_INFO
    {
        /*[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szBankerName;					//庄玩家
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szBankerAccounts;*/
        /*public ushort dwUserID;				//用户ID
        public UInt32 wBankCount;				//做庄次数
        public Int64 iBankerScore;			//庄金币*/
    }

    /// <summary>
    /// 用户续押
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_C_ContinueJetton
    {
        public Int64 iAllJettonScore;									//上把总下注
    };

    /// <summary>
    /// 用户下注_客户端
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_C_PlaceJetton
    {
        public Int32 eGamble;                        // 游戏类型
        public STAnimalInfo stAnimalInfo;             // 动物类型和颜色
        public Int32 eEnjoyGameInfo;                 // 庄闲和游戏定义
        public Int64 iPlaceJettonScore;              // 当前下注
    }

    /// <summary>
    /// 用户下注_服务器
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_PlaceJetton
    {
        public Int64 iPlaceJettonScore;			    // 当前下注
        public Int64 iTotalPlayerJetton;				// 庄家时候，显示其他玩家下注总和
        //public byte cbBanker;							// 是否是庄家，0： 非庄家，1：庄家
        public ushort wChairID;							// 用户位置
        public Int32 eGamble;                           // 游戏类型
        public Int32 eEnjoyGameInfo;                    // 庄和闲属性
        public STAnimalInfo stAnimalInfo;               // 动物类型和颜色
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
	    public Int64[] iUserAllJetton;                  // 玩家个人所有区域下注总数
    }


    /// <summary>
    /// 失败结构  加注失败 与 CMD_S_PlaceJetton一起用
    /// dwErrorCode:说明：
    ///	1：积分不够
    /// 2: 达到个体下注上限
    /// 3: 不在下注时间
    /// 4: 达到个人下注上限
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_PlaceJettonFail
	{
		public Int32 dwErrorCode;  //返回错误信息
        public Int32 eGamble;//类型
        public STAnimalInfo stAnimalInfo;//动物加注信息
        public Int32 eEnjoyGameInfo;//庄闲和加注信息
        public Int64 iPlaceJettonScore;				    //当前下注
        //BYTE							lJettonArea;						//下注区域
        //__int64							iPlaceScore;				    //当前下注
    }

    /// <summary>
    /// 游戏类型
    /// </summary>
    public enum eGambleType
    {
        eGambleType_Invalid = -1,
        eGambleType_AnimalGame = 0,	//3d动物
        eGambleType_EnjoyGame,		//小游戏，庄闲和
        eGambleType_Max,
    }

    /// <summary>
    /// 动物模式的开奖模式
    /// </summary>
    public enum eAnimalPrizeMode
    {
        eAnimalPrizeMode_Invalid = -1,
        eAnimalPrizeMode_SingleAnimalSingleColor = 0,   // 单动物单颜色
        eAnimalPrizeMode_AllAnimalSingleColr,           // 所有动物一个颜色
        eAnimalPrizeMode_AllColorSingleAnimal,          // 所有颜色一个动物
        eAnimalPrizeMode_SysPrize,                      // 
        eAnimalPrizeMode_RepeatTime,                    // 重复开奖
        eAnimalPrizeMode_Flash,                         // 闪电?

        eAnimalPrizeMode_Max,
    }

    /// <summary>
    /// 庄闲和开奖信息
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct STEnjoyGamePrizeInfo
    {
        public Int32 ePrizeGameType;           // 庄闲和游戏定义
    }

    /// <summary>
    /// 动物开奖信息
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct STAnimalPrize
    {
        public STAnimalInfo stAnimalInfo;
        public Int32 ePrizeMode;    // 动物模式的开奖模式

        /*
        当prizemode=eAnimalPrizeMode_SysPrize时，qwFlag表示开出来的系统彩金，
        当prizemode=eAnimalPrizeMode_RepeatTime时，qwFlag表示重复次数
        当prizemode=eAnimalPrizeMode_Flash时，qwFlag表示系统倍率
        */
        public UInt64 qwFlag;

        //在repeat下，另外再开的动物列表,最高2个
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public STAnimalInfo[] arrstRepeatModePrize;
    }
    /// <summary>
    /// 发送转盘信息
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_CreateTrun
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public int[] TrunColor;								//转盘颜色
    }

    /// <summary>
    /// 发送开奖颜色下标
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_PrizeColor
    {
        // 最多开奖三个(重复开奖最多开两个)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	    public int[] PrizeColorIndex;							//开奖颜色下标
    }

    //游戏记录
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct GamePrizeRecord
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] animalIndex;								    //开奖动物
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] colorIndex;									//开奖颜色
        public int enjoyGameIndex;								//开奖庄闲和
        public int gameType;									//开奖模式
    }
    /// <summary>
    /// 游戏记录
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_GameRecord
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public GamePrizeRecord[] arrGamePrizeRecord;
    }

    public class CGameXY : MonoBehaviour
    {
        public static CGameXY _instance;
        void Start()
        {
            _instance = this;
        }

        void OnDestroy()
        {
            _instance = null;
        }

        // 动物类型转换(接包得到Int转换成对应类型)
        public string eAnimalType_change(int temp_int)
        {
            string temp = "";
            switch (temp_int)
            {
                case -1: 
                    {
                        temp = "非动物";
                        break;
                    }
                case 0:
                    {
                        temp = "狮子";
                        break;
                    }
                case 1:
                    {
                        temp = "熊猫";
                        break;
                    }
                case 2:
                    {
                        temp = "猴子";
                        break;
                    }
                case 3:
                    {
                        temp = "兔子";
                        break;
                    }
            }
            return temp;
        }

        // 颜色类型转换(接包得到Int转换成对应类型)
        public string eColorType_change(int temp_int)
        {
            string temp = "";
            switch (temp_int)
            {
                case -1:
                    {
                        temp = "非颜色";
                        break;
                    }
                case 0:
                    {
                        temp = "红色";
                        break;
                    }
                case 1:
                    {
                        temp = "绿色";
                        break;
                    }
                case 2:
                    {
                        temp = "黄色";
                        break;
                    }
            }
            return temp;
        }

        public static void WhatTheFuck()
        {
            GameConvert.ByteToStruct<STEnjoyGameAtt>(null, 0);
            GameConvert.StructToByteArray<STEnjoyGameAtt>(new STEnjoyGameAtt());
            typeof(GameConvert).MakeGenericType(typeof(STEnjoyGameAtt));

            GameConvert.ByteToStruct<CMD_S_ContinueJettons_temp>(null, 0);
            GameConvert.StructToByteArray<CMD_S_ContinueJettons_temp>(new CMD_S_ContinueJettons_temp());

            GameConvert.ByteToStruct<STAnimalInfo>(null, 0);
            GameConvert.StructToByteArray<STAnimalInfo>(new STAnimalInfo());

            GameConvert.ByteToStruct<STAnimalPrize>(null, 0);
            GameConvert.StructToByteArray<STAnimalPrize>(new STAnimalPrize());

            GameConvert.ByteToStruct<STEnjoyGamePrizeInfo>(null, 0);
            GameConvert.StructToByteArray<STEnjoyGamePrizeInfo>(new STEnjoyGamePrizeInfo());

            GameConvert.ByteToStruct<STAnimalAttArray>(null, 0);
            GameConvert.StructToByteArray<STAnimalAttArray>(new STAnimalAttArray());

            GameConvert.ByteToStruct<STAnimalAtt>(null, 0);
            GameConvert.StructToByteArray<STAnimalAtt>(new STAnimalAtt());

            GameConvert.ByteToStruct<STEnjoyGameAtt>(null, 0);
            GameConvert.StructToByteArray<STEnjoyGameAtt>(new STEnjoyGameAtt());

            GameConvert.ByteToStruct<GamePrizeRecord>(null, 0);
            GameConvert.StructToByteArray<GamePrizeRecord>(new GamePrizeRecord());
        }

        // 庄和闲类型转换(接包得到Int转换成对应类型)
        public string eEnjoyGameType_change(int temp_int)
        {
            string temp = "";
            switch (temp_int)
            {
                case -1:
                    {
                        temp = "非庄闲和";
                        break;
                    }
                case 0:
                    {
                        temp = "庄";
                        break;
                    }
                case 1:
                    {
                        temp = "闲";
                        break;
                    }
                case 2:
                    {
                        temp = "和";
                        break;
                    }
            }
            return temp;
        }

        // 游戏类型转换(接包得到Int转换成对应类型)
        eGambleType eGambleType_change(int temp_int)
        {
            eGambleType temp = eGambleType.eGambleType_Invalid;
            switch (temp_int)
            {
                case -1:
                    {
                        temp = eGambleType.eGambleType_Invalid;
                        return temp;
                    }
                case 0:
                    {
                        temp = eGambleType.eGambleType_AnimalGame;
                        return temp;
                    }
                case 1:
                    {
                        temp = eGambleType.eGambleType_EnjoyGame;
                        return temp;
                    }
            }
            return temp;
        }



    }
}
