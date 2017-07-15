using System;
using Shared;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.CX
{

	//主指令
	class MainCmd : MainCommand
	{
	};
	
	//子指令
	class SubCmd : SubCommand
	{
		//服务端命令结构
        public const ushort SUB_S_GAME_START = 101;								//游戏开始
        public const ushort SUB_S_USER_INVEST = 107;						    //用户下本
        public const ushort SUB_S_ADD_SCORE = 102;								//加注结果
        public const ushort SUB_S_SEND_CARD = 103;								//发牌消息
        public const ushort SUB_S_GAME_END = 104;								//游戏结束
        public const ushort SUB_S_OPEN_START = 115;								//开始分牌
        public const ushort SUB_S_OPEN_CARD = 105;								//用户分牌
        public const ushort SUB_S_GIVE_UP = 106;								//用户放弃


        //客户端命令结构
        public const ushort SUB_C_USER_INVEST = 1;                          //用户下本
        public const ushort SUB_C_ADD_SCORE = 2;                            //用户加注
        public const ushort SUB_C_OPEN_CARD = 3;                            //用户摊牌
        public const ushort SUB_C_GIVE_UP = 4;                              //用户放弃
	};
	
	//
	class ExtraCmd : SubCommand
	{
		public const ushort SUB_GP_SET_PASS 		=       11;
		public const ushort SUB_GP_EXP_BUY_SCORE 	=       22;
		public const ushort SUB_GP_EXP_BUY_AWARD 	=       23;
        public const ushort SUB_GP_NOTICE           =       30;
	};


    //
    enum GameTimer
    {
        TIMER_SEND_CARD = 1000,

    };
}

