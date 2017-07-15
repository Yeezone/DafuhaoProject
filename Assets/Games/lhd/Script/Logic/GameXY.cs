using System;
using Shared;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.LHD
{

	//主指令
	class MainCmd : MainCommand
	{
	};
	
	//子指令
	class SubCmd : SubCommand
	{
		//服务端命令结构
//        public const ushort SUB_S_GAME_START        =       100;                                 //游戏开始
//        public const ushort SUB_S_ADD_SCORE         =       101;                                 //加注结果
//        public const ushort SUB_S_PLAYER_EXIT       =       102;                                 //用户强退
//        public const ushort SUB_S_SEND_CARD         =       103;                                 //发牌消息
//        public const ushort SUB_S_GAME_END          =       104;                                 //游戏结束
//        public const ushort SUB_S_OPEN_CARD         =       105;                                 //用户摊牌
//        public const ushort SUB_S_CALL_BANKER       =       106;                                 //用户叫庄

		public const ushort SUB_S_GAME_FREE       	  = 	99;								    //游戏空闲
		public const ushort SUB_S_GAME_START       	  =     100;								//游戏开始
		public const ushort SUB_S_PLACE_JETTON    	  =     101;								//用户下注
		public const ushort SUB_S_GAME_END        	  =     102;								//游戏结束
		public const ushort SUB_S_APPLY_BANKER   	  =     103;								//申请庄家
		public const ushort SUB_S_CHANGE_BANKER   	  =     104;								//切换庄家
		public const ushort SUB_S_CHANGE_USER_SCORE   =     105;								//更新积分
		public const ushort SUB_S_SEND_RECORD         =     106;								//游戏记录		
		public const ushort SUB_S_PLACE_JETTON_FAIL   =     107;								//下注失败
		public const ushort SUB_S_CANCEL_BANKER	   	  =     108;								//取消申请
		public const ushort SUB_S_AMDIN_COMMAND   	  =     109;								//系统控制
		
	
        //客户端命令结构
		public const ushort SUB_C_PLACE_JETTON      =       1;                                   //用户下注
		public const ushort SUB_C_APPLY_BANKER      =       2;                                   //申请庄家
		public const ushort SUB_C_CANCEL_BANKER     =       3;                                   //取消申请
		public const ushort SUB_C_AMDIN_COMMAND	    =       4;                                   //系统控制

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

