using UnityEngine;
using System.Collections;
using Shared;
using com.QH.QPGame.Services.NetFox;
using System;
using System.Runtime.InteropServices;

namespace com.QH.QPGame.BRPM
{
    //主指令
    class MainCmd : MainCommand
    {

    };

	//子指令
	class SubCmd : SubCommand
	{

		//////////////////////////////////////////////////////////////////////////		//服务器命令结构
		
		public const ushort SUB_S_BET_START			=	100;								//开始下注
		public const ushort SUB_S_BET_END			=	101;								//下注结束
		public const ushort SUB_S_HORSES_START		=	102;								//跑马开始
		public const ushort SUB_S_PLAYER_BET		=	103;								//用户下注
		public const ushort SUB_S_PLAYER_BET_FAIL	=	104;								//下注失败
		public const ushort SUB_S_CONTROL_SYSTEM	=	105;								//系统控制
		public const ushort SUB_S_NAMED_HORSES		=	106;								//马屁冠名
		public const ushort SUB_S_HORSES_END		=	107;								//跑马结束
		public const ushort SUB_S_MANDATOY_END		=	108;								//强制结束
		public const ushort SUB_S_ADMIN_COMMDN		=	109;								//系统控制
        public const ushort SUB_S_PLAYER_CLEANBET   =   110;                                //清除下注
		
		//客户端命令结构
        public const ushort SUB_C_PLAYER_BET = 1;                                            //用户下注
		public const ushort SUB_C_AMDIN_COMMAND	= 2;                                         //系统控制
        public const ushort SUB_C_ADMIN_COMMDN = 3;									         //系统控制
        public const ushort SUB_C_CLEANBET = 4;                                              //清理下注		
	};
	
    public class GameXY : MonoBehaviour
    {
        //游戏信息
        public const ushort KIND_ID = 1020;
        public const ushort GAME_PLAYER = 100;
        public const string GAME_NAME = "百人跑马";


        //当前游戏状态
        public const ushort GS_FREE = (ushort)GameState.GS_FREE;				//空闲状态
        public const ushort GS_BET = (ushort)GameState.GS_PLAYING;		        //下注状态
        public const ushort GS_BET_END = (ushort)GameState.GS_PLAYING + 1;		//下注结束状态
        public const ushort GS_HORSES = (ushort)GameState.GS_PLAYING + 2;       //结束状态


        //马匹索引
        public const byte HORSES_ONE = 0;                                       //1号马
        public const byte HORSES_TWO = 1;                                       //2号马
        public const byte HORSES_THREE = 2;                                     //3号马
        public const byte HORSES_FOUR = 3;                                      //4号马
        public const byte HORSES_FIVE = 4;                                      //5号马
        public const byte HORSES_SIX = 5;                                       //6号马
        public const byte HORSES_ALL = 6;                                       //合计索引


        //下注区域索引
        public const byte AREA_1_6 = 0;                                       //1_6 索引
        public const byte AREA_1_5 = 1;                                       //1_5 索引
        public const byte AREA_1_4 = 2;                                       //1_4 索引
        public const byte AREA_1_3 = 3;                                       //1_3 索引
        public const byte AREA_1_2 = 4;                                       //1_2 索引
        public const byte AREA_2_6 = 5;                                       //2_6 索引
        public const byte AREA_2_5 = 6;                                       //2_5 索引
        public const byte AREA_2_4 = 7;                                       //2_4 索引
        public const byte AREA_2_3 = 8;                                       //2_3 索引
        public const byte AREA_3_6 = 9;                                       //3_6 索引
        public const byte AREA_3_5 = 10;                                      //3_5 索引
        public const byte AREA_3_4 = 11;                                      //3_4 索引
        public const byte AREA_4_6 = 12;                                      //4_6 索引
        public const byte AREA_4_5 = 13;                                      //4_5 索引
        public const byte AREA_5_6 = 14;                                      //5_6 索引
        public const byte AREA_ALL = 15;                                      //合计索引


        //跑马名次
        public const byte RANKING_FIRST = 0;                                   //合计索引
        public const byte RANKING_SECOND = 1;                                  //合计索引
        public const byte RANKING_THIRD = 2;                                   //合计索引
        public const byte RANKING_FOURTH = 3;                                  //合计索引
        public const byte RANKING_FIFTH = 4;                                   //合计索引
        public const byte RANKING_SIXTH = 5;                                   //合计索引
        public const byte RANKING_NULL = 6;                                    //合计索引

