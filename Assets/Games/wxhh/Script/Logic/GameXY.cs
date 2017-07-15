using System;
using Shared;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.WXHH
{

	//主指令
	class MainCmd : MainCommand
	{
	};
	
	//子指令
	class SubCmd : SubCommand
	{
		//服务端命令结构
		public const ushort SUB_S_GAME_FREE       	  = 	99;								    //游戏空闲
		public const ushort SUB_S_GAME_START       	  =     100;								//游戏开始							//用户下注
		public const ushort SUB_S_GAME_END        	  =     101;								//游戏结束
		public const ushort SUB_S_CANCEL_BET	   	  =     109;								//清除下注
		public const ushort SUB_S_USERPOINT	   	  	  =     110;								//用户下注
		public const ushort SUB_S_GAME_REWARD	   	  =     111;								//获取彩金				
		public const ushort SUB_S_PRIZE_POOL	   	  =     112;								//彩池刷新

        //客户端命令结构
		public const ushort SUB_C_SET_POINT      =       5;                                   //用户下注
		public const ushort SUB_C_LEAVE      	 =       6;                                   //用户离开                             //系统控制
		public const ushort SUB_C_CANCEL      	 =       7;                                   //清楚下注  
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

