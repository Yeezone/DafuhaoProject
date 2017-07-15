using UnityEngine;
using System.Collections;
using Shared;
using com.QH.QPGame.Services.NetFox;
using System;

namespace com.QH.QPGame.ATT
{
   
        //主指令
        class MainCmd : MainCommand
        {
        };


        	//子指令
        class SubCmd : SubCommand
        {
            //客户端命令结构
            public const ushort SUB_C_START_DEAL_JX = 1001;                                    //请求发牌
            public const ushort SUB_C_UPDATE_CARD_JX = 1007;                                   //更新牌
            public const ushort SUB_C_COMPARE_XZ = 1011 ;                                      //比倍下注
            public const ushort SUB_C_COMPARE_GS = 1012;                                       //比倍结束
            public const ushort SUB_C_GET_RECORD = 1013;                                       //比倍结束
            public const ushort SUB_C_COMPARE_FREE = 1014;                                     //空闲比倍

            //服务器命令结构
            public const ushort SUB_S_START_JETTON_JX = 2001;								    //开始下注
            public const ushort SUB_S_START_DEAL_CARD_JX = 2002;                                //开始发牌
            public const ushort SUB_S_UPDATE_CARD_JX = 2007;                                    //更新牌
            public const ushort SUB_S_COMPARE_RETURN_JG = 2025;                                 //比倍结果返回
            public const ushort SUB_S_GAME_END = 2026;                                          //游戏结束
            public const ushort SUB_S_SEND_CHAIRID = 2027;                                      //把椅子ID发给其他玩家设置头像
            public const ushort SUB_S_SEND_CHAIRID_MY = 2028;                                   //把其他玩家椅子号发给自己
            public const ushort SUB_S_SEND_RECORD = 2029;                                       //发送历史记录
            public const ushort SUB_S_UPDATE_PIZI = 2030;                                       //更新皮子分
            public const ushort SUB_S_REWARD = 2031;                                            //中奖4K 以上
            public const ushort SUB_S_USER_NO_MONEY = 2032;                                     //金币不够
            public const ushort SUB_S_COMPARE_F_REURN = 2033;                                   //空闲比倍请求返回
            public const ushort SUB_S_SEND_PRIZE_DATA  = 2034;                                  //发送彩金信息
            public const ushort SUB_S_SEND_PRIZE_REWARD = 2035;                                 //发送彩金中奖
            public const ushort SUB_S_USER_BET_MIN = 2036;                                      //小于最小下注

        };

        /// <summary>
        /// 游戏开始场景初始信息
        /// </summary>
        public class CGameFree
        {
            public int m_ilExchangeScale;			         //兑换比例
            public int m_lExchangeGold;                      //兑换金币
            public bool m_IsCompare;                         //是否可以比倍
            public int m_nSmallChip;                         //最小下注
        }

        public class CMD_S_Update_Card
        {
           public tagUserCard cCard = new tagUserCard();   //用户扑克
           public byte bCardType;//牌型

           public Int64 chip;           //筹码
           public int nDie;           //赔率类型
           public int nState;         //状态
           public bool bIsGameOver;//是否结束游戏
           public bool bIsCompare;//是否比倍
        }

    public class CMD_S_COMPARE_RESUALT
    {
        public bool bIsWin; //是否赢
        public byte cbCardData;//牌数据
        public byte cbCompareID;
    }



    
}