        //金币个数
        public const int CHIP_COUNT = 5;

        //马匹名字
        public const string HORSES_NAME_LENGTH = "";

		//马匹奔跑的时间
		public const int HORSES_TIME = 10; 

        //预计完成时间
        public const byte COMPLETION_TIME = 25;

        //申请上庄人数上限
        public const ushort MAX_APPLY_BANKER = 5;

        //空用户					
        public const byte NULL_CHAIR = 255;
	
    }

  #region ###############################################################框架消息#################################################################################
    [System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct tagHistoryRecord
	{
		public int nStreak;						//场次	
		public int nRanking;					//排名
		public int nRiskCompensate;				//赔率
		public int nHours;						//小时
		public int nMinutes;					//分钟
		public int nSeconds;					//秒钟
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_SceneFreeChange
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string szHorsesName; 
	}
	
	/// <summary>
	///  游戏状态_空闲
	/// </summary>
	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_SceneFree
	{
        public Int64 lAreaLimitScore;								//区域总限制
        public Int64 lUserLimitScore;								//个人区域限制

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] nPlayerContinueScore;		                //续压

        public Int64 lUserLimitAllScore;							//个人下注总限制

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Int64[] nChipScore;     				                //筹码数值

		public int nTimeLeave;										//剩余时间
		public int nStreak;											//场次

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]		
		public int[] nMultiple;   									//区域倍数

// 		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
//         public CMD_S_SceneFreeChange[] sHorsesName;					//马匹名字

// 		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
// 		public tagHistoryRecord[]  GameRecords;						//游戏记录

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nStreakRecord;						            //场次	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nRankingRecord;					            //排名
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nRiskCompensateRecord;				             //赔率
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nHoursRecord;						             //小时
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nMinutesRecord;					             //分钟
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nSecondsRecord;					              //秒钟

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
		public int[] nWinCount;										//全天赢的场次

/*		public CMD_S_SceneFreeChange szGameRoomName;				//房间名字*/
	}

	/// <summary>
	///  游戏状态_下注
	/// </summary>
	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_SceneBet
	{
        public Int64 lAreaLimitScore;								//区域总限制
        public Int64 lUserLimitScore;								//个人区域限制
        public Int64 lUserLimitAllScore;							//个人下注总限制
        public Int64 lUserMaxScore;									//玩家最大下分数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] nPlayerContinueScore;		                //续压

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] lPlayerBet;									//玩家下注

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] lPlayerBetAll;								//所有下注

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Int64[] nChipScore;     				                //筹码数值

		public int nTimeLeave;										//剩余时间
		public int nStreak;											//场次
		
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]		
		public int[] nMultiple;   									//区域倍数
		
// 		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
// 		public CMD_S_SceneFreeChange[] sHorsesName;					//马匹名字
 		
//  		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//  		public tagHistoryRecord[]  GameRecords;						//游戏记录
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nStreakRecord;						            //场次	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nRankingRecord;					            //排名
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nRiskCompensateRecord;				             //赔率
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nHoursRecord;						             //小时
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nMinutesRecord;					             //分钟
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nSecondsRecord;					              //秒钟
		
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
		public int[] nWinCount;										//全天赢的场次

		public int nBetPlayerCount;									//下注人数

/*		public CMD_S_SceneFreeChange szGameRoomName;				//房间名字*/
	}

    /// <summary>
    ///  下注结束场景
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_SceneBetEnd
    {
        public Int64 lAreaLimitScore;								//区域总限制
        public Int64 lUserLimitScore;								//个人区域限制
        public Int64 lUserLimitAllScore;							//个人下注总限制

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] nPlayerContinueScore;		                //续压

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] lPlayerBet;									//玩家下注

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] lPlayerBetAll;								//所有下注

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Int64[] nChipScore;     				                //筹码数值

        public int nTimeLeave;										//剩余时间
        public int nStreak;											//场次

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
		public int[] nMultiple;   									//区域倍数

// 		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
// 		public CMD_S_SceneFreeChange[] sHorsesName;						//马匹名字

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public tagHistoryRecord[] GameRecords;						//游戏记录
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nStreakRecord;						            //场次	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nRankingRecord;					            //排名
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nRiskCompensateRecord;				             //赔率
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nHoursRecord;						             //小时
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nMinutesRecord;					             //分钟
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nSecondsRecord;					              //秒钟

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
		public int[] nWinCount;										//全天赢的场次

        public int nBetPlayerCount;									//下注人数

