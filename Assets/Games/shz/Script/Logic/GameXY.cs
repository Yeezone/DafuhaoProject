using System;
using Shared;
using com.QH.QPGame.Services.NetFox;

namespace com.QH.QPGame.SHZ
{

	//主指令
	class MainCmd : MainCommand
	{
	};
	
	//子指令
	class SubCmd : SubCommand
	{
		//服务端命令结构
		public const ushort SUB_S_SCENE1_START        =     100;							// 滚动结果
		public const ushort SUB_S_SCENE2_RESULT    	  =     101;							// 骰子结果
		public const ushort SUB_S_SCENE3_RESULT       =     102;							// 玛丽结果
		public const ushort SUB_S_STOCK_RESULT        =     103;
		public const ushort SUB_S_ADMIN_LIST_WHITE    =     104;
		public const ushort SUB_S_ADMIN_LIST_BLACK    =     105;
		public const ushort SUB_S_PLACE_JETTON_FAIL	  =     107;							//下注失败
		public const ushort SUB_S_GAME_REWARD	   	  =     111;							//获取彩金				
		public const ushort SUB_S_PRIZE_POOL	   	  =     112;							//彩池刷新

        //客户端命令结构
		public const ushort SUB_C_ADD_CREDIT_SCORE   	  =       1;                        //上分
		public const ushort SUB_C_REDUCE_CREDIT_SCORE     =       2;                        //下分
		public const ushort SUB_C_SCENE1_START      	  =       3;                           
		public const ushort SUB_C_SCENE2_BUY_TYPE 	      =       4;                        //买大小
		public const ushort SUB_C_SCORE  	     		  =       5;                        //得分
		public const ushort SUB_C_SCENE3_START  	      =       6;                           
		public const ushort SUB_C_GLOBAL_MESSAGE  	      =       7;
		public const ushort SUB_C_STOCK_OPERATE  	      =       8;                           
		public const ushort SUB_C_ADMIN_CONTROL   	      =       9;
		public const ushort SUB_C_ADMIN_LIST_WHITE  	  =      10;                           
		public const ushort SUB_C_ADMIN_LIST_BLACK   	  =      11;
		public const ushort SUB_C_RECONNECT   	  		  =      18;	
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

