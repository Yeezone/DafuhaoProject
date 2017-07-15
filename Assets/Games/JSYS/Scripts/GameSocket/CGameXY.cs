using UnityEngine;
using System.Collections;
using Shared;
using com.QH.QPGame.Services.NetFox;
using System;
using System.Runtime.InteropServices;
namespace com.QH.QPGame.JSYS
{
    //主指令
    class MainCmd : MainCommand
    {
    };

    //子指令
    class SubCmd : SubCommand
    {
        //客户端命令结构
        public const ushort SUB_C_PLAY_BET = 102;                   //用户下注
        public const ushort SUB_C_BET_CLEAR = 103;					//清除下注
        public const ushort SUB_C_CONTINUE_BET = 106;               //续压

        //服务器命令结构
        public const ushort SUB_S_PLAY_BET = 104;					//用户下注
        public const ushort SUB_S_GAME_END = 103;					//游戏结束
        public const ushort SUB_S_GAME_START = 102;				    //游戏开始
        public const ushort SUB_S_BET_CLEAR = 106;					//清除下注
        public const ushort SUB_S_CONTINUE_BET_DEFEAT = 109;        //续压失败
        public const ushort SUB_S_SEND_PRIZE_DATA = 110;           //发送彩金
        public const ushort SUB_S_SEND_PRIZE_REWARD = 111;          //发送彩金奖励
        public const ushort SUB_S_PLAY_BET_DEAFEAT = 112;			//用户下注失败
        public const ushort SUB_S_GAME_END_REVENUE = 113;           //更新抽水
        public const ushort SUB_S_GAME_END_IDI = 114;              //游戏结算
        //游戏状态
        public const ushort GAME_SCENE_FREE = 0;
        public const ushort GAME_SCENE_BET = 100;
        public const ushort GAME_SCENE_END = 101;

    };

    //空闲状态
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_StatusFree
    {
        public Int64 lPlayScore;							//玩家分数
        public Int64 lStorageStart;						    //库存（彩池）
        public Int64 lAreaLimitScore;					    //区域限制
        public Int64 lPlayLimitScore;					    //玩家限制
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int64[] lBetNum;                             //下注筹码
        public Int32 lCellScore;							//底分
        public byte cbTimeLeave;						    //剩余时间

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public int[] nTurnTableRecord;	    //游戏记录

    };

    //游戏下注状态
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_StatusPlay
    {
        public UInt64 lPlayScore;							//玩家分数
        public Int64 lPlayChip;							    //玩家筹码 
        public Int64 lStorageStart;						    //库存（彩池）		
        public Int64 lAreaLimitScore;					    //区域限制
        public Int64 lPlayLimitScore;					    //玩家限制
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public Int64[] lAllBet;				                //总下注
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public Int64[] lPlayBet;		                    //玩家下注 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int64[] lBetNum;                             //下注筹码
        public UInt32 lCellScore;							//底分
        public byte cbTimeLeave;						    //剩余时间

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public int[] nAnimalMultiple;		                //动物倍数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public int[] nTurnTableRecord;	                    //游戏记录

    };

    public class C_CMD_S_StatusPlay
    {
        public byte cbTimeLeave;						    //剩余时间

        public UInt32 lCellScore;							//底分
        public UInt64 lPlayScore;							//玩家分数

        public Int64 lPlayChip;							    //玩家筹码 
        public Int64 lStorageStart;						    //库存（彩池）		
        public Int64 lAreaLimitScore;					    //区域限制
        public Int64 lPlayLimitScore;					    //玩家限制
        public Int64[] lAllBet = new Int64[12];				//总下注
        public Int64[] lPlayBet = new Int64[12];		    //玩家下注

        public int[] nAnimalMultiple = new int[12];		   //动物倍数
        public int[] nTurnTableRecord = new int[40];	   //游戏记录
        public Int64[] lBetNum = new Int64[3];             //下注筹码
    };

    //下注消息
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_C_PlayBet
    {
        public Int64 lBetChip;							//筹码数量
        public int nAnimalIndex;						//下注动物

    };

    //用户下注
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_PlayBet
    {
        public long lBetChip;							//筹码数量
        public long mHBet;								//会员玩家下注
        public short wChairID;							//玩家位置
        public int nAnimalIndex;						//下注动物

    };

    public class C_CMD_S_PlayBet
    {
        public short wChairID;							//玩家位置
        public long mHBet;								//会员玩家下注
        public int nAnimalIndex;						//下注动物
        public long lBetChip;							//筹码数量
    };

    //游戏结束
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_GameEnd
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int64[] lPlayWin;		                     //玩家输赢
        public Int64 lPlayPrizes;						//玩家彩金
        public Int64 lPlayShowPrizes;					//显示彩金
        public byte cbTimeLeave;						//剩余时间
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bTurnTwoTime;					    //转2次
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public Int32[] nTurnTableTarget;				    //转盘目标
        public Int32 nPrizesMultiple;		 		    	//彩金	
    };

    public class C_CMD_S_GameEnd
    {
        public byte cbTimeLeave;						//剩余时间
        public byte[] bTurnTwoTime = new byte[8];		//转2次

        public Int32[] nTurnTableTarget = new Int32[9];	//转盘目标

        public Int32 nPrizesMultiple;		 		    	//彩金	

        public Int64[] lPlayWin = new Int64[9];		                     //玩家输赢

        public Int64 lPlayPrizes;						//玩家彩金

        public Int64 lPlayShowPrizes;					//显示彩金

    };

    //游戏开始
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_GameStart
    {
        public Int64 m_lAreaLimitScore;                                                 //区域限制
        public Int64 m_lPlayLimitScore;                                                 //个人限制
        public Int64 lStorageStart;						                                //库存（彩池）
        public byte cbTimeLeave;						                                //剩余时间

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public int[] nAnimalMultiple;		                                            //动物倍数
    };

    //清除下注
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_BetClear
    {
        public Int16 wChairID;							     //玩家位置
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public long[] lPlayBet;				                 //玩家清除数量
    };

    //游戏状态
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_StatusEnd
    {
        public long lPlayScore;							//玩家分数
        public long lPlayChip;							//玩家筹码 
        public long lStorageStart;						//库存（彩池）
        public long lAreaLimitScore;					//区域限制
        public long lPlayLimitScore;					//玩家限制 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public long[] lAllBet;			            	//总下注
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public long[] lPlayBet;				            //玩家下注 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int64[] lBetNum;                         //下注筹码

        public byte cbTimeLeave;						//剩余时间
        public Int32 lCellScore;							//底分


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public int[] nAnimalMultiple;		//动物倍数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public int[] nTurnTableRecord;	//游戏记录
    };

    //彩金中奖
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CMD_S_SendPrizePoolReward
    {
        public Int64 lRewardGold;                         //中奖金币
        public Int16 wTableID;                            //中奖玩家桌子ID
        public Int16 wChairID;                            //中奖玩家ID                     

    };
    public class CGameXY : MonoBehaviour
    {

    }
}
