using System;
using Shared;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.ZJH
{
	//主指令
	class MainCmd : MainCommand
	{
	};
	
	//子指令
	class SubCmd : SubCommand
	{
		//服务端命令结构
        public const ushort SUB_S_GAME_START        =       100;                                //游戏开始
        public const ushort SUB_S_ADD_SCORE         =       101;                                //加注结果
        public const ushort SUB_S_GIVE_UP           =       102;                                //放弃跟注
        public const ushort SUB_S_SEND_CARD         =       103;                                //发牌消息
        public const ushort SUB_S_GAME_END          =       104;                                //游戏结束
        public const ushort SUB_S_COMPARE_CARD      =       105;                                //比牌跟注
        public const ushort SUB_S_LOOK_CARD         =       106;                                //看牌跟注
        public const ushort SUB_S_PLAYER_EXIT       =       107;                                //用户强退
        public const ushort SUB_S_OPEN_CARD         =       108;                                //开牌消息
        public const ushort SUB_S_WAIT_COMPARE      =       109;                                //等待比牌
        public const ushort SUB_S_QIANG_BANKER      =       110;                                //抢庄
        public const ushort SUB_S_CHEAP_CARD        =       111;                                //诈牌
        public const ushort SUB_S_QIANG_START       =       112;                                //抢庄开始

        //客户端命令结构
        public const ushort SUB_C_ADD_SCORE         =       1;                                  //用户加注
        public const ushort SUB_C_GIVE_UP           =       2;                                  //放弃消息
        public const ushort SUB_C_COMPARE_CARD      =       3;                                  //比牌消息
        public const ushort SUB_C_LOOK_CARD         =       4;                                  //看牌消息
        public const ushort SUB_C_OPEN_CARD         =       5;                                  //开牌消息
        public const ushort SUB_C_WAIT_COMPARE      =       6;                                  //等待比牌
        public const ushort SUB_C_FINISH_FLASH      =       7;                                  //完成动画
        public const ushort SUB_C_QIANG_BANKER      =       8;                                  //抢庄
        public const ushort SUB_C_CHEAP_CARD        =       9;                                  //诈牌
		
	};
	
	//
	class ExtraCmd : SubCommand
	{
		public const ushort SUB_GP_SET_PASS 		=       11;
		public const ushort SUB_GP_EXP_BUY_SCORE 	=       22;
		public const ushort SUB_GP_EXP_BUY_AWARD 	=       23;
        public const ushort SUB_GP_NOTICE           =       30;
	};

}