/*		public CMD_S_SceneFreeChange szGameRoomName;				//房间名字*/
    }

    /// <summary>
    /// 跑马开始
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_SceneHorses
    {
        public Int64 lAreaLimitScore;								//区域总限制
        public Int64 lUserLimitScore;								//个人区域限制
        public Int64 lUserLimitAllScore;							//个人下注总限制

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] nPlayerContinueScore;		                //续压

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] lPlayerBet;									//玩家下注

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] lPlayerBetAll;								//所有下注

        public Int64 lPlayerWinning;                                //玩家输赢

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Int64[] nChipScore;     				                //筹码数值

        public int nTimeLeave;										//剩余时间
        public int nStreak;											//场次

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
		public int[] nMultiple;   									//区域倍数

// 		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
// 		public CMD_S_SceneFreeChange[] sHorsesName;						//马匹名字

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public tagHistoryRecord[] GameRecords;						//游戏记录
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nStreakRecord;						            //场次	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nRankingRecord;					            //排名
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] nRiskCompensateRecord;				             //赔率
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nHoursRecord;						             //小时
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nMinutesRecord;					             //分钟
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         public int[] nSecondsRecord;					              //秒钟

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
		public int[] nWinCount;										//全天赢的场次

        public int nBetPlayerCount;									//下注人数

/*		public CMD_S_SceneFreeChange szGameRoomName;				//房间名字*/
    }
  #endregion


  #region ####################################################################游戏消息#################################################################
	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct CMD_S_HorsesSpeed
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_TIME)]
		public int[] HorsesSpeed; 
	}

	/// <summary>
	///  开始下注
	/// </summary>
	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct CMD_S_BetStart
	{
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
        public Int64[] nPlayerContinueScore;		                //续压

        public Int64 lUserMaxScore;								    //玩家最大下分数
		public int 		nTimeLeave;									//剩余时间		
		public int		nTimeBetEnd;								//下注结束时间
		public int		nChipRobotCount;							//人数上限（下注机器人）
	}

	/// <summary>
	///  下注结束
	/// </summary>
	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct CMD_S_BetEnd
	{
		public int 		nTimeLeave;									//剩余时间
	}

	/// <summary>
	///  跑马开始
	/// </summary>
	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct CMD_S_HorsesStart
	{
        //玩家成绩
        public Int64 lPlayerWinning;                                  //玩家输赢
        public Int64 lPlayerReturnBet;                                //玩家返回下注

		public int 		nTimeLeave;									//剩余时间

// 		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
// 		public CMD_S_HorsesSpeed[] HorsesSpeed;                     //马匹速度
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL * GameXY.HORSES_TIME)]
        public int[] HorsesSpeed;                                   //马匹速度

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
		public byte[] cbHorsesRanking;                               //名次

        public int nMultiple;                                        //赔率
	}


    /// <summary>
    ///  跑马强制结束
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_HorsesForceEnd
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public Int64[] lRankingNote;                                   //玩家成绩排行

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] lRankingIndex;                                  //玩家成绩排行对应的id

        public byte lPlayerRanking;                                   //当前玩家排名
    }
    /// <summary>
    ///  跑马结束
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_HorsesEnd
    {
        //玩家成绩
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
//         public Int64[] lPlayerWinning;                                     //玩家输赢
//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
//         public Int64[] lPlayerBet;                                         //玩家返回下注

        public int nTimeLeave;									        //剩余时间

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
// 		public int nWinCount;                                            //全天赢的次数
// 
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
		public int[] nMultiple;                                           //区域倍数
    }

    /// <summary>
    ///  玩家下注
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_PlayerBet
    {
        public Int64 lBetScore;                                           //玩家下注
        public byte areaId;                                               //下注区域
        public ushort wChairID;									        //下注玩家

//      [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.AREA_ALL)]
// 		public Int64[] lBetScore;                                         //玩家下注        

        public int nBetPlayerCount;                                    //下注人数

        public byte bIsAndroid;                                         //是否是机器人
    }

    /// <summary>
    ///  玩家下注失败
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_PlayerBetFail
    {
        public ushort wChairID;									        //位置

        public byte cbFailType;                                         //失败类型

// 		[MarshalAs(UnmanagedType.ByValArray, SizeConst = GameXY.HORSES_ALL)]
// 		public Int64[] lBetScore;                                         //下注人数
    }

    /// <summary>
    ///  玩家下注失败
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_PlayerCelanBetFail
    {
        public ushort wChairID;									        //位置
        public byte cbFailType;                                         //失败类型
    }

  #endregion

}